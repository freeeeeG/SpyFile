﻿using System;
using System.Diagnostics;

// Token: 0x020007AE RID: 1966
public class CellModifyMassEvent : CellEvent
{
	// Token: 0x0600368F RID: 13967 RVA: 0x00126CEF File Offset: 0x00124EEF
	public CellModifyMassEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x00126CFC File Offset: 0x00124EFC
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x00126D30 File Offset: 0x00124F30
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
