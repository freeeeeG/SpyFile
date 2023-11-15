using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000AE RID: 174
	public class ScaleBounce : MonoBehaviour
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B13F File Offset: 0x0001933F
		private void OnEnable()
		{
			this._tweenID = LeanTween.scale(base.gameObject, this.scaleTo, 1f / (this.bouncePerSecond / 2f)).setLoopPingPong().setIgnoreTimeScale(this.ignoreTimeScale).id;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B17F File Offset: 0x0001937F
		private void OnDisable()
		{
			if (LeanTween.isTweening(this._tweenID))
			{
				LeanTween.cancel(this._tweenID);
			}
		}

		// Token: 0x04000399 RID: 921
		[SerializeField]
		private Vector3 scaleTo;

		// Token: 0x0400039A RID: 922
		[SerializeField]
		private float bouncePerSecond;

		// Token: 0x0400039B RID: 923
		[SerializeField]
		private bool ignoreTimeScale;

		// Token: 0x0400039C RID: 924
		private int _tweenID;
	}
}
