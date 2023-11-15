using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000107 RID: 263
	public class SalvoShooter : Shooter
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x00020750 File Offset: 0x0001E950
		public override void Init()
		{
			this._layer = 1 << TagLayerUtil.Enemy;
			this.player = PlayerController.Instance;
			this._lastPos = this.player.transform.position;
			this._currPos = this.player.transform.position;
			this.OP.AddObject(this.targetIndicatorPrefab.name, this.targetIndicatorPrefab, 20, true);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000207D4 File Offset: 0x0001E9D4
		private void Update()
		{
			if (this._shootSalveCR != null)
			{
				return;
			}
			this.UpdateDistanceMoved();
			this.UpdateIndicators();
			if (this._distanceCtr >= this.distanceToLockOn)
			{
				this._distanceCtr -= this.distanceToLockOn;
				Transform closestUnlockedEnemy = this.GetClosestUnlockedEnemy();
				if (closestUnlockedEnemy != null)
				{
					this.LockOn(closestUnlockedEnemy);
				}
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00020830 File Offset: 0x0001EA30
		private void OnDisable()
		{
			foreach (GameObject gameObject in this._indicators)
			{
				gameObject.SetActive(false);
			}
			this._indicators.Clear();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002088C File Offset: 0x0001EA8C
		public override void Shoot(ProjectileRecipe recipe, Vector2 pointDirection, int numProjectiles, float spread, float inaccuracy)
		{
			if (this._shootSalveCR == null)
			{
				this._shootSalveCR = this.SalvoShootCR(recipe, numProjectiles, spread, inaccuracy);
				base.StartCoroutine(this._shootSalveCR);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000208B8 File Offset: 0x0001EAB8
		private void UpdateDistanceMoved()
		{
			this._lastPos = this._currPos;
			this._currPos = this.player.transform.position;
			Vector2 vector = this._lastPos - this._currPos;
			this._distanceCtr += vector.magnitude;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00020914 File Offset: 0x0001EB14
		private void UpdateIndicators()
		{
			for (int i = this._targets.Count - 1; i >= 0; i--)
			{
				if (this._targets[i].gameObject.activeSelf)
				{
					this._indicators[i].transform.position = this._targets[i].position;
				}
				else
				{
					this._indicators[i].SetActive(false);
					this._indicators.RemoveAt(i);
					this._targets.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000209A4 File Offset: 0x0001EBA4
		private Transform GetClosestUnlockedEnemy()
		{
			Collider2D[] array = new Collider2D[250];
			int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.range, array, this._layer);
			Transform result = null;
			float num2 = float.PositiveInfinity;
			Vector2 b = base.transform.position;
			for (int i = 0; i < num; i++)
			{
				Transform transform = array[i].transform;
				if (!this._targets.Contains(transform) && !transform.tag.Contains("Passive"))
				{
					float magnitude = (transform.position - b).magnitude;
					if (magnitude < num2)
					{
						num2 = magnitude;
						result = transform;
					}
				}
			}
			return result;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00020A60 File Offset: 0x0001EC60
		private void LockOn(Transform target)
		{
			if (this._targets.Count >= this.player.ammo.amount)
			{
				return;
			}
			this._targets.Add(target);
			GameObject pooledObject = this.OP.GetPooledObject(this.targetIndicatorPrefab.name);
			this._indicators.Add(pooledObject);
			pooledObject.transform.position = target.position;
			pooledObject.SetActive(true);
			SoundEffectSO soundEffectSO = this.targetAcquiredSFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00020AE4 File Offset: 0x0001ECE4
		private IEnumerator SalvoShootCR(ProjectileRecipe recipe, int numProjectiles, float spread, float inaccuracy)
		{
			foreach (GameObject gameObject in this._indicators)
			{
				gameObject.SetActive(false);
			}
			this._indicators.Clear();
			float delayBetweenShots = this.player.stats[StatType.FireRate].ModifyInverse(0.05f);
			foreach (Transform transform in this._targets)
			{
				if (this.player.ammo.amount != 0)
				{
					this.player.gun.gunData.gunshotSFX.Play(null);
					Vector3 v = transform.position - base.transform.position;
					base.Shoot(recipe, v, numProjectiles, spread, inaccuracy);
					yield return new WaitForSeconds(delayBetweenShots);
				}
			}
			List<Transform>.Enumerator enumerator2 = default(List<Transform>.Enumerator);
			this._targets.Clear();
			this._shootSalveCR = null;
			yield break;
			yield break;
		}

		// Token: 0x0400054D RID: 1357
		[SerializeField]
		private GameObject targetIndicatorPrefab;

		// Token: 0x0400054E RID: 1358
		[SerializeField]
		private float distanceToLockOn;

		// Token: 0x0400054F RID: 1359
		[SerializeField]
		private float range = 7f;

		// Token: 0x04000550 RID: 1360
		[SerializeField]
		private SoundEffectSO targetAcquiredSFX;

		// Token: 0x04000551 RID: 1361
		private PlayerController player;

		// Token: 0x04000552 RID: 1362
		private List<GameObject> _indicators = new List<GameObject>();

		// Token: 0x04000553 RID: 1363
		private List<Transform> _targets = new List<Transform>();

		// Token: 0x04000554 RID: 1364
		private int _layer;

		// Token: 0x04000555 RID: 1365
		private float _distanceCtr;

		// Token: 0x04000556 RID: 1366
		private Vector2 _lastPos;

		// Token: 0x04000557 RID: 1367
		private Vector2 _currPos;

		// Token: 0x04000558 RID: 1368
		private IEnumerator _shootSalveCR;
	}
}
