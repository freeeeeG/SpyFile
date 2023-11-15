using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x020009F0 RID: 2544
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x001A9BD2 File Offset: 0x001A7DD2
	// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x001A9BDC File Offset: 0x001A7DDC
	public StoryInstance.State CurrentState
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state == value)
			{
				return;
			}
			this.state = value;
			this.Telemetry.LogStateChange(this.state, GameClock.Instance.GetTimeInCycles());
			Action<StoryInstance.State> storyStateChanged = this.StoryStateChanged;
			if (storyStateChanged == null)
			{
				return;
			}
			storyStateChanged(this.state);
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06004BDA RID: 19418 RVA: 0x001A9C2B File Offset: 0x001A7E2B
	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (this.telemetry == null)
			{
				this.telemetry = new StoryManager.StoryTelemetry();
			}
			return this.telemetry;
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06004BDB RID: 19419 RVA: 0x001A9C46 File Offset: 0x001A7E46
	// (set) Token: 0x06004BDC RID: 19420 RVA: 0x001A9C4E File Offset: 0x001A7E4E
	public EventInfoData EventInfo { get; private set; }

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06004BDD RID: 19421 RVA: 0x001A9C57 File Offset: 0x001A7E57
	// (set) Token: 0x06004BDE RID: 19422 RVA: 0x001A9C5F File Offset: 0x001A7E5F
	public Notification Notification { get; private set; }

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06004BDF RID: 19423 RVA: 0x001A9C68 File Offset: 0x001A7E68
	// (set) Token: 0x06004BE0 RID: 19424 RVA: 0x001A9C70 File Offset: 0x001A7E70
	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;

	// Token: 0x06004BE1 RID: 19425 RVA: 0x001A9C79 File Offset: 0x001A7E79
	public Story GetStory()
	{
		if (this._story == null)
		{
			this._story = Db.Get().Stories.Get(this.storyId);
		}
		return this._story;
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x001A9CA4 File Offset: 0x001A7EA4
	public StoryInstance()
	{
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001A9CBE File Offset: 0x001A7EBE
	public StoryInstance(Story story, int worldId)
	{
		this._story = story;
		this.storyId = story.Id;
		this.worldId = worldId;
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x001A9CF2 File Offset: 0x001A7EF2
	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		return this.popupDisplayedStates != null && this.popupDisplayedStates.Contains(type);
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x001A9D0C File Offset: 0x001A7F0C
	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		this.EventInfo = eventInfo;
		this.Notification = notification;
		this.PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(this.OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x001A9D64 File Offset: 0x001A7F64
	private void OnPopupDisplayed()
	{
		if (this.popupDisplayedStates == null)
		{
			this.popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		this.popupDisplayedStates.Add(this.PendingType);
		this.EventInfo = null;
		this.Notification = null;
		this.PendingType = EventInfoDataHelper.PopupType.NONE;
	}

	// Token: 0x0400318C RID: 12684
	public Action<StoryInstance.State> StoryStateChanged;

	// Token: 0x0400318D RID: 12685
	[Serialize]
	public readonly string storyId;

	// Token: 0x0400318E RID: 12686
	[Serialize]
	public int worldId;

	// Token: 0x0400318F RID: 12687
	[Serialize]
	private StoryInstance.State state;

	// Token: 0x04003190 RID: 12688
	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	// Token: 0x04003191 RID: 12689
	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	// Token: 0x04003195 RID: 12693
	private Story _story;

	// Token: 0x02001872 RID: 6258
	public enum State
	{
		// Token: 0x04007203 RID: 29187
		RETROFITTED = -1,
		// Token: 0x04007204 RID: 29188
		NOT_STARTED,
		// Token: 0x04007205 RID: 29189
		DISCOVERED,
		// Token: 0x04007206 RID: 29190
		IN_PROGRESS,
		// Token: 0x04007207 RID: 29191
		COMPLETE
	}
}
