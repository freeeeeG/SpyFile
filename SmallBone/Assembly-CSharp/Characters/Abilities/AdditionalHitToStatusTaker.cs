using System;
using Characters.Operations;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009B5 RID: 2485
	[Serializable]
	public class AdditionalHitToStatusTaker : Ability
	{
		// Token: 0x06003518 RID: 13592 RVA: 0x0009D533 File Offset: 0x0009B733
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0009D546 File Offset: 0x0009B746
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AdditionalHitToStatusTaker.Instance(owner, this);
		}

		// Token: 0x04002AC7 RID: 10951
		[SerializeField]
		private CharacterStatusKindBoolArray _statuses;

		// Token: 0x04002AC8 RID: 10952
		[SerializeField]
		private float _additionalDamageAmount;

		// Token: 0x04002AC9 RID: 10953
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002ACA RID: 10954
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002ACB RID: 10955
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x04002ACC RID: 10956
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x020009B6 RID: 2486
		public class Instance : AbilityInstance<AdditionalHitToStatusTaker>
		{
			// Token: 0x0600351B RID: 13595 RVA: 0x0009D563 File Offset: 0x0009B763
			public Instance(Character owner, AdditionalHitToStatusTaker ability) : base(owner, ability)
			{
			}

			// Token: 0x0600351C RID: 13596 RVA: 0x0009D56D File Offset: 0x0009B76D
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(owner.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}

			// Token: 0x0600351D RID: 13597 RVA: 0x0009D596 File Offset: 0x0009B796
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(owner.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}

			// Token: 0x0600351E RID: 13598 RVA: 0x0009D5C0 File Offset: 0x0009B7C0
			private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
			{
				if (!result || !this.ability._statuses[applyInfo.kind])
				{
					return;
				}
				if (this.ability._targetPoint != null)
				{
					this.ability._targetPoint.position = target.transform.position;
				}
				Damage damage = this.owner.stat.GetDamage((double)this.ability._additionalDamageAmount, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target));
				this.owner.Attack(target, ref damage);
			}
		}
	}
}
