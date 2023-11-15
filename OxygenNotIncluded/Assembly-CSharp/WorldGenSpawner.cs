using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000A35 RID: 2613
[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
	// Token: 0x06004E9A RID: 20122 RVA: 0x001B8AE7 File Offset: 0x001B6CE7
	public bool SpawnsRemain()
	{
		return this.spawnables.Count > 0;
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x001B8AF8 File Offset: 0x001B6CF8
	public void SpawnEverything()
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			this.spawnables[i].TrySpawn();
		}
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x001B8B2C File Offset: 0x001B6D2C
	public void SpawnTag(string id)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (this.spawnables[i].spawnInfo.id == id)
			{
				this.spawnables[i].TrySpawn();
			}
		}
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x001B8B80 File Offset: 0x001B6D80
	public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
	{
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[i].cell, area))
			{
				this.spawnables[i].FreeResources();
			}
		}
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x001B8BD3 File Offset: 0x001B6DD3
	public IReadOnlyList<WorldGenSpawner.Spawnable> GetSpawnables()
	{
		return this.spawnables;
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x001B8BDC File Offset: 0x001B6DDC
	protected override void OnSpawn()
	{
		if (!this.hasPlacedTemplates)
		{
			global::Debug.Assert(SaveLoader.Instance.ClusterLayout != null, "Trying to place templates for an already-loaded save, no worldgen data available");
			this.DoReveal(SaveLoader.Instance.ClusterLayout);
			this.PlaceTemplates(SaveLoader.Instance.ClusterLayout);
			this.hasPlacedTemplates = true;
		}
		if (this.spawnInfos == null)
		{
			return;
		}
		for (int i = 0; i < this.spawnInfos.Length; i++)
		{
			this.AddSpawnable(this.spawnInfos[i]);
		}
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x001B8C5C File Offset: 0x001B6E5C
	[OnSerializing]
	private void OnSerializing()
	{
		List<Prefab> list = new List<Prefab>();
		for (int i = 0; i < this.spawnables.Count; i++)
		{
			WorldGenSpawner.Spawnable spawnable = this.spawnables[i];
			if (!spawnable.isSpawned)
			{
				list.Add(spawnable.spawnInfo);
			}
		}
		this.spawnInfos = list.ToArray();
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x001B8CB2 File Offset: 0x001B6EB2
	private void AddSpawnable(Prefab prefab)
	{
		this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x001B8CC8 File Offset: 0x001B6EC8
	public void AddLegacySpawner(Tag tag, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, vector2I.x, vector2I.y, SimHashes.Carbon, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0));
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001B8D10 File Offset: 0x001B6F10
	public List<Tag> GetUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => !match.isSpawned && (int)Grid.WorldIdx[match.cell] == worldID && Assets.GetPrefab(match.spawnInfo.id) != null && Assets.GetPrefab(match.spawnInfo.id).GetComponent<T>() != null));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001B8DAC File Offset: 0x001B6FAC
	public List<Tag> GetSpawnersWithTag(Tag tag, int worldID, bool includeSpawned = false)
	{
		List<Tag> list = new List<Tag>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => (includeSpawned || !match.isSpawned) && (int)Grid.WorldIdx[match.cell] == worldID && match.spawnInfo.id == tag));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			list.Add(spawnable.spawnInfo.id);
		}
		return list;
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001B8E58 File Offset: 0x001B7058
	public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(bool includeSpawned = false, params Tag[] tags)
	{
		List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
		List<WorldGenSpawner.Spawnable> list2 = this.spawnables;
		Predicate<WorldGenSpawner.Spawnable> <>9__0;
		Predicate<WorldGenSpawner.Spawnable> match2;
		if ((match2 = <>9__0) == null)
		{
			match2 = (<>9__0 = ((WorldGenSpawner.Spawnable match) => includeSpawned || !match.isSpawned));
		}
		foreach (WorldGenSpawner.Spawnable spawnable in list2.FindAll(match2))
		{
			foreach (Tag b in tags)
			{
				if (spawnable.spawnInfo.id == b)
				{
					list.Add(spawnable);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001B8F24 File Offset: 0x001B7124
	private void PlaceTemplates(Cluster clusterLayout)
	{
		this.spawnables = new List<WorldGenSpawner.Spawnable>();
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			foreach (Prefab prefab in worldGen.SpawnData.buildings)
			{
				prefab.location_x += worldGen.data.world.offset.x;
				prefab.location_y += worldGen.data.world.offset.y;
				prefab.type = Prefab.Type.Building;
				this.AddSpawnable(prefab);
			}
			foreach (Prefab prefab2 in worldGen.SpawnData.elementalOres)
			{
				prefab2.location_x += worldGen.data.world.offset.x;
				prefab2.location_y += worldGen.data.world.offset.y;
				prefab2.type = Prefab.Type.Ore;
				this.AddSpawnable(prefab2);
			}
			foreach (Prefab prefab3 in worldGen.SpawnData.otherEntities)
			{
				prefab3.location_x += worldGen.data.world.offset.x;
				prefab3.location_y += worldGen.data.world.offset.y;
				prefab3.type = Prefab.Type.Other;
				this.AddSpawnable(prefab3);
			}
			foreach (Prefab prefab4 in worldGen.SpawnData.pickupables)
			{
				prefab4.location_x += worldGen.data.world.offset.x;
				prefab4.location_y += worldGen.data.world.offset.y;
				prefab4.type = Prefab.Type.Pickupable;
				this.AddSpawnable(prefab4);
			}
			worldGen.SpawnData.buildings.Clear();
			worldGen.SpawnData.elementalOres.Clear();
			worldGen.SpawnData.otherEntities.Clear();
			worldGen.SpawnData.pickupables.Clear();
		}
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001B9254 File Offset: 0x001B7454
	private void DoReveal(Cluster clusterLayout)
	{
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			Game.Instance.Reset(worldGen.SpawnData, worldGen.WorldOffset);
		}
		for (int i = 0; i < Grid.CellCount; i++)
		{
			Grid.Revealed[i] = false;
			Grid.Spawnable[i] = 0;
		}
		float innerRadius = 16.5f;
		int radius = 18;
		Vector2I vector2I = clusterLayout.currentWorld.SpawnData.baseStartPos;
		vector2I += clusterLayout.currentWorld.WorldOffset;
		GridVisibility.Reveal(vector2I.x, vector2I.y, radius, innerRadius);
	}

	// Token: 0x04003329 RID: 13097
	[Serialize]
	private Prefab[] spawnInfos;

	// Token: 0x0400332A RID: 13098
	[Serialize]
	private bool hasPlacedTemplates;

	// Token: 0x0400332B RID: 13099
	private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

	// Token: 0x020018BC RID: 6332
	public class Spawnable
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060092AC RID: 37548 RVA: 0x0032CD59 File Offset: 0x0032AF59
		// (set) Token: 0x060092AD RID: 37549 RVA: 0x0032CD61 File Offset: 0x0032AF61
		public Prefab spawnInfo { get; private set; }

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060092AE RID: 37550 RVA: 0x0032CD6A File Offset: 0x0032AF6A
		// (set) Token: 0x060092AF RID: 37551 RVA: 0x0032CD72 File Offset: 0x0032AF72
		public bool isSpawned { get; private set; }

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060092B0 RID: 37552 RVA: 0x0032CD7B File Offset: 0x0032AF7B
		// (set) Token: 0x060092B1 RID: 37553 RVA: 0x0032CD83 File Offset: 0x0032AF83
		public int cell { get; private set; }

		// Token: 0x060092B2 RID: 37554 RVA: 0x0032CD8C File Offset: 0x0032AF8C
		public Spawnable(Prefab spawn_info)
		{
			this.spawnInfo = spawn_info;
			int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
			GameObject prefab = Assets.GetPrefab(spawn_info.id);
			if (prefab != null)
			{
				WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
				if (def != null && def.adjustSpawnLocationCb != null)
				{
					num = def.adjustSpawnLocationCb(num);
				}
			}
			this.cell = num;
			global::Debug.Assert(Grid.IsValidCell(this.cell));
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
				return;
			}
			this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new Action<object>(this.OnReveal));
		}

		// Token: 0x060092B3 RID: 37555 RVA: 0x0032CE5A File Offset: 0x0032B05A
		private void OnReveal(object data)
		{
			if (Grid.Spawnable[this.cell] > 0)
			{
				this.TrySpawn();
			}
		}

		// Token: 0x060092B4 RID: 37556 RVA: 0x0032CE71 File Offset: 0x0032B071
		private void OnSolidChanged(object data)
		{
			if (!Grid.Solid[this.cell])
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				this.Spawn();
			}
		}

		// Token: 0x060092B5 RID: 37557 RVA: 0x0032CEB0 File Offset: 0x0032B0B0
		public void FreeResources()
		{
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
				if (Game.Instance != null)
				{
					Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
				}
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			this.isSpawned = true;
		}

		// Token: 0x060092B6 RID: 37558 RVA: 0x0032CF14 File Offset: 0x0032B114
		public void TrySpawn()
		{
			if (this.isSpawned)
			{
				return;
			}
			if (this.solidChangedPartitionerEntry.IsValid())
			{
				return;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[this.cell]);
			bool flag = world != null && world.IsDiscovered;
			GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
			if (!(prefab != null))
			{
				if (flag)
				{
					GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
					this.Spawn();
				}
				return;
			}
			if (!(flag | prefab.HasTag(GameTags.WarpTech)))
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
			bool flag2 = false;
			if (prefab.GetComponent<Pickupable>() != null && !prefab.HasTag(GameTags.Creatures.Digger))
			{
				flag2 = true;
			}
			else if (prefab.GetDef<BurrowMonitor.Def>() != null)
			{
				flag2 = true;
			}
			if (flag2 && Grid.Solid[this.cell])
			{
				this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
				Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
				return;
			}
			this.Spawn();
		}

		// Token: 0x060092B7 RID: 37559 RVA: 0x0032D048 File Offset: 0x0032B248
		private Tag GetPrefabTag()
		{
			Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
			if (mob != null && mob.prefabName != null)
			{
				return new Tag(mob.prefabName);
			}
			return new Tag(this.spawnInfo.id);
		}

		// Token: 0x060092B8 RID: 37560 RVA: 0x0032D094 File Offset: 0x0032B294
		private void Spawn()
		{
			this.isSpawned = true;
			GameObject gameObject = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
			if (gameObject != null && gameObject)
			{
				gameObject.SetActive(true);
				gameObject.Trigger(1119167081, null);
			}
			this.FreeResources();
		}

		// Token: 0x060092B9 RID: 37561 RVA: 0x0032D0F0 File Offset: 0x0032B2F0
		public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(Prefab.Type type)
		{
			switch (type)
			{
			case Prefab.Type.Building:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
			case Prefab.Type.Ore:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
			case Prefab.Type.Pickupable:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
			case Prefab.Type.Other:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			default:
				return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
			}
		}

		// Token: 0x040072D6 RID: 29398
		private HandleVector<int>.Handle fogOfWarPartitionerEntry;

		// Token: 0x040072D7 RID: 29399
		private HandleVector<int>.Handle solidChangedPartitionerEntry;

		// Token: 0x0200220B RID: 8715
		// (Invoke) Token: 0x0600AC9D RID: 44189
		public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
	}
}
