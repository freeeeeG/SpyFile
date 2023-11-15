using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200009A RID: 154
	public class BounceOnSpawn : MonoBehaviour
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x0001A520 File Offset: 0x00018720
		private void OnEnable()
		{
			this._startPos = base.transform.localPosition;
			Vector3 to = this._startPos + new Vector3(0f, this.bounceHeight, 0f);
			this._tweenID = LeanTween.moveLocal(base.gameObject, to, this.duration / 4f).setEase(LeanTweenType.easeOutSine).setOnComplete(new Action(this.Fall)).id;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001A59A File Offset: 0x0001879A
		private void OnDisable()
		{
			if (LeanTween.isTweening(this._tweenID))
			{
				LeanTween.cancel(this._tweenID);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001A5B4 File Offset: 0x000187B4
		private void Fall()
		{
			this._tweenID = LeanTween.moveLocal(base.gameObject, this._startPos, this.duration * 0.75f).setEase(LeanTweenType.easeOutBounce).id;
		}

		// Token: 0x04000369 RID: 873
		[SerializeField]
		private float bounceHeight;

		// Token: 0x0400036A RID: 874
		[SerializeField]
		private float duration;

		// Token: 0x0400036B RID: 875
		private Vector3 _startPos;

		// Token: 0x0400036C RID: 876
		private int _tweenID;
	}
}
