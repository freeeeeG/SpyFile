using System;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200077F RID: 1919
	public sealed class Heal : CharacterHitOperation
	{
		// Token: 0x0600277E RID: 10110 RVA: 0x000767BF File Offset: 0x000749BF
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			if (character == null)
			{
				return;
			}
			character.health.Heal(this.GetAmount(character.health), true);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000767E4 File Offset: 0x000749E4
		private double GetAmount(Health health)
		{
			Heal.Type type = this._type;
			if (type == Heal.Type.Percent)
			{
				return (double)this._amount.value * health.maximumHealth * 0.009999999776482582;
			}
			if (type != Heal.Type.Constnat)
			{
				return 0.0;
			}
			return (double)this._amount.value;
		}

		// Token: 0x0400219F RID: 8607
		[SerializeField]
		private Heal.Type _type;

		// Token: 0x040021A0 RID: 8608
		[SerializeField]
		private CustomFloat _amount;

		// Token: 0x02000780 RID: 1920
		private enum Type
		{
			// Token: 0x040021A2 RID: 8610
			Percent,
			// Token: 0x040021A3 RID: 8611
			Constnat
		}
	}
}
