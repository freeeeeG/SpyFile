using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009B2 RID: 2482
	[Serializable]
	public class AdditionalHitOnStatusTrigger : Ability
	{
		// Token: 0x06003510 RID: 13584 RVA: 0x0009D2F7 File Offset: 0x0009B4F7
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x0009D30A File Offset: 0x0009B50A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AdditionalHitOnStatusTrigger.Instance(owner, this);
		}

		// Token: 0x04002ABF RID: 10943
		[SerializeField]
		private CharacterStatus.Timing _timing;

		// Token: 0x04002AC0 RID: 10944
		[SerializeField]
		private CharacterStatusKindBoolArray _statuses;

		// Token: 0x04002AC1 RID: 10945
		[SerializeField]
		private CustomFloat _additionalDamageAmount;

		// Token: 0x04002AC2 RID: 10946
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04002AC3 RID: 10947
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002AC4 RID: 10948
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002AC5 RID: 10949
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x020009B3 RID: 2483
		public class Instance : AbilityInstance<AdditionalHitOnStatusTrigger>
		{
			// Token: 0x06003513 RID: 13587 RVA: 0x0009D327 File Offset: 0x0009B527
			public Instance(Character owner, AdditionalHitOnStatusTrigger ability) : base(owner, ability)
			{
				this._handle = new CharacterStatus.OnTimeDelegate(this.HandleOnTime);
			}

			// Token: 0x06003514 RID: 13588 RVA: 0x0009D344 File Offset: 0x0009B544
			protected override void OnAttach()
			{
				foreach (object obj in Enum.GetValues(typeof(CharacterStatus.Kind)))
				{
					CharacterStatus.Kind kind = (CharacterStatus.Kind)obj;
					if (this.ability._statuses[kind])
					{
						this.owner.status.Register(kind, this.ability._timing, this._handle);
					}
				}
			}

			// Token: 0x06003515 RID: 13589 RVA: 0x0009D3D4 File Offset: 0x0009B5D4
			protected override void OnDetach()
			{
				foreach (object obj in Enum.GetValues(typeof(CharacterStatus.Kind)))
				{
					CharacterStatus.Kind kind = (CharacterStatus.Kind)obj;
					if (this.ability._statuses[kind])
					{
						this.owner.status.Unregister(kind, this.ability._timing, this._handle);
					}
				}
			}

			// Token: 0x06003516 RID: 13590 RVA: 0x0009D464 File Offset: 0x0009B664
			private void HandleOnTime(Character attacker, Character target)
			{
				if (this.ability._targetPoint != null)
				{
					this.ability._targetPoint.position = target.transform.position;
				}
				if (this.ability._adaptiveForce)
				{
					this.ability._additionalHit.ChangeAdaptiveDamageAttribute(attacker);
				}
				Damage damage = this.owner.stat.GetDamage((double)this.ability._additionalDamageAmount.value, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target));
				this.owner.Attack(target, ref damage);
			}

			// Token: 0x04002AC6 RID: 10950
			private CharacterStatus.OnTimeDelegate _handle;
		}
	}
}
