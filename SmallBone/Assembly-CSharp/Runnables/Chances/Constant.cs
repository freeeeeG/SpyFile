using System;
using UnityEngine;

namespace Runnables.Chances
{
	// Token: 0x02000347 RID: 839
	public sealed class Constant : Chance
	{
		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002F9C1 File Offset: 0x0002DBC1
		public override bool IsTrue()
		{
			return MMMaths.Chance(this._truePercent);
		}

		// Token: 0x04000CFA RID: 3322
		[Range(0f, 1f)]
		[SerializeField]
		private float _truePercent;
	}
}
