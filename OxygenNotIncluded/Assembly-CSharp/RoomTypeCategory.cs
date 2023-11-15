using System;

// Token: 0x02000545 RID: 1349
public class RoomTypeCategory : Resource
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060020BB RID: 8379 RVA: 0x000AF6B1 File Offset: 0x000AD8B1
	// (set) Token: 0x060020BC RID: 8380 RVA: 0x000AF6B9 File Offset: 0x000AD8B9
	public string colorName { get; private set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060020BD RID: 8381 RVA: 0x000AF6C2 File Offset: 0x000AD8C2
	// (set) Token: 0x060020BE RID: 8382 RVA: 0x000AF6CA File Offset: 0x000AD8CA
	public string icon { get; private set; }

	// Token: 0x060020BF RID: 8383 RVA: 0x000AF6D3 File Offset: 0x000AD8D3
	public RoomTypeCategory(string id, string name, string colorName, string icon) : base(id, name)
	{
		this.colorName = colorName;
		this.icon = icon;
	}
}
