using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F0 RID: 240
	public class PeriodicGameObjectActivator : MonoBehaviour
	{
		// Token: 0x060006F3 RID: 1779 RVA: 0x0001EAE2 File Offset: 0x0001CCE2
		private void Start()
		{
			base.InvokeRepeating("ActivateObject", this.timeBetweenActivations, this.timeBetweenActivations);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001EAFB File Offset: 0x0001CCFB
		private void ActivateObject()
		{
			this.obj.SetActive(true);
		}

		// Token: 0x040004BA RID: 1210
		[SerializeField]
		private GameObject obj;

		// Token: 0x040004BB RID: 1211
		public float timeBetweenActivations;
	}
}
