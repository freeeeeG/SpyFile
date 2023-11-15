using System;
using UnityEngine;

// Token: 0x02000A86 RID: 2694
public class EmbeddedContextualIconTextLookup : EmbeddedTextImageLookupBase
{
	// Token: 0x06003544 RID: 13636 RVA: 0x000F8525 File Offset: 0x000F6925
	protected void Awake()
	{
		this.m_semanticIconLookup = GameUtils.RequireManager<SemanticIconLookup>();
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x000F8534 File Offset: 0x000F6934
	protected override Sprite GetIcon(int _materialNum)
	{
		SemanticIconLookup.Semantic semantic = this.m_sprites.TryAtIndex(_materialNum);
		return this.m_semanticIconLookup.GetIcon(semantic, PlayerInputLookup.Player.One, ControllerIconLookup.IconContext.Bordered);
	}

	// Token: 0x04002AB5 RID: 10933
	[SerializeField]
	private SemanticIconLookup.Semantic[] m_sprites = new SemanticIconLookup.Semantic[0];

	// Token: 0x04002AB6 RID: 10934
	private SemanticIconLookup m_semanticIconLookup;
}
