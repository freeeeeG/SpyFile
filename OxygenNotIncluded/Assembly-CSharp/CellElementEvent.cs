using System;
using System.Diagnostics;

// Token: 0x020007AA RID: 1962
public class CellElementEvent : CellEvent
{
	// Token: 0x06003684 RID: 13956 RVA: 0x001263C2 File Offset: 0x001245C2
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x001263D0 File Offset: 0x001245D0
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x001263FC File Offset: 0x001245FC
	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
