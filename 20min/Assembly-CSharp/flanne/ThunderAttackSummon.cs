using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000130 RID: 304
	public class ThunderAttackSummon : AttackingSummon
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x000228EE File Offset: 0x00020AEE
		protected override void Init()
		{
			this.TGen = ThunderGenerator.SharedInstance;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000228FC File Offset: 0x00020AFC
		protected override bool Attack()
		{
			Vector2 center = base.transform.position;
			GameObject gameObject = null;
			for (int i = 0; i < this.thundersPerAttack; i++)
			{
				gameObject = EnemyFinder.GetRandomEnemy(center, this.range);
				if (gameObject != null)
				{
					this.TGen.GenerateAt(gameObject, Mathf.FloorToInt(base.summonDamageMod.Modify((float)this.baseDamage)));
				}
			}
			return gameObject != null;
		}

		// Token: 0x04000600 RID: 1536
		[SerializeField]
		private int thundersPerAttack;

		// Token: 0x04000601 RID: 1537
		[SerializeField]
		private Vector2 range;

		// Token: 0x04000602 RID: 1538
		[SerializeField]
		private int baseDamage;

		// Token: 0x04000603 RID: 1539
		private ThunderGenerator TGen;
	}
}
