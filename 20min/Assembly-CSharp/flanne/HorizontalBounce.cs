using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000AA RID: 170
	public class HorizontalBounce : MonoBehaviour
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x0001AE5C File Offset: 0x0001905C
		private void OnEnable()
		{
			float to = base.transform.localPosition.y + this.bounceHeight;
			this._tweenID = LeanTween.moveLocalY(base.gameObject, to, 1f / (this.bouncePerSecond / 2f)).setLoopPingPong().id;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001AEAF File Offset: 0x000190AF
		private void OnDisable()
		{
			if (LeanTween.isTweening(this._tweenID))
			{
				LeanTween.cancel(this._tweenID);
			}
		}

		// Token: 0x0400038E RID: 910
		[SerializeField]
		private float bounceHeight;

		// Token: 0x0400038F RID: 911
		[SerializeField]
		private float bouncePerSecond;

		// Token: 0x04000390 RID: 912
		private int _tweenID;
	}
}
