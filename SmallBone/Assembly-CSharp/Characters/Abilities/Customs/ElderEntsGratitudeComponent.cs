using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000DA5 RID: 3493
	public class ElderEntsGratitudeComponent : AbilityComponent<ElderEntsGratitude>, IStackable
	{
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x000CB452 File Offset: 0x000C9652
		// (set) Token: 0x06004655 RID: 18005 RVA: 0x000CB45A File Offset: 0x000C965A
		public float stack
		{
			get
			{
				return this.GetShieldAmount();
			}
			set
			{
				this.SetShieldAmount(value);
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x000CB463 File Offset: 0x000C9663
		// (set) Token: 0x06004657 RID: 18007 RVA: 0x000CB46B File Offset: 0x000C966B
		public float savedShieldAmount { get; set; }

		// Token: 0x06004658 RID: 18008 RVA: 0x000CB474 File Offset: 0x000C9674
		private void Awake()
		{
			this.savedShieldAmount = base.baseAbility.amount;
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x000CB487 File Offset: 0x000C9687
		public override void Initialize()
		{
			base.Initialize();
			base.baseAbility.component = this;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x000CB49B File Offset: 0x000C969B
		private float GetShieldAmount()
		{
			return base.baseAbility.GetShieldAmount();
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x000CB4A8 File Offset: 0x000C96A8
		private void SetShieldAmount(float amount)
		{
			this.savedShieldAmount = amount;
			base.baseAbility.SetShieldAmount(amount);
		}
	}
}
