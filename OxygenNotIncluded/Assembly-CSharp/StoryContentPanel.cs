using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C73 RID: 3187
public class StoryContentPanel : KMonoBehaviour
{
	// Token: 0x060065A5 RID: 26021 RVA: 0x0025E63C File Offset: 0x0025C83C
	public List<string> GetActiveStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x060065A6 RID: 26022 RVA: 0x0025E6A8 File Offset: 0x0025C8A8
	public void Init()
	{
		this.SpawnRows();
		this.RefreshRows();
		this.RefreshDescriptionPanel();
		this.SelectDefault();
		CustomGameSettings.Instance.OnStorySettingChanged += this.OnStorySettingChanged;
	}

	// Token: 0x060065A7 RID: 26023 RVA: 0x0025E6D8 File Offset: 0x0025C8D8
	public void Cleanup()
	{
		CustomGameSettings.Instance.OnStorySettingChanged -= this.OnStorySettingChanged;
	}

	// Token: 0x060065A8 RID: 26024 RVA: 0x0025E6F0 File Offset: 0x0025C8F0
	private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
	{
		this.storyStates[config.id] = ((level.id == "Guaranteed") ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		this.RefreshStoryDisplay(config.id);
	}

	// Token: 0x060065A9 RID: 26025 RVA: 0x0025E728 File Offset: 0x0025C928
	private void SpawnRows()
	{
		using (List<Story>.Enumerator enumerator = Db.Get().Stories.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Story story = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.storyRowPrefab, this.storyRowContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(Strings.Get(story.StoryTrait.name));
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
				{
					this.SelectRow(story.Id);
				}));
				this.storyRows.Add(story.Id, gameObject);
				component.GetReference<Image>("Icon").sprite = Assets.GetSprite(story.StoryTrait.icon);
				MultiToggle reference = component.GetReference<MultiToggle>("checkbox");
				reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
				{
					this.IncrementStorySetting(story.Id, true);
					this.RefreshStoryDisplay(story.Id);
				}));
				this.storyStates.Add(story.Id, this._defaultStoryState);
			}
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshStoryLabel();
	}

	// Token: 0x060065AA RID: 26026 RVA: 0x0025E8A4 File Offset: 0x0025CAA4
	private void SelectRow(string id)
	{
		this.selectedStoryId = id;
		this.RefreshRows();
		this.RefreshDescriptionPanel();
	}

	// Token: 0x060065AB RID: 26027 RVA: 0x0025E8BC File Offset: 0x0025CABC
	public void SelectDefault()
	{
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				this.SelectRow(keyValuePair.Key);
				return;
			}
		}
		using (Dictionary<string, StoryContentPanel.StoryState>.Enumerator enumerator = this.storyStates.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 = enumerator.Current;
				this.SelectRow(keyValuePair2.Key);
			}
		}
	}

	// Token: 0x060065AC RID: 26028 RVA: 0x0025E96C File Offset: 0x0025CB6C
	private void IncrementStorySetting(string storyId, bool forward = true)
	{
		int num = (int)this.storyStates[storyId];
		num += (forward ? 1 : -1);
		if (num < 0)
		{
			num += 2;
		}
		num %= 2;
		this.SetStoryState(storyId, (StoryContentPanel.StoryState)num);
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x060065AD RID: 26029 RVA: 0x0025E9B0 File Offset: 0x0025CBB0
	private void SetStoryState(string storyId, StoryContentPanel.StoryState state)
	{
		this.storyStates[storyId] = state;
		SettingConfig config = CustomGameSettings.Instance.StorySettings[storyId];
		CustomGameSettings.Instance.SetStorySetting(config, this.storyStates[storyId] == StoryContentPanel.StoryState.Guaranteed);
	}

	// Token: 0x060065AE RID: 26030 RVA: 0x0025E9F8 File Offset: 0x0025CBF8
	public void SelectRandomStories(int min = 4, int max = 4, bool useBias = false)
	{
		int num = UnityEngine.Random.Range(min, max);
		List<Story> list = new List<Story>(Db.Get().Stories.resources);
		List<Story> list2 = new List<Story>();
		list.Shuffle<Story>();
		int num2 = 0;
		while (num2 < num && list.Count - 1 >= num2)
		{
			list2.Add(list[num2]);
			num2++;
		}
		float num3 = 0.7f;
		int num4 = list2.Count((Story x) => x.IsNew());
		if (useBias && num4 == 0 && UnityEngine.Random.value < num3)
		{
			List<Story> list3 = (from x in Db.Get().Stories.resources
			where x.IsNew()
			select x).ToList<Story>();
			list3.Shuffle<Story>();
			if (list3.Count > 0)
			{
				list2.RemoveAt(0);
				list2.Add(list3[0]);
			}
		}
		foreach (Story story in list)
		{
			this.SetStoryState(story.Id, list2.Contains(story) ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x060065AF RID: 26031 RVA: 0x0025EB5C File Offset: 0x0025CD5C
	private void RefreshAllStoryStates()
	{
		foreach (string id in this.storyRows.Keys)
		{
			this.RefreshStoryDisplay(id);
		}
	}

	// Token: 0x060065B0 RID: 26032 RVA: 0x0025EBB4 File Offset: 0x0025CDB4
	private void RefreshStoryDisplay(string id)
	{
		MultiToggle reference = this.storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
		StoryContentPanel.StoryState storyState = this.storyStates[id];
		if (storyState == StoryContentPanel.StoryState.Forbidden)
		{
			reference.ChangeState(0);
			return;
		}
		if (storyState != StoryContentPanel.StoryState.Guaranteed)
		{
			return;
		}
		reference.ChangeState(1);
	}

	// Token: 0x060065B1 RID: 26033 RVA: 0x0025EC04 File Offset: 0x0025CE04
	private void RefreshRows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storyRows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.selectedStoryId) ? 1 : 0);
		}
	}

	// Token: 0x060065B2 RID: 26034 RVA: 0x0025EC7C File Offset: 0x0025CE7C
	private void RefreshDescriptionPanel()
	{
		if (this.selectedStoryId.IsNullOrWhiteSpace())
		{
			this.selectedStoryTitleLabel.SetText("");
			this.selectedStoryDescriptionLabel.SetText("");
			return;
		}
		WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(this.selectedStoryId, true);
		this.selectedStoryTitleLabel.SetText(Strings.Get(storyTrait.name));
		this.selectedStoryDescriptionLabel.SetText(Strings.Get(storyTrait.description));
		string s = storyTrait.icon.Replace("_icon", "_image");
		this.selectedStoryImage.sprite = Assets.GetSprite(s);
	}

	// Token: 0x060065B3 RID: 26035 RVA: 0x0025ED30 File Offset: 0x0025CF30
	public string GetTraitsString(bool tooltip = false)
	{
		int num = 0;
		int num2 = 4;
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				num++;
			}
		}
		string text = UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
		string str;
		if (num != 0)
		{
			if (num != 1)
			{
				str = string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, num);
			}
			else
			{
				str = UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT;
			}
		}
		else
		{
			str = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		text = text + ": " + str;
		if (num > num2)
		{
			text = text + " " + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING;
		}
		if (tooltip)
		{
			foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 in this.storyStates)
			{
				if (keyValuePair2.Value == StoryContentPanel.StoryState.Guaranteed)
				{
					WorldTrait storyTrait = Db.Get().Stories.Get(keyValuePair2.Key).StoryTrait;
					text = string.Concat(new string[]
					{
						text,
						"\n\n<b>",
						Strings.Get(storyTrait.name).String,
						"</b>\n",
						Strings.Get(storyTrait.description).String
					});
				}
			}
			if (num > num2)
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP;
			}
		}
		return text;
	}

	// Token: 0x040045F3 RID: 17907
	[SerializeField]
	private GameObject storyRowPrefab;

	// Token: 0x040045F4 RID: 17908
	[SerializeField]
	private GameObject storyRowContainer;

	// Token: 0x040045F5 RID: 17909
	private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();

	// Token: 0x040045F6 RID: 17910
	public const int DEFAULT_RANDOMIZE_STORY_COUNT = 4;

	// Token: 0x040045F7 RID: 17911
	private Dictionary<string, StoryContentPanel.StoryState> storyStates = new Dictionary<string, StoryContentPanel.StoryState>();

	// Token: 0x040045F8 RID: 17912
	private string selectedStoryId = "";

	// Token: 0x040045F9 RID: 17913
	[SerializeField]
	private ColonyDestinationSelectScreen mainScreen;

	// Token: 0x040045FA RID: 17914
	[Header("Trait Count")]
	[Header("SelectedStory")]
	[SerializeField]
	private Image selectedStoryImage;

	// Token: 0x040045FB RID: 17915
	[SerializeField]
	private LocText selectedStoryTitleLabel;

	// Token: 0x040045FC RID: 17916
	[SerializeField]
	private LocText selectedStoryDescriptionLabel;

	// Token: 0x040045FD RID: 17917
	[SerializeField]
	private Sprite spriteForbidden;

	// Token: 0x040045FE RID: 17918
	[SerializeField]
	private Sprite spritePossible;

	// Token: 0x040045FF RID: 17919
	[SerializeField]
	private Sprite spriteGuaranteed;

	// Token: 0x04004600 RID: 17920
	private StoryContentPanel.StoryState _defaultStoryState;

	// Token: 0x04004601 RID: 17921
	private List<string> storyTraitSettings = new List<string>
	{
		"None",
		"Few",
		"Lots"
	};

	// Token: 0x02001BAA RID: 7082
	private enum StoryState
	{
		// Token: 0x04007D67 RID: 32103
		Forbidden,
		// Token: 0x04007D68 RID: 32104
		Guaranteed,
		// Token: 0x04007D69 RID: 32105
		LENGTH
	}
}
