using System;
using Characters.Gear.Weapons;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper
{
	// Token: 0x02000C0C RID: 3084
	[Serializable]
	public class GrimReaperPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x000B7A04 File Offset: 0x000B5C04
		// (set) Token: 0x06003F54 RID: 16212 RVA: 0x000B7A0C File Offset: 0x000B5C0C
		public Character owner { get; set; }

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003F55 RID: 16213 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003F56 RID: 16214 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x06003F57 RID: 16215 RVA: 0x00002191 File Offset: 0x00000391
		public float remainTime
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003F59 RID: 16217 RVA: 0x000B7A15 File Offset: 0x000B5C15
		public Sprite icon
		{
			get
			{
				if (this.stack <= 0 || !(this._currentWeapon == this._weapon))
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x000B7A3B File Offset: 0x000B5C3B
		public int iconStacks
		{
			get
			{
				return this.stack;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x000B7A43 File Offset: 0x000B5C43
		// (set) Token: 0x06003F5E RID: 16222 RVA: 0x000B7A4B File Offset: 0x000B5C4B
		public int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = value;
				this.UpdateStat();
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x000B7A5C File Offset: 0x000B5C5C
		public void Attach()
		{
			this._currentWeapon = this.owner.playerComponents.inventory.weapon.current;
			this.owner.playerComponents.inventory.weapon.onSwap += this.UpdateCurreuntWeapon;
			this.owner.playerComponents.inventory.weapon.onChanged += this.Weapon_onChanged;
			this._stat = this._statPerStack.Clone();
			this.owner.stat.AttachOrUpdateValues(this._stat);
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnKilledEnemy));
			this.SetEnhanceSkill();
			this.UpdateStat();
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x000B7B2F File Offset: 0x000B5D2F
		private void Weapon_onChanged(Weapon old, Weapon @new)
		{
			this.UpdateCurreuntWeapon();
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x000B7B38 File Offset: 0x000B5D38
		private void UpdateCurreuntWeapon()
		{
			this._currentWeapon = this.owner.playerComponents.inventory.weapon.current;
			if (this._currentWeapon != this._weapon)
			{
				this.owner.stat.DetachValues(this._stat);
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnKilledEnemy));
				return;
			}
			this.owner.stat.AttachOrUpdateValues(this._stat);
			Character owner2 = this.owner;
			owner2.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner2.onKilled, new Character.OnKilledDelegate(this.OnKilledEnemy));
			Character owner3 = this.owner;
			owner3.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner3.onKilled, new Character.OnKilledDelegate(this.OnKilledEnemy));
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x000B7C1A File Offset: 0x000B5E1A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			this._onEnhancedSkill.Initialize();
			this._onEnhancedSkill2.Initialize();
			return this;
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x000B7C3C File Offset: 0x000B5E3C
		public void Detach()
		{
			this.owner.playerComponents.inventory.weapon.onSwap -= this.UpdateCurreuntWeapon;
			this.owner.playerComponents.inventory.weapon.onChanged -= this.Weapon_onChanged;
			this.owner.stat.DetachValues(this._stat);
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnKilledEnemy));
			if (this.stack >= this._enhanced2StackPoint)
			{
				this._weapon.DetachSkillChanges(this._skills, this._enhanced2Skills, false);
				return;
			}
			if (this.stack >= this._enhancedStackPoint)
			{
				this._weapon.DetachSkillChanges(this._skills, this._enhancedSkills, false);
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x000B7D20 File Offset: 0x000B5F20
		private void OnKilledEnemy(ITarget target, ref Damage damage)
		{
			if (target.character == null)
			{
				return;
			}
			if (!this._characterType[target.character.type])
			{
				return;
			}
			if (!MMMaths.PercentChance(this._possibility))
			{
				return;
			}
			this._grimReaperSoul.Spawn(target.character.collider.bounds.center, this);
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x000B7D88 File Offset: 0x000B5F88
		public void AddStack()
		{
			int stack = this.stack;
			this.stack = stack + 1;
			this.TryToEnhanceSkill();
			this.UpdateStat();
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x000B7DB4 File Offset: 0x000B5FB4
		private void TryToEnhanceSkill()
		{
			if (this.stack == this._enhancedStackPoint)
			{
				this._weapon.AttachSkillChanges(this._skills, this._enhancedSkills, false);
				this.owner.StartCoroutine(this._onEnhancedSkill.CRun(this.owner));
				return;
			}
			if (this.stack == this._enhanced2StackPoint)
			{
				this._weapon.DetachSkillChanges(this._skills, this._enhancedSkills, false);
				this._weapon.AttachSkillChanges(this._skills, this._enhanced2Skills, false);
				this.owner.StartCoroutine(this._onEnhancedSkill2.CRun(this.owner));
			}
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x000B7E60 File Offset: 0x000B6060
		public void SetEnhanceSkill()
		{
			if (this.stack >= this._enhancedStackPoint && this.stack < this._enhanced2StackPoint)
			{
				this._weapon.AttachSkillChanges(this._skills, this._enhancedSkills, false);
				return;
			}
			if (this.stack >= this._enhanced2StackPoint)
			{
				this._weapon.AttachSkillChanges(this._skills, this._enhanced2Skills, false);
			}
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x000B7EC8 File Offset: 0x000B60C8
		private void UpdateStat()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)this.stack);
			}
			this.owner.stat.SetNeedUpdate();
		}

		// Token: 0x040030BC RID: 12476
		[SerializeField]
		[Header("스킬 강화")]
		private Weapon _weapon;

		// Token: 0x040030BD RID: 12477
		[SerializeField]
		private SkillInfo[] _skills;

		// Token: 0x040030BE RID: 12478
		[SerializeField]
		private int _enhancedStackPoint;

		// Token: 0x040030BF RID: 12479
		[SerializeField]
		private SkillInfo[] _enhancedSkills;

		// Token: 0x040030C0 RID: 12480
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onEnhancedSkill;

		// Token: 0x040030C1 RID: 12481
		[SerializeField]
		private int _enhanced2StackPoint;

		// Token: 0x040030C2 RID: 12482
		[SerializeField]
		private SkillInfo[] _enhanced2Skills;

		// Token: 0x040030C3 RID: 12483
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onEnhancedSkill2;

		// Token: 0x040030C4 RID: 12484
		[Header("영혼 생성")]
		[SerializeField]
		private GrimReaperSoul _grimReaperSoul;

		// Token: 0x040030C5 RID: 12485
		[SerializeField]
		[Range(1f, 100f)]
		private int _possibility;

		// Token: 0x040030C6 RID: 12486
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x040030C7 RID: 12487
		[SerializeField]
		[Header("영혼 스텟")]
		private Stat.Values _statPerStack;

		// Token: 0x040030C8 RID: 12488
		private Stat.Values _stat;

		// Token: 0x040030C9 RID: 12489
		private int _stack;

		// Token: 0x040030CA RID: 12490
		private Weapon _currentWeapon;
	}
}
