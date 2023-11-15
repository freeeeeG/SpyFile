using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.Player
{
	// Token: 0x0200015E RID: 350
	public class PlayerBuffs : MonoBehaviour
	{
		// Token: 0x060008E8 RID: 2280 RVA: 0x0002533E File Offset: 0x0002353E
		private void Start()
		{
			this.buffs = new List<Buff>();
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002534B File Offset: 0x0002354B
		public void Add(Buff buff)
		{
			this.buffs.Add(buff);
			buff.owner = this;
			buff.OnAttach();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00025366 File Offset: 0x00023566
		public void Remove(Buff buff)
		{
			this.buffs.Remove(buff);
			buff.OnUnattach();
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002537C File Offset: 0x0002357C
		private void OnDestroy()
		{
			foreach (Buff buff in this.buffs)
			{
				buff.OnUnattach();
			}
		}

		// Token: 0x0400069F RID: 1695
		private List<Buff> buffs;
	}
}
