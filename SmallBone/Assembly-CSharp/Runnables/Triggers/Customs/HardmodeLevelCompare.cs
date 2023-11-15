using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers.Customs
{
	// Token: 0x02000377 RID: 887
	public sealed class HardmodeLevelCompare : Trigger
	{
		// Token: 0x0600104E RID: 4174 RVA: 0x0003055B File Offset: 0x0002E75B
		protected override bool Check()
		{
			return GameData.HardmodeProgress.hardmodeLevel.Compare(this._compareLevel, this._operation);
		}

		// Token: 0x04000D51 RID: 3409
		[SerializeField]
		private int _compareLevel;

		// Token: 0x04000D52 RID: 3410
		[SerializeField]
		private ExtensionMethods.CompareOperation _operation;
	}
}
