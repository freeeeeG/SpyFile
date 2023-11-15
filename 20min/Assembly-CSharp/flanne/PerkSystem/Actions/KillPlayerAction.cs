using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B5 RID: 437
	public class KillPlayerAction : Action
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x000278FF File Offset: 0x00025AFF
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.playerHealth.AutoKill();
		}

		// Token: 0x04000718 RID: 1816
		[SerializeField]
		private bool affectChampions;
	}
}
