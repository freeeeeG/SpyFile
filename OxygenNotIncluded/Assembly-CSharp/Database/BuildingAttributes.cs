using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000CED RID: 3309
	public class BuildingAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x06006950 RID: 26960 RVA: 0x0027DD58 File Offset: 0x0027BF58
		public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent)
		{
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.DecorRadius = base.Add(new Klei.AI.Attribute("DecorRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollution = base.Add(new Klei.AI.Attribute("NoisePollution", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollutionRadius = base.Add(new Klei.AI.Attribute("NoisePollutionRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Hygiene = base.Add(new Klei.AI.Attribute("Hygiene", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Comfort = base.Add(new Klei.AI.Attribute("Comfort", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature = base.Add(new Klei.AI.Attribute("OverheatTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
			this.FatalTemperature = base.Add(new Klei.AI.Attribute("FatalTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
		}

		// Token: 0x040048FC RID: 18684
		public Klei.AI.Attribute Decor;

		// Token: 0x040048FD RID: 18685
		public Klei.AI.Attribute DecorRadius;

		// Token: 0x040048FE RID: 18686
		public Klei.AI.Attribute NoisePollution;

		// Token: 0x040048FF RID: 18687
		public Klei.AI.Attribute NoisePollutionRadius;

		// Token: 0x04004900 RID: 18688
		public Klei.AI.Attribute Hygiene;

		// Token: 0x04004901 RID: 18689
		public Klei.AI.Attribute Comfort;

		// Token: 0x04004902 RID: 18690
		public Klei.AI.Attribute OverheatTemperature;

		// Token: 0x04004903 RID: 18691
		public Klei.AI.Attribute FatalTemperature;
	}
}
