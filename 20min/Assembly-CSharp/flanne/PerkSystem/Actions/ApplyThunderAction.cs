using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A6 RID: 422
	public class ApplyThunderAction : Action
	{
		// Token: 0x060009E9 RID: 2537 RVA: 0x00027484 File Offset: 0x00025684
		public override void Init()
		{
			this.TG = ThunderGenerator.SharedInstance;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00027491 File Offset: 0x00025691
		public override void Activate(GameObject target)
		{
			this.TG.GenerateAt(target, this.damage);
		}

		// Token: 0x04000703 RID: 1795
		[SerializeField]
		private int damage;

		// Token: 0x04000704 RID: 1796
		private ThunderGenerator TG;
	}
}
