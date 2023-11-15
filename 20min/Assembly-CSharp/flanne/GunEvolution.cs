using System;
using flanne.PerkSystem;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000BA RID: 186
	[CreateAssetMenu(fileName = "GunEvo", menuName = "GunEvo")]
	public class GunEvolution : ScriptableObject
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0001BFE1 File Offset: 0x0001A1E1
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStrID.key);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
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

		// Token: 0x0600060F RID: 1551 RVA: 0x0001C174 File Offset: 0x0001A374
		public void Apply(PlayerController player)
		{
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
			if (this.gunData != null)
			{
				player.gun.LoadGun(this.gunData);
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001C204 File Offset: 0x0001A404
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

		// Token: 0x06000611 RID: 1553 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
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

		// Token: 0x040003E1 RID: 993
		public Sprite icon;

		// Token: 0x040003E2 RID: 994
		[SerializeField]
		private LocalizedString nameStrID;

		// Token: 0x040003E3 RID: 995
		[SerializeField]
		private LocalizedString desStrID;

		// Token: 0x040003E4 RID: 996
		[SerializeField]
		private GunData gunData;

		// Token: 0x040003E5 RID: 997
		[SerializeField]
		private StatChange[] statChanges;

		// Token: 0x040003E6 RID: 998
		[SerializeField]
		private PerkEffect[] effects;
	}
}
