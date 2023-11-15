using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000103 RID: 259
	public class ProjectileFactory : MonoBehaviour
	{
		// Token: 0x0600076F RID: 1903 RVA: 0x0002058F File Offset: 0x0001E78F
		private void Awake()
		{
			ProjectileFactory.SharedInstance = this;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00020597 File Offset: 0x0001E797
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000205A4 File Offset: 0x0001E7A4
		public Projectile SpawnProjectile(ProjectileRecipe recipe, Vector2 direction, Vector3 position, float damageMultiplier = 1f, bool isSecondary = false)
		{
			GameObject pooledObject = this.OP.GetPooledObject(recipe.objectPoolTag);
			pooledObject.SetActive(true);
			pooledObject.transform.position = position;
			Projectile component = pooledObject.GetComponent<Projectile>();
			component.isSecondary = isSecondary;
			component.vector = recipe.projectileSpeed * direction.normalized;
			component.angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			component.size = recipe.size;
			component.damage = Mathf.Max(1f, recipe.damage * damageMultiplier);
			component.knockback = recipe.knockback;
			component.bounce = recipe.bounce;
			component.piercing = recipe.piercing;
			component.owner = recipe.owner;
			return component;
		}

		// Token: 0x04000540 RID: 1344
		public static ProjectileFactory SharedInstance;

		// Token: 0x04000541 RID: 1345
		private ObjectPooler OP;
	}
}
