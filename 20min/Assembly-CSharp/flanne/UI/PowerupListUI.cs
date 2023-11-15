using System;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200020E RID: 526
	public class PowerupListUI : MonoBehaviour
	{
		// Token: 0x06000BD7 RID: 3031 RVA: 0x0002C170 File Offset: 0x0002A370
		private void OnPowerupApplied(object sender, object args)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.powerupIconPrefab);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localScale = Vector3.one;
			Powerup data = sender as Powerup;
			gameObject.GetComponent<PowerupIcon>().data = data;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002C1BB File Offset: 0x0002A3BB
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnPowerupApplied), Powerup.AppliedNotifcation);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0002C1D4 File Offset: 0x0002A3D4
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnPowerupApplied), Powerup.AppliedNotifcation);
		}

		// Token: 0x04000846 RID: 2118
		[SerializeField]
		private GameObject powerupIconPrefab;
	}
}
