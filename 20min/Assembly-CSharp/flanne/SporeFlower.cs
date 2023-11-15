using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200012C RID: 300
	public class SporeFlower : AttackingSummon
	{
		// Token: 0x06000815 RID: 2069 RVA: 0x0002234C File Offset: 0x0002054C
		protected override void Init()
		{
			this._layer = 1 << TagLayerUtil.Enemy;
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.sporePrefab.name, this.sporePrefab.gameObject, 20, true);
			this.OP.AddObject(this.sporeExplosionPrefab.name, this.sporeExplosionPrefab.gameObject, 20, true);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000223C4 File Offset: 0x000205C4
		protected override bool Attack()
		{
			Collider2D[] array = new Collider2D[2];
			int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.range, array, this._layer);
			if (num == 0)
			{
				return false;
			}
			this.LaunchSpore(array[Random.Range(0, num)].transform.position, 0.7f);
			return true;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00022424 File Offset: 0x00020624
		private void OnCollisionEnter2D(Collision2D collision)
		{
			this.animator.SetTrigger("Attack");
			this._hitCtr++;
			if (this._hitCtr >= this.sprayThreshold)
			{
				this._hitCtr -= this.sprayThreshold;
				this.SpraySpores();
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00022478 File Offset: 0x00020678
		private void LaunchSpore(Vector2 targetPos, float duration)
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.sporePrefab.name);
			pooledObject.transform.position = this.firePoint.position;
			pooledObject.SetActive(true);
			AreaProjectile spore = pooledObject.GetComponent<AreaProjectile>();
			spore.TargetPos(targetPos, duration);
			spore.onTargetReached.AddListener(delegate()
			{
				this.OnSporeHit(spore);
			});
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000224FC File Offset: 0x000206FC
		private void SpraySpores()
		{
			Collider2D[] array = new Collider2D[this.amountSporeTrack];
			int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.range, array, this._layer);
			for (int i = 0; i < Mathf.Min(this.sprayAmount, num); i++)
			{
				this.LaunchSpore(array[i].transform.position, 0.5f + Random.Range(-0.2f, 0.2f));
			}
			for (int j = num; j < this.sprayAmount; j++)
			{
				Vector2 targetPos = base.transform.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(1f, this.range);
				this.LaunchSpore(targetPos, 0.7f + Random.Range(-0.2f, 0.2f));
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00022608 File Offset: 0x00020808
		private void OnSporeHit(AreaProjectile spore)
		{
			spore.onTargetReached.RemoveAllListeners();
			GameObject pooledObject = this.OP.GetPooledObject(this.sporeExplosionPrefab.name);
			HarmfulOnContact component = pooledObject.GetComponent<HarmfulOnContact>();
			int num = Mathf.CeilToInt(base.summonDamageMod.Modify((float)this.baseDamage));
			if (this.multiplyByBulletDamage)
			{
				num = Mathf.CeilToInt(this.player.stats[StatType.BulletDamage].Modify((float)num));
			}
			component.damageAmount = num;
			component.procOnHit = this.procsOnHit;
			pooledObject.transform.position = spore.transform.position;
			pooledObject.SetActive(true);
			SoundEffectSO soundEffectSO = this.explosionSFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x040005E4 RID: 1508
		[SerializeField]
		private AreaProjectile sporePrefab;

		// Token: 0x040005E5 RID: 1509
		[SerializeField]
		private Harmful sporeExplosionPrefab;

		// Token: 0x040005E6 RID: 1510
		[SerializeField]
		private float range = 5f;

		// Token: 0x040005E7 RID: 1511
		[SerializeField]
		private int baseDamage = 24;

		// Token: 0x040005E8 RID: 1512
		[SerializeField]
		private Transform firePoint;

		// Token: 0x040005E9 RID: 1513
		[SerializeField]
		private SoundEffectSO explosionSFX;

		// Token: 0x040005EA RID: 1514
		[SerializeField]
		private int sprayThreshold = 5;

		// Token: 0x040005EB RID: 1515
		[SerializeField]
		private int sprayAmount = 5;

		// Token: 0x040005EC RID: 1516
		[SerializeField]
		private int amountSporeTrack = 5;

		// Token: 0x040005ED RID: 1517
		public bool multiplyByBulletDamage;

		// Token: 0x040005EE RID: 1518
		public bool procsOnHit;

		// Token: 0x040005EF RID: 1519
		private ObjectPooler OP;

		// Token: 0x040005F0 RID: 1520
		private int _layer;

		// Token: 0x040005F1 RID: 1521
		private int _hitCtr;
	}
}
