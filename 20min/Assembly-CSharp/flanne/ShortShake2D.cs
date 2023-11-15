using System;
using CameraShake;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000AF RID: 175
	public class ShortShake2D : MonoBehaviour
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B199 File Offset: 0x00019399
		public void Shake()
		{
			CameraShaker.Presets.ShortShake2D(this.positionStrength, this.rotationStrength, 25f, 5);
		}

		// Token: 0x0400039D RID: 925
		[SerializeField]
		private float positionStrength;

		// Token: 0x0400039E RID: 926
		[SerializeField]
		private float rotationStrength;
	}
}
