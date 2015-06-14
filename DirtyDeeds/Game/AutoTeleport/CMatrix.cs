using System;
using System.Drawing;
using DD.Extensions;

namespace DD.Game.AutoTeleport
{
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message) { }
    }

    public class CMatrix<T>
    {
        public int CX { get { return m_cx; } }
        public int CY { get { return m_cy; } }

        protected int m_cx;
        protected int m_cy;
        protected T[][] m_ppData;

        public CMatrix()
        {
            m_cx = 0;
            m_cy = 0;
            m_ppData = null;
        }

        public void Destroy()
        {
            m_cx = 0;
            m_cy = 0;
            m_ppData = null;
        }

        public bool Create(int cx, int cy)
        {
            if (cx <= 0 || cy <= 0)
                return false;

            m_cx = cx;
            m_cy = cy;

            m_ppData = new T[m_cx][];
            for (var i = 0; i < m_cx; ++i)
                m_ppData[i] = new T[m_cy];

            return true;
        }

        public bool Create(int cx, int cy, T initValue)
        {
            if (!Create(cx, cy))
                return false;

            for (var i = 0; i < m_cx; ++i)
                for (var j = 0; j < m_cy; ++j)
                    m_ppData[i][j] = initValue;

            return true;
        }

        public bool IsValidIndex(int x, int y)
        {
            return m_ppData != null && x >= 0 && y >= 0 && x < m_cx && y < m_cy;
        }

        public T GetAt(int x, int y)
        {
            if (!IsValidIndex(x, y))
                throw new MatrixException("Invalid index specified");

            return m_ppData[x][y];
        }

        public bool SetAt(int x, int y, T data)
        {
            if (!IsValidIndex(x, y))
                return false;

            m_ppData[x][y] = data;
            return true;
        }

        public T[][] GetData()
        {
            return m_ppData;
        }

        public T[] this[int nIndex]
        {
            get
            {
                if (nIndex < 0 || nIndex >= m_cx)
                    return null;

                return m_ppData[nIndex];
            }
            set
            {
                if (nIndex < 0 || nIndex >= m_cx)
                    return;

                m_ppData[nIndex] = value;
            }
        }

        public bool IsCreated()
        {
            return m_ppData != null;
        }

        protected Size ExportData(ref T[][] ppBuffer, int cx, int cy)
        {

            if (ppBuffer == m_ppData)
                return new Size(m_cx, m_cy);

            if (ppBuffer == null || cx <= 0 || cy <= 0 || !IsCreated())
                return new Size();

            var cz = new Size(Math.Min(cx, m_cx), Math.Min(cy, m_cy));

            for (var i = 0; i < cz.X(); ++i)
                for (var j = 0; j < cz.Y(); ++i)
                    ppBuffer[i][j] = m_ppData[i][j];

            return cz;
        }

        public bool ExportData(CMatrix<T> rMatrix)
        {
            if (rMatrix == this)
                return IsCreated();

            if (!IsCreated())
                return false;

            if (rMatrix.CX != m_cx || rMatrix.CY != m_cy)
            {
                if (!rMatrix.Create(m_cx, m_cy))
                    return false;
            }

            for (var i = 0; i < m_cx; ++i)
            {
                for (var j = 0; j < m_cy; ++j)
                    rMatrix.m_ppData[i][j] = m_ppData[i][j];
            }

            return true;
        }
    }
}
