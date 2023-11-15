using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001BB RID: 443
	public class TweenScaleByVector3 : TweenBase
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002E1EC File Offset: 0x0002C3EC
		protected override void UpdateTween()
		{
			base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, this.curve.Evaluate(this.tweenT));
		}

		// Token: 0x04000957 RID: 2391
		[SerializeField]
		[Header("開始尺寸")]
		private Vector3 startScale;

		// Token: 0x04000958 RID: 2392
		[SerializeField]
		[Header("結束尺寸")]
		private Vector3 endScale;
	}
}
