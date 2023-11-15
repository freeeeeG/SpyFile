using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200003A RID: 58
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DisableIfAttribute : EnableIfAttributeBase
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00003705 File Offset: 0x00001905
		public DisableIfAttribute(string condition) : base(condition)
		{
			base.Inverted = true;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003715 File Offset: 0x00001915
		public DisableIfAttribute(EConditionOperator conditionOperator, params string[] conditions) : base(conditionOperator, conditions)
		{
			base.Inverted = true;
		}
	}
}
