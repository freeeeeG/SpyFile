using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D8 RID: 472
	public class TriggerOnHitAction : Action
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x00028996 File Offset: 0x00026B96
		public override void Activate(GameObject target)
		{
			PlayerController.Instance.gameObject.PostNotification(Projectile.ImpactEvent, target);
		}
	}
}
