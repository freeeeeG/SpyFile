using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000102 RID: 258
	public class Projectile : MonoBehaviour
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00020296 File Offset: 0x0001E496
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x0002029F File Offset: 0x0001E49F
		public virtual float damage
		{
			get
			{
				return (float)this._damage;
			}
			set
			{
				this._damage = Mathf.FloorToInt(value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x000202AD File Offset: 0x0001E4AD
		public float knockback
		{
			set
			{
				this.kb.knockbackForce = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x000202BB File Offset: 0x0001E4BB
		public float angle
		{
			set
			{
				this.rb.rotation = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x000202C9 File Offset: 0x0001E4C9
		public float size
		{
			set
			{
				this.SetSize(Mathf.Clamp(value, 0f, 5f));
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x000202E1 File Offset: 0x0001E4E1
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x000202EE File Offset: 0x0001E4EE
		public Vector2 vector
		{
			get
			{
				return this.move.vector;
			}
			set
			{
				this.move.vector = value;
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000202FC File Offset: 0x0001E4FC
		protected virtual void OnCollisionEnter2D(Collision2D other)
		{
			Health component = other.gameObject.GetComponent<Health>();
			if (component == null)
			{
				return;
			}
			component.TakeDamage(DamageType.Bullet, this._damage, 1f);
			if (component.isDead)
			{
				this.PostNotification(Projectile.KillEvent);
			}
			this.owner.PostNotification(Projectile.ImpactEvent, other.gameObject);
			this.PostNotification(Projectile.TweakPierceBounce);
			if (this.piercing != 0)
			{
				this.piercing--;
				return;
			}
			if (this.bounce == 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.bounce--;
			AIComponent component2 = other.gameObject.GetComponent<AIComponent>();
			this.BounceOffEnemy(component2);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000203B2 File Offset: 0x0001E5B2
		protected virtual void SetSize(float size)
		{
			base.transform.localScale = size * Vector3.one;
			if (this.trail != null)
			{
				this.trail.widthMultiplier = size;
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000203E4 File Offset: 0x0001E5E4
		protected void BounceOffEnemy(AIComponent enemy)
		{
			this.PostNotification(Projectile.BounceEvent, enemy.gameObject);
			if (enemy != null)
			{
				Vector2 vector = enemy.transform.position;
				GameObject closestEnemy = EnemyFinder.GetClosestEnemy(vector, enemy);
				Transform transform = (closestEnemy != null) ? closestEnemy.transform : null;
				if (transform != null)
				{
					this.move.vector = (new Vector2(transform.position.x, transform.position.y) - vector).normalized * this.move.vector.magnitude;
					if (!this.dontRotateOnBounce)
					{
						this.angle = Mathf.Atan2(this.move.vector.y, this.move.vector.x) * 57.29578f;
					}
					return;
				}
			}
			float magnitude = this.move.vector.magnitude;
			Vector2 v = Vector2.Reflect(base.transform.right, this.move.vector).normalized * magnitude;
			v = v.Rotate((float)Random.Range(-45, 45));
			this.move.vector = v.normalized * magnitude;
			this.angle = Mathf.Atan2(this.move.vector.y, this.move.vector.x) * 57.29578f;
		}

		// Token: 0x04000532 RID: 1330
		public static string ImpactEvent = "Projectile.ImpactEvent";

		// Token: 0x04000533 RID: 1331
		public static string KillEvent = "Projectile.KillEvent";

		// Token: 0x04000534 RID: 1332
		public static string TweakPierceBounce = "Projectile.TweakPierceBounce";

		// Token: 0x04000535 RID: 1333
		public static string BounceEvent = "Projectile.BounceEvent";

		// Token: 0x04000536 RID: 1334
		[SerializeField]
		protected Rigidbody2D rb;

		// Token: 0x04000537 RID: 1335
		[SerializeField]
		protected Knockback kb;

		// Token: 0x04000538 RID: 1336
		[SerializeField]
		protected MoveComponent2D move;

		// Token: 0x04000539 RID: 1337
		[SerializeField]
		protected TrailRenderer trail;

		// Token: 0x0400053A RID: 1338
		public int bounce;

		// Token: 0x0400053B RID: 1339
		public int piercing;

		// Token: 0x0400053C RID: 1340
		public bool dontRotateOnBounce;

		// Token: 0x0400053D RID: 1341
		[NonSerialized]
		public bool isSecondary;

		// Token: 0x0400053E RID: 1342
		[NonSerialized]
		public GameObject owner;

		// Token: 0x0400053F RID: 1343
		private int _damage;
	}
}
