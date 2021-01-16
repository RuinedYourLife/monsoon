﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoR2;
using System.Text;


namespace UmbraMenu.Menus
{
    public sealed class Player : Menu
    {
        private static readonly IMenu player = new NormalMenu(1, new Rect(374, 10, 20, 20), "PLAYER MENU");

        public static bool SkillToggle, AimBotToggle, GodToggle;
        public static ulong xpToGive = 50;
        public static uint moneyToGive = 50, coinsToGive = 50;

        public ulong XPToGive
        {
            get
            {
                return xpToGive;
            }

            set
            {
                xpToGive = value;
                giveExperience.SetText($"G I V E   E X P E R I E N C E : {xpToGive}");
            }
        }
        public uint MoneyToGive
        {
            get
            {
                return moneyToGive;
            }

            set
            {
                moneyToGive = value;
                giveMoney.SetText($"G I V E   M O N E Y : {moneyToGive}");
            }
        }
        public uint CoinsToGive
        {
            get
            {
                return coinsToGive;
            }

            set
            {
                coinsToGive = value;
                giveCoins.SetText($"G I V E   L U N A R   C O I N S : {coinsToGive}");
            }
        }

        public Button giveMoney;
        public Button giveCoins;
        public Button giveExperience;
        public Button toggleStatsMod;
        public Button toggleChangeCharacter;
        public Button toggleBuff;
        public Button removeBuffs;
        public Button toggleAimbot;
        public Button toggleGod;
        public Button toggleSkillCD;
        public Button unlockAll;

        public Player() : base(player)
        {
            if (UmbraMenu.characterCollected)
            {
                void ToggleStatsMenu() => Utility.FindMenuById(8).ToggleMenu();
                void ToggleCharacterListMenu() => Utility.FindMenuById(10).ToggleMenu();
                void ToggleBuffListMenu() => Utility.FindMenuById(11).ToggleMenu();
                void DoNothing() => Utility.StubbedFunction();

                giveMoney = new Button(new MulButton(this, 1, $"GIVE MONEY : {moneyToGive}", GiveMoney, IncreaseMoney, DecreaseMoney));
                giveCoins = new Button(new MulButton(this, 2, $"G I V E   L U N A R   C O I N S : {coinsToGive}", GiveLunarCoins, IncreaseCoins, DecreaseCoins));
                giveExperience = new Button(new MulButton(this, 3, $"G I V E   E X P E R I E N C E : {xpToGive}", GiveXP, IncreaseXP, DecreaseXP));
                toggleStatsMod = new Button(new TogglableButton(this, 4, "S T A T S   M E N U : O F F", "S T A T S   M E N U : O N", ToggleStatsMenu, ToggleStatsMenu));
                toggleChangeCharacter = new Button(new TogglableButton(this, 5, "C H A N G E   C H A R A C T E R : O F F", "C H A N G E   C H A R A C T E R : O N", ToggleCharacterListMenu, ToggleCharacterListMenu));
                toggleBuff = new Button(new TogglableButton(this, 6, "G I V E   B U F F   M E N U : O F F", "G I V E   B U F F   M E N U : O N", ToggleBuffListMenu, ToggleBuffListMenu));
                removeBuffs = new Button(new NormalButton(this, 7, "R E M O V E   A L L   B U F F S", RemoveAllBuffs));
                toggleAimbot = new Button(new TogglableButton(this, 8, "A I M B O T : O F F", "A I M B O T : O N", ToggleAimbot, ToggleAimbot));
                toggleGod = new Button(new TogglableButton(this, 9, "G O D   M O D E : O F F", "G O D   M O D E : O N", ToggleGodMode, ToggleGodMode));
                toggleSkillCD = new Button(new TogglableButton(this, 10, "I N F I N I T E   S K I L L S : O F F", "I N F I N I T E   S K I L L S : O N", ToggleSkillCD, ToggleSkillCD));
                unlockAll = new Button(new TogglableButton(this, 11, "U N L O C K   A L L", "C O N F I R M ?", DoNothing, UnlockAll));

                AddButtons(new List<Button>()
                {
                    giveMoney,
                    giveCoins,
                    giveExperience,
                    toggleStatsMod,
                    toggleChangeCharacter,
                    toggleBuff,
                    removeBuffs,
                    toggleAimbot,
                    toggleGod,
                    toggleSkillCD,
                    unlockAll
                });
                SetActivatingButton(Utility.FindButtonById(0, 1));
                SetPrevMenuId(0);
            }
        }

        public override void Draw()
        {
            if (IsEnabled())
            {
                SetWindow();
                base.Draw();
            }
        }

        public override void Reset()
        {
            SkillToggle = false;
            AimBotToggle = false;
            GodToggle = false;
            xpToGive = 50;
            moneyToGive = 50;
            coinsToGive = 50;
            base.Reset();
        }

        public void ToggleMenu(Menu menu)
        {
            menu.ToggleMenu();
        }

        public void ToggleAimbot()
        {
            AimBotToggle = !AimBotToggle;
        }

        public void ToggleGodMode()
        {
            GodToggle = !GodToggle;
            if (!GodToggle)
            {
                DisabledGodMode();
            }
        }

        public static void ToggleSkillCD()
        {
            SkillToggle = !SkillToggle;
        }

        public static void RemoveAllBuffs()
        {
            foreach (string buffName in Enum.GetNames(typeof(BuffIndex)))
            {
                try
                {
                    BuffIndex buffIndex = (BuffIndex)Enum.Parse(typeof(BuffIndex), buffName);
                    while (UmbraMenu.LocalPlayerBody.HasBuff(buffIndex))
                    {
                        UmbraMenu.LocalPlayerBody.RemoveBuff(buffIndex);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    continue;
                }
            }
        }

        // self explanatory
        public static void GiveXP()
        {
            UmbraMenu.LocalPlayer.GiveExperience(xpToGive);
        }

        public static void GiveMoney()
        {
            UmbraMenu.LocalPlayer.GiveMoney(moneyToGive);
        }

        //uh, duh.
        public static void GiveLunarCoins()
        {
            UmbraMenu.LocalNetworkUser.AwardLunarCoins(coinsToGive);
        }

        public static void AimBot()
        {
            if (Utility.CursorIsVisible())
            {
                return;
            }

            var localUser = LocalUserManager.GetFirstLocalUser();
            var controller = localUser.cachedMasterController;
            if (!controller)
            {
                return;
            }

            var body = controller.master.GetBody();
            if (!body)
            {
                return;
            }

            var inputBank = body.GetComponent<InputBankTest>();
            var aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
            var bullseyeSearch = new BullseyeSearch();
            var team = body.GetComponent<TeamComponent>();
            bullseyeSearch.teamMaskFilter = TeamMask.all;
            bullseyeSearch.teamMaskFilter.RemoveTeam(team.teamIndex);
            bullseyeSearch.filterByLoS = true;
            bullseyeSearch.searchOrigin = aimRay.origin;
            bullseyeSearch.searchDirection = aimRay.direction;
            bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
            bullseyeSearch.maxDistanceFilter = float.MaxValue;
            bullseyeSearch.maxAngleFilter = 20f;// ;// float.MaxValue;
            bullseyeSearch.RefreshCandidates();
            var hurtBox = bullseyeSearch.GetResults().FirstOrDefault();
            if (hurtBox)
            {
                Vector3 direction = hurtBox.transform.position - aimRay.origin;
                inputBank.aimDirection = direction;
            }
        }

        public static void EnableGodMode()
        {
            switch (UmbraMenu.GodVersion)
            {
                case 0:
                    {
                        // works
                        // Normal
                        UmbraMenu.LocalHealth.godMode = true;
                        break;
                    }

                case 1:
                    {
                        // works
                        // Buff
                        UmbraMenu.LocalPlayerBody.AddBuff(BuffIndex.Intangible);
                        break;
                    }

                case 2:
                    {
                        // works
                        // Regen
                        UmbraMenu.LocalHealth.Heal(float.MaxValue, new ProcChainMask(), false);
                        break;
                    }

                case 3:
                    {
                        // works
                        // Negative
                        UmbraMenu.LocalHealth.SetField<bool>("wasAlive", false);
                        break;
                    }

                case 4:
                    {
                        // works
                        // Revive
                        UmbraMenu.LocalHealth.SetField<bool>("wasAlive", false);
                        int itemELCount = UmbraMenu.LocalPlayerInv.GetItemCount(ItemIndex.ExtraLife);
                        int itemELCCount = UmbraMenu.LocalPlayerInv.GetItemCount(ItemIndex.ExtraLifeConsumed);
                        if (UmbraMenu.LocalHealth.health < 1)
                        {
                            if (itemELCount == 0)
                            {
                                ItemList.GiveItem(ItemIndex.ExtraLife);
                                UmbraMenu.LocalHealth.SetField<bool>("wasAlive", true);
                            }
                        }
                        if (itemELCCount > 0)
                        {
                            UmbraMenu.LocalPlayerInv.RemoveItem(ItemIndex.ExtraLifeConsumed, itemELCCount);
                        }
                        if (itemELCount > 0)
                        {
                            UmbraMenu.LocalPlayerInv.RemoveItem(ItemIndex.ExtraLifeConsumed, itemELCount);
                        }
                        break;
                    }


                default:
                    break;
            }
        }

        public static void DisabledGodMode()
        {
            switch (UmbraMenu.GodVersion)
            {
                case 0:
                    {
                        UmbraMenu.LocalHealth.godMode = false;
                        break;
                    }

                case 1:
                    {
                        RemoveAllBuffs();
                        break;
                    }

                case 3:
                    {
                        if (UmbraMenu.LocalHealth.health < 0)
                        {
                            UmbraMenu.LocalHealth.health = 1;
                        }
                        UmbraMenu.LocalHealth.SetField<bool>("wasAlive", true);
                        break;
                    }

                case 4:
                    {
                        if (UmbraMenu.LocalHealth.health < 0)
                        {
                            UmbraMenu.LocalHealth.health = 1;
                        }
                        UmbraMenu.LocalHealth.SetField<bool>("wasAlive", true);
                        int itemELCount = UmbraMenu.LocalPlayerInv.GetItemCount(ItemIndex.ExtraLife);
                        int itemELCCount = UmbraMenu.LocalPlayerInv.GetItemCount(ItemIndex.ExtraLifeConsumed);
                        if (itemELCCount > 0)
                        {
                            UmbraMenu.LocalPlayerInv.RemoveItem(ItemIndex.ExtraLifeConsumed, itemELCCount);
                        }
                        if (itemELCount > 0)
                        {
                            UmbraMenu.LocalPlayerInv.RemoveItem(ItemIndex.ExtraLifeConsumed, itemELCount);
                        }
                        break;
                    }

                default:
                    break;
            }
        }

        public static void UnlockAll()
        {
            //Goes through resource file containing all unlockables... Easily updatable, just paste "RoR2.UnlockCatalog" and GetAllUnlockable does the rest.
            //This is needed to unlock logs
            foreach (var unlockableName in UmbraMenu.unlockableNames)
            {
                var unlockableDef = UnlockableCatalog.GetUnlockableDef(unlockableName);
                NetworkUser networkUser = Util.LookUpBodyNetworkUser(UmbraMenu.LocalPlayerBody);
                if (networkUser)
                {
                    networkUser.ServerHandleUnlock(unlockableDef);
                }
            }

            //Gives all achievements.
            var achievementManager = AchievementManager.GetUserAchievementManager(LocalUserManager.GetFirstLocalUser());
            foreach (var achievement in AchievementManager.allAchievementDefs)
            {
                achievementManager.GrantAchievement(achievement);
            }

            //Give all survivors
            var profile = LocalUserManager.GetFirstLocalUser().userProfile;
            foreach (var survivor in SurvivorCatalog.allSurvivorDefs)
            {
                if (profile.statSheet.GetStatValueDouble(RoR2.Stats.PerBodyStatDef.totalTimeAlive, survivor.bodyPrefab.name) == 0.0)
                    profile.statSheet.SetStatValueFromString(RoR2.Stats.PerBodyStatDef.totalTimeAlive.FindStatDef(survivor.bodyPrefab.name), "0.1");
            }

            //All items and equipments
            foreach (string itemName in Enum.GetNames(typeof(ItemIndex)))
            {
                ItemIndex itemIndex = (ItemIndex)Enum.Parse(typeof(ItemIndex), itemName);
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex(itemIndex));
            }

            foreach (string equipmentName in Enum.GetNames(typeof(EquipmentIndex)))
            {
                EquipmentIndex equipmentIndex = (EquipmentIndex)Enum.Parse(typeof(EquipmentIndex), equipmentName);
                profile.DiscoverPickup(PickupCatalog.FindPickupIndex(equipmentIndex));
            }

            //All Eclipse unlockables as well
            StringBuilder stringBuilder = StringBuilderPool.RentStringBuilder();
            foreach (SurvivorDef survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                for (int i = 2; i < 9; i++)
                {
                    stringBuilder.Clear().Append("Eclipse.").Append(survivorDef.name).Append(".").AppendInt(i, 0U, uint.MaxValue);
                    UnlockableDef unlockableDef = UnlockableCatalog.GetUnlockableDef(stringBuilder.ToString());
                    NetworkUser networkUser = Util.LookUpBodyNetworkUser(UmbraMenu.LocalPlayerBody);
                    if (networkUser)
                    {
                        networkUser.ServerHandleUnlock(unlockableDef);
                    }
                }

            }
        }

        #region Increase/Decrease Value Actions
        public void IncreaseMoney()
        {
            if (MoneyToGive >= 50)
                MoneyToGive += 50;
        }

        public void IncreaseCoins()
        {
            if (CoinsToGive >= 10)
                CoinsToGive += 10;
        }

        public void IncreaseXP()
        {
            if (XPToGive >= 50)
                XPToGive += 50;
        }

        public void DecreaseMoney()
        {
            if (MoneyToGive > 50)
                MoneyToGive -= 50;
        }

        public void DecreaseCoins()
        {
            if (CoinsToGive > 10)
                CoinsToGive -= 10;
        }

        public void DecreaseXP()
        {
            if (XPToGive > 50)
                XPToGive -= 50;
        }
        #endregion
    }
}

