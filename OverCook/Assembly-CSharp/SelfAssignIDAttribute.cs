using System;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class SelfAssignIDAttribute : PropertyAttribute
{
	// Token: 0x06000BD1 RID: 3025 RVA: 0x0003DC39 File Offset: 0x0003C039
	public SelfAssignIDAttribute(Visibility _vis = Visibility.Show)
	{
		this.ShowOrHide = _vis;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0003DC48 File Offset: 0x0003C048
	public SelfAssignIDAttribute(bool _assignOnce)
	{
		this.AssignOnce = _assignOnce;
	}

	// Token: 0x04000903 RID: 2307
	public Visibility ShowOrHide;

	// Token: 0x04000904 RID: 2308
	public bool AssignOnce;
}
