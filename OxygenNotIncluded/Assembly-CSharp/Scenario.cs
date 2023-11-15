using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000952 RID: 2386
[AddComponentMenu("KMonoBehaviour/scripts/Scenario")]
public class Scenario : KMonoBehaviour
{
	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x060045C9 RID: 17865 RVA: 0x0018A82F File Offset: 0x00188A2F
	// (set) Token: 0x060045CA RID: 17866 RVA: 0x0018A837 File Offset: 0x00188A37
	public bool[] ReplaceElementMask { get; set; }

	// Token: 0x060045CB RID: 17867 RVA: 0x0018A840 File Offset: 0x00188A40
	public static void DestroyInstance()
	{
		Scenario.Instance = null;
	}

	// Token: 0x060045CC RID: 17868 RVA: 0x0018A848 File Offset: 0x00188A48
	protected override void OnPrefabInit()
	{
		Scenario.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x0018A876 File Offset: 0x00188A76
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.Init();
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x0018A880 File Offset: 0x00188A80
	private void Init()
	{
		this.Bot = Grid.HeightInCells / 4;
		this.Left = 150;
		this.RootCell = Grid.OffsetCell(0, this.Left, this.Bot);
		this.ReplaceElementMask = new bool[Grid.CellCount];
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x0018A8D0 File Offset: 0x00188AD0
	private void DigHole(int x, int y, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + j), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 200f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + width, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y - 1), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + height), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x0018AA0F File Offset: 0x00188C0F
	private void Fill(int x, int y, SimHashes id = SimHashes.Ice)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), id, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x0018AA40 File Offset: 0x00188C40
	private void PlaceColumn(int x, int y, int height)
	{
		for (int i = 0; i < height; i++)
		{
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + i), SimHashes.Ice, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x0018AA90 File Offset: 0x00188C90
	private void PlaceTileX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left + i, bot, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x0018AAC0 File Offset: 0x00188CC0
	private void PlaceTileY(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left, bot + i, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x0018AAEE File Offset: 0x00188CEE
	private void Clear(int x, int y)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x0018AB24 File Offset: 0x00188D24
	private void PlacerLadder(int x, int y, int amount)
	{
		int num = 1;
		if (amount < 0)
		{
			amount = -amount;
			num = -1;
		}
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(x, y + i * num, "Ladder", SimHashes.Cuprite);
		}
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x0018AB60 File Offset: 0x00188D60
	private void PlaceBuildings(int left, int bot)
	{
		this.PlaceBuilding(++left, bot, "ManualGenerator", SimHashes.Iron);
		this.PlaceBuilding(left += 2, bot, "OxygenMachine", SimHashes.Steel);
		this.PlaceBuilding(left += 2, bot, "SpaceHeater", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Electrolyzer", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Smelter", SimHashes.Steel);
		this.SpawnOre(left, bot + 1, SimHashes.Ice);
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x0018ABF4 File Offset: 0x00188DF4
	private IEnumerator TurnOn(GameObject go)
	{
		yield return null;
		yield return null;
		go.GetComponent<BuildingEnabledButton>().IsEnabled = true;
		yield break;
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x0018AC04 File Offset: 0x00188E04
	private void SetupPlacerTest(Scenario.Builder b, Element element)
	{
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.Name != "Excavator")
			{
				b.Placer(buildingDef.PrefabID, element);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x0018AC80 File Offset: 0x00188E80
	private void SetupBuildingTest(Scenario.RowLayout row_layout, bool is_powered, bool break_building)
	{
		Scenario.Builder builder = null;
		int num = 0;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (builder == null)
			{
				builder = row_layout.NextRow();
				num = this.Left;
				if (is_powered)
				{
					builder.Minion(null);
					builder.Minion(null);
				}
			}
			if (buildingDef.Name != "Excavator")
			{
				GameObject gameObject = builder.Building(buildingDef.PrefabID);
				if (break_building)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						component.DoDamage(int.MaxValue);
					}
				}
			}
			if (builder.Left > num + 100)
			{
				builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
				builder = null;
			}
		}
		builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x0018AD6C File Offset: 0x00188F6C
	private IEnumerator RunAfterNextUpdateRoutine(System.Action action)
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		action();
		yield break;
	}

	// Token: 0x060045DB RID: 17883 RVA: 0x0018AD7B File Offset: 0x00188F7B
	private void RunAfterNextUpdate(System.Action action)
	{
		base.StartCoroutine(this.RunAfterNextUpdateRoutine(action));
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x0018AD8C File Offset: 0x00188F8C
	public void SetupFabricatorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Cuprite);
		b.Minion(null);
		b.Building("Masonry");
		b.InAndOuts();
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x0018ADE2 File Offset: 0x00188FE2
	public void SetupDoorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x0018AE1C File Offset: 0x0018901C
	public void SetupHatchTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045DF RID: 17887 RVA: 0x0018AE4E File Offset: 0x0018904E
	public void SetupPropaneGeneratorTest(Scenario.Builder b)
	{
		b.Building("PropaneGenerator");
		b.Building("OxygenMachine");
		b.FinalizeRoom(SimHashes.Propane, SimHashes.Steel);
	}

	// Token: 0x060045E0 RID: 17888 RVA: 0x0018AE7C File Offset: 0x0018907C
	public void SetupAirLockTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("PoweredAirlock");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E1 RID: 17889 RVA: 0x0018AED0 File Offset: 0x001890D0
	public void SetupBedTest(Scenario.Builder b)
	{
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ManualGenerator");
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ComfyBed");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E2 RID: 17890 RVA: 0x0018AF50 File Offset: 0x00189150
	public void SetupHexapedTest(Scenario.Builder b)
	{
		b.Fill(4, 4, SimHashes.Oxygen);
		b.Prefab("Hexaped", null);
		b.Jump(2, 0);
		b.Ore(1, SimHashes.Iron);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E3 RID: 17891 RVA: 0x0018AF90 File Offset: 0x00189190
	public void SetupElectrolyzerTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Ice);
		b.Minion(null);
		b.Building("Electrolyzer");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E4 RID: 17892 RVA: 0x0018AFE0 File Offset: 0x001891E0
	public void SetupOrePerformanceTest(Scenario.Builder b)
	{
		int num = 20;
		int num2 = 20;
		int left = b.Left;
		int bot = b.Bot;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j += 2)
			{
				b.Jump(i, j);
				b.Ore(1, SimHashes.Cuprite);
				b.JumpTo(left, bot);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E5 RID: 17893 RVA: 0x0018B050 File Offset: 0x00189250
	public void SetupFeedingTest(Scenario.Builder b)
	{
		b.FillOffsets(SimHashes.IgneousRock, new int[]
		{
			1,
			0,
			3,
			0,
			3,
			1,
			5,
			0,
			5,
			1,
			5,
			2,
			7,
			0,
			7,
			1,
			7,
			2,
			9,
			0,
			9,
			1,
			11,
			0
		});
		b.PrefabOffsets("Hexaped", new int[]
		{
			0,
			0,
			2,
			0,
			4,
			0,
			7,
			3,
			9,
			2,
			11,
			1
		});
		b.OreOffsets(1, SimHashes.IronOre, new int[]
		{
			1,
			1,
			3,
			2,
			5,
			3,
			8,
			0,
			10,
			0,
			12,
			0
		});
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E6 RID: 17894 RVA: 0x0018B0C8 File Offset: 0x001892C8
	public void SetupLiquifierTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Minion(null);
		b.Ore(2, SimHashes.Ice);
		b.Building("ManualGenerator");
		b.Building("Liquifier");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E7 RID: 17895 RVA: 0x0018B118 File Offset: 0x00189318
	public void SetupFallTest(Scenario.Builder b)
	{
		b.Jump(0, 5);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Building("Tile");
		b.Building("Tile");
		b.Jump(-1, 1);
		b.Minion(null);
		b.Jump(2, 0);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Jump(2, 1);
		b.Minion(null);
		b.Building("Ladder");
		b.Jump(-1, -1);
		b.Building("Tile");
		b.Jump(-1, -3);
		b.Building("Ladder");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045E8 RID: 17896 RVA: 0x0018B1E8 File Offset: 0x001893E8
	public void SetupClimbTest(int left, int bot)
	{
		this.DigHole(left, bot, 13, 5);
		this.SpawnPrefab(left + 1, bot + 1, "Minion", Grid.SceneLayer.Ore);
		int num = left + 2;
		this.Clear(num++, bot - 1);
		num++;
		this.Fill(num++, bot, SimHashes.Ice);
		num++;
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		this.Fill(num++, bot, SimHashes.Ice);
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		num++;
		this.Fill(num, bot, SimHashes.Ice);
		this.Fill(num, bot + 1, SimHashes.Ice);
	}

	// Token: 0x060045E9 RID: 17897 RVA: 0x0018B2A0 File Offset: 0x001894A0
	private void SetupSuitRechargeTest(Scenario.Builder b)
	{
		b.Prefab("PressureSuit", delegate(GameObject go)
		{
			go.GetComponent<SuitTank>().Empty();
		});
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("SuitRecharger");
		b.Minion(null);
		b.Building("GasVent");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045EA RID: 17898 RVA: 0x0018B31C File Offset: 0x0018951C
	private void SetupSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.Jump(1, 2);
		b.Building("Tile");
		b.Jump(-1, -2);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x0018B384 File Offset: 0x00189584
	private void SetupTwoKelvinsOneSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(2, 0);
		b.Building("Door");
		b.Jump(2, 0);
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045EC RID: 17900 RVA: 0x0018B3D8 File Offset: 0x001895D8
	public void Clear()
	{
		foreach (Brain brain in Components.Brains.Items)
		{
			UnityEngine.Object.Destroy(brain.gameObject);
		}
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			UnityEngine.Object.Destroy(buildingComplete.gameObject);
		}
	}

	// Token: 0x060045ED RID: 17901 RVA: 0x0018B4C4 File Offset: 0x001896C4
	public void SetupGameplayTest()
	{
		this.Init();
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		if (this.ClearExistingScene)
		{
			this.Clear();
		}
		Scenario.RowLayout rowLayout = new Scenario.RowLayout(0, 0);
		if (this.CementMixerTest)
		{
			this.SetupCementMixerTest(rowLayout.NextRow());
		}
		if (this.RockCrusherTest)
		{
			this.SetupRockCrusherTest(rowLayout.NextRow());
		}
		if (this.PropaneGeneratorTest)
		{
			this.SetupPropaneGeneratorTest(rowLayout.NextRow());
		}
		if (this.DoorTest)
		{
			this.SetupDoorTest(rowLayout.NextRow());
		}
		if (this.HatchTest)
		{
			this.SetupHatchTest(rowLayout.NextRow());
		}
		if (this.AirLockTest)
		{
			this.SetupAirLockTest(rowLayout.NextRow());
		}
		if (this.BedTest)
		{
			this.SetupBedTest(rowLayout.NextRow());
		}
		if (this.LiquifierTest)
		{
			this.SetupLiquifierTest(rowLayout.NextRow());
		}
		if (this.SuitTest)
		{
			this.SetupSuitTest(rowLayout.NextRow());
		}
		if (this.SuitRechargeTest)
		{
			this.SetupSuitRechargeTest(rowLayout.NextRow());
		}
		if (this.TwoKelvinsOneSuitTest)
		{
			this.SetupTwoKelvinsOneSuitTest(rowLayout.NextRow());
		}
		if (this.FabricatorTest)
		{
			this.SetupFabricatorTest(rowLayout.NextRow());
		}
		if (this.ElectrolyzerTest)
		{
			this.SetupElectrolyzerTest(rowLayout.NextRow());
		}
		if (this.HexapedTest)
		{
			this.SetupHexapedTest(rowLayout.NextRow());
		}
		if (this.FallTest)
		{
			this.SetupFallTest(rowLayout.NextRow());
		}
		if (this.FeedingTest)
		{
			this.SetupFeedingTest(rowLayout.NextRow());
		}
		if (this.OrePerformanceTest)
		{
			this.SetupOrePerformanceTest(rowLayout.NextRow());
		}
		if (this.KilnTest)
		{
			this.SetupKilnTest(rowLayout.NextRow());
		}
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x0018B66D File Offset: 0x0018986D
	private GameObject SpawnMinion(int x, int y)
	{
		return this.SpawnPrefab(x, y, "Minion", Grid.SceneLayer.Move);
	}

	// Token: 0x060045EF RID: 17903 RVA: 0x0018B680 File Offset: 0x00189880
	private void SetupLadderTest(int left, int bot)
	{
		int num = 5;
		this.DigHole(left, bot, 13, num);
		this.SpawnMinion(left + 1, bot);
		int x = left + 1;
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot - 1, -num);
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x0018B75C File Offset: 0x0018995C
	public void PlaceUtilitiesX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceUtilities(left + i, bot);
		}
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x0018B77F File Offset: 0x0018997F
	public void PlaceUtilities(int left, int bot)
	{
		this.PlaceBuilding(left, bot, "Wire", SimHashes.Cuprite);
		this.PlaceBuilding(left, bot, "GasConduit", SimHashes.Cuprite);
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0018B7A8 File Offset: 0x001899A8
	public void SetupVisualTest()
	{
		this.Init();
		Scenario.RowLayout row_layout = new Scenario.RowLayout(this.Left, this.Bot);
		this.SetupBuildingTest(row_layout, false, false);
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x0018B7D8 File Offset: 0x001899D8
	private void SpawnMaterialTest(Scenario.Builder b)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				b.Element = element.id;
				b.Building("Generator");
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x0018B854 File Offset: 0x00189A54
	public GameObject PlaceBuilding(int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		return Scenario.PlaceBuilding(this.RootCell, x, y, prefab_id, element);
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x0018B868 File Offset: 0x00189A68
	public static GameObject PlaceBuilding(int root_cell, int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		int cell = Grid.OffsetCell(root_cell, x, y);
		BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
		if (buildingDef == null || buildingDef.PlacementOffsets == null)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Missing def for",
				prefab_id
			});
		}
		Element element2 = ElementLoader.FindElementByHash(element);
		global::Debug.Assert(element2 != null, string.Concat(new string[]
		{
			"Missing primary element '",
			Enum.GetName(typeof(SimHashes), element),
			"' in '",
			prefab_id,
			"'"
		}));
		GameObject gameObject = buildingDef.Build(buildingDef.GetBuildingCell(cell), Orientation.Neutral, null, new Tag[]
		{
			element2.tag,
			ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
		}, 293.15f, false, -1f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.InternalTemperature = 300f;
		component.Temperature = 300f;
		return gameObject;
	}

	// Token: 0x060045F6 RID: 17910 RVA: 0x0018B95C File Offset: 0x00189B5C
	private void SpawnOre(int x, int y, SimHashes element = SimHashes.Cuprite)
	{
		this.RunAfterNextUpdate(delegate
		{
			Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(this.RootCell, x, y), Grid.SceneLayer.Ore);
			position.x += UnityEngine.Random.Range(-0.1f, 0.1f);
			ElementLoader.FindElementByHash(element).substance.SpawnResource(position, 4000f, 293f, byte.MaxValue, 0, false, false, false);
		});
	}

	// Token: 0x060045F7 RID: 17911 RVA: 0x0018B99D File Offset: 0x00189B9D
	public GameObject SpawnPrefab(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		return Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x0018B9B0 File Offset: 0x00189BB0
	public void SpawnPrefabLate(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		this.RunAfterNextUpdate(delegate
		{
			Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
		});
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x0018B9FC File Offset: 0x00189BFC
	public static GameObject SpawnPrefab(int RootCell, int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		int cell = Grid.OffsetCell(RootCell, x, y);
		GameObject prefab = Assets.GetPrefab(TagManager.Create(name));
		if (prefab == null)
		{
			return null;
		}
		return GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell, scene_layer), scene_layer, null, 0);
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x0018BA3C File Offset: 0x00189C3C
	public void SetupElementTest()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		HashSet<Element> elements = new HashSet<Element>();
		int bot = builder.Bot;
		foreach (Element element5 in (from element in ElementLoader.elements
		where element.IsSolid
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			if (element5.IsSolid)
			{
				Element element2 = element5;
				int left = builder.Left;
				bool hasTransitionUp;
				do
				{
					elements.Add(element2);
					builder.Hole(2, 3);
					builder.Fill(2, 2, element2.id);
					builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
					builder = new Scenario.Builder(left, builder.Bot + 4, SimHashes.Copper);
					hasTransitionUp = element2.HasTransitionUp;
					if (hasTransitionUp)
					{
						element2 = element2.highTempTransition;
					}
				}
				while (hasTransitionUp);
				builder = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
			}
		}
		foreach (Element element3 in (from element in ElementLoader.elements
		where element.IsLiquid && !elements.Contains(element)
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			int left2 = builder.Left;
			bool hasTransitionUp2;
			do
			{
				elements.Add(element3);
				builder.Hole(2, 3);
				builder.Fill(2, 2, element3.id);
				builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
				builder = new Scenario.Builder(left2, builder.Bot + 4, SimHashes.Copper);
				hasTransitionUp2 = element3.HasTransitionUp;
				if (hasTransitionUp2)
				{
					element3 = element3.highTempTransition;
				}
			}
			while (hasTransitionUp2);
			builder = new Scenario.Builder(left2 + 3, bot, SimHashes.Copper);
		}
		foreach (Element element4 in (from element in ElementLoader.elements
		where element.state == Element.State.Gas && !elements.Contains(element)
		select element).ToList<Element>())
		{
			int left3 = builder.Left;
			builder.Hole(2, 3);
			builder.Fill(2, 2, element4.id);
			builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
			builder = new Scenario.Builder(left3, builder.Bot + 4, SimHashes.Copper);
			builder = new Scenario.Builder(left3 + 3, bot, SimHashes.Copper);
		}
	}

	// Token: 0x060045FB RID: 17915 RVA: 0x0018BD60 File Offset: 0x00189F60
	private void InitDebugScenario()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x0018BD9C File Offset: 0x00189F9C
	public void SetupTileTest()
	{
		this.InitDebugScenario();
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			for (int j = 0; j < Grid.WidthInCells; j++)
			{
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 100f, -1f, byte.MaxValue, 0, -1);
			}
		}
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		for (int k = 0; k < 16; k++)
		{
			builder.Jump(0, 0);
			builder.Fill(1, 1, ((k & 1) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 2) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(-1, 1);
			builder.Fill(1, 1, ((k & 4) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 8) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(2, -1);
		}
	}

	// Token: 0x060045FD RID: 17917 RVA: 0x0018BEA8 File Offset: 0x0018A0A8
	public void SetupRiverTest()
	{
		this.InitDebugScenario();
		int num = Mathf.Min(64, Grid.WidthInCells);
		int num2 = Mathf.Min(64, Grid.HeightInCells);
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsLiquid)
			{
				list.Add(element);
			}
		}
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				SimHashes new_element = (i == 0) ? SimHashes.Unobtanium : SimHashes.Oxygen;
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), new_element, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
		}
	}

	// Token: 0x060045FE RID: 17918 RVA: 0x0018BF88 File Offset: 0x0018A188
	public void SetupRockCrusherTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060045FF RID: 17919 RVA: 0x0018BFC8 File Offset: 0x0018A1C8
	public void SetupCementMixerTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("Generator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.Minion(null);
		b.Building("Mixer");
		b.Ore(20, SimHashes.SedimentaryRock);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x0018C034 File Offset: 0x0018A234
	public void SetupKilnTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Kiln");
		b.Minion(null);
		b.Ore(20, SimHashes.SandCement);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x04002E46 RID: 11846
	private int Bot;

	// Token: 0x04002E47 RID: 11847
	private int Left;

	// Token: 0x04002E48 RID: 11848
	public int RootCell;

	// Token: 0x04002E4A RID: 11850
	public static Scenario Instance;

	// Token: 0x04002E4B RID: 11851
	public bool PropaneGeneratorTest = true;

	// Token: 0x04002E4C RID: 11852
	public bool HatchTest = true;

	// Token: 0x04002E4D RID: 11853
	public bool DoorTest = true;

	// Token: 0x04002E4E RID: 11854
	public bool AirLockTest = true;

	// Token: 0x04002E4F RID: 11855
	public bool BedTest = true;

	// Token: 0x04002E50 RID: 11856
	public bool SuitTest = true;

	// Token: 0x04002E51 RID: 11857
	public bool SuitRechargeTest = true;

	// Token: 0x04002E52 RID: 11858
	public bool FabricatorTest = true;

	// Token: 0x04002E53 RID: 11859
	public bool ElectrolyzerTest = true;

	// Token: 0x04002E54 RID: 11860
	public bool HexapedTest = true;

	// Token: 0x04002E55 RID: 11861
	public bool FallTest = true;

	// Token: 0x04002E56 RID: 11862
	public bool FeedingTest = true;

	// Token: 0x04002E57 RID: 11863
	public bool OrePerformanceTest = true;

	// Token: 0x04002E58 RID: 11864
	public bool TwoKelvinsOneSuitTest = true;

	// Token: 0x04002E59 RID: 11865
	public bool LiquifierTest = true;

	// Token: 0x04002E5A RID: 11866
	public bool RockCrusherTest = true;

	// Token: 0x04002E5B RID: 11867
	public bool CementMixerTest = true;

	// Token: 0x04002E5C RID: 11868
	public bool KilnTest = true;

	// Token: 0x04002E5D RID: 11869
	public bool ClearExistingScene = true;

	// Token: 0x020017B5 RID: 6069
	public class RowLayout
	{
		// Token: 0x06008F2B RID: 36651 RVA: 0x0032183D File Offset: 0x0031FA3D
		public RowLayout(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x06008F2C RID: 36652 RVA: 0x00321854 File Offset: 0x0031FA54
		public Scenario.Builder NextRow()
		{
			if (this.Builder != null)
			{
				this.Bot = this.Builder.Max.y + 1;
			}
			this.Builder = new Scenario.Builder(this.Left, this.Bot, SimHashes.Copper);
			return this.Builder;
		}

		// Token: 0x04006FAA RID: 28586
		public int Left;

		// Token: 0x04006FAB RID: 28587
		public int Bot;

		// Token: 0x04006FAC RID: 28588
		public Scenario.Builder Builder;
	}

	// Token: 0x020017B6 RID: 6070
	public class Builder
	{
		// Token: 0x06008F2D RID: 36653 RVA: 0x003218A4 File Offset: 0x0031FAA4
		public Builder(int left, int bot, SimHashes element = SimHashes.Copper)
		{
			this.Left = left;
			this.Bot = bot;
			this.Element = element;
			this.Scenario = Scenario.Instance;
			this.PlaceUtilities = true;
			this.Min = new Vector2I(left, bot);
			this.Max = new Vector2I(left, bot);
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x003218F8 File Offset: 0x0031FAF8
		private void UpdateMinMax(int x, int y)
		{
			this.Min.x = Math.Min(x, this.Min.x);
			this.Min.y = Math.Min(y, this.Min.y);
			this.Max.x = Math.Max(x + 1, this.Max.x);
			this.Max.y = Math.Max(y + 1, this.Max.y);
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x0032197C File Offset: 0x0031FB7C
		public void Utilities(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Scenario.PlaceUtilities(this.Left, this.Bot);
				this.Left++;
			}
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x003219BC File Offset: 0x0031FBBC
		public void BuildingOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Building(prefab_id);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x00321A0C File Offset: 0x0031FC0C
		public void Placer(string prefab_id, Element element)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			int buildingCell = buildingDef.GetBuildingCell(Grid.OffsetCell(Scenario.Instance.RootCell, this.Left, this.Bot));
			Vector3 pos = Grid.CellToPosCBC(buildingCell, Grid.SceneLayer.Building);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			this.Left += buildingDef.WidthInCells;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				Assets.GetBuildingDef(prefab_id).TryPlace(null, pos, Orientation.Neutral, new Tag[]
				{
					element.tag,
					ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
				}, 0);
			});
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x00321ACC File Offset: 0x0031FCCC
		public GameObject Building(string prefab_id)
		{
			GameObject result = this.Scenario.PlaceBuilding(this.Left, this.Bot, prefab_id, this.Element);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			if (this.PlaceUtilities)
			{
				for (int i = 0; i < buildingDef.WidthInCells; i++)
				{
					this.UpdateMinMax(this.Left + i, this.Bot);
					this.Scenario.PlaceUtilities(this.Left + i, this.Bot);
				}
			}
			this.Left += buildingDef.WidthInCells;
			return result;
		}

		// Token: 0x06008F33 RID: 36659 RVA: 0x00321B90 File Offset: 0x0031FD90
		public void Minion(Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnMinion(left, bot);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x06008F34 RID: 36660 RVA: 0x00321BF2 File Offset: 0x0031FDF2
		private GameObject Hexaped()
		{
			return this.Scenario.SpawnPrefab(this.Left, this.Bot, "Hexaped", Grid.SceneLayer.Front);
		}

		// Token: 0x06008F35 RID: 36661 RVA: 0x00321C14 File Offset: 0x0031FE14
		public void OreOffsets(int count, SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Ore(count, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008F36 RID: 36662 RVA: 0x00321C64 File Offset: 0x0031FE64
		public void Ore(int count = 1, SimHashes element = SimHashes.Cuprite)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			for (int i = 0; i < count; i++)
			{
				this.Scenario.SpawnOre(this.Left, this.Bot, element);
			}
		}

		// Token: 0x06008F37 RID: 36663 RVA: 0x00321CA8 File Offset: 0x0031FEA8
		public void PrefabOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Prefab(prefab_id, null);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008F38 RID: 36664 RVA: 0x00321CF8 File Offset: 0x0031FEF8
		public void Prefab(string prefab_id, Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnPrefab(left, bot, prefab_id, Grid.SceneLayer.Ore);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x06008F39 RID: 36665 RVA: 0x00321D64 File Offset: 0x0031FF64
		public void Wall(int height)
		{
			for (int i = 0; i < height; i++)
			{
				this.Scenario.PlaceBuilding(this.Left, this.Bot + i, "Tile", SimHashes.Cuprite);
				this.UpdateMinMax(this.Left, this.Bot + i);
				if (this.PlaceUtilities)
				{
					this.Scenario.PlaceUtilities(this.Left, this.Bot + i);
				}
			}
			this.Left++;
		}

		// Token: 0x06008F3A RID: 36666 RVA: 0x00321DE4 File Offset: 0x0031FFE4
		public void Jump(int x = 0, int y = 0)
		{
			this.Left += x;
			this.Bot += y;
		}

		// Token: 0x06008F3B RID: 36667 RVA: 0x00321E02 File Offset: 0x00320002
		public void JumpTo(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x06008F3C RID: 36668 RVA: 0x00321E14 File Offset: 0x00320014
		public void Hole(int width, int height)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.Scenario, 0f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x06008F3D RID: 36669 RVA: 0x00321EA4 File Offset: 0x003200A4
		public void FillOffsets(SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Fill(1, 1, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008F3E RID: 36670 RVA: 0x00321EF4 File Offset: 0x003200F4
		public void Fill(int width, int height, SimHashes element)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, element, CellEventLogger.Instance.Scenario, 5000f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x06008F3F RID: 36671 RVA: 0x00321F80 File Offset: 0x00320180
		public void InAndOuts()
		{
			this.Wall(3);
			this.Building("GasVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Building("LiquidVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Fill(3, 3, SimHashes.Water);
			this.Utilities(2);
			GameObject pump = this.Building("Pump");
			this.Scenario.RunAfterNextUpdate(delegate
			{
				pump.GetComponent<BuildingEnabledButton>().IsEnabled = true;
			});
		}

		// Token: 0x06008F40 RID: 36672 RVA: 0x0032201C File Offset: 0x0032021C
		public Scenario.Builder FinalizeRoom(SimHashes element = SimHashes.Oxygen, SimHashes tileElement = SimHashes.Steel)
		{
			for (int i = this.Min.x - 1; i <= this.Max.x; i++)
			{
				if (i == this.Min.x - 1 || i == this.Max.x)
				{
					for (int j = this.Min.y - 1; j <= this.Max.y; j++)
					{
						this.Scenario.PlaceBuilding(i, j, "Tile", tileElement);
					}
				}
				else
				{
					int num = 500;
					if (element == SimHashes.Void)
					{
						num = 0;
					}
					for (int k = this.Min.y; k < this.Max.y; k++)
					{
						int num2 = Grid.OffsetCell(this.Scenario.RootCell, i, k);
						if (!this.Scenario.ReplaceElementMask[num2])
						{
							SimMessages.ReplaceElement(num2, element, CellEventLogger.Instance.Scenario, (float)num, -1f, byte.MaxValue, 0, -1);
						}
					}
				}
				this.Scenario.PlaceBuilding(i, this.Min.y - 1, "Tile", tileElement);
				this.Scenario.PlaceBuilding(i, this.Max.y, "Tile", tileElement);
			}
			return new Scenario.Builder(this.Max.x + 1, this.Min.y, SimHashes.Copper);
		}

		// Token: 0x04006FAD RID: 28589
		public bool PlaceUtilities;

		// Token: 0x04006FAE RID: 28590
		public int Left;

		// Token: 0x04006FAF RID: 28591
		public int Bot;

		// Token: 0x04006FB0 RID: 28592
		public Vector2I Min;

		// Token: 0x04006FB1 RID: 28593
		public Vector2I Max;

		// Token: 0x04006FB2 RID: 28594
		public SimHashes Element;

		// Token: 0x04006FB3 RID: 28595
		private Scenario Scenario;
	}
}
