using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001BA RID: 442
	public class TweenRotationByVector3 : TweenBase
	{
		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002E1B0 File Offset: 0x0002C3B0
		protected override void UpdateTween()
		{
			base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(this.startRotation, this.endRotation, this.curve.Evaluate(this.tweenT)));
		}

		// Token: 0x04000955 RID: 2389
		[SerializeField]
		[Header("開始位置")]
		private Vector3 startRotation;

		// Token: 0x04000956 RID: 2390
		[SerializeField]
		[Header("結束位置")]
		private Vector3 endRotation;
	}
}
