using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class ShopNode : ABaseNode
{
	// Token: 0x06000386 RID: 902 RVA: 0x0000E3C8 File Offset: 0x0000C5C8
	public override string GetName()
	{
		return this.shopName;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
	public override void OnElementSelected()
	{
		Debug.Log("Shop selected: " + this.shopName);
	}

	// Token: 0x040003B5 RID: 949
	public string shopName;
}
