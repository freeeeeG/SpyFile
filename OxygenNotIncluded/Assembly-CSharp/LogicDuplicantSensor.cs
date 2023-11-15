using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200062D RID: 1581
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDuplicantSensor : Switch, ISim1000ms, ISim200ms
{
	// Token: 0x06002858 RID: 10328 RVA: 0x000DA534 File Offset: 0x000D8734
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000DA544 File Offset: 0x000D8744
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.RefreshReachableCells();
		this.wasOn = this.switchedOn;
		Vector2I vector2I = Grid.CellToXY(this.NaturalBuildingCell());
		int cell = Grid.XYToCell(vector2I.x, vector2I.y + this.pickupRange / 2);
		CellOffset rotatedCellOffset = new CellOffset(0, this.pickupRange / 2);
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
			if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
			{
				cell = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
			}
		}
		this.pickupableExtents = new Extents(cell, this.pickupRange / 2);
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("DuplicantSensor.PickupablesChanged", base.gameObject, this.pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.pickupablesDirty = true;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x000DA646 File Offset: 0x000D8846
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x000DA66A File Offset: 0x000D886A
	public void Sim1000ms(float dt)
	{
		this.RefreshReachableCells();
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000DA672 File Offset: 0x000D8872
	public void Sim200ms(float dt)
	{
		this.RefreshPickupables();
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000DA67C File Offset: 0x000D887C
	private void RefreshReachableCells()
	{
		ListPool<int, LogicDuplicantSensor>.PooledList pooledList = ListPool<int, LogicDuplicantSensor>.Allocate(this.reachableCells);
		this.reachableCells.Clear();
		int num;
		int num2;
		Grid.CellToXY(this.NaturalBuildingCell(), out num, out num2);
		int num3 = num - this.pickupRange / 2;
		for (int i = num2; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num3; j < num3 + this.pickupRange + 1; j++)
			{
				int num4 = Grid.XYToCell(j, i);
				CellOffset rotatedCellOffset = new CellOffset(j - num, i - num2);
				if (this.rotatable)
				{
					rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
					if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
					{
						num4 = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
						Vector2I vector2I = Grid.CellToXY(num4);
						if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, vector2I.x, vector2I.y, true))
						{
							this.reachableCells.Add(num4);
						}
					}
				}
				else if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					this.reachableCells.Add(num4);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x000DA7AF File Offset: 0x000D89AF
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x000DA7C0 File Offset: 0x000D89C0
	private void RefreshPickupables()
	{
		if (!this.pickupablesDirty)
		{
			return;
		}
		this.duplicants.Clear();
		ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(this.pickupableExtents.x, this.pickupableExtents.y, this.pickupableExtents.width, this.pickupableExtents.height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		int cell_a = Grid.PosToCell(this);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			int pickupableCell = this.GetPickupableCell(pickupable);
			int cellRange = Grid.GetCellRange(cell_a, pickupableCell);
			if (this.IsPickupableRelevantToMyInterestsAndReachable(pickupable) && cellRange <= this.pickupRange)
			{
				this.duplicants.Add(pickupable);
			}
		}
		this.SetState(this.duplicants.Count > 0);
		this.pickupablesDirty = false;
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x000DA8A0 File Offset: 0x000D8AA0
	private void OnPickupablesChanged(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable && this.IsPickupableRelevantToMyInterests(pickupable))
		{
			this.pickupablesDirty = true;
		}
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000DA8CC File Offset: 0x000D8ACC
	private bool IsPickupableRelevantToMyInterests(Pickupable pickupable)
	{
		return pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x000DA8E4 File Offset: 0x000D8AE4
	private bool IsPickupableRelevantToMyInterestsAndReachable(Pickupable pickupable)
	{
		if (!this.IsPickupableRelevantToMyInterests(pickupable))
		{
			return false;
		}
		int pickupableCell = this.GetPickupableCell(pickupable);
		return this.IsCellReachable(pickupableCell);
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000DA910 File Offset: 0x000D8B10
	private int GetPickupableCell(Pickupable pickupable)
	{
		return pickupable.cachedCell;
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000DA918 File Offset: 0x000D8B18
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000DA927 File Offset: 0x000D8B27
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000DA948 File Offset: 0x000D8B48
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000DA9D0 File Offset: 0x000D8BD0
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001770 RID: 6000
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001771 RID: 6001
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001772 RID: 6002
	public int pickupRange = 4;

	// Token: 0x04001773 RID: 6003
	private bool wasOn;

	// Token: 0x04001774 RID: 6004
	private List<Pickupable> duplicants = new List<Pickupable>();

	// Token: 0x04001775 RID: 6005
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001776 RID: 6006
	private bool pickupablesDirty;

	// Token: 0x04001777 RID: 6007
	private Extents pickupableExtents;

	// Token: 0x04001778 RID: 6008
	private List<int> reachableCells = new List<int>(100);
}
