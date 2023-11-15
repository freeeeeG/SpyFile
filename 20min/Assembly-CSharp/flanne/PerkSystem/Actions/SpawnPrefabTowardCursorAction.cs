using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D2 RID: 466
	public class SpawnPrefabTowardCursorAction : Action
	{
		// Token: 0x06000A56 RID: 2646 RVA: 0x0002847C File Offset: 0x0002667C
		public override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, 30, true);
			this.player = PlayerController.Instance;
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x000284CC File Offset: 0x000266CC
		public override void Activate(GameObject target)
		{
			for (int i = 0; i < this.amountToSpawn; i++)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
				Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
				Vector2 vector = this.player.transform.position;
				Vector2 a2 = a - vector;
				Vector2 v = vector + a2 * this.distance;
				pooledObject.transform.position = v;
				if (this.randomizeRotation)
				{
					pooledObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(this.minAngle, this.maxAngle));
				}
				pooledObject.SetActive(true);
			}
		}

		// Token: 0x04000754 RID: 1876
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000755 RID: 1877
		[SerializeField]
		private int amountToSpawn = 1;

		// Token: 0x04000756 RID: 1878
		[SerializeField]
		private float distance;

		// Token: 0x04000757 RID: 1879
		[SerializeField]
		private bool randomizeRotation;

		// Token: 0x04000758 RID: 1880
		[SerializeField]
		private float maxAngle;

		// Token: 0x04000759 RID: 1881
		[SerializeField]
		private float minAngle;

		// Token: 0x0400075A RID: 1882
		[NonSerialized]
		private ObjectPooler OP;

		// Token: 0x0400075B RID: 1883
		private PlayerController player;

		// Token: 0x0400075C RID: 1884
		private ShootingCursor SC;
	}
}
