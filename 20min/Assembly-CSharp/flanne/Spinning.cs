using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B1 RID: 177
	public class Spinning : MonoBehaviour
	{
		// Token: 0x060005BE RID: 1470 RVA: 0x0001B245 File Offset: 0x00019445
		private void Update()
		{
			base.transform.Rotate(this.spin.x, this.spin.y, this.spin.z * Time.deltaTime);
		}

		// Token: 0x040003A3 RID: 931
		[SerializeField]
		private Vector3 spin = Vector3.zero;
	}
}
