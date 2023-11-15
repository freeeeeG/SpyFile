using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D61 RID: 3425
	[Serializable]
	public class LivingArmorPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x000C883A File Offset: 0x000C6A3A
		// (set) Token: 0x06004507 RID: 17671 RVA: 0x000C8842 File Offset: 0x000C6A42
		public Character owner { get; set; }

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06004508 RID: 17672 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06004509 RID: 17673 RVA: 0x000C884B File Offset: 0x000C6A4B
		// (set) Token: 0x0600450A RID: 17674 RVA: 0x000C8853 File Offset: 0x000C6A53
		public float remainTime { get; set; }

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x0600450C RID: 17676 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x0600450D RID: 17677 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600450F RID: 17679 RVA: 0x000C885C File Offset: 0x000C6A5C
		// (set) Token: 0x06004510 RID: 17680 RVA: 0x000C8864 File Offset: 0x000C6A64
		public bool expired { get; private set; }

		// Token: 0x06004511 RID: 17681 RVA: 0x000C8870 File Offset: 0x000C6A70
		public override void Initialize()
		{
			base.Initialize();
			this._attackOperations.Initialize();
			for (int i = 0; i < this._skills.Length; i++)
			{
				this._enhancedSkills[i].action.onStart += this.Expire;
			}
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x000C88C0 File Offset: 0x000C6AC0
		private void Expire()
		{
			if (this._gauge.isMax() || this._gauge.gaugePercent <= this._gauge.defaultBarGaugeColor.proportion)
			{
				this._gauge.Clear();
			}
			else
			{
				float num = this._gauge.maxValue * this._gauge.defaultBarGaugeColor.proportion;
				float num2 = this._gauge.maxValue * this._gauge.secondBarGaugeColor.proportion;
				float num3 = (this._gauge.currentValue - num) / num2;
				float value = this._gauge.maxValue * this._gauge.defaultBarGaugeColor.proportion * num3;
				this._gauge.Set(value);
			}
			this.expired = true;
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x000C8980 File Offset: 0x000C6B80
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x000C898C File Offset: 0x000C6B8C
		public void UpdateTime(float deltaTime)
		{
			this._attackOperationRemainTime -= deltaTime;
			if (this._attackOperationRemainTime < 0f)
			{
				this._attackOperationRemainTime += this._attackOperationInterval;
				this._attackOperationRunner.Stop();
				this._attackOperationRunner = this.owner.StartCoroutineWithReference(this._attackOperations.CRun(this.owner));
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x000C89F4 File Offset: 0x000C6BF4
		public void Attach()
		{
			this._attackOperationRemainTime = 0f;
			this.expired = false;
			this._weapon.AttachSkillChanges(this._skills, this._enhancedSkills, false);
			this._loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x000C8A67 File Offset: 0x000C6C67
		public void Detach()
		{
			this._weapon.DetachSkillChanges(this._skills, this._enhancedSkills, false);
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
				this._loopEffectInstance = null;
			}
		}

		// Token: 0x04003491 RID: 13457
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04003492 RID: 13458
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04003493 RID: 13459
		[SerializeField]
		private SkillInfo[] _skills;

		// Token: 0x04003494 RID: 13460
		[SerializeField]
		private SkillInfo[] _enhancedSkills;

		// Token: 0x04003495 RID: 13461
		[SerializeField]
		private float _attackOperationInterval;

		// Token: 0x04003496 RID: 13462
		private float _attackOperationRemainTime;

		// Token: 0x04003497 RID: 13463
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _attackOperations;

		// Token: 0x04003498 RID: 13464
		private CoroutineReference _attackOperationRunner;

		// Token: 0x04003499 RID: 13465
		private EffectPoolInstance _loopEffectInstance;
	}
}
