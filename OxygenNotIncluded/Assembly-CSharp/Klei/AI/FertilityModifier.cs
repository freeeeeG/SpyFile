using System;

namespace Klei.AI
{
	// Token: 0x02000E00 RID: 3584
	public class FertilityModifier : Resource
	{
		// Token: 0x06006DF5 RID: 28149 RVA: 0x002B58BC File Offset: 0x002B3ABC
		public FertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction) : base(id, name)
		{
			this.Description = description;
			this.TargetTag = targetTag;
			this.TooltipCB = tooltipCB;
			this.ApplyFunction = applyFunction;
		}

		// Token: 0x06006DF6 RID: 28150 RVA: 0x002B58E5 File Offset: 0x002B3AE5
		public string GetTooltip()
		{
			if (this.TooltipCB != null)
			{
				return this.TooltipCB(this.Description);
			}
			return this.Description;
		}

		// Token: 0x0400527E RID: 21118
		public string Description;

		// Token: 0x0400527F RID: 21119
		public Tag TargetTag;

		// Token: 0x04005280 RID: 21120
		public Func<string, string> TooltipCB;

		// Token: 0x04005281 RID: 21121
		public FertilityModifier.FertilityModFn ApplyFunction;

		// Token: 0x02001F89 RID: 8073
		// (Invoke) Token: 0x0600A2DC RID: 41692
		public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
	}
}
