using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D6E RID: 3438
	public class Skills : ResourceSet<Skill>
	{
		// Token: 0x06006B43 RID: 27459 RVA: 0x0029E570 File Offset: 0x0029C770
		public Skills(ResourceSet parent) : base("Skills", parent)
		{
			this.Mining1 = this.AddSkill(new Skill("Mining1", DUPLICANTS.ROLES.JUNIOR_MINER.NAME, DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION, "", 0, "hat_role_mining1", "skillbadge_role_mining1", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedSmall,
				Db.Get().SkillPerks.CanDigVeryFirm
			}, null));
			this.Mining2 = this.AddSkill(new Skill("Mining2", DUPLICANTS.ROLES.MINER.NAME, DUPLICANTS.ROLES.MINER.DESCRIPTION, "", 1, "hat_role_mining2", "skillbadge_role_mining2", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedMedium,
				Db.Get().SkillPerks.CanDigNearlyImpenetrable
			}, new List<string>
			{
				this.Mining1.Id
			}));
			this.Mining3 = this.AddSkill(new Skill("Mining3", DUPLICANTS.ROLES.SENIOR_MINER.NAME, DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION, "", 2, "hat_role_mining3", "skillbadge_role_mining3", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedLarge,
				Db.Get().SkillPerks.CanDigSuperDuperHard
			}, new List<string>
			{
				this.Mining2.Id
			}));
			this.Mining4 = this.AddSkill(new Skill("Mining4", DUPLICANTS.ROLES.MASTER_MINER.NAME, DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_mining4", "skillbadge_role_mining4", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDigRadioactiveMaterials
			}, new List<string>
			{
				this.Mining3.Id
			}));
			this.Building1 = this.AddSkill(new Skill("Building1", DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION, "", 0, "hat_role_building1", "skillbadge_role_building1", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionSmall
			}, null));
			this.Building2 = this.AddSkill(new Skill("Building2", DUPLICANTS.ROLES.BUILDER.NAME, DUPLICANTS.ROLES.BUILDER.DESCRIPTION, "", 1, "hat_role_building2", "skillbadge_role_building2", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionMedium
			}, new List<string>
			{
				this.Building1.Id
			}));
			this.Building3 = this.AddSkill(new Skill("Building3", DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION, "", 2, "hat_role_building3", "skillbadge_role_building3", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionLarge,
				Db.Get().SkillPerks.CanDemolish
			}, new List<string>
			{
				this.Building2.Id
			}));
			this.Farming1 = this.AddSkill(new Skill("Farming1", DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION, "", 0, "hat_role_farming1", "skillbadge_role_farming1", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanySmall
			}, null));
			this.Farming2 = this.AddSkill(new Skill("Farming2", DUPLICANTS.ROLES.FARMER.NAME, DUPLICANTS.ROLES.FARMER.DESCRIPTION, "", 1, "hat_role_farming2", "skillbadge_role_farming2", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyMedium,
				Db.Get().SkillPerks.CanFarmTinker
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Farming3 = this.AddSkill(new Skill("Farming3", DUPLICANTS.ROLES.SENIOR_FARMER.NAME, DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION, "", 2, "hat_role_farming3", "skillbadge_role_farming3", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyLarge
			}, new List<string>
			{
				this.Farming2.Id
			}));
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				this.Farming3.perks.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
			}
			this.Ranching1 = this.AddSkill(new Skill("Ranching1", DUPLICANTS.ROLES.RANCHER.NAME, DUPLICANTS.ROLES.RANCHER.DESCRIPTION, "", 1, "hat_role_rancher1", "skillbadge_role_rancher1", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanWrangleCreatures,
				Db.Get().SkillPerks.CanUseRanchStation,
				Db.Get().SkillPerks.IncreaseRanchingSmall
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Ranching2 = this.AddSkill(new Skill("Ranching2", DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION, "", 2, "hat_role_rancher2", "skillbadge_role_rancher2", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseMilkingStation,
				Db.Get().SkillPerks.IncreaseRanchingMedium
			}, new List<string>
			{
				this.Ranching1.Id
			}));
			this.Researching1 = this.AddSkill(new Skill("Researching1", DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION, "", 0, "hat_role_research1", "skillbadge_role_research1", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningSmall,
				Db.Get().SkillPerks.AllowAdvancedResearch
			}, null));
			this.Researching2 = this.AddSkill(new Skill("Researching2", DUPLICANTS.ROLES.RESEARCHER.NAME, DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION, "", 1, "hat_role_research2", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningMedium,
				Db.Get().SkillPerks.CanStudyWorldObjects,
				Db.Get().SkillPerks.AllowGeyserTuning
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.AtomicResearch = this.AddSkill(new Skill("AtomicResearch", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research5", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4 = this.AddSkill(new Skill("Researching4", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4.deprecated = true;
			this.Researching3 = this.AddSkill(new Skill("Researching3", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "", 2, "hat_role_research3", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowInterstellarResearch,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching3.deprecated = DlcManager.IsExpansion1Active();
			this.Astronomy = this.AddSkill(new Skill("Astronomy", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_research3", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseClusterTelescope,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.SpaceResearch = this.AddSkill(new Skill("SpaceResearch", DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, DUPLICANTS.ROLES.SPACE_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLargeSpace,
				Db.Get().SkillPerks.AllowOrbitalResearch
			}, new List<string>
			{
				this.Astronomy.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.RocketPiloting1 = this.AddSkill(new Skill("RocketPiloting1", DUPLICANTS.ROLES.ROCKETPILOT.NAME, DUPLICANTS.ROLES.ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 0, "hat_role_astronaut1", "skillbadge_role_rocketry1", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRocketControlStation
				}, new List<string>()));
				this.RocketPiloting2 = this.AddSkill(new Skill("RocketPiloting2", DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.NAME, DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_astronaut2", "skillbadge_role_rocketry3", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseRocketSpeedSmall
				}, new List<string>
				{
					this.RocketPiloting1.Id,
					this.Astronomy.Id
				}));
			}
			this.Cooking1 = this.AddSkill(new Skill("Cooking1", DUPLICANTS.ROLES.JUNIOR_COOK.NAME, DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION, "", 0, "hat_role_cooking1", "skillbadge_role_cooking1", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingSmall,
				Db.Get().SkillPerks.CanElectricGrill
			}, null));
			this.Cooking2 = this.AddSkill(new Skill("Cooking2", DUPLICANTS.ROLES.COOK.NAME, DUPLICANTS.ROLES.COOK.DESCRIPTION, "", 1, "hat_role_cooking2", "skillbadge_role_cooking2", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingMedium,
				Db.Get().SkillPerks.CanSpiceGrinder
			}, new List<string>
			{
				this.Cooking1.Id
			}));
			this.Arting1 = this.AddSkill(new Skill("Arting1", DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION, "", 0, "hat_role_art1", "skillbadge_role_art1", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArt,
				Db.Get().SkillPerks.CanArtUgly,
				Db.Get().SkillPerks.IncreaseArtSmall
			}, null));
			this.Arting2 = this.AddSkill(new Skill("Arting2", DUPLICANTS.ROLES.ARTIST.NAME, DUPLICANTS.ROLES.ARTIST.DESCRIPTION, "", 1, "hat_role_art2", "skillbadge_role_art2", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtOkay,
				Db.Get().SkillPerks.IncreaseArtMedium,
				Db.Get().SkillPerks.CanClothingAlteration
			}, new List<string>
			{
				this.Arting1.Id
			}));
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Arting2.perks.Add(Db.Get().SkillPerks.CanStudyArtifact);
			}
			this.Arting3 = this.AddSkill(new Skill("Arting3", DUPLICANTS.ROLES.MASTER_ARTIST.NAME, DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION, "", 2, "hat_role_art3", "skillbadge_role_art3", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtGreat,
				Db.Get().SkillPerks.IncreaseArtLarge
			}, new List<string>
			{
				this.Arting2.Id
			}));
			this.Hauling1 = this.AddSkill(new Skill("Hauling1", DUPLICANTS.ROLES.HAULER.NAME, DUPLICANTS.ROLES.HAULER.DESCRIPTION, "", 0, "hat_role_hauling1", "skillbadge_role_hauling1", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGofer,
				Db.Get().SkillPerks.IncreaseCarryAmountSmall
			}, null));
			this.Hauling2 = this.AddSkill(new Skill("Hauling2", DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION, "", 1, "hat_role_hauling2", "skillbadge_role_hauling2", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthCourier,
				Db.Get().SkillPerks.IncreaseCarryAmountMedium
			}, new List<string>
			{
				this.Hauling1.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id,
					this.RocketPiloting1.Id
				}));
			}
			else
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id
				}));
			}
			this.Suits1 = this.AddSkill(new Skill("Suits1", DUPLICANTS.ROLES.SUIT_EXPERT.NAME, DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION, "", 2, "hat_role_suits2", "skillbadge_role_suits3", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.ExosuitExpertise,
				Db.Get().SkillPerks.IncreaseAthleticsMedium
			}, new List<string>
			{
				this.ThermalSuits.Id
			}));
			this.Technicals1 = this.AddSkill(new Skill("Technicals1", DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION, "", 0, "hat_role_technicals1", "skillbadge_role_technicals1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachinerySmall
			}, null));
			this.Technicals2 = this.AddSkill(new Skill("Technicals2", DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION, "", 1, "hat_role_technicals2", "skillbadge_role_technicals2", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryMedium,
				Db.Get().SkillPerks.CanPowerTinker
			}, new List<string>
			{
				this.Technicals1.Id
			}));
			this.Engineering1 = this.AddSkill(new Skill("Engineering1", DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION, "", 2, "hat_role_engineering1", "skillbadge_role_engineering1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryLarge,
				Db.Get().SkillPerks.IncreaseConstructionMechatronics,
				Db.Get().SkillPerks.ConveyorBuild
			}, new List<string>
			{
				this.Hauling2.Id,
				this.Technicals2.Id
			}));
			this.Basekeeping1 = this.AddSkill(new Skill("Basekeeping1", DUPLICANTS.ROLES.HANDYMAN.NAME, DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION, "", 0, "hat_role_basekeeping1", "skillbadge_role_basekeeping1", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGroundskeeper
			}, null));
			this.Basekeeping2 = this.AddSkill(new Skill("Basekeeping2", DUPLICANTS.ROLES.PLUMBER.NAME, DUPLICANTS.ROLES.PLUMBER.DESCRIPTION, "", 1, "hat_role_basekeeping2", "skillbadge_role_basekeeping2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthPlumber,
				Db.Get().SkillPerks.CanDoPlumbing
			}, new List<string>
			{
				this.Basekeeping1.Id
			}));
			this.Pyrotechnics = this.AddSkill(new Skill("Pyrotechnics", DUPLICANTS.ROLES.PYROTECHNIC.NAME, DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION, "", 2, "hat_role_technicals2", "skillbadge_role_technicals2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanMakeMissiles
			}, new List<string>
			{
				this.Basekeeping2.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting1.deprecated = true;
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
				this.Astronauting2.deprecated = true;
			}
			else
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME, DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION, "", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRockets
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.ASTRONAUT.NAME, DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION, "", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.FasterSpaceFlight
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
			}
			this.Medicine1 = this.AddSkill(new Skill("Medicine1", DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION, "", 0, "hat_role_medicalaid1", "skillbadge_role_medicalaid1", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanCompound,
				Db.Get().SkillPerks.IncreaseCaringSmall
			}, null));
			this.Medicine2 = this.AddSkill(new Skill("Medicine2", DUPLICANTS.ROLES.MEDIC.NAME, DUPLICANTS.ROLES.MEDIC.DESCRIPTION, "", 1, "hat_role_medicalaid2", "skillbadge_role_medicalaid2", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDoctor,
				Db.Get().SkillPerks.IncreaseCaringMedium
			}, new List<string>
			{
				this.Medicine1.Id
			}));
			this.Medicine3 = this.AddSkill(new Skill("Medicine3", DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION, "", 2, "hat_role_medicalaid3", "skillbadge_role_medicalaid3", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanAdvancedMedicine,
				Db.Get().SkillPerks.IncreaseCaringLarge
			}, new List<string>
			{
				this.Medicine2.Id
			}));
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x0029FE3F File Offset: 0x0029E03F
		private Skill AddSkill(Skill skill)
		{
			if (DlcManager.IsContentActive(skill.dlcId))
			{
				return base.Add(skill);
			}
			return skill;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x0029FE58 File Offset: 0x0029E058
		public List<Skill> GetSkillsWithPerk(string perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x0029FEC0 File Offset: 0x0029E0C0
		public List<Skill> GetSkillsWithPerk(SkillPerk perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06006B47 RID: 27463 RVA: 0x0029FF24 File Offset: 0x0029E124
		public List<Skill> GetAllPriorSkills(Skill skill)
		{
			List<Skill> list = new List<Skill>();
			foreach (string id in skill.priorSkills)
			{
				Skill skill2 = base.Get(id);
				list.Add(skill2);
				list.AddRange(this.GetAllPriorSkills(skill2));
			}
			return list;
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x0029FF94 File Offset: 0x0029E194
		public List<Skill> GetTerminalSkills()
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				bool flag = true;
				using (List<Skill>.Enumerator enumerator2 = this.resources.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.priorSkills.Contains(skill.Id))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x04004E74 RID: 20084
		public Skill Mining1;

		// Token: 0x04004E75 RID: 20085
		public Skill Mining2;

		// Token: 0x04004E76 RID: 20086
		public Skill Mining3;

		// Token: 0x04004E77 RID: 20087
		public Skill Mining4;

		// Token: 0x04004E78 RID: 20088
		public Skill Building1;

		// Token: 0x04004E79 RID: 20089
		public Skill Building2;

		// Token: 0x04004E7A RID: 20090
		public Skill Building3;

		// Token: 0x04004E7B RID: 20091
		public Skill Farming1;

		// Token: 0x04004E7C RID: 20092
		public Skill Farming2;

		// Token: 0x04004E7D RID: 20093
		public Skill Farming3;

		// Token: 0x04004E7E RID: 20094
		public Skill Ranching1;

		// Token: 0x04004E7F RID: 20095
		public Skill Ranching2;

		// Token: 0x04004E80 RID: 20096
		public Skill Researching1;

		// Token: 0x04004E81 RID: 20097
		public Skill Researching2;

		// Token: 0x04004E82 RID: 20098
		public Skill Researching3;

		// Token: 0x04004E83 RID: 20099
		public Skill Researching4;

		// Token: 0x04004E84 RID: 20100
		public Skill AtomicResearch;

		// Token: 0x04004E85 RID: 20101
		public Skill SpaceResearch;

		// Token: 0x04004E86 RID: 20102
		public Skill Astronomy;

		// Token: 0x04004E87 RID: 20103
		public Skill RocketPiloting1;

		// Token: 0x04004E88 RID: 20104
		public Skill RocketPiloting2;

		// Token: 0x04004E89 RID: 20105
		public Skill Cooking1;

		// Token: 0x04004E8A RID: 20106
		public Skill Cooking2;

		// Token: 0x04004E8B RID: 20107
		public Skill Arting1;

		// Token: 0x04004E8C RID: 20108
		public Skill Arting2;

		// Token: 0x04004E8D RID: 20109
		public Skill Arting3;

		// Token: 0x04004E8E RID: 20110
		public Skill Hauling1;

		// Token: 0x04004E8F RID: 20111
		public Skill Hauling2;

		// Token: 0x04004E90 RID: 20112
		public Skill ThermalSuits;

		// Token: 0x04004E91 RID: 20113
		public Skill Suits1;

		// Token: 0x04004E92 RID: 20114
		public Skill Technicals1;

		// Token: 0x04004E93 RID: 20115
		public Skill Technicals2;

		// Token: 0x04004E94 RID: 20116
		public Skill Engineering1;

		// Token: 0x04004E95 RID: 20117
		public Skill Basekeeping1;

		// Token: 0x04004E96 RID: 20118
		public Skill Basekeeping2;

		// Token: 0x04004E97 RID: 20119
		public Skill Pyrotechnics;

		// Token: 0x04004E98 RID: 20120
		public Skill Astronauting1;

		// Token: 0x04004E99 RID: 20121
		public Skill Astronauting2;

		// Token: 0x04004E9A RID: 20122
		public Skill Medicine1;

		// Token: 0x04004E9B RID: 20123
		public Skill Medicine2;

		// Token: 0x04004E9C RID: 20124
		public Skill Medicine3;
	}
}
