using System;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x02000629 RID: 1577
	[RequireComponent(typeof(EnemyWave))]
	public class IncreaseByWave : MonoBehaviour
	{
		// Token: 0x06001F9F RID: 8095 RVA: 0x000602B0 File Offset: 0x0005E4B0
		private void Awake()
		{
			this._wave.onSpawn += delegate()
			{
				this._event.AddSpeed(this._amountPerSeconds * (double)this._event.updateInterval);
			};
		}

		// Token: 0x04001ACC RID: 6860
		[GetComponent]
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x04001ACD RID: 6861
		[SerializeField]
		private TimeCostEvent _event;

		// Token: 0x04001ACE RID: 6862
		[SerializeField]
		private double _amountPerSeconds;
	}
}
