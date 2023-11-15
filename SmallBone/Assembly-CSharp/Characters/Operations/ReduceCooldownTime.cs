using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E2F RID: 3631
	public class ReduceCooldownTime : CharacterOperation
	{
		// Token: 0x06004870 RID: 18544 RVA: 0x000D28D4 File Offset: 0x000D0AD4
		public override void Run(Character owner)
		{
			if (this._specificAction != null)
			{
				this.Reduce(this._specificAction);
				return;
			}
			foreach (Characters.Actions.Action action in owner.actions)
			{
				if (action.cooldown.time != null)
				{
					bool flag = this._skill && action.type == Characters.Actions.Action.Type.Skill;
					bool flag2 = this._dash && action.type == Characters.Actions.Action.Type.Dash;
					bool flag3 = this._swap && action.type == Characters.Actions.Action.Type.Swap;
					if (flag || flag2 || flag3)
					{
						this.Reduce(action);
					}
				}
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x000D2998 File Offset: 0x000D0B98
		private void Reduce(Characters.Actions.Action action)
		{
			ReduceCooldownTime.Type type = this._type;
			if (type == ReduceCooldownTime.Type.Constant)
			{
				action.cooldown.time.ReduceCooldown(this._amount);
				return;
			}
			if (type != ReduceCooldownTime.Type.Percent)
			{
				return;
			}
			action.cooldown.time.ReduceCooldownPercent(this._amount);
		}

		// Token: 0x04003776 RID: 14198
		[SerializeField]
		private ReduceCooldownTime.Type _type;

		// Token: 0x04003777 RID: 14199
		[SerializeField]
		[Information("Percent의 경우 (0~1)", InformationAttribute.InformationType.Info, false)]
		private float _amount;

		// Token: 0x04003778 RID: 14200
		[SerializeField]
		[Information("특정 액션만 적용할 때", InformationAttribute.InformationType.Info, false)]
		private Characters.Actions.Action _specificAction;

		// Token: 0x04003779 RID: 14201
		[SerializeField]
		private bool _skill;

		// Token: 0x0400377A RID: 14202
		[SerializeField]
		private bool _dash;

		// Token: 0x0400377B RID: 14203
		[SerializeField]
		private bool _swap;

		// Token: 0x02000E30 RID: 3632
		private enum Type
		{
			// Token: 0x0400377D RID: 14205
			Constant,
			// Token: 0x0400377E RID: 14206
			Percent
		}
	}
}
