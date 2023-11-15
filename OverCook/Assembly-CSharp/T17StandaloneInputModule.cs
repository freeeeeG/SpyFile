using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000835 RID: 2101
[AddComponentMenu("Event/T17 Standalone Input Module")]
public class T17StandaloneInputModule : PointerInputModule
{
	// Token: 0x06002845 RID: 10309 RVA: 0x000BCDD8 File Offset: 0x000BB1D8
	protected T17StandaloneInputModule()
	{
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06002846 RID: 10310 RVA: 0x000BCE00 File Offset: 0x000BB200
	public bool WasUsingMouse
	{
		get
		{
			return this.m_wasUsingMouse;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06002847 RID: 10311 RVA: 0x000BCE08 File Offset: 0x000BB208
	// (set) Token: 0x06002848 RID: 10312 RVA: 0x000BCE10 File Offset: 0x000BB210
	public bool MoveOneElementPerAxisPress
	{
		get
		{
			return this.moveOneElementPerAxisPress;
		}
		set
		{
			this.moveOneElementPerAxisPress = value;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06002849 RID: 10313 RVA: 0x000BCE19 File Offset: 0x000BB219
	// (set) Token: 0x0600284A RID: 10314 RVA: 0x000BCE21 File Offset: 0x000BB221
	public bool allowMouseInput
	{
		get
		{
			return this.m_allowMouseInput;
		}
		set
		{
			this.m_allowMouseInput = value;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x0600284B RID: 10315 RVA: 0x000BCE2A File Offset: 0x000BB22A
	// (set) Token: 0x0600284C RID: 10316 RVA: 0x000BCE32 File Offset: 0x000BB232
	public bool allowMouseInputIfTouchSupported
	{
		get
		{
			return this.m_allowMouseInputIfTouchSupported;
		}
		set
		{
			this.m_allowMouseInputIfTouchSupported = value;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600284D RID: 10317 RVA: 0x000BCE3B File Offset: 0x000BB23B
	protected bool isMouseSupported
	{
		get
		{
			return this.m_allowMouseInput && (!this.isTouchSupported || this.m_allowMouseInputIfTouchSupported);
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x0600284E RID: 10318 RVA: 0x000BCE61 File Offset: 0x000BB261
	// (set) Token: 0x0600284F RID: 10319 RVA: 0x000BCE69 File Offset: 0x000BB269
	public bool forceModuleActive
	{
		get
		{
			return this.m_ForceModuleActive;
		}
		set
		{
			this.m_ForceModuleActive = value;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06002850 RID: 10320 RVA: 0x000BCE72 File Offset: 0x000BB272
	// (set) Token: 0x06002851 RID: 10321 RVA: 0x000BCE7A File Offset: 0x000BB27A
	public float inputActionsPerSecond
	{
		get
		{
			return this.m_InputActionsPerSecond;
		}
		set
		{
			this.m_InputActionsPerSecond = value;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06002852 RID: 10322 RVA: 0x000BCE83 File Offset: 0x000BB283
	// (set) Token: 0x06002853 RID: 10323 RVA: 0x000BCE8B File Offset: 0x000BB28B
	public float repeatDelay
	{
		get
		{
			return this.m_RepeatDelay;
		}
		set
		{
			this.m_RepeatDelay = value;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06002854 RID: 10324 RVA: 0x000BCE94 File Offset: 0x000BB294
	// (set) Token: 0x06002855 RID: 10325 RVA: 0x000BCE9C File Offset: 0x000BB29C
	public bool InvertYAxis
	{
		get
		{
			return this.invertYAxis;
		}
		set
		{
			this.invertYAxis = value;
		}
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000BCEA5 File Offset: 0x000BB2A5
	protected override void Awake()
	{
		base.Awake();
		this.isTouchSupported = Input.touchSupported;
		this.m_lastScreenWidth = (float)Screen.width;
		this.m_lastScreenHeight = (float)Screen.height;
		this.m_wasFullscreen = Screen.fullScreen;
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000BCEDB File Offset: 0x000BB2DB
	public override void UpdateModule()
	{
		if (!this.m_bReadyForUse)
		{
			return;
		}
		if (this.isMouseSupported)
		{
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_MousePosition = Input.mousePosition;
		}
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x000BCF10 File Offset: 0x000BB310
	public override bool IsModuleSupported()
	{
		return true;
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000BCF14 File Offset: 0x000BB314
	public override bool ShouldActivateModule()
	{
		if (!base.ShouldActivateModule() || !this.m_bReadyForUse)
		{
			return false;
		}
		bool flag = this.m_ForceModuleActive;
		flag |= this.m_SelectButton.IsDown();
		flag |= this.m_CancelButton.IsDown();
		if (this.moveOneElementPerAxisPress)
		{
			flag |= (this.m_UpButton.JustPressed() || this.m_DownButton.JustPressed());
			flag |= (this.m_LeftButton.JustPressed() || this.m_RightButton.JustPressed());
		}
		else
		{
			flag |= !Mathf.Approximately(this.m_HorizontalAxis.GetValue(), 0f);
			flag |= !Mathf.Approximately(this.m_VerticalAxis.GetValue(), 0f);
		}
		flag |= this.IsMouseActive();
		if (this.isTouchSupported)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				flag |= (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary);
			}
		}
		return flag;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x000BD040 File Offset: 0x000BB440
	protected bool IsMouseActive()
	{
		bool flag = false;
		if (this.isMouseSupported)
		{
			flag |= ((this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f);
			flag |= Input.GetMouseButtonDown(0);
		}
		return flag;
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x000BD088 File Offset: 0x000BB488
	public override void ActivateModule()
	{
		base.ActivateModule();
		if (this.isMouseSupported)
		{
			Vector2 vector = Input.mousePosition;
			this.m_MousePosition = vector;
			this.m_LastMousePosition = vector;
		}
		GameObject gameObject = base.eventSystem.currentSelectedGameObject;
		if (gameObject == null)
		{
			gameObject = base.eventSystem.firstSelectedGameObject;
		}
		base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000BD0F5 File Offset: 0x000BB4F5
	public override void DeactivateModule()
	{
		base.DeactivateModule();
		base.ClearSelection();
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000BD104 File Offset: 0x000BB504
	public override void Process()
	{
		if (!this.m_bReadyForUse)
		{
			return;
		}
		bool flag = this.SendUpdateEventToSelectedObject();
		if (base.eventSystem.sendNavigationEvents)
		{
			if (!flag)
			{
				flag |= this.SendMoveEventToSelectedObject();
			}
			if (!flag)
			{
				this.SendSubmitEventToSelectedObject();
			}
		}
		if (!this.ProcessTouchEvents() && this.isMouseSupported && this.m_bProcessMouseEvents)
		{
			this.ProcessMouseEvent();
		}
		this.UpdateMouseKeyboardFocus();
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x000BD180 File Offset: 0x000BB580
	private bool ProcessTouchEvents()
	{
		if (!this.isTouchSupported)
		{
			return false;
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.type != TouchType.Indirect)
			{
				bool pressed;
				bool flag;
				PointerEventData touchPointerEventData = base.GetTouchPointerEventData(touch, out pressed, out flag);
				this.ProcessTouchPress(touchPointerEventData, pressed, flag);
				if (!flag)
				{
					this.ProcessMove(touchPointerEventData);
					this.ProcessDrag(touchPointerEventData);
				}
				else
				{
					base.RemovePointerData(touchPointerEventData);
				}
			}
		}
		return Input.touchCount > 0;
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x000BD20C File Offset: 0x000BB60C
	private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
	{
		GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
		if (pressed)
		{
			pointerEvent.eligibleForClick = true;
			pointerEvent.delta = Vector2.zero;
			pointerEvent.dragging = false;
			pointerEvent.useDragThreshold = true;
			pointerEvent.pressPosition = pointerEvent.position;
			pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
			base.DeselectIfSelectionChanged(gameObject, pointerEvent);
			if (pointerEvent.pointerEnter != gameObject)
			{
				base.HandlePointerExitAndEnter(pointerEvent, gameObject);
				pointerEvent.pointerEnter = gameObject;
			}
			GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
			if (gameObject2 == null)
			{
				gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			}
			float unscaledTime = Time.unscaledTime;
			if (gameObject2 == pointerEvent.lastPress)
			{
				float num = unscaledTime - pointerEvent.clickTime;
				if (num < 0.3f)
				{
					pointerEvent.clickCount++;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.clickTime = unscaledTime;
			}
			else
			{
				pointerEvent.clickCount = 1;
			}
			pointerEvent.pointerPress = gameObject2;
			pointerEvent.rawPointerPress = gameObject;
			pointerEvent.clickTime = unscaledTime;
			pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
			if (pointerEvent.pointerDrag != null)
			{
				ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
			}
		}
		if (released)
		{
			ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
			{
				ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
			}
			else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
			{
				ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointerEvent, ExecuteEvents.dropHandler);
			}
			pointerEvent.eligibleForClick = false;
			pointerEvent.pointerPress = null;
			pointerEvent.rawPointerPress = null;
			if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
			{
				ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
			}
			pointerEvent.dragging = false;
			pointerEvent.pointerDrag = null;
			if (pointerEvent.pointerDrag != null)
			{
				ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
			}
			pointerEvent.pointerDrag = null;
			ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
			pointerEvent.pointerEnter = null;
		}
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x000BD460 File Offset: 0x000BB860
	protected bool SendSubmitEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null || !this.m_bReadyForUse)
		{
			this.m_SelectButton.ClaimPressEvent();
			this.m_SelectButton.ClaimReleaseEvent();
			this.m_CancelButton.ClaimPressEvent();
			this.m_CancelButton.ClaimReleaseEvent();
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		if (this.m_SelectButton.JustPressed())
		{
			ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
		}
		if (this.m_CancelButton.JustPressed())
		{
			ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
		}
		return baseEventData.used;
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000BD518 File Offset: 0x000BB918
	private Vector2 GetRawMoveVector()
	{
		if (!this.m_bReadyForUse)
		{
			return Vector2.zero;
		}
		Vector2 zero = Vector2.zero;
		bool flag = false;
		bool flag2 = false;
		if (this.moveOneElementPerAxisPress)
		{
			float num = 0f;
			if (this.m_RightButton.JustPressed())
			{
				num = 1f;
			}
			else if (this.m_LeftButton.JustPressed())
			{
				num = -1f;
			}
			float num2 = 0f;
			if (this.m_UpButton.JustPressed())
			{
				num2 = 1f;
			}
			else if (this.m_DownButton.JustPressed())
			{
				num2 = -1f;
			}
			zero.x += num;
			zero.y += num2;
		}
		else
		{
			zero.x += this.m_HorizontalAxis.GetValue();
			zero.y += this.m_VerticalAxis.GetValue() * (float)((!this.invertYAxis) ? 1 : -1);
		}
		bool flag3 = this.m_UpButton.JustPressed();
		bool flag4 = this.m_DownButton.JustPressed();
		bool flag5 = this.m_LeftButton.JustPressed();
		bool flag6 = this.m_RightButton.JustPressed();
		flag2 |= (flag3 || flag4);
		flag |= (flag5 || flag6);
		if (flag)
		{
			if (zero.x < 0f)
			{
				zero.x = -1f;
			}
			else if (zero.x > 0f)
			{
				zero.x = 1f;
			}
			else
			{
				zero.x = ((!flag6) ? -1f : 1f);
			}
		}
		if (flag2)
		{
			if (zero.y < 0f)
			{
				zero.y = -1f;
			}
			else if (zero.y > 0f)
			{
				zero.y = 1f;
			}
			else
			{
				zero.y = ((!flag3) ? -1f : 1f);
			}
		}
		return zero;
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x000BD748 File Offset: 0x000BBB48
	protected bool SendMoveEventToSelectedObject()
	{
		if (!this.m_bReadyForUse)
		{
			return false;
		}
		float unscaledTime = Time.unscaledTime;
		this.m_bButtonPressed = false;
		Vector2 rawMoveVector = this.GetRawMoveVector();
		if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
		{
			this.m_ConsecutiveMoveCount = 0;
			this.m_transitioningFromMouse = false;
			return false;
		}
		this.m_bButtonPressed = true;
		if (this.m_wasUsingMouse || this.m_transitioningFromMouse)
		{
			this.ClearEventSystemLogicalButtons();
			return false;
		}
		bool flag = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
		bool flag2 = this.CheckButtonOrKeyMovement(unscaledTime);
		bool flag3 = flag2;
		if (!flag3)
		{
			if (this.m_RepeatDelay > 0f)
			{
				if (flag && this.m_ConsecutiveMoveCount == 1)
				{
					flag3 = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			else
			{
				flag3 = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
			}
		}
		if (!flag3)
		{
			return false;
		}
		AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
		if (axisEventData.moveDir == MoveDirection.None)
		{
			return false;
		}
		ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
		if (!flag)
		{
			this.m_ConsecutiveMoveCount = 0;
		}
		this.m_ConsecutiveMoveCount++;
		this.m_PrevActionTime = unscaledTime;
		this.m_LastMoveVector = rawMoveVector;
		return axisEventData.used;
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000BD8E8 File Offset: 0x000BBCE8
	private bool CheckButtonOrKeyMovement(float time)
	{
		if (!this.m_bReadyForUse)
		{
			return false;
		}
		bool flag = false;
		flag |= (this.m_UpButton.JustPressed() || this.m_DownButton.JustPressed());
		return flag | (this.m_LeftButton.JustPressed() || this.m_RightButton.JustPressed());
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000BD947 File Offset: 0x000BBD47
	protected void ProcessMouseEvent()
	{
		this.ProcessMouseEvent(0);
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000BD950 File Offset: 0x000BBD50
	protected void ProcessMouseEvent(int id)
	{
		PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
		PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		bool fullScreen = Screen.fullScreen;
		if (this.m_lastScreenWidth != num || this.m_lastScreenHeight != num2 || this.m_wasFullscreen != fullScreen)
		{
			this.m_lastScreenWidth = num;
			this.m_lastScreenHeight = num2;
			this.m_wasFullscreen = fullScreen;
			eventData.buttonData.delta = Vector2.zero;
			this.m_CurrentMouseDelta = 0f;
		}
		else
		{
			this.m_CurrentMouseDelta = eventData.buttonData.delta.magnitude;
		}
		this.ProcessMousePress(eventData);
		if (this.m_CurrentMouseDelta != 0f)
		{
			this.ProcessMove(eventData.buttonData);
			this.m_LastMousedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.m_wasUsingMouse = true;
		}
		else
		{
			GameObject x = null;
			GameObject y = null;
			if (this.m_wasUsingMouse)
			{
				x = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(eventData.buttonData.pointerEnter);
				y = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
			}
			if (!this.m_wasUsingMouse || (this.m_wasUsingMouse && x != y))
			{
				ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(eventData.buttonData.pointerEnter, eventData.buttonData, ExecuteEvents.pointerExitHandler);
			}
		}
		this.ProcessDrag(eventData.buttonData);
		this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
		this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
		this.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
		this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
		if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
			ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
		}
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000BDB74 File Offset: 0x000BBF74
	protected bool SendUpdateEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null || !this.m_bReadyForUse)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
		return baseEventData.used;
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000BDBC8 File Offset: 0x000BBFC8
	protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
	{
		PointerEventData buttonData = data.buttonData;
		GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
		if (data.PressedThisFrame())
		{
			buttonData.eligibleForClick = true;
			buttonData.delta = Vector2.zero;
			buttonData.dragging = false;
			buttonData.useDragThreshold = true;
			buttonData.pressPosition = buttonData.position;
			buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
			base.DeselectIfSelectionChanged(gameObject, buttonData);
			GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
			if (gameObject2 == null)
			{
				gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			}
			this.m_objWeSentPressEventTo = gameObject2;
			float unscaledTime = Time.unscaledTime;
			if (gameObject2 == buttonData.lastPress)
			{
				float num = unscaledTime - buttonData.clickTime;
				if (num < 0.3f)
				{
					buttonData.clickCount++;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.clickTime = unscaledTime;
			}
			else
			{
				buttonData.clickCount = 1;
			}
			buttonData.pointerPress = gameObject2;
			buttonData.rawPointerPress = gameObject;
			buttonData.clickTime = unscaledTime;
			buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
			if (buttonData.pointerDrag != null)
			{
				ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
			}
		}
		if (data.ReleasedThisFrame())
		{
			if (buttonData.pointerPress != null)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
			}
			else
			{
				this.ProcessMove(data.buttonData);
				gameObject = ExecuteEvents.GetEventHandler<ISelectHandler>(data.buttonData.pointerCurrentRaycast.gameObject);
				if (gameObject != ExecuteEvents.GetEventHandler<ISelectHandler>(this.m_objWeSentPressEventTo))
				{
					ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(this.m_objWeSentPressEventTo, buttonData, ExecuteEvents.pointerUpHandler);
				}
				if (gameObject != base.eventSystem.currentSelectedGameObject)
				{
					base.eventSystem.SetSelectedGameObject(null);
				}
				if (gameObject != null)
				{
					this.SetLastSelected(gameObject);
				}
				this.m_wasUsingMouse = true;
				buttonData = data.buttonData;
			}
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
			if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
			{
				ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
			}
			else if (buttonData.pointerDrag != null && buttonData.dragging)
			{
				ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
			}
			buttonData.eligibleForClick = false;
			buttonData.pointerPress = null;
			buttonData.rawPointerPress = null;
			if (buttonData.pointerDrag != null && buttonData.dragging)
			{
				ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
			}
			buttonData.dragging = false;
			buttonData.pointerDrag = null;
			if (gameObject != buttonData.pointerEnter)
			{
				base.HandlePointerExitAndEnter(buttonData, null);
				base.HandlePointerExitAndEnter(buttonData, gameObject);
			}
		}
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x000BDE9C File Offset: 0x000BC29C
	public void Initialize()
	{
		this.m_HorizontalAxis = PlayerInputLookup.GetUIValue(PlayerInputLookup.LogicalValueID.MovementX, PlayerInputLookup.Player.One);
		this.m_VerticalAxis = PlayerInputLookup.GetUIValue(PlayerInputLookup.LogicalValueID.MovementY, PlayerInputLookup.Player.One);
		this.m_UpButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIUp, PlayerInputLookup.Player.One);
		this.m_DownButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIDown, PlayerInputLookup.Player.One);
		this.m_LeftButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UILeft, PlayerInputLookup.Player.One);
		this.m_RightButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIRight, PlayerInputLookup.Player.One);
		this.m_SelectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PlayerInputLookup.Player.One);
		this.m_CancelButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		this.m_bReadyForUse = true;
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x000BDF1C File Offset: 0x000BC31C
	private void UpdateMouseKeyboardFocus()
	{
		if (base.eventSystem == null)
		{
			return;
		}
		T17EventSystem t17EventSystem = (T17EventSystem)base.eventSystem;
		if (t17EventSystem == null)
		{
			return;
		}
		if (this.m_CurrentMouseDelta != 0f && (!this.m_wasUsingMouse || this.m_LastMousedGameObject != base.eventSystem.currentSelectedGameObject))
		{
			base.eventSystem.SetSelectedGameObject(null);
			if (this.m_LastMousedGameObject != null)
			{
				Selectable selectable = this.m_LastMousedGameObject.GetComponent<Selectable>();
				if (selectable == null)
				{
					selectable = this.m_LastMousedGameObject.GetComponentInParent<Selectable>();
				}
				if (selectable != null)
				{
					this.m_LastSelectedGameObject = selectable.gameObject;
				}
			}
		}
		if (this.m_bButtonPressed && this.m_wasUsingMouse)
		{
			GameObject selectedGameObject;
			if (t17EventSystem.currentSelectedGameObject == null)
			{
				selectedGameObject = this.m_LastSelectedGameObject;
			}
			else
			{
				selectedGameObject = base.eventSystem.currentSelectedGameObject;
			}
			this.m_wasUsingMouse = false;
			base.ClearSelection();
			base.eventSystem.SetSelectedGameObject(selectedGameObject);
			this.m_transitioningFromMouse = true;
		}
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000BE044 File Offset: 0x000BC444
	public void SetLastSelected(GameObject _lastSelected)
	{
		this.m_LastMousedGameObject = _lastSelected;
		this.m_LastSelectedGameObject = _lastSelected;
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000BE054 File Offset: 0x000BC454
	protected void OnApplicationFocus(bool focus)
	{
		this.m_bProcessMouseEvents = focus;
		base.eventSystem.sendNavigationEvents = focus;
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000BE069 File Offset: 0x000BC469
	protected override void OnEnable()
	{
		base.OnEnable();
		this.OnApplicationFocus(Application.isFocused);
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000BE07C File Offset: 0x000BC47C
	public void ClearEventSystemLogicalButtons()
	{
		this.m_UpButton.ClaimPressEvent();
		this.m_UpButton.ClaimReleaseEvent();
		this.m_DownButton.ClaimPressEvent();
		this.m_DownButton.ClaimReleaseEvent();
		this.m_LeftButton.ClaimPressEvent();
		this.m_LeftButton.ClaimReleaseEvent();
		this.m_RightButton.ClaimPressEvent();
		this.m_RightButton.ClaimReleaseEvent();
		this.m_SelectButton.ClaimPressEvent();
		this.m_SelectButton.ClaimReleaseEvent();
		this.m_CancelButton.ClaimPressEvent();
		this.m_CancelButton.ClaimReleaseEvent();
	}

	// Token: 0x04001FD9 RID: 8153
	private bool m_bReadyForUse;

	// Token: 0x04001FDA RID: 8154
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04001FDB RID: 8155
	private ILogicalValue m_HorizontalAxis;

	// Token: 0x04001FDC RID: 8156
	private ILogicalValue m_VerticalAxis;

	// Token: 0x04001FDD RID: 8157
	private ILogicalButton m_UpButton;

	// Token: 0x04001FDE RID: 8158
	private ILogicalButton m_DownButton;

	// Token: 0x04001FDF RID: 8159
	private ILogicalButton m_LeftButton;

	// Token: 0x04001FE0 RID: 8160
	private ILogicalButton m_RightButton;

	// Token: 0x04001FE1 RID: 8161
	private ILogicalButton m_SelectButton;

	// Token: 0x04001FE2 RID: 8162
	private ILogicalButton m_CancelButton;

	// Token: 0x04001FE3 RID: 8163
	private bool isTouchSupported;

	// Token: 0x04001FE4 RID: 8164
	private GameObject m_LastSelectedGameObject;

	// Token: 0x04001FE5 RID: 8165
	private GameObject m_LastMousedGameObject;

	// Token: 0x04001FE6 RID: 8166
	private GameObject m_objWeSentPressEventTo;

	// Token: 0x04001FE7 RID: 8167
	private float m_lastScreenWidth;

	// Token: 0x04001FE8 RID: 8168
	private float m_lastScreenHeight;

	// Token: 0x04001FE9 RID: 8169
	private bool m_wasFullscreen;

	// Token: 0x04001FEA RID: 8170
	private float m_CurrentMouseDelta;

	// Token: 0x04001FEB RID: 8171
	private bool m_bButtonPressed;

	// Token: 0x04001FEC RID: 8172
	private bool m_wasUsingMouse;

	// Token: 0x04001FED RID: 8173
	private bool m_transitioningFromMouse;

	// Token: 0x04001FEE RID: 8174
	[SerializeField]
	[global::Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
	private bool moveOneElementPerAxisPress;

	// Token: 0x04001FEF RID: 8175
	protected bool m_bProcessMouseEvents = true;

	// Token: 0x04001FF0 RID: 8176
	private float m_PrevActionTime;

	// Token: 0x04001FF1 RID: 8177
	private Vector2 m_LastMoveVector;

	// Token: 0x04001FF2 RID: 8178
	private int m_ConsecutiveMoveCount;

	// Token: 0x04001FF3 RID: 8179
	private Vector2 m_LastMousePosition;

	// Token: 0x04001FF4 RID: 8180
	private Vector2 m_MousePosition;

	// Token: 0x04001FF5 RID: 8181
	[SerializeField]
	[global::Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
	private bool invertYAxis;

	// Token: 0x04001FF6 RID: 8182
	[SerializeField]
	[global::Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
	private float m_InputActionsPerSecond = 10f;

	// Token: 0x04001FF7 RID: 8183
	[SerializeField]
	[global::Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
	private float m_RepeatDelay;

	// Token: 0x04001FF8 RID: 8184
	[SerializeField]
	[global::Tooltip("Allows the mouse to be used to select elements.")]
	private bool m_allowMouseInput = true;

	// Token: 0x04001FF9 RID: 8185
	[SerializeField]
	[global::Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
	private bool m_allowMouseInputIfTouchSupported = true;

	// Token: 0x04001FFA RID: 8186
	[SerializeField]
	[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
	[global::Tooltip("Forces the module to always be active.")]
	private bool m_ForceModuleActive;
}
