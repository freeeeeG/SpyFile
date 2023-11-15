using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002CB RID: 715
	[ExecuteInEditMode]
	public class TouchManager : SingletonMonoBehavior<TouchManager>
	{
		// Token: 0x06000E9C RID: 3740 RVA: 0x00046DC8 File Offset: 0x000451C8
		protected TouchManager()
		{
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000E9D RID: 3741 RVA: 0x00046DE8 File Offset: 0x000451E8
		// (remove) Token: 0x06000E9E RID: 3742 RVA: 0x00046E1C File Offset: 0x0004521C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x06000E9F RID: 3743 RVA: 0x00046E50 File Offset: 0x00045250
		private void OnEnable()
		{
			if (!base.SetupSingleton())
			{
				return;
			}
			this.touchControls = base.GetComponentsInChildren<TouchControl>(true);
			if (Application.isPlaying)
			{
				InputManager.OnSetup += this.Setup;
				InputManager.OnUpdateDevices += this.UpdateDevice;
				InputManager.OnCommitDevices += this.CommitDevice;
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00046EB4 File Offset: 0x000452B4
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				InputManager.OnSetup -= this.Setup;
				InputManager.OnUpdateDevices -= this.UpdateDevice;
				InputManager.OnCommitDevices -= this.CommitDevice;
			}
			this.Reset();
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00046F04 File Offset: 0x00045304
		private void Setup()
		{
			this.UpdateScreenSize(new Vector2((float)Screen.width, (float)Screen.height));
			this.CreateDevice();
			this.CreateTouches();
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00046F43 File Offset: 0x00045343
		private void Reset()
		{
			this.device = null;
			this.mouseTouch = null;
			this.cachedTouches = null;
			this.activeTouches = null;
			this.readOnlyActiveTouches = null;
			this.touchControls = null;
			TouchManager.OnSetup = null;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00046F75 File Offset: 0x00045375
		private void Start()
		{
			base.StartCoroutine(this.Ready());
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00046F84 File Offset: 0x00045384
		private IEnumerator Ready()
		{
			yield return new WaitForEndOfFrame();
			this.isReady = true;
			this.UpdateScreenSize(new Vector2((float)Screen.width, (float)Screen.height));
			yield return null;
			yield break;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00046FA0 File Offset: 0x000453A0
		private void Update()
		{
			if (!this.isReady)
			{
				return;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			if (this.screenSize != vector)
			{
				this.UpdateScreenSize(vector);
			}
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00047000 File Offset: 0x00045400
		private void CreateDevice()
		{
			this.device = new InputDevice("TouchDevice");
			this.device.RawSticks = true;
			this.device.AddControl(InputControlType.LeftStickLeft, "LeftStickLeft");
			this.device.AddControl(InputControlType.LeftStickRight, "LeftStickRight");
			this.device.AddControl(InputControlType.LeftStickUp, "LeftStickUp");
			this.device.AddControl(InputControlType.LeftStickDown, "LeftStickDown");
			this.device.AddControl(InputControlType.RightStickLeft, "RightStickLeft");
			this.device.AddControl(InputControlType.RightStickRight, "RightStickRight");
			this.device.AddControl(InputControlType.RightStickUp, "RightStickUp");
			this.device.AddControl(InputControlType.RightStickDown, "RightStickDown");
			this.device.AddControl(InputControlType.LeftTrigger, "LeftTrigger");
			this.device.AddControl(InputControlType.RightTrigger, "RightTrigger");
			this.device.AddControl(InputControlType.DPadUp, "DPadUp");
			this.device.AddControl(InputControlType.DPadDown, "DPadDown");
			this.device.AddControl(InputControlType.DPadLeft, "DPadLeft");
			this.device.AddControl(InputControlType.DPadRight, "DPadRight");
			this.device.AddControl(InputControlType.Action1, "Action1");
			this.device.AddControl(InputControlType.Action2, "Action2");
			this.device.AddControl(InputControlType.Action3, "Action3");
			this.device.AddControl(InputControlType.Action4, "Action4");
			this.device.AddControl(InputControlType.LeftBumper, "LeftBumper");
			this.device.AddControl(InputControlType.RightBumper, "RightBumper");
			this.device.AddControl(InputControlType.Menu, "Menu");
			for (InputControlType inputControlType = InputControlType.Button0; inputControlType <= InputControlType.Button19; inputControlType++)
			{
				this.device.AddControl(inputControlType, inputControlType.ToString());
			}
			InputManager.AttachDevice(this.device);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x000471EA File Offset: 0x000455EA
		private void UpdateDevice(ulong updateTick, float deltaTime)
		{
			this.UpdateTouches(updateTick, deltaTime);
			this.SubmitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x000471FC File Offset: 0x000455FC
		private void CommitDevice(ulong updateTick, float deltaTime)
		{
			this.CommitControlStates(updateTick, deltaTime);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00047208 File Offset: 0x00045608
		private void SubmitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.SubmitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0004725C File Offset: 0x0004565C
		private void CommitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.CommitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x000472B0 File Offset: 0x000456B0
		private void UpdateScreenSize(Vector2 currentScreenSize)
		{
			this.screenSize = currentScreenSize;
			this.halfScreenSize = this.screenSize / 2f;
			this.viewSize = this.ConvertViewToWorldPoint(Vector2.one) * 0.02f;
			this.percentToWorld = Mathf.Min(this.viewSize.x, this.viewSize.y);
			this.halfPercentToWorld = this.percentToWorld / 2f;
			if (this.touchCamera != null)
			{
				this.halfPixelToWorld = this.touchCamera.orthographicSize / this.screenSize.y;
				this.pixelToWorld = this.halfPixelToWorld * 2f;
			}
			if (this.touchControls != null)
			{
				int num = this.touchControls.Length;
				for (int i = 0; i < num; i++)
				{
					this.touchControls[i].ConfigureControl();
				}
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0004739C File Offset: 0x0004579C
		private void CreateTouches()
		{
			this.cachedTouches = new Touch[16];
			for (int i = 0; i < 16; i++)
			{
				this.cachedTouches[i] = new Touch(i);
			}
			this.mouseTouch = this.cachedTouches[15];
			this.activeTouches = new List<Touch>(16);
			this.readOnlyActiveTouches = new ReadOnlyCollection<Touch>(this.activeTouches);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00047404 File Offset: 0x00045804
		private void UpdateTouches(ulong updateTick, float deltaTime)
		{
			this.activeTouches.Clear();
			if (this.mouseTouch.SetWithMouseData(updateTick, deltaTime))
			{
				this.activeTouches.Add(this.mouseTouch);
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Touch touch2 = this.cachedTouches[touch.fingerId];
				touch2.SetWithTouchData(touch, updateTick, deltaTime);
				this.activeTouches.Add(touch2);
			}
			for (int j = 0; j < 16; j++)
			{
				Touch touch3 = this.cachedTouches[j];
				if (touch3.phase != TouchPhase.Ended && touch3.updateTick != updateTick)
				{
					touch3.phase = TouchPhase.Ended;
					this.activeTouches.Add(touch3);
				}
			}
			this.InvokeTouchEvents();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000474D4 File Offset: 0x000458D4
		private void SendTouchBegan(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchBegan(touch);
				}
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00047528 File Offset: 0x00045928
		private void SendTouchMoved(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchMoved(touch);
				}
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0004757C File Offset: 0x0004597C
		private void SendTouchEnded(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchEnded(touch);
				}
			}
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000475D0 File Offset: 0x000459D0
		private void InvokeTouchEvents()
		{
			int count = this.activeTouches.Count;
			if (this.enableControlsOnTouch && count > 0 && !this.controlsEnabled)
			{
				TouchManager.Device.RequestActivation();
				this.controlsEnabled = true;
			}
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.activeTouches[i];
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.SendTouchBegan(touch);
					break;
				case TouchPhase.Moved:
					this.SendTouchMoved(touch);
					break;
				case TouchPhase.Ended:
					this.SendTouchEnded(touch);
					break;
				case TouchPhase.Canceled:
					this.SendTouchEnded(touch);
					break;
				}
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0004768C File Offset: 0x00045A8C
		private bool TouchCameraIsValid()
		{
			return !(this.touchCamera == null) && !Utility.IsZero(this.touchCamera.orthographicSize) && (!Utility.IsZero(this.touchCamera.rect.width) || !Utility.IsZero(this.touchCamera.rect.height)) && (!Utility.IsZero(this.touchCamera.pixelRect.width) || !Utility.IsZero(this.touchCamera.pixelRect.height));
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0004773C File Offset: 0x00045B3C
		private Vector3 ConvertScreenToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00047794 File Offset: 0x00045B94
		private Vector3 ConvertViewToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ViewportToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x000477EC File Offset: 0x00045BEC
		private Vector3 ConvertScreenToViewPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToViewportPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x00047841 File Offset: 0x00045C41
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x0004784C File Offset: 0x00045C4C
		public bool controlsEnabled
		{
			get
			{
				return this._controlsEnabled;
			}
			set
			{
				if (this._controlsEnabled != value)
				{
					int num = this.touchControls.Length;
					for (int i = 0; i < num; i++)
					{
						this.touchControls[i].enabled = value;
					}
					this._controlsEnabled = value;
				}
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00047895 File Offset: 0x00045C95
		public static ReadOnlyCollection<Touch> Touches
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.readOnlyActiveTouches;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x000478A1 File Offset: 0x00045CA1
		public static int TouchCount
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.activeTouches.Count;
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x000478B2 File Offset: 0x00045CB2
		public static Touch GetTouch(int touchIndex)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.activeTouches[touchIndex];
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000478C4 File Offset: 0x00045CC4
		public static Touch GetTouchByFingerId(int fingerId)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.cachedTouches[fingerId];
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000478D2 File Offset: 0x00045CD2
		public static Vector3 ScreenToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertScreenToWorldPoint(point);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000478DF File Offset: 0x00045CDF
		public static Vector3 ViewToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertViewToWorldPoint(point);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000478EC File Offset: 0x00045CEC
		public static Vector3 ScreenToViewPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager>.Instance.ConvertScreenToViewPoint(point);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000478F9 File Offset: 0x00045CF9
		public static float ConvertToWorld(float value, TouchUnitType unitType)
		{
			return value * ((unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorld : TouchManager.PixelToWorld);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00047914 File Offset: 0x00045D14
		public static Rect PercentToWorldRect(Rect rect)
		{
			return new Rect((rect.xMin - 50f) * TouchManager.ViewSize.x, (rect.yMin - 50f) * TouchManager.ViewSize.y, rect.width * TouchManager.ViewSize.x, rect.height * TouchManager.ViewSize.y);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00047988 File Offset: 0x00045D88
		public static Rect PixelToWorldRect(Rect rect)
		{
			return new Rect(Mathf.Round(rect.xMin - TouchManager.HalfScreenSize.x) * TouchManager.PixelToWorld, Mathf.Round(rect.yMin - TouchManager.HalfScreenSize.y) * TouchManager.PixelToWorld, Mathf.Round(rect.width) * TouchManager.PixelToWorld, Mathf.Round(rect.height) * TouchManager.PixelToWorld);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000479FE File Offset: 0x00045DFE
		public static Rect ConvertToWorld(Rect rect, TouchUnitType unitType)
		{
			return (unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorldRect(rect) : TouchManager.PixelToWorldRect(rect);
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00047A18 File Offset: 0x00045E18
		public static Camera Camera
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.touchCamera;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00047A24 File Offset: 0x00045E24
		public static InputDevice Device
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.device;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00047A30 File Offset: 0x00045E30
		public static Vector3 ViewSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.viewSize;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x00047A3C File Offset: 0x00045E3C
		public static float PercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.percentToWorld;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00047A48 File Offset: 0x00045E48
		public static float HalfPercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfPercentToWorld;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00047A54 File Offset: 0x00045E54
		public static float PixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.pixelToWorld;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00047A60 File Offset: 0x00045E60
		public static float HalfPixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfPixelToWorld;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00047A6C File Offset: 0x00045E6C
		public static Vector2 ScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.screenSize;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00047A78 File Offset: 0x00045E78
		public static Vector2 HalfScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.halfScreenSize;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00047A84 File Offset: 0x00045E84
		public static TouchManager.GizmoShowOption ControlsShowGizmos
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.controlsShowGizmos;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00047A90 File Offset: 0x00045E90
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x00047A9C File Offset: 0x00045E9C
		public static bool ControlsEnabled
		{
			get
			{
				return SingletonMonoBehavior<TouchManager>.Instance.controlsEnabled;
			}
			set
			{
				SingletonMonoBehavior<TouchManager>.Instance.controlsEnabled = value;
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00047AA9 File Offset: 0x00045EA9
		public static implicit operator bool(TouchManager instance)
		{
			return instance != null;
		}

		// Token: 0x04000B86 RID: 2950
		private const int MaxTouches = 16;

		// Token: 0x04000B87 RID: 2951
		[Space(10f)]
		public Camera touchCamera;

		// Token: 0x04000B88 RID: 2952
		public TouchManager.GizmoShowOption controlsShowGizmos = TouchManager.GizmoShowOption.Always;

		// Token: 0x04000B89 RID: 2953
		[HideInInspector]
		public bool enableControlsOnTouch;

		// Token: 0x04000B8A RID: 2954
		[SerializeField]
		[HideInInspector]
		private bool _controlsEnabled = true;

		// Token: 0x04000B8B RID: 2955
		[HideInInspector]
		public int controlsLayer = 5;

		// Token: 0x04000B8D RID: 2957
		private InputDevice device;

		// Token: 0x04000B8E RID: 2958
		private Vector3 viewSize;

		// Token: 0x04000B8F RID: 2959
		private Vector2 screenSize;

		// Token: 0x04000B90 RID: 2960
		private Vector2 halfScreenSize;

		// Token: 0x04000B91 RID: 2961
		private float percentToWorld;

		// Token: 0x04000B92 RID: 2962
		private float halfPercentToWorld;

		// Token: 0x04000B93 RID: 2963
		private float pixelToWorld;

		// Token: 0x04000B94 RID: 2964
		private float halfPixelToWorld;

		// Token: 0x04000B95 RID: 2965
		private TouchControl[] touchControls;

		// Token: 0x04000B96 RID: 2966
		private Touch[] cachedTouches;

		// Token: 0x04000B97 RID: 2967
		private List<Touch> activeTouches;

		// Token: 0x04000B98 RID: 2968
		private ReadOnlyCollection<Touch> readOnlyActiveTouches;

		// Token: 0x04000B99 RID: 2969
		private bool isReady;

		// Token: 0x04000B9A RID: 2970
		private Touch mouseTouch;

		// Token: 0x020002CC RID: 716
		public enum GizmoShowOption
		{
			// Token: 0x04000B9C RID: 2972
			Never,
			// Token: 0x04000B9D RID: 2973
			WhenSelected,
			// Token: 0x04000B9E RID: 2974
			UnlessPlaying,
			// Token: 0x04000B9F RID: 2975
			Always
		}
	}
}
