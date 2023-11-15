using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs
{
	// Token: 0x02000378 RID: 888
	public sealed class Mode : Trigger
	{
		// Token: 0x06001050 RID: 4176 RVA: 0x00030573 File Offset: 0x0002E773
		protected override bool Check()
		{
			return (this._level == Mode.Level.Hard && GameData.HardmodeProgress.hardmode) || (this._level == Mode.Level.Normal && !GameData.HardmodeProgress.hardmode);
		}

		// Token: 0x04000D53 RID: 3411
		[SerializeField]
		private Mode.Level _level;

		// Token: 0x02000379 RID: 889
		private enum Level
		{
			// Token: 0x04000D55 RID: 3413
			Normal,
			// Token: 0x04000D56 RID: 3414
			Hard
		}
	}
}
