using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000043 RID: 67
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowIfAttribute : ShowIfAttributeBase
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00003838 File Offset: 0x00001A38
		public ShowIfAttribute(string condition) : base(condition)
		{
			base.Inverted = false;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003848 File Offset: 0x00001A48
		public ShowIfAttribute(EConditionOperator conditionOperator, params string[] conditions) : base(conditionOperator, conditions)
		{
			base.Inverted = false;
		}
	}
}
