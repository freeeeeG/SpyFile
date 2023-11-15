using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A4 RID: 420
	public class ApplyFreezeAction : Action
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x00027451 File Offset: 0x00025651
		public override void Activate(GameObject target)
		{
			FreezeSystem.SharedInstance.Freeze(target);
		}
	}
}
