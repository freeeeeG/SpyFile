using System;
using System.Collections.Generic;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008ED RID: 2285
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06004209 RID: 16905 RVA: 0x001716A3 File Offset: 0x0016F8A3
	public string handlerName
	{
		get
		{
			return "PlayerController";
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600420A RID: 16906 RVA: 0x001716AA File Offset: 0x0016F8AA
	// (set) Token: 0x0600420B RID: 16907 RVA: 0x001716B2 File Offset: 0x0016F8B2
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600420C RID: 16908 RVA: 0x001716BB File Offset: 0x0016F8BB
	public InterfaceTool ActiveTool
	{
		get
		{
			return this.activeTool;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x0600420D RID: 16909 RVA: 0x001716C3 File Offset: 0x0016F8C3
	// (set) Token: 0x0600420E RID: 16910 RVA: 0x001716CA File Offset: 0x0016F8CA
	public static PlayerController Instance { get; private set; }

	// Token: 0x0600420F RID: 16911 RVA: 0x001716D2 File Offset: 0x0016F8D2
	public static void DestroyInstance()
	{
		PlayerController.Instance = null;
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x001716DC File Offset: 0x0016F8DC
	protected override void OnPrefabInit()
	{
		PlayerController.Instance = this;
		InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
		this.vim = UnityEngine.Object.FindObjectOfType<VirtualInputModule>(true);
		for (int i = 0; i < this.tools.Length; i++)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(this.tools[i].DlcIDs))
			{
				GameObject gameObject = Util.KInstantiate(this.tools[i].gameObject, base.gameObject, null);
				this.tools[i] = gameObject.GetComponent<InterfaceTool>();
				this.tools[i].gameObject.SetActive(true);
				this.tools[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x00171781 File Offset: 0x0016F981
	protected override void OnSpawn()
	{
		if (this.tools.Length == 0)
		{
			return;
		}
		this.ActivateTool(this.tools[0]);
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x0017179B File Offset: 0x0016F99B
	private void InitializeConfigs()
	{
	}

	// Token: 0x06004213 RID: 16915 RVA: 0x0017179D File Offset: 0x0016F99D
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06004214 RID: 16916 RVA: 0x001717AC File Offset: 0x0016F9AC
	public static Vector3 GetCursorPos(Vector3 mouse_pos)
	{
		RaycastHit raycastHit;
		Vector3 vector;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out raycastHit, float.PositiveInfinity, Game.BlockSelectionLayerMask))
		{
			vector = raycastHit.point;
		}
		else
		{
			mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			vector = Camera.main.ScreenToWorldPoint(mouse_pos);
		}
		float num = vector.x;
		float num2 = vector.y;
		num = Mathf.Max(num, 0f);
		num = Mathf.Min(num, Grid.WidthInMeters);
		num2 = Mathf.Max(num2, 0f);
		num2 = Mathf.Min(num2, Grid.HeightInMeters);
		vector.x = num;
		vector.y = num2;
		return vector;
	}

	// Token: 0x06004215 RID: 16917 RVA: 0x00171860 File Offset: 0x0016FA60
	private void UpdateHover()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			this.activeTool.OnFocus(!current.IsPointerOverGameObject());
		}
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x00171890 File Offset: 0x0016FA90
	private void Update()
	{
		this.UpdateDrag();
		if (this.activeTool && this.activeTool.enabled)
		{
			this.UpdateHover();
			Vector3 cursorPos = this.GetCursorPos();
			if (cursorPos != this.prevMousePos)
			{
				this.prevMousePos = cursorPos;
				this.activeTool.OnMouseMove(cursorPos);
			}
		}
		if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			this.DebugHidingCursor = !this.DebugHidingCursor;
			Cursor.visible = !this.DebugHidingCursor;
			HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
		}
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x0017193F File Offset: 0x0016FB3F
	private void OnCleanup()
	{
		Global.GetInputManager().usedMenus.Remove(this);
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x00171952 File Offset: 0x0016FB52
	private void LateUpdate()
	{
		if (this.queueStopDrag)
		{
			this.queueStopDrag = false;
			this.dragging = false;
			this.dragAction = global::Action.Invalid;
			this.dragDelta = Vector3.zero;
			this.worldDragDelta = Vector3.zero;
		}
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x00171988 File Offset: 0x0016FB88
	public void ActivateTool(InterfaceTool tool)
	{
		if (this.activeTool == tool)
		{
			return;
		}
		this.DeactivateTool(tool);
		this.activeTool = tool;
		this.activeTool.enabled = true;
		this.activeTool.gameObject.SetActive(true);
		this.activeTool.ActivateTool();
		this.UpdateHover();
	}

	// Token: 0x0600421A RID: 16922 RVA: 0x001719E0 File Offset: 0x0016FBE0
	public void ToolDeactivated(InterfaceTool tool)
	{
		if (this.activeTool == tool && this.activeTool != null)
		{
			this.DeactivateTool(null);
		}
		if (this.activeTool == null)
		{
			this.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x0600421B RID: 16923 RVA: 0x00171A1E File Offset: 0x0016FC1E
	private void DeactivateTool(InterfaceTool new_tool = null)
	{
		if (this.activeTool != null)
		{
			this.activeTool.enabled = false;
			this.activeTool.gameObject.SetActive(false);
			InterfaceTool interfaceTool = this.activeTool;
			this.activeTool = null;
			interfaceTool.DeactivateTool(new_tool);
		}
	}

	// Token: 0x0600421C RID: 16924 RVA: 0x00171A5E File Offset: 0x0016FC5E
	public bool IsUsingDefaultTool()
	{
		return this.tools.Length != 0 && this.activeTool == this.tools[0];
	}

	// Token: 0x0600421D RID: 16925 RVA: 0x00171A7E File Offset: 0x0016FC7E
	private void StartDrag(global::Action action)
	{
		if (this.dragAction == global::Action.Invalid)
		{
			this.dragAction = action;
			this.startDragPos = KInputManager.GetMousePos();
			this.startDragTime = Time.unscaledTime;
		}
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x00171AA8 File Offset: 0x0016FCA8
	private void UpdateDrag()
	{
		this.dragDelta = Vector2.zero;
		Vector3 mousePos = KInputManager.GetMousePos();
		if (!this.dragging && this.CanDrag() && ((mousePos - this.startDragPos).sqrMagnitude > 36f || Time.unscaledTime - this.startDragTime > 0.3f))
		{
			this.dragging = true;
		}
		if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging)
		{
			return;
		}
		if (this.dragging)
		{
			this.dragDelta = mousePos - this.startDragPos;
			this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
			this.startDragPos = mousePos;
		}
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x00171B6E File Offset: 0x0016FD6E
	private void StopDrag(global::Action action)
	{
		if (this.dragAction == action)
		{
			this.queueStopDrag = true;
			if (KInputManager.currentControllerIsGamepad)
			{
				this.dragging = false;
			}
		}
	}

	// Token: 0x06004220 RID: 16928 RVA: 0x00171B90 File Offset: 0x0016FD90
	public void CancelDragging()
	{
		this.queueStopDrag = true;
		if (this.activeTool != null)
		{
			DragTool dragTool = this.activeTool as DragTool;
			if (dragTool != null)
			{
				dragTool.CancelDragging();
			}
		}
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x00171BCD File Offset: 0x0016FDCD
	public void OnCancelInput()
	{
		this.CancelDragging();
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x00171BD8 File Offset: 0x0016FDD8
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.ToggleScreenshotMode))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		if (DebugHandler.HideUI && e.TryConsume(global::Action.Escape))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StartDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StartDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StartDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
		pointerEventData.position = KInputManager.GetMousePos();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				return;
			}
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
		{
			this.activeTool.OnLeftClickDown(this.GetCursorPos());
			return;
		}
		if (e.IsAction(global::Action.MouseRight))
		{
			this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
			return;
		}
		this.activeTool.OnKeyDown(e);
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x00171D14 File Offset: 0x0016FF14
	public void OnKeyUp(KButtonEvent e)
	{
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StopDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StopDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StopDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		if (!this.activeTool.hasFocus)
		{
			return;
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
		else
		{
			if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x00171E45 File Offset: 0x00170045
	public bool ConsumeIfNotDragging(KButtonEvent e, global::Action action)
	{
		return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x00171E61 File Offset: 0x00170061
	public bool IsDragging()
	{
		return this.dragging && this.CanDrag();
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x00171E73 File Offset: 0x00170073
	public bool CanDrag()
	{
		return this.draggingAllowed && this.dragAction > global::Action.Invalid;
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x00171E88 File Offset: 0x00170088
	public void AllowDragging(bool allow)
	{
		this.draggingAllowed = allow;
	}

	// Token: 0x06004228 RID: 16936 RVA: 0x00171E91 File Offset: 0x00170091
	public Vector3 GetDragDelta()
	{
		return this.dragDelta;
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x00171E99 File Offset: 0x00170099
	public Vector3 GetWorldDragDelta()
	{
		if (!this.draggingAllowed)
		{
			return Vector3.zero;
		}
		return this.worldDragDelta;
	}

	// Token: 0x04002B23 RID: 11043
	[SerializeField]
	private global::Action defaultConfigKey;

	// Token: 0x04002B24 RID: 11044
	[SerializeField]
	private List<InterfaceToolConfig> interfaceConfigs;

	// Token: 0x04002B26 RID: 11046
	public InterfaceTool[] tools;

	// Token: 0x04002B27 RID: 11047
	private InterfaceTool activeTool;

	// Token: 0x04002B28 RID: 11048
	public VirtualInputModule vim;

	// Token: 0x04002B2A RID: 11050
	private bool DebugHidingCursor;

	// Token: 0x04002B2B RID: 11051
	private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0f, 0f);

	// Token: 0x04002B2C RID: 11052
	private const float MIN_DRAG_DIST_SQR = 36f;

	// Token: 0x04002B2D RID: 11053
	private const float MIN_DRAG_TIME = 0.3f;

	// Token: 0x04002B2E RID: 11054
	private global::Action dragAction;

	// Token: 0x04002B2F RID: 11055
	private bool draggingAllowed = true;

	// Token: 0x04002B30 RID: 11056
	private bool dragging;

	// Token: 0x04002B31 RID: 11057
	private bool queueStopDrag;

	// Token: 0x04002B32 RID: 11058
	private Vector3 startDragPos;

	// Token: 0x04002B33 RID: 11059
	private float startDragTime;

	// Token: 0x04002B34 RID: 11060
	private Vector3 dragDelta;

	// Token: 0x04002B35 RID: 11061
	private Vector3 worldDragDelta;
}
