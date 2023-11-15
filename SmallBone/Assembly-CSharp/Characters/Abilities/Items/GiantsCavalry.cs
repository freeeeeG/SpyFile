using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CC2 RID: 3266
	[Serializable]
	public sealed class GiantsCavalry : Ability
	{
		// Token: 0x06004240 RID: 16960 RVA: 0x000C102A File Offset: 0x000BF22A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GiantsCavalry.Instance(owner, this);
		}

		// Token: 0x02000CC3 RID: 3267
		public class Instance : AbilityInstance<GiantsCavalry>
		{
			// Token: 0x06004242 RID: 16962 RVA: 0x000C1033 File Offset: 0x000BF233
			public Instance(Character owner, GiantsCavalry ability) : base(owner, ability)
			{
			}

			// Token: 0x06004243 RID: 16963 RVA: 0x000C103D File Offset: 0x000BF23D
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06004244 RID: 16964 RVA: 0x000C1060 File Offset: 0x000BF260
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (damage.motionType == Damage.MotionType.Dash)
				{
					damage.stoppingPower *= 2f;
				}
				return false;
			}

			// Token: 0x06004245 RID: 16965 RVA: 0x000C107B File Offset: 0x000BF27B
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}
		}
	}
}
