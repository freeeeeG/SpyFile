using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000134 RID: 308
	public class TieredUnlockManager : MonoBehaviour
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00022C14 File Offset: 0x00020E14
		public TieredUnlockData unlockData
		{
			get
			{
				TieredUnlockData tieredUnlockData = new TieredUnlockData(this.unlockables.Length);
				for (int i = 0; i < this.unlockables.Length; i++)
				{
					tieredUnlockData.unlocks[i] = this.unlockables[i].level;
				}
				return tieredUnlockData;
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00022C58 File Offset: 0x00020E58
		public void LoadData(TieredUnlockData data)
		{
			if (data == null)
			{
				data = new TieredUnlockData(this.unlockables.Length);
			}
			if (data.unlocks == null)
			{
				data.unlocks = new int[this.unlockables.Length];
			}
			if (data.unlocks.Length != this.unlockables.Length)
			{
				Array.Resize<int>(ref data.unlocks, this.unlockables.Length);
			}
			for (int i = 0; i < this.unlockables.Length; i++)
			{
				this.unlockables[i].level = data.unlocks[i];
			}
		}

		// Token: 0x0400060F RID: 1551
		[SerializeField]
		[SerializeReference]
		private TieredUnlockable[] unlockables;
	}
}
