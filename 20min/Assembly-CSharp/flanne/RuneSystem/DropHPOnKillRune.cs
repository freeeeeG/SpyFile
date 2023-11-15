using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200014B RID: 331
	public class DropHPOnKillRune : Rune
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00024135 File Offset: 0x00022335
		public int killThreshold
		{
			get
			{
				return this.baseKillThreshold - this.thresholdReductionPerLevel * this.level;
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0002414C File Offset: 0x0002234C
		protected override void Init()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.hpPrefab.name, this.hpPrefab, 100, true);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002419A File Offset: 0x0002239A
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x000241B4 File Offset: 0x000223B4
		private void OnDeath(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (gameObject.tag == "Enemy")
			{
				this._counter++;
				if (this._counter >= this.killThreshold)
				{
					this.DropHP(gameObject.transform.position);
					this._counter = 0;
				}
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00024213 File Offset: 0x00022413
		private void DropHP(Vector3 position)
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.hpPrefab.name);
			pooledObject.transform.position = position;
			pooledObject.SetActive(true);
		}

		// Token: 0x04000651 RID: 1617
		[SerializeField]
		private GameObject hpPrefab;

		// Token: 0x04000652 RID: 1618
		[SerializeField]
		private int baseKillThreshold;

		// Token: 0x04000653 RID: 1619
		[SerializeField]
		private int thresholdReductionPerLevel;

		// Token: 0x04000654 RID: 1620
		private int _counter;

		// Token: 0x04000655 RID: 1621
		private ObjectPooler OP;
	}
}
