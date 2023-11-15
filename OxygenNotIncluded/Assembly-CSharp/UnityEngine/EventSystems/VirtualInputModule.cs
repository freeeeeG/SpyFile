﻿using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000CC0 RID: 3264
	[AddComponentMenu("Event/Virtual Input Module")]
	public class VirtualInputModule : PointerInputModule, IInputHandler
	{
		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06006860 RID: 26720 RVA: 0x002779EF File Offset: 0x00275BEF
		public string handlerName
		{
			get
			{
				return "VirtualCursorInput";
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06006861 RID: 26721 RVA: 0x002779F6 File Offset: 0x00275BF6
		// (set) Token: 0x06006862 RID: 26722 RVA: 0x002779FE File Offset: 0x00275BFE
		public KInputHandler inputHandler { get; set; }

		// Token: 0x06006863 RID: 26723 RVA: 0x00277A08 File Offset: 0x00275C08
		protected VirtualInputModule()
		{
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06006864 RID: 26724 RVA: 0x00277A7E File Offset: 0x00275C7E
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public VirtualInputModule.InputMode inputMode
		{
			get
			{
				return VirtualInputModule.InputMode.Mouse;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06006865 RID: 26725 RVA: 0x00277A81 File Offset: 0x00275C81
		// (set) Token: 0x06006866 RID: 26726 RVA: 0x00277A89 File Offset: 0x00275C89
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
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

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06006867 RID: 26727 RVA: 0x00277A92 File Offset: 0x00275C92
		// (set) Token: 0x06006868 RID: 26728 RVA: 0x00277A9A File Offset: 0x00275C9A
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

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06006869 RID: 26729 RVA: 0x00277AA3 File Offset: 0x00275CA3
		// (set) Token: 0x0600686A RID: 26730 RVA: 0x00277AAB File Offset: 0x00275CAB
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

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600686B RID: 26731 RVA: 0x00277AB4 File Offset: 0x00275CB4
		// (set) Token: 0x0600686C RID: 26732 RVA: 0x00277ABC File Offset: 0x00275CBC
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

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600686D RID: 26733 RVA: 0x00277AC5 File Offset: 0x00275CC5
		// (set) Token: 0x0600686E RID: 26734 RVA: 0x00277ACD File Offset: 0x00275CCD
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600686F RID: 26735 RVA: 0x00277AD6 File Offset: 0x00275CD6
		// (set) Token: 0x06006870 RID: 26736 RVA: 0x00277ADE File Offset: 0x00275CDE
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06006871 RID: 26737 RVA: 0x00277AE7 File Offset: 0x00275CE7
		// (set) Token: 0x06006872 RID: 26738 RVA: 0x00277AEF File Offset: 0x00275CEF
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06006873 RID: 26739 RVA: 0x00277AF8 File Offset: 0x00275CF8
		// (set) Token: 0x06006874 RID: 26740 RVA: 0x00277B00 File Offset: 0x00275D00
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x00277B09 File Offset: 0x00275D09
		public void SetCursor(Texture2D tex)
		{
			this.UpdateModule();
			if (this.m_VirtualCursor)
			{
				this.m_VirtualCursor.GetComponent<RawImage>().texture = tex;
			}
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x00277B30 File Offset: 0x00275D30
		public override void UpdateModule()
		{
			GameInputManager inputManager = Global.GetInputManager();
			if (inputManager.GetControllerCount() <= 1)
			{
				return;
			}
			if (this.inputHandler == null || !this.inputHandler.UsesController(this, inputManager.GetController(1)))
			{
				KInputHandler.Add(inputManager.GetController(1), this, int.MaxValue);
				if (!inputManager.usedMenus.Contains(this))
				{
					inputManager.usedMenus.Add(this);
				}
				this.debugName = SceneManager.GetActiveScene().name + "-VirtualInputModule";
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (CameraController.Instance != null)
			{
				this.m_canvasCamera.CopyFrom(CameraController.Instance.overlayCamera);
			}
			else if (this.CursorCanvasShouldBeOverlay)
			{
				this.m_canvasCamera.CopyFrom(GameObject.Find("FrontEndCamera").GetComponent<Camera>());
			}
			if (this.m_canvasCamera != null && this.VCcam == null)
			{
				this.VCcam = GameObject.Find("VirtualCursorCamera").GetComponent<Camera>();
				if (this.VCcam != null)
				{
					if (this.m_virtualCursorCanvas == null)
					{
						this.m_virtualCursorCanvas = GameObject.Find("VirtualCursorCanvas").GetComponent<Canvas>();
						this.m_virtualCursorScaler = this.m_virtualCursorCanvas.GetComponent<CanvasScaler>();
					}
					if (this.CursorCanvasShouldBeOverlay)
					{
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
						this.VCcam.orthographic = false;
					}
					else
					{
						this.VCcam.orthographic = this.m_canvasCamera.orthographic;
						this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
						this.VCcam.transform.position = this.m_canvasCamera.transform.position;
						this.VCcam.enabled = true;
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceCamera;
						this.m_virtualCursorCanvas.worldCamera = this.VCcam;
					}
				}
			}
			if (this.m_canvasCamera != null && this.VCcam != null)
			{
				this.VCcam.orthographic = this.m_canvasCamera.orthographic;
				this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
				this.VCcam.transform.position = this.m_canvasCamera.transform.position;
				this.VCcam.aspect = this.m_canvasCamera.aspect;
				this.VCcam.enabled = true;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			if (this.m_virtualCursorScaler != null && this.m_virtualCursorScaler.referenceResolution != vector)
			{
				this.m_virtualCursorScaler.referenceResolution = vector;
			}
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_VirtualCursor.localScale = Vector2.one;
			Vector2 steamCursorMovement = KInputManager.steamInputInterpreter.GetSteamCursorMovement();
			float num = 1f / (4500f / vector.x);
			steamCursorMovement.x *= num;
			steamCursorMovement.y *= num;
			this.m_VirtualCursor.anchoredPosition += steamCursorMovement * this.m_VirtualCursorSpeed;
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, vector.x), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, vector.y));
			KInputManager.virtualCursorPos = new Vector3F(this.m_VirtualCursor.anchoredPosition.x, this.m_VirtualCursor.anchoredPosition.y, 0f);
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x00277F2E File Offset: 0x0027612E
		public override bool IsModuleSupported()
		{
			return this.m_ForceModuleActive || Input.mousePresent;
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x00277F40 File Offset: 0x00276140
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return true;
			}
			bool forceModuleActive = this.m_ForceModuleActive;
			Input.GetButtonDown(this.m_SubmitButton);
			return forceModuleActive | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f) | (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f | Input.GetMouseButtonDown(0);
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x00277FD8 File Offset: 0x002761D8
		public override void ActivateModule()
		{
			base.ActivateModule();
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (Input.mousePosition.x > 0f && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.y > 0f && Input.mousePosition.y < (float)Screen.height)
			{
				this.m_VirtualCursor.anchoredPosition = Input.mousePosition;
			}
			else
			{
				this.m_VirtualCursor.anchoredPosition = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			}
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, (float)Screen.width), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, (float)Screen.height));
			this.m_VirtualCursor.localScale = Vector2.zero;
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
			this.m_LastMousePosition = this.m_VirtualCursor.anchoredPosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = GameObject.Find("FrontEndCamera").GetComponent<Camera>();
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x00278194 File Offset: 0x00276394
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
			this.conButtonStates.affirmativeDown = false;
			this.conButtonStates.affirmativeHoldTime = 0f;
			this.conButtonStates.negativeDown = false;
			this.conButtonStates.negativeHoldTime = 0f;
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x002781E8 File Offset: 0x002763E8
		public override void Process()
		{
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
			this.ProcessMouseEvent();
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x00278228 File Offset: 0x00276428
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x002782A0 File Offset: 0x002764A0
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x00278358 File Offset: 0x00276558
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x0027846B File Offset: 0x0027666B
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x00278474 File Offset: 0x00276674
		protected void ProcessMouseEvent(int id)
		{
			if (this.mouseMovementOnly)
			{
				return;
			}
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.ProcessControllerPress(eventData, true);
			this.ProcessControllerPress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData, false);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x00278564 File Offset: 0x00276764
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x002785AC File Offset: 0x002767AC
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
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
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
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
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

		// Token: 0x06006883 RID: 26755 RVA: 0x002787B8 File Offset: 0x002769B8
		public void OnKeyDown(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					if (this.conButtonStates.affirmativeDown)
					{
						this.conButtonStates.affirmativeHoldTime = this.conButtonStates.affirmativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.affirmativeDown)
					{
						this.leftFirstClick = true;
						this.leftReleased = false;
					}
					this.conButtonStates.affirmativeDown = true;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					if (this.conButtonStates.negativeDown)
					{
						this.conButtonStates.negativeHoldTime = this.conButtonStates.negativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.negativeDown)
					{
						this.rightFirstClick = true;
						this.rightReleased = false;
					}
					this.conButtonStates.negativeDown = true;
				}
			}
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x0027887C File Offset: 0x00276A7C
		public void OnKeyUp(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					this.conButtonStates.affirmativeHoldTime = 0f;
					this.leftReleased = true;
					this.leftFirstClick = false;
					this.conButtonStates.affirmativeDown = false;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					this.conButtonStates.negativeHoldTime = 0f;
					this.rightReleased = true;
					this.rightFirstClick = false;
					this.conButtonStates.negativeDown = false;
				}
			}
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x00278900 File Offset: 0x00276B00
		protected void ProcessControllerPress(PointerInputModule.MouseButtonEventData data, bool leftClick)
		{
			if (this.leftClickData == null)
			{
				this.leftClickData = data.buttonData;
			}
			if (this.rightClickData == null)
			{
				this.rightClickData = data.buttonData;
			}
			if (leftClick)
			{
				PointerEventData buttonData = data.buttonData;
				GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				if (this.leftFirstClick)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					buttonData.eligibleForClick = true;
					buttonData.delta = Vector2.zero;
					buttonData.dragging = false;
					buttonData.useDragThreshold = true;
					buttonData.pressPosition = buttonData.position;
					buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
					buttonData.position = new Vector2(KInputManager.virtualCursorPos.x, KInputManager.virtualCursorPos.y);
					base.DeselectIfSelectionChanged(gameObject, buttonData);
					GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
					float unscaledTime = Time.unscaledTime;
					if (gameObject2 == buttonData.lastPress)
					{
						if (unscaledTime - buttonData.clickTime < 0.3f)
						{
							PointerEventData pointerEventData = buttonData;
							int clickCount = pointerEventData.clickCount + 1;
							pointerEventData.clickCount = clickCount;
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
					this.leftFirstClick = false;
					return;
				}
				if (this.leftReleased)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
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
					this.leftReleased = false;
					return;
				}
			}
			else
			{
				PointerEventData buttonData2 = data.buttonData;
				GameObject gameObject3 = buttonData2.pointerCurrentRaycast.gameObject;
				buttonData2.position = this.m_VirtualCursor.anchoredPosition;
				if (this.rightFirstClick)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					buttonData2.eligibleForClick = true;
					buttonData2.delta = Vector2.zero;
					buttonData2.dragging = false;
					buttonData2.useDragThreshold = true;
					buttonData2.pressPosition = buttonData2.position;
					buttonData2.pointerPressRaycast = buttonData2.pointerCurrentRaycast;
					buttonData2.position = this.m_VirtualCursor.anchoredPosition;
					base.DeselectIfSelectionChanged(gameObject3, buttonData2);
					GameObject gameObject4 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject3, buttonData2, ExecuteEvents.pointerDownHandler);
					if (gameObject4 == null)
					{
						gameObject4 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					}
					float unscaledTime2 = Time.unscaledTime;
					if (gameObject4 == buttonData2.lastPress)
					{
						if (unscaledTime2 - buttonData2.clickTime < 0.3f)
						{
							PointerEventData pointerEventData2 = buttonData2;
							int clickCount = pointerEventData2.clickCount + 1;
							pointerEventData2.clickCount = clickCount;
						}
						else
						{
							buttonData2.clickCount = 1;
						}
						buttonData2.clickTime = unscaledTime2;
					}
					else
					{
						buttonData2.clickCount = 1;
					}
					buttonData2.pointerPress = gameObject4;
					buttonData2.rawPointerPress = gameObject3;
					buttonData2.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject3);
					if (buttonData2.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.initializePotentialDrag);
					}
					this.rightFirstClick = false;
					return;
				}
				if (this.rightReleased)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					if (buttonData2.pointerPress == eventHandler2 && buttonData2.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject3, buttonData2, ExecuteEvents.dropHandler);
					}
					buttonData2.eligibleForClick = false;
					buttonData2.pointerPress = null;
					buttonData2.rawPointerPress = null;
					if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.endDragHandler);
					}
					buttonData2.dragging = false;
					buttonData2.pointerDrag = null;
					if (gameObject3 != buttonData2.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData2, null);
						base.HandlePointerExitAndEnter(buttonData2, gameObject3);
					}
					this.rightReleased = false;
					return;
				}
			}
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x00278DDC File Offset: 0x00276FDC
		protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			Vector2 position = RectTransformUtility.WorldToScreenPoint(this.m_canvasCamera, this.m_VirtualCursor.position);
			if (pointerData)
			{
				pointerEventData.position = position;
			}
			Vector2 anchoredPosition = this.m_VirtualCursor.anchoredPosition;
			pointerEventData.delta = anchoredPosition - pointerEventData.position;
			pointerEventData.position = anchoredPosition;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			base.GetPointerData(-2, out pointerEventData2, true);
			base.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			base.GetPointerData(-3, out pointerEventData3, true);
			base.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x0400481B RID: 18459
		private float m_PrevActionTime;

		// Token: 0x0400481C RID: 18460
		private Vector2 m_LastMoveVector;

		// Token: 0x0400481D RID: 18461
		private int m_ConsecutiveMoveCount;

		// Token: 0x0400481E RID: 18462
		private string debugName;

		// Token: 0x0400481F RID: 18463
		private Vector2 m_LastMousePosition;

		// Token: 0x04004820 RID: 18464
		private Vector2 m_MousePosition;

		// Token: 0x04004821 RID: 18465
		public bool mouseMovementOnly;

		// Token: 0x04004822 RID: 18466
		[SerializeField]
		private RectTransform m_VirtualCursor;

		// Token: 0x04004823 RID: 18467
		[SerializeField]
		private float m_VirtualCursorSpeed = 1f;

		// Token: 0x04004824 RID: 18468
		[SerializeField]
		private Vector2 m_VirtualCursorOffset = Vector2.zero;

		// Token: 0x04004825 RID: 18469
		[SerializeField]
		private Camera m_canvasCamera;

		// Token: 0x04004826 RID: 18470
		private Camera VCcam;

		// Token: 0x04004827 RID: 18471
		public bool CursorCanvasShouldBeOverlay;

		// Token: 0x04004828 RID: 18472
		private Canvas m_virtualCursorCanvas;

		// Token: 0x04004829 RID: 18473
		private CanvasScaler m_virtualCursorScaler;

		// Token: 0x0400482A RID: 18474
		private PointerEventData leftClickData;

		// Token: 0x0400482B RID: 18475
		private PointerEventData rightClickData;

		// Token: 0x0400482C RID: 18476
		private VirtualInputModule.ControllerButtonStates conButtonStates;

		// Token: 0x0400482D RID: 18477
		private GameObject m_CurrentFocusedGameObject;

		// Token: 0x0400482E RID: 18478
		private bool leftReleased;

		// Token: 0x0400482F RID: 18479
		private bool rightReleased;

		// Token: 0x04004830 RID: 18480
		private bool leftFirstClick;

		// Token: 0x04004831 RID: 18481
		private bool rightFirstClick;

		// Token: 0x04004832 RID: 18482
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04004833 RID: 18483
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04004834 RID: 18484
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04004835 RID: 18485
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04004836 RID: 18486
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04004837 RID: 18487
		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		// Token: 0x04004838 RID: 18488
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		// Token: 0x04004839 RID: 18489
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x02001C0A RID: 7178
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04007EB2 RID: 32434
			Mouse,
			// Token: 0x04007EB3 RID: 32435
			Buttons
		}

		// Token: 0x02001C0B RID: 7179
		private struct ControllerButtonStates
		{
			// Token: 0x04007EB4 RID: 32436
			public bool affirmativeDown;

			// Token: 0x04007EB5 RID: 32437
			public float affirmativeHoldTime;

			// Token: 0x04007EB6 RID: 32438
			public bool negativeDown;

			// Token: 0x04007EB7 RID: 32439
			public float negativeHoldTime;
		}
	}
}
