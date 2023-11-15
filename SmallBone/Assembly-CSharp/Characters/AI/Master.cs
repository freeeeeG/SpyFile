using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200110A RID: 4362
	public class Master : MonoBehaviour
	{
		// Token: 0x060054D8 RID: 21720 RVA: 0x000FDA80 File Offset: 0x000FBC80
		public void AddSlave(Slave slave)
		{
			this._slaves.Add(slave);
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x000FDA8E File Offset: 0x000FBC8E
		public void RemoveSlave(Slave slave)
		{
			this._slaves.Remove(slave);
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x000FDA9D File Offset: 0x000FBC9D
		public bool isCleared()
		{
			return this._slaves.Count == 0;
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x000FDAAD File Offset: 0x000FBCAD
		public int GetSlavesLeft()
		{
			return this._slaves.Count;
		}

		// Token: 0x0400440F RID: 17423
		public Character character;

		// Token: 0x04004410 RID: 17424
		private List<Slave> _slaves = new List<Slave>();
	}
}
