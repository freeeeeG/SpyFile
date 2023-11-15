using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A82 RID: 2690
	[Serializable]
	public sealed class ModifyStatusDuration : Ability
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x000A4F3D File Offset: 0x000A313D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ModifyStatusDuration.Instance(owner, this);
		}

		// Token: 0x04002C93 RID: 11411
		[SerializeField]
		private CharacterStatus.Kind _kind;

		// Token: 0x04002C94 RID: 11412
		[SerializeField]
		private float _multiplier;

		// Token: 0x02000A83 RID: 2691
		public sealed class Instance : AbilityInstance<ModifyStatusDuration>
		{
			// Token: 0x060037E3 RID: 14307 RVA: 0x000A4F46 File Offset: 0x000A3146
			public Instance(Character owner, ModifyStatusDuration ability) : base(owner, ability)
			{
			}

			// Token: 0x060037E4 RID: 14308 RVA: 0x000A4F50 File Offset: 0x000A3150
			protected override void OnAttach()
			{
				this.owner.status.durationMultiplier[this.ability._kind].AddOrUpdate(this, this.ability._multiplier);
			}

			// Token: 0x060037E5 RID: 14309 RVA: 0x000A4F83 File Offset: 0x000A3183
			protected override void OnDetach()
			{
				this.owner.status.durationMultiplier[this.ability._kind].Remove(this);
			}
		}
	}
}
