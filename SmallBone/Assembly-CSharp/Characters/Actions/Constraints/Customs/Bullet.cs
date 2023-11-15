using System;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x02000987 RID: 2439
	public class Bullet
	{
		// Token: 0x0600344A RID: 13386 RVA: 0x0009AC7C File Offset: 0x00098E7C
		public Bullet(int maxCount)
		{
			this._maxCount = maxCount;
			this._currentCount = maxCount;
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x0009AC92 File Offset: 0x00098E92
		public bool Has(int amount)
		{
			return this._currentCount >= amount;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x0009ACA0 File Offset: 0x00098EA0
		public bool Consume(int amount)
		{
			if (!this.Has(amount))
			{
				return false;
			}
			this._currentCount -= amount;
			return true;
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x0009ACBC File Offset: 0x00098EBC
		public void Reload()
		{
			this._currentCount = this._maxCount;
		}

		// Token: 0x04002A4F RID: 10831
		private int _maxCount;

		// Token: 0x04002A50 RID: 10832
		private int _currentCount;
	}
}
