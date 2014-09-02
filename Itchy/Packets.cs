using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public partial class D2Game
    {
        public bool SetSkill(SkillType skillId, bool left = false)
        {
            if (GetSkill(left) == skillId)
                return true;

            if (!HasSkill(skillId))
                return false;

            var packet = new List<byte>();
            packet.Add((byte)GameClientPacket.SelectSkill);
            packet.AddRange(BitConverter.GetBytes((ushort)skillId));
            packet.Add(0);
            packet.Add(left ? (byte)0x80 : (byte)0);
            packet.AddRange(BitConverter.GetBytes((int)-1));

            SendPacket(packet.ToArray());
            return true;
        }

        public bool CastSkillXY(SkillType skillId, ushort x, ushort y)
        {
            if (!SetSkill(skillId))
                return false;

            var packet = new List<byte>();
            packet.Add((byte)GameClientPacket.CastRightSkill);
            packet.AddRange(BitConverter.GetBytes(x));
            packet.AddRange(BitConverter.GetBytes(y));

            SendPacket(packet.ToArray());
            return true;
        }

        public bool CastSkillOnTarget(SkillType skillId, UnitType unitType, uint uid)
        {
            if (!SetSkill(skillId))
                return false;

            var packet = new List<byte>();
            packet.Add((byte)GameClientPacket.CastRightSkillOnTarget);
            packet.AddRange(BitConverter.GetBytes((uint)unitType));
            packet.AddRange(BitConverter.GetBytes(uid));

            SendPacket(packet.ToArray());
            return true;
        }

        public void PickItem(uint uid)
        {
            var packet = new List<byte>();

            packet.Add((byte)GameClientPacket.PickItem);
            packet.Add(0x4);
            packet.AddRange(new byte[] { 0, 0, 0 });
            packet.AddRange(BitConverter.GetBytes(uid));
            packet.AddRange(new byte[] { 0, 0, 0, 0 });

            SendPacket(packet.ToArray());
        }
    }
}