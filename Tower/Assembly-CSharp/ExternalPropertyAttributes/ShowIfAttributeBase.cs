using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000044 RID: 68
	public class ShowIfAttributeBase : MetaAttribute
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003859 File Offset: 0x00001A59
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003861 File Offset: 0x00001A61
		public string[] Conditions { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000386A File Offset: 0x00001A6A
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003872 File Offset: 0x00001A72
		public EConditionOperator ConditionOperator { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000387B File Offset: 0x00001A7B
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00003883 File Offset: 0x00001A83
		public bool Inverted { get; protected set; }

		// Token: 0x060000BA RID: 186 RVA: 0x0000388C File Offset: 0x00001A8C
		public ShowIfAttributeBase(string condition)
		{
			this.ConditionOperator = EConditionOperator.And;
			this.Conditions = new string[]
			{
				condition
			};
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000038AB File Offset: 0x00001AAB
		public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
		{
			this.ConditionOperator = conditionOperator;
			this.Conditions = conditions;
		}
	}
}
