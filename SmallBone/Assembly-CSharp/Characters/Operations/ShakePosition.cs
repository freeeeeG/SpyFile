using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E01 RID: 3585
	public class ShakePosition : CharacterOperation
	{
		// Token: 0x060047AC RID: 18348 RVA: 0x000D06EA File Offset: 0x000CE8EA
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x000D06FA File Offset: 0x000CE8FA
		private IEnumerator CRun(Character owner)
		{
			float elapsed = 0f;
			float intervalElapsed = 0f;
			Vector3 shakeVector = Vector3.zero;
			while (elapsed <= this._curve.duration)
			{
				float deltaTime = owner.chronometer.master.deltaTime;
				elapsed += deltaTime;
				intervalElapsed -= deltaTime;
				shakeVector -= shakeVector * 60f * deltaTime;
				if (intervalElapsed <= 0f)
				{
					intervalElapsed = this._interval;
					float num = 1f - this._curve.Evaluate(elapsed);
					shakeVector.x = UnityEngine.Random.Range(-this._power, this._power) * num;
					shakeVector.y = UnityEngine.Random.Range(-this._power, this._power) * num;
				}
				this._target.localPosition = shakeVector;
				yield return null;
			}
			this._target.localPosition = Vector3.zero;
			yield break;
		}

		// Token: 0x040036B7 RID: 14007
		[SerializeField]
		private Transform _target;

		// Token: 0x040036B8 RID: 14008
		[SerializeField]
		private float _power = 0.2f;

		// Token: 0x040036B9 RID: 14009
		[SerializeField]
		private Curve _curve;

		// Token: 0x040036BA RID: 14010
		[SerializeField]
		private float _interval = 0.1f;
	}
}
