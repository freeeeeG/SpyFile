using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FF3 RID: 4083
	public class TranslateTo : Operation
	{
		// Token: 0x06004EDB RID: 20187 RVA: 0x000ECCC5 File Offset: 0x000EAEC5
		public override void Run()
		{
			if (this._cReference != null)
			{
				base.StopCoroutine(this._cReference);
			}
			this._cReference = base.StartCoroutine(this.CTranslate());
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x000ECCED File Offset: 0x000EAEED
		private IEnumerator CTranslate()
		{
			float elapsed = 0f;
			float duration = this._curve.duration;
			Vector3 start = this._object.position;
			Vector3 end = this._target.position;
			while (elapsed <= duration)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
				this._object.position = Vector2.Lerp(start, end, this._curve.Evaluate(elapsed));
			}
			this._object.position = end;
			yield break;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x000ECCFC File Offset: 0x000EAEFC
		public override void Stop()
		{
			base.Stop();
			if (this._cReference != null)
			{
				base.StopCoroutine(this._cReference);
			}
		}

		// Token: 0x04003EFB RID: 16123
		[SerializeField]
		private Transform _object;

		// Token: 0x04003EFC RID: 16124
		[SerializeField]
		private Transform _target;

		// Token: 0x04003EFD RID: 16125
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003EFE RID: 16126
		private Coroutine _cReference;
	}
}
