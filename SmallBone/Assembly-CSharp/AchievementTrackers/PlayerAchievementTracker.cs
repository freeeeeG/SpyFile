using System;
using System.Collections.ObjectModel;
using Characters;
using Characters.Gear.Weapons;
using CutScenes;
using Data;
using Level;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x0200143B RID: 5179
	public class PlayerAchievementTracker : MonoBehaviour
	{
		// Token: 0x06006590 RID: 26000 RVA: 0x00125F2C File Offset: 0x0012412C
		private void Awake()
		{
			this._player.health.onDied += this.OnPlayerDied;
			this._player.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.TrackMapAchievement;
			this._player.playerComponents.inventory.weapon.onChanged += this.TrackHeadLootAchievement;
			GameData.Currency.heartQuartz.onEarn += this.HandleOnEarnHeartQuartz;
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x00125FC8 File Offset: 0x001241C8
		private void HandleOnEarnHeartQuartz(int amount)
		{
			if (GameData.Currency.heartQuartz.totalIncome >= 100)
			{
				Achievement.Type.Darkheart.Set();
			}
		}

		// Token: 0x06006592 RID: 26002 RVA: 0x00125FDF File Offset: 0x001241DF
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.TrackMapAchievement;
		}

		// Token: 0x06006593 RID: 26003 RVA: 0x00126004 File Offset: 0x00124204
		private void OnPlayerDied()
		{
			Achievement.Type.TheLegendBegins.Set();
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x0012600C File Offset: 0x0012420C
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (this._player.health.currentHealth > 0.0)
			{
				return;
			}
			bool flag = tookDamage.attacker.trap != null;
			if (tookDamage.attacker.character != null && tookDamage.attacker.character.type == Character.Type.Trap)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			Achievement.Type.Concentration.Set();
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x00126079 File Offset: 0x00124279
		private void TrackMapAchievement(Map old, Map @new)
		{
			if (Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.Castle)
			{
				return;
			}
			if (!GameData.Progress.cutscene.GetData(CutScenes.Key.ending))
			{
				return;
			}
			Achievement.Type.GoHome.Set();
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x001260AC File Offset: 0x001242AC
		private void TrackHeadLootAchievement(Weapon old, Weapon @new)
		{
			if (@new == null)
			{
				return;
			}
			if (@new.rarity != Rarity.Legendary)
			{
				return;
			}
			ReadOnlyCollection<string> names = EnumValues<Achievement.Type>.Names;
			int num = -1;
			for (int i = 0; i < names.Count; i++)
			{
				if (@new.name.IndexOf(names[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				Debug.Log("There is no achievement for Legendary Head " + @new.name + ".");
				return;
			}
			EnumValues<Achievement.Type>.Values[num].Set();
		}

		// Token: 0x040051C7 RID: 20935
		[SerializeField]
		private Character _player;
	}
}
