using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BE RID: 446
	public class ModMagicLensBounceAction : Action
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x00027B84 File Offset: 0x00025D84
		public override void Activate(GameObject target)
		{
			BulletEnhanceSummon[] componentsInChildren = PlayerController.Instance.GetComponentsInChildren<BulletEnhanceSummon>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].additionalBounces += this.additionalBounces;
			}
		}

		// Token: 0x04000724 RID: 1828
		[SerializeField]
		private int additionalBounces;
	}
}
