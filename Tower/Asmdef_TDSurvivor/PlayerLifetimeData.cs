using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000064 RID: 100
[Serializable]
public class PlayerLifetimeData
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000268 RID: 616 RVA: 0x0000A433 File Offset: 0x00008633
	public int Exp
	{
		get
		{
			return this.exp;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000269 RID: 617 RVA: 0x0000A43B File Offset: 0x0000863B
	public bool IsTutorialStageFinished
	{
		get
		{
			return this.isTutorialStageFinished;
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000A443 File Offset: 0x00008643
	public PlayerLifetimeData()
	{
		this.list_LearnedTalent = new List<eTalentType>();
		this.list_FinishedTutorial = new List<eTutorialType>();
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000A464 File Offset: 0x00008664
	public void RegisterEvents()
	{
		this.isEventRegistered = true;
		EventMgr.Register<int>(eGameEvents.RequestAddExp, new Action<int>(this.OnRequestAddExp));
		EventMgr.Register<int>(eGameEvents.RequestSetExp, new Action<int>(this.OnRequestSetExp));
		EventMgr.Register<eTalentType>(eGameEvents.RequestLearnTalent, new Action<eTalentType>(this.OnRequestLearnTalent));
		EventMgr.Register(eGameEvents.RequestResetTalent, new Action(this.OnRequestResetTalent));
		EventMgr.Register<eTutorialType>(eGameEvents.RequestSetTutorialFinished, new Action<eTutorialType>(this.OnFinishedTutorial));
		EventMgr.Register<eItemType>(eGameEvents.RequestRecordTowerBuilt, new Action<eItemType>(this.OnRequestRecordTowerBuilt));
		EventMgr.Register(eGameEvents.RequestSetTutorialStageCompleted, new Action(this.OnRequestSetTutorialStageCompleted));
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000A520 File Offset: 0x00008720
	public void ClearEvents()
	{
		this.isEventRegistered = false;
		EventMgr.Remove<int>(eGameEvents.RequestAddExp, new Action<int>(this.OnRequestAddExp));
		EventMgr.Remove<int>(eGameEvents.RequestSetExp, new Action<int>(this.OnRequestSetExp));
		EventMgr.Remove<eTalentType>(eGameEvents.RequestLearnTalent, new Action<eTalentType>(this.OnRequestLearnTalent));
		EventMgr.Remove(eGameEvents.RequestResetTalent, new Action(this.OnRequestResetTalent));
		EventMgr.Remove<eTutorialType>(eGameEvents.RequestSetTutorialFinished, new Action<eTutorialType>(this.OnFinishedTutorial));
		EventMgr.Remove<eItemType>(eGameEvents.RequestRecordTowerBuilt, new Action<eItemType>(this.OnRequestRecordTowerBuilt));
		EventMgr.Remove(eGameEvents.RequestSetTutorialStageCompleted, new Action(this.OnRequestSetTutorialStageCompleted));
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000A5DC File Offset: 0x000087DC
	private void OnRequestAddExp(int value)
	{
		this.exp += value;
		EventMgr.SendEvent<int>(eGameEvents.OnExpChanged, this.exp);
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000A609 File Offset: 0x00008809
	private void OnRequestSetExp(int value)
	{
		this.exp = value;
		EventMgr.SendEvent<int>(eGameEvents.OnExpChanged, this.exp);
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000A630 File Offset: 0x00008830
	private void OnRequestLearnTalent(eTalentType type)
	{
		if (this.list_LearnedTalent.Contains(type))
		{
			Debug.LogWarningFormat("已經學過天賦:{0}", new object[]
			{
				type
			});
			return;
		}
		this.list_LearnedTalent.Add(type);
		EventMgr.SendEvent<eTalentType>(eGameEvents.OnTalentChanged, type);
		Debug.Log(string.Format("學習天賦:{0}", type));
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000A69E File Offset: 0x0000889E
	public bool IsTalentLearned(eTalentType type)
	{
		return this.list_LearnedTalent.Contains(type);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000A6AC File Offset: 0x000088AC
	private void OnRequestResetTalent()
	{
		this.list_LearnedTalent.Clear();
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000A6B9 File Offset: 0x000088B9
	private void OnRequestSetTutorialStageCompleted()
	{
		this.isTutorialStageFinished = true;
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000A6CC File Offset: 0x000088CC
	private void OnFinishedTutorial(eTutorialType type)
	{
		if (!this.list_FinishedTutorial.Contains(type))
		{
			this.list_FinishedTutorial.Add(type);
		}
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000A6F2 File Offset: 0x000088F2
	public bool IsFinishedTutorial(eTutorialType type)
	{
		return this.list_FinishedTutorial.Contains(type);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000A700 File Offset: 0x00008900
	private void OnRequestRecordTowerBuilt(eItemType itemType)
	{
		if (this.list_BuiltTowerRecord == null)
		{
			this.list_BuiltTowerRecord = new List<eItemType>();
		}
		if (!this.list_BuiltTowerRecord.Contains(itemType))
		{
			this.list_BuiltTowerRecord.Add(itemType);
		}
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000A739 File Offset: 0x00008939
	public bool IsTowerBuiltInRecord(eItemType itemType)
	{
		if (this.list_BuiltTowerRecord == null)
		{
			this.list_BuiltTowerRecord = new List<eItemType>();
		}
		return this.list_BuiltTowerRecord.Contains(itemType);
	}

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private int exp;

	// Token: 0x040001C3 RID: 451
	public List<eTalentType> list_LearnedTalent;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private List<eTutorialType> list_FinishedTutorial;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private List<eItemType> list_BuiltTowerRecord;

	// Token: 0x040001C6 RID: 454
	public bool isEventRegistered;

	// Token: 0x040001C7 RID: 455
	public bool isSeenGameIntro;

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private bool isTutorialStageFinished;

	// Token: 0x040001C9 RID: 457
	public int gamesPlayed;

	// Token: 0x040001CA RID: 458
	public int gamesWin;

	// Token: 0x040001CB RID: 459
	public int gameLost;
}
