using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D8B RID: 3467
	[Serializable]
	public class Samurai2Passive : Ability, IAbilityInstance
	{
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x000CA58F File Offset: 0x000C878F
		// (set) Token: 0x060045CB RID: 17867 RVA: 0x000CA597 File Offset: 0x000C8797
		public Character owner { get; set; }

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x060045CD RID: 17869 RVA: 0x000CA5A0 File Offset: 0x000C87A0
		// (set) Token: 0x060045CE RID: 17870 RVA: 0x000CA5A8 File Offset: 0x000C87A8
		public float remainTime { get; set; }

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x060045CF RID: 17871 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x060045D1 RID: 17873 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x060045D3 RID: 17875 RVA: 0x000CA5B1 File Offset: 0x000C87B1
		// (set) Token: 0x060045D4 RID: 17876 RVA: 0x000CA5B9 File Offset: 0x000C87B9
		public bool expired { get; private set; }

		// Token: 0x060045D5 RID: 17877 RVA: 0x000CA5C4 File Offset: 0x000C87C4
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
			for (int i = 0; i < this._skills.Length; i++)
			{
				this._enhancedSkills[i].action.onStart += this.Expire;
			}
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x000CA613 File Offset: 0x000C8813
		private void Expire()
		{
			this._gauge.Clear();
			this.expired = true;
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x000CA627 File Offset: 0x000C8827
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x000CA634 File Offset: 0x000C8834
		public void UpdateTime(float deltaTime)
		{
			this._operationRemainTime -= deltaTime;
			if (this._operationRemainTime < 0f)
			{
				this._operationRemainTime += this._operationInterval;
				this._operationRunner.Stop();
				this._operationRunner = this.owner.StartCoroutineWithReference(this._operations.CRun(this.owner));
			}
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x000CA69C File Offset: 0x000C889C
		public void Attach()
		{
			this._operationRemainTime = 0f;
			this.expired = false;
			this._weapon.AttachSkillChanges(this._skills, this._enhancedSkills, false);
			this._loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x000CA70F File Offset: 0x000C890F
		public void Detach()
		{
			this._weapon.DetachSkillChanges(this._skills, this._enhancedSkills, false);
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
				this._loopEffectInstance = null;
			}
		}

		// Token: 0x04003503 RID: 13571
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04003504 RID: 13572
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04003505 RID: 13573
		[SerializeField]
		private SkillInfo[] _skills;

		// Token: 0x04003506 RID: 13574
		[SerializeField]
		private SkillInfo[] _enhancedSkills;

		// Token: 0x04003507 RID: 13575
		[Header("Operation")]
		[SerializeField]
		private float _operationInterval;

		// Token: 0x04003508 RID: 13576
		private float _operationRemainTime;

		// Token: 0x04003509 RID: 13577
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0400350A RID: 13578
		private CoroutineReference _operationRunner;

		// Token: 0x0400350B RID: 13579
		private EffectPoolInstance _loopEffectInstance;
	}
}
