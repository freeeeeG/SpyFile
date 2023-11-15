using System;

// Token: 0x0200003A RID: 58
[Serializable]
public class TutorialData
{
	// Token: 0x0400022C RID: 556
	public int ID = -1;

	// Token: 0x0400022D RID: 557
	public int preID = -2;

	// Token: 0x0400022E RID: 558
	public int posType = -1;

	// Token: 0x0400022F RID: 559
	public LanguageSet name = new LanguageSet();

	// Token: 0x04000230 RID: 560
	public LanguageSet info = new LanguageSet();
}
