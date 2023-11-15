using System;
using System.Collections;
using FX;
using GameResources;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI
{
	// Token: 0x020003C6 RID: 966
	public class PressingButton : MonoBehaviour
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060011F1 RID: 4593 RVA: 0x00034F0C File Offset: 0x0003310C
		// (remove) Token: 0x060011F2 RID: 4594 RVA: 0x00034F44 File Offset: 0x00033144
		public event Action onPressed;

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00034F79 File Offset: 0x00033179
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00034F86 File Offset: 0x00033186
		public string description
		{
			get
			{
				return this._text.text;
			}
			set
			{
				this._text.text = value;
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00034F94 File Offset: 0x00033194
		private void Awake()
		{
			this._action = this.FindAction();
			if (this._action == null)
			{
				throw new Exception("Couldn't found key " + this._actionName);
			}
			KeyMapper.Map.OnSimplifiedLastInputTypeChanged += this.OnLastInputTypeChanged;
			this._action.OnBindingsChanged += this.UpdateImage;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00034FF8 File Offset: 0x000331F8
		private void OnEnable()
		{
			this.StopPressingSound();
			if (this._detectPressingSelf)
			{
				base.StartCoroutine(this.CWaitForPressing());
			}
			this.UpdateImage();
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003501B File Offset: 0x0003321B
		private void OnDisable()
		{
			this.StopPressing();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00035023 File Offset: 0x00033223
		private void Start()
		{
			this.UpdateImage();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003502B File Offset: 0x0003322B
		private void OnDestroy()
		{
			KeyMapper.Map.OnSimplifiedLastInputTypeChanged -= this.OnLastInputTypeChanged;
			this._action.OnBindingsChanged -= this.UpdateImage;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0003505A File Offset: 0x0003325A
		public void PlayPressingSound()
		{
			this._pressingSound.Play();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00035067 File Offset: 0x00033267
		public void StopPressingSound()
		{
			this._pressingSound.Stop();
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00035074 File Offset: 0x00033274
		public void StopPressing()
		{
			this._pressing.Stop();
			this._iconOutline.fillAmount = 1f;
			this.StopPressingSound();
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00035097 File Offset: 0x00033297
		public void SetPercent(float percent)
		{
			this._iconOutline.fillAmount = percent;
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00035023 File Offset: 0x00033223
		private void OnLastInputTypeChanged(BindingSourceType bindingSourceType)
		{
			this.UpdateImage();
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000350A8 File Offset: 0x000332A8
		private void UpdateImage()
		{
			if (this._icon != null)
			{
				this._icon.SetNativeSize();
			}
			BindingSourceType simplifiedLastInputType = KeyMapper.Map.SimplifiedLastInputType;
			foreach (BindingSource bindingSource in this._action.Bindings)
			{
				BindingSourceType bindingSourceType = KeyMap.SimplifyBindingSourceType(bindingSource.BindingSourceType);
				if (simplifiedLastInputType == bindingSourceType)
				{
					Sprite keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(bindingSource, false);
					if (this._icon != null)
					{
						this._icon.sprite = keyIconOrDefault;
						this._icon.SetNativeSize();
					}
					keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(bindingSource, true);
					if (this._icon != null)
					{
						this._iconOutline.sprite = keyIconOrDefault;
						this._iconOutline.SetNativeSize();
					}
					break;
				}
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00035198 File Offset: 0x00033398
		private PlayerAction FindAction()
		{
			foreach (PlayerAction playerAction in KeyMapper.Map.Actions)
			{
				if (playerAction.Name.Equals(this._actionName, StringComparison.OrdinalIgnoreCase))
				{
					return playerAction;
				}
			}
			return null;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00035200 File Offset: 0x00033400
		private IEnumerator CWaitForPressing()
		{
			for (;;)
			{
				if (this._action.WasPressed)
				{
					this._pressing = this.StartCoroutineWithReference(this.CPressing());
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0003520F File Offset: 0x0003340F
		private IEnumerator CPressing()
		{
			this.PlayPressingSound();
			for (float time = 0f; time < 1f; time += Time.unscaledDeltaTime)
			{
				if (!this._action.IsPressed)
				{
					this.StopPressing();
					yield break;
				}
				yield return null;
				this.SetPercent(time / 1f);
			}
			this.StopPressingSound();
			this._iconOutline.fillAmount = 1f;
			Action action = this.onPressed;
			if (action != null)
			{
				action();
			}
			yield break;
		}

		// Token: 0x04000ED8 RID: 3800
		private const float _pressingTime = 1f;

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		[Space]
		private bool _detectPressingSelf;

		// Token: 0x04000EDB RID: 3803
		[SerializeField]
		private Image _icon;

		// Token: 0x04000EDC RID: 3804
		[SerializeField]
		private Image _iconOutline;

		// Token: 0x04000EDD RID: 3805
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04000EDE RID: 3806
		[Space]
		[SerializeField]
		private string _actionName;

		// Token: 0x04000EDF RID: 3807
		[Space]
		[SerializeField]
		private PlaySoundInfo _pressingSound;

		// Token: 0x04000EE0 RID: 3808
		private PlayerAction _action;

		// Token: 0x04000EE1 RID: 3809
		private CoroutineReference _pressing;
	}
}
