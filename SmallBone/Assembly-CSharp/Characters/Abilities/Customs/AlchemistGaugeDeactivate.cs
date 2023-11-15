using System;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D1F RID: 3359
	[Serializable]
	public class AlchemistGaugeDeactivate : Ability
	{
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x060043BA RID: 17338 RVA: 0x000C529F File Offset: 0x000C349F
		public bool attached
		{
			get
			{
				AlchemistGaugeDeactivate.Instance instance = this._instance;
				return instance != null && instance.attached;
			}
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x000C52B4 File Offset: 0x000C34B4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new AlchemistGaugeDeactivate.Instance(owner, this);
		}

		// Token: 0x040033BA RID: 13242
		private AlchemistGaugeDeactivate.Instance _instance;

		// Token: 0x02000D20 RID: 3360
		public class Instance : AbilityInstance<AlchemistGaugeDeactivate>
		{
			// Token: 0x060043BD RID: 17341 RVA: 0x000C52D1 File Offset: 0x000C34D1
			public Instance(Character owner, AlchemistGaugeDeactivate ability) : base(owner, ability)
			{
			}

			// Token: 0x060043BE RID: 17342 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x060043BF RID: 17343 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
