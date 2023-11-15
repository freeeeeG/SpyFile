using System;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E84 RID: 3716
	public class Heal : CharacterOperation
	{
		// Token: 0x0600499D RID: 18845 RVA: 0x000D7160 File Offset: 0x000D5360
		public override void Run(Character owner)
		{
			if (this._target == null)
			{
				owner.health.Heal(this.GetAmount(owner), true);
				return;
			}
			this._target.health.Heal(this.GetAmount(this._target), true);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x000D71B0 File Offset: 0x000D53B0
		private double GetAmount(Character target)
		{
			Heal.Type type = this._type;
			if (type == Heal.Type.Percent)
			{
				return (double)this._amount.value * target.health.maximumHealth * 0.01;
			}
			if (type != Heal.Type.Constnat)
			{
				return 0.0;
			}
			return (double)this._amount.value;
		}

		// Token: 0x040038D2 RID: 14546
		[SerializeField]
		private Character _target;

		// Token: 0x040038D3 RID: 14547
		[SerializeField]
		private Heal.Type _type;

		// Token: 0x040038D4 RID: 14548
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x02000E85 RID: 3717
		private enum Type
		{
			// Token: 0x040038D6 RID: 14550
			Percent,
			// Token: 0x040038D7 RID: 14551
			Constnat
		}
	}
}
