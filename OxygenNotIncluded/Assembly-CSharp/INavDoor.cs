using System;

// Token: 0x020005F1 RID: 1521
public interface INavDoor
{
	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060025F6 RID: 9718
	bool isSpawned { get; }

	// Token: 0x060025F7 RID: 9719
	bool IsOpen();

	// Token: 0x060025F8 RID: 9720
	void Open();

	// Token: 0x060025F9 RID: 9721
	void Close();
}
