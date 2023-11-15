using System;
using System.Collections;
using Characters.Controllers;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003AB RID: 939
	public class LetterBox : MonoBehaviour
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x00033456 File Offset: 0x00031656
		public static LetterBox instance
		{
			get
			{
				return Scene<GameBase>.instance.uiManager.letterBox;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00019108 File Offset: 0x00017308
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x00033467 File Offset: 0x00031667
		public bool visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00033475 File Offset: 0x00031675
		private void Awake()
		{
			this._originHeight = this._top.rectTransform.sizeDelta.y;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00033492 File Offset: 0x00031692
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x000334A0 File Offset: 0x000316A0
		public void Appear(float duration = 0.4f)
		{
			base.StopAllCoroutines();
			this.visible = true;
			base.StartCoroutine(this.CAppear(duration));
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x000334BD File Offset: 0x000316BD
		public void Disappear(float duration = 0.4f)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			base.StartCoroutine(this.CDisappear(duration));
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x000334DB File Offset: 0x000316DB
		public IEnumerator CAppear(float duration = 0.4f)
		{
			PlayerInput.blocked.Attach(this);
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
			this.visible = true;
			float elapsed = 0f;
			float source = 0f;
			float destination = this._originHeight;
			for (;;)
			{
				float y = Mathf.Lerp(source, destination, elapsed / duration);
				this._top.rectTransform.sizeDelta = new Vector2(this._top.rectTransform.sizeDelta.x, y);
				this._bottom.rectTransform.sizeDelta = new Vector2(this._bottom.rectTransform.sizeDelta.x, y);
				if (elapsed > duration)
				{
					break;
				}
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000334F1 File Offset: 0x000316F1
		public IEnumerator CDisappear(float duration = 0.4f)
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			float elapsed = 0f;
			float destination = 0f;
			for (;;)
			{
				float y = Mathf.Lerp(this._originHeight, destination, elapsed / duration);
				this._top.rectTransform.sizeDelta = new Vector2(this._top.rectTransform.sizeDelta.x, y);
				this._bottom.rectTransform.sizeDelta = new Vector2(this._bottom.rectTransform.sizeDelta.x, y);
				if (elapsed > duration)
				{
					break;
				}
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			this.visible = false;
			PlayerInput.blocked.Detach(this);
			yield break;
		}

		// Token: 0x04000E41 RID: 3649
		private const float _defaultAnimationDuration = 0.4f;

		// Token: 0x04000E42 RID: 3650
		[SerializeField]
		private Image _top;

		// Token: 0x04000E43 RID: 3651
		[SerializeField]
		private Image _bottom;

		// Token: 0x04000E44 RID: 3652
		private float _originHeight;
	}
}
