using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000744 RID: 1860
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
	// Token: 0x06003355 RID: 13141 RVA: 0x00111258 File Offset: 0x0010F458
	private void Update()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i] != PathFinder.InvalidCell)
			{
				DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[i], Grid.SceneLayer.Background), 1f, 0f, true);
			}
		}
	}

	// Token: 0x04001ED2 RID: 7890
	public List<int> cells;
}
