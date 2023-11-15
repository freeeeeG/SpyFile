using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009D5 RID: 2517
	[Serializable]
	public sealed class AttachToOneTargetOnGaveDamage : Ability, IAbilityInstance
	{
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x0009EE0B File Offset: 0x0009D00B
		// (set) Token: 0x06003578 RID: 13688 RVA: 0x0009EE13 File Offset: 0x0009D013
		public Character owner { get; private set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x000716FD File Offset: 0x0006F8FD
		IAbility IAbilityInstance.ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x00018C86 File Offset: 0x00016E86
		public Sprite icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x0009EE1C File Offset: 0x0009D01C
		// (set) Token: 0x0600357D RID: 13693 RVA: 0x0009EE24 File Offset: 0x0009D024
		public float remainTime { get; set; }

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600357E RID: 13694 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003580 RID: 13696 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0009EE30 File Offset: 0x0009D030
		public void Attach()
		{
			if (this._onGiveDamage)
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.TryAttachAbility));
				return;
			}
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.TryAttachAbility));
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x0009EE90 File Offset: 0x0009D090
		private bool TryAttachAbility(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (character == null)
			{
				return false;
			}
			if (character == this.owner)
			{
				return false;
			}
			if (this._lastTarget == null)
			{
				this._lastTarget = character;
			}
			if (this._lastTarget.health.dead || !this._lastTarget.gameObject.activeInHierarchy)
			{
				this._lastTarget = null;
				return false;
			}
			if (this._lastTarget != character)
			{
				return false;
			}
			damage.percentMultiplier *= (double)this._damagePercent;
			damage.multiplier += (double)this._damagePercentPoint;
			damage.criticalChance += (double)this._extraCriticalChance;
			damage.criticalDamageMultiplier += (double)this._extraCriticalDamageMultiplier;
			this._lastTarget.ability.Add(this._abilityComponent.ability);
			return false;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x0009EF74 File Offset: 0x0009D174
		private void TryAttachAbility(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			Character character = target.character;
			if (character == null)
			{
				return;
			}
			if (this._lastTarget == null)
			{
				this._lastTarget = character;
			}
			if (this._lastTarget.health.dead || !this._lastTarget.gameObject.activeInHierarchy)
			{
				this._lastTarget = null;
				return;
			}
			if (this._lastTarget != character)
			{
				return;
			}
			this._lastTarget.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x0009EFFE File Offset: 0x0009D1FE
		public override void Initialize()
		{
			base.Initialize();
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x0009F011 File Offset: 0x0009D211
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x0009F01C File Offset: 0x0009D21C
		public void Detach()
		{
			if (this._onGiveDamage)
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.TryAttachAbility));
			}
			else
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.TryAttachAbility));
			}
			if (this._lastTarget != null && !this._lastTarget.health.dead)
			{
				this._lastTarget.ability.Remove(this._abilityComponent.ability);
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x04002B1B RID: 11035
		[Header("데미지 변경")]
		[SerializeField]
		private bool _onGiveDamage;

		// Token: 0x04002B1C RID: 11036
		[SerializeField]
		private float _damagePercent = 1f;

		// Token: 0x04002B1D RID: 11037
		[SerializeField]
		private float _damagePercentPoint;

		// Token: 0x04002B1E RID: 11038
		[SerializeField]
		private float _extraCriticalChance;

		// Token: 0x04002B1F RID: 11039
		[SerializeField]
		private float _extraCriticalDamageMultiplier;

		// Token: 0x04002B20 RID: 11040
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B21 RID: 11041
		private Character _lastTarget;
	}
}
