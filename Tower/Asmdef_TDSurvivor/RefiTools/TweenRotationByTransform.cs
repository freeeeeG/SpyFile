using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B9 RID: 441
	public class TweenRotationByTransform : TweenBase
	{
		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002E16F File Offset: 0x0002C36F
		protected override void UpdateTween()
		{
			base.transform.localRotation = Quaternion.Lerp(this.startRotation.rotation, this.endRotation.rotation, this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x04000953 RID: 2387
		[SerializeField]
		[Header("開始位置")]
		private Transform startRotation;

		// Token: 0x04000954 RID: 2388
		[SerializeField]
		[Header("結束位置")]
		private Transform endRotation;
	}
}
