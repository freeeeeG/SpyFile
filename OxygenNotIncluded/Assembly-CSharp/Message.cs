using System;
using KSerialization;

// Token: 0x02000B7F RID: 2943
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
	// Token: 0x06005B65 RID: 23397
	public abstract string GetTitle();

	// Token: 0x06005B66 RID: 23398
	public abstract string GetSound();

	// Token: 0x06005B67 RID: 23399
	public abstract string GetMessageBody();

	// Token: 0x06005B68 RID: 23400
	public abstract string GetTooltip();

	// Token: 0x06005B69 RID: 23401 RVA: 0x00218E89 File Offset: 0x00217089
	public virtual bool ShowDialog()
	{
		return true;
	}

	// Token: 0x06005B6A RID: 23402 RVA: 0x00218E8C File Offset: 0x0021708C
	public virtual void OnCleanUp()
	{
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x00218E8E File Offset: 0x0021708E
	public virtual bool IsValid()
	{
		return true;
	}

	// Token: 0x06005B6C RID: 23404 RVA: 0x00218E91 File Offset: 0x00217091
	public virtual bool PlayNotificationSound()
	{
		return true;
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x00218E94 File Offset: 0x00217094
	public virtual void OnClick()
	{
	}
}
