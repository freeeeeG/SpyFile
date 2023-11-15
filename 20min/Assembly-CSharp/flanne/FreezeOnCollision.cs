using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E3 RID: 227
	public class FreezeOnCollision : MonoBehaviour
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x0001E357 File Offset: 0x0001C557
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				FreezeSystem.SharedInstance.Freeze(other.gameObject);
			}
		}

		// Token: 0x04000489 RID: 1161
		[SerializeField]
		private string hitTag;
	}
}
