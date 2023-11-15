using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200048E RID: 1166
	public class Block : MonoBehaviour
	{
		// Token: 0x06001633 RID: 5683 RVA: 0x00045940 File Offset: 0x00043B40
		public void Activate()
		{
			this._collider2D.enabled = true;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0004594E File Offset: 0x00043B4E
		public void Deactivate()
		{
			this._collider2D.enabled = false;
		}

		// Token: 0x04001373 RID: 4979
		[GetComponent]
		[SerializeField]
		private Collider2D _collider2D;
	}
}
