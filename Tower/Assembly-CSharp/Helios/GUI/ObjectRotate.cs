using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000DD RID: 221
	public class ObjectRotate : MonoBehaviour
	{
		// Token: 0x06000336 RID: 822 RVA: 0x0000E7A6 File Offset: 0x0000C9A6
		private void Awake()
		{
			this._transform = base.transform;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
		private void LateUpdate()
		{
			this._transform.Rotate(this.Rotation);
		}

		// Token: 0x04000312 RID: 786
		public Vector3 Rotation = Vector3.one;

		// Token: 0x04000313 RID: 787
		private Transform _transform;
	}
}
