using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000257 RID: 599
	public class ThunderOnAttack : AttackOnShoot
	{
		// Token: 0x06000D01 RID: 3329 RVA: 0x0002F675 File Offset: 0x0002D875
		protected override void Init()
		{
			this.TGen = ThunderGenerator.SharedInstance;
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002F690 File Offset: 0x0002D890
		public override void Attack()
		{
			for (int i = 0; i < this.numThunderPerAttack; i++)
			{
				GameObject closestEnemy = EnemyFinder.GetClosestEnemy(Camera.main.ScreenToWorldPoint(this.SC.cursorPosition));
				if (closestEnemy != null)
				{
					this.TGen.GenerateAt(closestEnemy, this.baseDamage);
				}
			}
		}

		// Token: 0x0400094E RID: 2382
		[SerializeField]
		private int baseDamage;

		// Token: 0x0400094F RID: 2383
		[SerializeField]
		private int numThunderPerAttack = 1;

		// Token: 0x04000950 RID: 2384
		private ThunderGenerator TGen;

		// Token: 0x04000951 RID: 2385
		private ShootingCursor SC;
	}
}
