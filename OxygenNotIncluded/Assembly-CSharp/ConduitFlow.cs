using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020006F9 RID: 1785
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow
{
	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060030E7 RID: 12519 RVA: 0x00103328 File Offset: 0x00101528
	// (remove) Token: 0x060030E8 RID: 12520 RVA: 0x00103360 File Offset: 0x00101560
	public event System.Action onConduitsRebuilt;

	// Token: 0x060030E9 RID: 12521 RVA: 0x00103398 File Offset: 0x00101598
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new ConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x001033D0 File Offset: 0x001015D0
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

	// Token: 0x060030EB RID: 12523 RVA: 0x00103420 File Offset: 0x00101620
	private static ConduitFlow.FlowDirections ComputeFlowDirection(int index)
	{
		return (ConduitFlow.FlowDirections)(1 << index);
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x00103429 File Offset: 0x00101629
	private static int ComputeIndex(ConduitFlow.FlowDirections flow)
	{
		switch (flow)
		{
		case ConduitFlow.FlowDirections.Down:
			return 0;
		case ConduitFlow.FlowDirections.Left:
			return 1;
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return 2;
		default:
			if (flow == ConduitFlow.FlowDirections.Up)
			{
				return 3;
			}
			break;
		}
		global::Debug.Assert(false, "multiple bits are set in 'flow'...can't compute refuted index");
		return -1;
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x0010345D File Offset: 0x0010165D
	private static ConduitFlow.FlowDirections ComputeNextFlowDirection(ConduitFlow.FlowDirections current)
	{
		if (current != ConduitFlow.FlowDirections.None)
		{
			return ConduitFlow.ComputeFlowDirection((ConduitFlow.ComputeIndex(current) + 1) % 4);
		}
		return ConduitFlow.FlowDirections.Down;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x00103473 File Offset: 0x00101673
	public static ConduitFlow.FlowDirections Invert(ConduitFlow.FlowDirections directions)
	{
		return ConduitFlow.FlowDirections.All & ~directions;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0010347C File Offset: 0x0010167C
	public static ConduitFlow.FlowDirections Opposite(ConduitFlow.FlowDirections directions)
	{
		ConduitFlow.FlowDirections result = ConduitFlow.FlowDirections.None;
		if ((directions & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Right;
		}
		else if ((directions & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Left;
		}
		else if ((directions & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Down;
		}
		else if ((directions & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Up;
		}
		return result;
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x001034B0 File Offset: 0x001016B0
	public ConduitFlow(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, float max_conduit_mass, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.conduitType = conduit_type;
		this.networkMgr = network_mgr;
		this.MaxMass = max_conduit_mass;
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x00103560 File Offset: 0x00101760
	public void Initialize(int num_cells)
	{
		this.grid = new ConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.element = SimHashes.Vacuum;
			this.grid[i].contents.diseaseIdx = byte.MaxValue;
		}
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x001035D0 File Offset: 0x001017D0
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		int count = this.networks.Count - networks.Count;
		if (0 < this.networks.Count - networks.Count)
		{
			this.networks.RemoveRange(networks.Count, count);
		}
		global::Debug.Assert(this.networks.Count <= networks.Count);
		for (int num = 0; num != networks.Count; num++)
		{
			if (num < this.networks.Count)
			{
				this.networks[num] = new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = this.networks[num].cells
				};
				this.networks[num].cells.Clear();
			}
			else
			{
				this.networks.Add(new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = new List<int>()
				});
			}
		}
		this.build_network_job.Reset(this);
		foreach (ConduitFlow.Network network in this.networks)
		{
			this.build_network_job.Add(new ConduitFlow.BuildNetworkTask(network, this.soaInfo.NumEntries));
		}
		GlobalJobManager.Run(this.build_network_job);
		for (int num2 = 0; num2 != this.build_network_job.Count; num2++)
		{
			this.build_network_job.GetWorkItem(num2).Finish();
		}
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x00103790 File Offset: 0x00101990
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		ConduitFlow.ConnectContext connectContext = new ConduitFlow.ConnectContext(this);
		this.soaInfo.Clear(this);
		this.replacements.ExceptWith(root_nodes);
		ObjectLayer layer = (this.conduitType == ConduitType.Gas) ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
		foreach (int num in root_nodes)
		{
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				global::Conduit component = gameObject.GetComponent<global::Conduit>();
				if (!(component != null) || !component.IsDisconnected())
				{
					int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
					this.grid[num].conduitIdx = conduitIdx;
					connectContext.cells.Add(num);
				}
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		this.connect_job.Reset(connectContext);
		int num2 = 256;
		for (int i = 0; i < connectContext.cells.Count; i += num2)
		{
			this.connect_job.Add(new ConduitFlow.ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));
		}
		GlobalJobManager.Run(this.connect_job);
		connectContext.Finish();
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x001038F8 File Offset: 0x00101AF8
	private ConduitFlow.FlowDirections GetDirection(ConduitFlow.Conduit conduit, ConduitFlow.Conduit target_conduit)
	{
		global::Debug.Assert(conduit.idx != -1);
		global::Debug.Assert(target_conduit.idx != -1);
		ConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Right;
		}
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x0010397C File Offset: 0x00101B7C
	public int ComputeUpdateOrder(int cell)
	{
		foreach (ConduitFlow.Network network in this.networks)
		{
			int num = network.cells.IndexOf(cell);
			if (num != -1)
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x001039E0 File Offset: 0x00101BE0
	public ConduitFlow.ConduitContents GetContents(int cell)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell].contents;
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));
		}
		return contents;
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x00103A68 File Offset: 0x00101C68
	public void SetContents(int cell, ConduitFlow.ConduitContents contents)
	{
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x00103AB9 File Offset: 0x00101CB9
	public static int GetCellFromDirection(int cell, ConduitFlow.FlowDirections direction)
	{
		switch (direction)
		{
		case ConduitFlow.FlowDirections.Down:
			return Grid.CellBelow(cell);
		case ConduitFlow.FlowDirections.Left:
			return Grid.CellLeft(cell);
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return Grid.CellRight(cell);
		default:
			if (direction == ConduitFlow.FlowDirections.Up)
			{
				return Grid.CellAbove(cell);
			}
			break;
		}
		return -1;
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x00103AF8 File Offset: 0x00101CF8
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
		this.elapsedTime -= 1f;
		float obj = 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.PooledList pooledList = ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.Allocate();
		pooledList.Capacity = Mathf.Max(pooledList.Capacity, this.networks.Count);
		foreach (ConduitFlow.Network network in this.networks)
		{
			pooledList.Add(new ConduitFlow.UpdateNetworkTask(network));
		}
		int num = 0;
		while (num != 4 && pooledList.Count != 0)
		{
			this.update_networks_job.Reset(this);
			foreach (ConduitFlow.UpdateNetworkTask work_item in pooledList)
			{
				this.update_networks_job.Add(work_item);
			}
			GlobalJobManager.Run(this.update_networks_job);
			pooledList.Clear();
			for (int num2 = 0; num2 != this.update_networks_job.Count; num2++)
			{
				ConduitFlow.UpdateNetworkTask workItem = this.update_networks_job.GetWorkItem(num2);
				if (workItem.continue_updating && num != 3)
				{
					pooledList.Add(workItem);
				}
				else
				{
					workItem.Finish(this);
				}
			}
			num++;
		}
		pooledList.Recycle();
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((ConduitFlow.ConduitUpdater a, ConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x00103D08 File Offset: 0x00101F08
	private float ComputeMovableMass(ConduitFlow.GridNode grid_node, Dictionary<int, ConduitFlow.Sink> sinks)
	{
		ConduitFlow.ConduitContents contents = grid_node.contents;
		if (contents.element == SimHashes.Vacuum)
		{
			return 0f;
		}
		ConduitFlow.Sink sink;
		if (!sinks.TryGetValue(grid_node.conduitIdx, out sink) || !(sink.consumer != null))
		{
			return contents.movable_mass;
		}
		return Mathf.Max(0f, contents.movable_mass - sink.space_remaining);
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x00103D70 File Offset: 0x00101F70
	private bool UpdateConduit(ConduitFlow.Conduit conduit, Dictionary<int, ConduitFlow.Sink> sinks)
	{
		bool result = false;
		int cell = this.soaInfo.GetCell(conduit.idx);
		ConduitFlow.GridNode gridNode = this.grid[cell];
		float num = this.ComputeMovableMass(gridNode, sinks);
		ConduitFlow.FlowDirections permittedFlowDirections = this.soaInfo.GetPermittedFlowDirections(conduit.idx);
		ConduitFlow.FlowDirections flowDirections = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		if (num <= 0f)
		{
			for (int num2 = 0; num2 != 4; num2++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection.idx != -1);
					if ((this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None)
					{
						this.soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
					}
				}
			}
		}
		else
		{
			for (int num3 = 0; num3 != 4; num3++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection2.idx != -1);
					ConduitFlow.FlowDirections srcFlowDirection = this.soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
					bool flag = (srcFlowDirection & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None;
					if (srcFlowDirection != ConduitFlow.FlowDirections.None && !flag)
					{
						result = true;
					}
					else
					{
						int cell2 = this.soaInfo.GetCell(conduitFromDirection2.idx);
						global::Debug.Assert(cell2 != -1);
						ConduitFlow.ConduitContents contents = this.grid[cell2].contents;
						bool flag2 = contents.element == SimHashes.Vacuum || contents.element == gridNode.contents.element;
						float effectiveCapacity = contents.GetEffectiveCapacity(this.MaxMass);
						bool flag3 = flag2 && effectiveCapacity > 0f;
						float num4 = Mathf.Min(num, effectiveCapacity);
						if (flag && flag3)
						{
							this.soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
						}
						if (num4 > 0f && flag3)
						{
							this.soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
							global::Debug.Assert(gridNode.contents.temperature > 0f);
							contents.temperature = GameUtil.GetFinalTemperature(gridNode.contents.temperature, num4, contents.temperature, contents.mass);
							contents.AddMass(num4);
							contents.element = gridNode.contents.element;
							int num5 = (int)(num4 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
							if (num5 != 0)
							{
								SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx, num5, contents.diseaseIdx, contents.diseaseCount);
								contents.diseaseIdx = diseaseInfo.idx;
								contents.diseaseCount = diseaseInfo.count;
							}
							this.grid[cell2].contents = contents;
							global::Debug.Assert(num4 <= gridNode.contents.mass);
							float num6 = gridNode.contents.mass - num4;
							num -= num4;
							if (num6 <= 0f)
							{
								global::Debug.Assert(num <= 0f);
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
								gridNode.contents = ConduitFlow.ConduitContents.Empty;
							}
							else
							{
								int num7 = (int)(num6 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
								global::Debug.Assert(num7 >= 0);
								ConduitFlow.ConduitContents contents2 = gridNode.contents;
								contents2.RemoveMass(num6);
								contents2.diseaseCount -= num7;
								gridNode.contents.RemoveMass(num4);
								gridNode.contents.diseaseCount = num7;
								if (num7 == 0)
								{
									gridNode.contents.diseaseIdx = byte.MaxValue;
								}
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
							}
							this.grid[cell].contents = gridNode.contents;
							result = (0f < this.ComputeMovableMass(gridNode, sinks));
							break;
						}
					}
				}
			}
		}
		ConduitFlow.FlowDirections srcFlowDirection2 = this.soaInfo.GetSrcFlowDirection(conduit.idx);
		ConduitFlow.FlowDirections pullDirection = this.soaInfo.GetPullDirection(conduit.idx);
		if (srcFlowDirection2 == ConduitFlow.FlowDirections.None || (ConduitFlow.Opposite(srcFlowDirection2) & pullDirection) != ConduitFlow.FlowDirections.None)
		{
			this.soaInfo.SetPullDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			this.soaInfo.SetSrcFlowDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			for (int num8 = 0; num8 != 2; num8++)
			{
				ConduitFlow.FlowDirections flowDirections2 = srcFlowDirection2;
				for (int num9 = 0; num9 != 4; num9++)
				{
					flowDirections2 = ConduitFlow.ComputeNextFlowDirection(flowDirections2);
					ConduitFlow.Conduit conduitFromDirection3 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
					if (conduitFromDirection3.idx != -1 && (this.soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & ConduitFlow.Opposite(flowDirections2)) != ConduitFlow.FlowDirections.None)
					{
						int cell3 = this.soaInfo.GetCell(conduitFromDirection3.idx);
						ConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
						float num10 = (num8 == 0) ? contents3.movable_mass : contents3.mass;
						if (0f < num10)
						{
							this.soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
							break;
						}
					}
				}
				if (this.soaInfo.GetSrcFlowDirection(conduit.idx) != ConduitFlow.FlowDirections.None)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x060030FC RID: 12540 RVA: 0x001042D1 File Offset: 0x001024D1
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x060030FD RID: 12541 RVA: 0x001042EA File Offset: 0x001024EA
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x001042FD File Offset: 0x001024FD
	public float GetAmountAllowedForMerging(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massDesiredtoBeMoved)
	{
		return Mathf.Min(massDesiredtoBeMoved, this.MaxMass - to.mass);
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x00104313 File Offset: 0x00102513
	public bool CanMergeContents(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massToMove)
	{
		return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) && this.GetAmountAllowedForMerging(from, to, massToMove) > 0f;
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x00104350 File Offset: 0x00102550
	public float AddElement(int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			return 0f;
		}
		ConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, this.MaxMass - contents.mass);
		float num2 = num / mass;
		if (num <= 0f)
		{
			return 0f;
		}
		contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
		contents.AddMass(num);
		contents.element = element;
		contents.ConsolidateMass();
		int num3 = (int)(num2 * (float)disease_count);
		if (num3 > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);
			contents.diseaseIdx = diseaseInfo.idx;
			contents.diseaseCount = diseaseInfo.count;
		}
		this.SetContents(cell_idx, contents);
		return num;
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x00104450 File Offset: 0x00102650
	public ConduitFlow.ConduitContents RemoveElement(int cell, float delta)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.ConduitContents.Empty;
		}
		return this.RemoveElement(conduit, delta);
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x0010447C File Offset: 0x0010267C
	public ConduitFlow.ConduitContents RemoveElement(ConduitFlow.Conduit conduit, float delta)
	{
		ConduitFlow.ConduitContents contents = conduit.GetContents(this);
		float num = Mathf.Min(contents.mass, delta);
		float num2 = contents.mass - num;
		if (num2 <= 0f)
		{
			conduit.SetContents(this, ConduitFlow.ConduitContents.Empty);
			return contents;
		}
		ConduitFlow.ConduitContents result = contents;
		result.RemoveMass(num2);
		int num3 = (int)(num2 / contents.mass * (float)contents.diseaseCount);
		result.diseaseCount = contents.diseaseCount - num3;
		ConduitFlow.ConduitContents contents2 = contents;
		contents2.RemoveMass(num);
		contents2.diseaseCount = num3;
		if (num3 <= 0)
		{
			contents2.diseaseIdx = byte.MaxValue;
			contents2.diseaseCount = 0;
		}
		conduit.SetContents(this, contents2);
		return result;
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x0010452C File Offset: 0x0010272C
	public ConduitFlow.FlowDirections GetPermittedFlow(int cell)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.FlowDirections.None;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x0010455D File Offset: 0x0010275D
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x00104578 File Offset: 0x00102778
	public ConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return ConduitFlow.Conduit.Invalid;
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x001045B0 File Offset: 0x001027B0
	private void DumpPipeContents(int cell, ConduitFlow.ConduitContents contents)
	{
		if (contents.element != SimHashes.Vacuum && contents.mass > 0f)
		{
			SimMessages.AddRemoveSubstance(cell, contents.element, CellEventLogger.Instance.ConduitFlowEmptyConduit, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, true, -1);
			this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
		}
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x00104615 File Offset: 0x00102815
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell, this.grid[cell].contents);
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x0010463E File Offset: 0x0010283E
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x0010464D File Offset: 0x0010284D
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
	}

	// Token: 0x0600310A RID: 12554 RVA: 0x0010466D File Offset: 0x0010286D
	[Conditional("CHECK_NAN")]
	private void Validate(ConduitFlow.ConduitContents contents)
	{
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError("zero degree pipe contents");
		}
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x00104694 File Offset: 0x00102894
	[OnSerializing]
	private void OnSerializing()
	{
		int numEntries = this.soaInfo.NumEntries;
		if (numEntries > 0)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[numEntries];
			this.serializedIdx = new int[numEntries];
			for (int i = 0; i < numEntries; i++)
			{
				ConduitFlow.Conduit conduit = this.soaInfo.GetConduit(i);
				ConduitFlow.ConduitContents contents = conduit.GetContents(this);
				this.serializedIdx[i] = this.soaInfo.GetCell(conduit.idx);
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(contents);
			}
			return;
		}
		this.serializedContents = null;
		this.versionedSerializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x0010472C File Offset: 0x0010292C
	[OnSerialized]
	private void OnSerialized()
	{
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x00104744 File Offset: 0x00102944
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedContents != null)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[this.serializedContents.Length];
			for (int i = 0; i < this.serializedContents.Length; i++)
			{
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(this.serializedContents[i]);
			}
			this.serializedContents = null;
		}
		if (this.versionedSerializedContents == null)
		{
			return;
		}
		for (int j = 0; j < this.versionedSerializedContents.Length; j++)
		{
			int num = this.serializedIdx[j];
			ConduitFlow.SerializedContents serializedContents = this.versionedSerializedContents[j];
			ConduitFlow.ConduitContents conduitContents = (serializedContents.mass <= 0f) ? ConduitFlow.ConduitContents.Empty : new ConduitFlow.ConduitContents(serializedContents.element, Math.Min(this.MaxMass, serializedContents.mass), serializedContents.temperature, byte.MaxValue, 0);
			if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0)
			{
				conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
				conduitContents.diseaseCount = ((conduitContents.diseaseIdx == byte.MaxValue) ? 0 : serializedContents.diseaseCount);
			}
			if (float.IsNaN(conduitContents.temperature) || (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) || 10000f < conduitContents.temperature)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				DeserializeWarnings.Instance.PipeContentsTemperatureIsNan.Warn(string.Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})", new object[]
				{
					conduitContents.temperature,
					vector2I.x,
					vector2I.y,
					num
				}), null);
				conduitContents.temperature = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
			}
			this.SetContents(num, conduitContents);
		}
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x00104938 File Offset: 0x00102B38
	public UtilityNetwork GetNetwork(ConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x0600310F RID: 12559 RVA: 0x00104963 File Offset: 0x00102B63
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x06003110 RID: 12560 RVA: 0x00104970 File Offset: 0x00102B70
	public bool IsConduitFull(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return this.MaxMass - contents.mass <= 0f;
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x001049A8 File Offset: 0x00102BA8
	public bool IsConduitEmpty(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.mass <= 0f;
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x001049D8 File Offset: 0x00102BD8
	public void FreezeConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-700727624, null);
		}
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x00104A34 File Offset: 0x00102C34
	public void MeltConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-1152799878, null);
		}
	}

	// Token: 0x04001D6E RID: 7534
	public const float MAX_LIQUID_MASS = 10f;

	// Token: 0x04001D6F RID: 7535
	public const float MAX_GAS_MASS = 1f;

	// Token: 0x04001D70 RID: 7536
	public ConduitType conduitType;

	// Token: 0x04001D71 RID: 7537
	private float MaxMass = 10f;

	// Token: 0x04001D72 RID: 7538
	private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;

	// Token: 0x04001D73 RID: 7539
	public const float TickRate = 1f;

	// Token: 0x04001D74 RID: 7540
	public const float WaitTime = 1f;

	// Token: 0x04001D75 RID: 7541
	private float elapsedTime;

	// Token: 0x04001D76 RID: 7542
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04001D77 RID: 7543
	public ConduitFlow.SOAInfo soaInfo = new ConduitFlow.SOAInfo();

	// Token: 0x04001D79 RID: 7545
	private bool dirtyConduitUpdaters;

	// Token: 0x04001D7A RID: 7546
	private List<ConduitFlow.ConduitUpdater> conduitUpdaters = new List<ConduitFlow.ConduitUpdater>();

	// Token: 0x04001D7B RID: 7547
	private ConduitFlow.GridNode[] grid;

	// Token: 0x04001D7C RID: 7548
	[Serialize]
	public int[] serializedIdx;

	// Token: 0x04001D7D RID: 7549
	[Serialize]
	public ConduitFlow.ConduitContents[] serializedContents;

	// Token: 0x04001D7E RID: 7550
	[Serialize]
	public ConduitFlow.SerializedContents[] versionedSerializedContents;

	// Token: 0x04001D7F RID: 7551
	private IUtilityNetworkMgr networkMgr;

	// Token: 0x04001D80 RID: 7552
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04001D81 RID: 7553
	private const int FLOW_DIRECTION_COUNT = 4;

	// Token: 0x04001D82 RID: 7554
	private List<ConduitFlow.Network> networks = new List<ConduitFlow.Network>();

	// Token: 0x04001D83 RID: 7555
	private WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow> build_network_job = new WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow>();

	// Token: 0x04001D84 RID: 7556
	private WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext> connect_job = new WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext>();

	// Token: 0x04001D85 RID: 7557
	private WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow> update_networks_job = new WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow>();

	// Token: 0x0200143B RID: 5179
	[DebuggerDisplay("{NumEntries}")]
	public class SOAInfo
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06008400 RID: 33792 RVA: 0x0030099C File Offset: 0x002FEB9C
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x06008401 RID: 33793 RVA: 0x003009AC File Offset: 0x002FEBAC
		public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			ConduitFlow.Conduit item = new ConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new ConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(ConduitFlow.ConduitFlowInfo.DEFAULT);
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
			HandleVector<int>.Handle handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType, count, handle, ref contents);
			HandleVector<int>.Handle item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
			this.cells.Add(cell);
			this.diseaseContentsVisible.Add(false);
			this.structureTemperatureHandles.Add(handle);
			this.temperatureHandles.Add(handle2);
			this.diseaseHandles.Add(item2);
			this.conduitGOs.Add(conduit_go);
			this.permittedFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.srcFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.pullDirections.Add(ConduitFlow.FlowDirections.None);
			this.targetFlowDirections.Add(ConduitFlow.FlowDirections.None);
			return count;
		}

		// Token: 0x06008402 RID: 33794 RVA: 0x00300AF4 File Offset: 0x002FECF4
		public void Clear(ConduitFlow manager)
		{
			if (this.clearJob.Count == 0)
			{
				this.clearJob.Reset(this);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishTemperatureToSim>(this.publishTemperatureToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToSim>(this.publishDiseaseToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.ResetConduit>(this.resetConduit);
			}
			this.clearPermanentDiseaseContainer.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToSim.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToSim.Initialize(this.conduits.Count, manager);
			this.resetConduit.Initialize(this.conduits.Count, manager);
			this.clearPermanentDiseaseContainer.Run(this);
			GlobalJobManager.Run(this.clearJob);
			for (int num = 0; num != this.conduits.Count; num++)
			{
				Game.Instance.conduitDiseaseManager.Free(this.diseaseHandles[num]);
			}
			for (int num2 = 0; num2 != this.conduits.Count; num2++)
			{
				Game.Instance.conduitTemperatureManager.Free(this.temperatureHandles[num2]);
			}
			this.cells.Clear();
			this.diseaseContentsVisible.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.pullDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.diseaseHandles.Clear();
			this.temperatureHandles.Clear();
			this.structureTemperatureHandles.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x06008403 RID: 33795 RVA: 0x00300CBD File Offset: 0x002FEEBD
		public ConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x06008404 RID: 33796 RVA: 0x00300CCB File Offset: 0x002FEECB
		public ConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x06008405 RID: 33797 RVA: 0x00300CD9 File Offset: 0x002FEED9
		public void SetConduitConnections(int idx, ConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x06008406 RID: 33798 RVA: 0x00300CE8 File Offset: 0x002FEEE8
		public float GetConduitTemperature(int idx)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
			global::Debug.Assert(!float.IsNaN(temperature));
			return temperature;
		}

		// Token: 0x06008407 RID: 33799 RVA: 0x00300D20 File Offset: 0x002FEF20
		public void SetConduitTemperatureData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
		}

		// Token: 0x06008408 RID: 33800 RVA: 0x00300D4C File Offset: 0x002FEF4C
		public ConduitDiseaseManager.Data GetDiseaseData(int idx)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			return Game.Instance.conduitDiseaseManager.GetData(handle);
		}

		// Token: 0x06008409 RID: 33801 RVA: 0x00300D78 File Offset: 0x002FEF78
		public void SetDiseaseData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
		}

		// Token: 0x0600840A RID: 33802 RVA: 0x00300DA3 File Offset: 0x002FEFA3
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x0600840B RID: 33803 RVA: 0x00300DB4 File Offset: 0x002FEFB4
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

		// Token: 0x0600840C RID: 33804 RVA: 0x00300E00 File Offset: 0x002FF000
		public ConduitFlow.Conduit GetConduitFromDirection(int idx, ConduitFlow.FlowDirections direction)
		{
			ConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case ConduitFlow.FlowDirections.Down:
				if (conduitConnections.down == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.down];
			case ConduitFlow.FlowDirections.Left:
				if (conduitConnections.left == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.left];
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				if (conduitConnections.right == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.right];
			default:
				if (direction == ConduitFlow.FlowDirections.Up)
				{
					if (conduitConnections.up == -1)
					{
						return ConduitFlow.Conduit.Invalid;
					}
					return this.conduits[conduitConnections.up];
				}
				break;
			}
			return ConduitFlow.Conduit.Invalid;
		}

		// Token: 0x0600840D RID: 33805 RVA: 0x00300EC4 File Offset: 0x002FF0C4
		public void BeginFrame(ConduitFlow manager)
		{
			if (this.beginFrameJob.Count == 0)
			{
				this.beginFrameJob.Reset(this);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InitializeContentsTask>(this.initializeContents);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InvalidateLastFlow>(this.invalidateLastFlow);
			}
			this.initializeContents.Initialize(this.conduits.Count, manager);
			this.invalidateLastFlow.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.beginFrameJob);
		}

		// Token: 0x0600840E RID: 33806 RVA: 0x00300F48 File Offset: 0x002FF148
		public void EndFrame(ConduitFlow manager)
		{
			if (this.endFrameJob.Count == 0)
			{
				this.endFrameJob.Reset(this);
				this.endFrameJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToGame>(this.publishDiseaseToGame);
			}
			this.publishTemperatureToGame.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToGame.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToGame.Run(this);
			GlobalJobManager.Run(this.endFrameJob);
		}

		// Token: 0x0600840F RID: 33807 RVA: 0x00300FC4 File Offset: 0x002FF1C4
		public void UpdateFlowDirection(ConduitFlow manager)
		{
			if (this.updateFlowDirectionJob.Count == 0)
			{
				this.updateFlowDirectionJob.Reset(this);
				this.updateFlowDirectionJob.Add<ConduitFlow.SOAInfo.FlowThroughVacuum>(this.flowThroughVacuum);
			}
			this.flowThroughVacuum.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.updateFlowDirectionJob);
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x0030101D File Offset: 0x002FF21D
		public void ResetLastFlowInfo(int idx)
		{
			this.lastFlowInfo[idx] = ConduitFlow.ConduitFlowInfo.DEFAULT;
		}

		// Token: 0x06008411 RID: 33809 RVA: 0x00301030 File Offset: 0x002FF230
		public void SetLastFlowInfo(int idx, ConduitFlow.FlowDirections direction, ref ConduitFlow.ConduitContents contents)
		{
			if (this.lastFlowInfo[idx].direction == ConduitFlow.FlowDirections.None)
			{
				this.lastFlowInfo[idx] = new ConduitFlow.ConduitFlowInfo
				{
					direction = direction,
					contents = contents
				};
			}
		}

		// Token: 0x06008412 RID: 33810 RVA: 0x0030107A File Offset: 0x002FF27A
		public ConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x06008413 RID: 33811 RVA: 0x00301088 File Offset: 0x002FF288
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x06008414 RID: 33812 RVA: 0x00301096 File Offset: 0x002FF296
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x06008415 RID: 33813 RVA: 0x003010A4 File Offset: 0x002FF2A4
		public void SetPermittedFlowDirections(int idx, ConduitFlow.FlowDirections permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x06008416 RID: 33814 RVA: 0x003010B4 File Offset: 0x002FF2B4
		public ConduitFlow.FlowDirections AddPermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] |= delta;
		}

		// Token: 0x06008417 RID: 33815 RVA: 0x003010E0 File Offset: 0x002FF2E0
		public ConduitFlow.FlowDirections RemovePermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] &= ~delta;
		}

		// Token: 0x06008418 RID: 33816 RVA: 0x0030110B File Offset: 0x002FF30B
		public ConduitFlow.FlowDirections GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x06008419 RID: 33817 RVA: 0x00301119 File Offset: 0x002FF319
		public void SetTargetFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x0600841A RID: 33818 RVA: 0x00301128 File Offset: 0x002FF328
		public ConduitFlow.FlowDirections GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x0600841B RID: 33819 RVA: 0x00301136 File Offset: 0x002FF336
		public void SetSrcFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x0600841C RID: 33820 RVA: 0x00301145 File Offset: 0x002FF345
		public ConduitFlow.FlowDirections GetPullDirection(int idx)
		{
			return this.pullDirections[idx];
		}

		// Token: 0x0600841D RID: 33821 RVA: 0x00301153 File Offset: 0x002FF353
		public void SetPullDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.pullDirections[idx] = directions;
		}

		// Token: 0x0600841E RID: 33822 RVA: 0x00301162 File Offset: 0x002FF362
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x0600841F RID: 33823 RVA: 0x00301170 File Offset: 0x002FF370
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x040064BC RID: 25788
		private List<ConduitFlow.Conduit> conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x040064BD RID: 25789
		private List<ConduitFlow.ConduitConnections> conduitConnections = new List<ConduitFlow.ConduitConnections>();

		// Token: 0x040064BE RID: 25790
		private List<ConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<ConduitFlow.ConduitFlowInfo>();

		// Token: 0x040064BF RID: 25791
		private List<ConduitFlow.ConduitContents> initialContents = new List<ConduitFlow.ConduitContents>();

		// Token: 0x040064C0 RID: 25792
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x040064C1 RID: 25793
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x040064C2 RID: 25794
		private List<int> cells = new List<int>();

		// Token: 0x040064C3 RID: 25795
		private List<ConduitFlow.FlowDirections> permittedFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x040064C4 RID: 25796
		private List<ConduitFlow.FlowDirections> srcFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x040064C5 RID: 25797
		private List<ConduitFlow.FlowDirections> pullDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x040064C6 RID: 25798
		private List<ConduitFlow.FlowDirections> targetFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x040064C7 RID: 25799
		private List<HandleVector<int>.Handle> structureTemperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x040064C8 RID: 25800
		private List<HandleVector<int>.Handle> temperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x040064C9 RID: 25801
		private List<HandleVector<int>.Handle> diseaseHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x040064CA RID: 25802
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer>();

		// Token: 0x040064CB RID: 25803
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim> publishTemperatureToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim>();

		// Token: 0x040064CC RID: 25804
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim> publishDiseaseToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim>();

		// Token: 0x040064CD RID: 25805
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit> resetConduit = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit>();

		// Token: 0x040064CE RID: 25806
		private ConduitFlow.SOAInfo.ConduitJob clearJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x040064CF RID: 25807
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask> initializeContents = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask>();

		// Token: 0x040064D0 RID: 25808
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow> invalidateLastFlow = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow>();

		// Token: 0x040064D1 RID: 25809
		private ConduitFlow.SOAInfo.ConduitJob beginFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x040064D2 RID: 25810
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame> publishTemperatureToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame>();

		// Token: 0x040064D3 RID: 25811
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame> publishDiseaseToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame>();

		// Token: 0x040064D4 RID: 25812
		private ConduitFlow.SOAInfo.ConduitJob endFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x040064D5 RID: 25813
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum> flowThroughVacuum = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum>();

		// Token: 0x040064D6 RID: 25814
		private ConduitFlow.SOAInfo.ConduitJob updateFlowDirectionJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x02002152 RID: 8530
		private abstract class ConduitTask : DivisibleTask<ConduitFlow.SOAInfo>
		{
			// Token: 0x0600A9E8 RID: 43496 RVA: 0x00372B45 File Offset: 0x00370D45
			public ConduitTask(string name) : base(name)
			{
			}

			// Token: 0x0400956F RID: 38255
			public ConduitFlow manager;
		}

		// Token: 0x02002153 RID: 8531
		private class ConduitTaskDivision<Task> : TaskDivision<Task, ConduitFlow.SOAInfo> where Task : ConduitFlow.SOAInfo.ConduitTask, new()
		{
			// Token: 0x0600A9E9 RID: 43497 RVA: 0x00372B50 File Offset: 0x00370D50
			public void Initialize(int conduitCount, ConduitFlow manager)
			{
				base.Initialize(conduitCount);
				Task[] tasks = this.tasks;
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].manager = manager;
				}
			}
		}

		// Token: 0x02002154 RID: 8532
		private class ConduitJob : WorkItemCollection<ConduitFlow.SOAInfo.ConduitTask, ConduitFlow.SOAInfo>
		{
			// Token: 0x0600A9EB RID: 43499 RVA: 0x00372B94 File Offset: 0x00370D94
			public void Add<Task>(ConduitFlow.SOAInfo.ConduitTaskDivision<Task> taskDivision) where Task : ConduitFlow.SOAInfo.ConduitTask, new()
			{
				foreach (Task task in taskDivision.tasks)
				{
					base.Add(task);
				}
			}
		}

		// Token: 0x02002155 RID: 8533
		private class ClearPermanentDiseaseContainer : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9ED RID: 43501 RVA: 0x00372BD2 File Offset: 0x00370DD2
			public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer")
			{
			}

			// Token: 0x0600A9EE RID: 43502 RVA: 0x00372BE0 File Offset: 0x00370DE0
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.ForcePermanentDiseaseContainer(num, false);
				}
			}
		}

		// Token: 0x02002156 RID: 8534
		private class PublishTemperatureToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9EF RID: 43503 RVA: 0x00372C0B File Offset: 0x00370E0B
			public PublishTemperatureToSim() : base("PublishTemperatureToSim")
			{
			}

			// Token: 0x0600A9F0 RID: 43504 RVA: 0x00372C18 File Offset: 0x00370E18
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.temperatureHandles[num];
					if (handle.IsValid())
					{
						float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
						this.manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
					}
				}
			}
		}

		// Token: 0x02002157 RID: 8535
		private class PublishDiseaseToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9F1 RID: 43505 RVA: 0x00372C89 File Offset: 0x00370E89
			public PublishDiseaseToSim() : base("PublishDiseaseToSim")
			{
			}

			// Token: 0x0600A9F2 RID: 43506 RVA: 0x00372C98 File Offset: 0x00370E98
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.diseaseHandles[num];
					if (handle.IsValid())
					{
						ConduitDiseaseManager.Data data = Game.Instance.conduitDiseaseManager.GetData(handle);
						int num2 = soaInfo.cells[num];
						this.manager.grid[num2].contents.diseaseIdx = data.diseaseIdx;
						this.manager.grid[num2].contents.diseaseCount = data.diseaseCount;
					}
				}
			}
		}

		// Token: 0x02002158 RID: 8536
		private class ResetConduit : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9F3 RID: 43507 RVA: 0x00372D34 File Offset: 0x00370F34
			public ResetConduit() : base("ResetConduitTask")
			{
			}

			// Token: 0x0600A9F4 RID: 43508 RVA: 0x00372D44 File Offset: 0x00370F44
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					this.manager.grid[soaInfo.cells[num]].conduitIdx = -1;
				}
			}
		}

		// Token: 0x02002159 RID: 8537
		private class InitializeContentsTask : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9F5 RID: 43509 RVA: 0x00372D89 File Offset: 0x00370F89
			public InitializeContentsTask() : base("SetInitialContents")
			{
			}

			// Token: 0x0600A9F6 RID: 43510 RVA: 0x00372D98 File Offset: 0x00370F98
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					int num2 = soaInfo.cells[num];
					ConduitFlow.ConduitContents conduitContents = soaInfo.conduits[num].GetContents(this.manager);
					if (conduitContents.mass <= 0f)
					{
						conduitContents = ConduitFlow.ConduitContents.Empty;
					}
					soaInfo.initialContents[num] = conduitContents;
					this.manager.grid[num2].contents = conduitContents;
				}
			}
		}

		// Token: 0x0200215A RID: 8538
		private class InvalidateLastFlow : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9F7 RID: 43511 RVA: 0x00372E1B File Offset: 0x0037101B
			public InvalidateLastFlow() : base("InvalidateLastFlow")
			{
			}

			// Token: 0x0600A9F8 RID: 43512 RVA: 0x00372E28 File Offset: 0x00371028
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.lastFlowInfo[num] = ConduitFlow.ConduitFlowInfo.DEFAULT;
				}
			}
		}

		// Token: 0x0200215B RID: 8539
		private class PublishTemperatureToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9F9 RID: 43513 RVA: 0x00372E5C File Offset: 0x0037105C
			public PublishTemperatureToGame() : base("PublishTemperatureToGame")
			{
			}

			// Token: 0x0600A9FA RID: 43514 RVA: 0x00372E6C File Offset: 0x0037106C
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x0200215C RID: 8540
		private class PublishDiseaseToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9FB RID: 43515 RVA: 0x00372ECB File Offset: 0x003710CB
			public PublishDiseaseToGame() : base("PublishDiseaseToGame")
			{
			}

			// Token: 0x0600A9FC RID: 43516 RVA: 0x00372ED8 File Offset: 0x003710D8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x0200215D RID: 8541
		private class FlowThroughVacuum : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600A9FD RID: 43517 RVA: 0x00372F37 File Offset: 0x00371137
			public FlowThroughVacuum() : base("FlowThroughVacuum")
			{
			}

			// Token: 0x0600A9FE RID: 43518 RVA: 0x00372F44 File Offset: 0x00371144
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					ConduitFlow.Conduit conduit = soaInfo.conduits[num];
					int cell = conduit.GetCell(this.manager);
					if (this.manager.grid[cell].contents.element == SimHashes.Vacuum)
					{
						soaInfo.srcFlowDirections[conduit.idx] = ConduitFlow.FlowDirections.None;
					}
				}
			}
		}
	}

	// Token: 0x0200143C RID: 5180
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x040064D7 RID: 25815
		public ConduitFlowPriority priority;

		// Token: 0x040064D8 RID: 25816
		public Action<float> callback;
	}

	// Token: 0x0200143D RID: 5181
	[DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
	public struct GridNode
	{
		// Token: 0x040064D9 RID: 25817
		public int conduitIdx;

		// Token: 0x040064DA RID: 25818
		public ConduitFlow.ConduitContents contents;
	}

	// Token: 0x0200143E RID: 5182
	public struct SerializedContents
	{
		// Token: 0x06008421 RID: 33825 RVA: 0x003012BC File Offset: 0x002FF4BC
		public SerializedContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			this.element = element;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseHash = ((disease_idx != byte.MaxValue) ? Db.Get().Diseases[(int)disease_idx].id.GetHashCode() : 0);
			this.diseaseCount = disease_count;
			if (this.diseaseCount <= 0)
			{
				this.diseaseHash = 0;
			}
		}

		// Token: 0x06008422 RID: 33826 RVA: 0x00301329 File Offset: 0x002FF529
		public SerializedContents(ConduitFlow.ConduitContents src)
		{
			this = new ConduitFlow.SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
		}

		// Token: 0x040064DB RID: 25819
		public SimHashes element;

		// Token: 0x040064DC RID: 25820
		public float mass;

		// Token: 0x040064DD RID: 25821
		public float temperature;

		// Token: 0x040064DE RID: 25822
		public int diseaseHash;

		// Token: 0x040064DF RID: 25823
		public int diseaseCount;
	}

	// Token: 0x0200143F RID: 5183
	[Flags]
	public enum FlowDirections : byte
	{
		// Token: 0x040064E1 RID: 25825
		None = 0,
		// Token: 0x040064E2 RID: 25826
		Down = 1,
		// Token: 0x040064E3 RID: 25827
		Left = 2,
		// Token: 0x040064E4 RID: 25828
		Right = 4,
		// Token: 0x040064E5 RID: 25829
		Up = 8,
		// Token: 0x040064E6 RID: 25830
		All = 15
	}

	// Token: 0x02001440 RID: 5184
	[DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
	public struct ConduitConnections
	{
		// Token: 0x040064E7 RID: 25831
		public int left;

		// Token: 0x040064E8 RID: 25832
		public int right;

		// Token: 0x040064E9 RID: 25833
		public int up;

		// Token: 0x040064EA RID: 25834
		public int down;

		// Token: 0x040064EB RID: 25835
		public static readonly ConduitFlow.ConduitConnections DEFAULT = new ConduitFlow.ConduitConnections
		{
			left = -1,
			right = -1,
			up = -1,
			down = -1
		};
	}

	// Token: 0x02001441 RID: 5185
	[DebuggerDisplay("{direction}:{contents.element}")]
	public struct ConduitFlowInfo
	{
		// Token: 0x040064EC RID: 25836
		public ConduitFlow.FlowDirections direction;

		// Token: 0x040064ED RID: 25837
		public ConduitFlow.ConduitContents contents;

		// Token: 0x040064EE RID: 25838
		public static readonly ConduitFlow.ConduitFlowInfo DEFAULT = new ConduitFlow.ConduitFlowInfo
		{
			direction = ConduitFlow.FlowDirections.None,
			contents = ConduitFlow.ConduitContents.Empty
		};
	}

	// Token: 0x02001442 RID: 5186
	[DebuggerDisplay("conduit {idx}")]
	[Serializable]
	public struct Conduit : IEquatable<ConduitFlow.Conduit>
	{
		// Token: 0x06008425 RID: 33829 RVA: 0x003013BB File Offset: 0x002FF5BB
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x06008426 RID: 33830 RVA: 0x003013C4 File Offset: 0x002FF5C4
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(ConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06008427 RID: 33831 RVA: 0x003013D7 File Offset: 0x002FF5D7
		public void SetPermittedFlowDirections(ConduitFlow.FlowDirections permitted, ConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06008428 RID: 33832 RVA: 0x003013EB File Offset: 0x002FF5EB
		public ConduitFlow.FlowDirections GetTargetFlowDirection(ConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x06008429 RID: 33833 RVA: 0x003013FE File Offset: 0x002FF5FE
		public void SetTargetFlowDirection(ConduitFlow.FlowDirections directions, ConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x0600842A RID: 33834 RVA: 0x00301414 File Offset: 0x002FF614
		public ConduitFlow.ConduitContents GetContents(ConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			contents.temperature = soaInfo.GetConduitTemperature(this.idx);
			ConduitDiseaseManager.Data diseaseData = soaInfo.GetDiseaseData(this.idx);
			contents.diseaseIdx = diseaseData.diseaseIdx;
			contents.diseaseCount = diseaseData.diseaseCount;
			return contents;
		}

		// Token: 0x0600842B RID: 33835 RVA: 0x00301488 File Offset: 0x002FF688
		public void SetContents(ConduitFlow manager, ConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			soaInfo.SetConduitTemperatureData(this.idx, ref contents);
			soaInfo.ForcePermanentDiseaseContainer(this.idx, contents.diseaseIdx != byte.MaxValue);
			soaInfo.SetDiseaseData(this.idx, ref contents);
		}

		// Token: 0x0600842C RID: 33836 RVA: 0x003014F6 File Offset: 0x002FF6F6
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x0600842D RID: 33837 RVA: 0x00301509 File Offset: 0x002FF709
		public ConduitFlow.ConduitContents GetInitialContents(ConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x0600842E RID: 33838 RVA: 0x0030151C File Offset: 0x002FF71C
		public int GetCell(ConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x0600842F RID: 33839 RVA: 0x0030152F File Offset: 0x002FF72F
		public bool Equals(ConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x040064EF RID: 25839
		public static readonly ConduitFlow.Conduit Invalid = new ConduitFlow.Conduit(-1);

		// Token: 0x040064F0 RID: 25840
		public readonly int idx;
	}

	// Token: 0x02001443 RID: 5187
	[DebuggerDisplay("{element} M:{mass} T:{temperature}")]
	public struct ConduitContents
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06008431 RID: 33841 RVA: 0x0030154C File Offset: 0x002FF74C
		public float mass
		{
			get
			{
				return this.initial_mass + this.added_mass - this.removed_mass;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06008432 RID: 33842 RVA: 0x00301562 File Offset: 0x002FF762
		public float movable_mass
		{
			get
			{
				return this.initial_mass - this.removed_mass;
			}
		}

		// Token: 0x06008433 RID: 33843 RVA: 0x00301574 File Offset: 0x002FF774
		public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			global::Debug.Assert(!float.IsNaN(temperature));
			this.element = element;
			this.initial_mass = mass;
			this.added_mass = 0f;
			this.removed_mass = 0f;
			this.temperature = temperature;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x06008434 RID: 33844 RVA: 0x003015CA File Offset: 0x002FF7CA
		public void ConsolidateMass()
		{
			this.initial_mass += this.added_mass;
			this.added_mass = 0f;
			this.initial_mass -= this.removed_mass;
			this.removed_mass = 0f;
		}

		// Token: 0x06008435 RID: 33845 RVA: 0x00301608 File Offset: 0x002FF808
		public float GetEffectiveCapacity(float maximum_capacity)
		{
			float mass = this.mass;
			return Mathf.Max(0f, maximum_capacity - mass);
		}

		// Token: 0x06008436 RID: 33846 RVA: 0x00301629 File Offset: 0x002FF829
		public void AddMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			this.added_mass += amount;
		}

		// Token: 0x06008437 RID: 33847 RVA: 0x0030164C File Offset: 0x002FF84C
		public float RemoveMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			float result = 0f;
			float num = this.mass - amount;
			if (num < 0f)
			{
				amount += num;
				result = -num;
				global::Debug.Assert(false);
			}
			this.removed_mass += amount;
			return result;
		}

		// Token: 0x040064F1 RID: 25841
		public SimHashes element;

		// Token: 0x040064F2 RID: 25842
		private float initial_mass;

		// Token: 0x040064F3 RID: 25843
		private float added_mass;

		// Token: 0x040064F4 RID: 25844
		private float removed_mass;

		// Token: 0x040064F5 RID: 25845
		public float temperature;

		// Token: 0x040064F6 RID: 25846
		public byte diseaseIdx;

		// Token: 0x040064F7 RID: 25847
		public int diseaseCount;

		// Token: 0x040064F8 RID: 25848
		public static readonly ConduitFlow.ConduitContents Empty = new ConduitFlow.ConduitContents
		{
			element = SimHashes.Vacuum,
			initial_mass = 0f,
			added_mass = 0f,
			removed_mass = 0f,
			temperature = 0f,
			diseaseIdx = byte.MaxValue,
			diseaseCount = 0
		};
	}

	// Token: 0x02001444 RID: 5188
	[DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
	private struct Network
	{
		// Token: 0x040064F9 RID: 25849
		public List<int> cells;

		// Token: 0x040064FA RID: 25850
		public FlowUtilityNetwork network;
	}

	// Token: 0x02001445 RID: 5189
	private struct BuildNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x06008439 RID: 33849 RVA: 0x0030170C File Offset: 0x002FF90C
		public BuildNetworkTask(ConduitFlow.Network network, int conduit_count)
		{
			this.network = network;
			this.distance_nodes = QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.Allocate();
			this.distances_via_sources = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sources = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.distances_via_sinks = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sinks = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sources_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
			this.from_sinks_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x0030177C File Offset: 0x002FF97C
		public void Finish()
		{
			this.distances_via_sinks.Recycle();
			this.distances_via_sources.Recycle();
			this.distance_nodes.Recycle();
			this.from_sources.Recycle();
			this.from_sinks.Recycle();
			this.from_sources_graph.Recycle();
			this.from_sinks_graph.Recycle();
		}

		// Token: 0x0600843B RID: 33851 RVA: 0x003017D8 File Offset: 0x002FF9D8
		private void ComputeFlow(ConduitFlow outer)
		{
			this.from_sources_graph.Build(outer, this.network.network.sources, this.network.network.sinks, true);
			this.from_sinks_graph.Build(outer, this.network.network.sinks, this.network.network.sources, false);
			this.from_sources_graph.Merge(this.from_sinks_graph);
			this.from_sources_graph.BreakCycles();
			this.from_sources_graph.WriteFlow(false);
			this.from_sinks_graph.WriteFlow(true);
		}

		// Token: 0x0600843C RID: 33852 RVA: 0x00301874 File Offset: 0x002FFA74
		private void ComputeOrder(ConduitFlow outer)
		{
			foreach (int cell in this.from_sources_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sources_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell2 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell2,
						distance = 0
					});
				}
				goto IL_21D;
			}
			IL_B3:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode = this.distance_nodes.Dequeue();
			int conduitIdx = outer.grid[distanceNode.cell].conduitIdx;
			if (conduitIdx != -1)
			{
				this.distances_via_sources[distanceNode.cell] = distanceNode.distance;
				ConduitFlow.ConduitConnections conduitConnections = outer.soaInfo.GetConduitConnections(conduitIdx);
				ConduitFlow.FlowDirections permittedFlowDirections = outer.soaInfo.GetPermittedFlowDirections(conduitIdx);
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.up),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.down),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.left),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.right),
						distance = distanceNode.distance + 1
					});
				}
			}
			IL_21D:
			if (this.distance_nodes.Count != 0)
			{
				goto IL_B3;
			}
			this.from_sources.AddRange(this.distances_via_sources);
			this.from_sources.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value - a.Value);
			this.distance_nodes.Clear();
			foreach (int cell3 in this.from_sinks_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell3,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sinks_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell4 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell4,
						distance = 0
					});
				}
				goto IL_508;
			}
			IL_32A:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode2 = this.distance_nodes.Dequeue();
			int conduitIdx2 = outer.grid[distanceNode2.cell].conduitIdx;
			if (conduitIdx2 != -1)
			{
				if (!this.distances_via_sources.ContainsKey(distanceNode2.cell))
				{
					this.distances_via_sinks[distanceNode2.cell] = distanceNode2.distance;
				}
				ConduitFlow.ConduitConnections conduitConnections2 = outer.soaInfo.GetConduitConnections(conduitIdx2);
				if (conduitConnections2.up != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.up) & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.up),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.down != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.down) & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.down),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.left != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.left) & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.left),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.right != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.right) & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.right),
						distance = distanceNode2.distance + 1
					});
				}
			}
			IL_508:
			if (this.distance_nodes.Count == 0)
			{
				this.from_sinks.AddRange(this.distances_via_sinks);
				this.from_sinks.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => a.Value - b.Value);
				this.network.cells.Capacity = Mathf.Max(this.network.cells.Capacity, this.from_sources.Count + this.from_sinks.Count);
				foreach (KeyValuePair<int, int> keyValuePair in this.from_sources)
				{
					this.network.cells.Add(keyValuePair.Key);
				}
				foreach (KeyValuePair<int, int> keyValuePair2 in this.from_sinks)
				{
					this.network.cells.Add(keyValuePair2.Key);
				}
				return;
			}
			goto IL_32A;
		}

		// Token: 0x0600843D RID: 33853 RVA: 0x00301EEC File Offset: 0x003000EC
		public void Run(ConduitFlow outer)
		{
			this.ComputeFlow(outer);
			this.ComputeOrder(outer);
		}

		// Token: 0x040064FB RID: 25851
		private ConduitFlow.Network network;

		// Token: 0x040064FC RID: 25852
		private QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.PooledQueue distance_nodes;

		// Token: 0x040064FD RID: 25853
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sources;

		// Token: 0x040064FE RID: 25854
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;

		// Token: 0x040064FF RID: 25855
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sinks;

		// Token: 0x04006500 RID: 25856
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;

		// Token: 0x04006501 RID: 25857
		private ConduitFlow.BuildNetworkTask.Graph from_sources_graph;

		// Token: 0x04006502 RID: 25858
		private ConduitFlow.BuildNetworkTask.Graph from_sinks_graph;

		// Token: 0x0200215E RID: 8542
		[DebuggerDisplay("cell {cell}:{distance}")]
		private struct DistanceNode
		{
			// Token: 0x04009570 RID: 38256
			public int cell;

			// Token: 0x04009571 RID: 38257
			public int distance;
		}

		// Token: 0x0200215F RID: 8543
		[DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
		private struct Graph
		{
			// Token: 0x0600A9FF RID: 43519 RVA: 0x00372FB8 File Offset: 0x003711B8
			public Graph(FlowUtilityNetwork network)
			{
				this.conduit_flow = null;
				this.vertex_cells = HashSetPool<int, ConduitFlow>.Allocate();
				this.edges = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.cycles = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.bfs_traversal = QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.visited = HashSetPool<int, ConduitFlow>.Allocate();
				this.pseudo_sources = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.sources = HashSetPool<int, ConduitFlow>.Allocate();
				this.sinks = HashSetPool<int, ConduitFlow>.Allocate();
				this.dfs_path = HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dfs_traversal = ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dead_ends = HashSetPool<int, ConduitFlow>.Allocate();
				this.cycle_vertices = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
			}

			// Token: 0x0600AA00 RID: 43520 RVA: 0x00373050 File Offset: 0x00371250
			public void Recycle()
			{
				this.vertex_cells.Recycle();
				this.edges.Recycle();
				this.cycles.Recycle();
				this.bfs_traversal.Recycle();
				this.visited.Recycle();
				this.pseudo_sources.Recycle();
				this.sources.Recycle();
				this.sinks.Recycle();
				this.dfs_path.Recycle();
				this.dfs_traversal.Recycle();
				this.dead_ends.Recycle();
				this.cycle_vertices.Recycle();
			}

			// Token: 0x0600AA01 RID: 43521 RVA: 0x003730E4 File Offset: 0x003712E4
			public void Build(ConduitFlow conduit_flow, List<FlowUtilityNetwork.IItem> sources, List<FlowUtilityNetwork.IItem> sinks, bool are_dead_ends_pseudo_sources)
			{
				this.conduit_flow = conduit_flow;
				this.sources.Clear();
				for (int i = 0; i < sources.Count; i++)
				{
					int cell = sources[i].Cell;
					if (conduit_flow.grid[cell].conduitIdx != -1)
					{
						this.sources.Add(cell);
					}
				}
				this.sinks.Clear();
				for (int j = 0; j < sinks.Count; j++)
				{
					int cell2 = sinks[j].Cell;
					if (conduit_flow.grid[cell2].conduitIdx != -1)
					{
						this.sinks.Add(cell2);
					}
				}
				global::Debug.Assert(this.bfs_traversal.Count == 0);
				this.visited.Clear();
				foreach (int num in this.sources)
				{
					this.bfs_traversal.Enqueue(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = num,
						direction = ConduitFlow.FlowDirections.None
					});
					this.visited.Add(num);
				}
				this.pseudo_sources.Clear();
				this.dead_ends.Clear();
				this.cycles.Clear();
				while (this.bfs_traversal.Count != 0)
				{
					ConduitFlow.BuildNetworkTask.Graph.Vertex node = this.bfs_traversal.Dequeue();
					this.vertex_cells.Add(node.cell);
					ConduitFlow.FlowDirections flowDirections = ConduitFlow.FlowDirections.None;
					int num2 = 4;
					if (node.direction != ConduitFlow.FlowDirections.None)
					{
						flowDirections = ConduitFlow.Opposite(node.direction);
						num2 = 3;
					}
					int conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
					for (int num3 = 0; num3 != num2; num3++)
					{
						flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
						ConduitFlow.Conduit conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);
						ConduitFlow.BuildNetworkTask.Graph.Vertex new_node = this.WalkPath(conduitIdx, conduitFromDirection.idx, flowDirections, are_dead_ends_pseudo_sources);
						if (new_node.is_valid)
						{
							ConduitFlow.BuildNetworkTask.Graph.Edge item = new ConduitFlow.BuildNetworkTask.Graph.Edge
							{
								vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
								{
									new ConduitFlow.BuildNetworkTask.Graph.Vertex
									{
										cell = node.cell,
										direction = flowDirections
									},
									new_node
								}
							};
							if (new_node.cell == node.cell)
							{
								this.cycles.Add(item);
							}
							else if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == new_node.cell && edge.vertices[1].cell == node.cell) && !this.edges.Contains(item))
							{
								this.edges.Add(item);
								if (this.visited.Add(new_node.cell))
								{
									if (this.IsSink(new_node.cell))
									{
										this.pseudo_sources.Add(new_node);
									}
									else
									{
										this.bfs_traversal.Enqueue(new_node);
									}
								}
							}
						}
					}
					if (this.bfs_traversal.Count == 0)
					{
						foreach (ConduitFlow.BuildNetworkTask.Graph.Vertex item2 in this.pseudo_sources)
						{
							this.bfs_traversal.Enqueue(item2);
						}
						this.pseudo_sources.Clear();
					}
				}
			}

			// Token: 0x0600AA02 RID: 43522 RVA: 0x003734B0 File Offset: 0x003716B0
			private bool IsEndpoint(int cell)
			{
				global::Debug.Assert(cell != -1);
				return this.conduit_flow.grid[cell].conduitIdx == -1 || this.sources.Contains(cell) || this.sinks.Contains(cell) || this.dead_ends.Contains(cell);
			}

			// Token: 0x0600AA03 RID: 43523 RVA: 0x0037350C File Offset: 0x0037170C
			private bool IsSink(int cell)
			{
				return this.sinks.Contains(cell);
			}

			// Token: 0x0600AA04 RID: 43524 RVA: 0x0037351C File Offset: 0x0037171C
			private bool IsJunction(int cell)
			{
				global::Debug.Assert(cell != -1);
				ConduitFlow.GridNode gridNode = this.conduit_flow.grid[cell];
				global::Debug.Assert(gridNode.conduitIdx != -1);
				ConduitFlow.ConduitConnections conduitConnections = this.conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
				return 2 < this.JunctionValue(conduitConnections.down) + this.JunctionValue(conduitConnections.left) + this.JunctionValue(conduitConnections.up) + this.JunctionValue(conduitConnections.right);
			}

			// Token: 0x0600AA05 RID: 43525 RVA: 0x003735A5 File Offset: 0x003717A5
			private int JunctionValue(int conduit)
			{
				if (conduit != -1)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x0600AA06 RID: 43526 RVA: 0x003735B0 File Offset: 0x003717B0
			private ConduitFlow.BuildNetworkTask.Graph.Vertex WalkPath(int root_conduit, int conduit, ConduitFlow.FlowDirections direction, bool are_dead_ends_pseudo_sources)
			{
				if (conduit == -1)
				{
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				int cell;
				for (;;)
				{
					cell = this.conduit_flow.soaInfo.GetCell(conduit);
					if (this.IsEndpoint(cell) || this.IsJunction(cell))
					{
						break;
					}
					direction = ConduitFlow.Opposite(direction);
					bool flag = true;
					for (int num = 0; num != 3; num++)
					{
						direction = ConduitFlow.ComputeNextFlowDirection(direction);
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
						if (conduitFromDirection.idx != -1)
						{
							conduit = conduitFromDirection.idx;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						goto Block_4;
					}
				}
				return new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					cell = cell,
					direction = direction
				};
				Block_4:
				if (are_dead_ends_pseudo_sources)
				{
					this.pseudo_sources.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = cell,
						direction = ConduitFlow.ComputeNextFlowDirection(direction)
					});
					this.dead_ends.Add(cell);
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				ConduitFlow.BuildNetworkTask.Graph.Vertex result = default(ConduitFlow.BuildNetworkTask.Graph.Vertex);
				result.cell = cell;
				direction = (result.direction = ConduitFlow.Opposite(ConduitFlow.ComputeNextFlowDirection(direction)));
				return result;
			}

			// Token: 0x0600AA07 RID: 43527 RVA: 0x003736BC File Offset: 0x003718BC
			public void Merge(ConduitFlow.BuildNetworkTask.Graph inverted_graph)
			{
				using (List<ConduitFlow.BuildNetworkTask.Graph.Edge>.Enumerator enumerator = inverted_graph.edges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge = enumerator.Current;
						ConduitFlow.BuildNetworkTask.Graph.Edge candidate = inverted_edge.Invert();
						if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.Equals(inverted_edge) || edge.Equals(candidate)))
						{
							this.edges.Add(candidate);
							this.vertex_cells.Add(candidate.vertices[0].cell);
							this.vertex_cells.Add(candidate.vertices[1].cell);
						}
					}
				}
				int num = 1000;
				for (int num2 = 0; num2 != num; num2++)
				{
					global::Debug.Assert(num2 != num - 1);
					bool flag = false;
					using (HashSet<int>.Enumerator enumerator2 = this.vertex_cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int cell = enumerator2.Current;
							if (!this.IsSink(cell) && !this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == cell))
							{
								int num3 = inverted_graph.edges.FindIndex((ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge) => inverted_edge.vertices[1].cell == cell);
								if (num3 != -1)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge3 = inverted_graph.edges[num3];
									for (int num4 = 0; num4 != this.edges.Count; num4++)
									{
										ConduitFlow.BuildNetworkTask.Graph.Edge edge2 = this.edges[num4];
										if (edge2.vertices[0].cell == edge3.vertices[0].cell && edge2.vertices[1].cell == edge3.vertices[1].cell)
										{
											this.edges[num4] = edge2.Invert();
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}

			// Token: 0x0600AA08 RID: 43528 RVA: 0x00373920 File Offset: 0x00371B20
			public void BreakCycles()
			{
				this.visited.Clear();
				foreach (int num in this.vertex_cells)
				{
					if (!this.visited.Contains(num))
					{
						this.dfs_path.Clear();
						this.dfs_traversal.Clear();
						this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
						{
							cell = num,
							parent = null
						});
						while (this.dfs_traversal.Count != 0)
						{
							ConduitFlow.BuildNetworkTask.Graph.DFSNode dfsnode = this.dfs_traversal[this.dfs_traversal.Count - 1];
							this.dfs_traversal.RemoveAt(this.dfs_traversal.Count - 1);
							bool flag = false;
							for (ConduitFlow.BuildNetworkTask.Graph.DFSNode parent = dfsnode.parent; parent != null; parent = parent.parent)
							{
								if (parent.cell == dfsnode.cell)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								for (int num2 = this.edges.Count - 1; num2 != -1; num2--)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge = this.edges[num2];
									if (edge.vertices[0].cell == dfsnode.parent.cell && edge.vertices[1].cell == dfsnode.cell)
									{
										this.cycles.Add(edge);
										this.edges.RemoveAt(num2);
									}
								}
							}
							else if (this.visited.Add(dfsnode.cell))
							{
								foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.edges)
								{
									if (edge2.vertices[0].cell == dfsnode.cell)
									{
										this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
										{
											cell = edge2.vertices[1].cell,
											parent = dfsnode
										});
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x0600AA09 RID: 43529 RVA: 0x00373B70 File Offset: 0x00371D70
			public void WriteFlow(bool cycles_only = false)
			{
				if (!cycles_only)
				{
					foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge in this.edges)
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator = edge.Iter(this.conduit_flow);
						while (vertexIterator.IsValid())
						{
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertexIterator.cell].conduitIdx, vertexIterator.direction);
							vertexIterator.Next();
						}
					}
				}
				foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.cycles)
				{
					this.cycle_vertices.Clear();
					ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator2 = edge2.Iter(this.conduit_flow);
					vertexIterator2.Next();
					while (vertexIterator2.IsValid())
					{
						this.cycle_vertices.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
						{
							cell = vertexIterator2.cell,
							direction = vertexIterator2.direction
						});
						vertexIterator2.Next();
					}
					if (this.cycle_vertices.Count > 1)
					{
						int i = 0;
						int num = this.cycle_vertices.Count - 1;
						ConduitFlow.FlowDirections direction = edge2.vertices[0].direction;
						while (i <= num)
						{
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex = this.cycle_vertices[i];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex.cell].conduitIdx, ConduitFlow.Opposite(direction));
							direction = vertex.direction;
							i++;
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex2 = this.cycle_vertices[num];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex2.cell].conduitIdx, vertex2.direction);
							num--;
						}
						this.dead_ends.Add(this.cycle_vertices[i].cell);
						this.dead_ends.Add(this.cycle_vertices[num].cell);
					}
				}
			}

			// Token: 0x04009572 RID: 38258
			private ConduitFlow conduit_flow;

			// Token: 0x04009573 RID: 38259
			private HashSetPool<int, ConduitFlow>.PooledHashSet vertex_cells;

			// Token: 0x04009574 RID: 38260
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList edges;

			// Token: 0x04009575 RID: 38261
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList cycles;

			// Token: 0x04009576 RID: 38262
			private QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledQueue bfs_traversal;

			// Token: 0x04009577 RID: 38263
			private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

			// Token: 0x04009578 RID: 38264
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList pseudo_sources;

			// Token: 0x04009579 RID: 38265
			public HashSetPool<int, ConduitFlow>.PooledHashSet sources;

			// Token: 0x0400957A RID: 38266
			private HashSetPool<int, ConduitFlow>.PooledHashSet sinks;

			// Token: 0x0400957B RID: 38267
			private HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledHashSet dfs_path;

			// Token: 0x0400957C RID: 38268
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledList dfs_traversal;

			// Token: 0x0400957D RID: 38269
			public HashSetPool<int, ConduitFlow>.PooledHashSet dead_ends;

			// Token: 0x0400957E RID: 38270
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList cycle_vertices;

			// Token: 0x02002F60 RID: 12128
			[DebuggerDisplay("{cell}:{direction}")]
			public struct Vertex : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Vertex>
			{
				// Token: 0x17000A93 RID: 2707
				// (get) Token: 0x0600C6FB RID: 50939 RVA: 0x003C1CA8 File Offset: 0x003BFEA8
				public bool is_valid
				{
					get
					{
						return this.cell != -1;
					}
				}

				// Token: 0x0600C6FC RID: 50940 RVA: 0x003C1CB6 File Offset: 0x003BFEB6
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Vertex rhs)
				{
					return this.direction == rhs.direction && this.cell == rhs.cell;
				}

				// Token: 0x0400C1A3 RID: 49571
				public ConduitFlow.FlowDirections direction;

				// Token: 0x0400C1A4 RID: 49572
				public int cell;

				// Token: 0x0400C1A5 RID: 49573
				public static ConduitFlow.BuildNetworkTask.Graph.Vertex INVALID = new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					direction = ConduitFlow.FlowDirections.None,
					cell = -1
				};
			}

			// Token: 0x02002F61 RID: 12129
			[DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
			public struct Edge : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Edge>
			{
				// Token: 0x17000A94 RID: 2708
				// (get) Token: 0x0600C6FE RID: 50942 RVA: 0x003C1D03 File Offset: 0x003BFF03
				public bool is_valid
				{
					get
					{
						return this.vertices != null;
					}
				}

				// Token: 0x0600C6FF RID: 50943 RVA: 0x003C1D10 File Offset: 0x003BFF10
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Edge rhs)
				{
					if (this.vertices == null)
					{
						return rhs.vertices == null;
					}
					return rhs.vertices != null && (this.vertices.Length == rhs.vertices.Length && this.vertices.Length == 2 && this.vertices[0].Equals(rhs.vertices[0])) && this.vertices[1].Equals(rhs.vertices[1]);
				}

				// Token: 0x0600C700 RID: 50944 RVA: 0x003C1D94 File Offset: 0x003BFF94
				public ConduitFlow.BuildNetworkTask.Graph.Edge Invert()
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge
					{
						vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
						{
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[1].cell,
								direction = ConduitFlow.Opposite(this.vertices[1].direction)
							},
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[0].cell,
								direction = ConduitFlow.Opposite(this.vertices[0].direction)
							}
						}
					};
				}

				// Token: 0x0600C701 RID: 50945 RVA: 0x003C1E41 File Offset: 0x003C0041
				public ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator Iter(ConduitFlow conduit_flow)
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator(conduit_flow, this);
				}

				// Token: 0x0400C1A6 RID: 49574
				public ConduitFlow.BuildNetworkTask.Graph.Vertex[] vertices;

				// Token: 0x0400C1A7 RID: 49575
				public static readonly ConduitFlow.BuildNetworkTask.Graph.Edge INVALID = new ConduitFlow.BuildNetworkTask.Graph.Edge
				{
					vertices = null
				};

				// Token: 0x020031F7 RID: 12791
				[DebuggerDisplay("{cell}:{direction}")]
				public struct VertexIterator
				{
					// Token: 0x0600CBBC RID: 52156 RVA: 0x003CD5D3 File Offset: 0x003CB7D3
					public VertexIterator(ConduitFlow conduit_flow, ConduitFlow.BuildNetworkTask.Graph.Edge edge)
					{
						this.conduit_flow = conduit_flow;
						this.edge = edge;
						this.cell = edge.vertices[0].cell;
						this.direction = edge.vertices[0].direction;
					}

					// Token: 0x0600CBBD RID: 52157 RVA: 0x003CD614 File Offset: 0x003CB814
					public void Next()
					{
						int conduitIdx = this.conduit_flow.grid[this.cell].conduitIdx;
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, this.direction);
						global::Debug.Assert(conduitFromDirection.idx != -1);
						this.cell = conduitFromDirection.GetCell(this.conduit_flow);
						if (this.cell == this.edge.vertices[1].cell)
						{
							return;
						}
						this.direction = ConduitFlow.Opposite(this.direction);
						bool flag = false;
						for (int num = 0; num != 3; num++)
						{
							this.direction = ConduitFlow.ComputeNextFlowDirection(this.direction);
							if (this.conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.direction).idx != -1)
							{
								flag = true;
								break;
							}
						}
						global::Debug.Assert(flag);
						if (!flag)
						{
							this.cell = this.edge.vertices[1].cell;
						}
					}

					// Token: 0x0600CBBE RID: 52158 RVA: 0x003CD715 File Offset: 0x003CB915
					public bool IsValid()
					{
						return this.cell != this.edge.vertices[1].cell;
					}

					// Token: 0x0400C84E RID: 51278
					public int cell;

					// Token: 0x0400C84F RID: 51279
					public ConduitFlow.FlowDirections direction;

					// Token: 0x0400C850 RID: 51280
					private ConduitFlow conduit_flow;

					// Token: 0x0400C851 RID: 51281
					private ConduitFlow.BuildNetworkTask.Graph.Edge edge;
				}
			}

			// Token: 0x02002F62 RID: 12130
			[DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
			private class DFSNode
			{
				// Token: 0x0400C1A8 RID: 49576
				public int cell;

				// Token: 0x0400C1A9 RID: 49577
				public ConduitFlow.BuildNetworkTask.Graph.DFSNode parent;
			}
		}
	}

	// Token: 0x02001446 RID: 5190
	private struct ConnectContext
	{
		// Token: 0x0600843E RID: 33854 RVA: 0x00301EFC File Offset: 0x003000FC
		public ConnectContext(ConduitFlow outer)
		{
			this.outer = outer;
			this.cells = ListPool<int, ConduitFlow>.Allocate();
			this.cells.Capacity = Mathf.Max(this.cells.Capacity, outer.soaInfo.NumEntries);
		}

		// Token: 0x0600843F RID: 33855 RVA: 0x00301F36 File Offset: 0x00300136
		public void Finish()
		{
			this.cells.Recycle();
		}

		// Token: 0x04006503 RID: 25859
		public ListPool<int, ConduitFlow>.PooledList cells;

		// Token: 0x04006504 RID: 25860
		public ConduitFlow outer;
	}

	// Token: 0x02001447 RID: 5191
	private struct ConnectTask : IWorkItem<ConduitFlow.ConnectContext>
	{
		// Token: 0x06008440 RID: 33856 RVA: 0x00301F43 File Offset: 0x00300143
		public ConnectTask(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x06008441 RID: 33857 RVA: 0x00301F54 File Offset: 0x00300154
		public void Run(ConduitFlow.ConnectContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				int num2 = context.cells[num];
				int conduitIdx = context.outer.grid[num2].conduitIdx;
				if (conduitIdx != -1)
				{
					UtilityConnections connections = context.outer.networkMgr.GetConnections(num2, true);
					if (connections != (UtilityConnections)0)
					{
						ConduitFlow.ConduitConnections @default = ConduitFlow.ConduitConnections.DEFAULT;
						int num3 = num2 - 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
						{
							@default.left = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
						{
							@default.right = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 - Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
						{
							@default.down = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
						{
							@default.up = context.outer.grid[num3].conduitIdx;
						}
						context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
					}
				}
			}
		}

		// Token: 0x04006505 RID: 25861
		private int start;

		// Token: 0x04006506 RID: 25862
		private int end;
	}

	// Token: 0x02001448 RID: 5192
	private struct Sink
	{
		// Token: 0x06008442 RID: 33858 RVA: 0x003020A8 File Offset: 0x003002A8
		public Sink(FlowUtilityNetwork.IItem sink)
		{
			this.consumer = ((sink.GameObject != null) ? sink.GameObject.GetComponent<ConduitConsumer>() : null);
			this.space_remaining = ((this.consumer != null && this.consumer.operational.IsOperational) ? this.consumer.space_remaining_kg : 0f);
		}

		// Token: 0x04006507 RID: 25863
		public ConduitConsumer consumer;

		// Token: 0x04006508 RID: 25864
		public float space_remaining;
	}

	// Token: 0x02001449 RID: 5193
	private class UpdateNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06008443 RID: 33859 RVA: 0x0030210F File Offset: 0x0030030F
		// (set) Token: 0x06008444 RID: 33860 RVA: 0x00302117 File Offset: 0x00300317
		public bool continue_updating { get; private set; }

		// Token: 0x06008445 RID: 33861 RVA: 0x00302120 File Offset: 0x00300320
		public UpdateNetworkTask(ConduitFlow.Network network)
		{
			this.continue_updating = true;
			this.network = network;
			this.sinks = DictionaryPool<int, ConduitFlow.Sink, ConduitFlow>.Allocate();
			foreach (FlowUtilityNetwork.IItem item in network.network.sinks)
			{
				this.sinks.Add(item.Cell, new ConduitFlow.Sink(item));
			}
		}

		// Token: 0x06008446 RID: 33862 RVA: 0x003021A8 File Offset: 0x003003A8
		public void Run(ConduitFlow conduit_flow)
		{
			global::Debug.Assert(this.continue_updating);
			this.continue_updating = false;
			foreach (int num in this.network.cells)
			{
				int conduitIdx = conduit_flow.grid[num].conduitIdx;
				if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx), this.sinks))
				{
					this.continue_updating = true;
				}
			}
		}

		// Token: 0x06008447 RID: 33863 RVA: 0x00302240 File Offset: 0x00300440
		public void Finish(ConduitFlow conduit_flow)
		{
			foreach (int num in this.network.cells)
			{
				ConduitFlow.ConduitContents contents = conduit_flow.grid[num].contents;
				contents.ConsolidateMass();
				conduit_flow.grid[num].contents = contents;
			}
			this.sinks.Recycle();
		}

		// Token: 0x04006509 RID: 25865
		private ConduitFlow.Network network;

		// Token: 0x0400650A RID: 25866
		private DictionaryPool<int, ConduitFlow.Sink, ConduitFlow>.PooledDictionary sinks;
	}
}
