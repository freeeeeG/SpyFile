using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200015C RID: 348
	public class StatRune : Rune
	{
		// Token: 0x060008E2 RID: 2274 RVA: 0x00025160 File Offset: 0x00023360
		protected override void Init()
		{
			foreach (StatChange s in this.statChanges)
			{
				for (int j = 0; j < this.level; j++)
				{
					this.ApplyStat(s);
				}
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000251A4 File Offset: 0x000233A4
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

		// Token: 0x0400069D RID: 1693
		[SerializeField]
		private StatChange[] statChanges = new StatChange[0];
	}
}
