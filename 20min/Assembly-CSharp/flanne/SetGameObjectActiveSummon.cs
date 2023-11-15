using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000126 RID: 294
	public class SetGameObjectActiveSummon : AttackingSummon
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x00021F50 File Offset: 0x00020150
		protected override bool Attack()
		{
			this.attackObj.SetActive(true);
			return true;
		}

		// Token: 0x040005D3 RID: 1491
		[SerializeField]
		private GameObject attackObj;
	}
}
