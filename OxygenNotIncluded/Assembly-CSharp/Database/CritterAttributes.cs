using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000CF9 RID: 3321
	public class CritterAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06006987 RID: 27015 RVA: 0x0028AD68 File Offset: 0x00288F68
		public CritterAttributes(ResourceSet parent) : base("CritterAttributes", parent)
		{
			this.Happiness = base.Add(new Klei.AI.Attribute("Happiness", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.NAME"), "", Strings.Get("STRINGS.CREATURES.STATS.HAPPINESS.TOOLTIP"), 0f, Klei.AI.Attribute.Display.General, false, "ui_icon_happiness", null, null));
			this.Happiness.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.Metabolism = base.Add(new Klei.AI.Attribute("Metabolism", false, Klei.AI.Attribute.Display.Details, false, 0f, null, null, null, null));
			this.Metabolism.SetFormatter(new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04004B05 RID: 19205
		public Klei.AI.Attribute Happiness;

		// Token: 0x04004B06 RID: 19206
		public Klei.AI.Attribute Metabolism;
	}
}
