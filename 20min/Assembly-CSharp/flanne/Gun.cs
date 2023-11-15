using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using flanne.Core;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000B8 RID: 184
	public class Gun : MonoBehaviour
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001B734 File Offset: 0x00019934
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x0001B73C File Offset: 0x0001993C
		public StatsHolder stats { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001B745 File Offset: 0x00019945
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x0001B74D File Offset: 0x0001994D
		public GunData gunData { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001B756 File Offset: 0x00019956
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001B75E File Offset: 0x0001995E
		public List<Animator> gunAnimators { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001B767 File Offset: 0x00019967
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001B76F File Offset: 0x0001996F
		public List<SpriteRenderer> gunSprites { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001B778 File Offset: 0x00019978
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0001B780 File Offset: 0x00019980
		public List<Shooter> shooters { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001B789 File Offset: 0x00019989
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0001B791 File Offset: 0x00019991
		public GameObject gunObj { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001B79A File Offset: 0x0001999A
		public float shotCooldown
		{
			get
			{
				return this.stats[StatType.FireRate].ModifyInverse(this.gunData.shotCooldown);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001B7B8 File Offset: 0x000199B8
		public float reloadDuration
		{
			get
			{
				return this.stats[StatType.ReloadRate].ModifyInverse(this.gunData.reloadDuration);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001B7D6 File Offset: 0x000199D6
		public float damage
		{
			get
			{
				return this.stats[StatType.BulletDamage].Modify(this.gunData.damage);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001B7F4 File Offset: 0x000199F4
		public int numOfProjectiles
		{
			get
			{
				return Mathf.Max(1, (int)this.stats[StatType.Projectiles].Modify((float)this.gunData.numOfProjectiles));
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001B81A File Offset: 0x00019A1A
		public float spread
		{
			get
			{
				return this.stats[StatType.Spread].Modify(this.gunData.spread);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001B839 File Offset: 0x00019A39
		public int maxAmmo
		{
			get
			{
				return Mathf.Max(1, (int)this.stats[StatType.MaxAmmo].Modify((float)this.gunData.maxAmmo));
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001B85F File Offset: 0x00019A5F
		public bool shotReady
		{
			get
			{
				return this._shotTimer <= 0f;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0001B871 File Offset: 0x00019A71
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x0001B879 File Offset: 0x00019A79
		public bool isShooting { get; private set; }

		// Token: 0x060005FA RID: 1530 RVA: 0x0001B882 File Offset: 0x00019A82
		private void Start()
		{
			this.isShooting = false;
			this.stats = this.player.stats;
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001B8A8 File Offset: 0x00019AA8
		private void Update()
		{
			if (!PauseController.isPaused)
			{
				if (this._shotTimer > 0f)
				{
					this._shotTimer -= Time.deltaTime;
					return;
				}
				if (this.isShooting)
				{
					this.SetAnimationTrigger("Attack");
					if (this.gunData.isSummonGun)
					{
						this._shotTimer += this.stats[StatType.SummonAttackSpeed].ModifyInverse(this.shotCooldown);
					}
					else
					{
						this._shotTimer += this.shotCooldown;
					}
					Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
					Vector2 b = base.transform.position;
					Vector2 pointDirection = a - b;
					foreach (Shooter shooter in this.shooters)
					{
						ProjectileRecipe projectileRecipe = this.GetProjectileRecipe();
						this.PostNotification(Gun.ShootEvent, projectileRecipe);
						shooter.Shoot(projectileRecipe, pointDirection, this.numOfProjectiles, this.spread, this.gunData.inaccuracy);
					}
					if (!this.shooters[0].fireOnStop)
					{
						SoundEffectSO gunshotSFX = this.gunData.gunshotSFX;
						if (gunshotSFX == null)
						{
							return;
						}
						gunshotSFX.Play(null);
					}
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001BA0C File Offset: 0x00019C0C
		public void LoadGun(GunData gunToLoad)
		{
			if (gunToLoad == null)
			{
				this.gunData = this.defaultGun;
			}
			else
			{
				this.gunData = gunToLoad;
			}
			if (this.gunObj != null)
			{
				foreach (Shooter shooter in this.shooters)
				{
					shooter.onShoot.RemoveAllListeners();
				}
				this.shooters = new List<Shooter>();
				this.gunAnimators = new List<Animator>();
				this.gunSprites = new List<SpriteRenderer>();
				Object.Destroy(this.gunObj);
			}
			this.gunObj = Object.Instantiate<GameObject>(this.gunData.model);
			this.gunObj.transform.SetParent(base.transform);
			this.gunObj.transform.localPosition = Vector3.zero;
			this.gunAnimators = this.gunObj.GetComponentsInChildren<Animator>().ToList<Animator>();
			if (this.gunAnimators == null)
			{
				Debug.LogError(this.gunObj + "is missing an animator.");
			}
			this.gunSprites = new List<SpriteRenderer>();
			foreach (Animator animator in this.gunAnimators)
			{
				this.gunSprites.AddRange(new List<SpriteRenderer>(animator.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList<SpriteRenderer>()));
			}
			this.shooters = this.gunObj.GetComponentsInChildren<Shooter>().ToList<Shooter>();
			if (this.shooters == null)
			{
				Debug.LogError(this.gunObj + "is missing an shooter.");
			}
			foreach (Shooter shooter2 in this.shooters)
			{
				shooter2.onShoot.AddListener(new UnityAction(this.OnShooterShoot));
			}
			ObjectPooler.SharedInstance.AddObject(this.gunData.bulletOPTag, this.gunData.bullet, 5000, true);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001BC3C File Offset: 0x00019E3C
		public void AddShooter(Shooter shooter)
		{
			this.shooters.Add(shooter);
			Animator componentInChildren = shooter.GetComponentInChildren<Animator>();
			this.gunAnimators.Add(componentInChildren);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001BC68 File Offset: 0x00019E68
		public void SetAnimationTrigger(string trigger)
		{
			foreach (Animator animator in this.gunAnimators)
			{
				animator.SetTrigger(trigger);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001BCBC File Offset: 0x00019EBC
		public void SetVisible(bool visible)
		{
			foreach (SpriteRenderer spriteRenderer in this.gunSprites)
			{
				spriteRenderer.enabled = visible;
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001BD10 File Offset: 0x00019F10
		public void StartShooting()
		{
			base.StartCoroutine(this.WaitToStartShootCR());
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001BD20 File Offset: 0x00019F20
		public void StopShooting()
		{
			if (!this.isShooting)
			{
				return;
			}
			this.isShooting = false;
			foreach (Shooter shooter in this.shooters)
			{
				Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
				Vector2 b = base.transform.position;
				Vector2 pointDirection = a - b;
				shooter.OnStopShoot(this.GetProjectileRecipe(), pointDirection, this.numOfProjectiles, this.spread, this.gunData.inaccuracy);
			}
			if (this.shooters[0].fireOnStop)
			{
				SoundEffectSO gunshotSFX = this.gunData.gunshotSFX;
				if (gunshotSFX == null)
				{
					return;
				}
				gunshotSFX.Play(null);
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001BE00 File Offset: 0x0001A000
		public ProjectileRecipe GetProjectileRecipe()
		{
			ProjectileRecipe projectileRecipe = new ProjectileRecipe();
			projectileRecipe.objectPoolTag = this.gunData.bulletOPTag;
			if (this.gunData.isSummonGun)
			{
				projectileRecipe.damage = this.stats[StatType.SummonDamage].Modify(this.stats[StatType.BulletDamage].Modify(this.gunData.damage));
			}
			else
			{
				projectileRecipe.damage = this.stats[StatType.BulletDamage].Modify(this.gunData.damage);
			}
			projectileRecipe.projectileSpeed = this.stats[StatType.ProjectileSpeed].Modify(this.gunData.projectileSpeed);
			projectileRecipe.size = this.stats[StatType.ProjectileSize].Modify(1f);
			projectileRecipe.knockback = this.stats[StatType.Knockback].Modify(this.gunData.knockback);
			projectileRecipe.bounce = Mathf.Max(0, (int)this.stats[StatType.Bounce].Modify((float)this.gunData.bounce));
			projectileRecipe.piercing = Mathf.Max(0, (int)this.stats[StatType.Piercing].Modify((float)this.gunData.piercing));
			projectileRecipe.owner = this.player.gameObject;
			return projectileRecipe;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001BF51 File Offset: 0x0001A151
		public void PlayGunEvoAnimation()
		{
			this.gunEvoAnimator.SetTrigger("Start");
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001BF63 File Offset: 0x0001A163
		public void EndGunEvoAnimation()
		{
			this.gunEvoAnimator.SetTrigger("End");
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001BF75 File Offset: 0x0001A175
		private IEnumerator WaitToStartShootCR()
		{
			yield return null;
			this.isShooting = true;
			yield break;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001BF84 File Offset: 0x0001A184
		private void OnShooterShoot()
		{
			this.OnShoot.Invoke();
		}

		// Token: 0x040003BD RID: 957
		public static string ShootEvent = "Gun.ShootEvent";

		// Token: 0x040003BE RID: 958
		[SerializeField]
		private PlayerController player;

		// Token: 0x040003BF RID: 959
		[SerializeField]
		private GunData defaultGun;

		// Token: 0x040003C0 RID: 960
		[SerializeField]
		private Animator gunEvoAnimator;

		// Token: 0x040003C3 RID: 963
		public UnityEvent OnShoot;

		// Token: 0x040003C9 RID: 969
		private ShootingCursor SC;

		// Token: 0x040003CA RID: 970
		private IEnumerator _reloadCoroutine;

		// Token: 0x040003CB RID: 971
		private float _shotTimer;
	}
}
