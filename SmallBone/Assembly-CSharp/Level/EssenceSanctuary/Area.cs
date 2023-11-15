using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level.EssenceSanctuary
{
	// Token: 0x02000603 RID: 1539
	public class Area : MonoBehaviour
	{
		// Token: 0x06001EE3 RID: 7907 RVA: 0x0005DC39 File Offset: 0x0005BE39
		public void Initialize()
		{
			this._room = UnityEngine.Object.Instantiate<Room>(this._roomsForThisArea.Random<Room>(), base.transform);
			this._room.Initialize(this._baseTilemap, this._machine);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0005DC6E File Offset: 0x0005BE6E
		public void Accept()
		{
			this._room.Accept();
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0005DC7B File Offset: 0x0005BE7B
		public void Clear()
		{
			this._room.Clear();
		}

		// Token: 0x04001A15 RID: 6677
		[SerializeField]
		private Tilemap _baseTilemap;

		// Token: 0x04001A16 RID: 6678
		[SerializeField]
		private Transform _machine;

		// Token: 0x04001A17 RID: 6679
		[SerializeField]
		private Room[] _roomsForThisArea;

		// Token: 0x04001A18 RID: 6680
		private Room _room;
	}
}
