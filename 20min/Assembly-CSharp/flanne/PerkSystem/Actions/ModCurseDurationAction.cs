using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BA RID: 442
	public class ModCurseDurationAction : Action
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x00027A3D File Offset: 0x00025C3D
		public override void Activate(GameObject target)
		{
			CurseSystem.Instance.duration += (float)this.durationMod;
		}

		// Token: 0x0400071F RID: 1823
		[SerializeField]
		private int durationMod;
	}
}
