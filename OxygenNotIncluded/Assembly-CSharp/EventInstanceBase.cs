using System;
using KSerialization;

// Token: 0x020007B2 RID: 1970
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
	// Token: 0x0600369A RID: 13978 RVA: 0x00126ED8 File Offset: 0x001250D8
	public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x00126F04 File Offset: 0x00125104
	public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

	// Token: 0x04002187 RID: 8583
	[Serialize]
	public int frame;

	// Token: 0x04002188 RID: 8584
	[Serialize]
	public int eventHash;

	// Token: 0x04002189 RID: 8585
	public EventBase ev;
}
