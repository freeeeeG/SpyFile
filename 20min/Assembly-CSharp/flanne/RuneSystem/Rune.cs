using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000155 RID: 341
	public class Rune : MonoBehaviour
	{
		// Token: 0x060008CA RID: 2250 RVA: 0x00024D65 File Offset: 0x00022F65
		public void Attach(PlayerController player, int level)
		{
			this.player = player;
			base.transform.SetParent(player.transform);
			this.level = level;
			this.Init();
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00002F51 File Offset: 0x00001151
		protected virtual void Init()
		{
		}

		// Token: 0x04000682 RID: 1666
		protected PlayerController player;

		// Token: 0x04000683 RID: 1667
		protected int level;
	}
}
