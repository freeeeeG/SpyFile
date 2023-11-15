using System;
using CameraShake;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000A8 RID: 168
	public class ExplosionShake2D : MonoBehaviour
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x0001AD7B File Offset: 0x00018F7B
		public void Shake()
		{
			CameraShaker.Presets.Explosion2D(this.positionStrength, this.rotationStrength, 0.5f);
		}

		// Token: 0x04000385 RID: 901
		[SerializeField]
		private float positionStrength;

		// Token: 0x04000386 RID: 902
		[SerializeField]
		private float rotationStrength;
	}
}
