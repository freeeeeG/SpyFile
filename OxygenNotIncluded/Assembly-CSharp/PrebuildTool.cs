using System;
using UnityEngine;

// Token: 0x02000820 RID: 2080
public class PrebuildTool : InterfaceTool
{
	// Token: 0x06003C2E RID: 15406 RVA: 0x0014DDE1 File Offset: 0x0014BFE1
	public static void DestroyInstance()
	{
		PrebuildTool.Instance = null;
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x0014DDE9 File Offset: 0x0014BFE9
	protected override void OnPrefabInit()
	{
		PrebuildTool.Instance = this;
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0014DDF1 File Offset: 0x0014BFF1
	protected override void OnActivateTool()
	{
		this.viewMode = this.def.ViewMode;
		base.OnActivateTool();
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x0014DE0A File Offset: 0x0014C00A
	public void Activate(BuildingDef def, string errorMessage)
	{
		this.def = def;
		PlayerController.Instance.ActivateTool(this);
		PrebuildToolHoverTextCard component = base.GetComponent<PrebuildToolHoverTextCard>();
		component.errorMessage = errorMessage;
		component.currentDef = def;
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x0014DE31 File Offset: 0x0014C031
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		UISounds.PlaySound(UISounds.Sound.Negative);
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x0400278A RID: 10122
	public static PrebuildTool Instance;

	// Token: 0x0400278B RID: 10123
	private BuildingDef def;
}
