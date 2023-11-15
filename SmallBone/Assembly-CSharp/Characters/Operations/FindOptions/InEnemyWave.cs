using System;
using System.Collections.Generic;
using Level;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EAA RID: 3754
	[Serializable]
	public class InEnemyWave : IScope
	{
		// Token: 0x060049EB RID: 18923 RVA: 0x000D7CA8 File Offset: 0x000D5EA8
		public List<Character> GetEnemyList()
		{
			return Map.Instance.waveContainer.GetAllEnemies();
		}
	}
}
