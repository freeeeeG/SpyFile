using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200012A RID: 298
	public class ShootingSummon : AttackingSummon
	{
		// Token: 0x0600080D RID: 2061 RVA: 0x000220B4 File Offset: 0x000202B4
		private void OnHit(object sender, object args)
		{
			GameObject e = args as GameObject;
			this.PostNotification(Summon.SummonOnHitNotification, e);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000220D4 File Offset: 0x000202D4
		protected override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.projectilePrefab.name, this.projectilePrefab, 50, true);
			this.SC = ShootingCursor.Instance;
			this.AddObserver(new Action<object, object>(this.OnHit), Projectile.ImpactEvent, base.gameObject);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00022133 File Offset: 0x00020333
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnHit), Projectile.ImpactEvent, base.gameObject);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00022154 File Offset: 0x00020354
		protected override bool Attack()
		{
			Vector2 vector = base.transform.position;
			Vector2 zero = Vector2.zero;
			if (this.targetMouse)
			{
				Vector2 direction = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition) - vector;
				this.Shoot(direction);
				return true;
			}
			if (EnemyFinder.GetRandomEnemy(vector, new Vector2(9f, 6f)) != null)
			{
				Vector2 direction2 = EnemyFinder.GetRandomEnemy(vector, new Vector2(9f, 6f)).transform.position - vector;
				this.Shoot(direction2);
				return true;
			}
			return false;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00022204 File Offset: 0x00020404
		private void Shoot(Vector2 direction)
		{
			if (direction.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (direction.x > 0f)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			this.shooter.Shoot(this.GetProjectileRecipe(), direction, this.numProjectiles, (float)((this.numProjectiles - 1) * 15), 0f);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00022294 File Offset: 0x00020494
		private ProjectileRecipe GetProjectileRecipe()
		{
			ProjectileRecipe projectileRecipe = new ProjectileRecipe();
			projectileRecipe.objectPoolTag = this.projectilePrefab.name;
			float f;
			if (this.inheritPlayerDamage)
			{
				f = base.summonDamageMod.Modify(this.player.gun.damage);
			}
			else
			{
				f = base.summonDamageMod.Modify((float)this.baseDamage);
			}
			projectileRecipe.damage = (float)base.ApplyDamageMods(Mathf.FloorToInt(f));
			projectileRecipe.projectileSpeed = this.projectileSpeed;
			projectileRecipe.size = 1f;
			projectileRecipe.knockback = this.knockback;
			projectileRecipe.bounce = this.bounce;
			projectileRecipe.piercing = this.pierce;
			projectileRecipe.owner = base.gameObject;
			return projectileRecipe;
		}

		// Token: 0x040005D7 RID: 1495
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x040005D8 RID: 1496
		[SerializeField]
		private Shooter shooter;

		// Token: 0x040005D9 RID: 1497
		public bool targetMouse;

		// Token: 0x040005DA RID: 1498
		public bool inheritPlayerDamage;

		// Token: 0x040005DB RID: 1499
		public int baseDamage;

		// Token: 0x040005DC RID: 1500
		public int numProjectiles;

		// Token: 0x040005DD RID: 1501
		public float projectileSpeed;

		// Token: 0x040005DE RID: 1502
		public float knockback;

		// Token: 0x040005DF RID: 1503
		public int bounce;

		// Token: 0x040005E0 RID: 1504
		public int pierce;

		// Token: 0x040005E1 RID: 1505
		private ObjectPooler OP;

		// Token: 0x040005E2 RID: 1506
		private ShootingCursor SC;
	}
}
