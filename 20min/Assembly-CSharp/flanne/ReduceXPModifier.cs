using System;
using flanne.Core;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000089 RID: 137
	[CreateAssetMenu(fileName = "ReduceXPModifier", menuName = "DifficultyMods/ReduceXPModifier")]
	public class ReduceXPModifier : DifficultyModifier
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x0001959F File Offset: 0x0001779F
		public override void ModifyGame(GameController gameController)
		{
			gameController.playerXP.xpMultiplier.AddMultiplierReduction(1f - this.xpReduction);
		}

		// Token: 0x04000319 RID: 793
		[Range(0f, 1f)]
		[SerializeField]
		private float xpReduction;
	}
}
