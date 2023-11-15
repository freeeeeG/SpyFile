using System;
using UnityEngine;

// Token: 0x02000930 RID: 2352
public class TechTreeTitle : Resource
{
	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06004448 RID: 17480 RVA: 0x0017F085 File Offset: 0x0017D285
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06004449 RID: 17481 RVA: 0x0017F092 File Offset: 0x0017D292
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x0600444A RID: 17482 RVA: 0x0017F09F File Offset: 0x0017D29F
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x0017F0AC File Offset: 0x0017D2AC
	public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node) : base(id, parent, name)
	{
		this.node = node;
	}

	// Token: 0x04002D47 RID: 11591
	public string desc;

	// Token: 0x04002D48 RID: 11592
	private ResourceTreeNode node;
}
