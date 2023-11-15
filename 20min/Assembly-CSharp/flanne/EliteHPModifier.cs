using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000085 RID: 133
	[CreateAssetMenu(fileName = "EliteHPModifier", menuName = "DifficultyMods/EliteHPModifier")]
	public class EliteHPModifier : DifficultyModifier
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x0001953A File Offset: 0x0001773A
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.eliteHealthMultiplier += this.additionalHPPercent;
		}

		// Token: 0x04000314 RID: 788
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalHPPercent;
	}
}
