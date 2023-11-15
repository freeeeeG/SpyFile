using System;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06001B58 RID: 7000 RVA: 0x00093149 File Offset: 0x00091349
	public bool IsFlooded
	{
		get
		{
			return this.isFlooded;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06001B59 RID: 7001 RVA: 0x00093151 File Offset: 0x00091351
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x00093160 File Offset: 0x00091360
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000931B8 File Offset: 0x000913B8
	private void OnElementChanged(object data)
	{
		bool flag = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (Grid.IsSubstantialLiquid(this.building.PlacementCells[i], 0.35f))
			{
				flag = true;
				break;
			}
		}
		if (flag != this.isFlooded)
		{
			this.isFlooded = flag;
			this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, this);
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x00093247 File Offset: 0x00091447
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04000F3F RID: 3903
	[MyCmpReq]
	private Building building;

	// Token: 0x04000F40 RID: 3904
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04000F41 RID: 3905
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x04000F42 RID: 3906
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04000F43 RID: 3907
	public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);

	// Token: 0x04000F44 RID: 3908
	private bool isFlooded;

	// Token: 0x04000F45 RID: 3909
	private HandleVector<int>.Handle partitionerEntry;
}
