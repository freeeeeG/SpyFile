using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000FB RID: 251
	public class PowerupGenerator : MonoBehaviour
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0001FC3E File Offset: 0x0001DE3E
		private void Awake()
		{
			PowerupGenerator.CanReroll = false;
			if (PowerupGenerator.Instance == null)
			{
				PowerupGenerator.Instance = this;
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001FC68 File Offset: 0x0001DE68
		public void InitPowerupPool(int numTimesRepeatable)
		{
			this.powerupPool = this.GetPowerupPool(this.profile.powerupPool, numTimesRepeatable);
			this.devilPool = this.GetPowerupPool(this.devilDealProfile.powerupPool, -1);
			this._defaultNumTimesRepeatable = numTimesRepeatable;
			this.takenPowerups = new List<Powerup>();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001FCB7 File Offset: 0x0001DEB7
		public void SetCharacterPowerupPool(PowerupPoolProfile characterExclusiveProfile)
		{
			this.characterPool = this.GetPowerupPool(characterExclusiveProfile.powerupPool, this._defaultNumTimesRepeatable);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001FCD1 File Offset: 0x0001DED1
		public List<Powerup> GetRandomCharacterProfile(int num = 1)
		{
			if (this.characterPool == null || this.characterPool.Count < num)
			{
				return this.GetRandom(num, this.powerupPool);
			}
			return this.GetRandom(num, this.characterPool);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001FD04 File Offset: 0x0001DF04
		public List<Powerup> GetRandomDevilProfile(int num)
		{
			return this.GetRandom(num, this.devilPool);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001FD13 File Offset: 0x0001DF13
		public List<Powerup> GetRandom(int num)
		{
			return this.GetRandom(num, this.powerupPool);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001FD24 File Offset: 0x0001DF24
		public List<Powerup> GetRandom(int num, List<PowerupPoolItem> pool)
		{
			List<Powerup> list = new List<Powerup>();
			for (int i = 0; i < num; i++)
			{
				PowerupPoolItem powerupPoolItem = null;
				while (powerupPoolItem == null)
				{
					PowerupPoolItem powerupPoolItem2 = pool[Random.Range(0, pool.Count)];
					if (!list.Contains(powerupPoolItem2.powerup) && this.PrereqsMet(powerupPoolItem2.powerup))
					{
						powerupPoolItem = powerupPoolItem2;
					}
				}
				list.Add(powerupPoolItem.powerup);
			}
			return list;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001FD88 File Offset: 0x0001DF88
		public void AddToPool(List<Powerup> powerups, int amount)
		{
			this.powerupPool.AddRange(this.GetPowerupPool(powerups, amount));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001FDA0 File Offset: 0x0001DFA0
		public void RemoveFromCharacterPool(Powerup powerup)
		{
			PowerupPoolItem powerupPoolItem = this.characterPool.Find((PowerupPoolItem x) => x.powerup == powerup);
			if (powerupPoolItem == null)
			{
				return;
			}
			powerupPoolItem.numTimeRepeatable--;
			if (powerupPoolItem.numTimeRepeatable == 0)
			{
				this.characterPool.Remove(powerupPoolItem);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001FDFC File Offset: 0x0001DFFC
		public void RemoveFromDevilPool(Powerup powerup)
		{
			PowerupPoolItem powerupPoolItem = this.devilPool.Find((PowerupPoolItem x) => x.powerup == powerup);
			if (powerupPoolItem == null)
			{
				return;
			}
			powerupPoolItem.numTimeRepeatable--;
			if (powerupPoolItem.numTimeRepeatable == 0)
			{
				this.devilPool.Remove(powerupPoolItem);
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001FE58 File Offset: 0x0001E058
		public void RemoveFromPool(Powerup powerup)
		{
			PowerupPoolItem powerupPoolItem = this.powerupPool.Find((PowerupPoolItem x) => x.powerup == powerup);
			if (powerupPoolItem == null)
			{
				return;
			}
			powerupPoolItem.numTimeRepeatable--;
			if (powerupPoolItem.numTimeRepeatable == 0)
			{
				this.powerupPool.Remove(powerupPoolItem);
			}
			this.takenPowerups.Add(powerup);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001FEC4 File Offset: 0x0001E0C4
		private bool PrereqsMet(Powerup powerup)
		{
			if (powerup.anyPrereqFulfill)
			{
				foreach (Powerup item in powerup.prereqs)
				{
					if (this.takenPowerups.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Powerup item2 in powerup.prereqs)
			{
				if (!this.takenPowerups.Contains(item2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001FF7C File Offset: 0x0001E17C
		private List<PowerupPoolItem> GetPowerupPool(List<Powerup> powerups, int numTimesRepeatable)
		{
			List<PowerupPoolItem> list = new List<PowerupPoolItem>();
			foreach (Powerup powerup in powerups)
			{
				list.Add(new PowerupPoolItem
				{
					powerup = powerup,
					numTimeRepeatable = numTimesRepeatable
				});
			}
			return list;
		}

		// Token: 0x04000517 RID: 1303
		public static PowerupGenerator Instance;

		// Token: 0x04000518 RID: 1304
		public static bool CanReroll;

		// Token: 0x04000519 RID: 1305
		[SerializeField]
		private PowerupPoolProfile profile;

		// Token: 0x0400051A RID: 1306
		[SerializeField]
		private PowerupPoolProfile devilDealProfile;

		// Token: 0x0400051B RID: 1307
		private List<PowerupPoolItem> powerupPool;

		// Token: 0x0400051C RID: 1308
		private List<PowerupPoolItem> devilPool;

		// Token: 0x0400051D RID: 1309
		private List<PowerupPoolItem> characterPool;

		// Token: 0x0400051E RID: 1310
		private List<Powerup> takenPowerups;

		// Token: 0x0400051F RID: 1311
		private int _defaultNumTimesRepeatable;
	}
}
