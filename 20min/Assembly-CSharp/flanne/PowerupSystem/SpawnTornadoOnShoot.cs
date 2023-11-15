using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000253 RID: 595
	public class SpawnTornadoOnShoot : AttackOnShoot
	{
		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002F274 File Offset: 0x0002D474
		protected override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.tornadoPrefab.name, this.tornadoPrefab, 100, true);
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002F2AC File Offset: 0x0002D4AC
		public override void Attack()
		{
			for (int i = 0; i < this.numTornadoes; i++)
			{
				this.SpawnTornado();
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002F2D0 File Offset: 0x0002D4D0
		private void SpawnTornado()
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 b = base.transform.position;
			Vector2 normalized = (a - b).normalized;
			GameObject pooledObject = this.OP.GetPooledObject(this.tornadoPrefab.name);
			pooledObject.SetActive(true);
			Vector2 vector = normalized.normalized * 1.2f;
			if (this.fireBackwards)
			{
				vector *= -1f;
			}
			pooledObject.transform.position = base.transform.position + new Vector3(vector.x, vector.y, 0f);
			float num = -1f * this.inaccuracy / 2f;
			float max = -1f * num;
			Vector2 a2 = normalized.Rotate(Random.Range(num, max));
			if (this.fireBackwards)
			{
				a2 *= -1f;
			}
			pooledObject.GetComponent<MoveComponent2D>().vector = this.tornadoSpeed * a2;
		}

		// Token: 0x04000933 RID: 2355
		[SerializeField]
		private int numTornadoes;

		// Token: 0x04000934 RID: 2356
		[SerializeField]
		private GameObject tornadoPrefab;

		// Token: 0x04000935 RID: 2357
		[SerializeField]
		private float tornadoSpeed = 20f;

		// Token: 0x04000936 RID: 2358
		[SerializeField]
		private float inaccuracy = 65f;

		// Token: 0x04000937 RID: 2359
		[SerializeField]
		private bool fireBackwards;

		// Token: 0x04000938 RID: 2360
		private ShootingCursor SC;

		// Token: 0x04000939 RID: 2361
		private ObjectPooler OP;
	}
}
