using System;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000C7 RID: 199
	public sealed class SimpleTalker : NPC
	{
		// Token: 0x060003DA RID: 986 RVA: 0x0000D5FF File Offset: 0x0000B7FF
		protected override void Activate()
		{
			this._spriteRenderer.enabled = true;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Deactivate()
		{
		}

		// Token: 0x040002FC RID: 764
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
	}
}
