using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers.Customs
{
	// Token: 0x02000375 RID: 885
	public sealed class DarktechActivated : Trigger
	{
		// Token: 0x0600104A RID: 4170 RVA: 0x00030537 File Offset: 0x0002E737
		protected override bool Check()
		{
			return Singleton<DarktechManager>.Instance.IsActivated(this._darktech);
		}

		// Token: 0x04000D4F RID: 3407
		[SerializeField]
		private DarktechData _darktech;
	}
}
