using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E24 RID: 3620
	public sealed class ModifyRemainCooldownTime : CharacterOperation
	{
		// Token: 0x0600483B RID: 18491 RVA: 0x000D218C File Offset: 0x000D038C
		public override void Run(Character owner)
		{
			foreach (Characters.Actions.Action action in owner.actions)
			{
				if ((action.cooldown.time != null || action.type == Characters.Actions.Action.Type.Swap) && this._type[action.type])
				{
					if (action.type == Characters.Actions.Action.Type.Swap)
					{
						owner.playerComponents.inventory.weapon.SetSwapCooldown(this._amount.value);
					}
					else if (action.cooldown.time.stacks > 1 && action.cooldown.time.stacks != action.cooldown.time.maxStack)
					{
						action.cooldown.time.remainTime = this._amount.value;
					}
					else if (!action.cooldown.time.canUse)
					{
						action.cooldown.time.remainTime = this._amount.value;
					}
				}
			}
		}

		// Token: 0x04003756 RID: 14166
		[SerializeField]
		private ActionTypeBoolArray _type;

		// Token: 0x04003757 RID: 14167
		[SerializeField]
		private CustomFloat _amount;
	}
}
