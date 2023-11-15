using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
	// Token: 0x02001653 RID: 5715
	[TaskDescription("일정량의 데미지를 받을 때 까지 Running을 실행")]
	public sealed class RunningUntilAmountOfDamage : Action
	{
		// Token: 0x06006CF1 RID: 27889 RVA: 0x0013765D File Offset: 0x0013585D
		public override void OnAwake()
		{
			this._health = this._owner.Value.health;
		}

		// Token: 0x06006CF2 RID: 27890 RVA: 0x00137675 File Offset: 0x00135875
		public override void OnStart()
		{
			this._tookDamage = 0.0;
			this._health.onTookDamage += new TookDamageDelegate(this.SumTookDamage);
			this._amountOfDamageValue = this._amountOfDamage.Value;
		}

		// Token: 0x06006CF3 RID: 27891 RVA: 0x001376AE File Offset: 0x001358AE
		public override TaskStatus OnUpdate()
		{
			if (this._tookDamage < (double)this._amountOfDamageValue)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CF4 RID: 27892 RVA: 0x001376C2 File Offset: 0x001358C2
		public override void OnEnd()
		{
			this._health.onTookDamage -= new TookDamageDelegate(this.SumTookDamage);
		}

		// Token: 0x06006CF5 RID: 27893 RVA: 0x001376DB File Offset: 0x001358DB
		private void SumTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDelta)
		{
			this._tookDamage += damageDelta;
		}

		// Token: 0x040058B5 RID: 22709
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040058B6 RID: 22710
		private Health _health;

		// Token: 0x040058B7 RID: 22711
		[SerializeField]
		private SharedFloat _amountOfDamage;

		// Token: 0x040058B8 RID: 22712
		private float _amountOfDamageValue;

		// Token: 0x040058B9 RID: 22713
		private double _tookDamage;
	}
}
