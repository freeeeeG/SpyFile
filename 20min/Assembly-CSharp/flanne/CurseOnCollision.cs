using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000DA RID: 218
	public class CurseOnCollision : MonoBehaviour
	{
		// Token: 0x06000697 RID: 1687 RVA: 0x0001DC7D File Offset: 0x0001BE7D
		private void Start()
		{
			this.CS = CurseSystem.Instance;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001DC8A File Offset: 0x0001BE8A
		private void OnCollisionEnter2D(Collision2D other)
		{
			this.CS.Curse(other.gameObject);
		}

		// Token: 0x04000469 RID: 1129
		private CurseSystem CS;
	}
}
