using System.Windows.Forms;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ExileCore.Shared.Attributes;

namespace BuffUtil
{
    public class BuffUtilSettings : ISettings
    {
        public BuffUtilSettings()
        {
            FlameGolemConnectedSkill = new RangeNode<int>(1, 1, 13);

            RequireMinMonsterCount = new ToggleNode(false);
            NearbyMonsterCount = new RangeNode<int>(1, 1, 30);
            NearbyMonsterMaxDistance = new RangeNode<int>(500, 1, 2000);
            DisableInHideout = new ToggleNode(true);
            Debug = new ToggleNode(false);
            SilenceErrors = new ToggleNode(false);
        }

        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        #region Blood Rage (1)

        [Menu("Blood Rage", 1)]
        public ToggleNode BloodRage { get; set; } = new ToggleNode(false);

        [Menu("Blood Rage Key", "Which key to press to activate Blood Rage?", 11, 1)]
        public HotkeyNode BloodRageKey { get; set; } = new HotkeyNode(Keys.E);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 12, 1)]
        public RangeNode<int> BloodRageConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        [Menu("Max HP", "HP percent above which skill is not cast", 13, 1)]
        public RangeNode<int> BloodRageMaxHP { get; set; } = new RangeNode<int>(100, 0, 100);

        [Menu("Max Mana", "Mana percent above which skill is not cast", 14, 1)]
        public RangeNode<int> BloodRageMaxMP { get; set; } = new RangeNode<int>(100, 0, 100);

        #endregion

        #region Seismic Cry (2)

        [Menu("Seismic Cry", 2)]
        public ToggleNode SeismicCry { get; set; } = new ToggleNode(false);

        [Menu("Seismic Cry Key", 21, 2)]
        public HotkeyNode SeismicCryKey { get; set; } = new HotkeyNode(Keys.W);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 22, 2)]
        public RangeNode<int> SeismicCryConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        [Menu("Attack Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 23, 2)]
        public RangeNode<int> SeismicCryAttackConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        #endregion

        #region Intimidating Cry (3)

        [Menu("Intimidating Cry", 3)]
        public ToggleNode IntimidatingCry { get; set; } = new ToggleNode(false);

        [Menu("Intimidating Cry Key", 31, 3)]
        public HotkeyNode IntimidatingCryKey { get; set; } = new HotkeyNode(Keys.T);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 32, 3)]
        public RangeNode<int> IntimidatingCryConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        [Menu("Attack Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 33, 3)]
        public RangeNode<int> IntimidatingCryAttackConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        #endregion

        #region Molten Shell (4)

        [Menu("Molten Shell", 4)]
        public ToggleNode MoltenShell { get; set; } = new ToggleNode(false);

        [Menu("Molten Shell Key", 41, 4)]
        public HotkeyNode MoltenShellKey { get; set; } = new HotkeyNode(Keys.Q);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 42, 4)]
        public RangeNode<int> MoltenShellConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        [Menu("Max HP", "HP percent above which skill is not cast", 43, 4)]
        public RangeNode<int> MoltenShellMaxHP { get; set; } = new RangeNode<int>(50, 0, 100);

        #endregion

        #region Phase Run (5)

        [Menu("Phase Run", 5)]
        public ToggleNode PhaseRun { get; set; } = new ToggleNode(false);

        [Menu("Phase Run Key", 51, 5)]
        public HotkeyNode PhaseRunKey { get; set; } = new HotkeyNode(Keys.R);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 52, 5)]
        public RangeNode<int> PhaseRunConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        [Menu("Max HP", "HP percent above which skill is not cast", 53, 5)]
        public RangeNode<int> PhaseRunMaxHP { get; set; } = new RangeNode<int>(90, 0, 100);

        [Menu("Move time", "Time in ms spent moving after which skill can be cast", 54, 5)]
        public RangeNode<int> PhaseRunMinMoveTime { get; set; } = new RangeNode<int>(0, 0, 5000);

        #endregion

        #region Flame Golem (6)

        [Menu("Flame Golem", 6)]
        public ToggleNode FlameGolem { get; set; } = new ToggleNode(false);

        [Menu("Flame Golem Key", 61, 6)]
        public HotkeyNode FlameGolemKey { get; set; } = new HotkeyNode(Keys.Q);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 62, 6)]
        public RangeNode<int> FlameGolemConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        #endregion

        #region Bone Offering (7)

        [Menu("Bone Offering", 7)]
        public ToggleNode BoneOffering { get; set; } = new ToggleNode(false);

        [Menu("Bone Offering Key", 71, 7)]
        public HotkeyNode BoneOfferingKey { get; set; } = new HotkeyNode(Keys.Oemplus);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 72, 7)]
        public RangeNode<int> BoneOfferingConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        #endregion

        #region General's Cry (8)

        [Menu("General's Cry", 8)]
        public ToggleNode GeneralsCry { get; set; } = new ToggleNode(false);

        [Menu("General's Cry Key", 81, 8)]
        public HotkeyNode GeneralsCryKey { get; set; } = new HotkeyNode(Keys.OemMinus);

        [Menu("Connected Skill", "Set the skill slot (1 = top left, 8 = bottom right)", 82, 8)]
        public RangeNode<int> GeneralsCryConnectedSkill { get; set; } = new RangeNode<int>(1, 1, 13);

        #endregion

        #region Misc (10)

        [Menu("Misc", 10)] public EmptyNode MiscSettings { get; set; }

        [Menu("Nearby monsters", "Require a minimum count of nearby monsters to cast buffs?", 101, 10)]
        public ToggleNode RequireMinMonsterCount { get; set; }

        [Menu("Min Monster Count", "Minimum count of nearby monsters to cast", 102, 10)]
        public RangeNode<int> NearbyMonsterCount { get; set; }

        [Menu("Min Corpse Count", "Minimum count of nearby corpses to cast", 103, 10)]
        public RangeNode<int> NearbyCorpseCount { get; set; } = new RangeNode<int>(1, 1, 5);

        [Menu("Max Nearby Range", "Max distance of monsters to player to count as nearby", 104, 10)]
        public RangeNode<int> NearbyMonsterMaxDistance { get; set; }

        [Menu("Disable in hideout", "Disable the plugin in hideout?", 105, 10)]
        public ToggleNode DisableInHideout { get; set; }

        [Menu("Debug", "Print debug messages?", 106, 10)]
        public ToggleNode Debug { get; set; }

        [Menu("Silence errors", "Hide error messages?", 107, 10)]
        public ToggleNode SilenceErrors { get; set; }

        #endregion
    }
}