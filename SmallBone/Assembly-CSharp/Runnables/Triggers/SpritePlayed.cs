using System;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000367 RID: 871
	public class SpritePlayed : Trigger
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0002FFD8 File Offset: 0x0002E1D8
		protected override bool Check()
		{
			Sprite[] sprites = this._sprites;
			int i = 0;
			while (i < sprites.Length)
			{
				Sprite y = sprites[i];
				if (this._spriteRenderer.sprite == y)
				{
					if (this._playedThisSprite)
					{
						return false;
					}
					this._playedThisSprite = true;
					return true;
				}
				else
				{
					i++;
				}
			}
			this._playedThisSprite = false;
			return false;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003002C File Offset: 0x0002E22C
		private void OnDestroy()
		{
			for (int i = 0; i < this._sprites.Length; i++)
			{
				this._sprites[i] = null;
			}
		}

		// Token: 0x04000D33 RID: 3379
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04000D34 RID: 3380
		[SerializeField]
		private Sprite[] _sprites;

		// Token: 0x04000D35 RID: 3381
		private bool _playedThisSprite;
	}
}
