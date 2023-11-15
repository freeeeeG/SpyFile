using System;
using System.Collections;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001F8 RID: 504
	public sealed class WaitForTransfom : Sequence
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0001C7DB File Offset: 0x0001A9DB
		public override IEnumerator CRun()
		{
			for (float elapsed = 0f; elapsed <= (float)this._maxTime; elapsed += Chronometer.global.deltaTime)
			{
				yield return null;
				if (Vector2.Distance(this._target.position, this._destination.position) < this._epsilon)
				{
					break;
				}
			}
			yield break;
		}

		// Token: 0x0400086F RID: 2159
		[SerializeField]
		private Transform _target;

		// Token: 0x04000870 RID: 2160
		[SerializeField]
		private Transform _destination;

		// Token: 0x04000871 RID: 2161
		[SerializeField]
		private float _epsilon = 1f;

		// Token: 0x04000872 RID: 2162
		[SerializeField]
		private int _maxTime = 5;
	}
}
