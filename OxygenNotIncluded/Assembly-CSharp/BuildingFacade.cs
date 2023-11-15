using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x020005C4 RID: 1476
[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x0600245C RID: 9308 RVA: 0x000C656A File Offset: 0x000C476A
	public string CurrentFacade
	{
		get
		{
			return this.currentFacade;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600245D RID: 9309 RVA: 0x000C6572 File Offset: 0x000C4772
	public bool IsOriginal
	{
		get
		{
			return this.currentFacade.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x0600245E RID: 9310 RVA: 0x000C657F File Offset: 0x000C477F
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x0600245F RID: 9311 RVA: 0x000C6581 File Offset: 0x000C4781
	protected override void OnSpawn()
	{
		if (!this.IsOriginal)
		{
			this.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(this.currentFacade));
		}
	}

	// Token: 0x06002460 RID: 9312 RVA: 0x000C65A4 File Offset: 0x000C47A4
	public void ApplyBuildingFacade(BuildingFacadeResource facade)
	{
		if (facade == null)
		{
			this.ClearFacade();
			return;
		}
		this.currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[]
		{
			Assets.GetAnim(facade.AnimFile)
		};
		this.ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile);
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x000C65FC File Offset: 0x000C47FC
	private void ClearFacade()
	{
		Building component = base.GetComponent<Building>();
		this.ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null);
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x000C6638 File Offset: 0x000C4838
	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null)
	{
		this.interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			this.interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> keyValuePair in interactAnimsNames)
			{
				this.interactAnims.Add(keyValuePair.Key, new KAnimFile[]
				{
					Assets.GetAnim(keyValuePair.Value)
				});
			}
		}
		Building[] components = base.GetComponents<Building>();
		foreach (Building building in components)
		{
			building.SetDescription(desc);
			building.GetComponent<KBatchedAnimController>().SwapAnims(animFiles);
		}
		base.GetComponent<KSelectable>().SetName(displayName);
		if (base.GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x000C6740 File Offset: 0x000C4940
	public string GetNextFacade()
	{
		BuildingDef def = base.GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == this.currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}

	// Token: 0x040014DC RID: 5340
	[Serialize]
	private string currentFacade;

	// Token: 0x040014DD RID: 5341
	public KAnimFile[] animFiles;

	// Token: 0x040014DE RID: 5342
	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();
}
