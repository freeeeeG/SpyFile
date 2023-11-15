using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x02000826 RID: 2086
	public class Prisoner2 : MonoBehaviour
	{
		// Token: 0x06002B19 RID: 11033 RVA: 0x00084CCA File Offset: 0x00082ECA
		public IEnumerator COpenCursedChest()
		{
			this._openCursedChest.TryStart();
			while (this._openCursedChest.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x00084CD9 File Offset: 0x00082ED9
		public IEnumerator COpenChest()
		{
			this._openChest.TryStart();
			while (this._openChest.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x040024B6 RID: 9398
		[SerializeField]
		private Characters.Actions.Action _openCursedChest;

		// Token: 0x040024B7 RID: 9399
		[SerializeField]
		private Characters.Actions.Action _openChest;
	}
}
