using System;

// Token: 0x0200002B RID: 43
public class LTEvent
{
	// Token: 0x06000340 RID: 832 RVA: 0x00013B1C File Offset: 0x00011D1C
	public LTEvent(int id, object data)
	{
		this.id = id;
		this.data = data;
	}

	// Token: 0x040001BC RID: 444
	public int id;

	// Token: 0x040001BD RID: 445
	public object data;
}
