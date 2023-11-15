using System;
using System.Collections.Generic;

// Token: 0x020003F1 RID: 1009
public class NavGridUpdater
{
	// Token: 0x06001550 RID: 5456 RVA: 0x00070CE2 File Offset: 0x0006EEE2
	public static void InitializeNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type)
	{
		NavGridUpdater.MarkValidCells(nav_table, validators, bounding_offsets);
		NavGridUpdater.CreateLinks(nav_table, max_links_per_cell, links, transitions_by_nav_type, new Dictionary<int, int>());
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x00070CFC File Offset: 0x0006EEFC
	public static void UpdateNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions, HashSet<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateValidCells(dirty_nav_cells, nav_table, validators, bounding_offsets);
		NavGridUpdater.UpdateLinks(dirty_nav_cells, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00070D18 File Offset: 0x0006EF18
	private static void UpdateValidCells(HashSet<int> dirty_solid_cells, NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		foreach (int cell in dirty_solid_cells)
		{
			for (int i = 0; i < validators.Length; i++)
			{
				validators[i].UpdateCell(cell, nav_table, bounding_offsets);
			}
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x00070D7C File Offset: 0x0006EF7C
	private static void CreateLinksForCell(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		NavGridUpdater.CreateLinks(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00070D8C File Offset: 0x0006EF8C
	private static void UpdateLinks(HashSet<int> dirty_nav_cells, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		foreach (int cell in dirty_nav_cells)
		{
			NavGridUpdater.CreateLinksForCell(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x00070DE0 File Offset: 0x0006EFE0
	private static void CreateLinks(NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.CreateLinkWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00070E30 File Offset: 0x0006F030
	private static void CreateLinks(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		int num = cell * max_links_per_cell;
		int num2 = 0;
		for (int i = 0; i < 11; i++)
		{
			NavType nav_type = (NavType)i;
			NavGrid.Transition[] array = transitions_by_nav_type[i];
			if (array != null && nav_table.IsValid(cell, nav_type))
			{
				NavGrid.Transition[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					NavGrid.Transition transition;
					if ((transition = array2[j]).start == NavType.Teleport && teleport_transitions.ContainsKey(cell))
					{
						int num3;
						int num4;
						Grid.CellToXY(cell, out num3, out num4);
						int num5 = teleport_transitions[cell];
						int num6;
						int num7;
						Grid.CellToXY(teleport_transitions[cell], out num6, out num7);
						transition.x = num6 - num3;
						transition.y = num7 - num4;
					}
					int num8 = transition.IsValid(cell, nav_table);
					if (num8 != Grid.InvalidCell)
					{
						links[num] = new NavGrid.Link(num8, transition.start, transition.end, transition.id, transition.cost);
						num++;
						num2++;
					}
				}
			}
		}
		if (num2 >= max_links_per_cell)
		{
			Debug.LogError("Out of nav links. Need to increase maxLinksPerCell:" + max_links_per_cell.ToString());
		}
		links[num].link = Grid.InvalidCell;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x00070F5C File Offset: 0x0006F15C
	private static void MarkValidCells(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.MarkValidCellWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, bounding_offsets, validators));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x00070FA7 File Offset: 0x0006F1A7
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x00070FBC File Offset: 0x0006F1BC
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGridUpdater.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x04000BB0 RID: 2992
	public static int InvalidHandle = -1;

	// Token: 0x04000BB1 RID: 2993
	public static int InvalidIdx = -1;

	// Token: 0x04000BB2 RID: 2994
	public static int InvalidCell = -1;

	// Token: 0x02001081 RID: 4225
	private struct CreateLinkWorkItem : IWorkItem<object>
	{
		// Token: 0x060075D8 RID: 30168 RVA: 0x002CE0F2 File Offset: 0x002CC2F2
		public CreateLinkWorkItem(int start_cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.maxLinksPerCell = max_links_per_cell;
			this.links = links;
			this.transitionsByNavType = transitions_by_nav_type;
			this.teleportTransitions = teleport_transitions;
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x002CE124 File Offset: 0x002CC324
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				NavGridUpdater.CreateLinksForCell(this.startCell + i, this.navTable, this.maxLinksPerCell, this.links, this.transitionsByNavType, this.teleportTransitions);
			}
		}

		// Token: 0x04005955 RID: 22869
		private int startCell;

		// Token: 0x04005956 RID: 22870
		private NavTable navTable;

		// Token: 0x04005957 RID: 22871
		private int maxLinksPerCell;

		// Token: 0x04005958 RID: 22872
		private NavGrid.Link[] links;

		// Token: 0x04005959 RID: 22873
		private NavGrid.Transition[][] transitionsByNavType;

		// Token: 0x0400595A RID: 22874
		private Dictionary<int, int> teleportTransitions;
	}

	// Token: 0x02001082 RID: 4226
	private struct MarkValidCellWorkItem : IWorkItem<object>
	{
		// Token: 0x060075DA RID: 30170 RVA: 0x002CE16C File Offset: 0x002CC36C
		public MarkValidCellWorkItem(int start_cell, NavTable nav_table, CellOffset[] bounding_offsets, NavTableValidator[] validators)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.boundingOffsets = bounding_offsets;
			this.validators = validators;
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x002CE18C File Offset: 0x002CC38C
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				int cell = this.startCell + i;
				NavTableValidator[] array = this.validators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].UpdateCell(cell, this.navTable, this.boundingOffsets);
				}
			}
		}

		// Token: 0x0400595B RID: 22875
		private NavTable navTable;

		// Token: 0x0400595C RID: 22876
		private CellOffset[] boundingOffsets;

		// Token: 0x0400595D RID: 22877
		private NavTableValidator[] validators;

		// Token: 0x0400595E RID: 22878
		private int startCell;
	}
}
