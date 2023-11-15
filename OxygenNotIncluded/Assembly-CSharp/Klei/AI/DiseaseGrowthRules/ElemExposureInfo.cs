using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E16 RID: 3606
	public struct ElemExposureInfo
	{
		// Token: 0x06006E97 RID: 28311 RVA: 0x002B8345 File Offset: 0x002B6545
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.populationHalfLife);
		}

		// Token: 0x06006E98 RID: 28312 RVA: 0x002B8354 File Offset: 0x002B6554
		public static void SetBulk(ElemExposureInfo[] info, Func<Element, bool> test, ElemExposureInfo settings)
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

		// Token: 0x06006E99 RID: 28313 RVA: 0x002B838F File Offset: 0x002B658F
		public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
		{
			return (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
		}

		// Token: 0x040052D9 RID: 21209
		public float populationHalfLife;
	}
}
