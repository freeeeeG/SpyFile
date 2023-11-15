using System;
using System.Collections.Generic;
using Klei;
using TUNING;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class GeyserGenericConfig : IMultiEntityConfig
{
	// Token: 0x06000673 RID: 1651 RVA: 0x00029DAC File Offset: 0x00027FAC
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		List<GeyserGenericConfig.GeyserPrefabParams> configs = this.GenerateConfigs();
		foreach (GeyserGenericConfig.GeyserPrefabParams geyserPrefabParams in configs)
		{
			list.Add(GeyserGenericConfig.CreateGeyser(geyserPrefabParams.id, geyserPrefabParams.anim, geyserPrefabParams.width, geyserPrefabParams.height, Strings.Get(geyserPrefabParams.nameStringKey), Strings.Get(geyserPrefabParams.descStringKey), geyserPrefabParams.geyserType.idHash, geyserPrefabParams.geyserType.geyserTemperature));
		}
		configs.RemoveAll((GeyserGenericConfig.GeyserPrefabParams x) => !x.isGenericGeyser);
		GameObject gameObject = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			int num = 0;
			if (SaveLoader.Instance.clusterDetailSave != null)
			{
				num = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
			}
			else
			{
				global::Debug.LogWarning("Could not load global world seed for geysers");
			}
			num = num + (int)inst.transform.GetPosition().x + (int)inst.transform.GetPosition().y;
			int index = new KRandom(num).Next(0, configs.Count);
			GameUtil.KInstantiate(Assets.GetPrefab(configs[index].id), inst.transform.GetPosition(), Grid.SceneLayer.BuildingBack, null, 0).SetActive(true);
			inst.DeleteObject();
		};
		list.Add(gameObject);
		return list;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00029ED4 File Offset: 0x000280D4
	public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, float geyserTemperature)
	{
		float mass = 2000f;
		EffectorValues tier = BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim), "inactive", Grid.SceneLayer.BuildingBack, width, height, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.GeyserFeature
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Katairite, true);
		component.Temperature = geyserTemperature;
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uncoverable>();
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<GeyserConfigurator>().presetType = presetType;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00029FE3 File Offset: 0x000281E3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00029FE5 File Offset: 0x000281E5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00029FE8 File Offset: 0x000281E8
	private List<GeyserGenericConfig.GeyserPrefabParams> GenerateConfigs()
	{
		List<GeyserGenericConfig.GeyserPrefabParams> list = new List<GeyserGenericConfig.GeyserPrefabParams>();
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim", 2, 4, new GeyserConfigurator.GeyserType("steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 383.15f, 1000f, 2000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 773.15f, 500f, 1000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim", 4, 2, new GeyserConfigurator.GeyserType("hot_water", SimHashes.Water, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim", 4, 2, new GeyserConfigurator.GeyserType("filthy_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 303.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("FoodPoisoning"),
			count = 20000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_cool_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_salt_water", SimHashes.Brine, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 4, 2, new GeyserConfigurator.GeyserType("salt_water", SimHashes.SaltWater, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim", 3, 3, new GeyserConfigurator.GeyserType("small_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 400f, 800f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim", 3, 3, new GeyserConfigurator.GeyserType("big_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_co2_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_co2", SimHashes.LiquidCarbonDioxide, GeyserConfigurator.GeyserShape.Liquid, 218f, 100f, 200f, 50f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 218f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_co2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_co2", SimHashes.CarbonDioxide, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_hydrogen", SimHashes.Hydrogen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_slimy_kanim", 2, 4, new GeyserConfigurator.GeyserType("slimy_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("SlimeLung"),
			count = 5000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas", SimHashes.ChlorineGas, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim", 2, 4, new GeyserConfigurator.GeyserType("methane", SimHashes.Methane, GeyserConfigurator.GeyserShape.Gas, 423.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_copper", SimHashes.MoltenCopper, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_iron", SimHashes.MoltenIron, GeyserConfigurator.GeyserShape.Molten, 2800f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_gold", SimHashes.MoltenGold, GeyserConfigurator.GeyserShape.Molten, 2900f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_aluminum_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_aluminum", SimHashes.MoltenAluminum, GeyserConfigurator.GeyserShape.Molten, 2000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_tungsten_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_tungsten", SimHashes.MoltenTungsten, GeyserConfigurator.GeyserShape.Molten, 4000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_niobium_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_cobalt_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_cobalt", SimHashes.MoltenCobalt, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("oil_drip", SimHashes.CrudeOil, GeyserConfigurator.GeyserShape.Liquid, 600f, 1f, 250f, 50f, 600f, 600f, 1f, 1f, 100f, 500f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_sulfur_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_sulfur", SimHashes.LiquidSulfur, GeyserConfigurator.GeyserShape.Liquid, 438.34998f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.RemoveAll((GeyserGenericConfig.GeyserPrefabParams geyser) => !geyser.geyserType.DlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentActive(geyser.geyserType.DlcID));
		return list;
	}

	// Token: 0x04000474 RID: 1140
	public const string ID = "GeyserGeneric";

	// Token: 0x04000475 RID: 1141
	public const string Steam = "steam";

	// Token: 0x04000476 RID: 1142
	public const string HotSteam = "hot_steam";

	// Token: 0x04000477 RID: 1143
	public const string HotWater = "hot_water";

	// Token: 0x04000478 RID: 1144
	public const string SlushWater = "slush_water";

	// Token: 0x04000479 RID: 1145
	public const string FilthyWater = "filthy_water";

	// Token: 0x0400047A RID: 1146
	public const string SlushSaltWater = "slush_salt_water";

	// Token: 0x0400047B RID: 1147
	public const string SaltWater = "salt_water";

	// Token: 0x0400047C RID: 1148
	public const string SmallVolcano = "small_volcano";

	// Token: 0x0400047D RID: 1149
	public const string BigVolcano = "big_volcano";

	// Token: 0x0400047E RID: 1150
	public const string LiquidCO2 = "liquid_co2";

	// Token: 0x0400047F RID: 1151
	public const string HotCO2 = "hot_co2";

	// Token: 0x04000480 RID: 1152
	public const string HotHydrogen = "hot_hydrogen";

	// Token: 0x04000481 RID: 1153
	public const string HotPO2 = "hot_po2";

	// Token: 0x04000482 RID: 1154
	public const string SlimyPO2 = "slimy_po2";

	// Token: 0x04000483 RID: 1155
	public const string ChlorineGas = "chlorine_gas";

	// Token: 0x04000484 RID: 1156
	public const string Methane = "methane";

	// Token: 0x04000485 RID: 1157
	public const string MoltenCopper = "molten_copper";

	// Token: 0x04000486 RID: 1158
	public const string MoltenIron = "molten_iron";

	// Token: 0x04000487 RID: 1159
	public const string MoltenGold = "molten_gold";

	// Token: 0x04000488 RID: 1160
	public const string MoltenAluminum = "molten_aluminum";

	// Token: 0x04000489 RID: 1161
	public const string MoltenTungsten = "molten_tungsten";

	// Token: 0x0400048A RID: 1162
	public const string MoltenNiobium = "molten_niobium";

	// Token: 0x0400048B RID: 1163
	public const string MoltenCobalt = "molten_cobalt";

	// Token: 0x0400048C RID: 1164
	public const string OilDrip = "oil_drip";

	// Token: 0x0400048D RID: 1165
	public const string LiquidSulfur = "liquid_sulfur";

	// Token: 0x02000F37 RID: 3895
	public struct GeyserPrefabParams
	{
		// Token: 0x0600714D RID: 29005 RVA: 0x002BCF60 File Offset: 0x002BB160
		public GeyserPrefabParams(string anim, int width, int height, GeyserConfigurator.GeyserType geyserType, bool isGenericGeyser)
		{
			this.id = "GeyserGeneric_" + geyserType.id;
			this.anim = anim;
			this.width = width;
			this.height = height;
			this.nameStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".DESC");
			this.geyserType = geyserType;
			this.isGenericGeyser = isGenericGeyser;
		}

		// Token: 0x04005546 RID: 21830
		public string id;

		// Token: 0x04005547 RID: 21831
		public string anim;

		// Token: 0x04005548 RID: 21832
		public int width;

		// Token: 0x04005549 RID: 21833
		public int height;

		// Token: 0x0400554A RID: 21834
		public StringKey nameStringKey;

		// Token: 0x0400554B RID: 21835
		public StringKey descStringKey;

		// Token: 0x0400554C RID: 21836
		public GeyserConfigurator.GeyserType geyserType;

		// Token: 0x0400554D RID: 21837
		public bool isGenericGeyser;
	}

	// Token: 0x02000F38 RID: 3896
	private static class TEMPERATURES
	{
		// Token: 0x0400554E RID: 21838
		public const float BELOW_FREEZING = 263.15f;

		// Token: 0x0400554F RID: 21839
		public const float DUPE_NORMAL = 303.15f;

		// Token: 0x04005550 RID: 21840
		public const float DUPE_HOT = 333.15f;

		// Token: 0x04005551 RID: 21841
		public const float BELOW_BOILING = 368.15f;

		// Token: 0x04005552 RID: 21842
		public const float ABOVE_BOILING = 383.15f;

		// Token: 0x04005553 RID: 21843
		public const float HOT1 = 423.15f;

		// Token: 0x04005554 RID: 21844
		public const float HOT2 = 773.15f;

		// Token: 0x04005555 RID: 21845
		public const float MOLTEN_MAGMA = 2000f;
	}

	// Token: 0x02000F39 RID: 3897
	public static class RATES
	{
		// Token: 0x04005556 RID: 21846
		public const float GAS_SMALL_MIN = 40f;

		// Token: 0x04005557 RID: 21847
		public const float GAS_SMALL_MAX = 80f;

		// Token: 0x04005558 RID: 21848
		public const float GAS_NORMAL_MIN = 70f;

		// Token: 0x04005559 RID: 21849
		public const float GAS_NORMAL_MAX = 140f;

		// Token: 0x0400555A RID: 21850
		public const float GAS_BIG_MIN = 100f;

		// Token: 0x0400555B RID: 21851
		public const float GAS_BIG_MAX = 200f;

		// Token: 0x0400555C RID: 21852
		public const float LIQUID_SMALL_MIN = 500f;

		// Token: 0x0400555D RID: 21853
		public const float LIQUID_SMALL_MAX = 1000f;

		// Token: 0x0400555E RID: 21854
		public const float LIQUID_NORMAL_MIN = 1000f;

		// Token: 0x0400555F RID: 21855
		public const float LIQUID_NORMAL_MAX = 2000f;

		// Token: 0x04005560 RID: 21856
		public const float LIQUID_BIG_MIN = 2000f;

		// Token: 0x04005561 RID: 21857
		public const float LIQUID_BIG_MAX = 4000f;

		// Token: 0x04005562 RID: 21858
		public const float MOLTEN_NORMAL_MIN = 200f;

		// Token: 0x04005563 RID: 21859
		public const float MOLTEN_NORMAL_MAX = 400f;

		// Token: 0x04005564 RID: 21860
		public const float MOLTEN_BIG_MIN = 400f;

		// Token: 0x04005565 RID: 21861
		public const float MOLTEN_BIG_MAX = 800f;

		// Token: 0x04005566 RID: 21862
		public const float MOLTEN_HUGE_MIN = 800f;

		// Token: 0x04005567 RID: 21863
		public const float MOLTEN_HUGE_MAX = 1600f;
	}

	// Token: 0x02000F3A RID: 3898
	public static class MAX_PRESSURES
	{
		// Token: 0x04005568 RID: 21864
		public const float GAS = 5f;

		// Token: 0x04005569 RID: 21865
		public const float GAS_HIGH = 15f;

		// Token: 0x0400556A RID: 21866
		public const float MOLTEN = 150f;

		// Token: 0x0400556B RID: 21867
		public const float LIQUID_SMALL = 50f;

		// Token: 0x0400556C RID: 21868
		public const float LIQUID = 500f;
	}

	// Token: 0x02000F3B RID: 3899
	public static class ITERATIONS
	{
		// Token: 0x02001FAF RID: 8111
		public static class INFREQUENT_MOLTEN
		{
			// Token: 0x04008EC4 RID: 36548
			public const float PCT_MIN = 0.005f;

			// Token: 0x04008EC5 RID: 36549
			public const float PCT_MAX = 0.01f;

			// Token: 0x04008EC6 RID: 36550
			public const float LEN_MIN = 6000f;

			// Token: 0x04008EC7 RID: 36551
			public const float LEN_MAX = 12000f;
		}

		// Token: 0x02001FB0 RID: 8112
		public static class FREQUENT_MOLTEN
		{
			// Token: 0x04008EC8 RID: 36552
			public const float PCT_MIN = 0.016666668f;

			// Token: 0x04008EC9 RID: 36553
			public const float PCT_MAX = 0.1f;

			// Token: 0x04008ECA RID: 36554
			public const float LEN_MIN = 480f;

			// Token: 0x04008ECB RID: 36555
			public const float LEN_MAX = 1080f;
		}
	}
}
