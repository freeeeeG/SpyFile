using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConfigManager")]
public class BuildingConfigManager : KMonoBehaviour
{
	// Token: 0x06002451 RID: 9297 RVA: 0x000C5FFC File Offset: 0x000C41FC
	protected override void OnPrefabInit()
	{
		BuildingConfigManager.Instance = this;
		this.baseTemplate = new GameObject("BuildingTemplate");
		this.baseTemplate.SetActive(false);
		this.baseTemplate.AddComponent<KPrefabID>();
		this.baseTemplate.AddComponent<KSelectable>();
		this.baseTemplate.AddComponent<Modifiers>();
		this.baseTemplate.AddComponent<PrimaryElement>();
		this.baseTemplate.AddComponent<BuildingComplete>();
		this.baseTemplate.AddComponent<StateMachineController>();
		this.baseTemplate.AddComponent<Deconstructable>();
		this.baseTemplate.AddComponent<SaveLoadRoot>();
		this.baseTemplate.AddComponent<OccupyArea>();
		this.baseTemplate.AddComponent<DecorProvider>();
		this.baseTemplate.AddComponent<Operational>();
		this.baseTemplate.AddComponent<BuildingEnabledButton>();
		this.baseTemplate.AddComponent<Prioritizable>();
		this.baseTemplate.AddComponent<BuildingHP>();
		this.baseTemplate.AddComponent<LoopingSounds>();
		this.baseTemplate.AddComponent<InvalidPortReporter>();
		this.defaultBuildingCompleteKComponents.Add(typeof(RequiresFoundation));
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000C6101 File Offset: 0x000C4301
	public static string GetUnderConstructionName(string name)
	{
		return name + "UnderConstruction";
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000C6110 File Offset: 0x000C4310
	public void RegisterBuilding(IBuildingConfig config)
	{
		if (!DlcManager.IsDlcListValidForCurrentContent(config.GetDlcIds()))
		{
			return;
		}
		BuildingDef buildingDef = config.CreateBuildingDef();
		buildingDef.RequiredDlcIds = config.GetDlcIds();
		this.configTable[config] = buildingDef;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.baseTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
		gameObject.name = buildingDef.PrefabID + "Template";
		gameObject.GetComponent<Building>().Def = buildingDef;
		gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);
		if (buildingDef.Deprecated)
		{
			gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		}
		config.ConfigureBuildingTemplate(gameObject, buildingDef.Tag);
		buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);
		bool flag = true;
		for (int i = 0; i < this.NonBuildableBuildings.Length; i++)
		{
			if (buildingDef.PrefabID == this.NonBuildableBuildings[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
			buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
			buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
			GameObject buildingPreview = buildingDef.BuildingPreview;
			buildingPreview.name += "Preview";
		}
		buildingDef.PostProcess();
		config.DoPostConfigureComplete(buildingDef.BuildingComplete);
		if (flag)
		{
			config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
			config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
		}
		Assets.AddBuildingDef(buildingDef);
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x000C6298 File Offset: 0x000C4498
	public void ConfigurePost()
	{
		foreach (KeyValuePair<IBuildingConfig, BuildingDef> keyValuePair in this.configTable)
		{
			keyValuePair.Key.ConfigurePost(keyValuePair.Value);
		}
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x000C62F8 File Offset: 0x000C44F8
	public void IgnoreDefaultKComponent(Type type_to_ignore, Tag building_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.ignoredDefaultKComponents.TryGetValue(type_to_ignore, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.ignoredDefaultKComponents[type_to_ignore] = hashSet;
		}
		hashSet.Add(building_tag);
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x000C6330 File Offset: 0x000C4530
	private bool IsIgnoredDefaultKComponent(Tag building_tag, Type type)
	{
		bool result = false;
		HashSet<Tag> hashSet;
		if (this.ignoredDefaultKComponents.TryGetValue(type, out hashSet) && hashSet.Contains(building_tag))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x000C635C File Offset: 0x000C455C
	public void AddBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Add(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Add(go);
			}
		}
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x000C6410 File Offset: 0x000C4610
	public void DestroyBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Remove(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Remove(go);
			}
		}
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000C64C4 File Offset: 0x000C46C4
	public void AddDefaultBuildingCompleteKComponent(Type kcomponent_type)
	{
		this.defaultKComponents.Add(kcomponent_type);
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x000C64D4 File Offset: 0x000C46D4
	public void AddBuildingCompleteKComponent(Tag prefab_tag, Type kcomponent_type)
	{
		HashSet<Type> hashSet;
		if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			hashSet = new HashSet<Type>();
			this.buildingCompleteKComponents[prefab_tag] = hashSet;
		}
		hashSet.Add(kcomponent_type);
	}

	// Token: 0x040014D4 RID: 5332
	public static BuildingConfigManager Instance;

	// Token: 0x040014D5 RID: 5333
	private GameObject baseTemplate;

	// Token: 0x040014D6 RID: 5334
	private Dictionary<IBuildingConfig, BuildingDef> configTable = new Dictionary<IBuildingConfig, BuildingDef>();

	// Token: 0x040014D7 RID: 5335
	private string[] NonBuildableBuildings = new string[]
	{
		"Headquarters"
	};

	// Token: 0x040014D8 RID: 5336
	private HashSet<Type> defaultKComponents = new HashSet<Type>();

	// Token: 0x040014D9 RID: 5337
	private HashSet<Type> defaultBuildingCompleteKComponents = new HashSet<Type>();

	// Token: 0x040014DA RID: 5338
	private Dictionary<Type, HashSet<Tag>> ignoredDefaultKComponents = new Dictionary<Type, HashSet<Tag>>();

	// Token: 0x040014DB RID: 5339
	private Dictionary<Tag, HashSet<Type>> buildingCompleteKComponents = new Dictionary<Tag, HashSet<Type>>();
}
