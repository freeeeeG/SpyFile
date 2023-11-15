using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D1 RID: 465
	public class SpawnPrefabAction : Action
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x000283B9 File Offset: 0x000265B9
		public override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, 30, true);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000283E8 File Offset: 0x000265E8
		public override void Activate(GameObject target)
		{
			for (int i = 0; i < this.amountToSpawn; i++)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
				pooledObject.transform.position = target.transform.position;
				if (this.randomizeRotation)
				{
					pooledObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(this.minAngle, this.maxAngle));
				}
				pooledObject.SetActive(true);
			}
		}

		// Token: 0x0400074E RID: 1870
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400074F RID: 1871
		[SerializeField]
		private int amountToSpawn = 1;

		// Token: 0x04000750 RID: 1872
		[SerializeField]
		private bool randomizeRotation;

		// Token: 0x04000751 RID: 1873
		[SerializeField]
		private float maxAngle;

		// Token: 0x04000752 RID: 1874
		[SerializeField]
		private float minAngle;

		// Token: 0x04000753 RID: 1875
		[NonSerialized]
		private ObjectPooler OP;
	}
}
