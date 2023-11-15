using System;
using UnityEngine;

// Token: 0x02000811 RID: 2065
public class CopySettingsTool : DragTool
{
	// Token: 0x06003B64 RID: 15204 RVA: 0x0014A068 File Offset: 0x00148268
	public static void DestroyInstance()
	{
		CopySettingsTool.Instance = null;
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x0014A070 File Offset: 0x00148270
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CopySettingsTool.Instance = this;
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x0014A07E File Offset: 0x0014827E
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x0014A08B File Offset: 0x0014828B
	public void SetSourceObject(GameObject sourceGameObject)
	{
		this.sourceGameObject = sourceGameObject;
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0014A094 File Offset: 0x00148294
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.sourceGameObject == null)
		{
			return;
		}
		if (Grid.IsValidCell(cell))
		{
			CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
		}
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x0014A0BA File Offset: 0x001482BA
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x0014A0C2 File Offset: 0x001482C2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.sourceGameObject = null;
	}

	// Token: 0x04002734 RID: 10036
	public static CopySettingsTool Instance;

	// Token: 0x04002735 RID: 10037
	public GameObject Placer;

	// Token: 0x04002736 RID: 10038
	private GameObject sourceGameObject;
}
