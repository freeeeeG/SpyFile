using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
[Serializable]
public class SpriteList
{
	// Token: 0x0600025A RID: 602 RVA: 0x0000DECB File Offset: 0x0000C0CB
	public Sprite GetSpriteWithId(int id)
	{
		if (id >= this.sprites.Length)
		{
			return this.spriteDefault;
		}
		if (this.sprites[id] == null)
		{
			return this.spriteDefault;
		}
		return this.sprites[id];
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000DEFE File Offset: 0x0000C0FE
	public Sprite GetSprite_Default()
	{
		return this.spriteDefault;
	}

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private Sprite[] sprites = new Sprite[0];

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private Sprite spriteDefault;
}
