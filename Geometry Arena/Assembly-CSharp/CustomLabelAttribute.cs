using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class CustomLabelAttribute : PropertyAttribute
{
	// Token: 0x06000285 RID: 645 RVA: 0x0000F2B7 File Offset: 0x0000D4B7
	public CustomLabelAttribute(string name)
	{
		this.name = name;
	}

	// Token: 0x04000255 RID: 597
	public string name;
}
