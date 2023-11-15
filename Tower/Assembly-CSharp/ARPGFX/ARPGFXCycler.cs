using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x0200006B RID: 107
	public class ARPGFXCycler : MonoBehaviour
	{
		// Token: 0x06000183 RID: 387 RVA: 0x000071A4 File Offset: 0x000053A4
		private void Start()
		{
			this.instantiatedEffect = Object.Instantiate<GameObject>(this.listOfEffects[this.effectIndex], base.transform.position, base.transform.rotation);
			this.effectIndex++;
			this.timeOfLastInstantiate = Time.time;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000071FC File Offset: 0x000053FC
		private void Update()
		{
			if (Time.time >= this.timeOfLastInstantiate + this.loopTimeLength)
			{
				Object.Destroy(this.instantiatedEffect);
				this.instantiatedEffect = Object.Instantiate<GameObject>(this.listOfEffects[this.effectIndex], base.transform.position, base.transform.rotation);
				this.timeOfLastInstantiate = Time.time;
				if (this.effectIndex < this.listOfEffects.Count - 1)
				{
					this.effectIndex++;
					return;
				}
				this.effectIndex = 0;
			}
		}

		// Token: 0x0400017A RID: 378
		[SerializeField]
		private List<GameObject> listOfEffects;

		// Token: 0x0400017B RID: 379
		[Header("Loop length in seconds")]
		[SerializeField]
		private float loopTimeLength = 5f;

		// Token: 0x0400017C RID: 380
		private float timeOfLastInstantiate;

		// Token: 0x0400017D RID: 381
		private GameObject instantiatedEffect;

		// Token: 0x0400017E RID: 382
		private int effectIndex;
	}
}
