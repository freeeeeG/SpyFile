using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200097C RID: 2428
	public class HealthConstraint : Constraint
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x0009A7CC File Offset: 0x000989CC
		public override bool Pass()
		{
			if (this._action.owner.health == null)
			{
				return false;
			}
			HealthConstraint.Type type = this._type;
			if (type != HealthConstraint.Type.Constnat)
			{
				if (type == HealthConstraint.Type.Percent)
				{
					if (this._action.owner.health.percent < (double)this._amount * 0.01)
					{
						return false;
					}
				}
			}
			else if (this._action.owner.health.currentHealth < (double)this._amount)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x0009A850 File Offset: 0x00098A50
		public override void Consume()
		{
			base.Consume();
			if (!this._loseHealthOnConsume)
			{
				return;
			}
			double num = 0.0;
			if (this._type == HealthConstraint.Type.Percent)
			{
				num = num * this._action.owner.health.maximumHealth * 0.01;
			}
			else
			{
				num = (double)this._amount;
			}
			if (num < 1.0)
			{
				return;
			}
			this._action.owner.health.TakeHealth(num);
			if (this._spawnFloatingText)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(num, this._action.owner.transform.position);
			}
		}

		// Token: 0x04002A3F RID: 10815
		[SerializeField]
		private HealthConstraint.Type _type;

		// Token: 0x04002A40 RID: 10816
		[SerializeField]
		private float _amount;

		// Token: 0x04002A41 RID: 10817
		[SerializeField]
		[Tooltip("이 Constraint를 통과하고 액션이 실행될 때 체력을 소모시킬지")]
		private bool _loseHealthOnConsume;

		// Token: 0x04002A42 RID: 10818
		[Tooltip("피해입었을 때 나타나는 숫자를 띄울지")]
		[SerializeField]
		private bool _spawnFloatingText;

		// Token: 0x0200097D RID: 2429
		private enum Type
		{
			// Token: 0x04002A44 RID: 10820
			Constnat,
			// Token: 0x04002A45 RID: 10821
			Percent
		}
	}
}
