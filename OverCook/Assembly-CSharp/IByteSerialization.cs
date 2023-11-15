using System;

// Token: 0x02000716 RID: 1814
public interface IByteSerialization
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06002275 RID: 8821
	int ByteSaveSize { get; }

	// Token: 0x06002276 RID: 8822
	byte[] ByteSave();

	// Token: 0x06002277 RID: 8823
	bool ByteLoad(byte[] _data);
}
