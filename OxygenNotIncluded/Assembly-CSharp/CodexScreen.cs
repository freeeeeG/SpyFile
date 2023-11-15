using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AC1 RID: 2753
public class CodexScreen : KScreen
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x060054BE RID: 21694 RVA: 0x001ECFE2 File Offset: 0x001EB1E2
	// (set) Token: 0x060054BF RID: 21695 RVA: 0x001ECFEA File Offset: 0x001EB1EA
	public string activeEntryID
	{
		get
		{
			return this._activeEntryID;
		}
		private set
		{
			this._activeEntryID = value;
		}
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x001ECFF4 File Offset: 0x001EB1F4
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = true;
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		if (string.IsNullOrEmpty(this.activeEntryID))
		{
			this.ChangeArticle("HOME", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		}
		this.searchInputField.onValueChanged.AddListener(delegate(string value)
		{
			this.FilterSearch(value);
		});
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			this.editingSearch = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			this.editingSearch = false;
		});
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x001ED0D1 File Offset: 0x001EB2D1
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.editingSearch)
		{
			e.Consumed = true;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x001ED0E9 File Offset: 0x001EB2E9
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x001ED0F0 File Offset: 0x001EB2F0
	public void RefreshTutorialMessages()
	{
		if (!this.HasFocus)
		{
			return;
		}
		string key = CodexCache.FormatLinkID("MISCELLANEOUSTIPS");
		CodexEntry codexEntry;
		if (CodexCache.entries.TryGetValue(key, out codexEntry))
		{
			for (int i = 0; i < codexEntry.subEntries.Count; i++)
			{
				for (int j = 0; j < codexEntry.subEntries[i].contentContainers.Count; j++)
				{
					for (int k = 0; k < codexEntry.subEntries[i].contentContainers[j].content.Count; k++)
					{
						CodexText codexText = codexEntry.subEntries[i].contentContainers[j].content[k] as CodexText;
						if (codexText != null && codexText.messageID == MISC.NOTIFICATIONS.BASICCONTROLS.NAME)
						{
							if (KInputManager.currentControllerIsGamepad)
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODYALT;
							}
							else
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY;
							}
							if (!string.IsNullOrEmpty(this.activeEntryID))
							{
								this.ChangeArticle("MISCELLANEOUSTIPS0", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x001ED234 File Offset: 0x001EB434
	private void CodexScreenInit()
	{
		this.textStyles[CodexTextStyle.Title] = this.textStyleTitle;
		this.textStyles[CodexTextStyle.Subtitle] = this.textStyleSubtitle;
		this.textStyles[CodexTextStyle.Body] = this.textStyleBody;
		this.textStyles[CodexTextStyle.BodyWhite] = this.textStyleBodyWhite;
		this.SetupPrefabs();
		this.PopulatePools();
		this.CategorizeEntries();
		this.FilterSearch("");
		this.backButtonButton.onClick += this.HistoryStepBack;
		this.backButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx > 0);
		this.fwdButtonButton.onClick += this.HistoryStepForward;
		this.fwdButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx < this.history.Count - 1);
		Game.Instance.Subscribe(1594320620, delegate(object val)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.FilterSearch(this.searchInputField.text);
			if (!string.IsNullOrEmpty(this.activeEntryID))
			{
				this.ChangeArticle(this.activeEntryID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		});
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshTutorialMessages));
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x001ED340 File Offset: 0x001EB540
	private void SetupPrefabs()
	{
		this.contentContainerPool = new UIGameObjectPool(this.prefabContentContainer);
		this.contentContainerPool.disabledElementParent = this.widgetPool;
		this.ContentPrefabs[typeof(CodexText)] = this.prefabTextWidget;
		this.ContentPrefabs[typeof(CodexTextWithTooltip)] = this.prefabTextWithTooltipWidget;
		this.ContentPrefabs[typeof(CodexImage)] = this.prefabImageWidget;
		this.ContentPrefabs[typeof(CodexDividerLine)] = this.prefabDividerLineWidget;
		this.ContentPrefabs[typeof(CodexSpacer)] = this.prefabSpacer;
		this.ContentPrefabs[typeof(CodexLabelWithIcon)] = this.prefabLabelWithIcon;
		this.ContentPrefabs[typeof(CodexLabelWithLargeIcon)] = this.prefabLabelWithLargeIcon;
		this.ContentPrefabs[typeof(CodexContentLockedIndicator)] = this.prefabContentLocked;
		this.ContentPrefabs[typeof(CodexLargeSpacer)] = this.prefabLargeSpacer;
		this.ContentPrefabs[typeof(CodexVideo)] = this.prefabVideoWidget;
		this.ContentPrefabs[typeof(CodexIndentedLabelWithIcon)] = this.prefabIndentedLabelWithIcon;
		this.ContentPrefabs[typeof(CodexRecipePanel)] = this.prefabRecipePanel;
		this.ContentPrefabs[typeof(CodexConfigurableConsumerRecipePanel)] = this.PrefabConfigurableConsumerRecipePanel;
		this.ContentPrefabs[typeof(CodexTemperatureTransitionPanel)] = this.PrefabTemperatureTransitionPanel;
		this.ContentPrefabs[typeof(CodexConversionPanel)] = this.prefabConversionPanel;
		this.ContentPrefabs[typeof(CodexCollapsibleHeader)] = this.prefabCollapsibleHeader;
		this.ContentPrefabs[typeof(CodexCritterLifecycleWidget)] = this.prefabCritterLifecycleWidget;
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x001ED53C File Offset: 0x001EB73C
	private List<CodexEntry> FilterSearch(string input)
	{
		this.searchResults.Clear();
		this.subEntrySearchResults.Clear();
		input = input.ToLower();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			bool flag = false;
			string[] dlcIds = keyValuePair.Value.GetDlcIds();
			for (int i = 0; i < dlcIds.Length; i++)
			{
				if (DlcManager.IsContentActive(dlcIds[i]))
				{
					flag = true;
					break;
				}
			}
			string[] forbiddenDLCs = keyValuePair.Value.GetForbiddenDLCs();
			for (int j = 0; j < forbiddenDLCs.Length; j++)
			{
				if (DlcManager.IsContentActive(forbiddenDLCs[j]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (input == "")
				{
					if (!keyValuePair.Value.searchOnly)
					{
						this.searchResults.Add(keyValuePair.Value);
					}
				}
				else if (input == keyValuePair.Value.name.ToLower() || input.Contains(keyValuePair.Value.name.ToLower()) || keyValuePair.Value.name.ToLower().Contains(input))
				{
					this.searchResults.Add(keyValuePair.Value);
				}
			}
		}
		foreach (KeyValuePair<string, SubEntry> keyValuePair2 in CodexCache.subEntries)
		{
			if (input == keyValuePair2.Value.name.ToLower() || input.Contains(keyValuePair2.Value.name.ToLower()) || keyValuePair2.Value.name.ToLower().Contains(input))
			{
				this.subEntrySearchResults.Add(keyValuePair2.Value);
			}
		}
		this.FilterEntries(input != "");
		return this.searchResults;
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x001ED768 File Offset: 0x001EB968
	private bool HasUnlockedCategoryEntries(string entryID)
	{
		foreach (ContentContainer contentContainer in CodexCache.entries[entryID].contentContainers)
		{
			if (string.IsNullOrEmpty(contentContainer.lockID) || Game.Instance.unlocks.IsUnlocked(contentContainer.lockID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x001ED7EC File Offset: 0x001EB9EC
	private void FilterEntries(bool allowOpenCategories = true)
	{
		foreach (KeyValuePair<CodexEntry, GameObject> keyValuePair in this.entryButtons)
		{
			keyValuePair.Value.SetActive(this.searchResults.Contains(keyValuePair.Key) && this.HasUnlockedCategoryEntries(keyValuePair.Key.id));
		}
		foreach (KeyValuePair<SubEntry, GameObject> keyValuePair2 in this.subEntryButtons)
		{
			keyValuePair2.Value.SetActive(this.subEntrySearchResults.Contains(keyValuePair2.Key));
		}
		foreach (GameObject gameObject in this.categoryHeaders)
		{
			bool flag = false;
			Transform transform = gameObject.transform.Find("Content");
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeSelf)
				{
					flag = true;
				}
			}
			gameObject.SetActive(flag);
			if (allowOpenCategories)
			{
				if (flag)
				{
					this.ToggleCategoryOpen(gameObject, true);
				}
			}
			else
			{
				this.ToggleCategoryOpen(gameObject, false);
			}
		}
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x001ED96C File Offset: 0x001EBB6C
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		header.GetComponent<HierarchyReferences>().GetReference("Content").gameObject.SetActive(open);
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x001ED9A8 File Offset: 0x001EBBA8
	private void PopulatePools()
	{
		foreach (KeyValuePair<Type, GameObject> keyValuePair in this.ContentPrefabs)
		{
			UIGameObjectPool uigameObjectPool = new UIGameObjectPool(keyValuePair.Value);
			uigameObjectPool.disabledElementParent = this.widgetPool;
			this.ContentUIPools[keyValuePair.Key] = uigameObjectPool;
		}
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x001EDA20 File Offset: 0x001EBC20
	private GameObject NewCategoryHeader(KeyValuePair<string, CodexEntry> entryKVP, Dictionary<string, GameObject> categories)
	{
		if (entryKVP.Value.category == "")
		{
			entryKVP.Value.category = "Root";
		}
		GameObject categoryHeader = Util.KInstantiateUI(this.prefabCategoryHeader, this.navigatorContent.gameObject, true);
		GameObject categoryContent = categoryHeader.GetComponent<HierarchyReferences>().GetReference("Content").gameObject;
		categories.Add(entryKVP.Value.category, categoryContent);
		LocText reference = categoryHeader.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
		if (CodexCache.entries.ContainsKey(entryKVP.Value.category))
		{
			reference.text = CodexCache.entries[entryKVP.Value.category].name;
		}
		else
		{
			reference.text = Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES." + entryKVP.Value.category.ToUpper());
		}
		this.categoryHeaders.Add(categoryHeader);
		categoryContent.SetActive(false);
		categoryHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").onClick = delegate()
		{
			this.ToggleCategoryOpen(categoryHeader, !categoryContent.activeSelf);
		};
		return categoryHeader;
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x001EDB80 File Offset: 0x001EBD80
	private void CategorizeEntries()
	{
		GameObject gameObject = this.navigatorContent.gameObject;
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		List<global::Tuple<string, CodexEntry>> list = new List<global::Tuple<string, CodexEntry>>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value.sortString))
			{
				keyValuePair.Value.sortString = UI.StripLinkFormatting(Strings.Get(keyValuePair.Value.title));
			}
			list.Add(new global::Tuple<string, CodexEntry>(keyValuePair.Key, keyValuePair.Value));
		}
		list.Sort((global::Tuple<string, CodexEntry> a, global::Tuple<string, CodexEntry> b) => string.Compare(a.second.sortString, b.second.sortString));
		for (int i = 0; i < list.Count; i++)
		{
			global::Tuple<string, CodexEntry> tuple = list[i];
			string text = tuple.second.category;
			if (text == "" || text == "Root")
			{
				text = "Root";
			}
			if (!dictionary.ContainsKey(text))
			{
				this.NewCategoryHeader(new KeyValuePair<string, CodexEntry>(tuple.first, tuple.second), dictionary);
			}
			GameObject gameObject2 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
			string id = tuple.second.id;
			gameObject2.GetComponent<KButton>().onClick += delegate()
			{
				this.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
			if (string.IsNullOrEmpty(tuple.second.name))
			{
				tuple.second.name = Strings.Get(tuple.second.title);
			}
			gameObject2.GetComponentInChildren<LocText>().text = tuple.second.name;
			this.entryButtons.Add(tuple.second, gameObject2);
			foreach (SubEntry subEntry in tuple.second.subEntries)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
				string subEntryId = subEntry.id;
				gameObject3.GetComponent<KButton>().onClick += delegate()
				{
					this.ChangeArticle(subEntryId, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
				};
				if (string.IsNullOrEmpty(subEntry.name))
				{
					subEntry.name = Strings.Get(subEntry.title);
				}
				gameObject3.GetComponentInChildren<LocText>().text = subEntry.name;
				this.subEntryButtons.Add(subEntry, gameObject3);
				CodexCache.subEntries.Add(subEntry.id, subEntry);
			}
		}
		foreach (KeyValuePair<string, CodexEntry> keyValuePair2 in CodexCache.entries)
		{
			if (CodexCache.entries.ContainsKey(keyValuePair2.Value.category) && CodexCache.entries.ContainsKey(CodexCache.entries[keyValuePair2.Value.category].category))
			{
				keyValuePair2.Value.searchOnly = true;
			}
		}
		List<KeyValuePair<string, GameObject>> list2 = new List<KeyValuePair<string, GameObject>>();
		foreach (KeyValuePair<string, GameObject> item in dictionary)
		{
			list2.Add(item);
		}
		list2.Sort((KeyValuePair<string, GameObject> a, KeyValuePair<string, GameObject> b) => string.Compare(a.Value.name, b.Value.name));
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j].Value.transform.parent.SetSiblingIndex(j);
		}
		CodexScreen.SetupCategory(dictionary, "PLANTS");
		CodexScreen.SetupCategory(dictionary, "CREATURES");
		CodexScreen.SetupCategory(dictionary, "NOTICES");
		CodexScreen.SetupCategory(dictionary, "RESEARCHNOTES");
		CodexScreen.SetupCategory(dictionary, "JOURNALS");
		CodexScreen.SetupCategory(dictionary, "EMAILS");
		CodexScreen.SetupCategory(dictionary, "INVESTIGATIONS");
		CodexScreen.SetupCategory(dictionary, "MYLOG");
		CodexScreen.SetupCategory(dictionary, "CREATURES::GeneralInfo");
		CodexScreen.SetupCategory(dictionary, "LESSONS");
		CodexScreen.SetupCategory(dictionary, "Root");
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x001EE020 File Offset: 0x001EC220
	private static void SetupCategory(Dictionary<string, GameObject> categories, string category_name)
	{
		if (!categories.ContainsKey(category_name))
		{
			return;
		}
		categories[category_name].transform.parent.SetAsFirstSibling();
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x001EE044 File Offset: 0x001EC244
	public void ChangeArticle(string id, bool playClickSound = false, Vector3 targetPosition = default(Vector3), CodexScreen.HistoryDirection historyMovement = CodexScreen.HistoryDirection.NewArticle)
	{
		global::Debug.Assert(id != null);
		if (playClickSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		}
		if (this.contentContainerPool == null)
		{
			this.CodexScreenInit();
		}
		string text = "";
		SubEntry subEntry = null;
		if (!CodexCache.entries.ContainsKey(id))
		{
			subEntry = CodexCache.FindSubEntry(id);
			if (subEntry != null && !subEntry.disabled)
			{
				id = subEntry.parentEntryID.ToUpper();
				text = UI.StripLinkFormatting(subEntry.name);
			}
			else
			{
				id = "PAGENOTFOUND";
			}
		}
		if (CodexCache.entries[id].disabled)
		{
			id = "PAGENOTFOUND";
		}
		if (string.IsNullOrEmpty(text))
		{
			text = UI.StripLinkFormatting(CodexCache.entries[id].name);
		}
		ICodexWidget codexWidget = null;
		CodexCache.entries[id].GetFirstWidget();
		RectTransform rectTransform = null;
		if (subEntry != null)
		{
			foreach (ContentContainer contentContainer in CodexCache.entries[id].contentContainers)
			{
				if (contentContainer == subEntry.contentContainers[0])
				{
					codexWidget = contentContainer.content[0];
					break;
				}
			}
		}
		int num = 0;
		string text2 = "";
		while (this.contentContainers.transform.childCount > 0)
		{
			while (!string.IsNullOrEmpty(text2) && CodexCache.entries[this.activeEntryID].contentContainers[num].lockID == text2)
			{
				num++;
			}
			GameObject gameObject = this.contentContainers.transform.GetChild(0).gameObject;
			int num2 = 0;
			while (gameObject.transform.childCount > 0)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
				Type key;
				if (gameObject2.name == "PrefabContentLocked")
				{
					text2 = CodexCache.entries[this.activeEntryID].contentContainers[num].lockID;
					key = typeof(CodexContentLockedIndicator);
				}
				else
				{
					key = CodexCache.entries[this.activeEntryID].contentContainers[num].content[num2].GetType();
				}
				this.ContentUIPools[key].ClearElement(gameObject2);
				num2++;
			}
			this.contentContainerPool.ClearElement(this.contentContainers.transform.GetChild(0).gameObject);
			num++;
		}
		bool flag = CodexCache.entries[id] is CategoryEntry;
		this.activeEntryID = id;
		if (CodexCache.entries[id].contentContainers == null)
		{
			CodexCache.entries[id].CreateContentContainerCollection();
		}
		bool flag2 = false;
		string a = "";
		for (int i = 0; i < CodexCache.entries[id].contentContainers.Count; i++)
		{
			ContentContainer contentContainer2 = CodexCache.entries[id].contentContainers[i];
			if (!string.IsNullOrEmpty(contentContainer2.lockID) && !Game.Instance.unlocks.IsUnlocked(contentContainer2.lockID))
			{
				if (a != contentContainer2.lockID)
				{
					GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
					this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
					a = contentContainer2.lockID;
					GameObject gameObject4 = this.ContentUIPools[typeof(CodexContentLockedIndicator)].GetFreeElement(gameObject3, true).gameObject;
				}
			}
			else
			{
				GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
				this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
				flag2 = !flag2;
				if (contentContainer2.content != null)
				{
					foreach (ICodexWidget codexWidget2 in contentContainer2.content)
					{
						GameObject gameObject5 = this.ContentUIPools[codexWidget2.GetType()].GetFreeElement(gameObject3, true).gameObject;
						codexWidget2.Configure(gameObject5, this.displayPane, this.textStyles);
						if (codexWidget2 == codexWidget)
						{
							rectTransform = gameObject5.rectTransform();
						}
					}
				}
			}
		}
		string text3 = "";
		string text4 = id;
		int num3 = 0;
		while (text4 != CodexCache.FormatLinkID("HOME") && num3 < 10)
		{
			num3++;
			if (text4 != null)
			{
				if (text4 != id)
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
				}
				else
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name);
				}
				text4 = CodexCache.entries[text4].parentId;
			}
			else
			{
				text4 = CodexCache.entries[CodexCache.FormatLinkID("HOME")].id;
				text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
			}
		}
		this.currentLocationText.text = ((text3 == "") ? ("<b>" + UI.StripLinkFormatting(CodexCache.entries["HOME"].name) + "</b>") : text3);
		if (this.history.Count == 0)
		{
			this.history.Add(new CodexScreen.HistoryEntry(id, Vector3.zero, text));
			this.currentHistoryIdx = 0;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Back)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx--;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Forward)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx++;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.NewArticle || historyMovement == CodexScreen.HistoryDirection.Up)
		{
			if (this.currentHistoryIdx == this.history.Count - 1)
			{
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
			else
			{
				for (int j = this.history.Count - 1; j > this.currentHistoryIdx; j--)
				{
					this.history.RemoveAt(j);
				}
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.history.Count - 2].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
		}
		if (this.currentHistoryIdx > 0)
		{
			this.backButtonButton.GetComponent<Image>().color = Color.black;
			this.backButton.text = UI.FormatAsLink(string.Format(UI.CODEX.BACK_BUTTON, UI.StripLinkFormatting(CodexCache.entries[this.history[this.history.Count - 2].id].name)), CodexCache.entries[this.history[this.history.Count - 2].id].id);
			this.backButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.BACK_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx - 1].name);
		}
		else
		{
			this.backButtonButton.GetComponent<Image>().color = Color.grey;
			this.backButton.text = UI.StripLinkFormatting(GameUtil.ColourizeString(Color.grey, string.Format(UI.CODEX.BACK_BUTTON, CodexCache.entries["HOME"].name)));
			this.backButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.BACK_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (this.currentHistoryIdx < this.history.Count - 1)
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.black;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.FORWARD_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx + 1].name);
		}
		else
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.grey;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.FORWARD_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (targetPosition != Vector3.zero)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(targetPosition));
			return;
		}
		if (rectTransform != null)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
			return;
		}
		this.displayScrollRect.content.SetLocalPosition(Vector3.zero);
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x001EE9D8 File Offset: 0x001ECBD8
	private void HistoryStepBack()
	{
		if (this.currentHistoryIdx == 0)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx - 1].id, false, this.history[this.currentHistoryIdx - 1].position, CodexScreen.HistoryDirection.Back);
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x001EEA28 File Offset: 0x001ECC28
	private void HistoryStepForward()
	{
		if (this.currentHistoryIdx == this.history.Count - 1)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx + 1].id, false, this.history[this.currentHistoryIdx + 1].position, CodexScreen.HistoryDirection.Forward);
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x001EEA84 File Offset: 0x001ECC84
	private void HistoryStepUp()
	{
		if (string.IsNullOrEmpty(CodexCache.entries[this.activeEntryID].parentId))
		{
			return;
		}
		this.ChangeArticle(CodexCache.entries[this.activeEntryID].parentId, false, default(Vector3), CodexScreen.HistoryDirection.Up);
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x001EEAD4 File Offset: 0x001ECCD4
	private IEnumerator ScrollToTarget(RectTransform targetWidgetTransform)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(Vector3.down * (this.displayScrollRect.content.InverseTransformPoint(targetWidgetTransform.GetPosition()).y + 12f));
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x060054D3 RID: 21715 RVA: 0x001EEAEA File Offset: 0x001ECCEA
	private IEnumerator ScrollToTarget(Vector3 position)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(position);
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x001EEB00 File Offset: 0x001ECD00
	public void FocusContainer(ContentContainer target)
	{
		if (target == null || target.go == null)
		{
			return;
		}
		RectTransform rectTransform = target.go.transform.GetChild(0) as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		if (this.scrollToTargetRoutine != null)
		{
			base.StopCoroutine(this.scrollToTargetRoutine);
		}
		this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x001EEB68 File Offset: 0x001ECD68
	private void ConfigureContentContainer(ContentContainer container, GameObject containerGameObject, bool bgColor = false)
	{
		container.go = containerGameObject;
		LayoutGroup layoutGroup = containerGameObject.GetComponent<LayoutGroup>();
		if (layoutGroup != null)
		{
			UnityEngine.Object.DestroyImmediate(layoutGroup);
		}
		if (!Game.Instance.unlocks.IsUnlocked(container.lockID) && !string.IsNullOrEmpty(container.lockID))
		{
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		}
		switch (container.contentLayout)
		{
		case ContentContainer.ContentLayout.Vertical:
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Horizontal:
			layoutGroup = containerGameObject.AddComponent<HorizontalLayoutGroup>();
			layoutGroup.childAlignment = TextAnchor.MiddleLeft;
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Grid:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 4;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(128f, 180f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(6f, 6f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumn:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 32f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumnTall:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 64f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400387F RID: 14463
	private string _activeEntryID;

	// Token: 0x04003880 RID: 14464
	private Dictionary<Type, UIGameObjectPool> ContentUIPools = new Dictionary<Type, UIGameObjectPool>();

	// Token: 0x04003881 RID: 14465
	private Dictionary<Type, GameObject> ContentPrefabs = new Dictionary<Type, GameObject>();

	// Token: 0x04003882 RID: 14466
	private List<GameObject> categoryHeaders = new List<GameObject>();

	// Token: 0x04003883 RID: 14467
	private Dictionary<CodexEntry, GameObject> entryButtons = new Dictionary<CodexEntry, GameObject>();

	// Token: 0x04003884 RID: 14468
	private Dictionary<SubEntry, GameObject> subEntryButtons = new Dictionary<SubEntry, GameObject>();

	// Token: 0x04003885 RID: 14469
	private UIGameObjectPool contentContainerPool;

	// Token: 0x04003886 RID: 14470
	[SerializeField]
	private KScrollRect displayScrollRect;

	// Token: 0x04003887 RID: 14471
	[SerializeField]
	private RectTransform scrollContentPane;

	// Token: 0x04003888 RID: 14472
	private bool editingSearch;

	// Token: 0x04003889 RID: 14473
	private List<CodexScreen.HistoryEntry> history = new List<CodexScreen.HistoryEntry>();

	// Token: 0x0400388A RID: 14474
	private int currentHistoryIdx;

	// Token: 0x0400388B RID: 14475
	[Header("Hierarchy")]
	[SerializeField]
	private Transform navigatorContent;

	// Token: 0x0400388C RID: 14476
	[SerializeField]
	private Transform displayPane;

	// Token: 0x0400388D RID: 14477
	[SerializeField]
	private Transform contentContainers;

	// Token: 0x0400388E RID: 14478
	[SerializeField]
	private Transform widgetPool;

	// Token: 0x0400388F RID: 14479
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003890 RID: 14480
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x04003891 RID: 14481
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04003892 RID: 14482
	[SerializeField]
	private LocText backButton;

	// Token: 0x04003893 RID: 14483
	[SerializeField]
	private KButton backButtonButton;

	// Token: 0x04003894 RID: 14484
	[SerializeField]
	private KButton fwdButtonButton;

	// Token: 0x04003895 RID: 14485
	[SerializeField]
	private LocText currentLocationText;

	// Token: 0x04003896 RID: 14486
	[Header("Prefabs")]
	[SerializeField]
	private GameObject prefabNavigatorEntry;

	// Token: 0x04003897 RID: 14487
	[SerializeField]
	private GameObject prefabCategoryHeader;

	// Token: 0x04003898 RID: 14488
	[SerializeField]
	private GameObject prefabContentContainer;

	// Token: 0x04003899 RID: 14489
	[SerializeField]
	private GameObject prefabTextWidget;

	// Token: 0x0400389A RID: 14490
	[SerializeField]
	private GameObject prefabTextWithTooltipWidget;

	// Token: 0x0400389B RID: 14491
	[SerializeField]
	private GameObject prefabImageWidget;

	// Token: 0x0400389C RID: 14492
	[SerializeField]
	private GameObject prefabDividerLineWidget;

	// Token: 0x0400389D RID: 14493
	[SerializeField]
	private GameObject prefabSpacer;

	// Token: 0x0400389E RID: 14494
	[SerializeField]
	private GameObject prefabLargeSpacer;

	// Token: 0x0400389F RID: 14495
	[SerializeField]
	private GameObject prefabLabelWithIcon;

	// Token: 0x040038A0 RID: 14496
	[SerializeField]
	private GameObject prefabLabelWithLargeIcon;

	// Token: 0x040038A1 RID: 14497
	[SerializeField]
	private GameObject prefabContentLocked;

	// Token: 0x040038A2 RID: 14498
	[SerializeField]
	private GameObject prefabVideoWidget;

	// Token: 0x040038A3 RID: 14499
	[SerializeField]
	private GameObject prefabIndentedLabelWithIcon;

	// Token: 0x040038A4 RID: 14500
	[SerializeField]
	private GameObject prefabRecipePanel;

	// Token: 0x040038A5 RID: 14501
	[SerializeField]
	private GameObject PrefabConfigurableConsumerRecipePanel;

	// Token: 0x040038A6 RID: 14502
	[SerializeField]
	private GameObject PrefabTemperatureTransitionPanel;

	// Token: 0x040038A7 RID: 14503
	[SerializeField]
	private GameObject prefabConversionPanel;

	// Token: 0x040038A8 RID: 14504
	[SerializeField]
	private GameObject prefabCollapsibleHeader;

	// Token: 0x040038A9 RID: 14505
	[SerializeField]
	private GameObject prefabCritterLifecycleWidget;

	// Token: 0x040038AA RID: 14506
	[Header("Text Styles")]
	[SerializeField]
	private TextStyleSetting textStyleTitle;

	// Token: 0x040038AB RID: 14507
	[SerializeField]
	private TextStyleSetting textStyleSubtitle;

	// Token: 0x040038AC RID: 14508
	[SerializeField]
	private TextStyleSetting textStyleBody;

	// Token: 0x040038AD RID: 14509
	[SerializeField]
	private TextStyleSetting textStyleBodyWhite;

	// Token: 0x040038AE RID: 14510
	private Dictionary<CodexTextStyle, TextStyleSetting> textStyles = new Dictionary<CodexTextStyle, TextStyleSetting>();

	// Token: 0x040038AF RID: 14511
	private List<CodexEntry> searchResults = new List<CodexEntry>();

	// Token: 0x040038B0 RID: 14512
	private List<SubEntry> subEntrySearchResults = new List<SubEntry>();

	// Token: 0x040038B1 RID: 14513
	private Coroutine scrollToTargetRoutine;

	// Token: 0x020019DE RID: 6622
	public enum PlanCategory
	{
		// Token: 0x04007796 RID: 30614
		Home,
		// Token: 0x04007797 RID: 30615
		Tips,
		// Token: 0x04007798 RID: 30616
		MyLog,
		// Token: 0x04007799 RID: 30617
		Investigations,
		// Token: 0x0400779A RID: 30618
		Emails,
		// Token: 0x0400779B RID: 30619
		Journals,
		// Token: 0x0400779C RID: 30620
		ResearchNotes,
		// Token: 0x0400779D RID: 30621
		Creatures,
		// Token: 0x0400779E RID: 30622
		Plants,
		// Token: 0x0400779F RID: 30623
		Food,
		// Token: 0x040077A0 RID: 30624
		Tech,
		// Token: 0x040077A1 RID: 30625
		Diseases,
		// Token: 0x040077A2 RID: 30626
		Roles,
		// Token: 0x040077A3 RID: 30627
		Buildings,
		// Token: 0x040077A4 RID: 30628
		Elements
	}

	// Token: 0x020019DF RID: 6623
	public enum HistoryDirection
	{
		// Token: 0x040077A6 RID: 30630
		Back,
		// Token: 0x040077A7 RID: 30631
		Forward,
		// Token: 0x040077A8 RID: 30632
		Up,
		// Token: 0x040077A9 RID: 30633
		NewArticle
	}

	// Token: 0x020019E0 RID: 6624
	public class HistoryEntry
	{
		// Token: 0x0600957B RID: 38267 RVA: 0x0033A8AE File Offset: 0x00338AAE
		public HistoryEntry(string entry, Vector3 pos, string articleName)
		{
			this.id = entry;
			this.position = pos;
			this.name = articleName;
		}

		// Token: 0x040077AA RID: 30634
		public string id;

		// Token: 0x040077AB RID: 30635
		public Vector3 position;

		// Token: 0x040077AC RID: 30636
		public string name;
	}
}
