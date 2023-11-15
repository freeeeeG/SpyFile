using System;
using UnityEngine;

namespace Helios.GUI
{
	// Token: 0x020000E1 RID: 225
	public class ShowFxWhenClicked : MonoBehaviour
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000E8B1 File Offset: 0x0000CAB1
		private void Start()
		{
			this.particles = base.gameObject.transform.GetComponentsInChildren<ParticleSystem>();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E8CC File Offset: 0x0000CACC
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				this.particles[this.indexParticle].transform.position = new Vector3(this.mousePos.x, this.mousePos.y, 0f);
				this.particles[this.indexParticle].Play();
				this.indexParticle++;
				if (this.indexParticle >= this.particles.Length)
				{
					this.indexParticle = 0;
				}
			}
		}

		// Token: 0x04000317 RID: 791
		private ParticleSystem[] particles;

		// Token: 0x04000318 RID: 792
		private Vector2 mousePos;

		// Token: 0x04000319 RID: 793
		private int indexParticle;
	}
}
