using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne
{
	// Token: 0x0200006F RID: 111
	public class InputDetector : MonoBehaviour
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00017F8C File Offset: 0x0001618C
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00017F94 File Offset: 0x00016194
		public bool usingGamepad { get; private set; }

		// Token: 0x060004BA RID: 1210 RVA: 0x00017F9D File Offset: 0x0001619D
		private void OnKBMUsed(InputAction.CallbackContext context)
		{
			UnityEvent unityEvent = this.onControllerActive;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.usingGamepad = false;
			MonoBehaviour.print(this.usingGamepad);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00017FC7 File Offset: 0x000161C7
		private void OnGamepadUsed(InputAction.CallbackContext context)
		{
			UnityEvent unityEvent = this.onControllerInactive;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.usingGamepad = true;
			MonoBehaviour.print(this.usingGamepad);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00017FF1 File Offset: 0x000161F1
		private void Awake()
		{
			if (InputDetector.Instance == null)
			{
				InputDetector.Instance = this;
			}
			else if (InputDetector.Instance != this)
			{
				Object.Destroy(base.gameObject);
			}
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001802C File Offset: 0x0001622C
		private void Start()
		{
			this._kbmAction = this.inputs.FindActionMap("InputDetector", false).FindAction("KBMUsed", false);
			this._gamepadAction = this.inputs.FindActionMap("InputDetector", false).FindAction("GamepadUsed", false);
			this._kbmAction.started += this.OnKBMUsed;
			this._gamepadAction.started += this.OnGamepadUsed;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000180AB File Offset: 0x000162AB
		private void OnDestory()
		{
			this._kbmAction.started -= this.OnKBMUsed;
			this._gamepadAction.started -= this.OnGamepadUsed;
		}

		// Token: 0x040002B0 RID: 688
		public static InputDetector Instance;

		// Token: 0x040002B2 RID: 690
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x040002B3 RID: 691
		public UnityEvent onControllerActive;

		// Token: 0x040002B4 RID: 692
		public UnityEvent onControllerInactive;

		// Token: 0x040002B5 RID: 693
		private InputAction _kbmAction;

		// Token: 0x040002B6 RID: 694
		private InputAction _gamepadAction;
	}
}
