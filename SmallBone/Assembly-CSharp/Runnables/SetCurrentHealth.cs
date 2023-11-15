using System;
using Characters;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200032D RID: 813
	public sealed class SetCurrentHealth : Runnable
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x0002F250 File Offset: 0x0002D450
		public override void Run()
		{
			if (this._amount.value <= 0f)
			{
				this._target.health.Kill();
				return;
			}
			if (this._healthType == SetCurrentHealth.HealthType.Constant)
			{
				this._target.health.SetCurrentHealth((double)this._amount.value);
				return;
			}
			this._target.health.SetCurrentHealth(this._target.health.maximumHealth * (double)this._amount.value * 0.009999999776482582);
		}

		// Token: 0x04000CC6 RID: 3270
		[SerializeField]
		private Character _target;

		// Token: 0x04000CC7 RID: 3271
		[SerializeField]
		private SetCurrentHealth.HealthType _healthType = SetCurrentHealth.HealthType.Constant;

		// Token: 0x04000CC8 RID: 3272
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x0200032E RID: 814
		public enum HealthType
		{
			// Token: 0x04000CCA RID: 3274
			Percent,
			// Token: 0x04000CCB RID: 3275
			Constant
		}
	}
}
