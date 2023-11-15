using System;
using UnityEngine;

// Token: 0x0200081F RID: 2079
public class PlaceTool : DragTool
{
	// Token: 0x06003C21 RID: 15393 RVA: 0x0014DB35 File Offset: 0x0014BD35
	public static void DestroyInstance()
	{
		PlaceTool.Instance = null;
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x0014DB3D File Offset: 0x0014BD3D
	protected override void OnPrefabInit()
	{
		PlaceTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x0014DB54 File Offset: 0x0014BD54
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.visualizer = new GameObject("PlaceToolVisualizer");
		this.visualizer.SetActive(false);
		this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.source.kAnimName)
		};
		kbatchedAnimController.initialAnim = this.source.animName;
		this.visualizer.SetActive(true);
		this.ShowToolTip();
		base.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0014DC3C File Offset: 0x0014BE3C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		UnityEngine.Object.Destroy(this.visualizer);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
		this.source = null;
		this.onPlacedCallback = null;
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0014DC9C File Offset: 0x0014BE9C
	public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
	{
		this.source = source;
		this.onPlacedCallback = onPlacedCallback;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x0014DCB8 File Offset: 0x0014BEB8
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.visualizer == null)
		{
			return;
		}
		bool flag = false;
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			this.onPlacedCallback(this.source, cell);
			flag = true;
		}
		if (flag)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0014DD04 File Offset: 0x0014BF04
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x0014DD07 File Offset: 0x0014BF07
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x0014DD19 File Offset: 0x0014BF19
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x0014DD2C File Offset: 0x0014BF2C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			component.TintColour = Color.white;
		}
		else
		{
			component.TintColour = Color.red;
		}
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x0014DD98 File Offset: 0x0014BF98
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x0014DDD2 File Offset: 0x0014BFD2
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x04002784 RID: 10116
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04002785 RID: 10117
	private Action<Placeable, int> onPlacedCallback;

	// Token: 0x04002786 RID: 10118
	private Placeable source;

	// Token: 0x04002787 RID: 10119
	private ToolTip tooltip;

	// Token: 0x04002788 RID: 10120
	public static PlaceTool Instance;

	// Token: 0x04002789 RID: 10121
	private bool active;
}
