using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000D6A RID: 3434
	public class SkillPerks : ResourceSet<SkillPerk>
	{
		// Token: 0x06006B3B RID: 27451 RVA: 0x0029CF4C File Offset: 0x0029B14C
		public SkillPerks(ResourceSet parent) : base("SkillPerks", parent)
		{
			this.IncreaseDigSpeedSmall = base.Add(new SkillAttributePerk("IncreaseDigSpeedSmall", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MINER.NAME));
			this.IncreaseDigSpeedMedium = base.Add(new SkillAttributePerk("IncreaseDigSpeedMedium", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MINER.NAME));
			this.IncreaseDigSpeedLarge = base.Add(new SkillAttributePerk("IncreaseDigSpeedLarge", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MINER.NAME));
			this.CanDigVeryFirm = base.Add(new SimpleSkillPerk("CanDigVeryFirm", UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
			this.CanDigNearlyImpenetrable = base.Add(new SimpleSkillPerk("CanDigAbyssalite", UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
			this.CanDigSuperDuperHard = base.Add(new SimpleSkillPerk("CanDigDiamondAndObsidan", UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
			this.CanDigRadioactiveMaterials = base.Add(new SimpleSkillPerk("CanDigCorium", UI.ROLES_SCREEN.PERKS.CAN_DIG_RADIOACTIVE_MATERIALS.DESCRIPTION));
			this.CanDigUnobtanium = base.Add(new SimpleSkillPerk("CanDigUnobtanium", UI.ROLES_SCREEN.PERKS.CAN_DIG_UNOBTANIUM.DESCRIPTION));
			this.IncreaseConstructionSmall = base.Add(new SkillAttributePerk("IncreaseConstructionSmall", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME));
			this.IncreaseConstructionMedium = base.Add(new SkillAttributePerk("IncreaseConstructionMedium", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.BUILDER.NAME));
			this.IncreaseConstructionLarge = base.Add(new SkillAttributePerk("IncreaseConstructionLarge", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_BUILDER.NAME));
			this.IncreaseConstructionMechatronics = base.Add(new SkillAttributePerk("IncreaseConstructionMechatronics", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.CanDemolish = base.Add(new SimpleSkillPerk("CanDemonlish", UI.ROLES_SCREEN.PERKS.CAN_DEMOLISH.DESCRIPTION));
			this.IncreaseLearningSmall = base.Add(new SkillAttributePerk("IncreaseLearningSmall", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME));
			this.IncreaseLearningMedium = base.Add(new SkillAttributePerk("IncreaseLearningMedium", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.RESEARCHER.NAME));
			this.IncreaseLearningLarge = base.Add(new SkillAttributePerk("IncreaseLearningLarge", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME));
			this.IncreaseLearningLargeSpace = base.Add(new SkillAttributePerk("IncreaseLearningLargeSpace", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME));
			this.IncreaseBotanySmall = base.Add(new SkillAttributePerk("IncreaseBotanySmall", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_FARMER.NAME));
			this.IncreaseBotanyMedium = base.Add(new SkillAttributePerk("IncreaseBotanyMedium", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.FARMER.NAME));
			this.IncreaseBotanyLarge = base.Add(new SkillAttributePerk("IncreaseBotanyLarge", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_FARMER.NAME));
			this.CanFarmTinker = base.Add(new SimpleSkillPerk("CanFarmTinker", UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
			this.CanIdentifyMutantSeeds = base.Add(new SimpleSkillPerk("CanIdentifyMutantSeeds", UI.ROLES_SCREEN.PERKS.CAN_IDENTIFY_MUTANT_SEEDS.DESCRIPTION));
			this.IncreaseRanchingSmall = base.Add(new SkillAttributePerk("IncreaseRanchingSmall", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.RANCHER.NAME));
			this.IncreaseRanchingMedium = base.Add(new SkillAttributePerk("IncreaseRanchingMedium", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SENIOR_RANCHER.NAME));
			this.CanWrangleCreatures = base.Add(new SimpleSkillPerk("CanWrangleCreatures", UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
			this.CanUseRanchStation = base.Add(new SimpleSkillPerk("CanUseRanchStation", UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
			this.CanUseMilkingStation = base.Add(new SimpleSkillPerk("CanUseMilkingStation", UI.ROLES_SCREEN.PERKS.CAN_USE_MILKING_STATION.DESCRIPTION));
			this.IncreaseAthleticsSmall = base.Add(new SkillAttributePerk("IncreaseAthleticsSmall", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseAthleticsMedium = base.Add(new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_EXPERT.NAME));
			this.IncreaseAthleticsLarge = base.Add(new SkillAttributePerk("IncreaseAthleticsLarge", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_DURABILITY.NAME));
			this.IncreaseStrengthGofer = base.Add(new SkillAttributePerk("IncreaseStrengthGofer", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseStrengthCourier = base.Add(new SkillAttributePerk("IncreaseStrengthCourier", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseStrengthGroundskeeper = base.Add(new SkillAttributePerk("IncreaseStrengthGroundskeeper", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HANDYMAN.NAME));
			this.IncreaseStrengthPlumber = base.Add(new SkillAttributePerk("IncreaseStrengthPlumber", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.PLUMBER.NAME));
			this.IncreaseCarryAmountSmall = base.Add(new SkillAttributePerk("IncreaseCarryAmountSmall", Db.Get().Attributes.CarryAmount.Id, 400f, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseCarryAmountMedium = base.Add(new SkillAttributePerk("IncreaseCarryAmountMedium", Db.Get().Attributes.CarryAmount.Id, 800f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseArtSmall = base.Add(new SkillAttributePerk("IncreaseArtSmall", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME));
			this.IncreaseArtMedium = base.Add(new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.ARTIST.NAME));
			this.IncreaseArtLarge = base.Add(new SkillAttributePerk("IncreaseArtLarge", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MASTER_ARTIST.NAME));
			this.CanArt = base.Add(new SimpleSkillPerk("CanArt", UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
			this.CanArtUgly = base.Add(new SimpleSkillPerk("CanArtUgly", UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
			this.CanArtOkay = base.Add(new SimpleSkillPerk("CanArtOkay", UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
			this.CanArtGreat = base.Add(new SimpleSkillPerk("CanArtGreat", UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
			this.CanStudyArtifact = base.Add(new SimpleSkillPerk("CanStudyArtifact", UI.ROLES_SCREEN.PERKS.CAN_STUDY_ARTIFACTS.DESCRIPTION));
			this.CanClothingAlteration = base.Add(new SimpleSkillPerk("CanClothingAlteration", UI.ROLES_SCREEN.PERKS.CAN_CLOTHING_ALTERATION.DESCRIPTION));
			this.IncreaseMachinerySmall = base.Add(new SkillAttributePerk("IncreaseMachinerySmall", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME));
			this.IncreaseMachineryMedium = base.Add(new SkillAttributePerk("IncreaseMachineryMedium", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME));
			this.IncreaseMachineryLarge = base.Add(new SkillAttributePerk("IncreaseMachineryLarge", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.ConveyorBuild = base.Add(new SimpleSkillPerk("ConveyorBuild", UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
			this.CanPowerTinker = base.Add(new SimpleSkillPerk("CanPowerTinker", UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
			this.CanMakeMissiles = base.Add(new SimpleSkillPerk("CanMakeMissiles", UI.ROLES_SCREEN.PERKS.CAN_MAKE_MISSILES.DESCRIPTION));
			this.CanElectricGrill = base.Add(new SimpleSkillPerk("CanElectricGrill", UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
			this.IncreaseCookingSmall = base.Add(new SkillAttributePerk("IncreaseCookingSmall", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_COOK.NAME));
			this.IncreaseCookingMedium = base.Add(new SkillAttributePerk("IncreaseCookingMedium", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.COOK.NAME));
			this.CanSpiceGrinder = base.Add(new SimpleSkillPerk("CanSpiceGrinder ", UI.ROLES_SCREEN.PERKS.CAN_SPICE_GRINDER.DESCRIPTION));
			this.IncreaseCaringSmall = base.Add(new SkillAttributePerk("IncreaseCaringSmall", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME));
			this.IncreaseCaringMedium = base.Add(new SkillAttributePerk("IncreaseCaringMedium", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MEDIC.NAME));
			this.IncreaseCaringLarge = base.Add(new SkillAttributePerk("IncreaseCaringLarge", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MEDIC.NAME));
			this.CanCompound = base.Add(new SimpleSkillPerk("CanCompound", UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
			this.CanDoctor = base.Add(new SimpleSkillPerk("CanDoctor", UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
			this.CanAdvancedMedicine = base.Add(new SimpleSkillPerk("CanAdvancedMedicine", UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
			this.ExosuitExpertise = base.Add(new SimpleSkillPerk("ExosuitExpertise", UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
			this.ExosuitDurability = base.Add(new SimpleSkillPerk("ExosuitDurability", UI.ROLES_SCREEN.PERKS.EXOSUIT_DURABILITY.DESCRIPTION));
			this.AllowAdvancedResearch = base.Add(new SimpleSkillPerk("AllowAdvancedResearch", UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
			this.AllowInterstellarResearch = base.Add(new SimpleSkillPerk("AllowInterStellarResearch", UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
			this.AllowNuclearResearch = base.Add(new SimpleSkillPerk("AllowNuclearResearch", UI.ROLES_SCREEN.PERKS.NUCLEAR_RESEARCH.DESCRIPTION));
			this.AllowOrbitalResearch = base.Add(new SimpleSkillPerk("AllowOrbitalResearch", UI.ROLES_SCREEN.PERKS.ORBITAL_RESEARCH.DESCRIPTION));
			this.AllowGeyserTuning = base.Add(new SimpleSkillPerk("AllowGeyserTuning", UI.ROLES_SCREEN.PERKS.GEYSER_TUNING.DESCRIPTION));
			this.CanStudyWorldObjects = base.Add(new SimpleSkillPerk("CanStudyWorldObjects", UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
			this.CanUseClusterTelescope = base.Add(new SimpleSkillPerk("CanUseClusterTelescope", UI.ROLES_SCREEN.PERKS.CAN_USE_CLUSTER_TELESCOPE.DESCRIPTION));
			this.CanDoPlumbing = base.Add(new SimpleSkillPerk("CanDoPlumbing", UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
			this.CanUseRockets = base.Add(new SimpleSkillPerk("CanUseRockets", UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
			this.FasterSpaceFlight = base.Add(new SkillAttributePerk("FasterSpaceFlight", Db.Get().Attributes.SpaceNavigation.Id, 0.1f, DUPLICANTS.ROLES.ASTRONAUT.NAME));
			this.CanTrainToBeAstronaut = base.Add(new SimpleSkillPerk("CanTrainToBeAstronaut", UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
			this.CanMissionControl = base.Add(new SimpleSkillPerk("CanMissionControl", UI.ROLES_SCREEN.PERKS.CAN_MISSION_CONTROL.DESCRIPTION));
			this.CanUseRocketControlStation = base.Add(new SimpleSkillPerk("CanUseRocketControlStation", UI.ROLES_SCREEN.PERKS.CAN_PILOT_ROCKET.DESCRIPTION));
			this.IncreaseRocketSpeedSmall = base.Add(new SkillAttributePerk("IncreaseRocketSpeedSmall", Db.Get().Attributes.SpaceNavigation.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.ROCKETPILOT.NAME));
		}

		// Token: 0x04004E0A RID: 19978
		public SkillPerk IncreaseDigSpeedSmall;

		// Token: 0x04004E0B RID: 19979
		public SkillPerk IncreaseDigSpeedMedium;

		// Token: 0x04004E0C RID: 19980
		public SkillPerk IncreaseDigSpeedLarge;

		// Token: 0x04004E0D RID: 19981
		public SkillPerk CanDigVeryFirm;

		// Token: 0x04004E0E RID: 19982
		public SkillPerk CanDigNearlyImpenetrable;

		// Token: 0x04004E0F RID: 19983
		public SkillPerk CanDigSuperDuperHard;

		// Token: 0x04004E10 RID: 19984
		public SkillPerk CanDigRadioactiveMaterials;

		// Token: 0x04004E11 RID: 19985
		public SkillPerk CanDigUnobtanium;

		// Token: 0x04004E12 RID: 19986
		public SkillPerk IncreaseConstructionSmall;

		// Token: 0x04004E13 RID: 19987
		public SkillPerk IncreaseConstructionMedium;

		// Token: 0x04004E14 RID: 19988
		public SkillPerk IncreaseConstructionLarge;

		// Token: 0x04004E15 RID: 19989
		public SkillPerk IncreaseConstructionMechatronics;

		// Token: 0x04004E16 RID: 19990
		public SkillPerk CanDemolish;

		// Token: 0x04004E17 RID: 19991
		public SkillPerk IncreaseLearningSmall;

		// Token: 0x04004E18 RID: 19992
		public SkillPerk IncreaseLearningMedium;

		// Token: 0x04004E19 RID: 19993
		public SkillPerk IncreaseLearningLarge;

		// Token: 0x04004E1A RID: 19994
		public SkillPerk IncreaseLearningLargeSpace;

		// Token: 0x04004E1B RID: 19995
		public SkillPerk IncreaseBotanySmall;

		// Token: 0x04004E1C RID: 19996
		public SkillPerk IncreaseBotanyMedium;

		// Token: 0x04004E1D RID: 19997
		public SkillPerk IncreaseBotanyLarge;

		// Token: 0x04004E1E RID: 19998
		public SkillPerk CanFarmTinker;

		// Token: 0x04004E1F RID: 19999
		public SkillPerk CanIdentifyMutantSeeds;

		// Token: 0x04004E20 RID: 20000
		public SkillPerk CanWrangleCreatures;

		// Token: 0x04004E21 RID: 20001
		public SkillPerk CanUseRanchStation;

		// Token: 0x04004E22 RID: 20002
		public SkillPerk CanUseMilkingStation;

		// Token: 0x04004E23 RID: 20003
		public SkillPerk IncreaseRanchingSmall;

		// Token: 0x04004E24 RID: 20004
		public SkillPerk IncreaseRanchingMedium;

		// Token: 0x04004E25 RID: 20005
		public SkillPerk IncreaseAthleticsSmall;

		// Token: 0x04004E26 RID: 20006
		public SkillPerk IncreaseAthleticsMedium;

		// Token: 0x04004E27 RID: 20007
		public SkillPerk IncreaseAthleticsLarge;

		// Token: 0x04004E28 RID: 20008
		public SkillPerk IncreaseStrengthSmall;

		// Token: 0x04004E29 RID: 20009
		public SkillPerk IncreaseStrengthMedium;

		// Token: 0x04004E2A RID: 20010
		public SkillPerk IncreaseStrengthGofer;

		// Token: 0x04004E2B RID: 20011
		public SkillPerk IncreaseStrengthCourier;

		// Token: 0x04004E2C RID: 20012
		public SkillPerk IncreaseStrengthGroundskeeper;

		// Token: 0x04004E2D RID: 20013
		public SkillPerk IncreaseStrengthPlumber;

		// Token: 0x04004E2E RID: 20014
		public SkillPerk IncreaseCarryAmountSmall;

		// Token: 0x04004E2F RID: 20015
		public SkillPerk IncreaseCarryAmountMedium;

		// Token: 0x04004E30 RID: 20016
		public SkillPerk IncreaseArtSmall;

		// Token: 0x04004E31 RID: 20017
		public SkillPerk IncreaseArtMedium;

		// Token: 0x04004E32 RID: 20018
		public SkillPerk IncreaseArtLarge;

		// Token: 0x04004E33 RID: 20019
		public SkillPerk CanArt;

		// Token: 0x04004E34 RID: 20020
		public SkillPerk CanArtUgly;

		// Token: 0x04004E35 RID: 20021
		public SkillPerk CanArtOkay;

		// Token: 0x04004E36 RID: 20022
		public SkillPerk CanArtGreat;

		// Token: 0x04004E37 RID: 20023
		public SkillPerk CanStudyArtifact;

		// Token: 0x04004E38 RID: 20024
		public SkillPerk CanClothingAlteration;

		// Token: 0x04004E39 RID: 20025
		public SkillPerk IncreaseMachinerySmall;

		// Token: 0x04004E3A RID: 20026
		public SkillPerk IncreaseMachineryMedium;

		// Token: 0x04004E3B RID: 20027
		public SkillPerk IncreaseMachineryLarge;

		// Token: 0x04004E3C RID: 20028
		public SkillPerk ConveyorBuild;

		// Token: 0x04004E3D RID: 20029
		public SkillPerk CanMakeMissiles;

		// Token: 0x04004E3E RID: 20030
		public SkillPerk CanPowerTinker;

		// Token: 0x04004E3F RID: 20031
		public SkillPerk CanElectricGrill;

		// Token: 0x04004E40 RID: 20032
		public SkillPerk IncreaseCookingSmall;

		// Token: 0x04004E41 RID: 20033
		public SkillPerk IncreaseCookingMedium;

		// Token: 0x04004E42 RID: 20034
		public SkillPerk CanSpiceGrinder;

		// Token: 0x04004E43 RID: 20035
		public SkillPerk IncreaseCaringSmall;

		// Token: 0x04004E44 RID: 20036
		public SkillPerk IncreaseCaringMedium;

		// Token: 0x04004E45 RID: 20037
		public SkillPerk IncreaseCaringLarge;

		// Token: 0x04004E46 RID: 20038
		public SkillPerk CanCompound;

		// Token: 0x04004E47 RID: 20039
		public SkillPerk CanDoctor;

		// Token: 0x04004E48 RID: 20040
		public SkillPerk CanAdvancedMedicine;

		// Token: 0x04004E49 RID: 20041
		public SkillPerk ExosuitExpertise;

		// Token: 0x04004E4A RID: 20042
		public SkillPerk ExosuitDurability;

		// Token: 0x04004E4B RID: 20043
		public SkillPerk AllowAdvancedResearch;

		// Token: 0x04004E4C RID: 20044
		public SkillPerk AllowInterstellarResearch;

		// Token: 0x04004E4D RID: 20045
		public SkillPerk AllowNuclearResearch;

		// Token: 0x04004E4E RID: 20046
		public SkillPerk AllowOrbitalResearch;

		// Token: 0x04004E4F RID: 20047
		public SkillPerk AllowGeyserTuning;

		// Token: 0x04004E50 RID: 20048
		public SkillPerk CanStudyWorldObjects;

		// Token: 0x04004E51 RID: 20049
		public SkillPerk CanUseClusterTelescope;

		// Token: 0x04004E52 RID: 20050
		public SkillPerk IncreaseRocketSpeedSmall;

		// Token: 0x04004E53 RID: 20051
		public SkillPerk CanMissionControl;

		// Token: 0x04004E54 RID: 20052
		public SkillPerk CanDoPlumbing;

		// Token: 0x04004E55 RID: 20053
		public SkillPerk CanUseRockets;

		// Token: 0x04004E56 RID: 20054
		public SkillPerk FasterSpaceFlight;

		// Token: 0x04004E57 RID: 20055
		public SkillPerk CanTrainToBeAstronaut;

		// Token: 0x04004E58 RID: 20056
		public SkillPerk CanUseRocketControlStation;
	}
}
