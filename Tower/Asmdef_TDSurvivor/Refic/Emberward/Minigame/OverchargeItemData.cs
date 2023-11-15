using System;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001C7 RID: 455
	public class OverchargeItemData
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002EBE1 File Offset: 0x0002CDE1
		public OverchargeItemData()
		{
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002EBE9 File Offset: 0x0002CDE9
		public OverchargeItemData(int _contentValue, string _showText)
		{
			this.contentValue = _contentValue;
			this.showText = _showText;
		}

		// Token: 0x04000983 RID: 2435
		public int contentValue;

		// Token: 0x04000984 RID: 2436
		public string showText;
	}
}
