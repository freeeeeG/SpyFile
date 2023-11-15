using System;
using Klei.AI;

// Token: 0x02000AA0 RID: 2720
public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005316 RID: 21270 RVA: 0x001DCA68 File Offset: 0x001DAC68
	public GermResistanceAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x001DCA72 File Offset: 0x001DAC72
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
	}
}
