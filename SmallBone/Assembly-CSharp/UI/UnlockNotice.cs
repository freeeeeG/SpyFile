using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003E2 RID: 994
	public sealed class UnlockNotice : MonoBehaviour
	{
		// Token: 0x0600128C RID: 4748 RVA: 0x00037794 File Offset: 0x00035994
		public void Show(Sprite icon, string name)
		{
			this._icon.sprite = icon;
			this._icon.SetNativeSize();
			this._name.text = name;
			base.gameObject.SetActive(true);
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.CFadeInOut());
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x000377F1 File Offset: 0x000359F1
		private IEnumerator CFadeInOut()
		{
			if (this._animator.runtimeAnimatorController != null)
			{
				if (!this._animator.enabled)
				{
					this._animator.enabled = true;
				}
				this._animator.Play(0, 0, 0f);
			}
			float remain = this._animator.GetCurrentAnimatorStateInfo(0).length;
			this._animator.enabled = false;
			while (remain > 1E-45f)
			{
				yield return null;
				float unscaledDeltaTime = Time.unscaledDeltaTime;
				this._animator.Update(unscaledDeltaTime);
				remain -= unscaledDeltaTime;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000F8A RID: 3978
		[SerializeField]
		private Image _icon;

		// Token: 0x04000F8B RID: 3979
		[SerializeField]
		private TextMeshProUGUI _name;

		// Token: 0x04000F8C RID: 3980
		[SerializeField]
		private Animator _animator;
	}
}
