using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000D13 RID: 3347
	public class PlantAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x060069E5 RID: 27109 RVA: 0x0029138C File Offset: 0x0028F58C
		public PlantAttributes(ResourceSet parent) : base("PlantAttributes", parent)
		{
			this.WiltTempRangeMod = base.Add(new Klei.AI.Attribute("WiltTempRangeMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.WiltTempRangeMod.SetFormatter(new PercentAttributeFormatter());
			this.YieldAmount = base.Add(new Klei.AI.Attribute("YieldAmount", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.YieldAmount.SetFormatter(new PercentAttributeFormatter());
			this.HarvestTime = base.Add(new Klei.AI.Attribute("HarvestTime", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.HarvestTime.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Time, GameUtil.TimeSlice.None));
			this.DecorBonus = base.Add(new Klei.AI.Attribute("DecorBonus", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DecorBonus.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.MinLightLux = base.Add(new Klei.AI.Attribute("MinLightLux", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinLightLux.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Lux, GameUtil.TimeSlice.None));
			this.FertilizerUsageMod = base.Add(new Klei.AI.Attribute("FertilizerUsageMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.FertilizerUsageMod.SetFormatter(new PercentAttributeFormatter());
			this.MinRadiationThreshold = base.Add(new Klei.AI.Attribute("MinRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
			this.MaxRadiationThreshold = base.Add(new Klei.AI.Attribute("MaxRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MaxRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
		}

		// Token: 0x04004C99 RID: 19609
		public Klei.AI.Attribute WiltTempRangeMod;

		// Token: 0x04004C9A RID: 19610
		public Klei.AI.Attribute YieldAmount;

		// Token: 0x04004C9B RID: 19611
		public Klei.AI.Attribute HarvestTime;

		// Token: 0x04004C9C RID: 19612
		public Klei.AI.Attribute DecorBonus;

		// Token: 0x04004C9D RID: 19613
		public Klei.AI.Attribute MinLightLux;

		// Token: 0x04004C9E RID: 19614
		public Klei.AI.Attribute FertilizerUsageMod;

		// Token: 0x04004C9F RID: 19615
		public Klei.AI.Attribute MinRadiationThreshold;

		// Token: 0x04004CA0 RID: 19616
		public Klei.AI.Attribute MaxRadiationThreshold;
	}
}
