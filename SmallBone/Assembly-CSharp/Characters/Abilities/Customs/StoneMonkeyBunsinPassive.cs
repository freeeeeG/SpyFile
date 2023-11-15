using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D98 RID: 3480
	[Serializable]
	public class StoneMonkeyBunsinPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x000CAB3E File Offset: 0x000C8D3E
		public int iconStacks
		{
			get
			{
				return (int)this._shieldInstance.amount;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x000CAB4C File Offset: 0x000C8D4C
		// (set) Token: 0x060045FC RID: 17916 RVA: 0x000CAB54 File Offset: 0x000C8D54
		public Character owner { get; set; }

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x000CAB5D File Offset: 0x000C8D5D
		// (set) Token: 0x060045FF RID: 17919 RVA: 0x000CAB65 File Offset: 0x000C8D65
		public float remainTime { get; set; }

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06004600 RID: 17920 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x000CAB6E File Offset: 0x000C8D6E
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x000CAB83 File Offset: 0x000C8D83
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x000CAB95 File Offset: 0x000C8D95
		public void Refresh()
		{
			this.remainTime = base.duration;
			this._shieldInstance.amount = (double)this._shieldAmount;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x000CABB5 File Offset: 0x000C8DB5
		private void OnShieldBroke()
		{
			this.owner.ability.Remove(this);
			this.owner.ability.Remove(this._onAddShieldAbility.ability);
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x000CABE5 File Offset: 0x000C8DE5
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			this._onAddShieldAbility.Initialize();
			return this;
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x000CABFA File Offset: 0x000C8DFA
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x000CAC0C File Offset: 0x000C8E0C
		public void Attach()
		{
			this.remainTime = base.duration;
			this.owner.stat.AttachValues(this._stat);
			this._shieldInstance = this.owner.health.shield.Add(this.ability, (float)this._shieldAmount, new Action(this.OnShieldBroke));
			this.owner.ability.Add(this._onAddShieldAbility.ability);
			base.effectOnAttach.Spawn(this.owner.transform.position, 0f, 1f);
			this._loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x000CACF0 File Offset: 0x000C8EF0
		public void Detach()
		{
			this.owner.stat.DetachValues(this._stat);
			if (this._shieldInstance.amount > 0.0 && this.owner.health.shield.Remove(this.ability))
			{
				this._shieldInstance = null;
			}
			this.owner.ability.Remove(this._onAddShieldAbility.ability);
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
				this._loopEffectInstance = null;
			}
		}

		// Token: 0x0400351E RID: 13598
		[SerializeField]
		private int _shieldAmount;

		// Token: 0x0400351F RID: 13599
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04003520 RID: 13600
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04003521 RID: 13601
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _onAddShieldAbility;

		// Token: 0x04003522 RID: 13602
		private EffectPoolInstance _loopEffectInstance;

		// Token: 0x04003523 RID: 13603
		private Shield.Instance _shieldInstance;
	}
}
