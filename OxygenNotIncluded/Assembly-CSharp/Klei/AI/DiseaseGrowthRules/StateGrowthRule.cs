using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02000E12 RID: 3602
	public class StateGrowthRule : GrowthRule
	{
		// Token: 0x06006E8A RID: 28298 RVA: 0x002B8162 File Offset: 0x002B6362
		public StateGrowthRule(Element.State state)
		{
			this.state = state;
		}

		// Token: 0x06006E8B RID: 28299 RVA: 0x002B8171 File Offset: 0x002B6371
		public override bool Test(Element e)
		{
			return e.IsState(this.state);
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x002B817F File Offset: 0x002B637F
		public override string Name()
		{
			return Element.GetStateString(this.state);
		}

		// Token: 0x040052CD RID: 21197
		public Element.State state;
	}
}
