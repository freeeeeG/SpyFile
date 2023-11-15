using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C6 RID: 454
	public class ModThunderAreaAction : Action
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x00027EDD File Offset: 0x000260DD
		public override void Activate(GameObject target)
		{
			ThunderGenerator.SharedInstance.sizeMultiplier += this.thunderAOEMod;
		}

		// Token: 0x04000730 RID: 1840
		[SerializeField]
		private float thunderAOEMod;
	}
}
