using System;
using System.Diagnostics;

// Token: 0x020007A9 RID: 1961
public class CellDigEvent : CellEvent
{
	// Token: 0x06003681 RID: 13953 RVA: 0x0012636F File Offset: 0x0012456F
	public CellDigEvent(bool enable_logging = true) : base("Dig", "Dig", true, enable_logging)
	{
	}

	// Token: 0x06003682 RID: 13954 RVA: 0x00126384 File Offset: 0x00124584
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003683 RID: 13955 RVA: 0x001263B0 File Offset: 0x001245B0
	public override string GetDescription(EventInstanceBase ev)
	{
		return base.GetMessagePrefix() + "Dig=true";
	}
}
