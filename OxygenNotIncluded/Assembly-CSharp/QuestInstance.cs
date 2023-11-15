using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008F5 RID: 2293
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestInstance : ISaveLoadable
{
	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06004280 RID: 17024 RVA: 0x00174866 File Offset: 0x00172A66
	public HashedString Id
	{
		get
		{
			return this.quest.IdHash;
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06004281 RID: 17025 RVA: 0x00174873 File Offset: 0x00172A73
	public int CriteriaCount
	{
		get
		{
			return this.quest.Criteria.Length;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06004282 RID: 17026 RVA: 0x00174882 File Offset: 0x00172A82
	public string Name
	{
		get
		{
			return this.quest.Name;
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06004283 RID: 17027 RVA: 0x0017488F File Offset: 0x00172A8F
	public string CompletionText
	{
		get
		{
			return this.quest.CompletionText;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06004284 RID: 17028 RVA: 0x0017489C File Offset: 0x00172A9C
	public bool IsStarted
	{
		get
		{
			return this.currentState > Quest.State.NotStarted;
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06004285 RID: 17029 RVA: 0x001748A7 File Offset: 0x00172AA7
	public bool IsComplete
	{
		get
		{
			return this.currentState == Quest.State.Completed;
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x001748B2 File Offset: 0x00172AB2
	// (set) Token: 0x06004287 RID: 17031 RVA: 0x001748BA File Offset: 0x00172ABA
	public float CurrentProgress { get; private set; }

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06004288 RID: 17032 RVA: 0x001748C3 File Offset: 0x00172AC3
	public Quest.State CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x001748CC File Offset: 0x00172ACC
	public QuestInstance(Quest quest)
	{
		this.quest = quest;
		this.criteriaStates = new Dictionary<int, QuestInstance.CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
			{
				Handle = i
			};
			if (questCriteria.TargetValues != null)
			{
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
				{
					value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
				{
					value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
			}
			this.criteriaStates[questCriteria.CriteriaId.GetHash()] = value;
		}
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x0017499C File Offset: 0x00172B9C
	public void Initialize(Quest quest)
	{
		this.quest = quest;
		this.ValidateCriteriasOnLoad();
		this.UpdateQuestProgress(false);
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x001749B2 File Offset: 0x00172BB2
	public bool HasCriteria(HashedString criteriaId)
	{
		return this.criteriaStates.ContainsKey(criteriaId.HashValue);
	}

	// Token: 0x0600428C RID: 17036 RVA: 0x001749C8 File Offset: 0x00172BC8
	public bool HasBehavior(QuestCriteria.BehaviorFlags behavior)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < this.quest.Criteria.Length)
		{
			flag = ((this.quest.Criteria[num].EvaluationBehaviors & behavior) > QuestCriteria.BehaviorFlags.None);
			num++;
		}
		return flag;
	}

	// Token: 0x0600428D RID: 17037 RVA: 0x00174A0C File Offset: 0x00172C0C
	public int GetTargetCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return this.quest.Criteria[criteriaState.Handle].RequiredCount;
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x00174A48 File Offset: 0x00172C48
	public int GetCurrentCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return criteriaState.CurrentCount;
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x00174A74 File Offset: 0x00172C74
	public float GetCurrentValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.CurrentValues == null)
		{
			return float.NaN;
		}
		return criteriaState.CurrentValues[valueHandle];
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x00174AB0 File Offset: 0x00172CB0
	public float GetTargetValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (this.quest.Criteria[criteriaState.Handle].TargetValues == null)
		{
			return float.NaN;
		}
		return this.quest.Criteria[criteriaState.Handle].TargetValues[valueHandle];
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x00174B14 File Offset: 0x00172D14
	public Tag GetSatisfyingItem(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return default(Tag);
		}
		return criteriaState.SatisfyingItems[valueHandle];
	}

	// Token: 0x06004292 RID: 17042 RVA: 0x00174B58 File Offset: 0x00172D58
	public float GetAreaAverage(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (!QuestCriteria.HasBehavior(this.quest.Criteria[criteriaState.Handle].EvaluationBehaviors, (QuestCriteria.BehaviorFlags)5))
		{
			return float.NaN;
		}
		float num = 0f;
		for (int i = 0; i < criteriaState.CurrentValues.Length; i++)
		{
			num += criteriaState.CurrentValues[i];
		}
		return num / (float)criteriaState.CurrentValues.Length;
	}

	// Token: 0x06004293 RID: 17043 RVA: 0x00174BD8 File Offset: 0x00172DD8
	public bool IsItemRedundant(HashedString criteriaId, Tag item)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < criteriaState.SatisfyingItems.Length)
		{
			flag = (criteriaState.SatisfyingItems[num] == item);
			num++;
		}
		return flag;
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x00174C34 File Offset: 0x00172E34
	public bool IsCriteriaSatisfied(HashedString id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.HashValue, out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x00174C80 File Offset: 0x00172E80
	public bool IsCriteriaSatisfied(Tag id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.GetHash(), out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004296 RID: 17046 RVA: 0x00174CCC File Offset: 0x00172ECC
	public void TrackAreaForCriteria(HashedString criteriaId, Extents area)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return;
		}
		int num = area.width * area.height;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		global::Debug.Assert(num <= 32);
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues = new float[num];
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems))
		{
			criteriaState.SatisfyingItems = new Tag[num];
		}
		this.criteriaStates[criteriaId.HashValue] = criteriaState;
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x00174D68 File Offset: 0x00172F68
	private uint GetSatisfactionMask(QuestInstance.CriteriaState state)
	{
		QuestCriteria questCriteria = this.quest.Criteria[state.Handle];
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			int num = 0;
			if (state.SatisfyingItems != null)
			{
				num = state.SatisfyingItems.Length;
			}
			else if (state.CurrentValues != null)
			{
				num = state.CurrentValues.Length;
			}
			return (uint)(Mathf.Pow(2f, (float)num) - 1f);
		}
		return questCriteria.GetSatisfactionMask();
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x00174DD8 File Offset: 0x00172FD8
	public int TrackProgress(Quest.ItemData data, out bool dataSatisfies, out bool itemIsRedundant)
	{
		dataSatisfies = false;
		itemIsRedundant = false;
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return -1;
		}
		int valueHandle = data.ValueHandle;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		dataSatisfies = this.DataSatisfiesCriteria(data, ref valueHandle);
		if (valueHandle == -1)
		{
			return valueHandle;
		}
		bool flag = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.AllowsRegression);
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems);
		Tag tag = flag2 ? criteriaState.SatisfyingItems[valueHandle] : default(Tag);
		if (dataSatisfies)
		{
			itemIsRedundant = (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.UniqueItems) && this.IsItemRedundant(data.CriteriaId, data.SatisfyingItem));
			if (itemIsRedundant)
			{
				return valueHandle;
			}
			tag = data.SatisfyingItem;
			criteriaState.SatisfactionState |= questCriteria.GetValueMask(valueHandle);
		}
		else if (flag)
		{
			criteriaState.SatisfactionState &= ~questCriteria.GetValueMask(valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues[valueHandle] = data.CurrentValue;
		}
		if (flag2)
		{
			criteriaState.SatisfyingItems[valueHandle] = tag;
		}
		bool flag3 = this.IsCriteriaSatisfied(data.CriteriaId);
		bool flag4 = questCriteria.IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
		if (flag3 != flag4)
		{
			criteriaState.CurrentCount += (flag3 ? -1 : 1);
			if (flag4 && criteriaState.CurrentCount < questCriteria.RequiredCount)
			{
				criteriaState.SatisfactionState = 0U;
			}
		}
		this.criteriaStates[data.CriteriaId.HashValue] = criteriaState;
		this.UpdateQuestProgress(true);
		return valueHandle;
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x00174F74 File Offset: 0x00173174
	public bool DataSatisfiesCriteria(Quest.ItemData data, ref int valueHandle)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return false;
		}
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		bool flag = questCriteria.AcceptedTags == null || (data.QualifyingTag.IsValid && questCriteria.AcceptedTags.Contains(data.QualifyingTag));
		if (flag && questCriteria.TargetValues == null)
		{
			valueHandle = 0;
		}
		if (!flag || valueHandle != -1)
		{
			return flag && questCriteria.ValueSatisfies(data.CurrentValue, valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle = data.LocalCellId;
		}
		int num = -1;
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues);
		bool flag3 = false;
		int num2 = 0;
		while (!flag3 && num2 < questCriteria.TargetValues.Length)
		{
			if (questCriteria.ValueSatisfies(data.CurrentValue, num2))
			{
				flag3 = true;
				num = num2;
				break;
			}
			if (flag2 && (num == -1 || criteriaState.CurrentValues[num] > criteriaState.CurrentValues[num2]))
			{
				num = num2;
			}
			num2++;
		}
		if (valueHandle == -1 && num != -1)
		{
			valueHandle = questCriteria.RequiredCount * num + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
		}
		return flag3;
	}

	// Token: 0x0600429A RID: 17050 RVA: 0x001750AC File Offset: 0x001732AC
	private void UpdateQuestProgress(bool startQuest = false)
	{
		if (!this.IsStarted && !startQuest)
		{
			return;
		}
		float currentProgress = this.CurrentProgress;
		Quest.State state = this.currentState;
		this.currentState = Quest.State.InProgress;
		this.CurrentProgress = 0f;
		float num = 0f;
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = this.quest.Criteria[i];
			QuestInstance.CriteriaState criteriaState = this.criteriaStates[questCriteria.CriteriaId.GetHash()];
			float num2 = (float)((questCriteria.TargetValues != null) ? questCriteria.TargetValues.Length : 1);
			num += (float)questCriteria.RequiredCount;
			this.CurrentProgress += (float)criteriaState.CurrentCount;
			if (!this.IsCriteriaSatisfied(questCriteria.CriteriaId))
			{
				float num3 = 0f;
				int num4 = 0;
				while (questCriteria.TargetValues != null && (float)num4 < num2)
				{
					if ((criteriaState.SatisfactionState & questCriteria.GetValueMask(num4)) == 0U)
					{
						if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
						{
							int num5 = questCriteria.RequiredCount * num4 + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
							num3 += Mathf.Max(0f, criteriaState.CurrentValues[num5] / questCriteria.TargetValues[num4]);
						}
					}
					else
					{
						num3 += 1f;
					}
					num4++;
				}
				this.CurrentProgress += num3 / num2;
			}
		}
		this.CurrentProgress = Mathf.Clamp01(this.CurrentProgress / num);
		if (this.CurrentProgress == 1f)
		{
			this.currentState = Quest.State.Completed;
		}
		float num6 = this.CurrentProgress - currentProgress;
		if (state != this.currentState || Mathf.Abs(num6) > Mathf.Epsilon)
		{
			Action<QuestInstance, Quest.State, float> questProgressChanged = this.QuestProgressChanged;
			if (questProgressChanged == null)
			{
				return;
			}
			questProgressChanged(this, state, num6);
		}
	}

	// Token: 0x0600429B RID: 17051 RVA: 0x00175288 File Offset: 0x00173488
	public ICheckboxListGroupControl.CheckboxItem[] GetCheckBoxData(Func<int, string, QuestInstance, string> resolveToolTip = null)
	{
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[this.quest.Criteria.Length];
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria c = this.quest.Criteria[i];
			array[i] = new ICheckboxListGroupControl.CheckboxItem
			{
				text = c.Text,
				isOn = this.IsCriteriaSatisfied(c.CriteriaId),
				tooltip = c.Tooltip
			};
			if (resolveToolTip != null)
			{
				array[i].resolveTooltipCallback = ((string tooltip, object owner) => resolveToolTip(c.CriteriaId.GetHash(), c.Tooltip, this));
			}
		}
		return array;
	}

	// Token: 0x0600429C RID: 17052 RVA: 0x00175370 File Offset: 0x00173570
	public void ValidateCriteriasOnLoad()
	{
		if (this.criteriaStates.Count != this.quest.Criteria.Length)
		{
			Dictionary<int, QuestInstance.CriteriaState> dictionary = new Dictionary<int, QuestInstance.CriteriaState>(this.quest.Criteria.Length);
			for (int i = 0; i < this.quest.Criteria.Length; i++)
			{
				QuestCriteria questCriteria = this.quest.Criteria[i];
				int hash = questCriteria.CriteriaId.GetHash();
				if (this.criteriaStates.ContainsKey(hash))
				{
					dictionary[hash] = this.criteriaStates[hash];
				}
				else
				{
					QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
					{
						Handle = i
					};
					if (questCriteria.TargetValues != null)
					{
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
						{
							value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
						{
							value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
					}
					dictionary[hash] = value;
				}
			}
			this.criteriaStates = dictionary;
		}
	}

	// Token: 0x04002B70 RID: 11120
	public Action<QuestInstance, Quest.State, float> QuestProgressChanged;

	// Token: 0x04002B72 RID: 11122
	private Quest quest;

	// Token: 0x04002B73 RID: 11123
	[Serialize]
	private Dictionary<int, QuestInstance.CriteriaState> criteriaStates;

	// Token: 0x04002B74 RID: 11124
	[Serialize]
	private Quest.State currentState;

	// Token: 0x0200174D RID: 5965
	private struct CriteriaState
	{
		// Token: 0x06008E11 RID: 36369 RVA: 0x0031E6E4 File Offset: 0x0031C8E4
		public static bool ItemAlreadySatisfying(QuestInstance.CriteriaState state, Tag item)
		{
			bool result = false;
			int num = 0;
			while (state.SatisfyingItems != null && num < state.SatisfyingItems.Length)
			{
				if (state.SatisfyingItems[num] == item)
				{
					result = true;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x04006E52 RID: 28242
		public int Handle;

		// Token: 0x04006E53 RID: 28243
		public int CurrentCount;

		// Token: 0x04006E54 RID: 28244
		public uint SatisfactionState;

		// Token: 0x04006E55 RID: 28245
		public Tag[] SatisfyingItems;

		// Token: 0x04006E56 RID: 28246
		public float[] CurrentValues;
	}
}
