using System;
using System.Collections.Generic;
using flanne.PerkSystem;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F3 RID: 243
	[CreateAssetMenu(fileName = "Perk", menuName = "Perk")]
	public class Powerup : ScriptableObject
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001EC02 File Offset: 0x0001CE02
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStrID.key);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001EC14 File Offset: 0x0001CE14
		public string description
		{
			get
			{
				string text = string.Empty;
				foreach (StatChange statChange in this.statChanges)
				{
					text = text + LocalizationSystem.GetLocalizedValue(StatLabels.Labels[statChange.type]) + " ";
					if (statChange.isFlatMod)
					{
						if (statChange.flatValue > 0)
						{
							text = string.Concat(new object[]
							{
								text,
								"<color=#f5d6c1>+",
								statChange.flatValue,
								"</color><br>"
							});
						}
						else if (statChange.flatValue < 0)
						{
							text = string.Concat(new object[]
							{
								text,
								"<color=#fd5161>",
								statChange.flatValue,
								"</color><br>"
							});
						}
					}
					else if (statChange.value > 0f)
					{
						text = string.Concat(new object[]
						{
							text,
							"<color=#f5d6c1>+",
							Mathf.FloorToInt(statChange.value * 100f),
							"%</color><br>"
						});
					}
					else if (statChange.value < 0f)
					{
						text = string.Concat(new object[]
						{
							text,
							"<color=#fd5161>",
							Mathf.FloorToInt(statChange.value * 100f),
							"%</color><br>"
						});
					}
				}
				return text + LocalizationSystem.GetLocalizedValue(this.desStrID.key);
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001ED94 File Offset: 0x0001CF94
		public void Apply(PlayerController player)
		{
			this.PostNotification(Powerup.AppliedNotifcation);
			player.onDestroyed.AddListener(delegate()
			{
				this.OnPlayerDestroyed(player, this.effects);
			});
			this.ApplyStats(player);
			PerkEffect[] array = this.effects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Equip(player);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001EE0C File Offset: 0x0001D00C
		public void ApplyStack(PlayerController player)
		{
			this.PostNotification(Powerup.AppliedNotifcation);
			this.ApplyStats(player);
			PerkEffect[] array;
			if (this.stackedEffects.Length == 0)
			{
				player.onDestroyed.AddListener(delegate()
				{
					this.OnPlayerDestroyed(player, this.effects);
				});
				array = this.effects;
			}
			else
			{
				player.onDestroyed.AddListener(delegate()
				{
					this.OnPlayerDestroyed(player, this.stackedEffects);
				});
				array = this.stackedEffects;
			}
			PerkEffect[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Equip(player);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001EEB4 File Offset: 0x0001D0B4
		private void ApplyStats(PlayerController player)
		{
			StatsHolder componentInChildren = player.GetComponentInChildren<StatsHolder>();
			if (componentInChildren == null)
			{
				Debug.LogWarning("Cannot apply stat mods. No stats holder on target game object.");
				return;
			}
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					componentInChildren[statChange.type].AddFlatBonus(statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					componentInChildren[statChange.type].AddMultiplierBonus(statChange.value);
				}
				else if (statChange.value < 0f)
				{
					componentInChildren[statChange.type].AddMultiplierReduction(1f + statChange.value);
				}
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001EF70 File Offset: 0x0001D170
		private void OnPlayerDestroyed(PlayerController player, PerkEffect[] equippedEffects)
		{
			player.onDestroyed.RemoveListener(delegate()
			{
				this.OnPlayerDestroyed(player, equippedEffects);
			});
			PerkEffect[] equippedEffects2 = equippedEffects;
			for (int i = 0; i < equippedEffects2.Length; i++)
			{
				equippedEffects2[i].UnEquip(player);
			}
		}

		// Token: 0x040004C5 RID: 1221
		public static string AppliedNotifcation = "Powerup.AppliedNotifcation";

		// Token: 0x040004C6 RID: 1222
		public Sprite icon;

		// Token: 0x040004C7 RID: 1223
		[SerializeField]
		private LocalizedString nameStrID;

		// Token: 0x040004C8 RID: 1224
		[SerializeField]
		private LocalizedString desStrID;

		// Token: 0x040004C9 RID: 1225
		public bool anyPrereqFulfill;

		// Token: 0x040004CA RID: 1226
		public List<Powerup> prereqs;

		// Token: 0x040004CB RID: 1227
		public PowerupTreeUIData powerupTreeUIData;

		// Token: 0x040004CC RID: 1228
		[SerializeField]
		private StatChange[] statChanges;

		// Token: 0x040004CD RID: 1229
		[SerializeField]
		private PerkEffect[] effects;

		// Token: 0x040004CE RID: 1230
		[SerializeField]
		private PerkEffect[] stackedEffects;
	}
}
