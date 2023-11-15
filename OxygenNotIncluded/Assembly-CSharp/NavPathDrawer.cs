using System;
using UnityEngine;

// Token: 0x020004DB RID: 1243
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06001C97 RID: 7319 RVA: 0x00098FD7 File Offset: 0x000971D7
	// (set) Token: 0x06001C98 RID: 7320 RVA: 0x00098FDE File Offset: 0x000971DE
	public static NavPathDrawer Instance { get; private set; }

	// Token: 0x06001C99 RID: 7321 RVA: 0x00098FE6 File Offset: 0x000971E6
	public static void DestroyInstance()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x00098FF0 File Offset: 0x000971F0
	protected override void OnPrefabInit()
	{
		Shader shader = Shader.Find("Lines/Colored Blended");
		this.material = new Material(shader);
		NavPathDrawer.Instance = this;
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x0009901A File Offset: 0x0009721A
	protected override void OnCleanUp()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x00099022 File Offset: 0x00097222
	public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
	{
		this.navigatorPos = navigator_pos;
		this.navigatorPos.y = this.navigatorPos.y + 0.5f;
		this.path = path;
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x0009904E File Offset: 0x0009724E
	public Navigator GetNavigator()
	{
		return this.navigator;
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x00099056 File Offset: 0x00097256
	public void SetNavigator(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x0009905F File Offset: 0x0009725F
	public void ClearNavigator()
	{
		this.navigator = null;
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x00099068 File Offset: 0x00097268
	private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
	{
		if (path.nodes != null && path.nodes.Count > 1)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(navigator_pos);
			GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
			for (int i = 1; i < path.nodes.Count - 1; i++)
			{
				if ((int)Grid.WorldIdx[path.nodes[i].cell] == ClusterManager.Instance.activeWorldId && (int)Grid.WorldIdx[path.nodes[i + 1].cell] == ClusterManager.Instance.activeWorldId)
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(path.nodes[i].cell, path.nodes[i].navType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[i + 1].cell, path.nodes[i + 1].navType);
					GL.Vertex(navPos);
					GL.Vertex(navPos2);
				}
			}
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000991B4 File Offset: 0x000973B4
	private void OnPostRender()
	{
		this.DrawPath(this.path, this.navigatorPos, Color.white);
		this.path = default(PathFinder.Path);
		this.DebugDrawSelectedNavigator();
		if (this.navigator != null)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			PathFinderQuery query = PathFinderQueries.drawNavGridQuery.Reset(null);
			this.navigator.RunQuery(query);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x00099234 File Offset: 0x00097434
	private void DebugDrawSelectedNavigator()
	{
		if (!DebugHandler.DebugPathFinding)
		{
			return;
		}
		if (SelectTool.Instance == null)
		{
			return;
		}
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
		if (component == null)
		{
			return;
		}
		int mouseCell = DebugHandler.GetMouseCell();
		if (Grid.IsValidCell(mouseCell))
		{
			PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell(component), component.CurrentNavType, component.flags);
			PathFinder.Path path = default(PathFinder.Path);
			PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
			string text = "";
			text = text + "Source: " + Grid.PosToCell(component).ToString() + "\n";
			text = text + "Dest: " + mouseCell.ToString() + "\n";
			text = text + "Cost: " + path.cost.ToString();
			this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
			DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
		}
	}

	// Token: 0x04000FC9 RID: 4041
	private PathFinder.Path path;

	// Token: 0x04000FCA RID: 4042
	public Material material;

	// Token: 0x04000FCB RID: 4043
	private Vector3 navigatorPos;

	// Token: 0x04000FCC RID: 4044
	private Navigator navigator;
}
