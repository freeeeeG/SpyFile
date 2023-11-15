using System;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D4D RID: 3405
	[Serializable]
	public class FighterPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x000C72A9 File Offset: 0x000C54A9
		public bool buffAttached
		{
			get
			{
				return this._buffAttached;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x000C72B1 File Offset: 0x000C54B1
		// (set) Token: 0x06004498 RID: 17560 RVA: 0x000C72B9 File Offset: 0x000C54B9
		public Character owner { get; private set; }

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x000716FD File Offset: 0x0006F8FD
		IAbility IAbilityInstance.ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x000C72C2 File Offset: 0x000C54C2
		public Sprite icon
		{
			get
			{
				if (this._buffAttached)
				{
					return this._defaultIcon;
				}
				if (this.remainTime <= 0f)
				{
					return this._readyIcon;
				}
				return this._cooldownIcon;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x000C72ED File Offset: 0x000C54ED
		public float iconFillAmount
		{
			get
			{
				if (this._buffAttached)
				{
					return this.remainTime / base.duration;
				}
				if (this.remainTime > 0f)
				{
					return 1f - this.remainTime / this._cooldownTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x0600449C RID: 17564 RVA: 0x000C732B File Offset: 0x000C552B
		// (set) Token: 0x0600449D RID: 17565 RVA: 0x000C7333 File Offset: 0x000C5533
		public float remainTime { get; set; }

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x0600449E RID: 17566 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x0600449F RID: 17567 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x060044A0 RID: 17568 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060044A1 RID: 17569 RVA: 0x000C733C File Offset: 0x000C553C
		public bool rageReady
		{
			get
			{
				return !this._buffAttached && this.remainTime <= 0f;
			}
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x000C7358 File Offset: 0x000C5558
		~FighterPassive()
		{
			this._defaultIcon = null;
			this._cooldownIcon = null;
			this._readyIcon = null;
			this._rageIdleClip = null;
			this._rageWalkClip = null;
			this._rageJumpClip = null;
			this._rageFallClip = null;
			this._rageFallRepeatClip = null;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x000C73B8 File Offset: 0x000C55B8
		public override void Initialize()
		{
			base.Initialize();
			this._swapAttackAbility.Initialize();
			if (this._overrider == null)
			{
				this._overrider = new AnimationClipOverrider(this._rageBaseAnimator);
				this._overrider.Override("EmptyIdle", this._rageIdleClip);
				this._overrider.Override("EmptyWalk", this._rageWalkClip);
				this._overrider.Override("EmptyJumpUp", this._rageJumpClip);
				this._overrider.Override("EmptyJumpDown", this._rageFallClip);
				this._overrider.Override("EmptyJumpDownLoop", this._rageFallRepeatClip);
			}
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x000C745D File Offset: 0x000C565D
		public void Attach()
		{
			this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x000C7480 File Offset: 0x000C5680
		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if (damage.motionType != Damage.MotionType.Swap)
			{
				return false;
			}
			if (target == null)
			{
				return false;
			}
			if (target.character == null)
			{
				return false;
			}
			target.character.ability.Add(this._swapAttackAbility.ability);
			return false;
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x000C74BF File Offset: 0x000C56BF
		public void Detach()
		{
			this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
			this.DetachRage();
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x000C74E4 File Offset: 0x000C56E4
		public void AttachRage()
		{
			if (this._buffAttached || this.remainTime > 0f)
			{
				return;
			}
			this.remainTime = base.duration;
			this._buffAttached = true;
			this.owner.stat.AttachValues(this._stat);
			Chronometer.global.AttachTimeScale(this, this._timeScale);
			this.owner.chronometer.master.AttachTimeScale(this, 1f / this._timeScale);
			this._loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			base.effectOnAttach.Spawn(this.owner.transform.position, 0f, 1f);
			for (int i = 0; i < this._actions.Length; i++)
			{
				this._weapon.ChangeAction(this._actions[i], this._rageActions[i]);
				this._rageActions[i].cooldown.CopyCooldown(this._actions[i].cooldown);
			}
			this._weapon.AttachSkillChanges(this._skills, this._rageSkills, true);
			this._characterAnimation.AttachOverrider(this._overrider);
			this.owner.animationController.ForceUpdate();
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x000C764C File Offset: 0x000C584C
		private void DetachRage()
		{
			if (!this._buffAttached)
			{
				return;
			}
			this.remainTime = this._cooldownTime;
			this._buffAttached = false;
			this.owner.stat.DetachValues(this._stat);
			Chronometer.global.DetachTimeScale(this);
			this.owner.chronometer.master.DetachTimeScale(this);
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
				this._loopEffectInstance = null;
			}
			base.effectOnDetach.Spawn(this.owner.transform.position, 0f, 1f);
			for (int i = 0; i < this._actions.Length; i++)
			{
				this._weapon.ChangeAction(this._rageActions[i], this._actions[i]);
				this._actions[i].cooldown.CopyCooldown(this._rageActions[i].cooldown);
			}
			this._weapon.DetachSkillChanges(this._skills, this._rageSkills, true);
			this._characterAnimation.DetachOverrider(this._overrider);
			this.owner.animationController.ForceUpdate();
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x000C777C File Offset: 0x000C597C
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			if (this._buffAttached)
			{
				this._gauge.defaultBarGaugeColor.baseColor = this._buffBarColor;
				if (this.remainTime < 0f)
				{
					this.DetachRage();
					return;
				}
			}
			else
			{
				if (this.remainTime > 0f)
				{
					this._gauge.defaultBarGaugeColor.baseColor = this._defaultBarColor;
					return;
				}
				this.remainTime = 0f;
				this._gaugeAnimationTime += deltaTime * 2f;
				if (this._gaugeAnimationTime > 2f)
				{
					this._gaugeAnimationTime = 0f;
				}
				this._gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(this._defaultBarColor, this._buffBarColor, (this._gaugeAnimationTime < 1f) ? this._gaugeAnimationTime : (2f - this._gaugeAnimationTime));
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060044AB RID: 17579 RVA: 0x000C7869 File Offset: 0x000C5A69
		public float timeScale
		{
			get
			{
				return this._timeScale;
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x000C7871 File Offset: 0x000C5A71
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x04003444 RID: 13380
		[Header("교대기")]
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _swapAttackAbility;

		// Token: 0x04003445 RID: 13381
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04003446 RID: 13382
		[SerializeField]
		[Space]
		private SkillInfo[] _skills;

		// Token: 0x04003447 RID: 13383
		[SerializeField]
		private SkillInfo[] _rageSkills;

		// Token: 0x04003448 RID: 13384
		[Space]
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04003449 RID: 13385
		[SerializeField]
		private Characters.Actions.Action[] _rageActions;

		// Token: 0x0400344A RID: 13386
		[SerializeField]
		[Space]
		private CharacterAnimation _characterAnimation;

		// Token: 0x0400344B RID: 13387
		[SerializeField]
		private RuntimeAnimatorController _rageBaseAnimator;

		// Token: 0x0400344C RID: 13388
		[SerializeField]
		private AnimationClip _rageIdleClip;

		// Token: 0x0400344D RID: 13389
		[SerializeField]
		private AnimationClip _rageWalkClip;

		// Token: 0x0400344E RID: 13390
		[SerializeField]
		private AnimationClip _rageJumpClip;

		// Token: 0x0400344F RID: 13391
		[SerializeField]
		private AnimationClip _rageFallClip;

		// Token: 0x04003450 RID: 13392
		[SerializeField]
		private AnimationClip _rageFallRepeatClip;

		// Token: 0x04003451 RID: 13393
		private AnimationClipOverrider _overrider;

		// Token: 0x04003452 RID: 13394
		private EffectPoolInstance _loopEffectInstance;

		// Token: 0x04003453 RID: 13395
		private bool _buffAttached;

		// Token: 0x04003454 RID: 13396
		private float _gaugeAnimationTime;

		// Token: 0x04003457 RID: 13399
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04003458 RID: 13400
		[SerializeField]
		private Sprite _cooldownIcon;

		// Token: 0x04003459 RID: 13401
		[SerializeField]
		private Sprite _readyIcon;

		// Token: 0x0400345A RID: 13402
		[SerializeField]
		private Color _defaultBarColor;

		// Token: 0x0400345B RID: 13403
		[SerializeField]
		private Color _buffBarColor;

		// Token: 0x0400345C RID: 13404
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x0400345D RID: 13405
		[SerializeField]
		private float _timeScale;

		// Token: 0x0400345E RID: 13406
		[SerializeField]
		private Stat.Values _stat;
	}
}
