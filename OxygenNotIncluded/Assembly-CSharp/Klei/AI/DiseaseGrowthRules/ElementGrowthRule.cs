using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E13 RID: 3603
	public class ElementGrowthRule : GrowthRule
	{
		// Token: 0x06006E8D RID: 28301 RVA: 0x002B818C File Offset: 0x002B638C
		public ElementGrowthRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x002B819B File Offset: 0x002B639B
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x06006E8F RID: 28303 RVA: 0x002B81AB File Offset: 0x002B63AB
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x040052CE RID: 21198
		public SimHashes element;
	}
}
