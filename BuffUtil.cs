using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;

namespace BuffUtil
{
    public class BuffUtil : BaseSettingsPlugin<BuffUtilSettings>
    {
        private readonly HashSet<Entity> loadedMonsters = new HashSet<Entity>();
        private readonly object loadedMonstersLock = new object();

        private List<Buff> buffs;
        private List<ActorSkill> skills;
        private DateTime? currentTime;
        private InputSimulator inputSimulator;
        private Random rand;
        private DateTime? lastBloodRageCast;
        private DateTime? lastSeismicCryCast;
        private DateTime? lastIntimidatingCryCast;
        private DateTime? lastMoltenShellCast;
        private DateTime? lastPhaseRunCast;
        private DateTime? lastGolemCast;
        private DateTime? lastBoneOfferingCast;
        private DateTime? lastGeneralsCryCast;
        private DateTime? lastEnduringCryCast;
        private float HPPercent;
        private float MPPercent;
        private int? nearbyMonsterCount;
        private int? nearbyCorpseCount;
        private bool showErrors = true;
        private Stopwatch movementStopwatch { get; set; } = new Stopwatch();

        public override bool Initialise()
        {
            inputSimulator = new InputSimulator();
            rand = new Random();

            showErrors = !Settings.SilenceErrors;
            Settings.SilenceErrors.OnValueChanged += delegate { showErrors = !Settings.SilenceErrors; };
            return base.Initialise();
        }

        public override void OnPluginDestroyForHotReload()
        {
            if (loadedMonsters != null)
                lock (loadedMonstersLock)
                {
                    loadedMonsters.Clear();
                }

            base.OnPluginDestroyForHotReload();
        }

        public override void Render()
        {
            // Should move to Tick?
            if (OnPreExecute())
                OnExecute();
            OnPostExecute();
        }

        private void OnExecute()
        {
            try
            {
                HandleEnduringCry();
                HandleBoneOffering();
                HandleGeneralsCry();
                HandleBloodRage();
                HandleMoltenShell();
                HandlePhaseRun();
                HandleFlameGolem();
            }
            catch (Exception ex)
            {
                if (showErrors)
                {
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(OnExecute)}: {ex.StackTrace}", 3f);
                }
            }
        }

        private void HandleBloodRage()
        {
            try
            {
                if (!Settings.BloodRage)
                    return;

                if (lastBloodRageCast.HasValue && currentTime - lastBloodRageCast.Value <
                    C.BloodRage.TimeBetweenCasts)
                    return;

                if (HPPercent > Settings.BloodRageMaxHP.Value || MPPercent > Settings.BloodRageMaxMP)
                    return;

                var hasBuff = HasBuff(C.BloodRage.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                var skill = GetUsableSkill(C.BloodRage.Name, C.BloodRage.InternalName,
                    Settings.BloodRageConnectedSkill.Value);
                if (skill == null)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast Blood Rage - not found in usable skills.", 1);
                    return;
                }

                if (!NearbyMonsterCheck())
                    return;

                if (Settings.Debug)
                    LogMessage("Casting Blood Rage", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode) Settings.BloodRageKey.Value);
                lastBloodRageCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleBloodRage)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandleMoltenShell()
        {
            try
            {
                if (!Settings.MoltenShell)
                    return;

                if (lastMoltenShellCast.HasValue && currentTime - lastMoltenShellCast.Value <
                    C.MoltenShell.TimeBetweenCasts)
                    return;

                if (HPPercent > Settings.MoltenShellMaxHP.Value)
                    return;

                var hasBuff = HasBuff(C.MoltenShell.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                var skill = GetUsableSkill(C.MoltenShell.Name, C.MoltenShell.InternalName, Settings.MoltenShellConnectedSkill.Value);
                if (skill == null)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast Molten Shell - not found in usable skills.", 1);
                    return;
                }

                if (!NearbyMonsterCheck())
                    return;

                if (Settings.Debug)
                    LogMessage("Casting Molten Shell", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode) Settings.MoltenShellKey.Value);
                lastMoltenShellCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleMoltenShell)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandlePhaseRun()
        {
            try
            {
                if (!Settings.PhaseRun)
                    return;

                if (lastPhaseRunCast.HasValue && currentTime - lastPhaseRunCast.Value <
                    C.PhaseRun.TimeBetweenCasts)
                    return;

                if (HPPercent > Settings.PhaseRunMaxHP.Value)
                    return;

                if (movementStopwatch.ElapsedMilliseconds < Settings.PhaseRunMinMoveTime)
                    return;

                var hasBuff = HasBuff(C.PhaseRun.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                var skill = GetUsableSkill(C.PhaseRun.Name, C.PhaseRun.InternalName,
                    Settings.PhaseRunConnectedSkill.Value);
                if (skill == null)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast Phase Run - not found in usable skills.", 1);
                    return;
                }

                if (!NearbyMonsterCheck())
                    return;

                if (Settings.Debug)
                    LogMessage("Casting Phase Run", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode)Settings.PhaseRunKey.Value);
                lastPhaseRunCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandlePhaseRun)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandleFlameGolem()
        {
            try
            {
                if (!Settings.FlameGolem)
                    return;

                if (lastGolemCast.HasValue && currentTime - lastGolemCast.Value < C.FlameGolem.TimeBetweenCasts)
                    return;

                var hasBuff = HasBuff(C.FlameGolem.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                var skill = GetUsableSkill(C.FlameGolem.Name, C.FlameGolem.InternalName, Settings.FlameGolemConnectedSkill.Value);
                if (skill == null || !skill.CanBeUsed)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast Golem - not found in usable skills.", 1);
                    return;
                }

                if (Settings.Debug)
                    LogMessage("Casting Golem", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode)Settings.FlameGolemKey.Value);
                lastGolemCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleFlameGolem)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandleBoneOffering()
        {
            try
            {
                if (!Settings.BoneOffering)
                    return;

                if (lastBoneOfferingCast.HasValue && currentTime - lastBoneOfferingCast.Value < C.BoneOffering.TimeBetweenCasts)
                    return;

                var hasBuff = HasBuff(C.BoneOffering.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                if (!NearbyCorpseCheck())
                    return;

                var skill = GetUsableSkill(C.BoneOffering.Name, C.BoneOffering.InternalName, Settings.BoneOfferingConnectedSkill.Value);
                if (skill == null || !skill.CanBeUsed)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast bone offering - not found in usable skills.", 1);
                    return;
                }

                if (Settings.Debug)
                    LogMessage("Casting Bone Offering", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode)Settings.BoneOfferingKey.Value);
                lastBoneOfferingCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleFlameGolem)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandleGeneralsCry()
        {
            try
            {
                if (!Settings.GeneralsCry)
                    return;

                if (lastGeneralsCryCast.HasValue && currentTime - lastGeneralsCryCast.Value < C.GeneralsCry.TimeBetweenCasts)
                    return;

                var hasBuff = HasBuff(C.GeneralsCry.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                if (!NearbyMonsterCheck())
                    return;

                if (!NearbyCorpseCheck())
                    return;

                var skill = GetUsableSkill(C.GeneralsCry.Name, C.GeneralsCry.InternalName, Settings.GeneralsCryConnectedSkill.Value);
                if (skill == null || !skill.CanBeUsed)
                {
                    if (Settings.Debug)
                        LogMessage("Can not cast generals cry - not found in usable skills.", 1);
                    return;
                }

                if (Settings.Debug)
                    LogMessage("Casting General's Cry", 1);
                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode)Settings.GeneralsCryKey.Value);
                lastGeneralsCryCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleFlameGolem)}: {ex.StackTrace}", 3f);
            }
        }

        private void HandleEnduringCry()
        {
            try
            {
                if (!Settings.EnduringCry)
                    return;

                if (lastEnduringCryCast.HasValue && currentTime - lastEnduringCryCast.Value < C.EnduringCry.TimeBetweenCasts)
                    return;

                var hasBuff = HasBuff(C.EnduringCry.BuffName);
                if (!hasBuff.HasValue || hasBuff.Value)
                    return;

                if (!NearbyMonsterCheck())
                    return;

                var skill = GetUsableSkill(C.EnduringCry.Name, C.EnduringCry.InternalName, Settings.EnduringCryConnectedSkill.Value);
                if (skill == null || !skill.CanBeUsed)
                {
                    return;
                }

                if (Core.Current.IsForeground)
                    inputSimulator.Keyboard.KeyPress((VirtualKeyCode)Settings.EnduringCryKey.Value);
                lastEnduringCryCast = currentTime + TimeSpan.FromSeconds(rand.NextDouble(0, 0.2));
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(HandleFlameGolem)}: {ex.StackTrace}", 3f);
            }
        }

        private bool OnPreExecute()
        {
            try
            {
                if (!Settings.Enable)
                    return false;
                var inTown = GameController.Area.CurrentArea.IsTown;
                if (inTown)
                    return false;
                if (Settings.DisableInHideout && GameController.Area.CurrentArea.IsHideout)
                    return false;
                var player = GameController.Game.IngameState.Data.LocalPlayer;
                if (player == null)
                    return false;
                var playerLife = player.GetComponent<Life>();
                if (playerLife == null)
                    return false;
                var isDead = playerLife.CurHP <= 0;
                if (isDead)
                    return false;

                buffs = player.GetComponent<Buffs>()?.BuffsList;
                if (buffs == null)
                    return false;

                var gracePeriod = HasBuff(C.GracePeriod.BuffName);
                if (!gracePeriod.HasValue || gracePeriod.Value)
                    return false;

                skills = player.GetComponent<Actor>().ActorSkills;
                if (skills == null || skills.Count == 0)
                    return false;

                currentTime = DateTime.UtcNow;

                HPPercent = 100f * playerLife.HPPercentage;
                MPPercent = 100f * playerLife.MPPercentage;
                
                var playerActor = player.GetComponent<Actor>();
                if (player != null && player.Address != 0 && playerActor.isMoving)
                {
                    if (!movementStopwatch.IsRunning)
                        movementStopwatch.Start();
                }
                else
                {
                    movementStopwatch.Reset();
                }
                

                return true;
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(OnPreExecute)}: {ex.StackTrace}", 3f);
                return false;
            }
        }

        private void OnPostExecute()
        {
            try
            {
                buffs = null;
                skills = null;
                currentTime = null;
                nearbyMonsterCount = null;
                nearbyCorpseCount = null;
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(OnPostExecute)}: {ex.StackTrace}", 3f);
            }
        }

        private bool? HasBuff(string buffName)
        {
            if (buffs == null)
            {
                if (showErrors)
                    LogError("Requested buff check, but buff list is empty.", 1);
                return null;
            }

            return buffs.Any(b => string.Compare(b.Name, buffName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private Buff GetBuff(string buffName)
        {
            if (buffs == null)
            {
                if (showErrors)
                    LogError("Requested buff retrieval, but buff list is empty.", 1);
                return null;
            }

            return buffs.FirstOrDefault(b => string.Compare(b.Name, buffName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private ActorSkill GetUsableSkill(int skillSlotIndex)
        {
            if (skills == null)
            {
                if (showErrors)
                    LogError("Requested usable skill, but skill list is empty.", 1);
                return null;
            }

            return skills.FirstOrDefault(s => s.SkillSlotIndex == skillSlotIndex - 1);
        }

        private ActorSkill GetUsableSkill(string skillName, string skillInternalName, int skillSlotIndex)
        {
            if (skills == null)
            {
                if (showErrors)
                    LogError("Requested usable skill, but skill list is empty.", 1);
                return null;
            }

            return skills.FirstOrDefault(s =>
                (s.Name == skillName || s.InternalName == skillInternalName));
        }

        private int GetMonsterPower()
        {
            var playerPosition = GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Render>().Pos;

            List<Entity> localLoadedMonsters;
            lock (loadedMonstersLock)
            {
                localLoadedMonsters = new List<Entity>(loadedMonsters);
            }

            var maxDistance = Settings.NearbyMonsterMaxDistance.Value;
            var maxDistanceSquared = maxDistance * maxDistance;
            var monsterPower = 0;
            foreach (var monster in localLoadedMonsters)
                if (IsValidNearbyMonster(monster, playerPosition, maxDistanceSquared))
                {
                    switch(monster.Rarity)
                    {
                        case ExileCore.Shared.Enums.MonsterRarity.White:
                            monsterPower += 1;
                            break;
                        case ExileCore.Shared.Enums.MonsterRarity.Magic:
                            monsterPower += 2;
                            break;
                        case ExileCore.Shared.Enums.MonsterRarity.Rare:
                            monsterPower += 10;
                            break;
                        case ExileCore.Shared.Enums.MonsterRarity.Unique:
                            monsterPower += 20;
                            break;
                    }
                }
            return monsterPower;
        }

        private bool NearbyMonsterCheck()
        {
            if (!Settings.RequireMinMonsterCount)
                return true;

            if (nearbyMonsterCount.HasValue)
                return nearbyMonsterCount.Value >= Settings.NearbyMonsterCount.Value;

            var playerPosition = GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Render>().Pos;

            List<Entity> localLoadedMonsters;
            lock (loadedMonstersLock)
            {
                localLoadedMonsters = new List<Entity>(loadedMonsters);
            }

            var maxDistance = Settings.NearbyMonsterMaxDistance.Value;
            var maxDistanceSquared = maxDistance * maxDistance;
            var monsterCount = 0;
            foreach (var monster in localLoadedMonsters)
                if (IsValidNearbyMonster(monster, playerPosition, maxDistanceSquared))
                    monsterCount++;

            nearbyMonsterCount = monsterCount;
            var result = nearbyMonsterCount.Value >= Settings.NearbyMonsterCount;
            if (Settings.Debug.Value && !result)
                LogMessage("NearbyMonstersCheck failed.", 1);
            return result;
        }

        private bool IsValidNearbyMonster(Entity monster, Vector3 playerPosition, int maxDistanceSquared)
        {
            try
            {
                if (!monster.IsTargetable || !monster.IsAlive || !monster.IsHostile || monster.IsHidden || !monster.IsValid)
                    return false;

                var monsterPosition = monster.Pos;

                var xDiff = playerPosition.X - monsterPosition.X;
                var yDiff = playerPosition.Y - monsterPosition.Y;
                var monsterDistanceSquare = xDiff * xDiff + yDiff * yDiff;

                return monsterDistanceSquare <= maxDistanceSquared;
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(IsValidNearbyMonster)}: {ex.StackTrace}", 3f);
                return false;
            }
        }

        private bool NearbyCorpseCheck()
        {
            if (nearbyCorpseCount.HasValue)
                return nearbyCorpseCount.Value >= Settings.NearbyCorpseCount.Value;

            var playerPosition = GameController.Game.IngameState.Data.LocalPlayer.GetComponent<Render>().Pos;

            List<Entity> localLoadedMonsters;
            lock (loadedMonstersLock)
            {
                localLoadedMonsters = new List<Entity>(loadedMonsters);
            }

            var maxDistance = Settings.NearbyMonsterMaxDistance.Value;
            var maxDistanceSquared = maxDistance * maxDistance;
            var corpseCount = 0;
            foreach (var monster in localLoadedMonsters)
                if (IsValidNearbyCorpse(monster, playerPosition, maxDistanceSquared))
                    corpseCount++;

            nearbyCorpseCount = corpseCount;
            var result = nearbyCorpseCount.Value >= Settings.NearbyCorpseCount;
            if (Settings.Debug.Value && !result)
                LogMessage("NearbyCorpseCheck failed.", 1);
            return result;
        }

        private bool IsValidNearbyCorpse(Entity monster, Vector3 playerPosition, int maxDistanceSquared)
        {
            try
            {
                if (!monster.IsTargetable || monster.IsAlive || monster.IsHidden || !monster.IsValid)
                    return false;

                var monsterPosition = monster.Pos;

                var xDiff = playerPosition.X - monsterPosition.X;
                var yDiff = playerPosition.Y - monsterPosition.Y;
                var monsterDistanceSquare = xDiff * xDiff + yDiff * yDiff;

                return monsterDistanceSquare <= maxDistanceSquared;
            }
            catch (Exception ex)
            {
                if (showErrors)
                    LogError($"Exception in {nameof(BuffUtil)}.{nameof(IsValidNearbyCorpse)}: {ex.StackTrace}", 3f);
                return false;
            }
        }

        private bool IsMonster(Entity entity) => entity != null && entity.HasComponent<Monster>();

        public override void EntityAdded(Entity entity)
        {
            if (!IsMonster(entity))
                return;

            lock (loadedMonstersLock)
            {
                loadedMonsters.Add(entity);
            }
        }

        public override void EntityRemoved(Entity entity)
        {
            lock (loadedMonstersLock)
            {
                loadedMonsters.Remove(entity);
            }
        }
    }
}