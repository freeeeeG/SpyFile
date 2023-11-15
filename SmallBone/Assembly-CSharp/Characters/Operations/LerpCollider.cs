using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E20 RID: 3616
	public class LerpCollider : CharacterOperation
	{
		// Token: 0x06004827 RID: 18471 RVA: 0x000D1DF7 File Offset: 0x000CFFF7
		private void Awake()
		{
			this._originSize = this._source.size;
			this._originOffset = this._source.offset;
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x000D1E1B File Offset: 0x000D001B
		public override void Run(Character owner)
		{
			this._coroutineReference = this.StartCoroutineWithReference(this.CRun(owner));
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x000D1E30 File Offset: 0x000D0030
		private IEnumerator CRun(Character owner)
		{
			Vector2 source = this._source.size;
			Vector2 sourceOffset = this._source.offset;
			Vector2 dest = this._dest.size;
			Vector2 destOffset = this._dest.offset;
			destOffset.x *= base.transform.lossyScale.x;
			yield return this.CLerp(owner.chronometer.master, this._sourceToDestCurve, source, sourceOffset, dest, destOffset);
			if (!this._bounce)
			{
				yield break;
			}
			yield return owner.chronometer.master.WaitForSeconds(this._term);
			yield return this.CLerp(owner.chronometer.master, this._destToSourceCurve, dest, destOffset, source, sourceOffset);
			yield break;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x000D1E46 File Offset: 0x000D0046
		private IEnumerator CLerp(Chronometer chronometer, Curve curve, Vector2 source, Vector2 sourceOffset, Vector2 dest, Vector2 destOffset)
		{
			for (float elapsed = 0f; elapsed < curve.duration; elapsed += chronometer.deltaTime)
			{
				yield return null;
				this._source.size = Vector2.Lerp(source, dest, curve.Evaluate(elapsed));
				this._source.offset = Vector2.Lerp(sourceOffset, destOffset, curve.Evaluate(elapsed));
			}
			this._source.size = dest;
			this._source.offset = destOffset;
			yield break;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x000D1E82 File Offset: 0x000D0082
		public override void Stop()
		{
			this._source.size = this._originSize;
			this._source.offset = this._originOffset;
			this._coroutineReference.Stop();
		}

		// Token: 0x0400373A RID: 14138
		[SerializeField]
		private BoxCollider2D _source;

		// Token: 0x0400373B RID: 14139
		[SerializeField]
		private BoxCollider2D _dest;

		// Token: 0x0400373C RID: 14140
		[SerializeField]
		private Curve _sourceToDestCurve;

		// Token: 0x0400373D RID: 14141
		[FrameTime]
		[SerializeField]
		private float _term;

		// Token: 0x0400373E RID: 14142
		[SerializeField]
		private Curve _destToSourceCurve;

		// Token: 0x0400373F RID: 14143
		[SerializeField]
		private bool _bounce;

		// Token: 0x04003740 RID: 14144
		private CoroutineReference _coroutineReference;

		// Token: 0x04003741 RID: 14145
		private Vector2 _originSize;

		// Token: 0x04003742 RID: 14146
		private Vector2 _originOffset;
	}
}
