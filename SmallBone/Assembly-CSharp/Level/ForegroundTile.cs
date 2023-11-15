using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
	// Token: 0x020004D9 RID: 1241
	public class ForegroundTile : MonoBehaviour
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x0004BD42 File Offset: 0x00049F42
		// (set) Token: 0x06001830 RID: 6192 RVA: 0x0004BD54 File Offset: 0x00049F54
		private float _alpha
		{
			get
			{
				return this._tilemap.color.a;
			}
			set
			{
				Color color = this._tilemap.color;
				color.a = value;
				this._tilemap.color = color;
			}
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0004BD81 File Offset: 0x00049F81
		private void Active()
		{
			if (this._alphaChange != null)
			{
				base.StopCoroutine(this._alphaChange);
			}
			this._alphaChange = base.StartCoroutine(this.CAlphaChange(1f, this._alphaChangeTime));
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0004BDB4 File Offset: 0x00049FB4
		private void Inactive()
		{
			if (this._alphaChange != null)
			{
				base.StopCoroutine(this._alphaChange);
			}
			this._alphaChange = base.StartCoroutine(this.CAlphaChange(0f, this._alphaChangeTime));
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0004BDE7 File Offset: 0x00049FE7
		private IEnumerator CAlphaChange(float targetAlpha, float time)
		{
			float startTime = Time.time;
			float startAlpha = this._alpha;
			float num = Mathf.Abs(targetAlpha - startAlpha);
			float newTime = time * num;
			float spendTime = Time.time - startTime;
			while (spendTime < newTime)
			{
				this._alpha = startAlpha + (targetAlpha - startAlpha) * (spendTime / newTime);
				spendTime = Time.time - startTime;
				yield return null;
			}
			this._alpha = targetAlpha;
			yield break;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0004BE04 File Offset: 0x0004A004
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				this._triggerCount++;
				if (this._triggerCount > 0)
				{
					this.Inactive();
				}
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0004BE32 File Offset: 0x0004A032
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				this._triggerCount--;
				if (this._triggerCount <= 0)
				{
					this.Active();
				}
			}
		}

		// Token: 0x0400150E RID: 5390
		[SerializeField]
		private Tilemap _tilemap;

		// Token: 0x0400150F RID: 5391
		[SerializeField]
		private float _alphaChangeTime = 0.3f;

		// Token: 0x04001510 RID: 5392
		private Coroutine _alphaChange;

		// Token: 0x04001511 RID: 5393
		private int _triggerCount;
	}
}
