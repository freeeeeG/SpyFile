using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Actions;
using Characters.Cooldowns;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class SkulHeadController : MonoBehaviour
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000065 RID: 101 RVA: 0x0000395F File Offset: 0x00001B5F
	public CooldownSerializer cooldown
	{
		get
		{
			return this._action.cooldown;
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000396C File Offset: 0x00001B6C
	private void Awake()
	{
		this._skulSpritesMap = this._skulSprites.ToDictionary((Sprite s) => s.name);
		this._skulHeadlessSpritesMap = this._skulHeadlessSprites.ToDictionary((Sprite s) => s.name);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000039DC File Offset: 0x00001BDC
	private void LateUpdate()
	{
		Sprite sprite;
		if (this._action.cooldown.canUse)
		{
			if (this._skulSpritesMap.TryGetValue(this._spriteRenderer.sprite.name, out sprite))
			{
				this._spriteRenderer.sprite = sprite;
				return;
			}
		}
		else if (this._skulHeadlessSpritesMap.TryGetValue(this._spriteRenderer.sprite.name, out sprite))
		{
			this._spriteRenderer.sprite = sprite;
		}
	}

	// Token: 0x0400005F RID: 95
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	// Token: 0x04000060 RID: 96
	[SerializeField]
	private Characters.Actions.Action _action;

	// Token: 0x04000061 RID: 97
	[SerializeField]
	private Sprite[] _skulSprites;

	// Token: 0x04000062 RID: 98
	[SerializeField]
	private Sprite[] _skulHeadlessSprites;

	// Token: 0x04000063 RID: 99
	private Dictionary<string, Sprite> _skulSpritesMap;

	// Token: 0x04000064 RID: 100
	private Dictionary<string, Sprite> _skulHeadlessSpritesMap;
}
