using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200013F RID: 319
	public class UnlockablesManager : MonoBehaviour
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00023184 File Offset: 0x00021384
		public UnlockData unlockData
		{
			get
			{
				UnlockData unlockData = new UnlockData(this.unlockables.Length);
				for (int i = 0; i < this.unlockables.Length; i++)
				{
					unlockData.unlocks[i] = !this.unlockables[i].IsLocked;
				}
				return unlockData;
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000231CC File Offset: 0x000213CC
		public void LoadData(UnlockData data)
		{
			if (data == null)
			{
				data = new UnlockData(this.unlockables.Length);
			}
			if (data.unlocks == null)
			{
				data.unlocks = new bool[this.unlockables.Length];
			}
			if (data.unlocks.Length != this.unlockables.Length)
			{
				Array.Resize<bool>(ref data.unlocks, this.unlockables.Length);
			}
			for (int i = 0; i < this.unlockables.Length; i++)
			{
				if (data.unlocks[i])
				{
					this.unlockables[i].gameObject.SetActive(true);
					this.unlockables[i].Unlock();
				}
				else
				{
					this.unlockables[i].Lock();
				}
			}
		}

		// Token: 0x0400062E RID: 1582
		[SerializeField]
		private Unlockable[] unlockables;
	}
}
