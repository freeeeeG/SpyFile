using System;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class AssignChildAttribute : PropertyAttribute
{
	// Token: 0x06000BC2 RID: 3010 RVA: 0x0003DAD7 File Offset: 0x0003BED7
	public AssignChildAttribute(string _name, Editorbility _canEdit = Editorbility.NonEditable)
	{
		this.Name = _name;
		this.Editable = _canEdit;
	}

	// Token: 0x040008ED RID: 2285
	public string Name;

	// Token: 0x040008EE RID: 2286
	public Editorbility Editable = Editorbility.NonEditable;
}
