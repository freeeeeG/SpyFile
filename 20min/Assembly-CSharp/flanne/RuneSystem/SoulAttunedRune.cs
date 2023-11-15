using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000159 RID: 345
	public class SoulAttunedRune : Rune
	{
		// Token: 0x060008D8 RID: 2264 RVA: 0x00024EE8 File Offset: 0x000230E8
		protected override void Init()
		{
			PlayerController.Instance.playerHealth.maxSHP += this.shpMaxPerLevel * this.level;
		}

		// Token: 0x04000690 RID: 1680
		[SerializeField]
		private int shpMaxPerLevel;
	}
}
