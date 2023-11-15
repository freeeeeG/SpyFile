using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using TUNING;
using UnityEngine;

// Token: 0x02000A33 RID: 2611
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06004E42 RID: 20034 RVA: 0x001B64E5 File Offset: 0x001B46E5
	// (set) Token: 0x06004E43 RID: 20035 RVA: 0x001B64ED File Offset: 0x001B46ED
	[Serialize]
	public WorldInventory worldInventory { get; private set; }

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06004E44 RID: 20036 RVA: 0x001B64F6 File Offset: 0x001B46F6
	// (set) Token: 0x06004E45 RID: 20037 RVA: 0x001B64FE File Offset: 0x001B46FE
	public Dictionary<Tag, float> materialNeeds { get; private set; }

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06004E46 RID: 20038 RVA: 0x001B6507 File Offset: 0x001B4707
	public bool IsModuleInterior
	{
		get
		{
			return this.isModuleInterior;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06004E47 RID: 20039 RVA: 0x001B650F File Offset: 0x001B470F
	public bool IsDiscovered
	{
		get
		{
			return this.isDiscovered || DebugHandler.RevealFogOfWar;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06004E48 RID: 20040 RVA: 0x001B6520 File Offset: 0x001B4720
	public bool IsStartWorld
	{
		get
		{
			return this.isStartWorld;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06004E49 RID: 20041 RVA: 0x001B6528 File Offset: 0x001B4728
	public bool IsDupeVisited
	{
		get
		{
			return this.isDupeVisited;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06004E4A RID: 20042 RVA: 0x001B6530 File Offset: 0x001B4730
	public float DupeVisitedTimestamp
	{
		get
		{
			return this.dupeVisitedTimestamp;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06004E4B RID: 20043 RVA: 0x001B6538 File Offset: 0x001B4738
	public float DiscoveryTimestamp
	{
		get
		{
			return this.discoveryTimestamp;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06004E4C RID: 20044 RVA: 0x001B6540 File Offset: 0x001B4740
	public bool IsRoverVisted
	{
		get
		{
			return this.isRoverVisited;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06004E4D RID: 20045 RVA: 0x001B6548 File Offset: 0x001B4748
	public bool IsSurfaceRevealed
	{
		get
		{
			return this.isSurfaceRevealed;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06004E4E RID: 20046 RVA: 0x001B6550 File Offset: 0x001B4750
	public Dictionary<string, int> SunlightFixedTraits
	{
		get
		{
			return this.sunlightFixedTraits;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06004E4F RID: 20047 RVA: 0x001B6558 File Offset: 0x001B4758
	public Dictionary<string, int> CosmicRadiationFixedTraits
	{
		get
		{
			return this.cosmicRadiationFixedTraits;
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x06004E50 RID: 20048 RVA: 0x001B6560 File Offset: 0x001B4760
	public List<string> Biomes
	{
		get
		{
			return this.m_subworldNames;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06004E51 RID: 20049 RVA: 0x001B6568 File Offset: 0x001B4768
	public List<string> WorldTraitIds
	{
		get
		{
			return this.m_worldTraitIds;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06004E52 RID: 20050 RVA: 0x001B6570 File Offset: 0x001B4770
	public List<string> StoryTraitIds
	{
		get
		{
			return this.m_storyTraitIds;
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06004E53 RID: 20051 RVA: 0x001B6578 File Offset: 0x001B4778
	public AlertStateManager.Instance AlertManager
	{
		get
		{
			if (this.m_alertManager == null)
			{
				StateMachineController component = base.GetComponent<StateMachineController>();
				this.m_alertManager = component.GetSMI<AlertStateManager.Instance>();
			}
			global::Debug.Assert(this.m_alertManager != null, "AlertStateManager should never be null.");
			return this.m_alertManager;
		}
	}

	// Token: 0x06004E54 RID: 20052 RVA: 0x001B65B9 File Offset: 0x001B47B9
	public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		if (!this.yellowAlertTasks.Contains(prioritizable))
		{
			this.yellowAlertTasks.Add(prioritizable);
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x06004E55 RID: 20053 RVA: 0x001B65DC File Offset: 0x001B47DC
	public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		for (int i = this.yellowAlertTasks.Count - 1; i >= 0; i--)
		{
			if (this.yellowAlertTasks[i] == prioritizable || this.yellowAlertTasks[i].Equals(null))
			{
				this.yellowAlertTasks.RemoveAt(i);
			}
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x06004E56 RID: 20054 RVA: 0x001B663B File Offset: 0x001B483B
	// (set) Token: 0x06004E57 RID: 20055 RVA: 0x001B6643 File Offset: 0x001B4843
	public int ParentWorldId { get; private set; }

	// Token: 0x06004E58 RID: 20056 RVA: 0x001B664C File Offset: 0x001B484C
	public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
	{
		Vector2 vector = new Vector2((float)this.WorldSize.x * Grid.CellSizeInMeters, (float)this.worldSize.y * Grid.CellSizeInMeters);
		Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
		Vector2 vector3 = Grid.CellToPos2D(cell);
		Quadrant[] array = new Quadrant[depth];
		Vector2 vector4 = new Vector2(vector2.x, (float)this.worldOffset.y + vector.y);
		Vector2 vector5 = new Vector2(vector2.x + vector.x, (float)this.worldOffset.y);
		for (int i = 0; i < depth; i++)
		{
			float num = vector5.x - vector4.x;
			float num2 = vector4.y - vector5.y;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NE;
			}
			if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SE;
			}
			if (vector3.x < vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SW;
			}
			if (vector3.x < vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NW;
			}
			switch (array[i])
			{
			case Quadrant.NE:
				vector4.x += num3;
				vector5.y += num4;
				break;
			case Quadrant.NW:
				vector5.x -= num3;
				vector5.y += num4;
				break;
			case Quadrant.SW:
				vector4.y -= num4;
				vector5.x -= num3;
				break;
			case Quadrant.SE:
				vector4.x += num3;
				vector4.y -= num4;
				break;
			}
		}
		return array;
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001B6879 File Offset: 0x001B4A79
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ParentWorldId = this.id;
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001B6887 File Offset: 0x001B4A87
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldInventory = base.GetComponent<WorldInventory>();
		this.materialNeeds = new Dictionary<Tag, float>();
		ClusterManager.Instance.RegisterWorldContainer(this);
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001B68B4 File Offset: 0x001B4AB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
		this.RefreshHasTopPriorityChore();
		this.UpgradeFixedTraits();
		this.RefreshFixedTraits();
		if (DlcManager.IsPureVanilla())
		{
			this.isStartWorld = true;
			this.isDupeVisited = true;
		}
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001B6904 File Offset: 0x001B4B04
	protected override void OnCleanUp()
	{
		SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
		base.OnCleanUp();
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001B6924 File Offset: 0x001B4B24
	private void UpgradeFixedTraits()
	{
		if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					160000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				},
				{
					0,
					FIXEDTRAITS.SUNLIGHT.NAME.NONE
				},
				{
					10000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
				},
				{
					20000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
				},
				{
					30000,
					FIXEDTRAITS.SUNLIGHT.NAME.LOW
				},
				{
					35000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
				},
				{
					40000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED
				},
				{
					50000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
				},
				{
					60000,
					FIXEDTRAITS.SUNLIGHT.NAME.HIGH
				},
				{
					80000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
				},
				{
					120000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
		}
		if (this.cosmicRadiationFixedTrait == null || this.cosmicRadiationFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					0,
					FIXEDTRAITS.COSMICRADIATION.NAME.NONE
				},
				{
					6,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
				},
				{
					12,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
				},
				{
					18,
					FIXEDTRAITS.COSMICRADIATION.NAME.LOW
				},
				{
					21,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
				},
				{
					25,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED
				},
				{
					31,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
				},
				{
					37,
					FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
				},
				{
					50,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
				},
				{
					75,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
		}
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001B6AC5 File Offset: 0x001B4CC5
	private void RefreshFixedTraits()
	{
		this.sunlight = this.GetSunlightValueFromFixedTrait();
		this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x001B6ADF File Offset: 0x001B4CDF
	private void RefreshHasTopPriorityChore()
	{
		if (this.AlertManager != null)
		{
			this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
		}
	}

	// Token: 0x06004E60 RID: 20064 RVA: 0x001B6B02 File Offset: 0x001B4D02
	public List<string> GetSeasonIds()
	{
		return this.m_seasonIds;
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x001B6B0A File Offset: 0x001B4D0A
	public bool IsRedAlert()
	{
		return this.m_alertManager.IsRedAlert();
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001B6B17 File Offset: 0x001B4D17
	public bool IsYellowAlert()
	{
		return this.m_alertManager.IsYellowAlert();
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001B6B24 File Offset: 0x001B4D24
	public string GetRandomName()
	{
		return GameUtil.GenerateRandomWorldName(this.nameTables);
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x001B6B31 File Offset: 0x001B4D31
	public void SetID(int id)
	{
		this.id = id;
		this.ParentWorldId = id;
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x001B6B44 File Offset: 0x001B4D44
	public void SetParentIdx(int parentIdx)
	{
		this.parentChangeArgs.lastParentId = this.ParentWorldId;
		this.parentChangeArgs.world = this;
		this.ParentWorldId = parentIdx;
		Game.Instance.Trigger(880851192, this.parentChangeArgs);
		this.parentChangeArgs.lastParentId = 255;
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06004E66 RID: 20070 RVA: 0x001B6B9A File Offset: 0x001B4D9A
	public Vector2 minimumBounds
	{
		get
		{
			return new Vector2((float)this.worldOffset.x, (float)this.worldOffset.y);
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06004E67 RID: 20071 RVA: 0x001B6BB9 File Offset: 0x001B4DB9
	public Vector2 maximumBounds
	{
		get
		{
			return new Vector2((float)(this.worldOffset.x + (this.worldSize.x - 1)), (float)(this.worldOffset.y + (this.worldSize.y - 1)));
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06004E68 RID: 20072 RVA: 0x001B6BF4 File Offset: 0x001B4DF4
	public Vector2I WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06004E69 RID: 20073 RVA: 0x001B6BFC File Offset: 0x001B4DFC
	public Vector2I WorldOffset
	{
		get
		{
			return this.worldOffset;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06004E6A RID: 20074 RVA: 0x001B6C04 File Offset: 0x001B4E04
	public bool FullyEnclosedBorder
	{
		get
		{
			return this.fullyEnclosedBorder;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06004E6B RID: 20075 RVA: 0x001B6C0C File Offset: 0x001B4E0C
	public int Height
	{
		get
		{
			return this.worldSize.y;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06004E6C RID: 20076 RVA: 0x001B6C19 File Offset: 0x001B4E19
	public int Width
	{
		get
		{
			return this.worldSize.x;
		}
	}

	// Token: 0x06004E6D RID: 20077 RVA: 0x001B6C26 File Offset: 0x001B4E26
	public void SetDiscovered(bool reveal_surface = false)
	{
		if (!this.isDiscovered)
		{
			this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
		}
		this.isDiscovered = true;
		if (reveal_surface)
		{
			this.LookAtSurface();
		}
		Game.Instance.Trigger(-521212405, this);
	}

	// Token: 0x06004E6E RID: 20078 RVA: 0x001B6C5B File Offset: 0x001B4E5B
	public void SetDupeVisited()
	{
		if (!this.isDupeVisited)
		{
			this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
			this.isDupeVisited = true;
			Game.Instance.Trigger(-434755240, this);
		}
	}

	// Token: 0x06004E6F RID: 20079 RVA: 0x001B6C87 File Offset: 0x001B4E87
	public void SetRoverLanded()
	{
		this.isRoverVisited = true;
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x001B6C90 File Offset: 0x001B4E90
	public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
	{
		this.SetID(world_id);
		this.fullyEnclosedBorder = true;
		this.worldOffset = offset;
		this.worldSize = size;
		this.isDiscovered = true;
		this.isModuleInterior = true;
		this.m_seasonIds = new List<string>();
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001B6CC8 File Offset: 0x001B4EC8
	private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
	{
		if (first == second)
		{
			return 0;
		}
		Vector2 vector = first - origin;
		Vector2 vector2 = second - origin;
		float num = Mathf.Atan2(vector.x, vector.y);
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		if (vector.sqrMagnitude >= vector2.sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001B6D34 File Offset: 0x001B4F34
	public void PlaceInteriorTemplate(string template_name, System.Action callback)
	{
		TemplateContainer template = TemplateCache.GetTemplate(template_name);
		Vector2 pos = new Vector2((float)(this.worldSize.x / 2 + this.worldOffset.x), (float)(this.worldSize.y / 2 + this.worldOffset.y));
		TemplateLoader.Stamp(template, pos, callback);
		float val = template.info.size.X / 2f;
		float val2 = template.info.size.Y / 2f;
		float num = Math.Max(val, val2);
		GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		this.overworldCell = new WorldDetailSave.OverworldCell();
		List<Vector2> list = new List<Vector2>(template.cells.Count);
		foreach (Prefab prefab in template.buildings)
		{
			if (prefab.id == "RocketWallTile")
			{
				Vector2 vector = new Vector2((float)prefab.location_x + pos.x, (float)prefab.location_y + pos.y);
				if (vector.x > pos.x)
				{
					vector.x += 0.5f;
				}
				if (vector.y > pos.y)
				{
					vector.y += 0.5f;
				}
				list.Add(vector);
			}
		}
		list.Sort((Vector2 v1, Vector2 v2) => WorldContainer.IsClockwise(v1, v2, pos));
		Polygon polygon = new Polygon(list);
		this.overworldCell.poly = polygon;
		this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
		this.overworldCell.tags = new TagSet
		{
			WorldGenTags.RocketInterior
		};
		clusterDetailSave.overworldCells.Add(this.overworldCell);
		for (int i = 0; i < this.worldSize.y; i++)
		{
			for (int j = 0; j < this.worldSize.x; j++)
			{
				Vector2I vector2I = new Vector2I(this.worldOffset.x + j, this.worldOffset.y + i);
				int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
				if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
				{
					SimMessages.ModifyCellWorldZone(num2, 14);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
				}
				else
				{
					SimMessages.ModifyCellWorldZone(num2, 7);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
				}
			}
		}
	}

	// Token: 0x06004E73 RID: 20083 RVA: 0x001B703C File Offset: 0x001B523C
	private string GetSunlightFromFixedTraits(WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (this.sunlightFixedTraits.ContainsKey(text))
			{
				return text;
			}
		}
		return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x001B70AC File Offset: 0x001B52AC
	private string GetCosmicRadiationFromFixedTraits(WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (this.cosmicRadiationFixedTraits.ContainsKey(text))
			{
				return text;
			}
		}
		return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x001B711C File Offset: 0x001B531C
	private int GetSunlightValueFromFixedTrait()
	{
		if (this.sunlightFixedTrait == null)
		{
			this.sunlightFixedTrait = FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
		}
		if (this.sunlightFixedTraits.ContainsKey(this.sunlightFixedTrait))
		{
			return this.sunlightFixedTraits[this.sunlightFixedTrait];
		}
		return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
	}

	// Token: 0x06004E76 RID: 20086 RVA: 0x001B715B File Offset: 0x001B535B
	private int GetCosmicRadiationValueFromFixedTrait()
	{
		if (this.cosmicRadiationFixedTrait == null)
		{
			this.cosmicRadiationFixedTrait = FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
		}
		if (this.cosmicRadiationFixedTraits.ContainsKey(this.cosmicRadiationFixedTrait))
		{
			return this.cosmicRadiationFixedTraits[this.cosmicRadiationFixedTrait];
		}
		return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
	}

	// Token: 0x06004E77 RID: 20087 RVA: 0x001B719C File Offset: 0x001B539C
	public void SetWorldDetails(WorldGen world)
	{
		if (world != null)
		{
			this.fullyEnclosedBorder = (world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum"));
			this.worldOffset = world.GetPosition();
			this.worldSize = world.GetSize();
			this.isDiscovered = world.isStartingWorld;
			this.isStartWorld = world.isStartingWorld;
			this.worldName = world.Settings.world.filePath;
			this.nameTables = world.Settings.world.nameTables;
			this.worldDescription = world.Settings.world.description;
			this.worldType = world.Settings.world.name;
			this.isModuleInterior = world.Settings.world.moduleInterior;
			this.m_seasonIds = new List<string>(world.Settings.world.seasons);
			this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
			this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
			this.sunlight = this.GetSunlightValueFromFixedTrait();
			this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
			this.currentCosmicIntensity = (float)this.cosmicRadiation;
			this.m_subworldNames = new List<string>();
			foreach (WeightedSubworldName weightedSubworldName in world.Settings.world.subworldFiles)
			{
				string text = weightedSubworldName.name;
				text = text.Substring(0, text.LastIndexOf('/'));
				text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
				this.m_subworldNames.Add(text);
			}
			this.m_worldTraitIds = new List<string>();
			this.m_worldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
			this.m_storyTraitIds = new List<string>();
			this.m_storyTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
			return;
		}
		this.fullyEnclosedBorder = false;
		this.worldOffset = Vector2I.zero;
		this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
		this.isDiscovered = true;
		this.isStartWorld = true;
		this.isDupeVisited = true;
		this.m_seasonIds = new List<string>
		{
			Db.Get().GameplaySeasons.MeteorShowers.Id
		};
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x001B7410 File Offset: 0x001B5610
	public bool ContainsPoint(Vector2 point)
	{
		return point.x >= (float)this.worldOffset.x && point.y >= (float)this.worldOffset.y && point.x < (float)(this.worldOffset.x + this.worldSize.x) && point.y < (float)(this.worldOffset.y + this.worldSize.y);
	}

	// Token: 0x06004E79 RID: 20089 RVA: 0x001B7488 File Offset: 0x001B5688
	public void LookAtSurface()
	{
		if (!this.IsDupeVisited)
		{
			this.RevealSurface();
		}
		Vector3? vector = this.SetSurfaceCameraPos();
		if (ClusterManager.Instance.activeWorldId == this.id && vector != null)
		{
			CameraController.Instance.SnapTo(vector.Value);
		}
	}

	// Token: 0x06004E7A RID: 20090 RVA: 0x001B74D8 File Offset: 0x001B56D8
	public void RevealSurface()
	{
		if (this.isSurfaceRevealed)
		{
			return;
		}
		this.isSurfaceRevealed = true;
		for (int i = 0; i < this.worldSize.x; i++)
		{
			for (int j = this.worldSize.y - 1; j >= 0; j--)
			{
				int cell = Grid.XYToCell(i + this.worldOffset.x, j + this.worldOffset.y);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell))
				{
					break;
				}
				GridVisibility.Reveal(i + this.worldOffset.X, j + this.worldOffset.y, 7, 1f);
			}
		}
	}

	// Token: 0x06004E7B RID: 20091 RVA: 0x001B7584 File Offset: 0x001B5784
	private Vector3? SetSurfaceCameraPos()
	{
		if (SaveGame.Instance != null)
		{
			int num = (int)this.maximumBounds.y;
			for (int i = 0; i < this.worldSize.X; i++)
			{
				for (int j = this.worldSize.y - 1; j >= 0; j--)
				{
					int num2 = j + this.worldOffset.y;
					int num3 = Grid.XYToCell(i + this.worldOffset.x, num2);
					if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3)))
					{
						num = Math.Min(num, num2);
						break;
					}
				}
			}
			int num4 = (num + this.worldOffset.y + this.worldSize.y) / 2;
			Vector3 vector = new Vector3((float)(this.WorldOffset.x + this.Width / 2), (float)num4, 0f);
			SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, vector);
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x06004E7C RID: 20092 RVA: 0x001B7698 File Offset: 0x001B5898
	public void EjectAllDupes(Vector3 spawn_pos)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			minionIdentity.transform.SetLocalPosition(spawn_pos);
		}
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001B76FC File Offset: 0x001B58FC
	public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			if (!minionIdentity.HasTag(GameTags.Dead))
			{
				Vector3 position = new Vector3(-1f, -1f, 0f);
				GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
				gameObject.GetComponent<PrimaryElement>().SetElement(podElement, true);
				gameObject.SetActive(true);
				gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
				TravellingCargoLander.StatesInstance smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
				smi.StartSM();
				smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
			}
		}
	}

	// Token: 0x06004E7E RID: 20094 RVA: 0x001B77D4 File Offset: 0x001B59D4
	public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
	{
		this.TransferBuildingMaterials(out noRefundTiles);
		foreach (ClustercraftInteriorDoor cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id, false))
		{
			cmp.DeleteObject();
		}
		this.ClearWorldZones();
	}

	// Token: 0x06004E7F RID: 20095 RVA: 0x001B783C File Offset: 0x001B5A3C
	public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
	{
		this.TransferPickupables(spawn_pos);
		this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
	}

	// Token: 0x06004E80 RID: 20096 RVA: 0x001B7850 File Offset: 0x001B5A50
	public void TransferResourcesToDebris(AxialI sourceLocation, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		List<Storage> list = new List<Storage>();
		this.TransferPickupablesToDebris(ref list, debrisContainerElement);
		this.TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
		foreach (Storage cmp in list)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
		}
	}

	// Token: 0x06004E81 RID: 20097 RVA: 0x001B78CC File Offset: 0x001B5ACC
	private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
	{
		HashSet<int> retTemplateFoundationCells = new HashSet<int>();
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);
		Action<int> <>9__0;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
			if (buildingComplete != null)
			{
				Deconstructable component = buildingComplete.GetComponent<Deconstructable>();
				if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund))
				{
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					float temperature = component2.Temperature;
					byte diseaseIdx = component2.DiseaseIdx;
					int diseaseCount = component2.DiseaseCount;
					int num = 0;
					while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num)
					{
						Element element = ElementLoader.GetElement(component.constructionElements[num]);
						if (element != null)
						{
							element.substance.SpawnResource(buildingComplete.transform.GetPosition(), buildingComplete.Def.Mass[num], temperature, diseaseIdx, diseaseCount, false, false, false);
						}
						else
						{
							GameObject prefab = Assets.GetPrefab(component.constructionElements[num]);
							int num2 = 0;
							while ((float)num2 < buildingComplete.Def.Mass[num])
							{
								GameUtil.KInstantiate(prefab, buildingComplete.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
								num2++;
							}
						}
						num++;
					}
				}
				SimCellOccupier component3 = buildingComplete.GetComponent<SimCellOccupier>();
				if (component3 != null && component3.doReplaceElement)
				{
					Building building = buildingComplete;
					Action<int> callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(int cell)
						{
							retTemplateFoundationCells.Add(cell);
						});
					}
					building.RunOnArea(callback);
				}
				Storage component4 = buildingComplete.GetComponent<Storage>();
				if (component4 != null)
				{
					component4.DropAll(false, false, default(Vector3), true, null);
				}
				PlantablePlot component5 = buildingComplete.GetComponent<PlantablePlot>();
				if (component5 != null)
				{
					SeedProducer seedProducer = (component5.Occupant != null) ? component5.Occupant.GetComponent<SeedProducer>() : null;
					if (seedProducer != null)
					{
						seedProducer.DropSeed(null);
					}
				}
				buildingComplete.DeleteObject();
			}
		}
		pooledList.Clear();
		noRefundTiles = retTemplateFoundationCells;
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x001B7B5C File Offset: 0x001B5D5C
	private void TransferPickupables(Vector3 pos)
	{
		int cell = Grid.PosToCell(pos);
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06004E83 RID: 20099 RVA: 0x001B7C28 File Offset: 0x001B5E28
	private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						element.substance.SpawnResource(pos, Grid.Mass[num3], Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, false, false);
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06004E84 RID: 20100 RVA: 0x001B7CE0 File Offset: 0x001B5EE0
	private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement)
	{
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					if (pickupable.IsPrefabID(GameTags.Minion))
					{
						global::Util.KDestroyGameObject(pickupable.gameObject);
					}
					else
					{
						pickupable.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && pickupable.PrimaryElement.Mass > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity())
						{
							Pickupable pickupable2 = pickupable.Take(storage.RemainingCapacity());
							storage.Store(pickupable2.gameObject, false, false, true, false);
							storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
							debrisObjects.Add(storage);
						}
						if (pickupable.PrimaryElement.Mass > 0f)
						{
							storage.Store(pickupable.gameObject, false, false, true, false);
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06004E85 RID: 20101 RVA: 0x001B7EB4 File Offset: 0x001B60B4
	private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						float num4 = Grid.Mass[num3];
						num4 *= 0.5f;
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && num4 > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (num4 > 0f)
						{
							float num5 = Mathf.Min(num4, storage.RemainingCapacity());
							num4 -= num5;
							storage.AddOre(element.id, num5, Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, true);
							if (num4 > 0f)
							{
								storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
								debrisObjects.Add(storage);
							}
						}
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06004E86 RID: 20102 RVA: 0x001B801C File Offset: 0x001B621C
	public void CancelChores()
	{
		for (int i = 0; i < 45; i++)
		{
			int num = (int)this.minimumBounds.x;
			while ((float)num <= this.maximumBounds.x)
			{
				int num2 = (int)this.minimumBounds.y;
				while ((float)num2 <= this.maximumBounds.y)
				{
					int cell = Grid.XYToCell(num, num2);
					GameObject gameObject = Grid.Objects[cell, i];
					if (gameObject != null)
					{
						gameObject.Trigger(2127324410, true);
					}
					num2++;
				}
				num++;
			}
		}
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out list);
		int num3 = 0;
		while (list != null && num3 < list.Count)
		{
			Chore chore = list[num3];
			if (chore != null && chore.target != null && !chore.isNull)
			{
				chore.Cancel("World destroyed");
			}
			num3++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out list2);
		int num4 = 0;
		while (list2 != null && num4 < list2.Count)
		{
			FetchChore fetchChore = list2[num4];
			if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
			{
				fetchChore.Cancel("World destroyed");
			}
			num4++;
		}
	}

	// Token: 0x06004E87 RID: 20103 RVA: 0x001B8174 File Offset: 0x001B6374
	public void ClearWorldZones()
	{
		if (this.overworldCell != null)
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			int num = -1;
			for (int i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++)
			{
				WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
				if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				clusterDetailSave.overworldCells.RemoveAt(num);
			}
		}
		int num2 = (int)this.minimumBounds.y;
		while ((float)num2 <= this.maximumBounds.y)
		{
			int num3 = (int)this.minimumBounds.x;
			while ((float)num3 <= this.maximumBounds.x)
			{
				SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
				num3++;
			}
			num2++;
		}
	}

	// Token: 0x06004E88 RID: 20104 RVA: 0x001B82AC File Offset: 0x001B64AC
	public int GetSafeCell()
	{
		if (this.IsModuleInterior)
		{
			using (List<RocketControlStation>.Enumerator enumerator = Components.RocketControlStations.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketControlStation rocketControlStation = enumerator.Current;
					if (rocketControlStation.GetMyWorldId() == this.id)
					{
						return Grid.PosToCell(rocketControlStation);
					}
				}
				goto IL_A2;
			}
		}
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			if (telepad.GetMyWorldId() == this.id)
			{
				return Grid.PosToCell(telepad);
			}
		}
		IL_A2:
		return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
	}

	// Token: 0x06004E89 RID: 20105 RVA: 0x001B83B0 File Offset: 0x001B65B0
	public string GetStatus()
	{
		return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
	}

	// Token: 0x040032FC RID: 13052
	[Serialize]
	public int id = -1;

	// Token: 0x040032FD RID: 13053
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04003300 RID: 13056
	[Serialize]
	private Vector2I worldOffset;

	// Token: 0x04003301 RID: 13057
	[Serialize]
	private Vector2I worldSize;

	// Token: 0x04003302 RID: 13058
	[Serialize]
	private bool fullyEnclosedBorder;

	// Token: 0x04003303 RID: 13059
	[Serialize]
	private bool isModuleInterior;

	// Token: 0x04003304 RID: 13060
	[Serialize]
	private WorldDetailSave.OverworldCell overworldCell;

	// Token: 0x04003305 RID: 13061
	[Serialize]
	private bool isDiscovered;

	// Token: 0x04003306 RID: 13062
	[Serialize]
	private bool isStartWorld;

	// Token: 0x04003307 RID: 13063
	[Serialize]
	private bool isDupeVisited;

	// Token: 0x04003308 RID: 13064
	[Serialize]
	private float dupeVisitedTimestamp;

	// Token: 0x04003309 RID: 13065
	[Serialize]
	private float discoveryTimestamp;

	// Token: 0x0400330A RID: 13066
	[Serialize]
	private bool isRoverVisited;

	// Token: 0x0400330B RID: 13067
	[Serialize]
	private bool isSurfaceRevealed;

	// Token: 0x0400330C RID: 13068
	[Serialize]
	public string worldName;

	// Token: 0x0400330D RID: 13069
	[Serialize]
	public string[] nameTables;

	// Token: 0x0400330E RID: 13070
	[Serialize]
	public string worldType;

	// Token: 0x0400330F RID: 13071
	[Serialize]
	public string worldDescription;

	// Token: 0x04003310 RID: 13072
	[Serialize]
	public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

	// Token: 0x04003311 RID: 13073
	[Serialize]
	public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003312 RID: 13074
	[Serialize]
	public float currentSunlightIntensity;

	// Token: 0x04003313 RID: 13075
	[Serialize]
	public float currentCosmicIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04003314 RID: 13076
	[Serialize]
	public string sunlightFixedTrait;

	// Token: 0x04003315 RID: 13077
	[Serialize]
	public string cosmicRadiationFixedTrait;

	// Token: 0x04003316 RID: 13078
	[Serialize]
	public int fixedTraitsUpdateVersion = 1;

	// Token: 0x04003317 RID: 13079
	private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.SUNLIGHT.NAME.NONE,
			FIXEDTRAITS.SUNLIGHT.NONE
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.LOW,
			FIXEDTRAITS.SUNLIGHT.LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
			FIXEDTRAITS.SUNLIGHT.MED_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED,
			FIXEDTRAITS.SUNLIGHT.MED
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
			FIXEDTRAITS.SUNLIGHT.MED_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
			FIXEDTRAITS.SUNLIGHT.HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
		}
	};

	// Token: 0x04003318 RID: 13080
	private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
			FIXEDTRAITS.COSMICRADIATION.NONE
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
			FIXEDTRAITS.COSMICRADIATION.LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
			FIXEDTRAITS.COSMICRADIATION.MED_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED,
			FIXEDTRAITS.COSMICRADIATION.MED
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
			FIXEDTRAITS.COSMICRADIATION.MED_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
			FIXEDTRAITS.COSMICRADIATION.HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
		}
	};

	// Token: 0x04003319 RID: 13081
	[Serialize]
	private List<string> m_seasonIds;

	// Token: 0x0400331A RID: 13082
	[Serialize]
	private List<string> m_subworldNames;

	// Token: 0x0400331B RID: 13083
	[Serialize]
	private List<string> m_worldTraitIds;

	// Token: 0x0400331C RID: 13084
	[Serialize]
	private List<string> m_storyTraitIds;

	// Token: 0x0400331D RID: 13085
	private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

	// Token: 0x0400331E RID: 13086
	[MySmiReq]
	private AlertStateManager.Instance m_alertManager;

	// Token: 0x0400331F RID: 13087
	private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();
}
