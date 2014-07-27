using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Itchy
{
    public partial class D2Game
    {
        public volatile bool chickening;

        public void Chickener(bool hostile)
        {
            if (chickening)
                return;

            if (!GameReady())
                return;

            if (IsInTown())
                return;

            if (hostile)
            {
                if (!Settings.Chicken.ChickenOnHostile)
                    return;

                if (Settings.Chicken.ChickenToTown)
                    LogWarning("Chickening to town because of hostility");
            }
            else
            {
                var unit = GetPlayerUnit();
                var life = GetUnitStat(unit, Stat.Health);
                var maxLife = GetUnitStat(unit, Stat.MaxHealth);
                life >>= 8;
                maxLife >>= 8;

                var hppct = life * 100f / maxLife;
                if (hppct > Settings.Chicken.ChickenLifePercent)
                    return;

                try
                {
                    if (Settings.Chicken.ChickenToTown)
                        LogWarning("Chickening to town at {0:0.00}% health", hppct);
                }
                catch (Exception e)
                {
                }
            }

            chickening = true;

            if (!Settings.Chicken.ChickenToTown)
            {
                ExitGame();
                return;
            }

            backToTown = true;
            if (!OpenPortal())
            {
                backToTown = false;
                ExitGame();
                return;
            }

            var now = DateTime.Now;
            while ((DateTime.Now.Ticks - now.Ticks) / TimeSpan.TicksPerMillisecond < 2000 && !IsInTown())
            {
                Thread.Sleep(300);
            }

            if (!IsInTown())
            {
                ExitGame();
                return;
            }

            chickening = false;
        }
    }
}
