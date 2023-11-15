using System;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000202 RID: 514
	public class DespawnRange : MonoBehaviour
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x0002B3CC File Offset: 0x000295CC
		private void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains("Enemy") && !other.gameObject.tag.Contains("Champion") && !other.gameObject.tag.Contains("Passive"))
			{
				other.gameObject.SetActive(false);
			}
		}
	}
}
