using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000033 RID: 51
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : SpecialCaseDrawerAttribute
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000368D File Offset: 0x0000188D
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003695 File Offset: 0x00001895
		public string Text { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000369E File Offset: 0x0000189E
		// (set) Token: 0x06000091 RID: 145 RVA: 0x000036A6 File Offset: 0x000018A6
		public EButtonEnableMode SelectedEnableMode { get; private set; }

		// Token: 0x06000092 RID: 146 RVA: 0x000036AF File Offset: 0x000018AF
		public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always)
		{
			this.Text = text;
			this.SelectedEnableMode = enabledMode;
		}
	}
}
