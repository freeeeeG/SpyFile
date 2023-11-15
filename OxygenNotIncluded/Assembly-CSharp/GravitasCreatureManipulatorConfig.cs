﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class GravitasCreatureManipulatorConfig : IBuildingConfig
{
	// Token: 0x060008F0 RID: 2288 RVA: 0x00034A9C File Offset: 0x00032C9C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasCreatureManipulator";
		int width = 3;
		int height = 4;
		string anim = "gravitas_critter_manipulator_kanim";
		int hitpoints = 250;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00034B38 File Offset: 0x00032D38
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		BuildingTemplates.ExtendBuildingToGravitas(go);
		go.AddComponent<Storage>();
		Activatable activatable = go.AddComponent<Activatable>();
		activatable.synchronizeAnims = false;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		activatable.SetWorkTime(30f);
		GravitasCreatureManipulator.Def def = go.AddOrGetDef<GravitasCreatureManipulator.Def>();
		def.pickupOffset = new CellOffset(-1, 0);
		def.dropOffset = new CellOffset(1, 0);
		def.numSpeciesToUnlockMorphMode = 5;
		def.workingDuration = 15f;
		def.cooldownDuration = 540f;
		MakeBaseSolid.Def def2 = go.AddOrGetDef<MakeBaseSolid.Def>();
		def2.solidOffsets = new CellOffset[4];
		for (int i = 0; i < 4; i++)
		{
			def2.solidOffsets[i] = new CellOffset(0, i);
		}
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight);
		};
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00034C50 File Offset: 0x00032E50
	public static Option<string> GetBodyContentForSpeciesTag(Tag species)
	{
		Option<string> nameForSpeciesTag = GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species);
		Option<string> descriptionForSpeciesTag = GravitasCreatureManipulatorConfig.GetDescriptionForSpeciesTag(species);
		if (nameForSpeciesTag.HasValue && descriptionForSpeciesTag.HasValue)
		{
			return GravitasCreatureManipulatorConfig.GetBodyContent(nameForSpeciesTag.Value, descriptionForSpeciesTag.Value);
		}
		return Option.None;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00034CA0 File Offset: 0x00032EA0
	public static string GetBodyContentForUnknownSpecies()
	{
		return GravitasCreatureManipulatorConfig.GetBodyContent(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN_TITLE, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00034CBB File Offset: 0x00032EBB
	public static string GetBodyContent(string name, string desc)
	{
		return "<size=125%><b>" + name + "</b></size><line-height=150%>\n</line-height>" + desc;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00034CD0 File Offset: 0x00032ED0
	public static Option<string> GetNameForSpeciesTag(Tag species)
	{
		if (species == GameTags.Creatures.Species.HatchSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.HATCHSPECIES);
		}
		if (species == GameTags.Creatures.Species.LightBugSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);
		}
		if (species == GameTags.Creatures.Species.OilFloaterSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);
		}
		if (species == GameTags.Creatures.Species.DreckoSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.DRECKOSPECIES);
		}
		if (species == GameTags.Creatures.Species.GlomSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.GLOMSPECIES);
		}
		if (species == GameTags.Creatures.Species.PuftSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.PUFTSPECIES);
		}
		if (species == GameTags.Creatures.Species.PacuSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.PACUSPECIES);
		}
		if (species == GameTags.Creatures.Species.MooSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.MOOSPECIES);
		}
		if (species == GameTags.Creatures.Species.MoleSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.MOLESPECIES);
		}
		if (species == GameTags.Creatures.Species.SquirrelSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);
		}
		if (species == GameTags.Creatures.Species.CrabSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.CRABSPECIES);
		}
		if (species == GameTags.Creatures.Species.DivergentSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);
		}
		if (species == GameTags.Creatures.Species.StaterpillarSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);
		}
		if (species == GameTags.Creatures.Species.BeetaSpecies)
		{
			return Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.BEETASPECIES);
		}
		return Option.None;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00034E80 File Offset: 0x00033080
	public static Option<string> GetDescriptionForSpeciesTag(Tag species)
	{
		if (species == GameTags.Creatures.Species.HatchSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.HATCH);
		}
		if (species == GameTags.Creatures.Species.LightBugSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.LIGHTBUG);
		}
		if (species == GameTags.Creatures.Species.OilFloaterSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.OILFLOATER);
		}
		if (species == GameTags.Creatures.Species.DreckoSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.DRECKO);
		}
		if (species == GameTags.Creatures.Species.GlomSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.GLOM);
		}
		if (species == GameTags.Creatures.Species.PuftSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.PUFT);
		}
		if (species == GameTags.Creatures.Species.PacuSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.PACU);
		}
		if (species == GameTags.Creatures.Species.MooSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.MOO);
		}
		if (species == GameTags.Creatures.Species.MoleSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.MOLE);
		}
		if (species == GameTags.Creatures.Species.SquirrelSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.SQUIRREL);
		}
		if (species == GameTags.Creatures.Species.CrabSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.CRAB);
		}
		if (species == GameTags.Creatures.Species.DivergentSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.DIVERGENTSPECIES);
		}
		if (species == GameTags.Creatures.Species.StaterpillarSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.STATERPILLAR);
		}
		if (species == GameTags.Creatures.Species.BeetaSpecies)
		{
			return Option.Some<string>(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.BEETA);
		}
		return Option.None;
	}

	// Token: 0x0400059C RID: 1436
	public const string ID = "GravitasCreatureManipulator";

	// Token: 0x0400059D RID: 1437
	public const string CODEX_ENTRY_ID = "STORYTRAITCRITTERMANIPULATOR";

	// Token: 0x0400059E RID: 1438
	public const string INITIAL_LORE_UNLOCK_ID = "story_trait_critter_manipulator_initial";

	// Token: 0x0400059F RID: 1439
	public const string PARKING_LORE_UNLOCK_ID = "story_trait_critter_manipulator_parking";

	// Token: 0x040005A0 RID: 1440
	public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_critter_manipulator_complete";

	// Token: 0x040005A1 RID: 1441
	private const int HEIGHT = 4;

	// Token: 0x02000F4D RID: 3917
	public static class CRITTER_LORE_UNLOCK_ID
	{
		// Token: 0x06007187 RID: 29063 RVA: 0x002BDA0B File Offset: 0x002BBC0B
		public static string For(Tag species)
		{
			return "story_trait_critter_manipulator_" + species.ToString().ToLower();
		}
	}
}
