using System;

namespace BuffUtil
{
    public static class C
    {
        public static class BloodRage
        {
            public const string BuffName = "blood_rage";
            public const string Name = "BloodRage";
            public const string InternalName = "blood_rage";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(1);
        }

        public static class SteelSkin
        {
            public const string BuffName = "steelskin";
            public const string Name = "QuicKGuard";
            public const string InternalName = "steelskin";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(1);
        }

        public static class ImmortalCall
        {
            public const string BuffName = "mortal_call";
            public const string Name = "ImmortalCall";
            public const string InternalName = "mortal_call";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(1);
        }

        public static class MoltenShell
        {
            public const string BuffName = "molten_shell_shield";
            public const string Name = "MoltenShell";
            public const string InternalName = "molten_shell_barrier";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(1);
        }

        public static class PhaseRun
        {
            public const string BuffName = "new_phase_run";
            public const string BuffName2 = "new_phase_run_damage";
            public const string Name = "NewPhaseRun";
            public const string InternalName = "new_phase_run";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(1);
        }

        public static class FlameGolem
        {
            public const string BuffName = "fire_elemental_buff";
            public const string Name = "SummonFireGolem";
            public const string InternalName = "summon_fire_elemental";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(6);
        }

        public static class BoneOffering
        {
            public const string BuffName = "offering_defensive";
            public const string Name = "BoneOffering";
            public const string InternalName = "bone_offering";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(7.5);
        }

        public static class GeneralsCry
        {
            public const string BuffName = "fortify";
            public const string Name = "GeneralsCry";
            public const string InternalName = "spiritual_cry";
            public static readonly TimeSpan TimeBetweenCasts = TimeSpan.FromSeconds(6);
        }

        public static class GracePeriod
        {
            public const string BuffName = "grace_period";
        }
    }
}