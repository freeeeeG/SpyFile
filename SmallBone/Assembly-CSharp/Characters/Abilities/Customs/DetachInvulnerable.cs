using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000DA0 RID: 3488
	[Serializable]
	public class DetachInvulnerable : Ability
	{
		// Token: 0x0600463C RID: 17980 RVA: 0x000CB19F File Offset: 0x000C939F
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DetachInvulnerable.Instance(owner, this);
		}

		// Token: 0x04003539 RID: 13625
		[SerializeField]
		private Transform _key;

		// Token: 0x02000DA1 RID: 3489
		public class Instance : AbilityInstance<DetachInvulnerable>
		{
			// Token: 0x0600463E RID: 17982 RVA: 0x000CB1A8 File Offset: 0x000C93A8
			public Instance(Character owner, DetachInvulnerable ability) : base(owner, ability)
			{
			}

			// Token: 0x0600463F RID: 17983 RVA: 0x000CB1B2 File Offset: 0x000C93B2
			protected override void OnAttach()
			{
				this.owner.invulnerable.Detach(this.ability._key);
			}

			// Token: 0x06004640 RID: 17984 RVA: 0x000CB1D0 File Offset: 0x000C93D0
			protected override void OnDetach()
			{
				this.owner.invulnerable.Attach(this.ability._key);
			}
		}
	}
}
