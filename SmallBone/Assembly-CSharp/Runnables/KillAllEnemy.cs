using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Level;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000326 RID: 806
	public class KillAllEnemy : Runnable
	{
		// Token: 0x06000F7F RID: 3967 RVA: 0x0002F14C File Offset: 0x0002D34C
		public override void Run()
		{
			(from enemy in Map.Instance.waveContainer.GetAllEnemies()
			where !this._excepts.Contains(enemy)
			select enemy).ToList<Character>().ForEach(delegate(Character enemy)
			{
				enemy.health.Kill();
			});
		}

		// Token: 0x04000CBE RID: 3262
		[SerializeField]
		private List<Character> _excepts;
	}
}
