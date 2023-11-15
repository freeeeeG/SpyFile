using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class SpiceVineConfig : IEntityConfig
{
	// Token: 0x060006B9 RID: 1721 RVA: 0x0002BF9D File Offset: 0x0002A19D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0002BFA4 File Offset: 0x0002A1A4
	public GameObject CreatePrefab()
	{
		string id = "SpiceVine";
		string name = STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vinespicenut_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, tier, default(EffectorValues), SimHashes.Creature, new List<Tag>
		{
			GameTags.Hanging
		}, 320f);
		EntityTemplates.MakeHangingOffsets(gameObject, 1, 3);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 308.15f, 358.15f, 448.15f, null, true, 0f, 0.15f, SpiceNutConfig.ID, true, true, true, true, 2400f, 0f, 9800f, "SpiceVineOriginal", STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.058333334f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Phosphorite,
				massConsumptionRate = 0.0016666667f
			}
		});
		gameObject.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		gameObject.AddOrGet<StandardCropPlant>();
		EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(gameObject, SeedProducer.ProductionType.Harvest, "SpiceVineSeed", STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME, STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC, Assets.GetAnim("seed_spicenut_kanim"), "object", 1, new List<Tag>
		{
			GameTags.CropSeed
		}, SingleEntityReceptacle.ReceptacleDirection.Bottom, default(Tag), 4, STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "SpiceVine_preview", Assets.GetAnim("vinespicenut_kanim"), "place", 1, 3), 1, 3);
		return gameObject;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002C1A2 File Offset: 0x0002A3A2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0002C1A4 File Offset: 0x0002A3A4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004B7 RID: 1207
	public const string ID = "SpiceVine";

	// Token: 0x040004B8 RID: 1208
	public const string SEED_ID = "SpiceVineSeed";

	// Token: 0x040004B9 RID: 1209
	public const float FERTILIZATION_RATE = 0.0016666667f;

	// Token: 0x040004BA RID: 1210
	public const float WATER_RATE = 0.058333334f;
}
