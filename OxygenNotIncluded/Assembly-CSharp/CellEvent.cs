using System;

// Token: 0x020007AB RID: 1963
public class CellEvent : EventBase
{
	// Token: 0x06003687 RID: 13959 RVA: 0x0012645A File Offset: 0x0012465A
	public CellEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id)
	{
		this.reason = reason;
		this.isSend = is_send;
		this.enableLogging = enable_logging;
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x00126479 File Offset: 0x00124679
	public string GetMessagePrefix()
	{
		if (this.isSend)
		{
			return ">>>: ";
		}
		return "<<<: ";
	}

	// Token: 0x04002140 RID: 8512
	public string reason;

	// Token: 0x04002141 RID: 8513
	public bool isSend;

	// Token: 0x04002142 RID: 8514
	public bool enableLogging;
}
