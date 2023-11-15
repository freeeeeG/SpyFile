using System;
using System.Collections.Generic;

// Token: 0x020007D2 RID: 2002
public class TagNameComparer : IComparer<Tag>
{
	// Token: 0x060037E4 RID: 14308 RVA: 0x00134645 File Offset: 0x00132845
	public TagNameComparer()
	{
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x0013464D File Offset: 0x0013284D
	public TagNameComparer(Tag firstTag)
	{
		this.firstTag = firstTag;
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x0013465C File Offset: 0x0013285C
	public int Compare(Tag x, Tag y)
	{
		if (x == y)
		{
			return 0;
		}
		if (this.firstTag.IsValid)
		{
			if (x == this.firstTag && y != this.firstTag)
			{
				return 1;
			}
			if (x != this.firstTag && y == this.firstTag)
			{
				return -1;
			}
		}
		return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
	}

	// Token: 0x0400246A RID: 9322
	private Tag firstTag;
}
