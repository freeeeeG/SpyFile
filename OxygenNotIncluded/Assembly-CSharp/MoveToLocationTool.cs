using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public class MoveToLocationTool : InterfaceTool
{
	// Token: 0x06003C14 RID: 15380 RVA: 0x0014D8DE File Offset: 0x0014BADE
	public static void DestroyInstance()
	{
		MoveToLocationTool.Instance = null;
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0014D8E6 File Offset: 0x0014BAE6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MoveToLocationTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0014D907 File Offset: 0x0014BB07
	public void Activate(Navigator navigator)
	{
		this.targetNavigator = navigator;
		this.targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x0014D922 File Offset: 0x0014BB22
	public void Activate(Movable movable)
	{
		this.targetNavigator = null;
		this.targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C18 RID: 15384 RVA: 0x0014D940 File Offset: 0x0014BB40
	public bool CanMoveTo(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			return this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell);
		}
		return this.targetMovable != null && this.targetMovable.CanMoveTo(target_cell);
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x0014D994 File Offset: 0x0014BB94
	private void SetMoveToLocation(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
			if (smi != null)
			{
				smi.MoveToLocation(target_cell);
				return;
			}
		}
		else if (this.targetMovable != null)
		{
			this.targetMovable.MoveToLocation(target_cell);
		}
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x0014D9E0 File Offset: 0x0014BBE0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.visualizer.gameObject.SetActive(true);
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x0014D9FC File Offset: 0x0014BBFC
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (this.targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
		}
		this.visualizer.gameObject.SetActive(false);
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x0014DA54 File Offset: 0x0014BC54
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.targetNavigator != null || this.targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (this.CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
				this.SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
				return;
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x0014DAC8 File Offset: 0x0014BCC8
	private void RefreshColor()
	{
		Color white = new Color(0.91f, 0.21f, 0.2f);
		if (this.CanMoveTo(DebugHandler.GetMouseCell()))
		{
			white = Color.white;
		}
		this.SetColor(this.visualizer, white);
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x0014DB0B File Offset: 0x0014BD0B
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.RefreshColor();
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x0014DB1A File Offset: 0x0014BD1A
	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}

	// Token: 0x04002781 RID: 10113
	public static MoveToLocationTool Instance;

	// Token: 0x04002782 RID: 10114
	private Navigator targetNavigator;

	// Token: 0x04002783 RID: 10115
	private Movable targetMovable;
}
