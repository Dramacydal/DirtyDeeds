using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itchy.AutoTeleport
{
    using PointList = List<Point>;

    public enum Range : short
    {
        TpRange = 35,
        Invalid = 10000,
    }

    public enum PathfindResult : byte
    {
        Fail = 0,
        Continue = 1,
        Reached = 2,
    }

    public class TeleportPath
    {
        protected ushort[][] m_ppTable = null;
        protected int m_nCX = 0;
        protected int m_nCY = 0;
        Point m_ptStart = new Point();
        Point m_ptEnd = new Point();

        public TeleportPath(ushort[][] pCollisionMap, int cx, int cy)
        {
            m_ppTable = pCollisionMap;
            m_nCX = cx;
            m_nCY = cy;
        }

        public static double CalculateDistance(Point pt1, Point pt2)
        {
            return CalculateDistance(pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(
                Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)
                );
        }

        protected bool MakeDistanceTable()
        {
            if (m_ppTable == null)
                return false;

            for (var x = 0; x < m_nCX; ++x)
            {
                for (int y = 0; y < m_nCY; y++)
                {
                    if ((m_ppTable[x][y] % 2) == 0)
                        m_ppTable[x][y] = (ushort)CalculateDistance(x, y, m_ptEnd.X, m_ptEnd.Y);
                    else
                        m_ppTable[x][y] = (ushort)Range.Invalid;
                }
            }

            m_ppTable[m_ptEnd.X][m_ptEnd.Y] = 1;
            return true;
        }

        protected bool IsValidIndex(int x, int y)
        {
            return x >= 0 && x < m_nCX && y >= 0 && y < m_nCY;
        }

        protected void Block(Point pos, int nRange)
        {
            nRange = Math.Max(nRange, 1);

            for (var i = pos.X - nRange; i < pos.X + nRange; ++i)
            {
                for (var j = pos.Y - nRange; j < pos.Y + nRange; ++j)
                {
                    if (IsValidIndex(i, j))
                        m_ppTable[i][j] = (ushort)Range.Invalid;
                }
            }
        }

        protected PathfindResult GetBestMove(ref Point pos, int nAdjust = 2)
        {
            if (CalculateDistance(m_ptEnd, pos) < (uint)Range.TpRange)
            {
                pos = m_ptEnd.Clone();
                return PathfindResult.Reached;
            }

            if (!IsValidIndex(pos.X, pos.Y))
                return PathfindResult.Fail;

            Block(pos, nAdjust);

            var p = new Point();
            var best = new Point();

            int value = (int)Range.Invalid;

            for (p.X = pos.X - (int)Range.TpRange; p.X <= pos.X + (int)Range.TpRange; ++p.X)
            {
                for (p.Y = pos.Y - (int)Range.TpRange; p.Y <= pos.Y + (int)Range.TpRange; ++p.Y)
                {
                    if (!IsValidIndex(p.X, p.Y))
                        continue;

                    if (m_ppTable[p.X][p.Y] < value && CalculateDistance(p, pos) <= (int)Range.TpRange)
                    {
                        value = m_ppTable[p.X][p.Y];
                        best = p.Clone();
                    }
                }
            }

            if (value >= (int)Range.Invalid)
                return PathfindResult.Fail; // no path at all

            pos = best;
            Block(pos, nAdjust);

            return PathfindResult.Continue;
        }

        protected int GetRedundancy(Point[] lpPath, uint dwMaxCount, Point pos)
        {
            // step redundancy check
            if (lpPath == null || dwMaxCount == 0)
                return -1;

            for (var i = 1; i < dwMaxCount; ++i)
            {
                if (CalculateDistance(lpPath[i].X, lpPath[i].Y, pos.X, pos.Y) <= (double)Range.TpRange / 2)
                    return i;
            }

            return -1;
        }

        public uint FindTeleportPath(Point ptStart, Point ptEnd, Point[] lpBuffer, uint dwMaxCount)
        {
            if (dwMaxCount == 0 || m_nCX <= 0 || m_nCY <= 0 || m_ppTable == null)
                return 0;

            for (var i = 0; i < dwMaxCount; ++i)
                lpBuffer[i] = new Point();

            m_ptStart = ptStart.Clone();
            m_ptEnd = ptEnd.Clone();

            MakeDistanceTable();

            lpBuffer[0] = ptStart;
            uint dwFound = 1;

            var pos = ptStart.Clone();

            var ok = false;
            var nRes = GetBestMove(ref pos);
            while (nRes != PathfindResult.Fail && dwFound < dwMaxCount)
            {
                if (nRes == PathfindResult.Reached)
                {
                    ok = true;
                    lpBuffer[dwFound] = ptEnd;
                    ++dwFound;
                    break;
                }

                // Perform a redundancy check
                var nRedundancy = GetRedundancy(lpBuffer, dwFound, pos);
                if (nRedundancy == -1)
                {
                    // no redundancy
                    lpBuffer[dwFound] = pos;
                    ++dwFound;
                }
                else
                {
                    // redundancy found, discard all redundant steps
                    dwFound = (uint)nRedundancy + 1;
                    lpBuffer[dwFound] = pos;
                }

                nRes = GetBestMove(ref pos);
            }

            if (!ok)
                dwFound = 0;

            return dwFound;
        }
    }
}
