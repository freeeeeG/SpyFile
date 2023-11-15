using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200008D RID: 141
	public class DisableOnCollision : MonoBehaviour
	{
		// Token: 0x06000536 RID: 1334 RVA: 0x000195FC File Offset: 0x000177FC
		private void OnCollisionEnter2D(Collision2D collision)
		{
			base.gameObject.SetActive(false);
		}
	}
}
