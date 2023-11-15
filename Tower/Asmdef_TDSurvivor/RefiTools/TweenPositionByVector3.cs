using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B8 RID: 440
	public class TweenPositionByVector3 : TweenBase
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
		protected override void UpdateTween()
		{
			if (this.isLocal)
			{
				base.transform.localPosition = Vector3.Lerp(this.startPosition, this.endPosition, this.curve.Evaluate(this.tweenT));
				return;
			}
			base.transform.position = Vector3.Lerp(this.startPosition, this.endPosition, this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0002E114 File Offset: 0x0002C314
		[ContextMenu("儲存目前數值到開始位置")]
		private void SetCurrentAsStartPosition()
		{
			this.startPosition = base.transform.localPosition;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0002E127 File Offset: 0x0002C327
		[ContextMenu("儲存目前數值到結束位置")]
		private void SetCurrentAsEndPosition()
		{
			this.endPosition = base.transform.localPosition;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0002E13A File Offset: 0x0002C33A
		[ContextMenu("讀取儲存的開始位置")]
		private void LoadStartPosition()
		{
			base.transform.localPosition = this.startPosition;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002E14D File Offset: 0x0002C34D
		[ContextMenu("讀取儲存的結束位置")]
		private void LoadEndPosition()
		{
			base.transform.localPosition = this.endPosition;
		}

		// Token: 0x04000950 RID: 2384
		[SerializeField]
		[Header("開始位置")]
		private Vector3 startPosition;

		// Token: 0x04000951 RID: 2385
		[SerializeField]
		[Header("結束位置")]
		private Vector3 endPosition;

		// Token: 0x04000952 RID: 2386
		[SerializeField]
		[Header("是否是Local")]
		private bool isLocal = true;
	}
}
