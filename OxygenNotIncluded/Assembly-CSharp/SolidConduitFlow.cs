using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

// Token: 0x02000982 RID: 2434
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitFlow : IConduitFlow
{
	// Token: 0x060047BF RID: 18367 RVA: 0x00194A51 File Offset: 0x00192C51
	public SolidConduitFlow.SOAInfo GetSOAInfo()
	{
		return this.soaInfo;
	}

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060047C0 RID: 18368 RVA: 0x00194A5C File Offset: 0x00192C5C
	// (remove) Token: 0x060047C1 RID: 18369 RVA: 0x00194A94 File Offset: 0x00192C94
	public event System.Action onConduitsRebuilt;

	// Token: 0x060047C2 RID: 18370 RVA: 0x00194ACC File Offset: 0x00192CCC
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new SolidConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x00194B04 File Offset: 0x00192D04
	public void RemoveConduitUpdater(Action<float> callback)
	{
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			if (this.conduitUpdaters[i].callback == callback)
			{
				this.conduitUpdaters.RemoveAt(i);
				this.dirtyConduitUpdaters = true;
				return;
			}
		}
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x00194B54 File Offset: 0x00192D54
	public static int FlowBit(SolidConduitFlow.FlowDirection direction)
	{
		return 1 << direction - SolidConduitFlow.FlowDirection.Left;
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x00194B60 File Offset: 0x00192D60
	public SolidConduitFlow(int num_cells, IUtilityNetworkMgr network_mgr, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.networkMgr = network_mgr;
		this.maskedOverlayLayer = LayerMask.NameToLayer("MaskedOverlay");
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x060047C6 RID: 18374 RVA: 0x00194C10 File Offset: 0x00192E10
	public void Initialize(int num_cells)
	{
		this.grid = new SolidConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.pickupableHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x00194C64 File Offset: 0x00192E64
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			FlowUtilityNetwork network = (FlowUtilityNetwork)utilityNetwork;
			this.ScanNetworkSources(network);
		}
		this.RefreshPaths();
	}

	// Token: 0x060047C8 RID: 18376 RVA: 0x00194CC0 File Offset: 0x00192EC0
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		this.soaInfo.Clear(this);
		this.pathList.Clear();
		ObjectLayer layer = ObjectLayer.SolidConduit;
		foreach (int num in root_nodes)
		{
			if (this.replacements.Contains(num))
			{
				this.replacements.Remove(num);
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
				this.grid[num].conduitIdx = conduitIdx;
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		foreach (int num2 in root_nodes)
		{
			UtilityConnections connections = this.networkMgr.GetConnections(num2, true);
			if (connections != (UtilityConnections)0 && this.grid[num2].conduitIdx != -1)
			{
				int conduitIdx2 = this.grid[num2].conduitIdx;
				SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduitIdx2);
				int num3 = num2 - 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					conduitConnections.left = this.grid[num3].conduitIdx;
				}
				num3 = num2 + 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					conduitConnections.right = this.grid[num3].conduitIdx;
				}
				num3 = num2 - Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					conduitConnections.down = this.grid[num3].conduitIdx;
				}
				num3 = num2 + Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					conduitConnections.up = this.grid[num3].conduitIdx;
				}
				this.soaInfo.SetConduitConnections(conduitIdx2, conduitConnections);
			}
		}
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x060047C9 RID: 18377 RVA: 0x00194F08 File Offset: 0x00193108
	public void ScanNetworkSources(FlowUtilityNetwork network)
	{
		if (network == null)
		{
			return;
		}
		for (int i = 0; i < network.sources.Count; i++)
		{
			FlowUtilityNetwork.IItem item = network.sources[i];
			this.path.Clear();
			this.visited.Clear();
			this.FindSinks(i, item.Cell);
		}
	}

	// Token: 0x060047CA RID: 18378 RVA: 0x00194F60 File Offset: 0x00193160
	public void RefreshPaths()
	{
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				SolidConduitFlow.Conduit conduit = list[i];
				SolidConduitFlow.Conduit target_conduit = list[i + 1];
				if (conduit.GetTargetFlowDirection(this) == SolidConduitFlow.FlowDirection.None)
				{
					SolidConduitFlow.FlowDirection direction = this.GetDirection(conduit, target_conduit);
					conduit.SetTargetFlowDirection(direction, this);
				}
			}
		}
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x00194FF4 File Offset: 0x001931F4
	private void FindSinks(int source_idx, int cell)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.FindSinksInternal(source_idx, gridNode.conduitIdx);
		}
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x00195024 File Offset: 0x00193224
	private void FindSinksInternal(int source_idx, int conduit_idx)
	{
		if (this.visited.Contains(conduit_idx))
		{
			return;
		}
		this.visited.Add(conduit_idx);
		SolidConduitFlow.Conduit conduit = this.soaInfo.GetConduit(conduit_idx);
		if (conduit.GetPermittedFlowDirections(this) == -1)
		{
			return;
		}
		this.path.Add(conduit);
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)this.networkMgr.GetEndpoint(this.soaInfo.GetCell(conduit_idx));
		if (item != null && item.EndpointType == Endpoint.Sink)
		{
			this.FoundSink(source_idx);
		}
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit_idx);
		if (conduitConnections.down != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.down);
		}
		if (conduitConnections.left != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.left);
		}
		if (conduitConnections.right != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.right);
		}
		if (conduitConnections.up != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.up);
		}
		if (this.path.Count > 0)
		{
			this.path.RemoveAt(this.path.Count - 1);
		}
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x00195130 File Offset: 0x00193330
	private SolidConduitFlow.FlowDirection GetDirection(SolidConduitFlow.Conduit conduit, SolidConduitFlow.Conduit target_conduit)
	{
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Right;
		}
		return SolidConduitFlow.FlowDirection.None;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x00195190 File Offset: 0x00193390
	private void FoundSink(int source_idx)
	{
		for (int i = 0; i < this.path.Count - 1; i++)
		{
			SolidConduitFlow.FlowDirection direction = this.GetDirection(this.path[i], this.path[i + 1]);
			SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(direction);
			int cellFromDirection = SolidConduitFlow.GetCellFromDirection(this.soaInfo.GetCell(this.path[i].idx), direction2);
			SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(this.path[i].idx, direction2);
			if (i == 0 || (this.path[i].GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0 || (cellFromDirection != this.soaInfo.GetCell(this.path[i - 1].idx) && (this.soaInfo.GetSrcFlowIdx(this.path[i].idx) == source_idx || (conduitFromDirection.GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0)))
			{
				int permittedFlowDirections = this.path[i].GetPermittedFlowDirections(this);
				this.soaInfo.SetSrcFlowIdx(this.path[i].idx, source_idx);
				this.path[i].SetPermittedFlowDirections(permittedFlowDirections | SolidConduitFlow.FlowBit(direction), this);
				this.path[i].SetTargetFlowDirection(direction, this);
			}
		}
		for (int j = 1; j < this.path.Count; j++)
		{
			SolidConduitFlow.FlowDirection direction3 = this.GetDirection(this.path[j], this.path[j - 1]);
			this.soaInfo.SetSrcFlowDirection(this.path[j].idx, direction3);
		}
		List<SolidConduitFlow.Conduit> list = new List<SolidConduitFlow.Conduit>(this.path);
		list.Reverse();
		this.TryAdd(list);
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x00195380 File Offset: 0x00193580
	private void TryAdd(List<SolidConduitFlow.Conduit> new_path)
	{
		Predicate<SolidConduitFlow.Conduit> <>9__0;
		Predicate<SolidConduitFlow.Conduit> <>9__1;
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			if (list.Count >= new_path.Count)
			{
				bool flag = false;
				List<SolidConduitFlow.Conduit> list2 = list;
				Predicate<SolidConduitFlow.Conduit> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[0].idx));
				}
				int num = list2.FindIndex(match);
				List<SolidConduitFlow.Conduit> list3 = list;
				Predicate<SolidConduitFlow.Conduit> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[new_path.Count - 1].idx));
				}
				int num2 = list3.FindIndex(match2);
				if (num != -1 && num2 != -1)
				{
					flag = true;
					int i = num;
					int num3 = 0;
					while (i < num2)
					{
						if (list[i].idx != new_path[num3].idx)
						{
							flag = false;
							break;
						}
						i++;
						num3++;
					}
				}
				if (flag)
				{
					return;
				}
			}
		}
		for (int j = this.pathList.Count - 1; j >= 0; j--)
		{
			if (this.pathList[j].Count <= 0)
			{
				this.pathList.RemoveAt(j);
			}
		}
		for (int k = this.pathList.Count - 1; k >= 0; k--)
		{
			List<SolidConduitFlow.Conduit> old_path = this.pathList[k];
			if (new_path.Count >= old_path.Count)
			{
				bool flag2 = false;
				int num4 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[0].idx);
				int num5 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[old_path.Count - 1].idx);
				if (num4 != -1 && num5 != -1)
				{
					flag2 = true;
					int l = num4;
					int num6 = 0;
					while (l < num5)
					{
						if (new_path[l].idx != old_path[num6].idx)
						{
							flag2 = false;
							break;
						}
						l++;
						num6++;
					}
				}
				if (flag2)
				{
					this.pathList.RemoveAt(k);
				}
			}
		}
		foreach (List<SolidConduitFlow.Conduit> list4 in this.pathList)
		{
			for (int m = new_path.Count - 1; m >= 0; m--)
			{
				SolidConduitFlow.Conduit new_conduit = new_path[m];
				if (list4.FindIndex((SolidConduitFlow.Conduit t) => t.idx == new_conduit.idx) != -1 && Mathf.IsPowerOfTwo(this.soaInfo.GetPermittedFlowDirections(new_conduit.idx)))
				{
					new_path.RemoveAt(m);
				}
			}
		}
		this.pathList.Add(new_path);
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x0019569C File Offset: 0x0019389C
	public SolidConduitFlow.ConduitContents GetContents(int cell)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		return contents;
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x001956F0 File Offset: 0x001938F0
	private void SetContents(int cell, SolidConduitFlow.ConduitContents contents)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x00195744 File Offset: 0x00193944
	public void SetContents(int cell, Pickupable pickupable)
	{
		SolidConduitFlow.ConduitContents contents = new SolidConduitFlow.ConduitContents
		{
			pickupableHandle = HandleVector<int>.InvalidHandle
		};
		if (pickupable != null)
		{
			KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
			SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
			{
				kbac = component,
				pickupable = pickupable
			};
			contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
			KBatchedAnimController component2 = pickupable.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			pickupable.Trigger(856640610, true);
		}
		this.SetContents(cell, contents);
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x001957D1 File Offset: 0x001939D1
	public static int GetCellFromDirection(int cell, SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return Grid.CellLeft(cell);
		case SolidConduitFlow.FlowDirection.Right:
			return Grid.CellRight(cell);
		case SolidConduitFlow.FlowDirection.Up:
			return Grid.CellAbove(cell);
		case SolidConduitFlow.FlowDirection.Down:
			return Grid.CellBelow(cell);
		default:
			return -1;
		}
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x0019580A File Offset: 0x00193A0A
	public static SolidConduitFlow.FlowDirection InverseFlow(SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return SolidConduitFlow.FlowDirection.Right;
		case SolidConduitFlow.FlowDirection.Right:
			return SolidConduitFlow.FlowDirection.Left;
		case SolidConduitFlow.FlowDirection.Up:
			return SolidConduitFlow.FlowDirection.Down;
		case SolidConduitFlow.FlowDirection.Down:
			return SolidConduitFlow.FlowDirection.Up;
		default:
			return SolidConduitFlow.FlowDirection.None;
		}
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x00195830 File Offset: 0x00193A30
	public void Sim200ms(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		this.elapsedTime += dt;
		if (this.elapsedTime < 1f)
		{
			return;
		}
		float obj = 1f;
		this.elapsedTime -= 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			foreach (SolidConduitFlow.Conduit conduit in list)
			{
				this.UpdateConduit(conduit);
			}
		}
		this.soaInfo.UpdateFlowDirection(this);
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((SolidConduitFlow.ConduitUpdater a, SolidConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x00195988 File Offset: 0x00193B88
	public void RenderEveryTick(float dt)
	{
		for (int i = 0; i < this.GetSOAInfo().NumEntries; i++)
		{
			SolidConduitFlow.Conduit conduit = this.GetSOAInfo().GetConduit(i);
			SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this);
			if (lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None)
			{
				int cell = conduit.GetCell(this);
				int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
				SolidConduitFlow.ConduitContents contents = this.GetContents(cellFromDirection);
				if (contents.pickupableHandle.IsValid())
				{
					Vector3 a = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					Vector3 b = Grid.CellToPosCCC(cellFromDirection, Grid.SceneLayer.SolidConduitContents);
					Vector3 position = Vector3.Lerp(a, b, this.ContinuousLerpPercent);
					Pickupable pickupable = this.GetPickupable(contents.pickupableHandle);
					if (pickupable != null)
					{
						pickupable.transform.SetPosition(position);
					}
				}
			}
		}
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x00195A48 File Offset: 0x00193C48
	private void UpdateConduit(SolidConduitFlow.Conduit conduit)
	{
		if (this.soaInfo.GetUpdated(conduit.idx))
		{
			return;
		}
		if (this.soaInfo.GetSrcFlowDirection(conduit.idx) == SolidConduitFlow.FlowDirection.None)
		{
			this.soaInfo.SetSrcFlowDirection(conduit.idx, conduit.GetNextFlowSource(this));
		}
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		if (!contents.pickupableHandle.IsValid())
		{
			return;
		}
		SolidConduitFlow.FlowDirection targetFlowDirection = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, targetFlowDirection);
		if (conduitFromDirection.idx == -1)
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		int cell2 = this.soaInfo.GetCell(conduitFromDirection.idx);
		SolidConduitFlow.ConduitContents contents2 = this.grid[cell2].contents;
		if (contents2.pickupableHandle.IsValid())
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		if ((this.soaInfo.GetPermittedFlowDirections(conduit.idx) & SolidConduitFlow.FlowBit(targetFlowDirection)) != 0)
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx));
				if (conduitFromDirection2.idx == conduit.idx)
				{
					flag = true;
					break;
				}
				if (conduitFromDirection2.idx != -1)
				{
					int cell3 = this.soaInfo.GetCell(conduitFromDirection2.idx);
					SolidConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
					if (contents3.pickupableHandle.IsValid())
					{
						break;
					}
				}
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
			if (flag && !contents2.pickupableHandle.IsValid())
			{
				SolidConduitFlow.ConduitContents contents4 = this.RemoveFromGrid(conduit);
				this.AddToGrid(cell2, contents4);
				this.soaInfo.SetLastFlowInfo(conduit.idx, this.soaInfo.GetTargetFlowDirection(conduit.idx));
				this.soaInfo.SetUpdated(conduitFromDirection.idx, true);
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
		}
		this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x060047D8 RID: 18392 RVA: 0x00195CB1 File Offset: 0x00193EB1
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x060047D9 RID: 18393 RVA: 0x00195CCA File Offset: 0x00193ECA
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x00195CDD File Offset: 0x00193EDD
	private void AddToGrid(int cell_idx, SolidConduitFlow.ConduitContents contents)
	{
		this.grid[cell_idx].contents = contents;
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x00195CF4 File Offset: 0x00193EF4
	private SolidConduitFlow.ConduitContents RemoveFromGrid(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.ConduitContents contents2 = SolidConduitFlow.ConduitContents.EmptyContents();
		this.grid[cell].contents = contents2;
		return contents;
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x00195D3C File Offset: 0x00193F3C
	public void AddPickupable(int cell_idx, Pickupable pickupable)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			global::Debug.LogWarning("No conduit in cell: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		SolidConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.pickupableHandle.IsValid())
		{
			global::Debug.LogWarning("Conduit already full: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
		SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
		{
			kbac = component,
			pickupable = pickupable
		};
		contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
		if (this.viewingConduits)
		{
			this.ApplyOverlayVisualization(component);
		}
		if (pickupable.storage)
		{
			pickupable.storage.Remove(pickupable.gameObject, true);
		}
		pickupable.Trigger(856640610, true);
		this.SetContents(cell_idx, contents);
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x00195E34 File Offset: 0x00194034
	public Pickupable RemovePickupable(int cell_idx)
	{
		Pickupable pickupable = null;
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell_idx);
		if (conduit.idx != -1)
		{
			SolidConduitFlow.ConduitContents conduitContents = this.RemoveFromGrid(conduit);
			if (conduitContents.pickupableHandle.IsValid())
			{
				SolidConduitFlow.StoredInfo data = this.conveyorPickupables.GetData(conduitContents.pickupableHandle);
				this.ClearOverlayVisualization(data.kbac);
				pickupable = data.pickupable;
				if (pickupable)
				{
					pickupable.Trigger(856640610, false);
				}
				this.freedHandles.Add(conduitContents.pickupableHandle);
			}
		}
		return pickupable;
	}

	// Token: 0x060047DE RID: 18398 RVA: 0x00195EBC File Offset: 0x001940BC
	public int GetPermittedFlow(int cell)
	{
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return 0;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x00195EED File Offset: 0x001940ED
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x00195F08 File Offset: 0x00194108
	public SolidConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return SolidConduitFlow.Conduit.Invalid();
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x00195F40 File Offset: 0x00194140
	private void DumpPipeContents(int cell)
	{
		Pickupable pickupable = this.RemovePickupable(cell);
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x00195F69 File Offset: 0x00194169
	private void DumpPickupable(Pickupable pickupable)
	{
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x00195F7F File Offset: 0x0019417F
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell);
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x00195F97 File Offset: 0x00194197
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x00195FA8 File Offset: 0x001941A8
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		SolidConduitFlow.ConduitContents contents = SolidConduitFlow.ConduitContents.EmptyContents();
		this.SetContents(cell, contents);
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x00195FD8 File Offset: 0x001941D8
	public UtilityNetwork GetNetwork(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x00196003 File Offset: 0x00194203
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x00196010 File Offset: 0x00194210
	public bool IsConduitFull(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.pickupableHandle.IsValid();
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x0019603C File Offset: 0x0019423C
	public bool IsConduitEmpty(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return !contents.pickupableHandle.IsValid();
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x0019606C File Offset: 0x0019426C
	public void Initialize()
	{
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			OverlayScreen instance2 = OverlayScreen.Instance;
			instance2.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance2.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		}
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x001960D4 File Offset: 0x001942D4
	private void OnOverlayChanged(HashedString mode)
	{
		bool flag = mode == OverlayModes.SolidConveyor.ID;
		if (flag == this.viewingConduits)
		{
			return;
		}
		this.viewingConduits = flag;
		int layer = this.viewingConduits ? this.maskedOverlayLayer : Game.PickupableLayer;
		Color32 tintColour = this.viewingConduits ? SolidConduitFlow.OverlayColour : SolidConduitFlow.NormalColour;
		List<SolidConduitFlow.StoredInfo> dataList = this.conveyorPickupables.GetDataList();
		for (int i = 0; i < dataList.Count; i++)
		{
			SolidConduitFlow.StoredInfo storedInfo = dataList[i];
			if (storedInfo.kbac != null)
			{
				storedInfo.kbac.SetLayer(layer);
				storedInfo.kbac.TintColour = tintColour;
			}
		}
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x0019617D File Offset: 0x0019437D
	private void ApplyOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(this.maskedOverlayLayer);
		kbac.TintColour = SolidConduitFlow.OverlayColour;
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x001961A0 File Offset: 0x001943A0
	private void ClearOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(Game.PickupableLayer);
		kbac.TintColour = SolidConduitFlow.NormalColour;
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x001961C4 File Offset: 0x001943C4
	public Pickupable GetPickupable(HandleVector<int>.Handle h)
	{
		Pickupable result = null;
		if (h.IsValid())
		{
			result = this.conveyorPickupables.GetData(h).pickupable;
		}
		return result;
	}

	// Token: 0x04002F8A RID: 12170
	public const float MAX_SOLID_MASS = 20f;

	// Token: 0x04002F8B RID: 12171
	public const float TickRate = 1f;

	// Token: 0x04002F8C RID: 12172
	public const float WaitTime = 1f;

	// Token: 0x04002F8D RID: 12173
	private float elapsedTime;

	// Token: 0x04002F8E RID: 12174
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04002F8F RID: 12175
	private KCompactedVector<SolidConduitFlow.StoredInfo> conveyorPickupables = new KCompactedVector<SolidConduitFlow.StoredInfo>(0);

	// Token: 0x04002F90 RID: 12176
	private List<HandleVector<int>.Handle> freedHandles = new List<HandleVector<int>.Handle>();

	// Token: 0x04002F91 RID: 12177
	private SolidConduitFlow.SOAInfo soaInfo = new SolidConduitFlow.SOAInfo();

	// Token: 0x04002F93 RID: 12179
	private bool dirtyConduitUpdaters;

	// Token: 0x04002F94 RID: 12180
	private List<SolidConduitFlow.ConduitUpdater> conduitUpdaters = new List<SolidConduitFlow.ConduitUpdater>();

	// Token: 0x04002F95 RID: 12181
	private SolidConduitFlow.GridNode[] grid;

	// Token: 0x04002F96 RID: 12182
	public IUtilityNetworkMgr networkMgr;

	// Token: 0x04002F97 RID: 12183
	private HashSet<int> visited = new HashSet<int>();

	// Token: 0x04002F98 RID: 12184
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04002F99 RID: 12185
	private List<SolidConduitFlow.Conduit> path = new List<SolidConduitFlow.Conduit>();

	// Token: 0x04002F9A RID: 12186
	private List<List<SolidConduitFlow.Conduit>> pathList = new List<List<SolidConduitFlow.Conduit>>();

	// Token: 0x04002F9B RID: 12187
	public static readonly SolidConduitFlow.ConduitContents emptyContents = new SolidConduitFlow.ConduitContents
	{
		pickupableHandle = HandleVector<int>.InvalidHandle
	};

	// Token: 0x04002F9C RID: 12188
	private int maskedOverlayLayer;

	// Token: 0x04002F9D RID: 12189
	private bool viewingConduits;

	// Token: 0x04002F9E RID: 12190
	private static readonly Color32 NormalColour = Color.white;

	// Token: 0x04002F9F RID: 12191
	private static readonly Color32 OverlayColour = new Color(0.25f, 0.25f, 0.25f, 0f);

	// Token: 0x020017E7 RID: 6119
	private struct StoredInfo
	{
		// Token: 0x0400704D RID: 28749
		public KBatchedAnimController kbac;

		// Token: 0x0400704E RID: 28750
		public Pickupable pickupable;
	}

	// Token: 0x020017E8 RID: 6120
	public class SOAInfo
	{
		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06008FBE RID: 36798 RVA: 0x0032396D File Offset: 0x00321B6D
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06008FBF RID: 36799 RVA: 0x0032397A File Offset: 0x00321B7A
		public List<int> Cells
		{
			get
			{
				return this.cells;
			}
		}

		// Token: 0x06008FC0 RID: 36800 RVA: 0x00323984 File Offset: 0x00321B84
		public int AddConduit(SolidConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			SolidConduitFlow.Conduit item = new SolidConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new SolidConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(new SolidConduitFlow.ConduitFlowInfo
			{
				direction = SolidConduitFlow.FlowDirection.None
			});
			this.cells.Add(cell);
			this.updated.Add(false);
			this.diseaseContentsVisible.Add(false);
			this.conduitGOs.Add(conduit_go);
			this.srcFlowIdx.Add(-1);
			this.permittedFlowDirections.Add(0);
			this.srcFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			this.targetFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			return count;
		}

		// Token: 0x06008FC1 RID: 36801 RVA: 0x00323A84 File Offset: 0x00321C84
		public void Clear(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.ForcePermanentDiseaseContainer(i, false);
				int num = this.cells[i];
				SolidConduitFlow.ConduitContents contents = manager.grid[num].contents;
				manager.grid[num].contents = contents;
				manager.grid[num].conduitIdx = -1;
			}
			this.cells.Clear();
			this.updated.Clear();
			this.diseaseContentsVisible.Clear();
			this.srcFlowIdx.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x06008FC2 RID: 36802 RVA: 0x00323B76 File Offset: 0x00321D76
		public SolidConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x06008FC3 RID: 36803 RVA: 0x00323B84 File Offset: 0x00321D84
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x00323B92 File Offset: 0x00321D92
		public SolidConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x06008FC5 RID: 36805 RVA: 0x00323BA0 File Offset: 0x00321DA0
		public void SetConduitConnections(int idx, SolidConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x00323BB0 File Offset: 0x00321DB0
		public void ForcePermanentDiseaseContainer(int idx, bool force_on)
		{
			if (this.diseaseContentsVisible[idx] != force_on)
			{
				this.diseaseContentsVisible[idx] = force_on;
				GameObject gameObject = this.conduitGOs[idx];
				if (gameObject == null)
				{
					return;
				}
				gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
			}
		}

		// Token: 0x06008FC7 RID: 36807 RVA: 0x00323BFC File Offset: 0x00321DFC
		public SolidConduitFlow.Conduit GetConduitFromDirection(int idx, SolidConduitFlow.FlowDirection direction)
		{
			SolidConduitFlow.Conduit result = SolidConduitFlow.Conduit.Invalid();
			SolidConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case SolidConduitFlow.FlowDirection.Left:
				result = ((conduitConnections.left != -1) ? this.conduits[conduitConnections.left] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Right:
				result = ((conduitConnections.right != -1) ? this.conduits[conduitConnections.right] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Up:
				result = ((conduitConnections.up != -1) ? this.conduits[conduitConnections.up] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Down:
				result = ((conduitConnections.down != -1) ? this.conduits[conduitConnections.down] : SolidConduitFlow.Conduit.Invalid());
				break;
			}
			return result;
		}

		// Token: 0x06008FC8 RID: 36808 RVA: 0x00323CC8 File Offset: 0x00321EC8
		public void BeginFrame(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.updated[i] = false;
				SolidConduitFlow.ConduitContents contents = this.conduits[i].GetContents(manager);
				this.initialContents[i] = contents;
				this.lastFlowInfo[i] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				int num = this.cells[i];
				manager.grid[num].contents = contents;
			}
			for (int j = 0; j < manager.freedHandles.Count; j++)
			{
				HandleVector<int>.Handle handle = manager.freedHandles[j];
				manager.conveyorPickupables.Free(handle);
			}
			manager.freedHandles.Clear();
		}

		// Token: 0x06008FC9 RID: 36809 RVA: 0x00323D9A File Offset: 0x00321F9A
		public void EndFrame(SolidConduitFlow manager)
		{
		}

		// Token: 0x06008FCA RID: 36810 RVA: 0x00323D9C File Offset: 0x00321F9C
		public void UpdateFlowDirection(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				SolidConduitFlow.Conduit conduit = this.conduits[i];
				if (!this.updated[i])
				{
					int cell = conduit.GetCell(manager);
					SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
					if (!contents.pickupableHandle.IsValid())
					{
						this.srcFlowDirections[conduit.idx] = conduit.GetNextFlowSource(manager);
					}
				}
			}
		}

		// Token: 0x06008FCB RID: 36811 RVA: 0x00323E1C File Offset: 0x0032201C
		public void MarkConduitEmpty(int idx, SolidConduitFlow manager)
		{
			if (this.lastFlowInfo[idx].direction != SolidConduitFlow.FlowDirection.None)
			{
				this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				SolidConduitFlow.Conduit conduit = this.conduits[idx];
				this.targetFlowDirections[idx] = conduit.GetNextFlowTarget(manager);
				int num = this.cells[idx];
				manager.grid[num].contents = SolidConduitFlow.ConduitContents.EmptyContents();
			}
		}

		// Token: 0x06008FCC RID: 36812 RVA: 0x00323EA0 File Offset: 0x003220A0
		public void SetLastFlowInfo(int idx, SolidConduitFlow.FlowDirection direction)
		{
			this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
			{
				direction = direction
			};
		}

		// Token: 0x06008FCD RID: 36813 RVA: 0x00323ECA File Offset: 0x003220CA
		public SolidConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x06008FCE RID: 36814 RVA: 0x00323ED8 File Offset: 0x003220D8
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x06008FCF RID: 36815 RVA: 0x00323EE6 File Offset: 0x003220E6
		public int GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x06008FD0 RID: 36816 RVA: 0x00323EF4 File Offset: 0x003220F4
		public void SetPermittedFlowDirections(int idx, int permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x06008FD1 RID: 36817 RVA: 0x00323F03 File Offset: 0x00322103
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x06008FD2 RID: 36818 RVA: 0x00323F11 File Offset: 0x00322111
		public void SetTargetFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x06008FD3 RID: 36819 RVA: 0x00323F20 File Offset: 0x00322120
		public int GetSrcFlowIdx(int idx)
		{
			return this.srcFlowIdx[idx];
		}

		// Token: 0x06008FD4 RID: 36820 RVA: 0x00323F2E File Offset: 0x0032212E
		public void SetSrcFlowIdx(int idx, int new_src_idx)
		{
			this.srcFlowIdx[idx] = new_src_idx;
		}

		// Token: 0x06008FD5 RID: 36821 RVA: 0x00323F3D File Offset: 0x0032213D
		public SolidConduitFlow.FlowDirection GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x06008FD6 RID: 36822 RVA: 0x00323F4B File Offset: 0x0032214B
		public void SetSrcFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x06008FD7 RID: 36823 RVA: 0x00323F5A File Offset: 0x0032215A
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x06008FD8 RID: 36824 RVA: 0x00323F68 File Offset: 0x00322168
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x06008FD9 RID: 36825 RVA: 0x00323F77 File Offset: 0x00322177
		public bool GetUpdated(int idx)
		{
			return this.updated[idx];
		}

		// Token: 0x06008FDA RID: 36826 RVA: 0x00323F85 File Offset: 0x00322185
		public void SetUpdated(int idx, bool is_updated)
		{
			this.updated[idx] = is_updated;
		}

		// Token: 0x0400704F RID: 28751
		private List<SolidConduitFlow.Conduit> conduits = new List<SolidConduitFlow.Conduit>();

		// Token: 0x04007050 RID: 28752
		private List<SolidConduitFlow.ConduitConnections> conduitConnections = new List<SolidConduitFlow.ConduitConnections>();

		// Token: 0x04007051 RID: 28753
		private List<SolidConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<SolidConduitFlow.ConduitFlowInfo>();

		// Token: 0x04007052 RID: 28754
		private List<SolidConduitFlow.ConduitContents> initialContents = new List<SolidConduitFlow.ConduitContents>();

		// Token: 0x04007053 RID: 28755
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04007054 RID: 28756
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04007055 RID: 28757
		private List<bool> updated = new List<bool>();

		// Token: 0x04007056 RID: 28758
		private List<int> cells = new List<int>();

		// Token: 0x04007057 RID: 28759
		private List<int> permittedFlowDirections = new List<int>();

		// Token: 0x04007058 RID: 28760
		private List<int> srcFlowIdx = new List<int>();

		// Token: 0x04007059 RID: 28761
		private List<SolidConduitFlow.FlowDirection> srcFlowDirections = new List<SolidConduitFlow.FlowDirection>();

		// Token: 0x0400705A RID: 28762
		private List<SolidConduitFlow.FlowDirection> targetFlowDirections = new List<SolidConduitFlow.FlowDirection>();
	}

	// Token: 0x020017E9 RID: 6121
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x0400705B RID: 28763
		public ConduitFlowPriority priority;

		// Token: 0x0400705C RID: 28764
		public Action<float> callback;
	}

	// Token: 0x020017EA RID: 6122
	public struct GridNode
	{
		// Token: 0x0400705D RID: 28765
		public int conduitIdx;

		// Token: 0x0400705E RID: 28766
		public SolidConduitFlow.ConduitContents contents;
	}

	// Token: 0x020017EB RID: 6123
	public enum FlowDirection
	{
		// Token: 0x04007060 RID: 28768
		Blocked = -1,
		// Token: 0x04007061 RID: 28769
		None,
		// Token: 0x04007062 RID: 28770
		Left,
		// Token: 0x04007063 RID: 28771
		Right,
		// Token: 0x04007064 RID: 28772
		Up,
		// Token: 0x04007065 RID: 28773
		Down,
		// Token: 0x04007066 RID: 28774
		Num
	}

	// Token: 0x020017EC RID: 6124
	public struct ConduitConnections
	{
		// Token: 0x04007067 RID: 28775
		public int left;

		// Token: 0x04007068 RID: 28776
		public int right;

		// Token: 0x04007069 RID: 28777
		public int up;

		// Token: 0x0400706A RID: 28778
		public int down;
	}

	// Token: 0x020017ED RID: 6125
	public struct ConduitFlowInfo
	{
		// Token: 0x0400706B RID: 28779
		public SolidConduitFlow.FlowDirection direction;
	}

	// Token: 0x020017EE RID: 6126
	[Serializable]
	public struct Conduit : IEquatable<SolidConduitFlow.Conduit>
	{
		// Token: 0x06008FDC RID: 36828 RVA: 0x0032402B File Offset: 0x0032222B
		public static SolidConduitFlow.Conduit Invalid()
		{
			return new SolidConduitFlow.Conduit(-1);
		}

		// Token: 0x06008FDD RID: 36829 RVA: 0x00324033 File Offset: 0x00322233
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x06008FDE RID: 36830 RVA: 0x0032403C File Offset: 0x0032223C
		public int GetPermittedFlowDirections(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06008FDF RID: 36831 RVA: 0x0032404F File Offset: 0x0032224F
		public void SetPermittedFlowDirections(int permitted, SolidConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06008FE0 RID: 36832 RVA: 0x00324063 File Offset: 0x00322263
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x06008FE1 RID: 36833 RVA: 0x00324076 File Offset: 0x00322276
		public void SetTargetFlowDirection(SolidConduitFlow.FlowDirection directions, SolidConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x06008FE2 RID: 36834 RVA: 0x0032408C File Offset: 0x0032228C
		public SolidConduitFlow.ConduitContents GetContents(SolidConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			return manager.grid[cell].contents;
		}

		// Token: 0x06008FE3 RID: 36835 RVA: 0x003240BC File Offset: 0x003222BC
		public void SetContents(SolidConduitFlow manager, SolidConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = manager.GetPickupable(contents.pickupableHandle);
				if (pickupable != null)
				{
					pickupable.transform.parent = null;
					Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					pickupable.transform.SetPosition(position);
					pickupable.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.SolidConduitContents);
				}
			}
		}

		// Token: 0x06008FE4 RID: 36836 RVA: 0x00324140 File Offset: 0x00322340
		public SolidConduitFlow.FlowDirection GetNextFlowSource(SolidConduitFlow manager)
		{
			if (manager.soaInfo.GetPermittedFlowDirections(this.idx) == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			SolidConduitFlow.FlowDirection flowDirection = manager.soaInfo.GetSrcFlowDirection(this.idx);
			if (flowDirection == SolidConduitFlow.FlowDirection.None)
			{
				flowDirection = SolidConduitFlow.FlowDirection.Down;
			}
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.FlowDirection flowDirection2 = (flowDirection + i - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.Conduit conduitFromDirection = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection2);
				if (conduitFromDirection.idx != -1)
				{
					SolidConduitFlow.ConduitContents contents = manager.grid[conduitFromDirection.GetCell(manager)].contents;
					if (contents.pickupableHandle.IsValid())
					{
						int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection.idx);
						if (permittedFlowDirections != -1)
						{
							SolidConduitFlow.FlowDirection direction = SolidConduitFlow.InverseFlow(flowDirection2);
							if (manager.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit(direction)) != 0)
							{
								return flowDirection2;
							}
						}
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				SolidConduitFlow.FlowDirection flowDirection3 = (manager.soaInfo.GetTargetFlowDirection(this.idx) + j - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(flowDirection3);
				SolidConduitFlow.Conduit conduitFromDirection2 = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection3);
				if (conduitFromDirection2.idx != -1)
				{
					int permittedFlowDirections2 = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection2.idx);
					if (permittedFlowDirections2 != -1 && (permittedFlowDirections2 & SolidConduitFlow.FlowBit(direction2)) != 0)
					{
						return flowDirection3;
					}
				}
			}
			return SolidConduitFlow.FlowDirection.None;
		}

		// Token: 0x06008FE5 RID: 36837 RVA: 0x003242A4 File Offset: 0x003224A4
		public SolidConduitFlow.FlowDirection GetNextFlowTarget(SolidConduitFlow manager)
		{
			int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(this.idx);
			if (permittedFlowDirections == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			for (int i = 0; i < 5; i++)
			{
				int num = (manager.soaInfo.GetTargetFlowDirection(this.idx) + i - SolidConduitFlow.FlowDirection.Left + 1) % 5 + 1;
				if (manager.soaInfo.GetConduitFromDirection(this.idx, (SolidConduitFlow.FlowDirection)num).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit((SolidConduitFlow.FlowDirection)num)) != 0)
				{
					return (SolidConduitFlow.FlowDirection)num;
				}
			}
			return SolidConduitFlow.FlowDirection.Blocked;
		}

		// Token: 0x06008FE6 RID: 36838 RVA: 0x00324318 File Offset: 0x00322518
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x06008FE7 RID: 36839 RVA: 0x0032432B File Offset: 0x0032252B
		public SolidConduitFlow.ConduitContents GetInitialContents(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x06008FE8 RID: 36840 RVA: 0x0032433E File Offset: 0x0032253E
		public int GetCell(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x06008FE9 RID: 36841 RVA: 0x00324351 File Offset: 0x00322551
		public bool Equals(SolidConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x0400706C RID: 28780
		public int idx;
	}

	// Token: 0x020017EF RID: 6127
	[DebuggerDisplay("{pickupable}")]
	public struct ConduitContents
	{
		// Token: 0x06008FEA RID: 36842 RVA: 0x00324361 File Offset: 0x00322561
		public ConduitContents(HandleVector<int>.Handle pickupable_handle)
		{
			this.pickupableHandle = pickupable_handle;
		}

		// Token: 0x06008FEB RID: 36843 RVA: 0x0032436C File Offset: 0x0032256C
		public static SolidConduitFlow.ConduitContents EmptyContents()
		{
			return new SolidConduitFlow.ConduitContents
			{
				pickupableHandle = HandleVector<int>.InvalidHandle
			};
		}

		// Token: 0x0400706D RID: 28781
		public HandleVector<int>.Handle pickupableHandle;
	}
}
