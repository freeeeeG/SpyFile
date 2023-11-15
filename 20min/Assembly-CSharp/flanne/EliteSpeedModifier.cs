using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000086 RID: 134
	[CreateAssetMenu(fileName = "EliteSpeedModifier", menuName = "DifficultyMods/EliteSpeedModifier")]
	public class EliteSpeedModifier : DifficultyModifier
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x0001954F File Offset: 0x0001774F
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.eliteSpeedMultiplier += this.additionalSpeedPercent;
		}

		// Token: 0x04000315 RID: 789
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalSpeedPercent;
	}
}
