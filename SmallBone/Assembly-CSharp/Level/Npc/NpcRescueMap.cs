using System;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005B1 RID: 1457
	public class NpcRescueMap : MonoBehaviour
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x00058B36 File Offset: 0x00056D36
		public Map map
		{
			get
			{
				return this._map;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x00058B3E File Offset: 0x00056D3E
		public NpcType npcType
		{
			get
			{
				return this._npcType;
			}
		}

		// Token: 0x0400188D RID: 6285
		[SerializeField]
		private Map _map;

		// Token: 0x0400188E RID: 6286
		[SerializeField]
		private NpcType _npcType;
	}
}
