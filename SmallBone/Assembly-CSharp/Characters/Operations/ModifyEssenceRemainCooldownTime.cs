using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E23 RID: 3619
	public sealed class ModifyEssenceRemainCooldownTime : CharacterOperation
	{
		// Token: 0x06004839 RID: 18489 RVA: 0x000D2149 File Offset: 0x000D0349
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			owner.playerComponents.inventory.quintessence.items[0].cooldown.time.remainTime = this._amount.value;
		}

		// Token: 0x04003755 RID: 14165
		[SerializeField]
		private CustomFloat _amount;
	}
}
