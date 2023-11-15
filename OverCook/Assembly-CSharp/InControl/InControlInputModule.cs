using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InControl
{
	// Token: 0x020002BE RID: 702
	[AddComponentMenu("Event/InControl Input Module")]
	public class InControlInputModule : StandaloneInputModule
	{
		// Token: 0x06000DCD RID: 3533 RVA: 0x00044368 File Offset: 0x00042768
		protected InControlInputModule()
		{
			this.direction = new TwoAxisInputControl();
			this.direction.StateThreshold = this.analogMoveThreshold;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000443D6 File Offset: 0x000427D6
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x000443DE File Offset: 0x000427DE
		public PlayerAction SubmitAction { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000443E7 File Offset: 0x000427E7
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x000443EF File Offset: 0x000427EF
		public PlayerAction CancelAction { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x000443F8 File Offset: 0x000427F8
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00044400 File Offset: 0x00042800
		public PlayerTwoAxisAction MoveAction { get; set; }

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00044409 File Offset: 0x00042809
		public override void UpdateModule()
		{
			this.lastMousePosition = this.thisMousePosition;
			this.thisMousePosition = Input.mousePosition;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00044422 File Offset: 0x00042822
		public override bool IsModuleSupported()
		{
			return this.allowMobileDevice || Input.mousePresent;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00044438 File Offset: 0x00042838
		public override bool ShouldActivateModule()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.UpdateInputState();
			bool flag = false;
			flag |= this.SubmitWasPressed;
			flag |= this.CancelWasPressed;
			flag |= this.VectorWasPressed;
			if (this.allowMouseInput)
			{
				flag |= this.MouseHasMoved;
				flag |= this.MouseButtonIsPressed;
			}
			return flag;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x000444A4 File Offset: 0x000428A4
		public override void ActivateModule()
		{
			base.ActivateModule();
			this.thisMousePosition = Input.mousePosition;
			this.lastMousePosition = Input.mousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00044504 File Offset: 0x00042904
		public override void Process()
		{
			bool flag = base.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag = this.SendVectorEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendButtonEventToSelectedObject();
				}
			}
			if (this.allowMouseInput)
			{
				base.ProcessMouseEvent();
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00044554 File Offset: 0x00042954
		private bool SendButtonEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (this.SubmitWasPressed)
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			else if (this.SubmitWasReleased)
			{
			}
			if (this.CancelWasPressed)
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x000445DC File Offset: 0x000429DC
		private bool SendVectorEventToSelectedObject()
		{
			if (!this.VectorWasPressed)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(this.thisVectorState.x, this.thisVectorState.y, 0.5f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				if (base.eventSystem.currentSelectedGameObject == null)
				{
					base.eventSystem.SetSelectedGameObject(base.eventSystem.firstSelectedGameObject, this.GetBaseEventData());
				}
				else
				{
					ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				}
				this.SetVectorRepeatTimer();
			}
			return axisEventData.used;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00044680 File Offset: 0x00042A80
		protected override void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject pointerEnter = pointerEvent.pointerEnter;
			base.ProcessMove(pointerEvent);
			if (this.focusOnMouseHover && pointerEnter != pointerEvent.pointerEnter)
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(pointerEvent.pointerEnter);
				base.eventSystem.SetSelectedGameObject(eventHandler, pointerEvent);
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x000446D0 File Offset: 0x00042AD0
		private void Update()
		{
			this.direction.Filter(this.Device.Direction, Time.deltaTime);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x000446F0 File Offset: 0x00042AF0
		private void UpdateInputState()
		{
			this.lastVectorState = this.thisVectorState;
			this.thisVectorState = Vector2.zero;
			TwoAxisInputControl twoAxisInputControl = this.MoveAction ?? this.direction;
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.X, this.analogMoveThreshold))
			{
				this.thisVectorState.x = Mathf.Sign(twoAxisInputControl.X);
			}
			if (Utility.AbsoluteIsOverThreshold(twoAxisInputControl.Y, this.analogMoveThreshold))
			{
				this.thisVectorState.y = Mathf.Sign(twoAxisInputControl.Y);
			}
			if (this.VectorIsReleased)
			{
				this.nextMoveRepeatTime = 0f;
			}
			if (this.VectorIsPressed)
			{
				if (this.lastVectorState == Vector2.zero)
				{
					if (Time.realtimeSinceStartup > this.lastVectorPressedTime + 0.1f)
					{
						this.nextMoveRepeatTime = Time.realtimeSinceStartup + this.moveRepeatFirstDuration;
					}
					else
					{
						this.nextMoveRepeatTime = Time.realtimeSinceStartup + this.moveRepeatDelayDuration;
					}
				}
				this.lastVectorPressedTime = Time.realtimeSinceStartup;
			}
			this.lastSubmitState = this.thisSubmitState;
			this.thisSubmitState = ((this.SubmitAction != null) ? this.SubmitAction.IsPressed : this.SubmitButton.IsPressed);
			this.lastCancelState = this.thisCancelState;
			this.thisCancelState = ((this.CancelAction != null) ? this.CancelAction.IsPressed : this.CancelButton.IsPressed);
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00044879 File Offset: 0x00042C79
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00044870 File Offset: 0x00042C70
		private InputDevice Device
		{
			get
			{
				return this.inputDevice ?? InputManager.ActiveDevice;
			}
			set
			{
				this.inputDevice = value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0004488D File Offset: 0x00042C8D
		private InputControl SubmitButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.submitButton);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x000448A0 File Offset: 0x00042CA0
		private InputControl CancelButton
		{
			get
			{
				return this.Device.GetControl((InputControlType)this.cancelButton);
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x000448B3 File Offset: 0x00042CB3
		private void SetVectorRepeatTimer()
		{
			this.nextMoveRepeatTime = Mathf.Max(this.nextMoveRepeatTime, Time.realtimeSinceStartup + this.moveRepeatDelayDuration);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x000448D2 File Offset: 0x00042CD2
		private bool VectorIsPressed
		{
			get
			{
				return this.thisVectorState != Vector2.zero;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x000448E4 File Offset: 0x00042CE4
		private bool VectorIsReleased
		{
			get
			{
				return this.thisVectorState == Vector2.zero;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x000448F6 File Offset: 0x00042CF6
		private bool VectorHasChanged
		{
			get
			{
				return this.thisVectorState != this.lastVectorState;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x00044909 File Offset: 0x00042D09
		private bool VectorWasPressed
		{
			get
			{
				return (this.VectorIsPressed && Time.realtimeSinceStartup > this.nextMoveRepeatTime) || (this.VectorIsPressed && this.lastVectorState == Vector2.zero);
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00044946 File Offset: 0x00042D46
		private bool SubmitWasPressed
		{
			get
			{
				return this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00044967 File Offset: 0x00042D67
		private bool SubmitWasReleased
		{
			get
			{
				return !this.thisSubmitState && this.thisSubmitState != this.lastSubmitState;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00044988 File Offset: 0x00042D88
		private bool CancelWasPressed
		{
			get
			{
				return this.thisCancelState && this.thisCancelState != this.lastCancelState;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x000449AC File Offset: 0x00042DAC
		private bool MouseHasMoved
		{
			get
			{
				return (this.thisMousePosition - this.lastMousePosition).sqrMagnitude > 0f;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x000449D9 File Offset: 0x00042DD9
		private bool MouseButtonIsPressed
		{
			get
			{
				return Input.GetMouseButtonDown(0);
			}
		}

		// Token: 0x04000ACA RID: 2762
		public new InControlInputModule.Button submitButton = InControlInputModule.Button.Action1;

		// Token: 0x04000ACB RID: 2763
		public new InControlInputModule.Button cancelButton = InControlInputModule.Button.Action2;

		// Token: 0x04000ACC RID: 2764
		[Range(0.1f, 0.9f)]
		public float analogMoveThreshold = 0.5f;

		// Token: 0x04000ACD RID: 2765
		public float moveRepeatFirstDuration = 0.8f;

		// Token: 0x04000ACE RID: 2766
		public float moveRepeatDelayDuration = 0.1f;

		// Token: 0x04000ACF RID: 2767
		public bool allowMobileDevice = true;

		// Token: 0x04000AD0 RID: 2768
		public bool allowMouseInput = true;

		// Token: 0x04000AD1 RID: 2769
		public bool focusOnMouseHover;

		// Token: 0x04000AD2 RID: 2770
		private InputDevice inputDevice;

		// Token: 0x04000AD3 RID: 2771
		private Vector3 thisMousePosition;

		// Token: 0x04000AD4 RID: 2772
		private Vector3 lastMousePosition;

		// Token: 0x04000AD5 RID: 2773
		private Vector2 thisVectorState;

		// Token: 0x04000AD6 RID: 2774
		private Vector2 lastVectorState;

		// Token: 0x04000AD7 RID: 2775
		private bool thisSubmitState;

		// Token: 0x04000AD8 RID: 2776
		private bool lastSubmitState;

		// Token: 0x04000AD9 RID: 2777
		private bool thisCancelState;

		// Token: 0x04000ADA RID: 2778
		private bool lastCancelState;

		// Token: 0x04000ADB RID: 2779
		private float nextMoveRepeatTime;

		// Token: 0x04000ADC RID: 2780
		private float lastVectorPressedTime;

		// Token: 0x04000ADD RID: 2781
		private TwoAxisInputControl direction;

		// Token: 0x020002BF RID: 703
		public enum Button
		{
			// Token: 0x04000AE2 RID: 2786
			Action1 = 15,
			// Token: 0x04000AE3 RID: 2787
			Action2,
			// Token: 0x04000AE4 RID: 2788
			Action3,
			// Token: 0x04000AE5 RID: 2789
			Action4
		}
	}
}
