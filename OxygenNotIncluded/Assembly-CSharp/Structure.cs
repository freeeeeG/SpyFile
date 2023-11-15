using System;
using UnityEngine;

// Token: 0x02000510 RID: 1296
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
	// Token: 0x06001F12 RID: 7954 RVA: 0x000A6446 File Offset: 0x000A4646
	public bool IsEntombed()
	{
		return this.isEntombed;
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000A6450 File Offset: 0x000A4650
	public static bool IsBuildingEntombed(Building building)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(building)))
		{
			return false;
		}
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			int num = building.PlacementCells[i];
			if (Grid.Element[num].IsSolid && !Grid.Foundation[num])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x000A64A8 File Offset: 0x000A46A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Extents extents = this.building.GetExtents();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		base.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x000A6511 File Offset: 0x000A4711
	public void UpdatePosition()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x000A652E File Offset: 0x000A472E
	private void RocketChanged(object data)
	{
		this.OnSolidChanged(data);
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x000A6538 File Offset: 0x000A4738
	private void OnSolidChanged(object data)
	{
		bool flag = Structure.IsBuildingEntombed(this.building);
		if (flag != this.isEntombed)
		{
			this.isEntombed = flag;
			if (this.isEntombed)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			}
			this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, this);
			base.Trigger(-1089732772, null);
		}
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x000A65D3 File Offset: 0x000A47D3
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0400117D RID: 4477
	[MyCmpReq]
	private Building building;

	// Token: 0x0400117E RID: 4478
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400117F RID: 4479
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001180 RID: 4480
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x04001181 RID: 4481
	private bool isEntombed;

	// Token: 0x04001182 RID: 4482
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001183 RID: 4483
	private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>(delegate(Structure cmp, object data)
	{
		cmp.RocketChanged(data);
	});
}
