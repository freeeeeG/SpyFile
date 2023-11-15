using System;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200091F RID: 2335
	public class AttackHitTriggerAction : Action
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x00094F9F File Offset: 0x0009319F
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._attackMotion);
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x00094FC9 File Offset: 0x000931C9
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._attackMotion,
					this._secondMotion
				};
			}
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x00094FE4 File Offset: 0x000931E4
		protected override void Awake()
		{
			base.Awake();
			this._attack = this._attackMotion.GetComponentInChildren<IAttack>();
			if (this._attack == null)
			{
				Debug.LogError("Attack is null " + base.gameObject.name);
				return;
			}
			this._attack.onHit += this.OnAttackHit;
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x00095042 File Offset: 0x00093242
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._attackMotion.Initialize(this);
			this._secondMotion.Initialize(this);
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x00095063 File Offset: 0x00093263
		private void OnDestroy()
		{
			this._attack.onHit -= this.OnAttackHit;
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0009507C File Offset: 0x0009327C
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._attackMotion);
			return true;
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x0009509D File Offset: 0x0009329D
		private void OnAttackHit(Target target, ref Damage damage)
		{
			this._attackMotion.EndBehaviour();
			base.DoMotion(this._secondMotion);
		}

		// Token: 0x04002905 RID: 10501
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _attackMotion;

		// Token: 0x04002906 RID: 10502
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _secondMotion;

		// Token: 0x04002907 RID: 10503
		private IAttack _attack;
	}
}
