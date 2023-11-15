using System;

// Token: 0x02000B67 RID: 2919
public interface IConsumableUIItem
{
	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06005A39 RID: 23097
	string ConsumableId { get; }

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06005A3A RID: 23098
	string ConsumableName { get; }

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06005A3B RID: 23099
	int MajorOrder { get; }

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06005A3C RID: 23100
	int MinorOrder { get; }

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06005A3D RID: 23101
	bool Display { get; }
}
