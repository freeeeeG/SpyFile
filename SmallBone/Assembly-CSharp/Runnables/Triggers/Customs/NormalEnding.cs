using System;
using Data;

namespace Runnables.Triggers.Customs
{
	// Token: 0x0200037A RID: 890
	public class NormalEnding : Trigger
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x00030599 File Offset: 0x0002E799
		protected override bool Check()
		{
			return GameData.Generic.normalEnding;
		}
	}
}
