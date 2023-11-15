using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class AssignResourceAttribute : PropertyAttribute
{
	// Token: 0x06000BCA RID: 3018 RVA: 0x0003DB82 File Offset: 0x0003BF82
	public AssignResourceAttribute(string _asset, Editorbility _editable = Editorbility.NonEditable)
	{
		this.AssetName = _asset;
		this.Editable = _editable;
	}

	// Token: 0x040008FC RID: 2300
	public string AssetName;

	// Token: 0x040008FD RID: 2301
	public Editorbility Editable = Editorbility.NonEditable;
}
