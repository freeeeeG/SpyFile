using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B0 RID: 176
	public class SpawnOnCollision : MonoBehaviour
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x0001B1B7 File Offset: 0x000193B7
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001B1C4 File Offset: 0x000193C4
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				for (int i = 0; i < other.contacts.Length; i++)
				{
					this.lastImpactFX = this.OP.GetPooledObject(this.objPoolTag);
					this.lastImpactFX.SetActive(true);
					this.lastImpactFX.transform.position = other.contacts[i].point;
				}
			}
		}

		// Token: 0x0400039F RID: 927
		[SerializeField]
		private string hitTag;

		// Token: 0x040003A0 RID: 928
		[SerializeField]
		private string objPoolTag;

		// Token: 0x040003A1 RID: 929
		private GameObject lastImpactFX;

		// Token: 0x040003A2 RID: 930
		private ObjectPooler OP;
	}
}
