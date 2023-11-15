using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E14 RID: 3604
	public class TagGrowthRule : GrowthRule
	{
		// Token: 0x06006E90 RID: 28304 RVA: 0x002B81BD File Offset: 0x002B63BD
		public TagGrowthRule(Tag tag)
		{
			this.tag = tag;
		}

		// Token: 0x06006E91 RID: 28305 RVA: 0x002B81CC File Offset: 0x002B63CC
		public override bool Test(Element e)
		{
			return e.HasTag(this.tag);
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x002B81DA File Offset: 0x002B63DA
		public override string Name()
		{
			return this.tag.ProperName();
		}

		// Token: 0x040052CF RID: 21199
		public Tag tag;
	}
}
