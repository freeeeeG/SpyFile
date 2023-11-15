using System;

namespace flanne
{
	// Token: 0x020000AB RID: 171
	public class NonStackingBuff : BuffPlayerStats
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0001AEC9 File Offset: 0x000190C9
		public void ApplyNonStackBuff()
		{
			if (this._isActive)
			{
				return;
			}
			base.ApplyBuff();
			this._isActive = true;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001AEE1 File Offset: 0x000190E1
		public void RemoveNonStackBuff()
		{
			if (!this._isActive)
			{
				return;
			}
			base.RemoveBuff();
			this._isActive = false;
		}

		// Token: 0x04000391 RID: 913
		private bool _isActive;
	}
}
