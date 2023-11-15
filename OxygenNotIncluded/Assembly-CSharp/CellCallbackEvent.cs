using System;
using System.Diagnostics;

// Token: 0x020007A8 RID: 1960
public class CellCallbackEvent : CellEvent
{
	// Token: 0x0600367E RID: 13950 RVA: 0x00126304 File Offset: 0x00124504
	public CellCallbackEvent(string id, bool is_send, bool enable_logging = true) : base(id, "Callback", is_send, enable_logging)
	{
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x00126314 File Offset: 0x00124514
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00126340 File Offset: 0x00124540
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
	}
}
