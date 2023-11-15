using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200024F RID: 591
	public class Rotate : MonoBehaviour
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x0001FF96 File Offset: 0x0001E196
		private void Update()
		{
			base.transform.Rotate(Vector3.forward, this._amount * Chronometer.global.deltaTime, Space.Self);
		}

		// Token: 0x040009A4 RID: 2468
		[SerializeField]
		private float _amount;
	}
}
