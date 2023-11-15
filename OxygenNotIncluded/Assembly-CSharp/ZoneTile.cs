using System;
using ProcGen;
using UnityEngine;

// Token: 0x02000A37 RID: 2615
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	// Token: 0x06004EBF RID: 20159 RVA: 0x001B9B94 File Offset: 0x001B7D94
	protected override void OnSpawn()
	{
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		base.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001B9BD5 File Offset: 0x001B7DD5
	protected override void OnCleanUp()
	{
		if (!this.wasReplaced)
		{
			this.ClearZone();
		}
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001B9BE5 File Offset: 0x001B7DE5
	private void OnObjectReplaced(object data)
	{
		this.ClearZone();
		this.wasReplaced = true;
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001B9BF4 File Offset: 0x001B7DF4
	private void ClearZone()
	{
		foreach (int num in this.building.PlacementCells)
		{
			GameObject gameObject;
			if (!Grid.ObjectLayers[(int)this.building.Def.ObjectLayer].TryGetValue(num, out gameObject) || !(gameObject != base.gameObject) || !(gameObject != null) || !(gameObject.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = global::World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = (subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType);
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}

	// Token: 0x04003337 RID: 13111
	[MyCmpReq]
	public Building building;

	// Token: 0x04003338 RID: 13112
	private bool wasReplaced;

	// Token: 0x04003339 RID: 13113
	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});
}
