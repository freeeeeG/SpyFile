using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000151 RID: 337
	public class HPThresholdStatRune : Rune
	{
		// Token: 0x060008B4 RID: 2228 RVA: 0x00024788 File Offset: 0x00022988
		private void OnHpChange(int hp)
		{
			if (this.statsActive == this.playerHealth.hp <= this.playerHealth.maxHP / 2)
			{
				return;
			}
			if (this.playerHealth.hp <= this.playerHealth.maxHP / 2)
			{
				this.Activate();
				return;
			}
			this.Deactivate();
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000247E2 File Offset: 0x000229E2
		protected override void Init()
		{
			this.statsActive = false;
			this.playerHealth = this.player.playerHealth;
			this.playerHealth.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHpChange));
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00024818 File Offset: 0x00022A18
		private void OnDestroy()
		{
			this.playerHealth.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHpChange));
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00024838 File Offset: 0x00022A38
		private void Activate()
		{
			foreach (StatChange s in this.statChanges)
			{
				for (int j = 0; j < this.level; j++)
				{
					this.ApplyStat(s);
				}
			}
			this.statsActive = true;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00024884 File Offset: 0x00022A84
		private void Deactivate()
		{
			foreach (StatChange s in this.statChanges)
			{
				for (int j = 0; j < this.level; j++)
				{
					this.RemoveStat(s);
				}
			}
			this.statsActive = false;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000248D0 File Offset: 0x00022AD0
		private void ApplyStat(StatChange s)
		{
			StatsHolder stats = this.player.stats;
			if (s.isFlatMod)
			{
				stats[s.type].AddFlatBonus(s.flatValue);
			}
			else if (s.value > 0f)
			{
				stats[s.type].AddMultiplierBonus(s.value);
			}
			else if (s.value < 0f)
			{
				stats[s.type].AddMultiplierReduction(1f + s.value);
			}
			if (s.type == StatType.MaxHP)
			{
				this.player.playerHealth.maxHP = Mathf.FloorToInt(stats[s.type].Modify((float)this.player.playerHealth.maxHP));
			}
			if (s.type == StatType.CharacterSize)
			{
				this.player.playerSprite.transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
			if (s.type == StatType.PickupRange)
			{
				GameObject.FindGameObjectWithTag("Pickupper").transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
			if (s.type == StatType.VisionRange)
			{
				GameObject.FindGameObjectWithTag("PlayerVision").transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00024A58 File Offset: 0x00022C58
		private void RemoveStat(StatChange s)
		{
			StatsHolder stats = this.player.stats;
			if (s.isFlatMod)
			{
				stats[s.type].AddFlatBonus(-1 * s.flatValue);
			}
			else if (s.value > 0f)
			{
				stats[s.type].AddMultiplierBonus(-1f * s.value);
			}
			else if (s.value < 0f)
			{
				stats[s.type].AddMultiplierReduction(1f + -1f * s.value);
			}
			if (s.type == StatType.MaxHP)
			{
				this.player.playerHealth.maxHP = Mathf.FloorToInt(stats[s.type].Modify((float)this.player.playerHealth.maxHP));
			}
			if (s.type == StatType.CharacterSize)
			{
				this.player.playerSprite.transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
			if (s.type == StatType.PickupRange)
			{
				GameObject.FindGameObjectWithTag("Pickupper").transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
			if (s.type == StatType.VisionRange)
			{
				GameObject.FindGameObjectWithTag("PlayerVision").transform.localScale = Vector3.one * stats[s.type].Modify(1f);
			}
		}

		// Token: 0x04000672 RID: 1650
		[SerializeField]
		private StatChange[] statChanges = new StatChange[0];

		// Token: 0x04000673 RID: 1651
		private bool statsActive;

		// Token: 0x04000674 RID: 1652
		private PlayerHealth playerHealth;
	}
}
