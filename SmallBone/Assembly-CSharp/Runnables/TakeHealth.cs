using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000336 RID: 822
	public sealed class TakeHealth : Runnable
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002F610 File Offset: 0x0002D810
		public override void Run()
		{
			Character character = this._target.character;
			float value = this._amount.value;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage((double)value, character.transform.position);
			character.health.TakeHealth((double)value);
		}

		// Token: 0x04000CDE RID: 3294
		[SerializeField]
		private Target _target;

		// Token: 0x04000CDF RID: 3295
		[SerializeField]
		private CustomFloat _amount;
	}
}
