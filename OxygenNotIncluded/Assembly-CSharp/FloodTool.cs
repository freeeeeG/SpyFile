using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class FloodTool : InterfaceTool
{
	// Token: 0x06003BD7 RID: 15319 RVA: 0x0014C600 File Offset: 0x0014A800
	public HashSet<int> Flood(int startCell)
	{
		HashSet<int> visited_cells = new HashSet<int>();
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.FloodFillConditional(startCell, this.floodCriteria, visited_cells, hashSet);
		return hashSet;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x0014C628 File Offset: 0x0014A828
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0014C648 File Offset: 0x0014A848
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.mouseCell = Grid.PosToCell(cursor_pos);
	}

	// Token: 0x04002759 RID: 10073
	public Func<int, bool> floodCriteria;

	// Token: 0x0400275A RID: 10074
	public Action<HashSet<int>> paintArea;

	// Token: 0x0400275B RID: 10075
	protected Color32 areaColour = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x0400275C RID: 10076
	protected int mouseCell = -1;
}
