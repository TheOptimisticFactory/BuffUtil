using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;

namespace BuffUtil
{
    public static class BuffExtension
    {
        public static int SkillIndex(this Buff buff)
        {
            return buff.M.Read<byte>(buff.Address + 0x3F);
        }
    }

    public static class ActorSkillExtension
    {
        public static int SlotIdentifier(this ActorSkill actorSkill)
        {
            return (actorSkill.Id >> 8) & 0xFF;
        }
    }
}
