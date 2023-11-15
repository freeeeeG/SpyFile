using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000C99 RID: 3225
	public class BloodEatingSwordComponent : AbilityComponent<BloodEatingSword>, IStackable
	{
		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x000BECB3 File Offset: 0x000BCEB3
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x000BECBB File Offset: 0x000BCEBB
		public int currentStack { get; set; }

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x000BECC4 File Offset: 0x000BCEC4
		// (set) Token: 0x0600419E RID: 16798 RVA: 0x000BECCD File Offset: 0x000BCECD
		public float stack
		{
			get
			{
				return (float)this.currentStack;
			}
			set
			{
				this.currentStack = (int)value;
			}
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x000BECD7 File Offset: 0x000BCED7
		public override void Initialize()
		{
			base.Initialize();
			base.baseAbility.component = this;
		}
	}
}
