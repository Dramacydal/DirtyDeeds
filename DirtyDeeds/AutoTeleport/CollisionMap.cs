using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.D2Enums;
using WhiteMagic;

namespace DD.AutoTeleport
{
    using WordMatrix = CMatrix<ushort>;
    using DD.D2Structs;

    public enum MapData
    {
        Invalid = 0xFFFF,
        Cleaned = 11110,
        Filled = 11111,
        Thickened = 11113,
        Avoid = 11115,
    }

    public enum ExitType
    {
        Level = 1,
        Tile = 2,
    }

    public class LevelExit
    {
        public Point ptPos = new Point();
        public uint dwTargetLevel = 0;
        public uint dwType = 0;
        public uint dwId = 0;
        public uint pRoom2 = 0;
    }

    public class CollisionMap
    {
        public Point LevelOrigin = new Point();

        protected D2Game game;
        public WordMatrix m_map;
        //protected List<ushort> m_aCollisionTypes = new List<ushort>();

        public CollisionMap(D2Game game)
        {
            this.game = game;
            m_map = new WordMatrix();
        }

        public void AddCollisionData(uint pColl)
        {
            if (pColl == 0)
                return;

            var coll = game.Debugger.Read<CollMap>(pColl);

            var x = (int)coll.dwPosGameX - LevelOrigin.X;
            var y = (int)coll.dwPosGameY - LevelOrigin.Y;
            var cx = (int)coll.dwSizeGameX;
            var cy = (int)coll.dwSizeGameY;

            if (!m_map.IsValidIndex(x, y))
                return;

            var nLimitX = x + cx;
            var nLimitY = y + cy;

            var p = coll.pMapStart;
            for (var j = y; j < nLimitY; ++j)
            {
                for (var i = x; i < nLimitX; ++i)
                {
                    var type = game.Debugger.ReadUShort(p);
                    m_map[i][j] = type;
                    if (type == 1024)
                        m_map[i][j] = (ushort)MapData.Invalid;

                    /*if (!m_aCollisionTypes.Contains(type))
                        m_aCollisionTypes.Add(type);*/

                    p += 2;
                }
            }
        }

        public void FillGaps()
        {
            if (!m_map.IsCreated())
                return;

            //m_map.Lock();

            var CX = m_map.CX;
            var CY = m_map.CY;

            for (var x = 0; x < CX; ++x)
            {
                for (var y = 0; y < CY; ++y)
                {
                    if (IsGap(x, y))
                        m_map[x][y] = (ushort)MapData.Filled;
                }
            }

            //m_map.Unlock();
        }

        bool IsGap(int x, int y)
        {
            if ((m_map[x][y] % 2) != 0)
                return false;

            int nSpaces = 0;
            int i = 0;

            // Horizontal check
            for (i = x - 2; i <= x + 2 && nSpaces < 3; ++i)
            {
                if (i < 0 || i >= m_map.CX || (m_map[i][y] % 2) != 0)
                    nSpaces = 0;
                else
                    ++nSpaces;
            }

            if (nSpaces < 3)
                return true;

            // Vertical check
            nSpaces = 0;
            for (i = y - 2; i <= y + 2 && nSpaces < 3; i++)
            {
                if (i < 0 || i >= m_map.CY || (m_map[x][i] % 2) != 0)
                    nSpaces = 0;
                else
                    ++nSpaces;
            }

            return nSpaces < 3;
        }

        public static uint GetTileLevelNo(D2Game game, Room2 room2, uint dwTileNo)
        {
            for (var pRoomTile = room2.pRoomTiles; pRoomTile != 0; )
            {
                var roomTile = game.Debugger.Read<RoomTile>(pRoomTile);
                if (game.Debugger.ReadUInt(roomTile.nNum) == dwTileNo)
                {
                    room2 = game.Debugger.Read<Room2>(roomTile.pRoom2);
                    var lvl = game.Debugger.Read<Level>(room2.pLevel);
                    return lvl.dwLevelNo;
                }

                pRoomTile = roomTile.pNext;
            }

            return 0;
        }

        public List<Point> GetPtCenters()
        {
            var ptExitPoints = new Point[0x40, 2];
            int nTotalPoints = 0;

            for (var i = 0; i < m_map.CX; ++i)
            {
                if ((m_map[i][0] % 2) == 0)
                {
                    ptExitPoints[nTotalPoints, 0].X = i;
                    ptExitPoints[nTotalPoints, 0].Y = 0;

                    for (i++; i < m_map.CX; ++i)
                    {
                        if ((m_map[i][0] % 2) != 0)
                        {
                            ptExitPoints[nTotalPoints, 1].X = i - 1;
                            ptExitPoints[nTotalPoints, 1].Y = 0;
                            break;
                        }
                    }

                    ++nTotalPoints;
                    break;
                }
            }

            for (int i = 0; i < m_map.CX; ++i)
            {
                if ((m_map[i][m_map.CY - 1] % 2) == 0)
                {
                    ptExitPoints[nTotalPoints, 0].X = i;
                    ptExitPoints[nTotalPoints, 0].Y = m_map.CY - 1;

                    for (i++; i < m_map.CX; ++i)
                    {
                        if ((m_map[i][m_map.CY - 1] % 2) != 0)
                        {
                            ptExitPoints[nTotalPoints, 1].X = i - 1;
                            ptExitPoints[nTotalPoints, 1].Y = m_map.CY - 1;
                            break;
                        }
                    }

                    ++nTotalPoints;
                    break;
                }
            }

            for (int i = 0; i < m_map.CY; ++i)
            {
                if ((m_map[0][i] % 2) == 0)
                {
                    ptExitPoints[nTotalPoints, 0].X = 0;
                    ptExitPoints[nTotalPoints, 0].Y = i;

                    for (i++; i < m_map.CY; i++)
                    {
                        if ((m_map[0][i] % 2) != 0)
                        {
                            ptExitPoints[nTotalPoints, 1].X = 0;
                            ptExitPoints[nTotalPoints, 1].Y = i - 1;
                            break;
                        }
                    }

                    ++nTotalPoints;
                    break;
                }
            }

            for (int i = 0; i < m_map.CY; ++i)
            {
                if ((m_map[m_map.CX - 1][i] % 2) == 0)
                {
                    ptExitPoints[nTotalPoints, 0].X = m_map.CX - 1;
                    ptExitPoints[nTotalPoints, 0].Y = i;

                    for (i++; i < m_map.CY; ++i)
                    {
                        if ((m_map[m_map.CX - 1][i] % 2) != 0)
                        {
                            ptExitPoints[nTotalPoints, 1].X = m_map.CX - 1;
                            ptExitPoints[nTotalPoints, 1].Y = i - 1;
                            break;
                        }
                    }

                    ++nTotalPoints;
                    break;
                }
            }

            var ptCenters = new List<Point>();
            for (var i = 0; i < nTotalPoints; ++i)
            {
                int nXDiff = ptExitPoints[i, 1].X - ptExitPoints[i, 0].X;
                int nYDiff = ptExitPoints[i, 1].Y - ptExitPoints[i, 0].Y;
                int nXCenter = 0, nYCenter = 0;

                if (nXDiff > 0)
                {
                    if ((nXDiff % 2) != 0)
                        nXCenter = ptExitPoints[i, 0].X + ((nXDiff - (nXDiff % 2)) / 2);
                    else
                        nXCenter = ptExitPoints[i, 0].X + (nXDiff / 2);
                }

                if (nYDiff > 0)
                {
                    if ((nYDiff % 2) != 0)
                        nYCenter = ptExitPoints[i, 0].Y + ((nYDiff - (nYDiff % 2)) / 2);
                    else
                        nYCenter = ptExitPoints[i, 0].Y + (nYDiff / 2);
                }

                ptCenters.Add(new Point(nXCenter != 0 ? nXCenter : ptExitPoints[i, 0].X, nYCenter != 0 ? nYCenter : ptExitPoints[i, 0].Y));
            }

            return ptCenters;
        }

        public bool IsValidAbsLocation(int x, int y)
        {
            if (!m_map.IsCreated())
                return false;

            x -= LevelOrigin.X;
            y -= LevelOrigin.Y;

            return m_map.IsValidIndex(x, y);
        }

        public void AbsToRelative(ref Point pt)
        {
            pt.X -= LevelOrigin.X;
            pt.Y -= LevelOrigin.Y;
        }

        public void RelativeToAbs(ref Point pt)
        {
            pt.X += LevelOrigin.X;
            pt.Y += LevelOrigin.Y;
        }

        public bool CopyMapData(WordMatrix rBuffer)
        {
            m_map.ExportData(rBuffer);
            return rBuffer.IsCreated();
        }

        public CollisionMap Merge(CollisionMap other)
        {
            var newMap = new CollisionMap(game);

            var newOrigin = LevelOrigin.Clone();
            if (other.LevelOrigin.X < newOrigin.X)
                newOrigin.X = other.LevelOrigin.X;
            if (other.LevelOrigin.Y < newOrigin.Y)
                newOrigin.Y = other.LevelOrigin.Y;

            var cX = 0;
            var cY = 0;

            if (newOrigin.X == LevelOrigin.X)
                cX = other.LevelOrigin.X - newOrigin.X + other.m_map.CX;
            else
                cX = LevelOrigin.X - newOrigin.X + m_map.CX;

            if (newOrigin.Y == LevelOrigin.Y)
                cY = other.LevelOrigin.Y - newOrigin.Y + other.m_map.CY;
            else
                cY = LevelOrigin.Y - newOrigin.Y + m_map.CY;

            if (!newMap.m_map.Create(cX, cY, (ushort)MapData.Invalid))
                return new CollisionMap(game);

            newMap.LevelOrigin = newOrigin;
            for (var i = 0; i < m_map.CX; ++i)
                for (var j = 0; j < m_map.CY; ++j)
                    newMap.m_map[LevelOrigin.X - newOrigin.X + i][LevelOrigin.Y - newOrigin.Y + j] = m_map[i][j];

            for (var i = 0; i < other.m_map.CX; ++i)
                for (var j = 0; j < other.m_map.CY; ++j)
                    newMap.m_map[other.LevelOrigin.X - newOrigin.X + i][other.LevelOrigin.Y - newOrigin.Y + j] = other.m_map[i][j];

            return newMap;
        }
    }
}
