using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F8 RID: 504
	public class PlayerSurvivedState : GameState
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x0002A834 File Offset: 0x00028A34
		private void OnClickRetry()
		{
			this.owner.ChangeState<TransitionToRetryState>();
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002A841 File Offset: 0x00028A41
		private void OnClickQuit()
		{
			this.owner.ChangeState<TransitionToTitleState>();
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002A948 File Offset: 0x00028B48
		public override void Enter()
		{
			base.retryRunButton.onClick.AddListener(new UnityAction(this.OnClickRetry));
			base.quitToTitleButton.onClick.AddListener(new UnityAction(this.OnClickQuit));
			AudioManager.Instance.SetLowPassFilter(true);
			base.youSurvivedSFX.Play(null);
			base.hud.Hide();
			Score score = ScoreCalculator.SharedInstance.GetScore();
			base.endScreenUIC.SetScores(score);
			base.endScreenUIC.Show(true);
			base.powerupListUI.Show();
			base.loadoutUI.Show();
			if (!SelectedMap.MapData.achievementsDisabled)
			{
				this.CheckDifficultyUnlock();
				this.CheckAchievmentUnlocks();
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002AA04 File Offset: 0x00028C04
		public override void Exit()
		{
			base.retryRunButton.onClick.RemoveListener(new UnityAction(this.OnClickRetry));
			base.quitToTitleButton.onClick.RemoveListener(new UnityAction(this.OnClickQuit));
			base.powerupListUI.Hide();
			base.loadoutUI.Hide();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0002AA6C File Offset: 0x00028C6C
		private void CheckDifficultyUnlock()
		{
			if (Loadout.difficultyLevel == SaveSystem.data.difficultyUnlocked && Loadout.difficultyLevel < 15)
			{
				SaveSystem.data.difficultyUnlocked++;
			}
			int characterIndex = Loadout.CharacterIndex;
			if (SaveSystem.data.characterHighestClear[characterIndex] < Loadout.difficultyLevel)
			{
				SaveSystem.data.characterHighestClear[characterIndex] = Loadout.difficultyLevel;
			}
			int gunIndex = Loadout.GunIndex;
			if (SaveSystem.data.gunHighestClear[gunIndex] < Loadout.difficultyLevel)
			{
				SaveSystem.data.gunHighestClear[gunIndex] = Loadout.difficultyLevel;
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0002AAFC File Offset: 0x00028CFC
		private void CheckAchievmentUnlocks()
		{
			SteamIntegration.UnlockAchievement("ACH_SURVIVE20");
			string name = Loadout.GunSelection.name;
			string name2 = Loadout.CharacterSelection.name;
			if (!base.shootDetector.usedShooting)
			{
				SteamIntegration.UnlockAchievement("ACH_PACIFIST");
			}
			if (!base.shootDetector.usedManualShooting && name2 == "Abby" && name == "GrenadeLauncherData")
			{
				SteamIntegration.UnlockAchievement("ACH_RECKLESS");
			}
			if (base.hitlessDetector.hitless)
			{
				SteamIntegration.UnlockAchievement("ACH_NIMBLE");
			}
			if (base.playerHealth.maxHP == 1)
			{
				SteamIntegration.UnlockAchievement("ACH_ON_THE_EDGE");
			}
			if (Object.FindObjectsOfType<Summon>().Length >= 8)
			{
				SteamIntegration.UnlockAchievement("ACH_CATCH_THEM_ALL");
			}
			if (Loadout.difficultyLevel >= 1)
			{
				SteamIntegration.UnlockAchievement("ACH_DARKNESS1");
			}
			if (Loadout.difficultyLevel >= 5)
			{
				SteamIntegration.UnlockAchievement("ACH_DARKNESS5");
			}
			if (Loadout.difficultyLevel >= 10)
			{
				SteamIntegration.UnlockAchievement("ACH_DARKNESS10");
			}
			if (Loadout.difficultyLevel >= 15)
			{
				SteamIntegration.UnlockAchievement("ACH_DARKNESS15");
				if (name == "RevolverData")
				{
					SteamIntegration.UnlockAchievement("ACH_REVOLVER");
				}
				else if (name == "ShotgunData")
				{
					SteamIntegration.UnlockAchievement("ACH_SHOTGUN");
				}
				else if (name == "CrossbowData")
				{
					SteamIntegration.UnlockAchievement("ACH_CROSSBOW");
				}
				else if (name == "FlameCannon")
				{
					SteamIntegration.UnlockAchievement("ACH_FLAME_CANNON");
				}
				else if (name == "DualSMGsData")
				{
					SteamIntegration.UnlockAchievement("ACH_SMGS");
				}
				else if (name == "BatGunData")
				{
					SteamIntegration.UnlockAchievement("ACH_BATGUN");
				}
				else if (name == "GrenadeLauncherData")
				{
					SteamIntegration.UnlockAchievement("ACH_GRENADE_LAUNCHER");
				}
				else if (name == "MagicBowData")
				{
					SteamIntegration.UnlockAchievement("ACH_MAGIC_BOW");
				}
				else if (name == "SwordData")
				{
					SteamIntegration.UnlockAchievement("ACH_CYCLONE_SWORD");
				}
				else if (name == "SalvoKnivesData")
				{
					SteamIntegration.UnlockAchievement("ACH_SALVO_KNIVES");
				}
				else if (name == "SporeGunData")
				{
					SteamIntegration.UnlockAchievement("ACH_WATERING_GUN");
				}
				if (name2 == "Shana")
				{
					SteamIntegration.UnlockAchievement("ACH_SHANA");
					return;
				}
				if (name2 == "Diamond")
				{
					SteamIntegration.UnlockAchievement("ACH_DIAMOND");
					return;
				}
				if (name2 == "Hina")
				{
					SteamIntegration.UnlockAchievement("ACH_HINA");
					return;
				}
				if (name2 == "Scarlett")
				{
					SteamIntegration.UnlockAchievement("ACH_SCARLETT");
					return;
				}
				if (name2 == "Spark")
				{
					SteamIntegration.UnlockAchievement("ACH_SPARK");
					return;
				}
				if (name2 == "Lilith")
				{
					SteamIntegration.UnlockAchievement("ACH_LILITH");
					return;
				}
				if (name2 == "Abby")
				{
					SteamIntegration.UnlockAchievement("ACH_ABBY");
					return;
				}
				if (name2 == "Yuki")
				{
					SteamIntegration.UnlockAchievement("ACH_YUKI");
					return;
				}
				if (name2 == "Luna")
				{
					SteamIntegration.UnlockAchievement("ACH_LUNA");
					return;
				}
				if (name2 == "Hastur")
				{
					SteamIntegration.UnlockAchievement("ACH_HASTUR");
					return;
				}
				if (name2 == "Raven")
				{
					SteamIntegration.UnlockAchievement("ACH_RAVEN");
					return;
				}
				if (name2 == "Dasher")
				{
					SteamIntegration.UnlockAchievement("ACH_DASHER");
					return;
				}
				if (name2 == "Katana")
				{
					SteamIntegration.UnlockAchievement("ACH_KATANA");
				}
			}
		}
	}
}
