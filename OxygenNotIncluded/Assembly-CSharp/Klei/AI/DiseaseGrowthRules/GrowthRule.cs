using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E11 RID: 3601
	public class GrowthRule
	{
		// Token: 0x06006E86 RID: 28294 RVA: 0x002B7FF8 File Offset: 0x002B61F8
		public void Apply(ElemGrowthInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				Element element = elements[i];
				if (element.id != SimHashes.Vacuum && this.Test(element))
				{
					ElemGrowthInfo elemGrowthInfo = infoList[i];
					if (this.underPopulationDeathRate != null)
					{
						elemGrowthInfo.underPopulationDeathRate = this.underPopulationDeathRate.Value;
					}
					if (this.populationHalfLife != null)
					{
						elemGrowthInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					if (this.overPopulationHalfLife != null)
					{
						elemGrowthInfo.overPopulationHalfLife = this.overPopulationHalfLife.Value;
					}
					if (this.diffusionScale != null)
					{
						elemGrowthInfo.diffusionScale = this.diffusionScale.Value;
					}
					if (this.minCountPerKG != null)
					{
						elemGrowthInfo.minCountPerKG = this.minCountPerKG.Value;
					}
					if (this.maxCountPerKG != null)
					{
						elemGrowthInfo.maxCountPerKG = this.maxCountPerKG.Value;
					}
					if (this.minDiffusionCount != null)
					{
						elemGrowthInfo.minDiffusionCount = this.minDiffusionCount.Value;
					}
					if (this.minDiffusionInfestationTickCount != null)
					{
						elemGrowthInfo.minDiffusionInfestationTickCount = this.minDiffusionInfestationTickCount.Value;
					}
					infoList[i] = elemGrowthInfo;
				}
			}
		}

		// Token: 0x06006E87 RID: 28295 RVA: 0x002B8154 File Offset: 0x002B6354
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x06006E88 RID: 28296 RVA: 0x002B8157 File Offset: 0x002B6357
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x040052C5 RID: 21189
		public float? underPopulationDeathRate;

		// Token: 0x040052C6 RID: 21190
		public float? populationHalfLife;

		// Token: 0x040052C7 RID: 21191
		public float? overPopulationHalfLife;

		// Token: 0x040052C8 RID: 21192
		public float? diffusionScale;

		// Token: 0x040052C9 RID: 21193
		public float? minCountPerKG;

		// Token: 0x040052CA RID: 21194
		public float? maxCountPerKG;

		// Token: 0x040052CB RID: 21195
		public int? minDiffusionCount;

		// Token: 0x040052CC RID: 21196
		public byte? minDiffusionInfestationTickCount;
	}
}
