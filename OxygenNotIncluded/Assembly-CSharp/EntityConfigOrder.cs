using System;

// Token: 0x02000791 RID: 1937
public class EntityConfigOrder : Attribute
{
	// Token: 0x060035F0 RID: 13808 RVA: 0x001239F0 File Offset: 0x00121BF0
	public EntityConfigOrder(int sort_order)
	{
		this.sortOrder = sort_order;
	}

	// Token: 0x040020EE RID: 8430
	public int sortOrder;
}
