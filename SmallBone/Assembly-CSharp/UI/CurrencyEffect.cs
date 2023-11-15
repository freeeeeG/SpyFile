using System;
using System.Collections;
using UnityEngine;

namespace UI
{
	// Token: 0x02000392 RID: 914
	public class CurrencyEffect : MonoBehaviour
	{
		// Token: 0x060010BF RID: 4287 RVA: 0x00031750 File Offset: 0x0002F950
		private void Awake()
		{
			this._animationLength = this._animator.GetCurrentAnimatorStateInfo(0).length;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00031778 File Offset: 0x0002F978
		public void Play()
		{
			this._animator.enabled = true;
			this._animator.Play(0, 0, 0f);
			this._animator.enabled = false;
			this._remainTime = this._animationLength;
			if (base.gameObject.activeSelf)
			{
				return;
			}
			base.gameObject.SetActive(true);
			CoroutineProxy.instance.StartCoroutine(this.CPlay());
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000317E6 File Offset: 0x0002F9E6
		private IEnumerator CPlay()
		{
			while (this._remainTime > 0f)
			{
				yield return null;
				float deltaTime = Chronometer.global.deltaTime;
				this._animator.Update(deltaTime);
				this._remainTime -= deltaTime;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000DBB RID: 3515
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000DBC RID: 3516
		private float _animationLength;

		// Token: 0x04000DBD RID: 3517
		private float _remainTime;
	}
}
