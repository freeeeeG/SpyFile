using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000029 RID: 41
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class InfoBoxAttribute : DrawerAttribute
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003520 File Offset: 0x00001720
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003528 File Offset: 0x00001728
		public string Text { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003531 File Offset: 0x00001731
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003539 File Offset: 0x00001739
		public EInfoBoxType Type { get; private set; }

		// Token: 0x06000072 RID: 114 RVA: 0x00003542 File Offset: 0x00001742
		public InfoBoxAttribute(string text, EInfoBoxType type = EInfoBoxType.Normal)
		{
			this.Text = text;
			this.Type = type;
		}
	}
}
