using System;

namespace Level.Waves
{
	// Token: 0x0200054D RID: 1357
	public sealed class Always : Leaf
	{
		// Token: 0x06001AF4 RID: 6900 RVA: 0x000076D4 File Offset: 0x000058D4
		protected override bool Check(EnemyWave wave)
		{
			return true;
		}
	}
}
