using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000594 RID: 1428
[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
	// Token: 0x060022B9 RID: 8889 RVA: 0x000BE95D File Offset: 0x000BCB5D
	public AssignableSlot(string id, string name, bool showInUI = true) : base(id, name)
	{
		this.showInUI = showInUI;
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000BE978 File Offset: 0x000BCB78
	public AssignableSlotInstance Lookup(GameObject go)
	{
		Assignables component = go.GetComponent<Assignables>();
		if (component != null)
		{
			return component.GetSlot(this);
		}
		return null;
	}

	// Token: 0x040013EB RID: 5099
	public bool showInUI = true;
}
