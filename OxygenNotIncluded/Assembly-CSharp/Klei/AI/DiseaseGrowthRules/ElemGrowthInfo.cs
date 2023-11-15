using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E10 RID: 3600
	public struct ElemGrowthInfo
	{
		// Token: 0x06006E83 RID: 28291 RVA: 0x002B7EE0 File Offset: 0x002B60E0
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.underPopulationDeathRate);
			writer.Write(this.populationHalfLife);
			writer.Write(this.overPopulationHalfLife);
			writer.Write(this.diffusionScale);
			writer.Write(this.minCountPerKG);
			writer.Write(this.maxCountPerKG);
			writer.Write(this.minDiffusionCount);
			writer.Write(this.minDiffusionInfestationTickCount);
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x002B7F50 File Offset: 0x002B6150
		public static void SetBulk(ElemGrowthInfo[] info, Func<Element, bool> test, ElemGrowthInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x002B7F8C File Offset: 0x002B618C
		public float CalculateDiseaseCountDelta(int disease_count, float kg, float dt)
		{
			float num = this.minCountPerKG * kg;
			float num2 = this.maxCountPerKG * kg;
			float result;
			if (num <= (float)disease_count && (float)disease_count <= num2)
			{
				result = (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
			}
			else if ((float)disease_count < num)
			{
				result = -this.underPopulationDeathRate * dt;
			}
			else
			{
				result = (Disease.HalfLifeToGrowthRate(this.overPopulationHalfLife, dt) - 1f) * (float)disease_count;
			}
			return result;
		}

		// Token: 0x040052BD RID: 21181
		public float underPopulationDeathRate;

		// Token: 0x040052BE RID: 21182
		public float populationHalfLife;

		// Token: 0x040052BF RID: 21183
		public float overPopulationHalfLife;

		// Token: 0x040052C0 RID: 21184
		public float diffusionScale;

		// Token: 0x040052C1 RID: 21185
		public float minCountPerKG;

		// Token: 0x040052C2 RID: 21186
		public float maxCountPerKG;

		// Token: 0x040052C3 RID: 21187
		public int minDiffusionCount;

		// Token: 0x040052C4 RID: 21188
		public byte minDiffusionInfestationTickCount;
	}
}
