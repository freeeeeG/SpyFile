using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000D6C RID: 3436
	public class SkillGroups : ResourceSet<SkillGroup>
	{
		// Token: 0x06006B3E RID: 27454 RVA: 0x0029DD28 File Offset: 0x0029BF28
		public SkillGroups(ResourceSet parent) : base("SkillGroups", parent)
		{
			this.Mining = base.Add(new SkillGroup("Mining", Db.Get().ChoreGroups.Dig.Id, DUPLICANTS.CHOREGROUPS.DIG.NAME, "icon_errand_dig", "icon_archetype_dig"));
			this.Mining.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Dig.attribute
			};
			this.Mining.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Dig.Id
			};
			this.Building = base.Add(new SkillGroup("Building", Db.Get().ChoreGroups.Build.Id, DUPLICANTS.CHOREGROUPS.BUILD.NAME, "status_item_pending_repair", "icon_archetype_build"));
			this.Building.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Build.attribute
			};
			this.Building.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Build.Id
			};
			this.Farming = base.Add(new SkillGroup("Farming", Db.Get().ChoreGroups.Farming.Id, DUPLICANTS.CHOREGROUPS.FARMING.NAME, "icon_errand_farm", "icon_archetype_farm"));
			this.Farming.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Farming.attribute
			};
			this.Farming.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Farming.Id
			};
			this.Ranching = base.Add(new SkillGroup("Ranching", Db.Get().ChoreGroups.Ranching.Id, DUPLICANTS.CHOREGROUPS.RANCHING.NAME, "icon_errand_ranch", "icon_archetype_ranch"));
			this.Ranching.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Ranching.attribute
			};
			this.Ranching.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Ranching.Id
			};
			this.Cooking = base.Add(new SkillGroup("Cooking", Db.Get().ChoreGroups.Cook.Id, DUPLICANTS.CHOREGROUPS.COOK.NAME, "icon_errand_cook", "icon_archetype_cook"));
			this.Cooking.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Cook.attribute
			};
			this.Cooking.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Cook.Id
			};
			this.Art = base.Add(new SkillGroup("Art", Db.Get().ChoreGroups.Art.Id, DUPLICANTS.CHOREGROUPS.ART.NAME, "icon_errand_art", "icon_archetype_art"));
			this.Art.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Art.attribute
			};
			this.Art.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Art.Id
			};
			this.Research = base.Add(new SkillGroup("Research", Db.Get().ChoreGroups.Research.Id, DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, "icon_errand_research", "icon_archetype_research"));
			this.Research.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Research.attribute
			};
			this.Research.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Research.Id
			};
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = base.Add(new SkillGroup("Rocketry", Db.Get().ChoreGroups.Rocketry.Id, DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, "icon_errand_rocketry", "icon_archetype_rocketry"));
				this.Rocketry.relevantAttributes = new List<Klei.AI.Attribute>
				{
					Db.Get().ChoreGroups.Rocketry.attribute
				};
				this.Rocketry.requiredChoreGroups = new List<string>
				{
					Db.Get().ChoreGroups.Rocketry.Id
				};
			}
			this.Suits = base.Add(new SkillGroup("Suits", "", DUPLICANTS.ROLES.GROUPS.SUITS, "suit_overlay_icon", "icon_archetype_astronaut"));
			this.Suits.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().Attributes.Athletics
			};
			this.Suits.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Hauling.Id
			};
			this.Hauling = base.Add(new SkillGroup("Hauling", Db.Get().ChoreGroups.Hauling.Id, DUPLICANTS.CHOREGROUPS.HAULING.NAME, "icon_errand_supply", "icon_archetype_storage"));
			this.Hauling.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Hauling.attribute
			};
			this.Hauling.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Hauling.Id
			};
			this.Technicals = base.Add(new SkillGroup("Technicals", Db.Get().ChoreGroups.MachineOperating.Id, DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, "icon_errand_operate", "icon_archetype_operate"));
			this.Technicals.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.MachineOperating.attribute
			};
			this.Technicals.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.MachineOperating.Id
			};
			this.MedicalAid = base.Add(new SkillGroup("MedicalAid", Db.Get().ChoreGroups.MedicalAid.Id, DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, "icon_errand_care", "icon_archetype_care"));
			this.MedicalAid.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.MedicalAid.attribute
			};
			this.Basekeeping = base.Add(new SkillGroup("Basekeeping", Db.Get().ChoreGroups.Basekeeping.Id, DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, "icon_errand_tidy", "icon_archetype_tidy"));
			this.Basekeeping.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Basekeeping.attribute
			};
			this.Basekeeping.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Basekeeping.Id
			};
		}

		// Token: 0x04004E5E RID: 20062
		public SkillGroup Mining;

		// Token: 0x04004E5F RID: 20063
		public SkillGroup Building;

		// Token: 0x04004E60 RID: 20064
		public SkillGroup Farming;

		// Token: 0x04004E61 RID: 20065
		public SkillGroup Ranching;

		// Token: 0x04004E62 RID: 20066
		public SkillGroup Cooking;

		// Token: 0x04004E63 RID: 20067
		public SkillGroup Art;

		// Token: 0x04004E64 RID: 20068
		public SkillGroup Research;

		// Token: 0x04004E65 RID: 20069
		public SkillGroup Rocketry;

		// Token: 0x04004E66 RID: 20070
		public SkillGroup Suits;

		// Token: 0x04004E67 RID: 20071
		public SkillGroup Hauling;

		// Token: 0x04004E68 RID: 20072
		public SkillGroup Technicals;

		// Token: 0x04004E69 RID: 20073
		public SkillGroup MedicalAid;

		// Token: 0x04004E6A RID: 20074
		public SkillGroup Basekeeping;
	}
}
