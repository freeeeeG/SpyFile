using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200003B RID: 59
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class EnableIfAttribute : EnableIfAttributeBase
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00003726 File Offset: 0x00001926
		public EnableIfAttribute(string condition) : base(condition)
		{
			base.Inverted = false;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003736 File Offset: 0x00001936
		public EnableIfAttribute(EConditionOperator conditionOperator, params string[] conditions) : base(conditionOperator, conditions)
		{
			base.Inverted = false;
		}
	}
}
