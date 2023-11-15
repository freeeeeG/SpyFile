using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050A RID: 1290
[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
[Obsolete("Deprecated, use RangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
	// Token: 0x06001E57 RID: 7767 RVA: 0x000A1ED0 File Offset: 0x000A00D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
		if (this.movable)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
			base.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
		}
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x000A1F30 File Offset: 0x000A0130
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
		this.ClearVisualizers();
		base.OnCleanUp();
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x000A1F88 File Offset: 0x000A0188
	private void OnSelect(object data)
	{
		if ((bool)data)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), base.transform.position, 1f);
			this.UpdateVisualizers();
			return;
		}
		SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), base.transform.position, 1f);
		this.ClearVisualizers();
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x000A1FEC File Offset: 0x000A01EC
	private void OnRotated(object data)
	{
		this.UpdateVisualizers();
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x000A1FF4 File Offset: 0x000A01F4
	private void OnCellChange()
	{
		this.UpdateVisualizers();
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000A1FFC File Offset: 0x000A01FC
	private void UpdateVisualizers()
	{
		this.newCells.Clear();
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out num, out num2);
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num3))
				{
					int x;
					int y;
					Grid.CellToXY(num3, out x, out y);
					if (Grid.TestLineOfSight(num, num2, x, y, this.blocking_cb, this.blocking_tile_visible, false))
					{
						this.newCells.Add(num3);
					}
				}
			}
		}
		for (int k = this.visualizers.Count - 1; k >= 0; k--)
		{
			if (this.newCells.Contains(this.visualizers[k].cell))
			{
				this.newCells.Remove(this.visualizers[k].cell);
			}
			else
			{
				this.DestroyEffect(this.visualizers[k].controller);
				this.visualizers.RemoveAt(k);
			}
		}
		for (int l = 0; l < this.newCells.Count; l++)
		{
			KBatchedAnimController controller = this.CreateEffect(this.newCells[l]);
			this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData
			{
				cell = this.newCells[l],
				controller = controller
			});
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x000A21EC File Offset: 0x000A03EC
	private void ClearVisualizers()
	{
		for (int i = 0; i < this.visualizers.Count; i++)
		{
			this.DestroyEffect(this.visualizers[i].controller);
		}
		this.visualizers.Clear();
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x000A2234 File Offset: 0x000A0434
	private KBatchedAnimController CreateEffect(int cell)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), null, false, this.sceneLayer, true);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
		return kbatchedAnimController;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x000A2286 File Offset: 0x000A0486
	private void DestroyEffect(KBatchedAnimController controller)
	{
		controller.destroyOnAnimComplete = true;
		controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001106 RID: 4358
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001107 RID: 4359
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001108 RID: 4360
	public int x;

	// Token: 0x04001109 RID: 4361
	public int y;

	// Token: 0x0400110A RID: 4362
	public int width;

	// Token: 0x0400110B RID: 4363
	public int height;

	// Token: 0x0400110C RID: 4364
	public bool movable;

	// Token: 0x0400110D RID: 4365
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;

	// Token: 0x0400110E RID: 4366
	public CellOffset vision_offset;

	// Token: 0x0400110F RID: 4367
	public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);

	// Token: 0x04001110 RID: 4368
	public bool blocking_tile_visible = true;

	// Token: 0x04001111 RID: 4369
	private static readonly string AnimName = "transferarmgrid_kanim";

	// Token: 0x04001112 RID: 4370
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x04001113 RID: 4371
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x04001114 RID: 4372
	private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();

	// Token: 0x04001115 RID: 4373
	private List<int> newCells = new List<int>();

	// Token: 0x04001116 RID: 4374
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnSelect(data);
	});

	// Token: 0x04001117 RID: 4375
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnRotated(data);
	});

	// Token: 0x020011B0 RID: 4528
	private struct VisData
	{
		// Token: 0x04005D3C RID: 23868
		public int cell;

		// Token: 0x04005D3D RID: 23869
		public KBatchedAnimController controller;
	}
}
