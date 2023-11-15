using System;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000C5 RID: 197
	public class Message : MonoBehaviour
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0000D564 File Offset: 0x0000B764
		public void Activate()
		{
			this._spriteRenderer.enabled = true;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000D572 File Offset: 0x0000B772
		public void Deactivate()
		{
			this._spriteRenderer.enabled = false;
		}

		// Token: 0x040002FA RID: 762
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
	}
}
