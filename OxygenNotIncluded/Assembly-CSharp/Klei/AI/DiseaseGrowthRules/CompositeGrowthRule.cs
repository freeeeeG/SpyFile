using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E15 RID: 3605
	public class CompositeGrowthRule
	{
		// Token: 0x06006E93 RID: 28307 RVA: 0x002B81E7 File Offset: 0x002B63E7
		public string Name()
		{
			return this.name;
		}

		// Token: 0x06006E94 RID: 28308 RVA: 0x002B81F0 File Offset: 0x002B63F0
		public void Overlay(GrowthRule rule)
		{
			if (rule.underPopulationDeathRate != null)
			{
				this.underPopulationDeathRate = rule.underPopulationDeathRate.Value;
			}
			if (rule.populationHalfLife != null)
			{
				this.populationHalfLife = rule.populationHalfLife.Value;
			}
			if (rule.overPopulationHalfLife != null)
			{
				this.overPopulationHalfLife = rule.overPopulationHalfLife.Value;
			}
			if (rule.diffusionScale != null)
			{
				this.diffusionScale = rule.diffusionScale.Value;
			}
			if (rule.minCountPerKG != null)
			{
				this.minCountPerKG = rule.minCountPerKG.Value;
			}
			if (rule.maxCountPerKG != null)
			{
				this.maxCountPerKG = rule.maxCountPerKG.Value;
			}
			if (rule.minDiffusionCount != null)
			{
				this.minDiffusionCount = rule.minDiffusionCount.Value;
			}
			if (rule.minDiffusionInfestationTickCount != null)
			{
				this.minDiffusionInfestationTickCount = rule.minDiffusionInfestationTickCount.Value;
			}
			this.name = rule.Name();
		}

		// Token: 0x06006E95 RID: 28309 RVA: 0x002B8300 File Offset: 0x002B6500
		public float GetHalfLifeForCount(int count, float kg)
		{
			int num = (int)(this.minCountPerKG * kg);
			int num2 = (int)(this.maxCountPerKG * kg);
			if (count < num)
			{
				return this.populationHalfLife;
			}
			if (count < num2)
			{
				return this.populationHalfLife;
			}
			return this.overPopulationHalfLife;
		}

		// Token: 0x040052D0 RID: 21200
		public string name;

		// Token: 0x040052D1 RID: 21201
		public float underPopulationDeathRate;

		// Token: 0x040052D2 RID: 21202
		public float populationHalfLife;

		// Token: 0x040052D3 RID: 21203
		public float overPopulationHalfLife;

		// Token: 0x040052D4 RID: 21204
		public float diffusionScale;

		// Token: 0x040052D5 RID: 21205
		public float minCountPerKG;

		// Token: 0x040052D6 RID: 21206
		public float maxCountPerKG;

		// Token: 0x040052D7 RID: 21207
		public int minDiffusionCount;

		// Token: 0x040052D8 RID: 21208
		public byte minDiffusionInfestationTickCount;
	}
}
