using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs
{
	// Token: 0x02000374 RID: 884
	public sealed class ClearedLevelCompare : Trigger
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x0003051F File Offset: 0x0002E71F
		protected override bool Check()
		{
			return GameData.HardmodeProgress.clearedLevel.Compare(this._compareLevel, this._operation);
		}

		// Token: 0x04000D4D RID: 3405
		[SerializeField]
		private int _compareLevel;

		// Token: 0x04000D4E RID: 3406
		[SerializeField]
		private ExtensionMethods.CompareOperation _operation;
	}
}
