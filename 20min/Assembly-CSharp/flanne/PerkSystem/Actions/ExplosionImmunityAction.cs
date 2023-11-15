using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AF RID: 431
	public class ExplosionImmunityAction : Action
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x00027808 File Offset: 0x00025A08
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.playerHealth.RemoveVulnerability("HarmfulToPlayer");
		}
	}
}
