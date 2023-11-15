using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000A9B RID: 2715
public interface IAttributeFormatter
{
	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x060052FC RID: 21244
	// (set) Token: 0x060052FD RID: 21245
	GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x060052FE RID: 21246
	string GetFormattedAttribute(AttributeInstance instance);

	// Token: 0x060052FF RID: 21247
	string GetFormattedModifier(AttributeModifier modifier);

	// Token: 0x06005300 RID: 21248
	string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

	// Token: 0x06005301 RID: 21249
	string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance);

	// Token: 0x06005302 RID: 21250
	string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters);
}
