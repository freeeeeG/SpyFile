using System;
using flanne.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace flanne
{
	// Token: 0x02000077 RID: 119
	public class ShootingCursor : MonoBehaviour
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00018AEF File Offset: 0x00016CEF
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00018AF7 File Offset: 0x00016CF7
		public Vector2 cursorPosition { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00018B00 File Offset: 0x00016D00
		public bool usingGamepad
		{
			get
			{
				return this._usingGamepad;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00018B08 File Offset: 0x00016D08
		private void OnPoint(InputAction.CallbackContext context)
		{
			if (this.autoAim && !PauseController.isPaused)
			{
				return;
			}
			this.cursorPosition = context.ReadValue<Vector2>();
			this.AnchorCursor(this.cursorPosition);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00018B33 File Offset: 0x00016D33
		private void OnAim(InputAction.CallbackContext context)
		{
			this._lastGamepadVector = context.ReadValue<Vector2>();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00018B42 File Offset: 0x00016D42
		private void OnAimCancel(InputAction.CallbackContext context)
		{
			this._lastGamepadVector = Vector2.zero;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00018B4F File Offset: 0x00016D4F
		private void OnAutoAimToggle(InputAction.CallbackContext context)
		{
			if (context.ReadValue<float>() == 1f)
			{
				this.autoAim = !this.autoAim;
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00018B70 File Offset: 0x00016D70
		private void OnControlsChanged(PlayerInput input)
		{
			if (input.currentControlScheme == "Keyboard&Mouse" && this.previousControlScheme != input.currentControlScheme)
			{
				this._usingGamepad = false;
				return;
			}
			if (input.currentControlScheme == "Gamepad" && this.previousControlScheme != input.currentControlScheme)
			{
				this._usingGamepad = true;
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00018BD6 File Offset: 0x00016DD6
		private void Awake()
		{
			if (ShootingCursor.Instance == null)
			{
				ShootingCursor.Instance = this;
				return;
			}
			if (ShootingCursor.Instance != this)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018C04 File Offset: 0x00016E04
		private void OnEnable()
		{
			Cursor.visible = false;
			this._pointAction = this.inputs.FindActionMap("PlayerMap", false).FindAction("Point", false);
			this._aimAction = this.inputs.FindActionMap("PlayerMap", false).FindAction("Aim", false);
			this._autoToggleAction = this.inputs.FindActionMap("PlayerMap", false).FindAction("AutoAim", false);
			this._pointAction.performed += this.OnPoint;
			this._aimAction.performed += this.OnAim;
			this._aimAction.canceled += this.OnAimCancel;
			this._autoToggleAction.performed += this.OnAutoAimToggle;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00018CDC File Offset: 0x00016EDC
		private void OnDisable()
		{
			this._pointAction.performed -= this.OnPoint;
			this._aimAction.performed -= this.OnAim;
			this._aimAction.canceled -= this.OnAimCancel;
			this._autoToggleAction.performed -= this.OnAutoAimToggle;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00018D48 File Offset: 0x00016F48
		private void Update()
		{
			if (this.previousControlScheme != this.playerInput.currentControlScheme)
			{
				this.OnControlsChanged(this.playerInput);
			}
			if (this.autoAim && !PauseController.isPaused)
			{
				this.AutoAim();
				return;
			}
			if (!this._usingGamepad || this._disableGamepadCursor)
			{
				return;
			}
			Vector2 a = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			if (this._lastGamepadVector != Vector2.zero)
			{
				this.cursorPosition = a + this._lastGamepadVector.normalized * this.fixedDistance;
				this.AnchorCursor(this.cursorPosition);
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00018DF7 File Offset: 0x00016FF7
		public void EnableGamepadCusor()
		{
			this._disableGamepadCursor = false;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00018E00 File Offset: 0x00017000
		public void DisableGamepadCursor()
		{
			this._disableGamepadCursor = true;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00018E0C File Offset: 0x0001700C
		private void AnchorCursor(Vector2 position)
		{
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform, position, this.canvas.worldCamera, out v);
			this.cursorTransform.position = this.canvas.transform.TransformPoint(v);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00018E60 File Offset: 0x00017060
		private void AutoAim()
		{
			Vector2 v = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector2 center = Camera.main.ScreenToWorldPoint(v);
			this.cursorPosition = Camera.main.WorldToScreenPoint(AIController.GetClosestAIPos(center));
			this.AnchorCursor(this.cursorPosition);
		}

		// Token: 0x040002DB RID: 731
		public static ShootingCursor Instance;

		// Token: 0x040002DD RID: 733
		[NonSerialized]
		public bool autoAim;

		// Token: 0x040002DE RID: 734
		[SerializeField]
		private PlayerInput playerInput;

		// Token: 0x040002DF RID: 735
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x040002E0 RID: 736
		[SerializeField]
		private RectTransform cursorTransform;

		// Token: 0x040002E1 RID: 737
		[SerializeField]
		private Canvas canvas;

		// Token: 0x040002E2 RID: 738
		[SerializeField]
		private float fixedDistance = 100f;

		// Token: 0x040002E3 RID: 739
		private const string gamepadScheme = "Gamepad";

		// Token: 0x040002E4 RID: 740
		private const string mouseScheme = "Keyboard&Mouse";

		// Token: 0x040002E5 RID: 741
		private string previousControlScheme = "";

		// Token: 0x040002E6 RID: 742
		private InputAction _pointAction;

		// Token: 0x040002E7 RID: 743
		private InputAction _aimAction;

		// Token: 0x040002E8 RID: 744
		private InputAction _autoToggleAction;

		// Token: 0x040002E9 RID: 745
		private Vector2 _lastGamepadVector = Vector2.zero;

		// Token: 0x040002EA RID: 746
		private bool _usingGamepad;

		// Token: 0x040002EB RID: 747
		private bool _disableGamepadCursor;
	}
}
