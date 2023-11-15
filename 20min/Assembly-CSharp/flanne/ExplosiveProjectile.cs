using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne
{
	// Token: 0x02000101 RID: 257
	public class ExplosiveProjectile : Projectile
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0002007A File Offset: 0x0001E27A
		private int contactDamage
		{
			get
			{
				return Mathf.FloorToInt(this.damage / 2f);
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00020090 File Offset: 0x0001E290
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.explosionPrefab.name, this.explosionPrefab, 50, true);
			this._initialized = true;
			this._isQuitting = false;
			SceneManager.activeSceneChanged += this.ChangedActiveScene;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000200E6 File Offset: 0x0001E2E6
		private void OnDestroy()
		{
			SceneManager.activeSceneChanged -= this.ChangedActiveScene;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000200FC File Offset: 0x0001E2FC
		protected override void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.GetComponent<Health>() == null)
			{
				return;
			}
			if (this.piercing != 0)
			{
				this.DealContactDamage(other.gameObject.GetComponent<Health>(), other);
				this.piercing--;
				return;
			}
			if (this.bounce == 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.DealContactDamage(other.gameObject.GetComponent<Health>(), other);
			this.bounce--;
			AIComponent component = other.gameObject.GetComponent<AIComponent>();
			base.BounceOffEnemy(component);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0002018E File Offset: 0x0001E38E
		private void OnDisable()
		{
			if (this._initialized && !this._isQuitting)
			{
				this.SpawnExplosive();
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000201A6 File Offset: 0x0001E3A6
		private void OnApplicationQuit()
		{
			this._isQuitting = true;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000201A6 File Offset: 0x0001E3A6
		private void ChangedActiveScene(Scene current, Scene next)
		{
			this._isQuitting = true;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000201B0 File Offset: 0x0001E3B0
		public void SpawnExplosive()
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.explosionPrefab.name);
			pooledObject.GetComponent<Harmful>().damageAmount = Mathf.FloorToInt(this.damage);
			pooledObject.transform.position = base.transform.position;
			pooledObject.transform.localScale = this._explosionSize;
			pooledObject.SetActive(true);
			ExplosionShake2D explosionShake2D = this.cameraShaker;
			if (explosionShake2D != null)
			{
				explosionShake2D.Shake();
			}
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			if (!this.isSecondary)
			{
				this.PostNotification(ExplosiveProjectile.ProjExplodeEvent);
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002024D File Offset: 0x0001E44D
		protected override void SetSize(float size)
		{
			this._explosionSize = new Vector3(size, size, 1f);
			base.SetSize(size);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00020268 File Offset: 0x0001E468
		private void DealContactDamage(Health health, Collision2D other)
		{
			if (health == null)
			{
				return;
			}
			health.HPChange(-1 * this.contactDamage);
		}

		// Token: 0x0400052A RID: 1322
		public static string ProjExplodeEvent = "ExplosiveProjectile.ProjExplodeEvent";

		// Token: 0x0400052B RID: 1323
		[SerializeField]
		private GameObject explosionPrefab;

		// Token: 0x0400052C RID: 1324
		[SerializeField]
		private ExplosionShake2D cameraShaker;

		// Token: 0x0400052D RID: 1325
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400052E RID: 1326
		private bool _initialized;

		// Token: 0x0400052F RID: 1327
		private bool _isQuitting;

		// Token: 0x04000530 RID: 1328
		private Vector3 _explosionSize;

		// Token: 0x04000531 RID: 1329
		private ObjectPooler OP;
	}
}
