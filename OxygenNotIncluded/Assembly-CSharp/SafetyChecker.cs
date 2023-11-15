using System;

// Token: 0x0200040A RID: 1034
public class SafetyChecker
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060015C9 RID: 5577 RVA: 0x00072E63 File Offset: 0x00071063
	// (set) Token: 0x060015CA RID: 5578 RVA: 0x00072E6B File Offset: 0x0007106B
	public SafetyChecker.Condition[] conditions { get; private set; }

	// Token: 0x060015CB RID: 5579 RVA: 0x00072E74 File Offset: 0x00071074
	public SafetyChecker(SafetyChecker.Condition[] conditions)
	{
		this.conditions = conditions;
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x00072E84 File Offset: 0x00071084
	public int GetSafetyConditions(int cell, int cost, SafetyChecker.Context context, out bool all_conditions_met)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.conditions.Length; i++)
		{
			SafetyChecker.Condition condition = this.conditions[i];
			if (condition.callback(cell, cost, context))
			{
				num |= condition.mask;
				num2++;
			}
		}
		all_conditions_met = (num2 == this.conditions.Length);
		return num;
	}

	// Token: 0x0200108C RID: 4236
	public struct Condition
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06007601 RID: 30209 RVA: 0x002CE4CE File Offset: 0x002CC6CE
		// (set) Token: 0x06007602 RID: 30210 RVA: 0x002CE4D6 File Offset: 0x002CC6D6
		public SafetyChecker.Condition.Callback callback { readonly get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06007603 RID: 30211 RVA: 0x002CE4DF File Offset: 0x002CC6DF
		// (set) Token: 0x06007604 RID: 30212 RVA: 0x002CE4E7 File Offset: 0x002CC6E7
		public int mask { readonly get; private set; }

		// Token: 0x06007605 RID: 30213 RVA: 0x002CE4F0 File Offset: 0x002CC6F0
		public Condition(string id, int condition_mask, SafetyChecker.Condition.Callback condition_callback)
		{
			this = default(SafetyChecker.Condition);
			this.callback = condition_callback;
			this.mask = condition_mask;
		}

		// Token: 0x02002009 RID: 8201
		// (Invoke) Token: 0x0600A480 RID: 42112
		public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
	}

	// Token: 0x0200108D RID: 4237
	public struct Context
	{
		// Token: 0x06007606 RID: 30214 RVA: 0x002CE508 File Offset: 0x002CC708
		public Context(KMonoBehaviour cmp)
		{
			this.cell = Grid.PosToCell(cmp);
			this.navigator = cmp.GetComponent<Navigator>();
			this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
			this.minionBrain = cmp.GetComponent<MinionBrain>();
			this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
			this.primaryElement = cmp.GetComponent<PrimaryElement>();
		}

		// Token: 0x0400597D RID: 22909
		public Navigator navigator;

		// Token: 0x0400597E RID: 22910
		public OxygenBreather oxygenBreather;

		// Token: 0x0400597F RID: 22911
		public SimTemperatureTransfer temperatureTransferer;

		// Token: 0x04005980 RID: 22912
		public PrimaryElement primaryElement;

		// Token: 0x04005981 RID: 22913
		public MinionBrain minionBrain;

		// Token: 0x04005982 RID: 22914
		public int cell;
	}
}
