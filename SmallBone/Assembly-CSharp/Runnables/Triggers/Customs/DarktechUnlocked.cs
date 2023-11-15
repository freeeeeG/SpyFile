using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers.Customs
{
	// Token: 0x02000376 RID: 886
	public sealed class DarktechUnlocked : Trigger
	{
		// Token: 0x0600104C RID: 4172 RVA: 0x00030549 File Offset: 0x0002E749
		protected override bool Check()
		{
			return Singleton<DarktechManager>.Instance.IsUnlocked(this._darktech);
		}

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		private DarktechData _darktech;
	}
}
