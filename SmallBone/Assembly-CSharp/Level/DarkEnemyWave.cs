using System;

namespace Level
{
	// Token: 0x02000472 RID: 1138
	public sealed class DarkEnemyWave : EnemyWave
	{
		// Token: 0x060015B8 RID: 5560 RVA: 0x000442B2 File Offset: 0x000424B2
		public override void Initialize()
		{
			base.Initialize();
			DarkEnemySelector.instance.ElectIn(base.characters);
		}
	}
}
