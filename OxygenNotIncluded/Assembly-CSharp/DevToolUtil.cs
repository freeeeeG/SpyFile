using System;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public static class DevToolUtil
{
	// Token: 0x060021EA RID: 8682 RVA: 0x000BA434 File Offset: 0x000B8634
	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000BA446 File Offset: 0x000B8646
	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000BA457 File Offset: 0x000B8657
	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000BA45F File Offset: 0x000B865F
	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000BA467 File Offset: 0x000B8667
	public static string GenerateDevToolName(DevTool devTool)
	{
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000BA474 File Offset: 0x000B8674
	public static string GenerateDevToolName(Type devToolType)
	{
		string result;
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out result))
		{
			return result;
		}
		string text = devToolType.Name;
		if (text.StartsWith("DevTool_"))
		{
			text = text.Substring("DevTool_".Length);
		}
		else if (text.StartsWith("DevTool"))
		{
			text = text.Substring("DevTool".Length);
		}
		return text;
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000BA4E4 File Offset: 0x000B86E4
	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		return DevToolUtil.GetCellIndexFor(gameObject).HasValue;
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000BA500 File Offset: 0x000B8700
	public static void RevealAndFocus(GameObject gameObject)
	{
		Option<int> cellIndexFor = DevToolUtil.GetCellIndexFor(gameObject);
		if (!cellIndexFor.HasValue)
		{
			return;
		}
		DevToolUtil.RevealAndFocusAt(cellIndexFor.Value);
		if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			return;
		}
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000BA558 File Offset: 0x000B8758
	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000BA577 File Offset: 0x000B8777
	public static Option<int> GetCellIndexFor(GameObject gameObject)
	{
		if (gameObject.IsNullOrDestroyed())
		{
			return Option.None;
		}
		if (!gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
		{
			return Option.None;
		}
		return Grid.PosToCell(gameObject);
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000BA5B0 File Offset: 0x000B87B0
	public static Option<int> GetCellIndexForUniqueBuilding(string prefabId)
	{
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(true);
		if (array == null)
		{
			return Option.None;
		}
		foreach (BuildingComplete buildingComplete in array)
		{
			if (prefabId == buildingComplete.Def.PrefabID)
			{
				return buildingComplete.GetCell();
			}
		}
		return Option.None;
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000BA610 File Offset: 0x000B8810
	public static void RevealAndFocusAt(int cellIndex)
	{
		int num;
		int num2;
		Grid.CellToXY(cellIndex, out num, out num2);
		GridVisibility.Reveal(num + 2, num2 + 2, 10, 10f);
		DevToolUtil.FocusCameraOnCell(cellIndex);
		Option<int> cellIndexForUniqueBuilding = DevToolUtil.GetCellIndexForUniqueBuilding("Headquarters");
		if (cellIndexForUniqueBuilding.IsSome())
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(cellIndexForUniqueBuilding.Unwrap());
			float num3 = 2f / Vector3.Distance(a, b);
			for (float num4 = 0f; num4 < 1f; num4 += num3)
			{
				int num5;
				int num6;
				Grid.PosToXY(Vector3.Lerp(a, b, num4), out num5, out num6);
				GridVisibility.Reveal(num5 + 2, num6 + 2, 4, 4f);
			}
		}
	}
}
