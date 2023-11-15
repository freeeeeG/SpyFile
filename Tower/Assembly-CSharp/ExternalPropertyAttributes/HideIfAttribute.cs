using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200003E RID: 62
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class HideIfAttribute : ShowIfAttributeBase
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000037CF File Offset: 0x000019CF
		public HideIfAttribute(string condition) : base(condition)
		{
			base.Inverted = true;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000037DF File Offset: 0x000019DF
		public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions) : base(conditionOperator, conditions)
		{
			base.Inverted = true;
		}
	}
}
