using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000817 RID: 2071
public class DragTool : InterfaceTool
{
	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06003BA1 RID: 15265 RVA: 0x0014B1DD File Offset: 0x001493DD
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0014B1E5 File Offset: 0x001493E5
	protected virtual DragTool.Mode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0014B1ED File Offset: 0x001493ED
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
		this.SetMode(this.mode);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0014B208 File Offset: 0x00149408
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.RemoveCurrentAreaText();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0014B234 File Offset: 0x00149434
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
			this.areaVisualizer.transform.SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0014B2F8 File Offset: 0x001494F8
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0014B301 File Offset: 0x00149501
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x0014B338 File Offset: 0x00149538
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.dragging = true;
		this.downPos = cursor_pos;
		this.cellChangedSinceDown = false;
		this.previousCursorPos = cursor_pos;
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			base.SetCurrentVirtualInputModuleMousMovementMode(true, delegate(VirtualInputModule module)
			{
				this.currentVirtualInputInUse = module;
			});
		}
		this.hasFocus = true;
		this.RemoveCurrentAreaText();
		if (this.areaVisualizerTextPrefab != null)
		{
			this.areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", this.areaVisualizerTextPrefab);
			NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = this.areaColour;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.AddDragPoint(cursor_pos);
				return;
			}
		}
		else if (mode == DragTool.Mode.Box || mode == DragTool.Mode.Line)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(false);
			}
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(true);
				this.areaVisualizer.transform.SetPosition(cursor_pos);
				this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
			}
		}
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x0014B4A5 File Offset: 0x001496A5
	public void RemoveCurrentAreaText()
	{
		if (this.areaVisualizerText != Guid.Empty)
		{
			NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
			this.areaVisualizerText = Guid.Empty;
		}
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x0014B4D4 File Offset: 0x001496D4
	public void CancelDragging()
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x0014B564 File Offset: 0x00149764
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Line || Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)))
		{
			cursor_pos = this.SnapToLine(cursor_pos);
		}
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
			int num;
			int num2;
			Grid.PosToXY(this.downPos, out num, out num2);
			int num3 = num;
			int num4 = num2;
			int num5;
			int num6;
			Grid.PosToXY(cursor_pos, out num5, out num6);
			if (num5 < num)
			{
				global::Util.Swap<int>(ref num, ref num5);
			}
			if (num6 < num2)
			{
				global::Util.Swap<int>(ref num2, ref num6);
			}
			for (int i = num2; i <= num6; i++)
			{
				for (int j = num; j <= num5; j++)
				{
					int cell = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
					{
						int num7 = i - num4;
						int num8 = j - num3;
						num7 = Mathf.Abs(num7);
						num8 = Mathf.Abs(num8);
						this.OnDragTool(cell, num7 + num8);
					}
				}
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound(), false));
			this.OnDragComplete(this.downPos, cursor_pos);
		}
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x0014B6F3 File Offset: 0x001498F3
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0014B6FA File Offset: 0x001498FA
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x0014B701 File Offset: 0x00149901
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x0014B708 File Offset: 0x00149908
	protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
	{
		position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
		position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
		return position;
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x0014B768 File Offset: 0x00149968
	protected Vector3 SnapToLine(Vector3 cursorPos)
	{
		Vector3 vector = cursorPos - this.downPos;
		if (this.canChangeDragAxis || (!this.canChangeDragAxis && !this.cellChangedSinceDown) || this.dragAxis == DragTool.DragAxis.Invalid)
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
			if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y))
			{
				this.dragAxis = DragTool.DragAxis.Vertical;
			}
			else
			{
				this.dragAxis = DragTool.DragAxis.Horizontal;
			}
		}
		DragTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis != DragTool.DragAxis.Horizontal)
		{
			if (dragAxis == DragTool.DragAxis.Vertical)
			{
				cursorPos.x = this.downPos.x;
				if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.y) > (float)(this.lineModeMaxLength - 1))
				{
					cursorPos.y = this.downPos.y + Mathf.Sign(vector.y) * (float)(this.lineModeMaxLength - 1);
				}
			}
		}
		else
		{
			cursorPos.y = this.downPos.y;
			if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.x) > (float)(this.lineModeMaxLength - 1))
			{
				cursorPos.x = this.downPos.x + Mathf.Sign(vector.x) * (float)(this.lineModeMaxLength - 1);
			}
		}
		return cursorPos;
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x0014B8A4 File Offset: 0x00149AA4
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.dragging && (Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)) || this.GetMode() == DragTool.Mode.Line))
		{
			cursorPos = this.SnapToLine(cursorPos);
		}
		else
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
		}
		base.OnMouseMove(cursorPos);
		if (!this.dragging)
		{
			return;
		}
		if (Grid.PosToCell(cursorPos) != Grid.PosToCell(this.downPos))
		{
			this.cellChangedSinceDown = true;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode != DragTool.Mode.Brush)
		{
			if (mode - DragTool.Mode.Box <= 1)
			{
				Vector2 vector = Vector3.Max(this.downPos, cursorPos);
				Vector2 vector2 = Vector3.Min(this.downPos, cursorPos);
				vector = base.GetWorldRestrictedPosition(vector);
				vector2 = base.GetWorldRestrictedPosition(vector2);
				vector = base.GetRegularizedPos(vector, false);
				vector2 = base.GetRegularizedPos(vector2, true);
				Vector2 vector3 = vector - vector2;
				Vector2 vector4 = (vector + vector2) * 0.5f;
				this.areaVisualizer.transform.SetPosition(new Vector2(vector4.x, vector4.y));
				int num = (int)(vector.x - vector2.x + (vector.y - vector2.y) - 1f);
				if (this.areaVisualizerSpriteRenderer.size != vector3)
				{
					string sound = GlobalAssets.GetSound(this.GetDragSound(), false);
					if (sound != null)
					{
						Vector3 position = this.areaVisualizer.transform.GetPosition();
						position.z = 0f;
						EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
						instance.setParameterByName("tileCount", (float)num, false);
						SoundEvent.EndOneShot(instance);
					}
				}
				this.areaVisualizerSpriteRenderer.size = vector3;
				if (this.areaVisualizerText != Guid.Empty)
				{
					Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
					LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
					component.text = string.Format(UI.TOOLS.TOOL_AREA_FMT, vector2I.x, vector2I.y, vector2I.x * vector2I.y);
					Vector2 v = vector4;
					component.transform.SetPosition(v);
				}
			}
		}
		else
		{
			this.AddDragPoints(cursorPos, this.previousCursorPos);
			if (this.areaVisualizerText != Guid.Empty)
			{
				int dragLength = this.GetDragLength();
				LocText component2 = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
				component2.text = string.Format(UI.TOOLS.TOOL_LENGTH_FMT, dragLength);
				Vector3 vector5 = Grid.CellToPos(Grid.PosToCell(cursorPos));
				vector5 += new Vector3(0f, 1f, 0f);
				component2.transform.SetPosition(vector5);
			}
		}
		this.previousCursorPos = cursorPos;
	}

	// Token: 0x06003BB2 RID: 15282 RVA: 0x0014BBA4 File Offset: 0x00149DA4
	protected virtual void OnDragTool(int cell, int distFromOrigin)
	{
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x0014BBA6 File Offset: 0x00149DA6
	protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
	{
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x0014BBA8 File Offset: 0x00149DA8
	protected virtual int GetDragLength()
	{
		return 0;
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x0014BBAC File Offset: 0x00149DAC
	private void AddDragPoint(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			this.OnDragTool(cell, 0);
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0014BBEC File Offset: 0x00149DEC
	private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		Vector3 a = cursorPos - previousCursorPos;
		float magnitude = a.magnitude;
		float num = Grid.CellSizeInMeters * 0.25f;
		int num2 = 1 + (int)(magnitude / num);
		a.Normalize();
		for (int i = 0; i < num2; i++)
		{
			Vector3 cursorPos2 = previousCursorPos + a * ((float)i * num);
			this.AddDragPoint(cursorPos2);
		}
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0014BC61 File Offset: 0x00149E61
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0014BC81 File Offset: 0x00149E81
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0014BCA4 File Offset: 0x00149EA4
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0014BD08 File Offset: 0x00149F08
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0014BD30 File Offset: 0x00149F30
	protected void SetMode(DragTool.Mode newMode)
	{
		this.mode = newMode;
		switch (this.mode)
		{
		case DragTool.Mode.Brush:
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(false);
			}
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			base.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Box:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Box;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Line:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Line;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		default:
			return;
		}
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x0014BE10 File Offset: 0x0014A010
	public override void OnFocus(bool focus)
	{
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(focus);
			}
			this.hasFocus = focus;
			return;
		}
		if (mode - DragTool.Mode.Box > 1)
		{
			return;
		}
		if (this.visualizer != null && !this.dragging)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = (focus || this.dragging);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x0014BE84 File Offset: 0x0014A084
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x0014BE8D File Offset: 0x0014A08D
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x04002743 RID: 10051
	[SerializeField]
	private Texture2D boxCursor;

	// Token: 0x04002744 RID: 10052
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002745 RID: 10053
	[SerializeField]
	private GameObject areaVisualizerTextPrefab;

	// Token: 0x04002746 RID: 10054
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002747 RID: 10055
	protected SpriteRenderer areaVisualizerSpriteRenderer;

	// Token: 0x04002748 RID: 10056
	protected Guid areaVisualizerText;

	// Token: 0x04002749 RID: 10057
	protected Vector3 placementPivot;

	// Token: 0x0400274A RID: 10058
	protected bool interceptNumberKeysForPriority;

	// Token: 0x0400274B RID: 10059
	private bool dragging;

	// Token: 0x0400274C RID: 10060
	private Vector3 previousCursorPos;

	// Token: 0x0400274D RID: 10061
	private DragTool.Mode mode = DragTool.Mode.Box;

	// Token: 0x0400274E RID: 10062
	private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;

	// Token: 0x0400274F RID: 10063
	protected bool canChangeDragAxis = true;

	// Token: 0x04002750 RID: 10064
	protected int lineModeMaxLength = -1;

	// Token: 0x04002751 RID: 10065
	protected Vector3 downPos;

	// Token: 0x04002752 RID: 10066
	private bool cellChangedSinceDown;

	// Token: 0x04002753 RID: 10067
	private VirtualInputModule currentVirtualInputInUse;

	// Token: 0x020015ED RID: 5613
	private enum DragAxis
	{
		// Token: 0x040069FF RID: 27135
		Invalid = -1,
		// Token: 0x04006A00 RID: 27136
		None,
		// Token: 0x04006A01 RID: 27137
		Horizontal,
		// Token: 0x04006A02 RID: 27138
		Vertical
	}

	// Token: 0x020015EE RID: 5614
	public enum Mode
	{
		// Token: 0x04006A04 RID: 27140
		Brush,
		// Token: 0x04006A05 RID: 27141
		Box,
		// Token: 0x04006A06 RID: 27142
		Line
	}
}
