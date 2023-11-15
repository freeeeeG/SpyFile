using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200003C RID: 60
	public abstract class EnableIfAttributeBase : MetaAttribute
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003747 File Offset: 0x00001947
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000374F File Offset: 0x0000194F
		public string[] Conditions { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003758 File Offset: 0x00001958
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003760 File Offset: 0x00001960
		public EConditionOperator ConditionOperator { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003769 File Offset: 0x00001969
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003771 File Offset: 0x00001971
		public bool Inverted { get; protected set; }

		// Token: 0x060000A4 RID: 164 RVA: 0x0000377A File Offset: 0x0000197A
		public EnableIfAttributeBase(string condition)
		{
			this.ConditionOperator = EConditionOperator.And;
			this.Conditions = new string[]
			{
				condition
			};
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003799 File Offset: 0x00001999
		public EnableIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
		{
			this.ConditionOperator = conditionOperator;
			this.Conditions = conditions;
		}
	}
}
