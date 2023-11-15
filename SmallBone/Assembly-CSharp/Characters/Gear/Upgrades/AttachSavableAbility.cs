using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084B RID: 2123
	public sealed class AttachSavableAbility : UpgradeAbility
	{
		// Token: 0x06002C34 RID: 11316 RVA: 0x00087624 File Offset: 0x00085824
		public override void Attach(Character target)
		{
			if (target == null)
			{
				Debug.LogError("Player is null");
				return;
			}
			this._target = target;
			if (this._stack == 0)
			{
				this._target.playerComponents.savableAbilityManager.Apply(this._name);
				return;
			}
			this._target.playerComponents.savableAbilityManager.Apply(this._name, this._stack);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00087691 File Offset: 0x00085891
		public override void Detach()
		{
			this._target.playerComponents.savableAbilityManager.Remove(this._name);
		}

		// Token: 0x0400255E RID: 9566
		[SerializeField]
		private SavableAbilityManager.Name _name;

		// Token: 0x0400255F RID: 9567
		[SerializeField]
		private int _stack;

		// Token: 0x04002560 RID: 9568
		private Character _target;
	}
}
