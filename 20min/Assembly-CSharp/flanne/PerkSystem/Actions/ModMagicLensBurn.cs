using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BF RID: 447
	public class ModMagicLensBurn : Action
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x00027BC0 File Offset: 0x00025DC0
		public override void Activate(GameObject target)
		{
			BulletEnhanceSummon[] componentsInChildren = PlayerController.Instance.GetComponentsInChildren<BulletEnhanceSummon>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].addBurn = this.addBurnOnHit;
			}
		}

		// Token: 0x04000725 RID: 1829
		[SerializeField]
		private bool addBurnOnHit;
	}
}
