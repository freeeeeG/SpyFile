using System;

// Token: 0x0200063B RID: 1595
internal interface IExceptionDisplayer
{
	// Token: 0x06001E5C RID: 7772
	void Initialize();

	// Token: 0x06001E5D RID: 7773
	void OnGUI();

	// Token: 0x06001E5E RID: 7774
	void Display(string exceptionString, string stackTrace, bool bJustOccured);
}
