using System;

namespace Database
{
	// Token: 0x02000D22 RID: 3362
	public class StatusItemCategories : ResourceSet<StatusItemCategory>
	{
		// Token: 0x06006A16 RID: 27158 RVA: 0x00294CB4 File Offset: 0x00292EB4
		public StatusItemCategories(ResourceSet parent) : base("StatusItemCategories", parent)
		{
			this.Main = new StatusItemCategory("Main", this, "Main");
			this.Role = new StatusItemCategory("Role", this, "Role");
			this.Power = new StatusItemCategory("Power", this, "Power");
			this.Toilet = new StatusItemCategory("Toilet", this, "Toilet");
			this.Research = new StatusItemCategory("Research", this, "Research");
			this.Hitpoints = new StatusItemCategory("Hitpoints", this, "Hitpoints");
			this.Suffocation = new StatusItemCategory("Suffocation", this, "Suffocation");
			this.WoundEffects = new StatusItemCategory("WoundEffects", this, "WoundEffects");
			this.EntityReceptacle = new StatusItemCategory("EntityReceptacle", this, "EntityReceptacle");
			this.PreservationState = new StatusItemCategory("PreservationState", this, "PreservationState");
			this.PreservationTemperature = new StatusItemCategory("PreservationTemperature", this, "PreservationTemperature");
			this.PreservationAtmosphere = new StatusItemCategory("PreservationAtmosphere", this, "PreservationAtmosphere");
			this.ExhaustTemperature = new StatusItemCategory("ExhaustTemperature", this, "ExhaustTemperature");
			this.OperatingEnergy = new StatusItemCategory("OperatingEnergy", this, "OperatingEnergy");
			this.AccessControl = new StatusItemCategory("AccessControl", this, "AccessControl");
			this.RequiredRoom = new StatusItemCategory("RequiredRoom", this, "RequiredRoom");
			this.Yield = new StatusItemCategory("Yield", this, "Yield");
			this.Heat = new StatusItemCategory("Heat", this, "Heat");
			this.Stored = new StatusItemCategory("Stored", this, "Stored");
			this.Ownable = new StatusItemCategory("Ownable", this, "Ownable");
		}

		// Token: 0x04004D23 RID: 19747
		public StatusItemCategory Main;

		// Token: 0x04004D24 RID: 19748
		public StatusItemCategory Role;

		// Token: 0x04004D25 RID: 19749
		public StatusItemCategory Power;

		// Token: 0x04004D26 RID: 19750
		public StatusItemCategory Toilet;

		// Token: 0x04004D27 RID: 19751
		public StatusItemCategory Research;

		// Token: 0x04004D28 RID: 19752
		public StatusItemCategory Hitpoints;

		// Token: 0x04004D29 RID: 19753
		public StatusItemCategory Suffocation;

		// Token: 0x04004D2A RID: 19754
		public StatusItemCategory WoundEffects;

		// Token: 0x04004D2B RID: 19755
		public StatusItemCategory EntityReceptacle;

		// Token: 0x04004D2C RID: 19756
		public StatusItemCategory PreservationState;

		// Token: 0x04004D2D RID: 19757
		public StatusItemCategory PreservationAtmosphere;

		// Token: 0x04004D2E RID: 19758
		public StatusItemCategory PreservationTemperature;

		// Token: 0x04004D2F RID: 19759
		public StatusItemCategory ExhaustTemperature;

		// Token: 0x04004D30 RID: 19760
		public StatusItemCategory OperatingEnergy;

		// Token: 0x04004D31 RID: 19761
		public StatusItemCategory AccessControl;

		// Token: 0x04004D32 RID: 19762
		public StatusItemCategory RequiredRoom;

		// Token: 0x04004D33 RID: 19763
		public StatusItemCategory Yield;

		// Token: 0x04004D34 RID: 19764
		public StatusItemCategory Heat;

		// Token: 0x04004D35 RID: 19765
		public StatusItemCategory Stored;

		// Token: 0x04004D36 RID: 19766
		public StatusItemCategory Ownable;
	}
}
