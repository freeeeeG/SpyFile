using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Level.EssenceSanctuary
{
	// Token: 0x02000604 RID: 1540
	public class Room : MonoBehaviour
	{
		// Token: 0x06001EE7 RID: 7911 RVA: 0x0005DC88 File Offset: 0x0005BE88
		public void Initialize(Tilemap baseTilemap, Transform machine)
		{
			this._pasteTile.Paste(baseTilemap);
			machine.position = this._machinePosition.position;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0005DCA7 File Offset: 0x0005BEA7
		public void Accept()
		{
			UnityEvent onAccept = this._onAccept;
			if (onAccept == null)
			{
				return;
			}
			onAccept.Invoke();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0005DCB9 File Offset: 0x0005BEB9
		public void Clear()
		{
			UnityEvent onClear = this._onClear;
			if (onClear == null)
			{
				return;
			}
			onClear.Invoke();
		}

		// Token: 0x04001A19 RID: 6681
		[SerializeField]
		private PasteTile _pasteTile;

		// Token: 0x04001A1A RID: 6682
		[SerializeField]
		private Transform _machinePosition;

		// Token: 0x04001A1B RID: 6683
		[SerializeField]
		private UnityEvent _onAccept;

		// Token: 0x04001A1C RID: 6684
		[SerializeField]
		private UnityEvent _onClear;
	}
}
