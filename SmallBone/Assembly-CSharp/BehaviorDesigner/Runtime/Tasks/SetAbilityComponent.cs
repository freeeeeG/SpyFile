using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200147F RID: 5247
	public sealed class SetAbilityComponent : Action
	{
		// Token: 0x0600664A RID: 26186 RVA: 0x00127DB9 File Offset: 0x00125FB9
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
			this._ability = this._abilityComponent.Value.ability;
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x00127DE2 File Offset: 0x00125FE2
		public override void OnStart()
		{
			this._ability.Initialize();
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x00127DF0 File Offset: 0x00125FF0
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue == null || this._ability == null)
			{
				return TaskStatus.Failure;
			}
			if (this._isAttach)
			{
				this._ownerValue.ability.Add(this._ability);
			}
			else
			{
				this._ownerValue.ability.Remove(this._ability);
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005256 RID: 21078
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005257 RID: 21079
		[SerializeField]
		private SharedAbilityComponent _abilityComponent;

		// Token: 0x04005258 RID: 21080
		[SerializeField]
		private bool _isAttach = true;

		// Token: 0x04005259 RID: 21081
		private Character _ownerValue;

		// Token: 0x0400525A RID: 21082
		private IAbility _ability;
	}
}
