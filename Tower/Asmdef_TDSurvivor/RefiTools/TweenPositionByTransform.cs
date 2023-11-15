using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B7 RID: 439
	public class TweenPositionByTransform : TweenBase
	{
		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002E062 File Offset: 0x0002C262
		protected override void UpdateTween()
		{
			base.transform.position = Vector3.Lerp(this.startPosition.position, this.endPosition.position, this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x0400094E RID: 2382
		[SerializeField]
		[Header("開始位置")]
		private Transform startPosition;

		// Token: 0x0400094F RID: 2383
		[SerializeField]
		[Header("結束位置")]
		private Transform endPosition;
	}
}
