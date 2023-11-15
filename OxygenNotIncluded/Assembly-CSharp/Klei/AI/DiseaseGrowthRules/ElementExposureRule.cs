using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E18 RID: 3608
	public class ElementExposureRule : ExposureRule
	{
		// Token: 0x06006E9E RID: 28318 RVA: 0x002B8419 File Offset: 0x002B6619
		public ElementExposureRule(SimHashes element)
		{
			this.element = element;
		}

		// Token: 0x06006E9F RID: 28319 RVA: 0x002B8428 File Offset: 0x002B6628
		public override bool Test(Element e)
		{
			return e.id == this.element;
		}

		// Token: 0x06006EA0 RID: 28320 RVA: 0x002B8438 File Offset: 0x002B6638
		public override string Name()
		{
			return ElementLoader.FindElementByHash(this.element).name;
		}

		// Token: 0x040052DB RID: 21211
		public SimHashes element;
	}
}
