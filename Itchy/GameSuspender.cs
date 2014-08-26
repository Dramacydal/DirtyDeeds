using System;

namespace Itchy
{
    public class GameSuspender : IDisposable
    {
        protected D2Game game;

        public GameSuspender(D2Game game)
        {
            this.game = game;
            game.Debugger.SuspendAllThreads();
        }

        public void Dispose()
        {
            game.Debugger.ResumeAllThreads();
        }
    }
}
