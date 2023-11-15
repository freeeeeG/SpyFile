using System;
using System.Collections.Generic;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E17 RID: 3607
	public class ExposureRule
	{
		// Token: 0x06006E9A RID: 28314 RVA: 0x002B83A8 File Offset: 0x002B65A8
		public void Apply(ElemExposureInfo[] infoList)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (this.Test(elements[i]))
				{
					ElemExposureInfo elemExposureInfo = infoList[i];
					if (this.populationHalfLife != null)
					{
						elemExposureInfo.populationHalfLife = this.populationHalfLife.Value;
					}
					infoList[i] = elemExposureInfo;
				}
			}
		}

		// Token: 0x06006E9B RID: 28315 RVA: 0x002B840B File Offset: 0x002B660B
		public virtual bool Test(Element e)
		{
			return true;
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x002B840E File Offset: 0x002B660E
		public virtual string Name()
		{
			return null;
		}

		// Token: 0x040052DA RID: 21210
		public float? populationHalfLife;
	}
}
