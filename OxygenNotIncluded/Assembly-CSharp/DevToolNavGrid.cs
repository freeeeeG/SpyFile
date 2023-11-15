using System;
using System.Linq;
using ImGuiNET;

// Token: 0x02000562 RID: 1378
public class DevToolNavGrid : DevTool
{
	// Token: 0x06002190 RID: 8592 RVA: 0x000B5A0E File Offset: 0x000B3C0E
	public DevToolNavGrid()
	{
		DevToolNavGrid.Instance = this;
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000B5A1C File Offset: 0x000B3C1C
	private bool Init()
	{
		if (Pathfinding.Instance == null)
		{
			return false;
		}
		if (this.navGridNames != null)
		{
			return true;
		}
		this.navGridNames = (from x in Pathfinding.Instance.GetNavGrids()
		select x.id).ToArray<string>();
		return true;
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000B5A7C File Offset: 0x000B3C7C
	protected override void RenderTo(DevPanel panel)
	{
		if (this.Init())
		{
			this.Contents();
			return;
		}
		ImGui.Text("Game not initialized");
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000B5A97 File Offset: 0x000B3C97
	public void SetCell(int cell)
	{
		this.selectedCell = cell;
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000B5AA0 File Offset: 0x000B3CA0
	private void Contents()
	{
		ImGui.Combo("Nav Grid ID", ref this.selectedNavGrid, this.navGridNames, this.navGridNames.Length);
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid(this.navGridNames[this.selectedNavGrid]);
		ImGui.Text("Max Links per cell: " + navGrid.maxLinksPerCell.ToString());
		ImGui.Spacing();
		if (ImGui.Button("Calculate Stats"))
		{
			this.linkStats = new int[navGrid.maxLinksPerCell];
			this.highestLinkCell = 0;
			this.highestLinkCount = 0;
			for (int i = 0; i < Grid.CellCount; i++)
			{
				int num = 0;
				for (int j = 0; j < navGrid.maxLinksPerCell; j++)
				{
					int num2 = i * navGrid.maxLinksPerCell + j;
					if (navGrid.Links[num2].link == Grid.InvalidCell)
					{
						break;
					}
					num++;
				}
				if (num > this.highestLinkCount)
				{
					this.highestLinkCell = i;
					this.highestLinkCount = num;
				}
				this.linkStats[num]++;
			}
		}
		ImGui.SameLine();
		if (ImGui.Button("Clear"))
		{
			this.linkStats = null;
		}
		if (this.linkStats != null)
		{
			ImGui.Text("Highest link count: " + this.highestLinkCount.ToString());
			ImGui.Text(string.Format("Utilized percentage: {0} %", (float)this.highestLinkCount / (float)navGrid.maxLinksPerCell * 100f));
			ImGui.SameLine();
			if (ImGui.Button(string.Format("Select {0}", this.highestLinkCell)))
			{
				this.selectedCell = this.highestLinkCell;
			}
			for (int k = 0; k < this.linkStats.Length; k++)
			{
				if (this.linkStats[k] > 0)
				{
					ImGui.Text(string.Format("\t{0}: {1}", k, this.linkStats[k]));
				}
			}
		}
		ImGui.Spacing();
		int num3;
		int num4;
		Grid.CellToXY(this.selectedCell, out num3, out num4);
		ImGui.Text(string.Format("Selected Cell: {0} ({1},{2})", this.selectedCell, num3, num4));
		if (Grid.IsValidCell(this.selectedCell) && navGrid.Links != null && navGrid.Links.Length > navGrid.maxLinksPerCell * this.selectedCell)
		{
			for (int l = 0; l < navGrid.maxLinksPerCell; l++)
			{
				int num5 = this.selectedCell * navGrid.maxLinksPerCell + l;
				NavGrid.Link link = navGrid.Links[num5];
				if (link.link == Grid.InvalidCell)
				{
					break;
				}
				this.DrawLink(l, link, navGrid);
			}
		}
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000B5D48 File Offset: 0x000B3F48
	private void DrawLink(int idx, NavGrid.Link l, NavGrid navGrid)
	{
		NavGrid.Transition transition = navGrid.transitions[(int)l.transitionId];
		ImGui.Text(string.Format("   {0} -> {1} x:{2} y:{3} anim:{4} cost:{5}", new object[]
		{
			transition.start,
			transition.end,
			transition.x,
			transition.y,
			transition.anim,
			transition.cost
		}));
	}

	// Token: 0x04001304 RID: 4868
	private const string INVALID_OVERLAY_MODE_STR = "None";

	// Token: 0x04001305 RID: 4869
	private string[] navGridNames;

	// Token: 0x04001306 RID: 4870
	private int selectedNavGrid;

	// Token: 0x04001307 RID: 4871
	public static DevToolNavGrid Instance;

	// Token: 0x04001308 RID: 4872
	private int[] linkStats;

	// Token: 0x04001309 RID: 4873
	private int highestLinkCell;

	// Token: 0x0400130A RID: 4874
	private int highestLinkCount;

	// Token: 0x0400130B RID: 4875
	private int selectedCell;
}
