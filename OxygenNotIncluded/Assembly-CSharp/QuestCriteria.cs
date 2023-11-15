using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F6 RID: 2294
public class QuestCriteria
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x0600429D RID: 17053 RVA: 0x00175484 File Offset: 0x00173684
	// (set) Token: 0x0600429E RID: 17054 RVA: 0x0017548C File Offset: 0x0017368C
	public string Text { get; private set; }

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x0600429F RID: 17055 RVA: 0x00175495 File Offset: 0x00173695
	// (set) Token: 0x060042A0 RID: 17056 RVA: 0x0017549D File Offset: 0x0017369D
	public string Tooltip { get; private set; }

	// Token: 0x060042A1 RID: 17057 RVA: 0x001754A8 File Offset: 0x001736A8
	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
	{
		global::Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		this.CriteriaId = id;
		this.EvaluationBehaviors = flags;
		this.TargetValues = targetValues;
		this.AcceptedTags = acceptedTags;
		this.RequiredCount = requiredCount;
	}

	// Token: 0x060042A2 RID: 17058 RVA: 0x00175504 File Offset: 0x00173704
	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = (this.TargetValues == null) ? 0f : this.TargetValues[valueHandle];
		return this.ValueSatisfies_Internal(value, target);
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x0017553B File Offset: 0x0017373B
	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x0017553E File Offset: 0x0017373E
	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x00175548 File Offset: 0x00173748
	public void PopulateStrings(string prefix)
	{
		string str = this.CriteriaId.Name.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".NAME", out stringEntry))
		{
			this.Text = stringEntry.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".TOOLTIP", out stringEntry))
		{
			this.Tooltip = stringEntry.String;
		}
	}

	// Token: 0x060042A6 RID: 17062 RVA: 0x001755B5 File Offset: 0x001737B5
	public uint GetSatisfactionMask()
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		return (uint)Mathf.Pow(2f, (float)(this.TargetValues.Length - 1));
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x001755D7 File Offset: 0x001737D7
	public uint GetValueMask(int valueHandle)
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle %= this.TargetValues.Length;
		}
		return 1U << valueHandle;
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x00175603 File Offset: 0x00173803
	public static bool HasBehavior(QuestCriteria.BehaviorFlags flags, QuestCriteria.BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}

	// Token: 0x04002B75 RID: 11125
	public const int MAX_VALUES = 32;

	// Token: 0x04002B76 RID: 11126
	public const int INVALID_VALUE = -1;

	// Token: 0x04002B77 RID: 11127
	public readonly Tag CriteriaId;

	// Token: 0x04002B78 RID: 11128
	public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;

	// Token: 0x04002B79 RID: 11129
	public readonly float[] TargetValues;

	// Token: 0x04002B7A RID: 11130
	public readonly int RequiredCount = 1;

	// Token: 0x04002B7B RID: 11131
	public readonly HashSet<Tag> AcceptedTags;

	// Token: 0x02001750 RID: 5968
	public enum BehaviorFlags
	{
		// Token: 0x04006E5C RID: 28252
		None,
		// Token: 0x04006E5D RID: 28253
		TrackArea,
		// Token: 0x04006E5E RID: 28254
		AllowsRegression,
		// Token: 0x04006E5F RID: 28255
		TrackValues = 4,
		// Token: 0x04006E60 RID: 28256
		TrackItems = 8,
		// Token: 0x04006E61 RID: 28257
		UniqueItems = 24
	}
}
