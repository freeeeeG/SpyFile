using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000AD8 RID: 2776
public class ColonyDestinationAsteroidBeltData
{
	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06005563 RID: 21859 RVA: 0x001F1152 File Offset: 0x001EF352
	// (set) Token: 0x06005564 RID: 21860 RVA: 0x001F115A File Offset: 0x001EF35A
	public float TargetScale { get; set; }

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06005565 RID: 21861 RVA: 0x001F1163 File Offset: 0x001EF363
	// (set) Token: 0x06005566 RID: 21862 RVA: 0x001F116B File Offset: 0x001EF36B
	public float Scale { get; set; }

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06005567 RID: 21863 RVA: 0x001F1174 File Offset: 0x001EF374
	// (set) Token: 0x06005568 RID: 21864 RVA: 0x001F117C File Offset: 0x001EF37C
	public int seed { get; private set; }

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06005569 RID: 21865 RVA: 0x001F1185 File Offset: 0x001EF385
	public string startWorldPath
	{
		get
		{
			return this.startWorld.filePath;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x0600556A RID: 21866 RVA: 0x001F1192 File Offset: 0x001EF392
	// (set) Token: 0x0600556B RID: 21867 RVA: 0x001F119A File Offset: 0x001EF39A
	public Sprite sprite { get; private set; }

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x0600556C RID: 21868 RVA: 0x001F11A3 File Offset: 0x001EF3A3
	// (set) Token: 0x0600556D RID: 21869 RVA: 0x001F11AB File Offset: 0x001EF3AB
	public int difficulty { get; private set; }

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x0600556E RID: 21870 RVA: 0x001F11B4 File Offset: 0x001EF3B4
	public string startWorldName
	{
		get
		{
			return Strings.Get(this.startWorld.name);
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x0600556F RID: 21871 RVA: 0x001F11CB File Offset: 0x001EF3CB
	public string properName
	{
		get
		{
			if (this.cluster == null)
			{
				return "";
			}
			return this.cluster.name;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06005570 RID: 21872 RVA: 0x001F11E6 File Offset: 0x001EF3E6
	public string beltPath
	{
		get
		{
			if (this.cluster == null)
			{
				return WorldGenSettings.ClusterDefaultName;
			}
			return this.cluster.filePath;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06005571 RID: 21873 RVA: 0x001F1201 File Offset: 0x001EF401
	// (set) Token: 0x06005572 RID: 21874 RVA: 0x001F1209 File Offset: 0x001EF409
	public List<ProcGen.World> worlds { get; private set; }

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06005573 RID: 21875 RVA: 0x001F1212 File Offset: 0x001EF412
	public ClusterLayout Layout
	{
		get
		{
			return this.cluster;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06005574 RID: 21876 RVA: 0x001F121A File Offset: 0x001EF41A
	public ProcGen.World GetStartWorld
	{
		get
		{
			return this.startWorld;
		}
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x001F1224 File Offset: 0x001EF424
	public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
	{
		this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
		this.Scale = (this.TargetScale = this.startWorld.iconScale);
		this.worlds = new List<ProcGen.World>();
		if (clusterPath != null)
		{
			this.cluster = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
			for (int i = 0; i < this.cluster.worldPlacements.Count; i++)
			{
				if (i != this.cluster.startWorldIndex)
				{
					this.worlds.Add(SettingsCache.worlds.GetWorldData(this.cluster.worldPlacements[i].world));
				}
			}
		}
		this.ReInitialize(seed);
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x001F12F4 File Offset: 0x001EF4F4
	public static Sprite GetUISprite(string filename)
	{
		if (filename.IsNullOrWhiteSpace())
		{
			filename = (DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone");
		}
		KAnimFile kanimFile;
		Assets.TryGetAnim(filename, out kanimFile);
		if (kanimFile != null)
		{
			return Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "");
		}
		return Assets.GetSprite(filename);
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001F1354 File Offset: 0x001EF554
	public void ReInitialize(int seed)
	{
		this.seed = seed;
		this.paramDescriptors.Clear();
		this.traitDescriptors.Clear();
		this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
		this.difficulty = this.cluster.difficulty;
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001F13A5 File Offset: 0x001EF5A5
	public List<AsteroidDescriptor> GetParamDescriptors()
	{
		if (this.paramDescriptors.Count == 0)
		{
			this.paramDescriptors = this.GenerateParamDescriptors();
		}
		return this.paramDescriptors;
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x001F13C6 File Offset: 0x001EF5C6
	public List<AsteroidDescriptor> GetTraitDescriptors()
	{
		if (this.traitDescriptors.Count == 0)
		{
			this.traitDescriptors = this.GenerateTraitDescriptors();
		}
		return this.traitDescriptors;
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x001F13E8 File Offset: 0x001EF5E8
	private List<AsteroidDescriptor> GenerateParamDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		if (this.cluster != null && DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, Strings.Get(this.cluster.name)), Strings.Get(this.cluster.description), Color.white, null, null));
		}
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.PLANETNAME, this.startWorldName), null, Color.white, null, null));
		list.Add(new AsteroidDescriptor(Strings.Get(this.startWorld.description), null, Color.white, null, null));
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.MOONNAMES, Array.Empty<object>()), null, Color.white, null, null));
			foreach (ProcGen.World world in this.worlds)
			{
				list.Add(new AsteroidDescriptor(string.Format("{0}", Strings.Get(world.name)), Strings.Get(world.description), Color.white, null, null));
			}
		}
		int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third), null, Color.white, null, null));
		return list;
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x001F15A0 File Offset: 0x001EF7A0
	private List<AsteroidDescriptor> GenerateTraitDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			ProcGen.World world = list2[i];
			if (DlcManager.IsExpansion1Active())
			{
				list.Add(new AsteroidDescriptor("", null, Color.white, null, null));
				list.Add(new AsteroidDescriptor(string.Format("<b>{0}</b>", Strings.Get(world.name)), null, Color.white, null, null));
			}
			List<WorldTrait> worldTraits = this.GetWorldTraits(world);
			foreach (WorldTrait worldTrait in worldTraits)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
			}
			if (worldTraits.Count == 0)
			{
				list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
			}
		}
		return list;
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x001F171C File Offset: 0x001EF91C
	public List<AsteroidDescriptor> GenerateTraitDescriptors(ProcGen.World singleWorld, bool includeDefaultTrait = true)
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World singleWorld2 = list2[i];
				List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld2);
				foreach (WorldTrait worldTrait in worldTraits)
				{
					string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
					list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
				}
				if (worldTraits.Count == 0 && includeDefaultTrait)
				{
					list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
				}
			}
		}
		return list;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x001F1864 File Offset: 0x001EFA64
	public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
	{
		List<WorldTrait> list = new List<WorldTrait>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World world = list2[i];
				int num = this.seed;
				if (num > 0)
				{
					num += this.cluster.worldPlacements.FindIndex((WorldPlacement x) => x.world == world.filePath);
				}
				foreach (string name in SettingsCache.GetRandomTraits(num, world))
				{
					WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(name, true);
					list.Add(cachedWorldTrait);
				}
			}
		}
		return list;
	}

	// Token: 0x040038FC RID: 14588
	private ProcGen.World startWorld;

	// Token: 0x040038FD RID: 14589
	private ClusterLayout cluster;

	// Token: 0x040038FE RID: 14590
	private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x040038FF RID: 14591
	private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x04003900 RID: 14592
	public static List<global::Tuple<string, string, string>> survivalOptions = new List<global::Tuple<string, string, string>>
	{
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
	};
}
