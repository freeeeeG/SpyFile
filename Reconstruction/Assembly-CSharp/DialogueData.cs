using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// Token: 0x02000244 RID: 580
[CreateAssetMenu(menuName = "Dialogue/GuideDialogue", fileName = "NewGuideDialogue")]
public class DialogueData : SerializedScriptableObject
{
	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x000277BC File Offset: 0x000259BC
	private string GuideNote
	{
		get
		{
			return this.PreviewWords();
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000277C4 File Offset: 0x000259C4
	public IEnumerable<Type> GetGuideEventList()
	{
		return from x in typeof(GuideEvent).Assembly.GetTypes()
		where !x.IsAbstract
		where !x.IsGenericTypeDefinition
		where typeof(GuideEvent).IsAssignableFrom(x)
		select x;
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00027854 File Offset: 0x00025A54
	public IEnumerable<Type> GetGuideConditionList()
	{
		return from x in typeof(GuideCondition).Assembly.GetTypes()
		where !x.IsAbstract
		where !x.IsGenericTypeDefinition
		where typeof(GuideCondition).IsAssignableFrom(x)
		select x;
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000278E4 File Offset: 0x00025AE4
	public bool JudgeConditions(TutorialType tutorialType)
	{
		if (tutorialType != this.TriggerType)
		{
			return false;
		}
		GuideCondition[] guideConditions = this.GuideConditions;
		for (int i = 0; i < guideConditions.Length; i++)
		{
			if (!guideConditions[i].Judge())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x00027920 File Offset: 0x00025B20
	public void TriggerGuideStartEvents()
	{
		GuideEvent[] guideStartEvents = this.GuideStartEvents;
		for (int i = 0; i < guideStartEvents.Length; i++)
		{
			guideStartEvents[i].Trigger();
		}
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0002794C File Offset: 0x00025B4C
	public void TriggerGuideEndEvents()
	{
		GuideEvent[] guideEndEvents = this.GuideEndEvents;
		for (int i = 0; i < guideEndEvents.Length; i++)
		{
			guideEndEvents[i].Trigger();
		}
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x00027976 File Offset: 0x00025B76
	private bool AlwaysFalse()
	{
		return false;
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0002797C File Offset: 0x00025B7C
	private string PreviewWords()
	{
		if (GameMultiLang.Fields != null)
		{
			string text = "";
			foreach (string key in this.Words)
			{
				text = text + GameMultiLang.GetTraduction(key) + "\n";
			}
			return text;
		}
		return "";
	}

	// Token: 0x04000757 RID: 1879
	public TutorialType TriggerType;

	// Token: 0x04000758 RID: 1880
	public GuideCondition[] GuideConditions;

	// Token: 0x04000759 RID: 1881
	public string[] Words;

	// Token: 0x0400075A RID: 1882
	public bool DontNeedClickEnd;

	// Token: 0x0400075B RID: 1883
	public float WaitingTime;

	// Token: 0x0400075C RID: 1884
	public GuideEvent[] GuideStartEvents;

	// Token: 0x0400075D RID: 1885
	public GuideEvent[] GuideEndEvents;
}
