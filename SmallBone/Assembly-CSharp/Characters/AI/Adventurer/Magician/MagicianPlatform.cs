using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician
{
	// Token: 0x020013F3 RID: 5107
	public class MagicianPlatform : MonoBehaviour
	{
		// Token: 0x0600648B RID: 25739 RVA: 0x00123AA8 File Offset: 0x00121CA8
		public void Initialize(MagicianPlatformController controller)
		{
			this._controller = controller;
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x00123AB1 File Offset: 0x00121CB1
		public void Show()
		{
			base.gameObject.SetActive(true);
			this._collider.enabled = true;
			base.StartCoroutine(this.CStartLifeCycle());
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x00123AD8 File Offset: 0x00121CD8
		private void Hide()
		{
			base.gameObject.SetActive(false);
			this._collider.enabled = false;
			this._controller.AddPlatform(this, this._left);
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x00123B04 File Offset: 0x00121D04
		private IEnumerator CStartLifeCycle()
		{
			this.FadeOut();
			yield return Chronometer.global.WaitForSeconds(this._lifeTime);
			this.FadeIn();
			yield break;
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x00123B13 File Offset: 0x00121D13
		private void FadeOut()
		{
			base.StartCoroutine(this.CFadeOut());
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x00123B22 File Offset: 0x00121D22
		private IEnumerator CFadeOut()
		{
			float t = 0f;
			this.SetFadeAlpha(0f);
			yield return null;
			while (t < 1f)
			{
				this.SetFadeAlpha(t);
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this.SetFadeAlpha(1f);
			yield break;
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x00123B31 File Offset: 0x00121D31
		private void FadeIn()
		{
			base.StartCoroutine(this.CFadeIn());
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x00123B40 File Offset: 0x00121D40
		private IEnumerator CFadeIn()
		{
			float t = 0f;
			this.SetFadeAlpha(1f);
			yield return null;
			while (t < 1f)
			{
				this.SetFadeAlpha(1f - t);
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this.SetFadeAlpha(0f);
			this.Hide();
			yield break;
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x00123B50 File Offset: 0x00121D50
		private void SetFadeAlpha(float alpha)
		{
			Color color = this._renderer.color;
			color.a = alpha;
			this._renderer.color = color;
		}

		// Token: 0x04005119 RID: 20761
		[SerializeField]
		private bool _left;

		// Token: 0x0400511A RID: 20762
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x0400511B RID: 20763
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x0400511C RID: 20764
		[SerializeField]
		private float _lifeTime;

		// Token: 0x0400511D RID: 20765
		private MagicianPlatformController _controller;
	}
}
