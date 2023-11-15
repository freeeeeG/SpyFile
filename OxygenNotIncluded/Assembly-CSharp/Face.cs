using System;

// Token: 0x020007BA RID: 1978
public class Face : Resource
{
	// Token: 0x060036BF RID: 14015 RVA: 0x001277D5 File Offset: 0x001259D5
	public Face(string id, string headFXSymbol = null) : base(id, null, null)
	{
		this.hash = new HashedString(id);
		this.headFXHash = headFXSymbol;
	}

	// Token: 0x040021A4 RID: 8612
	public HashedString hash;

	// Token: 0x040021A5 RID: 8613
	public HashedString headFXHash;

	// Token: 0x040021A6 RID: 8614
	private const string SYMBOL_PREFIX = "headfx_";
}
