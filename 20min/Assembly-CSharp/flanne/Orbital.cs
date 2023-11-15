using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000AC RID: 172
	public class Orbital : MonoBehaviour
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001AF04 File Offset: 0x00019104
		private void Start()
		{
			if (this.center == null)
			{
				this.center = base.transform.parent;
			}
			base.transform.position = (base.transform.position - this.center.position).normalized * this.radius + this.center.position;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001AF7C File Offset: 0x0001917C
		private void Update()
		{
			base.transform.RotateAround(this.center.position, this.axis, this.rotationSpeed * Time.deltaTime);
			Vector3 target = (base.transform.position - this.center.position).normalized * this.radius + this.center.position;
			base.transform.position = Vector3.MoveTowards(base.transform.position, target, Time.deltaTime * this.radiusSpeed);
			if (this.dontRotate)
			{
				base.transform.rotation = Quaternion.identity;
			}
		}

		// Token: 0x04000392 RID: 914
		public Transform center;

		// Token: 0x04000393 RID: 915
		public Vector3 axis = Vector3.up;

		// Token: 0x04000394 RID: 916
		public float radius = 2f;

		// Token: 0x04000395 RID: 917
		public float radiusSpeed = 0.5f;

		// Token: 0x04000396 RID: 918
		public float rotationSpeed = 80f;

		// Token: 0x04000397 RID: 919
		public bool dontRotate;
	}
}
