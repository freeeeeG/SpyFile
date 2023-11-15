using System;

namespace Level.Waves
{
	// Token: 0x02000555 RID: 1365
	public sealed class ReturnFail : Leaf
	{
		// Token: 0x06001B01 RID: 6913 RVA: 0x00018EC5 File Offset: 0x000170C5
		protected override bool Check(EnemyWave wave)
		{
			return false;
		}
	}
}
