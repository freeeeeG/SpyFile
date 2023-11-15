using System;
using System.Collections;
using Characters.Actions.Constraints;
using Characters.Controllers;
using Characters.Cooldowns;
using Characters.Operations;
using Data;
using InControl;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200091A RID: 2330
	public abstract class Action : MonoBehaviour
	{
		// Token: 0x1400008B RID: 139
		// (add) Token: 0x060031FD RID: 12797 RVA: 0x000947B4 File Offset: 0x000929B4
		// (remove) Token: 0x060031FE RID: 12798 RVA: 0x000947CD File Offset: 0x000929CD
		public event Action onStart
		{
			add
			{
				this._onStart = (Action)Delegate.Combine(this._onStart, value);
			}
			remove
			{
				this._onStart = (Action)Delegate.Remove(this._onStart, value);
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060031FF RID: 12799 RVA: 0x000947E6 File Offset: 0x000929E6
		// (remove) Token: 0x06003200 RID: 12800 RVA: 0x000947FF File Offset: 0x000929FF
		public event Action onEnd
		{
			add
			{
				this._onEnd = (Action)Delegate.Combine(this._onEnd, value);
			}
			remove
			{
				this._onEnd = (Action)Delegate.Remove(this._onEnd, value);
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x00094818 File Offset: 0x00092A18
		public Character owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x00094820 File Offset: 0x00092A20
		public int priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x00094828 File Offset: 0x00092A28
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x0009483A File Offset: 0x00092A3A
		public Button button
		{
			get
			{
				return Button.values[this._button];
			}
			set
			{
				this._button = value.index;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x00094848 File Offset: 0x00092A48
		protected PlayerAction defaultButton
		{
			get
			{
				return this._input[this._button];
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x0009485B File Offset: 0x00092A5B
		internal OperationInfo[] operations
		{
			get
			{
				return this._operations.components;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x00094868 File Offset: 0x00092A68
		public bool running
		{
			get
			{
				return this._owner.runningMotion != null && this._owner.runningMotion.action == this;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003208 RID: 12808
		public abstract bool canUse { get; }

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x00094895 File Offset: 0x00092A95
		public Action.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x0009489D File Offset: 0x00092A9D
		// (set) Token: 0x0600320B RID: 12811 RVA: 0x000948A5 File Offset: 0x00092AA5
		public float extraSpeedMultiplier { get; set; } = 1f;

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600320C RID: 12812
		public abstract Motion[] motions { get; }

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x000948AE File Offset: 0x00092AAE
		public CooldownSerializer cooldown
		{
			get
			{
				return this._cooldown;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x000948B6 File Offset: 0x00092AB6
		protected bool consumeCooldownManually
		{
			get
			{
				return this._triggerStartManually;
			}
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000948C0 File Offset: 0x00092AC0
		private void OnDestroy()
		{
			if (this._cancelOnGround)
			{
				this._owner.movement.onGrounded -= this.OnGrounded;
			}
			this._owner = null;
			this._onStart = null;
			this._onEnd = null;
			this._onCancel = null;
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0009490D File Offset: 0x00092B0D
		protected virtual void Awake()
		{
			Array.Sort<OperationInfo>(this._operations.components, (OperationInfo x, OperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x00094940 File Offset: 0x00092B40
		public virtual void Initialize(Character owner)
		{
			this._cooldown.Serialize();
			if (this._cooldown.type == CooldownSerializer.Type.Time)
			{
				Action.Type type = this._type;
				if (type != Action.Type.Dash)
				{
					if (type == Action.Type.Skill)
					{
						this._cooldown.time.GetCooldownSpeed = new Func<float>(owner.stat.GetSkillCooldownSpeed);
					}
				}
				else
				{
					this._cooldown.time.GetCooldownSpeed = new Func<float>(owner.stat.GetDashCooldownSpeed);
				}
			}
			this._owner = owner;
			this._input = this._owner.GetComponent<PlayerInput>();
			for (int i = 0; i < this._constraints.components.Length; i++)
			{
				this._constraints.components[i].Initilaize(this);
			}
			if (this._cancelOnGround)
			{
				this._owner.movement.onGrounded -= this.OnGrounded;
				this._owner.movement.onGrounded += this.OnGrounded;
			}
			if (this._cooldown.streak.count > 0)
			{
				this._owner.playerComponents.inventory.weapon.onSwap -= this._cooldown.streak.Expire;
				this._owner.playerComponents.inventory.weapon.onSwap += this._cooldown.streak.Expire;
			}
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x00094AB2 File Offset: 0x00092CB2
		private void OnGrounded()
		{
			if (this._owner.runningMotion != null && this._owner.runningMotion.action == this)
			{
				this._owner.CancelAction();
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x00094AEC File Offset: 0x00092CEC
		public virtual bool Process()
		{
			if (!base.gameObject.activeInHierarchy || this.defaultButton == null)
			{
				return false;
			}
			if (this._needForceEnd)
			{
				this.ForceEndProcess();
				return false;
			}
			if (this.type == Action.Type.Skill && this._owner.silence.value)
			{
				return false;
			}
			if (GameData.Settings.arrowDashEnabled && this.type == Action.Type.Dash && (this._input.left.IsDoublePressed || this._input.right.IsDoublePressed) && this.TryStart())
			{
				return true;
			}
			if ((this._inputMethod == Action.InputMethod.TryStartIsPressed && this.defaultButton.IsPressed) || (this._inputMethod == Action.InputMethod.TryStartWasPressed && this.defaultButton.WasPressed) || (this._inputMethod == Action.InputMethod.TryStartWasReleased && this.defaultButton.WasReleased))
			{
				return this.TryStart();
			}
			return this._owner.motion != null && this._owner.motion.action == this && (this._inputMethod == Action.InputMethod.TryStartIsPressed || this._inputMethod == Action.InputMethod.TryStartWasPressed) && this.defaultButton.WasReleased && this.TryEnd();
		}

		// Token: 0x06003214 RID: 12820
		public abstract bool TryStart();

		// Token: 0x06003215 RID: 12821 RVA: 0x00094C13 File Offset: 0x00092E13
		public virtual bool TryEnd()
		{
			this._needForceEnd = false;
			return false;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x00094C1D File Offset: 0x00092E1D
		public void ForceEnd()
		{
			this._needForceEnd = true;
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x00094C28 File Offset: 0x00092E28
		protected virtual void ForceEndProcess()
		{
			this._needForceEnd = false;
			foreach (Motion obj in this.motions)
			{
				if (this.owner.motion.Equals(obj))
				{
					this.owner.CancelAction();
					this._owner.motion.action = null;
					return;
				}
			}
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x00094C88 File Offset: 0x00092E88
		protected float GetSpeedMultiplier(Motion motion)
		{
			float num = 1f;
			switch (motion.speedMultiplierSource)
			{
			case Motion.SpeedMultiplierSource.Default:
			{
				Action.Type type = this.type;
				if (type - Action.Type.BasicAttack > 1)
				{
					if (type == Action.Type.Skill)
					{
						num = this._owner.stat.GetInterpolatedSkillAttackSpeed();
					}
				}
				else
				{
					num = this._owner.stat.GetInterpolatedBasicAttackSpeed();
				}
				break;
			}
			case Motion.SpeedMultiplierSource.ForceBasic:
				num = this._owner.stat.GetInterpolatedBasicAttackSpeed();
				break;
			case Motion.SpeedMultiplierSource.ForceSkill:
				num = this._owner.stat.GetInterpolatedSkillAttackSpeed();
				break;
			case Motion.SpeedMultiplierSource.ForceMovement:
				num = this._owner.stat.GetInterpolatedMovementSpeed();
				break;
			case Motion.SpeedMultiplierSource.ForceCharging:
				num = this._owner.stat.GetInterpolatedChargingSpeed();
				break;
			case Motion.SpeedMultiplierSource.ForceBasicAndCharging:
				num = this._owner.stat.GetInterpolatedBasicAttackChargingSpeed();
				break;
			case Motion.SpeedMultiplierSource.ForceSkillAndCharging:
				num = this._owner.stat.GetInterpolatedSkillAttackChargingSpeed();
				break;
			}
			return this.extraSpeedMultiplier * ((num - 1f) * motion.speedMultiplierFactor + 1f);
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x00094D94 File Offset: 0x00092F94
		protected void DoAction(Motion motion)
		{
			motion.action = this;
			this._owner.DoAction(motion, this.GetSpeedMultiplier(motion), !this._triggerStartManually);
			motion.ConsumeConstraints();
			if (this._triggerStartManually)
			{
				return;
			}
			Action onStart = this._onStart;
			if (onStart == null)
			{
				return;
			}
			onStart();
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x00094DE3 File Offset: 0x00092FE3
		protected void DoActionNonBlock(Motion motion)
		{
			motion.action = this;
			this._owner.DoActionNonBlock(motion);
			motion.ConsumeConstraints();
			Action onStart = this._onStart;
			if (onStart == null)
			{
				return;
			}
			onStart();
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x00094E0E File Offset: 0x0009300E
		protected void DoMotion(Motion motion)
		{
			motion.action = this;
			this._owner.DoMotion(motion, this.GetSpeedMultiplier(motion));
			motion.ConsumeConstraints();
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x00094E30 File Offset: 0x00093030
		public IEnumerator CWaitForEndOfRunning()
		{
			if (this._owner.runningMotion == null || this._owner.runningMotion.action != this)
			{
				yield break;
			}
			yield return this._owner.runningMotion.CWaitForEndOfRunning();
			yield break;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x00094E3F File Offset: 0x0009303F
		public void TriggerStartManually()
		{
			this._owner.TriggerOnStartActionManually(this);
			this._cooldown.Consume();
			Action onStart = this._onStart;
			if (onStart == null)
			{
				return;
			}
			onStart();
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x00094E69 File Offset: 0x00093069
		internal bool PassAllConstraints(Motion motion)
		{
			return this._constraints.components.Pass() && motion.PassConstraints();
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x00094E85 File Offset: 0x00093085
		internal bool PassConstraints(Motion motion)
		{
			return this._constraints.components.Pass();
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x00094E97 File Offset: 0x00093097
		internal void ConsumeConstraints()
		{
			this._constraints.components.Consume();
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x00094EA9 File Offset: 0x000930A9
		protected bool ConsumeCooldownIfNeeded()
		{
			return this._triggerStartManually || this._cooldown.Consume();
		}

		// Token: 0x040028E3 RID: 10467
		protected Action _onStart;

		// Token: 0x040028E4 RID: 10468
		protected Action _onEnd;

		// Token: 0x040028E5 RID: 10469
		protected Action _onCancel;

		// Token: 0x040028E6 RID: 10470
		protected Character _owner;

		// Token: 0x040028E7 RID: 10471
		protected PlayerInput _input;

		// Token: 0x040028E8 RID: 10472
		protected bool _needForceEnd;

		// Token: 0x040028E9 RID: 10473
		[Tooltip("숫자가 작을수록 우선순위가 높음")]
		[SerializeField]
		private int _priority;

		// Token: 0x040028EA RID: 10474
		[SerializeField]
		private Action.Type _type = Action.Type.Skill;

		// Token: 0x040028EB RID: 10475
		[SerializeField]
		[Button.StringPopupAttribute]
		private int _button;

		// Token: 0x040028EC RID: 10476
		[SerializeField]
		protected Action.InputMethod _inputMethod;

		// Token: 0x040028ED RID: 10477
		[SerializeField]
		private bool _cancelOnGround;

		// Token: 0x040028EE RID: 10478
		[SerializeField]
		[Tooltip("액션의 시작 구간을 수동으로 설정합니다. 액션이 시작될 때 관련 사용효과가 발동되고 쿨다운이 감소합니다.")]
		private bool _triggerStartManually;

		// Token: 0x040028EF RID: 10479
		[SerializeField]
		private CooldownSerializer _cooldown;

		// Token: 0x040028F0 RID: 10480
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		protected Constraint.Subcomponents _constraints;

		// Token: 0x040028F1 RID: 10481
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0200091B RID: 2331
		public enum Type
		{
			// Token: 0x040028F4 RID: 10484
			Dash,
			// Token: 0x040028F5 RID: 10485
			BasicAttack,
			// Token: 0x040028F6 RID: 10486
			JumpAttack,
			// Token: 0x040028F7 RID: 10487
			Jump,
			// Token: 0x040028F8 RID: 10488
			Skill,
			// Token: 0x040028F9 RID: 10489
			Swap,
			// Token: 0x040028FA RID: 10490
			Custom
		}

		// Token: 0x0200091C RID: 2332
		protected enum InputMethod
		{
			// Token: 0x040028FC RID: 10492
			TryStartIsPressed,
			// Token: 0x040028FD RID: 10493
			TryStartWasPressed,
			// Token: 0x040028FE RID: 10494
			TryStartWasReleased,
			// Token: 0x040028FF RID: 10495
			NotUsed
		}
	}
}
