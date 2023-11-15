using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities
{
	// Token: 0x020009BB RID: 2491
	[Serializable]
	public sealed class ApplyStatusOnApplyStatus : Ability
	{
		// Token: 0x0600352B RID: 13611 RVA: 0x0009D7B4 File Offset: 0x0009B9B4
		public override void Initialize()
		{
			base.Initialize();
			this._onGiveStatus.Initialize();
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0009D7C7 File Offset: 0x0009B9C7
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ApplyStatusOnApplyStatus.Instance(owner, this);
		}

		// Token: 0x04002AD1 RID: 10961
		[SerializeField]
		[Tooltip("default는 0초")]
		private float _cooldownTime;

		// Token: 0x04002AD2 RID: 10962
		[SerializeField]
		[Range(1f, 100f)]
		private int _chance = 100;

		// Token: 0x04002AD3 RID: 10963
		[SerializeField]
		private float _delay;

		// Token: 0x04002AD4 RID: 10964
		[SerializeField]
		private CharacterStatus.Kind[] _kinds;

		// Token: 0x04002AD5 RID: 10965
		[SerializeField]
		private CharacterStatus.Timing _timing;

		// Token: 0x04002AD6 RID: 10966
		[Header("부여 할 것")]
		[Tooltip("부여 했던 것을 다시 부여할 경우 선택")]
		[SerializeField]
		private bool _self;

		// Token: 0x04002AD7 RID: 10967
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x04002AD8 RID: 10968
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002AD9 RID: 10969
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002ADA RID: 10970
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onGiveStatus;

		// Token: 0x020009BC RID: 2492
		public class Instance : AbilityInstance<ApplyStatusOnApplyStatus>
		{
			// Token: 0x17000B85 RID: 2949
			// (get) Token: 0x0600352E RID: 13614 RVA: 0x0009D7E0 File Offset: 0x0009B9E0
			public override float iconFillAmount
			{
				get
				{
					if (this.ability._cooldownTime != 0f)
					{
						return this._remainTime / this.ability._cooldownTime;
					}
					return 0f;
				}
			}

			// Token: 0x0600352F RID: 13615 RVA: 0x0009D80C File Offset: 0x0009BA0C
			internal Instance(Character owner, ApplyStatusOnApplyStatus ability) : base(owner, ability)
			{
				if (ability._self)
				{
					this._stun = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun);
					this._burn = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Burn);
					this._poison = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Poison);
					this._freeze = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Freeze);
					this._wound = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Wound);
				}
			}

			// Token: 0x06003530 RID: 13616 RVA: 0x0009D868 File Offset: 0x0009BA68
			protected override void OnAttach()
			{
				if (this.ability._self)
				{
					this.owner.status.Register(CharacterStatus.Kind.Stun, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyStun));
					this.owner.status.Register(CharacterStatus.Kind.Burn, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyBurn));
					this.owner.status.Register(CharacterStatus.Kind.Poison, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyPoison));
					this.owner.status.Register(CharacterStatus.Kind.Freeze, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyFreeze));
					this.owner.status.Register(CharacterStatus.Kind.Wound, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyWound));
					return;
				}
				foreach (CharacterStatus.Kind kind in this.ability._kinds)
				{
					this.owner.status.Register(kind, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.HandleOnTimeDelegate));
				}
			}

			// Token: 0x06003531 RID: 13617 RVA: 0x0009D994 File Offset: 0x0009BB94
			private bool Pass()
			{
				return this._remainTime <= 0f && MMMaths.PercentChance(this.ability._chance);
			}

			// Token: 0x06003532 RID: 13618 RVA: 0x0009D9BA File Offset: 0x0009BBBA
			private void ApplyStun(Character attacker, Character target)
			{
				if (!this.Pass())
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this._stun));
			}

			// Token: 0x06003533 RID: 13619 RVA: 0x0009D9EB File Offset: 0x0009BBEB
			private void ApplyBurn(Character attacker, Character target)
			{
				if (!this.Pass())
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this._burn));
			}

			// Token: 0x06003534 RID: 13620 RVA: 0x0009DA1C File Offset: 0x0009BC1C
			private void ApplyPoison(Character attacker, Character target)
			{
				if (!this.Pass())
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this._poison));
			}

			// Token: 0x06003535 RID: 13621 RVA: 0x0009DA4D File Offset: 0x0009BC4D
			private void ApplyFreeze(Character attacker, Character target)
			{
				if (!this.Pass())
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this._freeze));
			}

			// Token: 0x06003536 RID: 13622 RVA: 0x0009DA7E File Offset: 0x0009BC7E
			private void ApplyWound(Character attacker, Character target)
			{
				if (!this.Pass())
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this._wound));
			}

			// Token: 0x06003537 RID: 13623 RVA: 0x0009DAB0 File Offset: 0x0009BCB0
			private void HandleOnTimeDelegate(Character attacker, Character target)
			{
				if (this._remainTime > 0f)
				{
					return;
				}
				if (!MMMaths.PercentChance(this.ability._chance))
				{
					return;
				}
				if (this.ability._self)
				{
					return;
				}
				this._remainTime = this.ability._cooldownTime;
				attacker.StartCoroutine(this.CGiveStatus(attacker, target, this.ability._status));
			}

			// Token: 0x06003538 RID: 13624 RVA: 0x0009DB18 File Offset: 0x0009BD18
			protected override void OnDetach()
			{
				if (this.ability._self)
				{
					this.owner.status.Unregister(CharacterStatus.Kind.Stun, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyStun));
					this.owner.status.Unregister(CharacterStatus.Kind.Burn, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyBurn));
					this.owner.status.Unregister(CharacterStatus.Kind.Poison, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyPoison));
					this.owner.status.Unregister(CharacterStatus.Kind.Freeze, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyFreeze));
					this.owner.status.Unregister(CharacterStatus.Kind.Wound, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.ApplyWound));
					return;
				}
				foreach (CharacterStatus.Kind kind in this.ability._kinds)
				{
					this.owner.status.Unregister(kind, this.ability._timing, new CharacterStatus.OnTimeDelegate(this.HandleOnTimeDelegate));
				}
			}

			// Token: 0x06003539 RID: 13625 RVA: 0x0009DC44 File Offset: 0x0009BE44
			private IEnumerator CGiveStatus(Character giver, Character target, CharacterStatus.ApplyInfo info)
			{
				yield return Chronometer.global.WaitForSeconds(this.ability._delay);
				if (this.ability._targetPoint != null)
				{
					this.ability._positionInfo.Attach(target, this.ability._targetPoint);
				}
				giver.GiveStatus(target, info);
				giver.StartCoroutine(this.ability._onGiveStatus.CRun(target));
				yield break;
			}

			// Token: 0x0600353A RID: 13626 RVA: 0x0009DC68 File Offset: 0x0009BE68
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
			}

			// Token: 0x04002ADB RID: 10971
			private float _remainTime;

			// Token: 0x04002ADC RID: 10972
			private CharacterStatus.ApplyInfo _stun;

			// Token: 0x04002ADD RID: 10973
			private CharacterStatus.ApplyInfo _burn;

			// Token: 0x04002ADE RID: 10974
			private CharacterStatus.ApplyInfo _poison;

			// Token: 0x04002ADF RID: 10975
			private CharacterStatus.ApplyInfo _freeze;

			// Token: 0x04002AE0 RID: 10976
			private CharacterStatus.ApplyInfo _wound;
		}
	}
}
