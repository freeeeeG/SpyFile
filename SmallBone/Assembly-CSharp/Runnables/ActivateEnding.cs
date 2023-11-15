using System;
using Data;

namespace Runnables
{
	// Token: 0x02000313 RID: 787
	public sealed class ActivateEnding : Runnable
	{
		// Token: 0x06000F47 RID: 3911 RVA: 0x0002EB40 File Offset: 0x0002CD40
		public override void Run()
		{
			GameData.Generic.normalEnding = true;
			GameData.Generic.SaveAll();
		}
	}
}
