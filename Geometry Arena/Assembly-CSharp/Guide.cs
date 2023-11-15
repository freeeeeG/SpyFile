using System;

// Token: 0x0200002A RID: 42
[Serializable]
public class Guide
{
	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600021E RID: 542 RVA: 0x0000CFFD File Offset: 0x0000B1FD
	public string Language_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600021F RID: 543 RVA: 0x0000D010 File Offset: 0x0000B210
	public string Language_Info
	{
		get
		{
			return this.infos[(int)Setting.Inst.language];
		}
	}

	// Token: 0x040001D5 RID: 469
	public string dataName = "UNINITED";

	// Token: 0x040001D6 RID: 470
	public int dataID = -1;

	// Token: 0x040001D7 RID: 471
	public int orderNo = -1;

	// Token: 0x040001D8 RID: 472
	public bool ifAvailable;

	// Token: 0x040001D9 RID: 473
	public string[] names = new string[3];

	// Token: 0x040001DA RID: 474
	public string[] infos = new string[3];
}
