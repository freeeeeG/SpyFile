using System;

// Token: 0x02000B80 RID: 2944
public abstract class MessageDialog : KMonoBehaviour
{
	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06005B6F RID: 23407 RVA: 0x00218E9E File Offset: 0x0021709E
	public virtual bool CanDontShowAgain
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06005B70 RID: 23408
	public abstract bool CanDisplay(Message message);

	// Token: 0x06005B71 RID: 23409
	public abstract void SetMessage(Message message);

	// Token: 0x06005B72 RID: 23410
	public abstract void OnClickAction();

	// Token: 0x06005B73 RID: 23411 RVA: 0x00218EA1 File Offset: 0x002170A1
	public virtual void OnDontShowAgain()
	{
	}
}
