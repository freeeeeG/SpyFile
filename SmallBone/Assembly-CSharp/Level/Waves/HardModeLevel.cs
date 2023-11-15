using System;
using Data;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000551 RID: 1361
	public sealed class HardModeLevel : Leaf
	{
		// Token: 0x06001AFC RID: 6908 RVA: 0x00054200 File Offset: 0x00052400
		protected override bool Check(EnemyWave wave)
		{
			switch (this._compare)
			{
			case HardModeLevel.Compare.Greater:
				return this._level < GameData.HardmodeProgress.hardmodeLevel;
			case HardModeLevel.Compare.Equal:
				return this._level == GameData.HardmodeProgress.hardmodeLevel;
			case HardModeLevel.Compare.Lower:
				return this._level > GameData.HardmodeProgress.hardmodeLevel;
			default:
				return true;
			}
		}

		// Token: 0x04001738 RID: 5944
		[SerializeField]
		private HardModeLevel.Compare _compare;

		// Token: 0x04001739 RID: 5945
		[SerializeField]
		[Range(0f, 10f)]
		private int _level;

		// Token: 0x02000552 RID: 1362
		private enum Compare
		{
			// Token: 0x0400173B RID: 5947
			Greater,
			// Token: 0x0400173C RID: 5948
			Equal,
			// Token: 0x0400173D RID: 5949
			Lower
		}
	}
}
