using System;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BD9 RID: 3033
	[Serializable]
	public sealed class Charm : Ability
	{
		// Token: 0x06003E6B RID: 15979 RVA: 0x000B5898 File Offset: 0x000B3A98
		public void SetAttacker(Character attacker)
		{
			this._attacker = attacker;
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x000B58A1 File Offset: 0x000B3AA1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Charm.Instance(owner, this);
		}

		// Token: 0x04003032 RID: 12338
		private Character _attacker;

		// Token: 0x02000BDA RID: 3034
		public class Instance : AbilityInstance<Charm>
		{
			// Token: 0x06003E6E RID: 15982 RVA: 0x000B58AA File Offset: 0x000B3AAA
			public Instance(Character owner, Charm ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E6F RID: 15983 RVA: 0x000B58B4 File Offset: 0x000B3AB4
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003E70 RID: 15984 RVA: 0x000B58D7 File Offset: 0x000B3AD7
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003E71 RID: 15985 RVA: 0x000B58F6 File Offset: 0x000B3AF6
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				return !(target.character != this.ability._attacker);
			}
		}
	}
}
