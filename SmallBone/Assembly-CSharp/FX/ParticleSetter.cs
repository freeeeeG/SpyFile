using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000248 RID: 584
	public class ParticleSetter : MonoBehaviour
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x0001FA07 File Offset: 0x0001DC07
		private void Awake()
		{
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x0400098E RID: 2446
		[SerializeField]
		private ParticleSystem _effect;

		// Token: 0x0400098F RID: 2447
		[SerializeField]
		private BoxCollider2D _range;

		// Token: 0x04000990 RID: 2448
		[SerializeField]
		private float _emmitPerTile;
	}
}
