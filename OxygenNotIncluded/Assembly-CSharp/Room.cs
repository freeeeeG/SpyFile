using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000945 RID: 2373
public class Room : IAssignableIdentity
{
	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x0600451F RID: 17695 RVA: 0x00185053 File Offset: 0x00183253
	public List<KPrefabID> buildings
	{
		get
		{
			return this.cavity.buildings;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06004520 RID: 17696 RVA: 0x00185060 File Offset: 0x00183260
	public List<KPrefabID> plants
	{
		get
		{
			return this.cavity.plants;
		}
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x0018506D File Offset: 0x0018326D
	public string GetProperName()
	{
		return this.roomType.Name;
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x0018507C File Offset: 0x0018327C
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (KPrefabID kprefabID in this.GetPrimaryEntities())
		{
			if (kprefabID != null)
			{
				Ownable component = kprefabID.GetComponent<Ownable>();
				if (component != null && component.assignee != null && component.assignee != this)
				{
					foreach (Ownables item in component.assignee.GetOwners())
					{
						if (!this.current_owners.Contains(item))
						{
							this.current_owners.Add(item);
						}
					}
				}
			}
		}
		return this.current_owners;
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x00185168 File Offset: 0x00183368
	public List<GameObject> GetBuildingsOnFloor()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.buildings.Count; i++)
		{
			if (!Grid.Solid[Grid.PosToCell(this.buildings[i])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell(this.buildings[i]))])
			{
				list.Add(this.buildings[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x001851E8 File Offset: 0x001833E8
	public Ownables GetSoleOwner()
	{
		List<Ownables> owners = this.GetOwners();
		if (owners.Count <= 0)
		{
			return null;
		}
		return owners[0];
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x00185210 File Offset: 0x00183410
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Find((Ownables x) => x == owner) != null;
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x00185247 File Offset: 0x00183447
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x00185254 File Offset: 0x00183454
	public List<KPrefabID> GetPrimaryEntities()
	{
		this.primary_buildings.Clear();
		RoomType roomType = this.roomType;
		if (roomType.primary_constraint != null)
		{
			foreach (KPrefabID kprefabID in this.buildings)
			{
				if (kprefabID != null && roomType.primary_constraint.building_criteria(kprefabID))
				{
					this.primary_buildings.Add(kprefabID);
				}
			}
			foreach (KPrefabID kprefabID2 in this.plants)
			{
				if (kprefabID2 != null && roomType.primary_constraint.building_criteria(kprefabID2))
				{
					this.primary_buildings.Add(kprefabID2);
				}
			}
		}
		return this.primary_buildings;
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x00185350 File Offset: 0x00183550
	public void RetriggerBuildings()
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, this);
			}
		}
		foreach (KPrefabID kprefabID2 in this.plants)
		{
			if (!(kprefabID2 == null))
			{
				kprefabID2.Trigger(144050788, this);
			}
		}
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x00185404 File Offset: 0x00183604
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x00185407 File Offset: 0x00183607
	public void CleanUp()
	{
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
	}

	// Token: 0x04002DB0 RID: 11696
	public CavityInfo cavity;

	// Token: 0x04002DB1 RID: 11697
	public RoomType roomType;

	// Token: 0x04002DB2 RID: 11698
	private List<KPrefabID> primary_buildings = new List<KPrefabID>();

	// Token: 0x04002DB3 RID: 11699
	private List<Ownables> current_owners = new List<Ownables>();
}
