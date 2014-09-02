using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    using WordMatrix = CMatrix<ushort>;
    using LevelExitList = List<LevelExit>;

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
        protected D2Game game;
        protected WordMatrix m_map;
        protected Point m_ptLevelOrigin = new Point();
        protected uint dwLevelId = 0;
        //protected List<ushort> m_aCollisionTypes = new List<ushort>();

        public CollisionMap(D2Game game)
        {
            this.game = game;
            m_map = new WordMatrix();
        }

        public bool CreateMap(byte[] areas)
        {
            return BuildMapData(areas);
        }

        protected bool BuildMapData(byte[] areas)
        {
            if (m_map.IsCreated())
                return true;

            var pUnit = game.GetPlayerUnit();
            if (pUnit == 0)
                return false;

            var unit = game.Debugger.Read<UnitAny>(pUnit);

            var pBestLevel = game.MapHandler.GetLevel(areas[0]);
            var lvl = game.Debugger.Read<Level>(pBestLevel);
            uint dwXSize = 0;
            uint dwYSize = 0;
            m_ptLevelOrigin.X = (int)(lvl.dwPosX * 5);
            m_ptLevelOrigin.Y = (int)(lvl.dwPosY * 5);
            dwLevelId = areas[0];

            var lvls = new List<Level>();
            foreach (var area in areas)
            {
                var pLevel = game.MapHandler.GetLevel(area);
                if (pLevel == 0)
                    continue;

                lvl = game.Debugger.Read<Level>(pLevel);
                lvls.Add(lvl);

                if (m_ptLevelOrigin.X / 5 > (int)lvl.dwPosX)
                    m_ptLevelOrigin.X = (int)(lvl.dwPosX * 5);
                if (m_ptLevelOrigin.Y / 5 > (int)lvl.dwPosY)
                    m_ptLevelOrigin.Y = (int)(lvl.dwPosY * 5);

                dwXSize += lvl.dwSizeX * 5;
                dwYSize += lvl.dwSizeY * 5;
            }

            if (!m_map.Create((int)dwXSize, (int)dwYSize, (ushort)MapData.Invalid))
                return false;

            foreach (var lvll in lvls)
                Search(lvll.pRoom2First, unit, new List<uint>(), lvll.dwLevelNo);

            FillGaps();
            FillGaps();

            return true;
        }

        protected void Search(uint ro, UnitAny player, List<uint> aSkip, uint scanArea)
        {
            if (ro == 0)
                return;

            if (aSkip.Contains(ro))
                return;

            var room = game.Debugger.Read<Room2>(ro);
            if (room.pLevel == 0)
                return;

            var lvl = game.Debugger.Read<Level>(room.pLevel);
            if (lvl.dwLevelNo != scanArea)
                return;

            var add_room = false;
            var path = game.Debugger.Read<Path>(player.pPath);
            if (room.pRoom1 == 0)
            {
                add_room
                    = true;
                game.Debugger.Call(D2Common.AddRoomData,
                    CallingConventionEx.ThisCall,
                    0, player.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
            }

            aSkip.Add(ro);

            room = game.Debugger.Read<Room2>(ro);

            if (room.pRoom1 != 0)
            {
                var room1 = game.Debugger.Read<Room1>(room.pRoom1);
                AddCollisionData(room1.Coll);
            }

            var roomsNear = game.Debugger.ReadArray<uint>(room.pRoom2Near, (int)room.dwRoomsNear);
            foreach (var room2 in roomsNear)
                Search(room2, player, aSkip, scanArea);

            if (add_room)
                game.Debugger.Call(D2Common.RemoveRoomData,
                CallingConventionEx.StdCall,
                player.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
        }

        public void AddCollisionData(uint pColl)
        {
            if (pColl == 0)
                return;

            var coll = game.Debugger.Read<CollMap>(pColl);

            var x = (int)coll.dwPosGameX - m_ptLevelOrigin.X;
            var y = (int)coll.dwPosGameY - m_ptLevelOrigin.Y;
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

        protected void FillGaps()
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
        uint GetTileLevelNo(uint pRoom2, uint dwTileNo)
        {
            var room2 = game.Debugger.Read<Room2>(pRoom2);

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

        public int GetLevelExits(LevelExitList lpLevel)
        {
            UnitAny Me;
            if (!game.GetPlayerUnit(out Me))
                return 0;

            var ptExitPoints = new Point[0x40, 2];
            int nTotalPoints = 0;
            int nCurrentExit = 0;
            int nMaxExits = 0x40;

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

            var ptCenters = new Point[nTotalPoints];
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

                ptCenters[i].X = nXCenter != 0 ? nXCenter : ptExitPoints[i, 0].X;
                ptCenters[i].Y = nYCenter != 0 ? nYCenter : ptExitPoints[i, 0].Y;
            }

            var pLevel = game.MapHandler.GetLevel(dwLevelId);
            var level = game.Debugger.Read<Level>(pLevel);

            for (var pRoom = level.pRoom2First; pRoom != 0; )
            {
                var room = game.Debugger.Read<Room2>(pRoom);
                var roomsNear = game.Debugger.ReadArray<uint>(room.pRoom2Near, (int)room.dwRoomsNear);
                foreach (var pr in roomsNear)
                {
                    var r = game.Debugger.Read<Room2>(pr);
                    var lvl = game.Debugger.Read<Level>(r.pLevel);
                    if (lvl.dwLevelNo != dwLevelId)
                    {
                        int nRoomX = (int)room.dwPosX * 5;
                        int nRoomY = (int)room.dwPosY * 5;

                        for (int j = 0; j < nTotalPoints; ++j)
                        {
                            if ((ptCenters[j].X + m_ptLevelOrigin.X) >= (short)nRoomX && (ptCenters[j].X + m_ptLevelOrigin.X) <= (short)(nRoomX + (room.dwSizeX * 5)))
                            {
                                if ((ptCenters[j].Y + m_ptLevelOrigin.Y) >= (short)nRoomY && (ptCenters[j].Y + m_ptLevelOrigin.Y) <= (short)(nRoomY + (room.dwSizeY * 5)))
                                {
                                    if (nCurrentExit >= nMaxExits)
                                        return 0;

                                    var exit = new LevelExit
                                    {
                                        dwTargetLevel = lvl.dwLevelNo,
                                        ptPos = new Point(ptCenters[j].X + m_ptLevelOrigin.X, ptCenters[j].Y + m_ptLevelOrigin.Y),
                                        dwType = (uint)ExitType.Level,
                                        dwId = 0,
                                        pRoom2 = 0,
                                    };

                                    lpLevel.Add(exit);
                                    ++nCurrentExit;
                                }
                            }
                        }

                        break;
                    }
                }

                var bAdded = false;
                var path = game.Debugger.Read<Path>(Me.pPath);
                if (room.pRoom1 == 0)
                {
                    game.Debugger.Call(D2Common.AddRoomData,
                        CallingConventionEx.ThisCall,
                        0, Me.pAct, level.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
                    bAdded = true;
                }

                for (var pUnit = room.pPreset; pUnit != 0; )
                {
                    var preset = game.Debugger.Read<PresetUnit>(pUnit);

                    if (nCurrentExit >= nMaxExits)
                    {
                        if (bAdded)
                        {
                            game.Debugger.Call(D2Common.RemoveRoomData,
                                CallingConventionEx.StdCall,
                                Me.pAct, level.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
                            return 0;
                        }
                    }

                    if (preset.dwType == (uint)UnitType.Tile)
                    {
                        var dwTargetLevel = GetTileLevelNo(pRoom, preset.dwTxtFileNo);

                        if (dwTargetLevel != 0)
                        {
                            var bExists = false;

                            for (var i = 0; i < nCurrentExit; ++i)
                            {
                                if (((uint)lpLevel[i].ptPos.X == (room.dwPosX * 5) + preset.dwPosX) &&
                                    ((uint)lpLevel[i].ptPos.Y == (room.dwPosY * 5) + preset.dwPosY))
                                    bExists = true;
                            }

                            if (!bExists)
                            {
                                var exit = new LevelExit
                                {
                                    dwTargetLevel = dwTargetLevel,
                                    ptPos = new Point((int)((room.dwPosX * 5) + preset.dwPosX), (int)((room.dwPosY * 5) + preset.dwPosY)),
                                    dwType = (uint)ExitType.Tile,
                                    dwId = preset.dwTxtFileNo,
                                    pRoom2 = pRoom
                                };
                                lpLevel.Add(exit);
                                ++nCurrentExit;
                            }
                        }
                    }

                    pUnit = preset.pPresetNext;
                }

                if (bAdded)
                    game.Debugger.Call(D2Common.RemoveRoomData,
                        CallingConventionEx.StdCall,
                        Me.pAct, level.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);

                pRoom = room.pRoom2Next;
            }

            return nCurrentExit;
        }

        public bool IsValidAbsLocation(int x, int y)
        {
            if (!m_map.IsCreated())
                return false;

            x -= m_ptLevelOrigin.X;
            y -= m_ptLevelOrigin.Y;

            return m_map.IsValidIndex(x, y);
        }

        public void AbsToRelative(ref Point pt)
        {
            pt.X -= m_ptLevelOrigin.X;
            pt.Y -= m_ptLevelOrigin.Y;
        }

        public void RelativeToAbs(ref Point pt)
        {
            pt.X += m_ptLevelOrigin.X;
            pt.Y += m_ptLevelOrigin.Y;
        }

        public bool CopyMapData(WordMatrix rBuffer)
        {
            m_map.ExportData(rBuffer);
            return rBuffer.IsCreated();
        }
    }
}
