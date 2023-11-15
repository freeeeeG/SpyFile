using System;
using UnityEngine;

// Token: 0x0200071E RID: 1822
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06003207 RID: 12807 RVA: 0x00109C0D File Offset: 0x00107E0D
	// (set) Token: 0x06003208 RID: 12808 RVA: 0x00109C15 File Offset: 0x00107E15
	public bool Valid { get; private set; }

	// Token: 0x06003209 RID: 12809 RVA: 0x00109C20 File Offset: 0x00107E20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
		if (this.objectLayer != ObjectLayer.NumLayers)
		{
			this.objectPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnAreaChanged));
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
		this.OnAreaChanged(null);
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x00109CE8 File Offset: 0x00107EE8
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x00109D37 File Offset: 0x00107F37
	private void OnCellChange()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
		this.OnAreaChanged(null);
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x00109D76 File Offset: 0x00107F76
	public void SetSolid()
	{
		this.occupyArea.ApplyToCells = true;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x00109D84 File Offset: 0x00107F84
	private void OnAreaChanged(object obj)
	{
		this.UpdateValidity();
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x00109D8C File Offset: 0x00107F8C
	public void UpdateValidity()
	{
		bool valid = this.Valid;
		this.Valid = this.occupyArea.TestArea(Grid.PosToCell(this), this, EntityPreview.ValidTestDelegate);
		if (this.Valid)
		{
			this.animController.TintColour = Color.white;
		}
		else
		{
			this.animController.TintColour = Color.red;
		}
		if (valid != this.Valid)
		{
			base.Trigger(-1820564715, this.Valid);
		}
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x00109E10 File Offset: 0x00108010
	private static bool ValidTest(int cell, object data)
	{
		EntityPreview entityPreview = (EntityPreview)data;
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && (entityPreview.objectLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)entityPreview.objectLayer] == entityPreview.gameObject || Grid.Objects[cell, (int)entityPreview.objectLayer] == null);
	}

	// Token: 0x04001DFC RID: 7676
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04001DFD RID: 7677
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04001DFE RID: 7678
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001DFF RID: 7679
	public ObjectLayer objectLayer = ObjectLayer.NumLayers;

	// Token: 0x04001E01 RID: 7681
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04001E02 RID: 7682
	private HandleVector<int>.Handle objectPartitionerEntry;

	// Token: 0x04001E03 RID: 7683
	private static readonly Func<int, object, bool> ValidTestDelegate = (int cell, object data) => EntityPreview.ValidTest(cell, data);
}
