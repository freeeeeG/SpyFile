using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DAA RID: 3498
	public class AttachAbility : CharacterOperation
	{
		// Token: 0x0600466C RID: 18028 RVA: 0x000CB6DB File Offset: 0x000C98DB
		public override void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x000CB6E8 File Offset: 0x000C98E8
		public override void Run(Character target)
		{
			this._target = target;
			target.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x000CB708 File Offset: 0x000C9908
		public override void Run(Character attacker, Character target)
		{
			this._target = target;
			target.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x000CB728 File Offset: 0x000C9928
		public override void Stop()
		{
			Character target = this._target;
			if (target == null)
			{
				return;
			}
			target.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04003553 RID: 13651
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04003554 RID: 13652
		private Character _target;
	}
}
