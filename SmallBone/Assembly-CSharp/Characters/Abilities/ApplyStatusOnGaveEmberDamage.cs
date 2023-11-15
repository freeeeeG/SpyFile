using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009C4 RID: 2500
	[Serializable]
	public class ApplyStatusOnGaveEmberDamage : Ability
	{
		// Token: 0x0600354D RID: 13645 RVA: 0x0009DF17 File Offset: 0x0009C117
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ApplyStatusOnGaveEmberDamage.Instance(owner, this);
		}

		// Token: 0x04002AEF RID: 10991
		[Tooltip("default는 0초")]
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002AF0 RID: 10992
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x04002AF1 RID: 10993
		[Range(1f, 100f)]
		[SerializeField]
		private int _chance = 100;

		// Token: 0x020009C5 RID: 2501
		public class Instance : AbilityInstance<ApplyStatusOnGaveEmberDamage>
		{
			// Token: 0x0600354F RID: 13647 RVA: 0x0009DF30 File Offset: 0x0009C130
			internal Instance(Character owner, ApplyStatusOnGaveEmberDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003550 RID: 13648 RVA: 0x0009DF41 File Offset: 0x0009C141
			protected override void OnAttach()
			{
				this.owner.status.onGaveEmberDamage += this.HandleOnGaveEmberDamage;
				this._elapsed = 0f;
			}

			// Token: 0x06003551 RID: 13649 RVA: 0x0009DF6A File Offset: 0x0009C16A
			protected override void OnDetach()
			{
				this.owner.status.onGaveEmberDamage -= this.HandleOnGaveEmberDamage;
			}

			// Token: 0x06003552 RID: 13650 RVA: 0x0009DF88 File Offset: 0x0009C188
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this.ability._cooldownTime == 0f)
				{
					this._canUse = true;
					return;
				}
				this._elapsed += deltaTime;
				if (this._elapsed >= this.ability._cooldownTime)
				{
					this._canUse = true;
					this._elapsed -= this.ability._cooldownTime;
				}
			}

			// Token: 0x06003553 RID: 13651 RVA: 0x0009DFF8 File Offset: 0x0009C1F8
			private void HandleOnGaveEmberDamage(Character attacker, Character target)
			{
				if (!this._canUse)
				{
					return;
				}
				if (target == null || target == this.owner || !MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				this.owner.GiveStatus(target, this.ability._status);
				this._canUse = false;
			}

			// Token: 0x04002AF2 RID: 10994
			private float _elapsed;

			// Token: 0x04002AF3 RID: 10995
			private bool _canUse = true;
		}
	}
}
