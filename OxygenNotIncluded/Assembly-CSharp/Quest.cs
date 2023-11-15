using System;

// Token: 0x020008F4 RID: 2292
public class Quest : Resource
{
	// Token: 0x0600427F RID: 17023 RVA: 0x001747CC File Offset: 0x001729CC
	public Quest(string id, QuestCriteria[] criteria) : base(id, id)
	{
		Debug.Assert(criteria.Length != 0);
		this.Criteria = criteria;
		string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(str + ".NAME", out stringEntry))
		{
			this.Title = stringEntry.String;
		}
		if (Strings.TryGet(str + ".COMPLETE", out stringEntry))
		{
			this.CompletionText = stringEntry.String;
		}
		for (int i = 0; i < this.Criteria.Length; i++)
		{
			this.Criteria[i].PopulateStrings("STRINGS.CODEX.QUESTS.");
		}
	}

	// Token: 0x04002B6C RID: 11116
	public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";

	// Token: 0x04002B6D RID: 11117
	public readonly QuestCriteria[] Criteria;

	// Token: 0x04002B6E RID: 11118
	public readonly string Title;

	// Token: 0x04002B6F RID: 11119
	public readonly string CompletionText;

	// Token: 0x0200174B RID: 5963
	public struct ItemData
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06008E0F RID: 36367 RVA: 0x0031E6CE File Offset: 0x0031C8CE
		// (set) Token: 0x06008E10 RID: 36368 RVA: 0x0031E6D8 File Offset: 0x0031C8D8
		public int ValueHandle
		{
			get
			{
				return this.valueHandle - 1;
			}
			set
			{
				this.valueHandle = value + 1;
			}
		}

		// Token: 0x04006E48 RID: 28232
		public int LocalCellId;

		// Token: 0x04006E49 RID: 28233
		public float CurrentValue;

		// Token: 0x04006E4A RID: 28234
		public Tag SatisfyingItem;

		// Token: 0x04006E4B RID: 28235
		public Tag QualifyingTag;

		// Token: 0x04006E4C RID: 28236
		public HashedString CriteriaId;

		// Token: 0x04006E4D RID: 28237
		private int valueHandle;
	}

	// Token: 0x0200174C RID: 5964
	public enum State
	{
		// Token: 0x04006E4F RID: 28239
		NotStarted,
		// Token: 0x04006E50 RID: 28240
		InProgress,
		// Token: 0x04006E51 RID: 28241
		Completed
	}
}
