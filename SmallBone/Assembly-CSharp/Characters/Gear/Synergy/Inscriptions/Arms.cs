using System;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000866 RID: 2150
	public class Arms : SimpleStatBonusKeyword
	{
		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002CDD RID: 11485 RVA: 0x00088DB4 File Offset: 0x00086FB4
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06002CDE RID: 11486 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06002CDF RID: 11487 RVA: 0x00088DC3 File Offset: 0x00086FC3
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.PhysicalAttackDamage;
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x00088DCA File Offset: 0x00086FCA
		protected override void Initialize()
		{
			base.Initialize();
			this._additionalHit.arms = this;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x00088DE0 File Offset: 0x00086FE0
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 2)
			{
				base.character.ability.Add(this._additionalHit.ability);
			}
			else
			{
				base.character.ability.Remove(this._additionalHit.ability);
			}
			if (this.keyword.isMaxStep)
			{
				this._additionalHit.enhanced = true;
				return;
			}
			this._additionalHit.enhanced = false;
		}

		// Token: 0x040025B2 RID: 9650
		[Header("2 세트 효과")]
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x040025B3 RID: 9651
		[Header("4 세트 효과")]
		[SerializeField]
		private Arms.AdditionalHit _additionalHit;

		// Token: 0x02000867 RID: 2151
		[Serializable]
		protected class AdditionalHit : IAbility, IAbilityInstance
		{
			// Token: 0x17000962 RID: 2402
			// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x00088E61 File Offset: 0x00087061
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x17000963 RID: 2403
			// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000964 RID: 2404
			// (get) Token: 0x06002CE5 RID: 11493 RVA: 0x00088E69 File Offset: 0x00087069
			// (set) Token: 0x06002CE6 RID: 11494 RVA: 0x00088E71 File Offset: 0x00087071
			public float remainTime { get; set; }

			// Token: 0x17000965 RID: 2405
			// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000966 RID: 2406
			// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x00088E7A File Offset: 0x0008707A
			public Sprite icon
			{
				get
				{
					if (this._remainCooldownTime <= 0f)
					{
						return null;
					}
					return this._icon;
				}
			}

			// Token: 0x17000967 RID: 2407
			// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x00088E91 File Offset: 0x00087091
			public float iconFillAmount
			{
				get
				{
					return this._remainCooldownTime / this._cooldownTime;
				}
			}

			// Token: 0x17000968 RID: 2408
			// (get) Token: 0x06002CEA RID: 11498 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000969 RID: 2409
			// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool iconFillFlipped
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700096A RID: 2410
			// (get) Token: 0x06002CEC RID: 11500 RVA: 0x00088EA0 File Offset: 0x000870A0
			public int iconStacks
			{
				get
				{
					if (!this.enhanced)
					{
						return 0;
					}
					return this._count;
				}
			}

			// Token: 0x1700096B RID: 2411
			// (get) Token: 0x06002CED RID: 11501 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700096C RID: 2412
			// (get) Token: 0x06002CEE RID: 11502 RVA: 0x00088EB2 File Offset: 0x000870B2
			// (set) Token: 0x06002CEF RID: 11503 RVA: 0x00088EBA File Offset: 0x000870BA
			public float duration { get; set; }

			// Token: 0x1700096D RID: 2413
			// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x1700096E RID: 2414
			// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700096F RID: 2415
			// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x00088EC3 File Offset: 0x000870C3
			// (set) Token: 0x06002CF3 RID: 11507 RVA: 0x00088ECB File Offset: 0x000870CB
			public bool enhanced { get; set; }

			// Token: 0x17000970 RID: 2416
			// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x00088ED4 File Offset: 0x000870D4
			// (set) Token: 0x06002CF5 RID: 11509 RVA: 0x00088EDC File Offset: 0x000870DC
			public Arms arms { get; set; }

			// Token: 0x06002CF6 RID: 11510 RVA: 0x00088EE5 File Offset: 0x000870E5
			public IAbilityInstance CreateInstance(Character owner)
			{
				this._owner = owner;
				this._additionalAttackOnGround.Initialize();
				this._additionalAttackOnAir.Initialize();
				this._additionalEnhancedAttackOnAir.Initialize();
				this._additionalEnhancedAttackOnGround.Initialize();
				return this;
			}

			// Token: 0x06002CF7 RID: 11511 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002CF8 RID: 11512 RVA: 0x00088F1B File Offset: 0x0008711B
			public void UpdateTime(float deltaTime)
			{
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06002CF9 RID: 11513 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002CFA RID: 11514 RVA: 0x00088F2B File Offset: 0x0008712B
			void IAbilityInstance.Attach()
			{
				this._count = 0;
				this._owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x06002CFB RID: 11515 RVA: 0x00088F4C File Offset: 0x0008714C
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.BasicAttack && action.type != Characters.Actions.Action.Type.JumpAttack)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				OperationInfo.Subcomponents subcomponents = this._owner.movement.isGrounded ? this._additionalAttackOnGround : this._additionalAttackOnAir;
				if (this.enhanced && this._count == this._cycle - 1)
				{
					subcomponents = (this._owner.movement.isGrounded ? this._additionalEnhancedAttackOnGround : this._additionalEnhancedAttackOnAir);
				}
				this._owner.StartCoroutine(subcomponents.CRun(this._owner));
				this._remainCooldownTime = this._cooldownTime;
				if (this.enhanced)
				{
					this._count++;
					if (this._count == this._cycle)
					{
						this._count = 0;
					}
				}
			}

			// Token: 0x06002CFC RID: 11516 RVA: 0x00089023 File Offset: 0x00087223
			void IAbilityInstance.Detach()
			{
				this._owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x040025B4 RID: 9652
			[Header("4세트 효과")]
			[SerializeField]
			private float _cooldownTime;

			// Token: 0x040025B5 RID: 9653
			[SerializeField]
			private MotionTypeBoolArray _motionType;

			// Token: 0x040025B6 RID: 9654
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _additionalAttackOnGround;

			// Token: 0x040025B7 RID: 9655
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _additionalAttackOnAir;

			// Token: 0x040025B8 RID: 9656
			[Header("6 세트 효과")]
			[Tooltip("cycle번째 마다 강화된 추가공격")]
			[SerializeField]
			private Sprite _icon;

			// Token: 0x040025B9 RID: 9657
			[SerializeField]
			private int _cycle;

			// Token: 0x040025BA RID: 9658
			[Subcomponent(typeof(OperationInfo))]
			[SerializeField]
			private OperationInfo.Subcomponents _additionalEnhancedAttackOnGround;

			// Token: 0x040025BB RID: 9659
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _additionalEnhancedAttackOnAir;

			// Token: 0x040025C0 RID: 9664
			private int _count;

			// Token: 0x040025C1 RID: 9665
			private Character _owner;

			// Token: 0x040025C2 RID: 9666
			private float _remainCooldownTime;
		}
	}
}
