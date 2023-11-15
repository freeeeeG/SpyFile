using System;
using flanne.Core;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000088 RID: 136
	[CreateAssetMenu(fileName = "ReducePowerupChoicesModifier", menuName = "DifficultyMods/ReducePowerupChoicesModifier")]
	public class ReducePowerupChoicesModifier : DifficultyModifier
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0001958A File Offset: 0x0001778A
		public override void ModifyGame(GameController gameController)
		{
			gameController.numPowerupChoices -= this.amountToReduce;
		}

		// Token: 0x04000318 RID: 792
		[SerializeField]
		private int amountToReduce;
	}
}
