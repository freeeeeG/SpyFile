using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D5 RID: 469
	public class ThrowProjectileTowardsCursorAction : Action
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x000286F4 File Offset: 0x000268F4
		public override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.projectilePrefab.name, this.projectilePrefab, 30, true);
			this.SC = ShootingCursor.Instance;
			this.player = PlayerController.Instance;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00028744 File Offset: 0x00026944
		public override void Activate(GameObject target)
		{
			Vector2 directionToCursor = this.GetDirectionToCursor();
			float num = -1f * this.inaccuracy / 2f;
			float max = -1f * num;
			if (this.numProjectiles > 1)
			{
				for (int i = 0; i < this.numProjectiles; i++)
				{
					float degrees = num + (float)i / (float)(this.numProjectiles - 1) * this.inaccuracy;
					Vector2 direction = directionToCursor.Rotate(degrees);
					this.ThrowProjectileTowards(direction);
				}
				return;
			}
			Vector2 direction2 = directionToCursor.Rotate(Random.Range(num, max));
			this.ThrowProjectileTowards(direction2);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000287D0 File Offset: 0x000269D0
		private Vector2 GetDirectionToCursor()
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = PlayerController.Instance.transform.position;
			return a - b;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00028818 File Offset: 0x00026A18
		private void ThrowProjectileTowards(Vector2 direction)
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.projectilePrefab.name);
			pooledObject.SetActive(true);
			Projectile component = pooledObject.GetComponent<Projectile>();
			component.vector = this.speed * direction.normalized;
			if (!this.lockRotation)
			{
				component.angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			}
			Vector2 vector = direction.normalized * this.spawnOffset;
			pooledObject.transform.position = this.player.transform.position + new Vector3(vector.x, vector.y, 0f);
		}

		// Token: 0x04000761 RID: 1889
		[SerializeField]
		private GameObject projectilePrefab;

		// Token: 0x04000762 RID: 1890
		[SerializeField]
		private int numProjectiles = 1;

		// Token: 0x04000763 RID: 1891
		[SerializeField]
		private float speed = 20f;

		// Token: 0x04000764 RID: 1892
		[SerializeField]
		private float inaccuracy = 45f;

		// Token: 0x04000765 RID: 1893
		[SerializeField]
		private float spawnOffset = 0.7f;

		// Token: 0x04000766 RID: 1894
		[SerializeField]
		private bool lockRotation;

		// Token: 0x04000767 RID: 1895
		[NonSerialized]
		private ObjectPooler OP;

		// Token: 0x04000768 RID: 1896
		[NonSerialized]
		private ShootingCursor SC;

		// Token: 0x04000769 RID: 1897
		[NonSerialized]
		private PlayerController player;
	}
}
