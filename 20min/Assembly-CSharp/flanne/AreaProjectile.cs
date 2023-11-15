using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000100 RID: 256
	public class AreaProjectile : MonoBehaviour
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x0001FFE8 File Offset: 0x0001E1E8
		public void TargetPos(Vector2 pos, float duration)
		{
			this.lobTransform.LeanMoveLocalY(2f, duration / 2f).setLoopPingPong(1).setEase(LeanTweenType.easeOutCubic);
			this.lobTransform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
			LeanTween.move(base.gameObject, pos, duration).setOnComplete(new Action(this.OnTargetReached));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00020061 File Offset: 0x0001E261
		private void OnTargetReached()
		{
			this.onTargetReached.Invoke();
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000528 RID: 1320
		[SerializeField]
		private Transform lobTransform;

		// Token: 0x04000529 RID: 1321
		public UnityEvent onTargetReached;
	}
}
