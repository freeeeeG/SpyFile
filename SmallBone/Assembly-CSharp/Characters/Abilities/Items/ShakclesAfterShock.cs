using System;
using Characters.Actions;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CF5 RID: 3317
	[Serializable]
	public sealed class ShakclesAfterShock : Ability
	{
		// Token: 0x06004307 RID: 17159 RVA: 0x000C3737 File Offset: 0x000C1937
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ShakclesAfterShock.Instance(owner, this);
		}

		// Token: 0x02000CF6 RID: 3318
		public class Instance : AbilityInstance<ShakclesAfterShock>
		{
			// Token: 0x06004309 RID: 17161 RVA: 0x000C3740 File Offset: 0x000C1940
			public Instance(Character owner, ShakclesAfterShock ability) : base(owner, ability)
			{
			}

			// Token: 0x0600430A RID: 17162 RVA: 0x000C374C File Offset: 0x000C194C
			protected override void OnAttach()
			{
				foreach (Characters.Actions.Action action in this.owner.actions)
				{
				}
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600430B RID: 17163 RVA: 0x00002191 File Offset: 0x00000391
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
			}

			// Token: 0x0600430C RID: 17164 RVA: 0x00099F2B File Offset: 0x0009812B
			protected override void OnDetach()
			{
				throw new NotImplementedException();
			}
		}
	}
}
