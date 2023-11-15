using System;
using System.Diagnostics;

// Token: 0x020007B8 RID: 1976
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
	// Token: 0x060036BD RID: 14013 RVA: 0x001277BB File Offset: 0x001259BB
	public Expression(string id, ResourceSet parent, Face face) : base(id, parent, null)
	{
		this.face = face;
	}

	// Token: 0x040021A2 RID: 8610
	public Face face;

	// Token: 0x040021A3 RID: 8611
	public int priority;
}
