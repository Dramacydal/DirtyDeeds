using System;
using System.Threading;

namespace DD
{
    public class MyTimer
    {
        public int Interval { get; set; }
        public bool IsStopping { get; private set; }
        public bool IsRunning { get { return loopThread != null; } }

        private Thread loopThread = null;

        public MyTimer()
        {
            Interval = 1000;
        }

        public MyTimer(int Interval)
        {
            this.Interval = Interval;
        }

        public void Start()
        {
            if (loopThread != null)
                return;

            loopThread = new Thread(LoopThread);
            loopThread.Start();
        }

        public void Stop(bool Block = false)
        {
            if (loopThread != null)
            {
                if (Block)
                {
                    IsStopping = true;
                    loopThread.Join();
                }
                else
                    loopThread.Abort();

                IsStopping = false;
                loopThread = null;
            }
        }

        public event EventHandler Tick;

        private void LoopThread()
        {
            for (; !IsStopping; )
            {
                if (Tick != null)
                    Tick(this, EventArgs.Empty);
                Thread.Sleep(Interval);
            }
        }
    }
}
