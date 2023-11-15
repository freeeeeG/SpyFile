using System;
using Level;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000372 RID: 882
	public class WavesOnClear : Trigger
	{
		// Token: 0x06001041 RID: 4161 RVA: 0x00030498 File Offset: 0x0002E698
		private void Start()
		{
			EnemyWave[] waves = this._waves;
			for (int i = 0; i < waves.Length; i++)
			{
				waves[i].onClear += delegate()
				{
					this._clearCount++;
				};
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000304CE File Offset: 0x0002E6CE
		protected override bool Check()
		{
			return this._clearCount >= this._waves.Length;
		}

		// Token: 0x04000D49 RID: 3401
		[SerializeField]
		private EnemyWave[] _waves;

		// Token: 0x04000D4A RID: 3402
		private int _clearCount;
	}
}
