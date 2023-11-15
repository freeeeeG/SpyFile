using System;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class AssignComponentAttribute : PropertyAttribute
{
	// Token: 0x06000BC4 RID: 3012 RVA: 0x0003DAFE File Offset: 0x0003BEFE
	public AssignComponentAttribute(Visibility _vis)
	{
		this.ShowOrHide = _vis;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0003DB14 File Offset: 0x0003BF14
	public AssignComponentAttribute(Editorbility _editor = Editorbility.NonEditable)
	{
		this.CanEdit = _editor;
		this.ShowOrHide = ((_editor != Editorbility.Editable) ? this.ShowOrHide : Visibility.Show);
	}

	// Token: 0x040008F2 RID: 2290
	public AssignComponentAttribute.LocatorType Type;

	// Token: 0x040008F3 RID: 2291
	public Visibility ShowOrHide;

	// Token: 0x040008F4 RID: 2292
	public Editorbility CanEdit = Editorbility.NonEditable;

	// Token: 0x02000277 RID: 631
	public enum LocatorType
	{
		// Token: 0x040008F6 RID: 2294
		Owner,
		// Token: 0x040008F7 RID: 2295
		DirectChild,
		// Token: 0x040008F8 RID: 2296
		Recursive
	}
}
