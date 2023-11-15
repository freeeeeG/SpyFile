using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000084 RID: 132
	[CreateAssetMenu(fileName = "EliteDamageModifier", menuName = "DifficultyMods/EliteDamageModifier")]
	public class EliteDamageModifier : DifficultyModifier
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00019525 File Offset: 0x00017725
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.eliteDamage += this.additionalDamage;
		}

		// Token: 0x04000313 RID: 787
		[SerializeField]
		private int additionalDamage;
	}
}
