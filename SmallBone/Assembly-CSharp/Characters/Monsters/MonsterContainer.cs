using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Monsters
{
	// Token: 0x02000814 RID: 2068
	public class MonsterContainer : MonoBehaviour
	{
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000832CE File Offset: 0x000814CE
		public List<Monster> monsters
		{
			get
			{
				return this._monsters;
			}
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000832D6 File Offset: 0x000814D6
		private void Awake()
		{
			this._monsters = new List<Monster>();
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000832E3 File Offset: 0x000814E3
		public void Add(Monster minion)
		{
			this._monsters.Add(minion);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000832F1 File Offset: 0x000814F1
		public bool Remove(Monster minion)
		{
			return this._monsters.Remove(minion);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000832FF File Offset: 0x000814FF
		public int Count()
		{
			return this._monsters.Count;
		}

		// Token: 0x04002440 RID: 9280
		private List<Monster> _monsters;
	}
}
