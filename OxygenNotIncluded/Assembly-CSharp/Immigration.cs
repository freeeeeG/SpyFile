using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000804 RID: 2052
[AddComponentMenu("KMonoBehaviour/scripts/Immigration")]
public class Immigration : KMonoBehaviour, ISaveLoadable, ISim200ms, IPersonalPriorityManager
{
	// Token: 0x06003A7D RID: 14973 RVA: 0x0014512F File Offset: 0x0014332F
	public static void DestroyInstance()
	{
		Immigration.Instance = null;
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x00145138 File Offset: 0x00143338
	protected override void OnPrefabInit()
	{
		this.bImmigrantAvailable = false;
		Immigration.Instance = this;
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		this.timeBeforeSpawn = this.spawnInterval[num];
		this.ResetPersonalPriorities();
		this.ConfigureCarePackages();
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x00145182 File Offset: 0x00143382
	private void ConfigureCarePackages()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ConfigureMultiWorldCarePackages();
			return;
		}
		this.ConfigureBaseGameCarePackages();
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x00145198 File Offset: 0x00143398
	private void ConfigureBaseGameCarePackages()
	{
		this.carePackages = new CarePackageInfo[]
		{
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, () => this.CycleCondition(12)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, () => this.CycleCondition(12) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, () => this.CycleCondition(12) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, () => this.CycleCondition(24) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, () => this.CycleCondition(24) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag)),
			new CarePackageInfo("PrickleGrassSeed", 3f, null),
			new CarePackageInfo("LeafyPlantSeed", 3f, null),
			new CarePackageInfo("CactusPlantSeed", 3f, null),
			new CarePackageInfo("MushroomSeed", 1f, null),
			new CarePackageInfo("PrickleFlowerSeed", 2f, null),
			new CarePackageInfo("OxyfernSeed", 1f, null),
			new CarePackageInfo("ForestTreeSeed", 1f, null),
			new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, () => this.CycleCondition(24)),
			new CarePackageInfo("SwampLilySeed", 1f, () => this.CycleCondition(24)),
			new CarePackageInfo("ColdBreatherSeed", 1f, () => this.CycleCondition(24)),
			new CarePackageInfo("SpiceVineSeed", 1f, () => this.CycleCondition(24)),
			new CarePackageInfo("FieldRation", 5f, null),
			new CarePackageInfo("BasicForagePlant", 6f, null),
			new CarePackageInfo("CookedEgg", 3f, () => this.CycleCondition(6)),
			new CarePackageInfo(PrickleFruitConfig.ID, 3f, () => this.CycleCondition(12)),
			new CarePackageInfo("FriedMushroom", 3f, () => this.CycleCondition(24)),
			new CarePackageInfo("CookedMeat", 3f, () => this.CycleCondition(48)),
			new CarePackageInfo("SpicyTofu", 3f, () => this.CycleCondition(48)),
			new CarePackageInfo("LightBugBaby", 1f, null),
			new CarePackageInfo("HatchBaby", 1f, null),
			new CarePackageInfo("PuftBaby", 1f, null),
			new CarePackageInfo("SquirrelBaby", 1f, null),
			new CarePackageInfo("CrabBaby", 1f, null),
			new CarePackageInfo("DreckoBaby", 1f, () => this.CycleCondition(24)),
			new CarePackageInfo("Pacu", 8f, () => this.CycleCondition(24)),
			new CarePackageInfo("MoleBaby", 1f, () => this.CycleCondition(48)),
			new CarePackageInfo("OilfloaterBaby", 1f, () => this.CycleCondition(48)),
			new CarePackageInfo("LightBugEgg", 3f, null),
			new CarePackageInfo("HatchEgg", 3f, null),
			new CarePackageInfo("PuftEgg", 3f, null),
			new CarePackageInfo("OilfloaterEgg", 3f, () => this.CycleCondition(12)),
			new CarePackageInfo("MoleEgg", 3f, () => this.CycleCondition(24)),
			new CarePackageInfo("DreckoEgg", 3f, () => this.CycleCondition(24)),
			new CarePackageInfo("SquirrelEgg", 2f, null),
			new CarePackageInfo("BasicCure", 3f, null),
			new CarePackageInfo("CustomClothing", 1f, null, "SELECTRANDOM"),
			new CarePackageInfo("Funky_Vest", 1f, null)
		};
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x00145944 File Offset: 0x00143B44
	private void ConfigureMultiWorldCarePackages()
	{
		this.carePackages = new CarePackageInfo[]
		{
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, () => this.CycleCondition(12)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, null),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, () => this.CycleCondition(12) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, () => this.CycleCondition(12) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, () => this.CycleCondition(24) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, () => this.CycleCondition(24) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag)),
			new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, () => this.CycleCondition(48) && this.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag)),
			new CarePackageInfo("PrickleGrassSeed", 3f, null),
			new CarePackageInfo("LeafyPlantSeed", 3f, null),
			new CarePackageInfo("CactusPlantSeed", 3f, null),
			new CarePackageInfo("WineCupsSeed", 3f, null),
			new CarePackageInfo("CylindricaSeed", 3f, null),
			new CarePackageInfo("MushroomSeed", 1f, null),
			new CarePackageInfo("PrickleFlowerSeed", 2f, () => this.DiscoveredCondition("PrickleFlowerSeed") || this.CycleCondition(500)),
			new CarePackageInfo("OxyfernSeed", 1f, null),
			new CarePackageInfo("ForestTreeSeed", 1f, () => this.DiscoveredCondition("ForestTreeSeed") || this.CycleCondition(500)),
			new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, () => this.CycleCondition(24) && (this.DiscoveredCondition(BasicFabricMaterialPlantConfig.SEED_ID) || this.CycleCondition(500))),
			new CarePackageInfo("SwampLilySeed", 1f, () => this.CycleCondition(24) && (this.DiscoveredCondition("SwampLilySeed") || this.CycleCondition(500))),
			new CarePackageInfo("ColdBreatherSeed", 1f, () => this.CycleCondition(24) && (this.DiscoveredCondition("ColdBreatherSeed") || this.CycleCondition(500))),
			new CarePackageInfo("SpiceVineSeed", 1f, () => this.CycleCondition(24) && (this.DiscoveredCondition("SpiceVineSeed") || this.CycleCondition(500))),
			new CarePackageInfo("WormPlantSeed", 1f, () => this.CycleCondition(24) && (this.DiscoveredCondition("WormPlantSeed") || this.CycleCondition(500))),
			new CarePackageInfo("FieldRation", 5f, null),
			new CarePackageInfo("BasicForagePlant", 6f, () => this.DiscoveredCondition("BasicForagePlant")),
			new CarePackageInfo("ForestForagePlant", 2f, () => this.DiscoveredCondition("ForestForagePlant")),
			new CarePackageInfo("SwampForagePlant", 2f, () => this.DiscoveredCondition("SwampForagePlant")),
			new CarePackageInfo("CookedEgg", 3f, () => this.CycleCondition(6)),
			new CarePackageInfo(PrickleFruitConfig.ID, 3f, () => this.CycleCondition(12) && (this.DiscoveredCondition(PrickleFruitConfig.ID) || this.CycleCondition(500))),
			new CarePackageInfo("FriedMushroom", 3f, () => this.CycleCondition(24)),
			new CarePackageInfo("CookedMeat", 3f, () => this.CycleCondition(48)),
			new CarePackageInfo("SpicyTofu", 3f, () => this.CycleCondition(48)),
			new CarePackageInfo("WormSuperFood", 2f, () => this.DiscoveredCondition("WormPlantSeed") || this.CycleCondition(500)),
			new CarePackageInfo("LightBugBaby", 1f, () => this.DiscoveredCondition("LightBugEgg") || this.CycleCondition(500)),
			new CarePackageInfo("HatchBaby", 1f, () => this.DiscoveredCondition("HatchEgg") || this.CycleCondition(500)),
			new CarePackageInfo("PuftBaby", 1f, () => this.DiscoveredCondition("PuftEgg") || this.CycleCondition(500)),
			new CarePackageInfo("SquirrelBaby", 1f, () => this.DiscoveredCondition("SquirrelEgg") || this.CycleCondition(24) || this.CycleCondition(500)),
			new CarePackageInfo("CrabBaby", 1f, () => this.DiscoveredCondition("CrabEgg") || this.CycleCondition(500)),
			new CarePackageInfo("DreckoBaby", 1f, () => this.CycleCondition(24) && (this.DiscoveredCondition("DreckoEgg") || this.CycleCondition(500))),
			new CarePackageInfo("Pacu", 8f, () => this.CycleCondition(24) && (this.DiscoveredCondition("PacuEgg") || this.CycleCondition(500))),
			new CarePackageInfo("MoleBaby", 1f, () => this.CycleCondition(48) && (this.DiscoveredCondition("MoleEgg") || this.CycleCondition(500))),
			new CarePackageInfo("OilfloaterBaby", 1f, () => this.CycleCondition(48) && (this.DiscoveredCondition("OilfloaterEgg") || this.CycleCondition(500))),
			new CarePackageInfo("DivergentBeetleBaby", 1f, () => this.CycleCondition(48) && (this.DiscoveredCondition("DivergentBeetleEgg") || this.CycleCondition(500))),
			new CarePackageInfo("StaterpillarBaby", 1f, () => this.CycleCondition(48) && (this.DiscoveredCondition("StaterpillarEgg") || this.CycleCondition(500))),
			new CarePackageInfo("LightBugEgg", 3f, () => this.DiscoveredCondition("LightBugEgg") || this.CycleCondition(500)),
			new CarePackageInfo("HatchEgg", 3f, () => this.DiscoveredCondition("HatchEgg") || this.CycleCondition(500)),
			new CarePackageInfo("PuftEgg", 3f, () => this.DiscoveredCondition("PuftEgg") || this.CycleCondition(500)),
			new CarePackageInfo("OilfloaterEgg", 3f, () => this.CycleCondition(12) && (this.DiscoveredCondition("OilfloaterEgg") || this.CycleCondition(500))),
			new CarePackageInfo("MoleEgg", 3f, () => this.CycleCondition(24) && (this.DiscoveredCondition("MoleEgg") || this.CycleCondition(500))),
			new CarePackageInfo("DreckoEgg", 3f, () => this.CycleCondition(24) && (this.DiscoveredCondition("DreckoEgg") || this.CycleCondition(500))),
			new CarePackageInfo("SquirrelEgg", 2f, () => this.DiscoveredCondition("SquirrelEgg") || this.CycleCondition(24) || this.CycleCondition(500)),
			new CarePackageInfo("DivergentBeetleEgg", 2f, () => this.CycleCondition(48) && (this.DiscoveredCondition("DivergentBeetleEgg") || this.CycleCondition(500))),
			new CarePackageInfo("StaterpillarEgg", 2f, () => this.CycleCondition(48) && (this.DiscoveredCondition("StaterpillarEgg") || this.CycleCondition(500))),
			new CarePackageInfo("BasicCure", 3f, null),
			new CarePackageInfo("CustomClothing", 1f, null, "SELECTRANDOM"),
			new CarePackageInfo("Funky_Vest", 1f, null)
		};
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x00146291 File Offset: 0x00144491
	private bool CycleCondition(int cycle)
	{
		return GameClock.Instance.GetCycle() >= cycle;
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x001462A3 File Offset: 0x001444A3
	private bool DiscoveredCondition(Tag tag)
	{
		return DiscoveredResources.Instance.IsDiscovered(tag);
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06003A84 RID: 14980 RVA: 0x001462B0 File Offset: 0x001444B0
	public bool ImmigrantsAvailable
	{
		get
		{
			return this.bImmigrantAvailable;
		}
	}

	// Token: 0x06003A85 RID: 14981 RVA: 0x001462B8 File Offset: 0x001444B8
	public int EndImmigration()
	{
		this.bImmigrantAvailable = false;
		this.spawnIdx++;
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		this.timeBeforeSpawn = this.spawnInterval[num];
		return this.spawnTable[num];
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x00146306 File Offset: 0x00144506
	public float GetTimeRemaining()
	{
		return this.timeBeforeSpawn;
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x00146310 File Offset: 0x00144510
	public float GetTotalWaitTime()
	{
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		return this.spawnInterval[num];
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0014633C File Offset: 0x0014453C
	public void Sim200ms(float dt)
	{
		if (this.IsHalted() || this.bImmigrantAvailable)
		{
			return;
		}
		this.timeBeforeSpawn -= dt;
		this.timeBeforeSpawn = Math.Max(this.timeBeforeSpawn, 0f);
		if (this.timeBeforeSpawn <= 0f)
		{
			this.bImmigrantAvailable = true;
		}
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x00146394 File Offset: 0x00144594
	private bool IsHalted()
	{
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			Operational component = telepad.GetComponent<Operational>();
			if (component != null && component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x00146404 File Offset: 0x00144604
	public int GetPersonalPriority(ChoreGroup group)
	{
		int result;
		if (!this.defaultPersonalPriorities.TryGetValue(group.IdHash, out result))
		{
			result = 3;
		}
		return result;
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0014642C File Offset: 0x0014462C
	public CarePackageInfo RandomCarePackage()
	{
		List<CarePackageInfo> list = new List<CarePackageInfo>();
		foreach (CarePackageInfo carePackageInfo in this.carePackages)
		{
			if (carePackageInfo.requirement == null || carePackageInfo.requirement())
			{
				list.Add(carePackageInfo);
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x00146486 File Offset: 0x00144686
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		this.defaultPersonalPriorities[group.IdHash] = value;
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x0014649A File Offset: 0x0014469A
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x001464A0 File Offset: 0x001446A0
	public void ApplyDefaultPersonalPriorities(GameObject minion)
	{
		IPersonalPriorityManager instance = Immigration.Instance;
		IPersonalPriorityManager component = minion.GetComponent<ChoreConsumer>();
		foreach (ChoreGroup group in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = instance.GetPersonalPriority(group);
			component.SetPersonalPriority(group, personalPriority);
		}
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00146514 File Offset: 0x00144714
	public void ResetPersonalPriorities()
	{
		bool advancedPersonalPriorities = Game.Instance.advancedPersonalPriorities;
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			this.defaultPersonalPriorities[choreGroup.IdHash] = (advancedPersonalPriorities ? choreGroup.DefaultPersonalPriority : 3);
		}
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00146594 File Offset: 0x00144794
	public bool IsChoreGroupDisabled(ChoreGroup g)
	{
		return false;
	}

	// Token: 0x040026EC RID: 9964
	public float[] spawnInterval;

	// Token: 0x040026ED RID: 9965
	public int[] spawnTable;

	// Token: 0x040026EE RID: 9966
	[Serialize]
	private Dictionary<HashedString, int> defaultPersonalPriorities = new Dictionary<HashedString, int>();

	// Token: 0x040026EF RID: 9967
	[Serialize]
	public float timeBeforeSpawn = float.PositiveInfinity;

	// Token: 0x040026F0 RID: 9968
	[Serialize]
	private bool bImmigrantAvailable;

	// Token: 0x040026F1 RID: 9969
	[Serialize]
	private int spawnIdx;

	// Token: 0x040026F2 RID: 9970
	private CarePackageInfo[] carePackages;

	// Token: 0x040026F3 RID: 9971
	public static Immigration Instance;

	// Token: 0x040026F4 RID: 9972
	private const int CYCLE_THRESHOLD_A = 6;

	// Token: 0x040026F5 RID: 9973
	private const int CYCLE_THRESHOLD_B = 12;

	// Token: 0x040026F6 RID: 9974
	private const int CYCLE_THRESHOLD_C = 24;

	// Token: 0x040026F7 RID: 9975
	private const int CYCLE_THRESHOLD_D = 48;

	// Token: 0x040026F8 RID: 9976
	private const int CYCLE_THRESHOLD_UNLOCK_EVERYTHING = 500;

	// Token: 0x040026F9 RID: 9977
	public const string FACADE_SELECT_RANDOM = "SELECTRANDOM";
}
