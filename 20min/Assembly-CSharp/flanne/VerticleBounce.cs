using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B5 RID: 181
	public class VerticleBounce : MonoBehaviour
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0001B340 File Offset: 0x00019540
		private void OnEnable()
		{
			float to = base.transform.localPosition.y + this.bounceHeight;
			this._tweenID = LeanTween.moveLocalY(base.gameObject, to, 1f / (this.bouncePerSecond / 2f)).setLoopPingPong().id;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001B393 File Offset: 0x00019593
		private void OnDisable()
		{
			if (LeanTween.isTweening(this._tweenID))
			{
				LeanTween.cancel(this._tweenID);
			}
		}

		// Token: 0x040003AA RID: 938
		[SerializeField]
		private float bounceHeight;

		// Token: 0x040003AB RID: 939
		[SerializeField]
		private float bouncePerSecond;

		// Token: 0x040003AC RID: 940
		private int _tweenID;
	}
}
