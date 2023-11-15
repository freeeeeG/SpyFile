using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B6 RID: 438
	public class TweenPositionAndRotationByVector3 : TweenBase
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002DEA4 File Offset: 0x0002C0A4
		[ContextMenu("儲存到開始位置")]
		private void SaveCurrentTransformAsStart()
		{
			if (this.isLocal)
			{
				this.startPosition = base.transform.localPosition;
				this.startRotation = base.transform.localRotation.eulerAngles;
				return;
			}
			this.startPosition = base.transform.position;
			this.startRotation = base.transform.rotation.eulerAngles;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002DF10 File Offset: 0x0002C110
		[ContextMenu("儲存到結束位置")]
		private void SaveCurrentTransformAsEnd()
		{
			if (this.isLocal)
			{
				this.endPosition = base.transform.localPosition;
				this.endRotation = base.transform.localRotation.eulerAngles;
				return;
			}
			this.endPosition = base.transform.position;
			this.endRotation = base.transform.rotation.eulerAngles;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002DF7C File Offset: 0x0002C17C
		protected override void UpdateTween()
		{
			if (this.isLocal)
			{
				base.transform.localPosition = Vector3.Lerp(this.startPosition, this.endPosition, this.curve.Evaluate(this.tweenT));
				base.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(this.startRotation), Quaternion.Euler(this.endRotation), this.curve.Evaluate(this.tweenT));
				return;
			}
			base.transform.position = Vector3.Lerp(this.startPosition, this.endPosition, this.curve.Evaluate(this.tweenT));
			base.transform.rotation = Quaternion.Lerp(Quaternion.Euler(this.startRotation), Quaternion.Euler(this.endRotation), this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x04000949 RID: 2377
		[SerializeField]
		[Header("開始位置")]
		private Vector3 startPosition;

		// Token: 0x0400094A RID: 2378
		[SerializeField]
		[Header("開始位置")]
		private Vector3 startRotation;

		// Token: 0x0400094B RID: 2379
		[SerializeField]
		[Header("結束位置")]
		private Vector3 endPosition;

		// Token: 0x0400094C RID: 2380
		[SerializeField]
		[Header("結束位置")]
		private Vector3 endRotation;

		// Token: 0x0400094D RID: 2381
		[SerializeField]
		[Header("是否是Local")]
		private bool isLocal;
	}
}
