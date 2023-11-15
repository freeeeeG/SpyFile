using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200015B RID: 347
	public class SpreadStatusRune : Rune
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x00025008 File Offset: 0x00023208
		private void OnDeath(object sender, object args)
		{
			if (Random.Range(0f, 1f) > this.spreadChancePerLevel * (float)this.level)
			{
				return;
			}
			GameObject gameObject = (sender as Health).gameObject;
			if (this.BurnSys.IsBurning(gameObject))
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.burnSpreadPrefab.name);
				pooledObject.transform.position = gameObject.transform.position;
				pooledObject.SetActive(true);
			}
			if (this.FreezeSys.IsFrozen(gameObject))
			{
				GameObject pooledObject2 = this.OP.GetPooledObject(this.freezeSpreadPrefab.name);
				pooledObject2.transform.position = gameObject.transform.position;
				pooledObject2.SetActive(true);
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000250C4 File Offset: 0x000232C4
		private void Start()
		{
			this.BurnSys = BurnSystem.SharedInstance;
			this.FreezeSys = FreezeSystem.SharedInstance;
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.burnSpreadPrefab.name, this.burnSpreadPrefab, 100, true);
			this.OP.AddObject(this.freezeSpreadPrefab.name, this.freezeSpreadPrefab, 100, true);
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00025147 File Offset: 0x00023347
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x04000697 RID: 1687
		[Range(0f, 1f)]
		[SerializeField]
		private float spreadChancePerLevel;

		// Token: 0x04000698 RID: 1688
		[SerializeField]
		private GameObject burnSpreadPrefab;

		// Token: 0x04000699 RID: 1689
		[SerializeField]
		private GameObject freezeSpreadPrefab;

		// Token: 0x0400069A RID: 1690
		private BurnSystem BurnSys;

		// Token: 0x0400069B RID: 1691
		private FreezeSystem FreezeSys;

		// Token: 0x0400069C RID: 1692
		private ObjectPooler OP;
	}
}
