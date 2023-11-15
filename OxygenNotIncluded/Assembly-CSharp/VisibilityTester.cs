using System;
using UnityEngine;

// Token: 0x02000A25 RID: 2597
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
	// Token: 0x06004DD4 RID: 19924 RVA: 0x001B483F File Offset: 0x001B2A3F
	public static void DestroyInstance()
	{
		VisibilityTester.Instance = null;
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001B4847 File Offset: 0x001B2A47
	protected override void OnPrefabInit()
	{
		VisibilityTester.Instance = this;
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001B4850 File Offset: 0x001B2A50
	private void Update()
	{
		if (SelectTool.Instance == null || SelectTool.Instance.selected == null || !this.enableTesting)
		{
			return;
		}
		int cell = Grid.PosToCell(SelectTool.Instance.selected);
		int mouseCell = DebugHandler.GetMouseCell();
		string text = "";
		text = text + "Source Cell: " + cell.ToString() + "\n";
		text = text + "Target Cell: " + mouseCell.ToString() + "\n";
		text = text + "Visible: " + Grid.VisibilityTest(cell, mouseCell, false).ToString();
		for (int i = 0; i < 10000; i++)
		{
			Grid.VisibilityTest(cell, mouseCell, false);
		}
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x040032BD RID: 12989
	public static VisibilityTester Instance;

	// Token: 0x040032BE RID: 12990
	public bool enableTesting;
}
