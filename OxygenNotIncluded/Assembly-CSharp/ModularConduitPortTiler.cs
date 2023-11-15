using System;
using UnityEngine;

// Token: 0x0200065D RID: 1629
public class ModularConduitPortTiler : KMonoBehaviour
{
	// Token: 0x06002AF3 RID: 10995 RVA: 0x000E5304 File Offset: 0x000E3504
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				GameTags.ModularConduitPort
			};
		}
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x000E5354 File Offset: 0x000E3554
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.leftCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
		if (this.manageLeftCap)
		{
			this.leftCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
			this.leftCapConduit = new KAnimSynchronizedController(component2, component2.GetLayer() + Grid.SceneLayer.Backwall, ModularConduitPortTiler.leftCapConduitStr);
		}
		this.rightCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
		if (this.manageRightCap)
		{
			this.rightCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
			this.rightCapConduit = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
		this.CorrectAdjacentLaunchPads();
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x000E54AA File Offset: 0x000E36AA
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x000E54C4 File Offset: 0x000E36C4
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellLeft = this.GetCellLeft();
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellLeft))
		{
			if (this.HasTileableNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (Grid.IsValidCell(cellRight))
		{
			if (this.HasTileableNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (this.manageLeftCap)
		{
			this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
		if (this.manageRightCap)
		{
			this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x000E55DC File Offset: 0x000E37DC
	private int GetCellLeft()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num - 1, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x000E5618 File Offset: 0x000E3818
	private int GetCellRight()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num + this.extents.width, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000E565C File Offset: 0x000E385C
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000E56A8 File Offset: 0x000E38A8
	private bool HasLaunchpadNeighbour(int neighbour_cell)
	{
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000E56E1 File Offset: 0x000E38E1
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000E5710 File Offset: 0x000E3910
	private void CorrectAdjacentLaunchPads()
	{
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
		{
			Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
		int cellLeft = this.GetCellLeft();
		if (Grid.IsValidCell(cellLeft) && this.HasLaunchpadNeighbour(cellLeft))
		{
			Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
	}

	// Token: 0x0400191D RID: 6429
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400191E RID: 6430
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x0400191F RID: 6431
	public Tag[] tags;

	// Token: 0x04001920 RID: 6432
	public bool manageLeftCap = true;

	// Token: 0x04001921 RID: 6433
	public bool manageRightCap = true;

	// Token: 0x04001922 RID: 6434
	public int leftCapDefaultSceneLayerAdjust;

	// Token: 0x04001923 RID: 6435
	public int rightCapDefaultSceneLayerAdjust;

	// Token: 0x04001924 RID: 6436
	private Extents extents;

	// Token: 0x04001925 RID: 6437
	private ModularConduitPortTiler.AnimCapType leftCapSetting;

	// Token: 0x04001926 RID: 6438
	private ModularConduitPortTiler.AnimCapType rightCapSetting;

	// Token: 0x04001927 RID: 6439
	private static readonly string leftCapDefaultStr = "#cap_left_default";

	// Token: 0x04001928 RID: 6440
	private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";

	// Token: 0x04001929 RID: 6441
	private static readonly string leftCapConduitStr = "#cap_left_conduit";

	// Token: 0x0400192A RID: 6442
	private static readonly string rightCapDefaultStr = "#cap_right_default";

	// Token: 0x0400192B RID: 6443
	private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";

	// Token: 0x0400192C RID: 6444
	private static readonly string rightCapConduitStr = "#cap_right_conduit";

	// Token: 0x0400192D RID: 6445
	private KAnimSynchronizedController leftCapDefault;

	// Token: 0x0400192E RID: 6446
	private KAnimSynchronizedController leftCapLaunchpad;

	// Token: 0x0400192F RID: 6447
	private KAnimSynchronizedController leftCapConduit;

	// Token: 0x04001930 RID: 6448
	private KAnimSynchronizedController rightCapDefault;

	// Token: 0x04001931 RID: 6449
	private KAnimSynchronizedController rightCapLaunchpad;

	// Token: 0x04001932 RID: 6450
	private KAnimSynchronizedController rightCapConduit;

	// Token: 0x02001349 RID: 4937
	private enum AnimCapType
	{
		// Token: 0x04006227 RID: 25127
		Default,
		// Token: 0x04006228 RID: 25128
		Conduit,
		// Token: 0x04006229 RID: 25129
		Launchpad
	}
}
