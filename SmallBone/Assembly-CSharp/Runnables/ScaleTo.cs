using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002FC RID: 764
	public sealed class ScaleTo : CRunnable
	{
		// Token: 0x06000F02 RID: 3842 RVA: 0x0002E289 File Offset: 0x0002C489
		public override IEnumerator CRun()
		{
			Vector3 start = this._target.transform.localScale;
			Vector2 end = this._destination;
			for (float elapsed = 0f; elapsed < this.curve.duration; elapsed += Chronometer.global.deltaTime)
			{
				yield return null;
				this._target.transform.localScale = Vector2.Lerp(start, end, this.curve.Evaluate(elapsed));
			}
			this._target.transform.localScale = end;
			yield break;
		}

		// Token: 0x04000C5A RID: 3162
		[SerializeField]
		private Transform _target;

		// Token: 0x04000C5B RID: 3163
		[SerializeField]
		private Vector2 _destination;

		// Token: 0x04000C5C RID: 3164
		[SerializeField]
		private Curve curve;
	}
}
