using System;
using Characters.Actions;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D43 RID: 3395
	[Serializable]
	public class DarkPaladinPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06004462 RID: 17506 RVA: 0x000C6BFC File Offset: 0x000C4DFC
		public int iconStacks
		{
			get
			{
				return (int)this._shieldInstance.amount;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06004463 RID: 17507 RVA: 0x000C6C0A File Offset: 0x000C4E0A
		// (set) Token: 0x06004464 RID: 17508 RVA: 0x000C6C12 File Offset: 0x000C4E12
		public Character owner { get; set; }

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06004465 RID: 17509 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x000C6C1B File Offset: 0x000C4E1B
		// (set) Token: 0x06004467 RID: 17511 RVA: 0x000C6C23 File Offset: 0x000C4E23
		public float remainTime { get; set; }

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06004469 RID: 17513 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x000C6C2C File Offset: 0x000C4E2C
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x0600446B RID: 17515 RVA: 0x000C6C41 File Offset: 0x000C4E41
		public bool expired
		{
			get
			{
				return this.remainTime <= 0f;
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x000C6C53 File Offset: 0x000C4E53
		public void Refresh()
		{
			this.remainTime = base.duration;
			this._shieldInstance.amount = (double)this._shieldAmount;
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x000C6C73 File Offset: 0x000C4E73
		private void OnShieldBroke()
		{
			this.owner.ability.Remove(this);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x000C6C87 File Offset: 0x000C4E87
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x000C6C91 File Offset: 0x000C4E91
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x000C6CA4 File Offset: 0x000C4EA4
		public void Attach()
		{
			this.remainTime = base.duration;
			this.owner.stat.AttachValues(this._stat);
			this._shieldInstance = this.owner.health.shield.Add(this.ability, (float)this._shieldAmount, new System.Action(this.OnShieldBroke));
			this._enhanceableComboAction.enhanced = true;
			if (this._enhanceableChainAction != null)
			{
				this._enhanceableChainAction.enhanced = true;
			}
			base.effectOnAttach.Spawn(this.owner.transform.position, 0f, 1f);
			this._loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x000C6D90 File Offset: 0x000C4F90
		public void Detach()
		{
			this.owner.stat.DetachValues(this._stat);
			if (this.owner.health.shield.Remove(this.ability))
			{
				this._shieldInstance = null;
			}
			this._enhanceableComboAction.enhanced = false;
			if (this._enhanceableChainAction != null)
			{
				this._enhanceableChainAction.enhanced = false;
			}
			if (this._loopEffectInstance != null)
			{
				this._loopEffectInstance.Stop();
				this._loopEffectInstance = null;
			}
		}

		// Token: 0x04003429 RID: 13353
		[SerializeField]
		private int _shieldAmount;

		// Token: 0x0400342A RID: 13354
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x0400342B RID: 13355
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x0400342C RID: 13356
		[Space]
		[SerializeField]
		private EnhanceableComboAction _enhanceableComboAction;

		// Token: 0x0400342D RID: 13357
		[SerializeField]
		private EnhanceableChainAction _enhanceableChainAction;

		// Token: 0x0400342E RID: 13358
		private EffectPoolInstance _loopEffectInstance;

		// Token: 0x0400342F RID: 13359
		private Shield.Instance _shieldInstance;
	}
}
