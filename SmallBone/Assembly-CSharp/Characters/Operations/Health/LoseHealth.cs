using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E8B RID: 3723
	public class LoseHealth : CharacterOperation
	{
		// Token: 0x060049B7 RID: 18871 RVA: 0x000D744C File Offset: 0x000D564C
		public override void Run(Character owner)
		{
			double amount = this.GetAmount(owner);
			if (!this._allowSmallAmount && amount < 1.0)
			{
				return;
			}
			owner.health.TakeHealth(amount);
			if (this._spawnFloatingText)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(amount, owner.transform.position);
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x000D74AC File Offset: 0x000D56AC
		private double GetAmount(Character owner)
		{
			switch (this._type)
			{
			case LoseHealth.Type.Constnat:
				return (double)this._amount.value;
			case LoseHealth.Type.Percent:
				return (double)this._amount.value * owner.health.maximumHealth * 0.01;
			case LoseHealth.Type.CurrentPercent:
				return (double)this._amount.value * owner.health.currentHealth * 0.01;
			default:
				return 0.0;
			}
		}

		// Token: 0x040038E6 RID: 14566
		[SerializeField]
		private LoseHealth.Type _type;

		// Token: 0x040038E7 RID: 14567
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x040038E8 RID: 14568
		[Tooltip("피해입었을 때 나타나는 숫자를 띄울지")]
		[SerializeField]
		private bool _spawnFloatingText;

		// Token: 0x040038E9 RID: 14569
		[SerializeField]
		private bool _allowSmallAmount;

		// Token: 0x02000E8C RID: 3724
		private enum Type
		{
			// Token: 0x040038EB RID: 14571
			Constnat,
			// Token: 0x040038EC RID: 14572
			Percent,
			// Token: 0x040038ED RID: 14573
			CurrentPercent
		}
	}
}
