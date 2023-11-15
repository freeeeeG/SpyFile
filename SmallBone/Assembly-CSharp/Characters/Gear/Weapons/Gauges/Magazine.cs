using System;
using System.Collections;
using Characters.Actions.Constraints.Customs;
using UnityEngine;

namespace Characters.Gear.Weapons.Gauges
{
	// Token: 0x0200083C RID: 2108
	public class Magazine : MonoBehaviour
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x0008666B File Offset: 0x0008486B
		// (set) Token: 0x06002BC2 RID: 11202 RVA: 0x00086673 File Offset: 0x00084873
		public bool nonConsumptionState { get; set; }

		// Token: 0x06002BC3 RID: 11203 RVA: 0x0008667C File Offset: 0x0008487C
		private void Awake()
		{
			this._bullet = new Bullet((int)this._gaugeWithValue.maxValue);
			this._gaugeWithValue.FillUp();
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000866A0 File Offset: 0x000848A0
		public bool Has(int amount)
		{
			return this._bullet.Has(amount);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000866AE File Offset: 0x000848AE
		public void Consume(int amount)
		{
			if (this.nonConsumptionState)
			{
				return;
			}
			if (this._bullet.Consume(amount))
			{
				this._gaugeWithValue.Consume((float)amount);
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000866D5 File Offset: 0x000848D5
		public void Reload()
		{
			CoroutineProxy.instance.StartCoroutine(this.CReload());
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000866E8 File Offset: 0x000848E8
		private IEnumerator CReload()
		{
			Color defaultColor = this._gaugeWithValue.defaultBarColor;
			this._gaugeWithValue.defaultBarColor = this._reloadColor;
			yield return Chronometer.global.WaitForSeconds(2f);
			this._gaugeWithValue.FillUp();
			this._bullet.Reload();
			this._gaugeWithValue.defaultBarColor = defaultColor;
			yield break;
		}

		// Token: 0x04002511 RID: 9489
		[SerializeField]
		private ValueGauge _gaugeWithValue;

		// Token: 0x04002512 RID: 9490
		[SerializeField]
		private Color _reloadColor;

		// Token: 0x04002513 RID: 9491
		private Bullet _bullet;
	}
}
