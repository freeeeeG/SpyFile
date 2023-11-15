using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000203 RID: 515
	public class RuneUnlocker : TieredUnlockable, ISelectHandler, IEventSystemHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0002B42A File Offset: 0x0002962A
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x0002B43C File Offset: 0x0002963C
		public override int level
		{
			get
			{
				return this.rune.data.level;
			}
			set
			{
				this.rune.data.level = value;
				this.CheckRuneLevelReq();
				this.rune.Refresh();
				UnityEvent unityEvent = this.onLevel;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002B470 File Offset: 0x00029670
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0002B47D File Offset: 0x0002967D
		public bool toggleOn
		{
			get
			{
				return this.toggle.isOn;
			}
			set
			{
				this.toggle.isOn = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0002B48B File Offset: 0x0002968B
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0002B49B File Offset: 0x0002969B
		public bool locked
		{
			get
			{
				return !this.button.interactable;
			}
			set
			{
				this.button.interactable = !value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0002B4AC File Offset: 0x000296AC
		public int costPerLevel
		{
			get
			{
				return this.rune.data.costPerLevel;
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002B4BE File Offset: 0x000296BE
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._isPointerOn = true;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002B4C7 File Offset: 0x000296C7
		public void OnPointerExit(PointerEventData eventData)
		{
			this._isPointerOn = false;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002B4D0 File Offset: 0x000296D0
		public void OnSelect(BaseEventData eventData)
		{
			this._isSelected = true;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002B4D9 File Offset: 0x000296D9
		public void OnDeselect(BaseEventData data)
		{
			this._isSelected = false;
			this.ReleasePress();
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002B4E8 File Offset: 0x000296E8
		public void OnSubmitChanged(InputAction.CallbackContext context)
		{
			if (context.ReadValue<float>() == 0f)
			{
				this.ReleasePress();
				return;
			}
			this.StartPress();
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002B505 File Offset: 0x00029705
		public void OnClickChanged(InputAction.CallbackContext context)
		{
			if (context.ReadValue<float>() == 0f)
			{
				this.ReleasePress();
				return;
			}
			if (this._isPointerOn)
			{
				this.StartPress();
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002B52C File Offset: 0x0002972C
		private void Start()
		{
			this.CheckRuneLevelReq();
			this._submitAction = this.inputs.FindActionMap("UI", false).FindAction("Submit", false);
			this._clickAction = this.inputs.FindActionMap("UI", false).FindAction("Click", false);
			this._submitAction.performed += this.OnSubmitChanged;
			this._clickAction.performed += this.OnClickChanged;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002B5B1 File Offset: 0x000297B1
		private void OnDestroy()
		{
			this._submitAction.performed -= this.OnSubmitChanged;
			this._clickAction.performed -= this.OnClickChanged;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002B5E1 File Offset: 0x000297E1
		private void CheckRuneLevelReq()
		{
			if (this.rune.data.level == 0)
			{
				this.toggle.interactable = false;
				return;
			}
			this.toggle.interactable = true;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002B610 File Offset: 0x00029810
		private void LevelUp()
		{
			int level = this.level;
			this.level = level + 1;
			this.rune.Refresh();
			this.toggle.isOn = true;
			this.levelupSlider.value = 0f;
			this.levelupParticles.Play();
			PointsTracker.pts -= this.costPerLevel;
			UnityEvent unityEvent = this.onLevel;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002B680 File Offset: 0x00029880
		private void StartPress()
		{
			this.ReleasePress();
			if (this.locked)
			{
				return;
			}
			if (this._isSelected && this.toggle.interactable)
			{
				this.toggle.isOn = !this.toggle.isOn;
			}
			if (this._isSelected && this.level < this.maxLevel && this.costPerLevel < PointsTracker.pts)
			{
				this._pressAndHoldCoroutine = this.PressAndHoldCR();
				base.StartCoroutine(this._pressAndHoldCoroutine);
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002B706 File Offset: 0x00029906
		private void ReleasePress()
		{
			if (this._pressAndHoldCoroutine != null)
			{
				base.StopCoroutine(this._pressAndHoldCoroutine);
				this._pressAndHoldCoroutine = null;
			}
			this.levelupSlider.value = 0f;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002B733 File Offset: 0x00029933
		private IEnumerator PressAndHoldCR()
		{
			float timer = 0f;
			while (timer < this.holdToUnlockTime)
			{
				yield return null;
				timer += Time.deltaTime;
				this.levelupSlider.value = timer / this.holdToUnlockTime;
			}
			this.LevelUp();
			this._pressAndHoldCoroutine = null;
			yield break;
		}

		// Token: 0x040007F3 RID: 2035
		public RuneIcon rune;

		// Token: 0x040007F4 RID: 2036
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x040007F5 RID: 2037
		[SerializeField]
		private Toggle toggle;

		// Token: 0x040007F6 RID: 2038
		[SerializeField]
		private Button button;

		// Token: 0x040007F7 RID: 2039
		[SerializeField]
		private Slider levelupSlider;

		// Token: 0x040007F8 RID: 2040
		[SerializeField]
		private ParticleSystem levelupParticles;

		// Token: 0x040007F9 RID: 2041
		[SerializeField]
		private int maxLevel = 5;

		// Token: 0x040007FA RID: 2042
		[SerializeField]
		private float holdToUnlockTime;

		// Token: 0x040007FB RID: 2043
		public UnityEvent onLevel;

		// Token: 0x040007FC RID: 2044
		private InputAction _submitAction;

		// Token: 0x040007FD RID: 2045
		private InputAction _clickAction;

		// Token: 0x040007FE RID: 2046
		private IEnumerator _pressAndHoldCoroutine;

		// Token: 0x040007FF RID: 2047
		private bool _isSelected;

		// Token: 0x04000800 RID: 2048
		private bool _isPointerOn;
	}
}
