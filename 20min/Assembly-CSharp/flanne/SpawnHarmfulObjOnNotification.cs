using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000114 RID: 276
	public class SpawnHarmfulObjOnNotification : MonoBehaviour
	{
		// Token: 0x060007B1 RID: 1969 RVA: 0x000211D4 File Offset: 0x0001F3D4
		private void OnNotification(object sender, object args)
		{
			GameObject gameObject = args as GameObject;
			this.Spawn(gameObject.transform.position);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000211FC File Offset: 0x0001F3FC
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.prefab.name, this.prefab, this.amountToInitInObjPool, true);
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.gun = componentInParent.gun;
			this.AddObserver(new Action<object, object>(this.OnNotification), this.notification);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00021262 File Offset: 0x0001F462
		private void OnDisable()
		{
			this.RemoveObserver(new Action<object, object>(this.OnNotification), this.notification);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0002127C File Offset: 0x0001F47C
		public void Spawn(Vector3 spawnPos)
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.prefab.name);
			pooledObject.transform.position = spawnPos;
			pooledObject.GetComponent<Harmful>().damageAmount = Mathf.FloorToInt(this.gun.damage * this.damageMultiplier);
			pooledObject.SetActive(true);
		}

		// Token: 0x04000584 RID: 1412
		[SerializeField]
		private string notification;

		// Token: 0x04000585 RID: 1413
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000586 RID: 1414
		[SerializeField]
		private int amountToInitInObjPool;

		// Token: 0x04000587 RID: 1415
		[SerializeField]
		private float damageMultiplier;

		// Token: 0x04000588 RID: 1416
		private ObjectPooler OP;

		// Token: 0x04000589 RID: 1417
		private Gun gun;
	}
}
