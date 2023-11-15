using System;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
public class EventInfoDataHelper
{
	// Token: 0x060056C2 RID: 22210 RVA: 0x001FB1C0 File Offset: 0x001F93C0
	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, EventInfoDataHelper.PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if (popupType == EventInfoDataHelper.PopupType.BEGIN)
		{
			eventInfoData.showCallback = delegate()
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup", false));
			};
		}
		if (popupType == EventInfoDataHelper.PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate()
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock", false);
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText, null);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}

	// Token: 0x02001A0B RID: 6667
	public enum PopupType
	{
		// Token: 0x04007825 RID: 30757
		NONE = -1,
		// Token: 0x04007826 RID: 30758
		BEGIN,
		// Token: 0x04007827 RID: 30759
		NORMAL,
		// Token: 0x04007828 RID: 30760
		COMPLETE
	}
}
