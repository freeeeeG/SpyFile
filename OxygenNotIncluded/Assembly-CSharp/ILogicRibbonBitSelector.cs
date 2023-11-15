using System;

// Token: 0x02000C2A RID: 3114
public interface ILogicRibbonBitSelector
{
	// Token: 0x06006284 RID: 25220
	void SetBitSelection(int bit);

	// Token: 0x06006285 RID: 25221
	int GetBitSelection();

	// Token: 0x06006286 RID: 25222
	int GetBitDepth();

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06006287 RID: 25223
	string SideScreenTitle { get; }

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06006288 RID: 25224
	string SideScreenDescription { get; }

	// Token: 0x06006289 RID: 25225
	bool SideScreenDisplayWriterDescription();

	// Token: 0x0600628A RID: 25226
	bool SideScreenDisplayReaderDescription();

	// Token: 0x0600628B RID: 25227
	bool IsBitActive(int bit);

	// Token: 0x0600628C RID: 25228
	int GetOutputValue();

	// Token: 0x0600628D RID: 25229
	int GetInputValue();

	// Token: 0x0600628E RID: 25230
	void UpdateVisuals();
}
