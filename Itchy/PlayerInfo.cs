using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itchy.D2Enums;

namespace Itchy
{
    using StatDictionary = ConcurrentDictionary<StatType, uint>;

    public class PlayerInfo
    {
        protected D2Game game = null;
        protected StatDictionary stats = new StatDictionary();

        protected static StatType[] CollectableStats = new StatType[]
        {
            StatType.Level,
            StatType.Exp,

            StatType.Strength,
            StatType.Dexterity,
            StatType.Vitality,
            StatType.Energy,

            StatType.FireResist,
            StatType.MaxFireResist,
            StatType.ColdResist,
            StatType.MaxColdResist,
            StatType.LightResist,
            StatType.MaxLightningResist,
            StatType.PoisonResist,
            StatType.MaxPoisonResist,
            StatType.MagicResist,
            StatType.MaxMagicResist,
            StatType.DamageResist,

            StatType.AbsorbFire,
            StatType.AbsorbFirePercent,
            StatType.AbsorbLight,
            StatType.AbsorbLightingPercent,
            StatType.AbsorbMagic,
            StatType.AbsorbMagicPercent,
            StatType.AbsorbCold,
            StatType.AbsorbColdPercent,

            StatType.GoldFind,
            StatType.MagicFind,

            StatType.FasterMoveVelocity,
            StatType.FasterAttackRate,
            StatType.FasterHitRecovery,
            StatType.FasterBlockRate,
            StatType.FasterCastRate,

            StatType.CrushingBlow,
            StatType.DeadlyStrike,
        };

        public PlayerInfo(D2Game game)
        {
            this.game = game;
        }

        public void SetStat(StatType stat, uint value)
        {
            if (!CollectableStats.Contains(stat))
                return;

            stats[stat] = value;
        }

        public uint GetStat(StatType stat)
        {
            if (!stats.ContainsKey(stat))
                return 0;

            return stats[stat];
        }

        public bool Update()
        {
            var player = game.GetPlayerUnit();
            if (player == 0)
                return false;

            if (!game.GameReady())
                return false;

            Name = game.GetPlayerName();

            foreach (var stat in CollectableStats)
                stats[stat] = game.GetUnitStat(player, stat);

            return true;
        }

        public void Reset()
        {
            Name = "";
            foreach (var stat in stats)
                stats[stat.Key] = 0;
        }

        public string Name = "";
    }
}
