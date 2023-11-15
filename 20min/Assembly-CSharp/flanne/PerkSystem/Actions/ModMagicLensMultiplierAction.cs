using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C0 RID: 448
	public class ModMagicLensMultiplierAction : Action
	{
		// Token: 0x06000A29 RID: 2601 RVA: 0x00027BF4 File Offset: 0x00025DF4
		public override void Activate(GameObject target)
		{
			BulletEnhanceSummon[] componentsInChildren = PlayerController.Instance.GetComponentsInChildren<BulletEnhanceSummon>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].overallMultiplier += this.overallMultplierMod;
			}
		}

		// Token: 0x04000726 RID: 1830
		[SerializeField]
		private float overallMultplierMod;
	}
}
