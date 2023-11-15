using System;
using System.Collections.Generic;
using HUSL;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public class NavGrid
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06001531 RID: 5425 RVA: 0x000704FD File Offset: 0x0006E6FD
	// (set) Token: 0x06001532 RID: 5426 RVA: 0x00070505 File Offset: 0x0006E705
	public NavTable NavTable { get; private set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007050E File Offset: 0x0006E70E
	// (set) Token: 0x06001534 RID: 5428 RVA: 0x00070516 File Offset: 0x0006E716
	public NavGrid.Transition[] transitions { get; set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06001535 RID: 5429 RVA: 0x0007051F File Offset: 0x0006E71F
	// (set) Token: 0x06001536 RID: 5430 RVA: 0x00070527 File Offset: 0x0006E727
	public NavGrid.Transition[][] transitionsByNavType { get; private set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x00070530 File Offset: 0x0006E730
	// (set) Token: 0x06001538 RID: 5432 RVA: 0x00070538 File Offset: 0x0006E738
	public int updateRangeX { get; private set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06001539 RID: 5433 RVA: 0x00070541 File Offset: 0x0006E741
	// (set) Token: 0x0600153A RID: 5434 RVA: 0x00070549 File Offset: 0x0006E749
	public int updateRangeY { get; private set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600153B RID: 5435 RVA: 0x00070552 File Offset: 0x0006E752
	// (set) Token: 0x0600153C RID: 5436 RVA: 0x0007055A File Offset: 0x0006E75A
	public int maxLinksPerCell { get; private set; }

	// Token: 0x0600153D RID: 5437 RVA: 0x00070563 File Offset: 0x0006E763
	public static NavType MirrorNavType(NavType nav_type)
	{
		if (nav_type == NavType.LeftWall)
		{
			return NavType.RightWall;
		}
		if (nav_type == NavType.RightWall)
		{
			return NavType.LeftWall;
		}
		return nav_type;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00070574 File Offset: 0x0006E774
	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
		this.id = id;
		this.Validators = validators;
		this.navTypeData = nav_type_data;
		this.transitions = transitions;
		this.boundingOffsets = bounding_offsets;
		List<NavType> list = new List<NavType>();
		this.updateRangeX = update_range_x;
		this.updateRangeY = update_range_y;
		this.maxLinksPerCell = max_links_per_cell + 1;
		for (int i = 0; i < transitions.Length; i++)
		{
			DebugUtil.Assert(i >= 0 && i <= 255);
			transitions[i].id = (byte)i;
			if (!list.Contains(transitions[i].start))
			{
				list.Add(transitions[i].start);
			}
			if (!list.Contains(transitions[i].end))
			{
				list.Add(transitions[i].end);
			}
		}
		this.ValidNavTypes = list.ToArray();
		this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
		this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
		foreach (NavType nav_type in this.ValidNavTypes)
		{
			this.GetNavTypeData(nav_type);
		}
		this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
		this.NavTable = new NavTable(Grid.CellCount);
		this.transitions = transitions;
		this.transitionsByNavType = new NavGrid.Transition[11][];
		for (int k = 0; k < 11; k++)
		{
			List<NavGrid.Transition> list2 = new List<NavGrid.Transition>();
			NavType navType = (NavType)k;
			foreach (NavGrid.Transition transition in transitions)
			{
				if (transition.start == navType)
				{
					list2.Add(transition);
				}
			}
			this.transitionsByNavType[k] = list2.ToArray();
		}
		foreach (NavTableValidator navTableValidator in validators)
		{
			navTableValidator.onDirty = (Action<int>)Delegate.Combine(navTableValidator.onDirty, new Action<int>(this.AddDirtyCell));
		}
		this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
		this.InitializeGraph();
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x000707B4 File Offset: 0x0006E9B4
	public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
	{
		foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
		{
			if (navTypeData.navType == nav_type)
			{
				return navTypeData;
			}
		}
		throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x00070808 File Offset: 0x0006EA08
	public bool HasNavTypeData(NavType nav_type)
	{
		NavGrid.NavTypeData[] array = this.navTypeData;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].navType == nav_type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x0007083C File Offset: 0x0006EA3C
	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x0007084A File Offset: 0x0006EA4A
	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x00070878 File Offset: 0x0006EA78
	public void UpdateGraph()
	{
		foreach (int cell in this.DirtyCells)
		{
			for (int i = -this.updateRangeY; i <= this.updateRangeY; i++)
			{
				for (int j = -this.updateRangeX; j <= this.updateRangeX; j++)
				{
					int num = Grid.OffsetCell(cell, j, i);
					if (Grid.IsValidCell(num))
					{
						this.ExpandedDirtyCells.Add(num);
					}
				}
			}
		}
		this.UpdateGraph(this.ExpandedDirtyCells);
		this.DirtyCells = new HashSet<int>();
		this.ExpandedDirtyCells = new HashSet<int>();
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00070938 File Offset: 0x0006EB38
	public void UpdateGraph(HashSet<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00070989 File Offset: 0x0006EB89
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x000709A0 File Offset: 0x0006EBA0
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGrid.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x000709F8 File Offset: 0x0006EBF8
	private void DebugDrawValidCells()
	{
		Color white = Color.white;
		int cellCount = Grid.CellCount;
		for (int i = 0; i < cellCount; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				NavType nav_type = (NavType)j;
				if (this.NavTable.IsValid(i, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
				{
					DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(i, nav_type), white, 1f, 0f, false);
				}
			}
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x00070A64 File Offset: 0x0006EC64
	private void DebugDrawLinks()
	{
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * this.maxLinksPerCell;
			for (int link = this.Links[num].link; link != NavGrid.InvalidCell; link = this.Links[num].link)
			{
				NavTypeHelper.GetNavPos(i, this.Links[num].startNavType);
				if (this.DrawNavTypeLink(this.Links[num].startNavType, ref white) || this.DrawNavTypeLink(this.Links[num].endNavType, ref white))
				{
					NavTypeHelper.GetNavPos(link, this.Links[num].endNavType);
				}
				num++;
			}
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00070B34 File Offset: 0x0006ED34
	private bool DrawNavTypeLink(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewLinksAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewLinkType[i];
			}
		}
		return false;
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00070B80 File Offset: 0x0006ED80
	private bool DrawNavTypeCell(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewValidCellsAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewValidCellsType[i];
			}
		}
		return false;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00070BCC File Offset: 0x0006EDCC
	public void DebugUpdate()
	{
		if (this.DebugViewValidCells)
		{
			this.DebugDrawValidCells();
		}
		if (this.DebugViewLinks)
		{
			this.DebugDrawLinks();
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x00070BEA File Offset: 0x0006EDEA
	public void AddDirtyCell(int cell)
	{
		this.DirtyCells.Add(cell);
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x00070BFC File Offset: 0x0006EDFC
	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x00070C28 File Offset: 0x0006EE28
	private Color NavTypeColor(NavType navType)
	{
		if (this.debugColorLookup == null)
		{
			this.debugColorLookup = new Color[11];
			for (int i = 0; i < 11; i++)
			{
				double num = (double)i / 11.0;
				IList<double> list = ColorConverter.HUSLToRGB(new double[]
				{
					num * 360.0,
					100.0,
					50.0
				});
				this.debugColorLookup[i] = new Color((float)list[0], (float)list[1], (float)list[2]);
			}
		}
		return this.debugColorLookup[(int)navType];
	}

	// Token: 0x04000B93 RID: 2963
	public bool DebugViewAllPaths;

	// Token: 0x04000B94 RID: 2964
	public bool DebugViewValidCells;

	// Token: 0x04000B95 RID: 2965
	public bool[] DebugViewValidCellsType;

	// Token: 0x04000B96 RID: 2966
	public bool DebugViewValidCellsAll;

	// Token: 0x04000B97 RID: 2967
	public bool DebugViewLinks;

	// Token: 0x04000B98 RID: 2968
	public bool[] DebugViewLinkType;

	// Token: 0x04000B99 RID: 2969
	public bool DebugViewLinksAll;

	// Token: 0x04000B9A RID: 2970
	public static int InvalidHandle = -1;

	// Token: 0x04000B9B RID: 2971
	public static int InvalidIdx = -1;

	// Token: 0x04000B9C RID: 2972
	public static int InvalidCell = -1;

	// Token: 0x04000B9D RID: 2973
	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	// Token: 0x04000B9E RID: 2974
	public NavGrid.Link[] Links;

	// Token: 0x04000BA0 RID: 2976
	private HashSet<int> DirtyCells = new HashSet<int>();

	// Token: 0x04000BA1 RID: 2977
	private HashSet<int> ExpandedDirtyCells = new HashSet<int>();

	// Token: 0x04000BA2 RID: 2978
	private NavTableValidator[] Validators = new NavTableValidator[0];

	// Token: 0x04000BA3 RID: 2979
	private CellOffset[] boundingOffsets;

	// Token: 0x04000BA4 RID: 2980
	public string id;

	// Token: 0x04000BA5 RID: 2981
	public bool updateEveryFrame;

	// Token: 0x04000BA6 RID: 2982
	public PathFinder.PotentialScratchPad potentialScratchPad;

	// Token: 0x04000BA7 RID: 2983
	public Action<HashSet<int>> OnNavGridUpdateComplete;

	// Token: 0x04000BAA RID: 2986
	public NavType[] ValidNavTypes;

	// Token: 0x04000BAB RID: 2987
	public NavGrid.NavTypeData[] navTypeData;

	// Token: 0x04000BAF RID: 2991
	private Color[] debugColorLookup;

	// Token: 0x0200107E RID: 4222
	public struct Link
	{
		// Token: 0x060075D4 RID: 30164 RVA: 0x002CDA30 File Offset: 0x002CBC30
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		// Token: 0x04005939 RID: 22841
		public int link;

		// Token: 0x0400593A RID: 22842
		public NavType startNavType;

		// Token: 0x0400593B RID: 22843
		public NavType endNavType;

		// Token: 0x0400593C RID: 22844
		public byte transitionId;

		// Token: 0x0400593D RID: 22845
		public byte cost;
	}

	// Token: 0x0200107F RID: 4223
	public struct NavTypeData
	{
		// Token: 0x0400593E RID: 22846
		public NavType navType;

		// Token: 0x0400593F RID: 22847
		public Vector2 animControllerOffset;

		// Token: 0x04005940 RID: 22848
		public bool flipX;

		// Token: 0x04005941 RID: 22849
		public bool flipY;

		// Token: 0x04005942 RID: 22850
		public float rotation;

		// Token: 0x04005943 RID: 22851
		public HashedString idleAnim;
	}

	// Token: 0x02001080 RID: 4224
	public struct Transition
	{
		// Token: 0x060075D5 RID: 30165 RVA: 0x002CDA58 File Offset: 0x002CBC58
		public override string ToString()
		{
			return string.Format("{0}: {1}->{2} ({3}); offset {4},{5}", new object[]
			{
				this.id,
				this.start,
				this.end,
				this.startAxis,
				this.x,
				this.y
			});
		}

		// Token: 0x060075D6 RID: 30166 RVA: 0x002CDACC File Offset: 0x002CBCCC
		public Transition(NavType start, NavType end, int x, int y, NavAxis start_axis, bool is_looping, bool loop_has_pre, bool is_escape, int cost, string anim, CellOffset[] void_offsets, CellOffset[] solid_offsets, NavOffset[] valid_nav_offsets, NavOffset[] invalid_nav_offsets, bool critter = false, float animSpeed = 1f)
		{
			DebugUtil.Assert(cost <= 255 && cost >= 0);
			this.id = byte.MaxValue;
			this.start = start;
			this.end = end;
			this.x = x;
			this.y = y;
			this.startAxis = start_axis;
			this.isLooping = is_looping;
			this.isEscape = is_escape;
			this.anim = anim;
			this.preAnim = "";
			this.cost = (byte)cost;
			if (string.IsNullOrEmpty(this.anim))
			{
				this.anim = string.Concat(new string[]
				{
					start.ToString().ToLower(),
					"_",
					end.ToString().ToLower(),
					"_",
					x.ToString(),
					"_",
					y.ToString()
				});
			}
			if (this.isLooping)
			{
				if (loop_has_pre)
				{
					this.preAnim = this.anim + "_pre";
				}
				this.anim += "_loop";
			}
			if (this.startAxis != NavAxis.NA)
			{
				this.anim += ((this.startAxis == NavAxis.X) ? "_x" : "_y");
			}
			this.voidOffsets = void_offsets;
			this.solidOffsets = solid_offsets;
			this.validNavOffsets = valid_nav_offsets;
			this.invalidNavOffsets = invalid_nav_offsets;
			this.isCritter = critter;
			this.animSpeed = animSpeed;
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x002CDC58 File Offset: 0x002CBE58
		public int IsValid(int cell, NavTable nav_table)
		{
			if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
			{
				return Grid.InvalidCell;
			}
			int num = Grid.OffsetCell(cell, this.x, this.y);
			if (!nav_table.IsValid(num, this.end))
			{
				return Grid.InvalidCell;
			}
			Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
			if (this.isCritter)
			{
				buildFlags |= Grid.BuildFlags.CritterImpassable;
			}
			foreach (CellOffset cellOffset in this.voidOffsets)
			{
				int num2 = Grid.OffsetCell(cell, cellOffset.x, cellOffset.y);
				if (Grid.IsValidCell(num2) && (Grid.BuildMasks[num2] & buildFlags) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
				{
					if (this.isCritter)
					{
						return Grid.InvalidCell;
					}
					if ((Grid.BuildMasks[num2] & Grid.BuildFlags.DupePassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
					{
						return Grid.InvalidCell;
					}
				}
			}
			foreach (CellOffset cellOffset2 in this.solidOffsets)
			{
				int num3 = Grid.OffsetCell(cell, cellOffset2.x, cellOffset2.y);
				if (Grid.IsValidCell(num3) && !Grid.Solid[num3])
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset in this.validNavOffsets)
			{
				int cell2 = Grid.OffsetCell(cell, navOffset.offset.x, navOffset.offset.y);
				if (!nav_table.IsValid(cell2, navOffset.navType))
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset2 in this.invalidNavOffsets)
			{
				int cell3 = Grid.OffsetCell(cell, navOffset2.offset.x, navOffset2.offset.y);
				if (nav_table.IsValid(cell3, navOffset2.navType))
				{
					return Grid.InvalidCell;
				}
			}
			if (this.start == NavType.Tube)
			{
				if (this.end == NavType.Tube)
				{
					GameObject gameObject = Grid.Objects[cell, 9];
					GameObject gameObject2 = Grid.Objects[num, 9];
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = gameObject ? gameObject.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink2 = gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					if (travelTubeUtilityNetworkLink)
					{
						int num4;
						int num5;
						travelTubeUtilityNetworkLink.GetCells(out num4, out num5);
						if (num != num4 && num != num5)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, num);
						if (utilityConnections == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(num, false) != utilityConnections)
						{
							return Grid.InvalidCell;
						}
					}
					else if (travelTubeUtilityNetworkLink2)
					{
						int num6;
						int num7;
						travelTubeUtilityNetworkLink2.GetCells(out num6, out num7);
						if (cell != num6 && cell != num7)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections2 = UtilityConnectionsExtensions.DirectionFromToCell(num, cell);
						if (utilityConnections2 == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(cell, false) != utilityConnections2)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						bool flag = this.startAxis == NavAxis.X;
						int cell4 = cell;
						for (int j = 0; j < 2; j++)
						{
							if ((flag && j == 0) || (!flag && j == 1))
							{
								int num8 = (this.x > 0) ? 1 : -1;
								for (int k = 0; k < Mathf.Abs(this.x); k++)
								{
									UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num8 > 0 && (connections & UtilityConnections.Right) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num8 < 0 && (connections & UtilityConnections.Left) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, num8, 0);
								}
							}
							else
							{
								int num9 = (this.y > 0) ? 1 : -1;
								for (int l = 0; l < Mathf.Abs(this.y); l++)
								{
									UtilityConnections connections2 = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num9 > 0 && (connections2 & UtilityConnections.Up) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num9 < 0 && (connections2 & UtilityConnections.Down) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, 0, num9);
								}
							}
						}
					}
				}
				else
				{
					UtilityConnections connections3 = Game.Instance.travelTubeSystem.GetConnections(cell, false);
					if (this.y > 0)
					{
						if (connections3 != UtilityConnections.Down)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x > 0)
					{
						if (connections3 != UtilityConnections.Left)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x < 0)
					{
						if (connections3 != UtilityConnections.Right)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						if (this.y >= 0)
						{
							return Grid.InvalidCell;
						}
						if (connections3 != UtilityConnections.Up)
						{
							return Grid.InvalidCell;
						}
					}
				}
			}
			else if (this.start == NavType.Floor && this.end == NavType.Tube)
			{
				int cell5 = Grid.OffsetCell(cell, this.x, this.y);
				if (Game.Instance.travelTubeSystem.GetConnections(cell5, false) != UtilityConnections.Up)
				{
					return Grid.InvalidCell;
				}
			}
			return num;
		}

		// Token: 0x04005944 RID: 22852
		public NavType start;

		// Token: 0x04005945 RID: 22853
		public NavType end;

		// Token: 0x04005946 RID: 22854
		public NavAxis startAxis;

		// Token: 0x04005947 RID: 22855
		public int x;

		// Token: 0x04005948 RID: 22856
		public int y;

		// Token: 0x04005949 RID: 22857
		public byte id;

		// Token: 0x0400594A RID: 22858
		public byte cost;

		// Token: 0x0400594B RID: 22859
		public bool isLooping;

		// Token: 0x0400594C RID: 22860
		public bool isEscape;

		// Token: 0x0400594D RID: 22861
		public string preAnim;

		// Token: 0x0400594E RID: 22862
		public string anim;

		// Token: 0x0400594F RID: 22863
		public float animSpeed;

		// Token: 0x04005950 RID: 22864
		public CellOffset[] voidOffsets;

		// Token: 0x04005951 RID: 22865
		public CellOffset[] solidOffsets;

		// Token: 0x04005952 RID: 22866
		public NavOffset[] validNavOffsets;

		// Token: 0x04005953 RID: 22867
		public NavOffset[] invalidNavOffsets;

		// Token: 0x04005954 RID: 22868
		public bool isCritter;
	}
}
