using System;

namespace Team17.Online
{
	// Token: 0x0200095F RID: 2399
	public class OnlineMultiplayerSessionPropertySearchValue
	{
		// Token: 0x04002582 RID: 9602
		public IOnlineMultiplayerSessionProperty m_property;

		// Token: 0x04002583 RID: 9603
		public uint m_value;

		// Token: 0x04002584 RID: 9604
		public uint m_valueMinRange;

		// Token: 0x04002585 RID: 9605
		public uint m_valueMaxRange;

		// Token: 0x04002586 RID: 9606
		public OnlineMultiplayerSessionPropertySearchValue.Operator m_operator;

		// Token: 0x02000960 RID: 2400
		public enum Operator
		{
			// Token: 0x04002588 RID: 9608
			eEquals,
			// Token: 0x04002589 RID: 9609
			eNotEquals,
			// Token: 0x0400258A RID: 9610
			eLessThan,
			// Token: 0x0400258B RID: 9611
			eLessEqualsThan,
			// Token: 0x0400258C RID: 9612
			eGreaterThan,
			// Token: 0x0400258D RID: 9613
			eGreaterEqualsThan
		}
	}
}
