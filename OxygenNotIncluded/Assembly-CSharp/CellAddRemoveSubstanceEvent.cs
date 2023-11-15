using System;
using System.Diagnostics;

// Token: 0x020007A7 RID: 1959
public class CellAddRemoveSubstanceEvent : CellEvent
{
	// Token: 0x0600367B RID: 13947 RVA: 0x00126241 File Offset: 0x00124441
	public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00126250 File Offset: 0x00124450
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00126284 File Offset: 0x00124484
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		SimHashes data = (SimHashes)cellEventInstance.data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			", Mass=",
			((float)cellEventInstance.data2 / 1000f).ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
