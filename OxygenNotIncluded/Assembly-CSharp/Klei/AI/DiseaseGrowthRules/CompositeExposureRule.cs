using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E19 RID: 3609
	public class CompositeExposureRule
	{
		// Token: 0x06006EA1 RID: 28321 RVA: 0x002B844A File Offset: 0x002B664A
		public string Name()
		{
			return this.name;
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x002B8452 File Offset: 0x002B6652
		public void Overlay(ExposureRule rule)
		{
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x06006EA3 RID: 28323 RVA: 0x002B847F File Offset: 0x002B667F
		public float GetHalfLifeForCount(int count)
		{
			return this.populationHalfLife;
		}

		// Token: 0x040052DC RID: 21212
		public string name;

		// Token: 0x040052DD RID: 21213
		public float populationHalfLife;
	}
}
