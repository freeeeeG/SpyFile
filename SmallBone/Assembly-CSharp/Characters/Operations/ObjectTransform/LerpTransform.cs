using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB8 RID: 4024
	public sealed class LerpTransform : CharacterOperation
	{
		// Token: 0x06004E05 RID: 19973 RVA: 0x000E957D File Offset: 0x000E777D
		public override void Run(Character owner)
		{
			this._reference = this.StartCoroutineWithReference(this.CLerp(owner.chronometer.master));
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x000E959C File Offset: 0x000E779C
		private IEnumerator CLerp(Chronometer chronometer)
		{
			float elapsed = 0f;
			this._target.position = this._source.position;
			this._target.rotation = this._source.rotation;
			this._target.localScale = this._source.localScale;
			while (elapsed < this._curve.duration)
			{
				yield return null;
				float t = this._curve.Evaluate(elapsed);
				this._target.position = Vector3.Lerp(this._source.position, this._dest.position, t);
				this._target.rotation = Quaternion.Lerp(this._source.rotation, this._dest.rotation, t);
				this._target.localScale = Vector3.Lerp(this._source.localScale, this._dest.localScale, t);
				elapsed += chronometer.deltaTime;
			}
			this._target.position = this._dest.position;
			this._target.rotation = this._dest.rotation;
			this._target.localScale = this._dest.localScale;
			yield break;
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x000E95B2 File Offset: 0x000E77B2
		public override void Stop()
		{
			this._reference.Stop();
		}

		// Token: 0x04003E00 RID: 15872
		[SerializeField]
		private Transform _target;

		// Token: 0x04003E01 RID: 15873
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003E02 RID: 15874
		[SerializeField]
		private Transform _source;

		// Token: 0x04003E03 RID: 15875
		[SerializeField]
		private Transform _dest;

		// Token: 0x04003E04 RID: 15876
		private CoroutineReference _reference;
	}
}
