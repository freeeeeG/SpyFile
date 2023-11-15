using System;
using System.Collections.Generic;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001C6 RID: 454
	public abstract class ATowerOverchargeMinigame
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0002EBB3 File Offset: 0x0002CDB3
		public List<OverchargeItemData> Data
		{
			get
			{
				return this.list_Data;
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002EBBB File Offset: 0x0002CDBB
		public void Initialize()
		{
			this.SetupMinigame();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0002EBC3 File Offset: 0x0002CDC3
		public OverchargeItemData GetItemData(int index)
		{
			return this.list_Data[index];
		}

		// Token: 0x06000BEE RID: 3054
		protected abstract void SetupMinigame();

		// Token: 0x06000BEF RID: 3055
		public abstract bool ValidateButtonPress(int index);

		// Token: 0x06000BF0 RID: 3056
		public abstract bool IsCompleted();

		// Token: 0x04000980 RID: 2432
		protected eOverchargeType type;

		// Token: 0x04000981 RID: 2433
		protected List<OverchargeItemData> list_Data;

		// Token: 0x04000982 RID: 2434
		protected readonly int MAX_BUTTON_COUNT = 9;
	}
}
