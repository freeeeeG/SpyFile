using System;
using System.Collections;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002FE RID: 766
	public sealed class TransformTranslateTo : CRunnable
	{
		// Token: 0x06000F0A RID: 3850 RVA: 0x0002E3A3 File Offset: 0x0002C5A3
		public override IEnumerator CRun()
		{
			Vector3 start = this._target.transform.position;
			Vector3 end = this._destination.position;
			for (float elapsed = 0f; elapsed < this.curve.duration; elapsed += Chronometer.global.deltaTime)
			{
				yield return null;
				this._target.transform.position = Vector2.Lerp(start, end, this.curve.Evaluate(elapsed));
			}
			this._target.transform.position = end;
			yield break;
		}

		// Token: 0x04000C63 RID: 3171
		[SerializeField]
		private Transform _target;

		// Token: 0x04000C64 RID: 3172
		[SerializeField]
		private Transform _destination;

		// Token: 0x04000C65 RID: 3173
		[SerializeField]
		private Curve curve;
	}
}
