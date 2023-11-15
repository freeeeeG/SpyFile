using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B9B RID: 2971
public class NameDisplayScreen : KScreen
{
	// Token: 0x06005C91 RID: 23697 RVA: 0x0021E8EF File Offset: 0x0021CAEF
	public static void DestroyInstance()
	{
		NameDisplayScreen.Instance = null;
	}

	// Token: 0x06005C92 RID: 23698 RVA: 0x0021E8F7 File Offset: 0x0021CAF7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NameDisplayScreen.Instance = this;
	}

	// Token: 0x06005C93 RID: 23699 RVA: 0x0021E908 File Offset: 0x0021CB08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(new Action<Health>(this.OnHealthAdded), null);
		Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), null);
		this.updateSectionIndex = 0;
		this.lateUpdateSections = new List<System.Action>
		{
			new System.Action(this.LateUpdatePart0),
			new System.Action(this.LateUpdatePart1),
			new System.Action(this.LateUpdatePart2)
		};
		this.BindOnOverlayChange();
		this.worldChangeEventHandle = Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
	}

	// Token: 0x06005C94 RID: 23700 RVA: 0x0021E9B8 File Offset: 0x0021CBB8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = false;
		}
		if (Game.Instance != null)
		{
			Game.Instance.Unsubscribe(this.worldChangeEventHandle);
		}
	}

	// Token: 0x06005C95 RID: 23701 RVA: 0x0021EA2C File Offset: 0x0021CC2C
	private void BindOnOverlayChange()
	{
		if (this.isOverlayChangeBound)
		{
			return;
		}
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = true;
		}
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x0021EA7C File Offset: 0x0021CC7C
	private void OnActiveWorldChanged(object data)
	{
		foreach (NameDisplayScreen.Entry entry in this.entries)
		{
			this.ToggleLabelTextForActiveWorld(entry);
		}
	}

	// Token: 0x06005C97 RID: 23703 RVA: 0x0021EAD0 File Offset: 0x0021CCD0
	public void RemoveWorldEntries(int worldId)
	{
		this.entries.RemoveAll((NameDisplayScreen.Entry entry) => entry.world_go.GetMyWorldId() == worldId);
	}

	// Token: 0x06005C98 RID: 23704 RVA: 0x0021EB02 File Offset: 0x0021CD02
	private void OnOverlayChanged(HashedString new_mode)
	{
		HashedString hashedString = this.lastKnownOverlayID;
		this.lastKnownOverlayID = new_mode;
		this.nameDisplayCanvas.enabled = (this.lastKnownOverlayID == OverlayModes.None.ID);
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x0021EB2D File Offset: 0x0021CD2D
	private void ToggleLabelTextForActiveWorld(NameDisplayScreen.Entry entry)
	{
		if (entry.nameLabel != null)
		{
			entry.nameLabel.enabled = (entry.world_go.GetMyWorldId() == ClusterManager.Instance.activeWorldId);
		}
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x0021EB5F File Offset: 0x0021CD5F
	private void OnHealthAdded(Health health)
	{
		this.RegisterComponent(health.gameObject, health, false);
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x0021EB70 File Offset: 0x0021CD70
	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject)
		{
			this.RegisterComponent(targetGameObject, equipment, false);
			return;
		}
		global::Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", new object[]
		{
			component.TargetInstanceID
		});
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x0021EBBC File Offset: 0x0021CDBC
	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		return component != null && component.shouldShowName;
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x0021EBE4 File Offset: 0x0021CDE4
	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		this.textEntries.Add(textEntry);
		return textEntry.guid;
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x0021EC40 File Offset: 0x0021CE40
	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005C9F RID: 23711 RVA: 0x0021ECA8 File Offset: 0x0021CEA8
	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < this.textEntries.Count; i++)
		{
			if (this.textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(this.textEntries[num].display_go);
			this.textEntries.RemoveAt(num);
		}
	}

	// Token: 0x06005CA0 RID: 23712 RVA: 0x0021ED10 File Offset: 0x0021CF10
	public void AddNewEntry(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject gameObject = Util.KInstantiateUI(this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab, this.nameDisplayCanvas.gameObject, true);
		entry.display_go = gameObject;
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		if (this.worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		this.entries.Add(entry);
		this.ToggleLabelTextForActiveWorld(entry);
		UnityEngine.Object component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (component != null)
		{
			if (component2 != null)
			{
				if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
				{
					this.UpdateName(representedObject);
					return;
				}
			}
			else
			{
				this.UpdateName(representedObject);
			}
		}
	}

	// Token: 0x06005CA1 RID: 23713 RVA: 0x0021EE2C File Offset: 0x0021D02C
	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		NameDisplayScreen.Entry entry = force_new_entry ? null : this.GetEntry(representedObject);
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = this.GetEntry(representedObject);
			}
		}
		if (entry == null)
		{
			return;
		}
		Transform reference = entry.refs.GetReference<Transform>("Bars");
		entry.bars_go = reference.gameObject;
		if (component is Health)
		{
			if (!entry.healthBar)
			{
				Health health = (Health)component;
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (representedObject.GetComponent<MinionBrain>() != null);
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
				return;
			}
			global::Debug.LogWarningFormat("Health added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("OxygenBreather added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBar = gameObject3.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject3.name = "Suit Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("SuitBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitFuelBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject4.name = "Suit Fuel Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("FuelBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBatteryBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject5.name = "Suit Battery Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("CoolantBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is ThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject6 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
				entry.thoughtBubble = gameObject6.GetComponent<HierarchyReferences>();
				gameObject6.name = "Thought Bubble";
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
				entry.thoughtBubbleConvo = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = "Thought Bubble Convo";
				return;
			}
			global::Debug.LogWarningFormat("ThoughtGraph added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else
		{
			if (!(component is GameplayEventMonitor.Instance))
			{
				if (component is Dreamer.Instance && !entry.dreamBubble)
				{
					GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go, false);
					gameObject8.name = "Dream Bubble";
					entry.dreamBubble = gameObject8.GetComponent<DreamBubble>();
				}
				return;
			}
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go, false);
				entry.gameplayEventDisplay = gameObject9.GetComponent<HierarchyReferences>();
				gameObject9.name = "Gameplay Event Display";
				return;
			}
			global::Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", new object[]
			{
				component
			});
			return;
		}
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x0021F354 File Offset: 0x0021D554
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		this.BindOnOverlayChange();
		Camera mainCamera = Game.MainCamera;
		if (mainCamera == null)
		{
			return;
		}
		if (this.lastKnownOverlayID != OverlayModes.None.ID)
		{
			return;
		}
		int count = this.entries.Count;
		this.LateUpdatePos(mainCamera.orthographicSize < this.HideDistance);
		this.lateUpdateSections[this.updateSectionIndex]();
		this.updateSectionIndex = (this.updateSectionIndex + 1) % this.lateUpdateSections.Count;
	}

	// Token: 0x06005CA3 RID: 23715 RVA: 0x0021F3EC File Offset: 0x0021D5EC
	private void LateUpdatePos(bool visibleToZoom)
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		bool flag = visibleToZoom && this.lastKnownOverlayID == OverlayModes.None.ID;
		if (this.nameDisplayCanvas.enabled != flag)
		{
			this.nameDisplayCanvas.enabled = flag;
		}
		if (!flag)
		{
			return;
		}
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			NameDisplayScreen.Entry entry = this.entries[i];
			GameObject world_go = entry.world_go;
			if (!(world_go == null))
			{
				Vector3 vector = world_go.transform.GetPosition();
				if (instance != null && followTarget == world_go.transform)
				{
					vector = instance.followTargetPos;
				}
				else if (entry.world_go_anim_controller != null)
				{
					vector = entry.world_go_anim_controller.GetWorldPivot();
				}
				entry.display_go_rect.anchoredPosition = (this.worldSpace ? vector : base.WorldToScreen(vector));
			}
		}
	}

	// Token: 0x06005CA4 RID: 23716 RVA: 0x0021F4F4 File Offset: 0x0021D6F4
	private void LateUpdatePart0()
	{
		int num = this.entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.entries[i].world_go == null)
			{
				UnityEngine.Object.Destroy(this.entries[i].display_go);
				num--;
				this.entries[i] = this.entries[num];
			}
			else
			{
				i++;
			}
		}
		this.entries.RemoveRange(num, this.entries.Count - num);
	}

	// Token: 0x06005CA5 RID: 23717 RVA: 0x0021F580 File Offset: 0x0021D780
	private void LateUpdatePart1()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(this.entries[i].world_go == null) && this.entries[i].world_go.HasTag(GameTags.Dead) && this.entries[i].bars_go.activeSelf)
			{
				this.entries[i].bars_go.SetActive(false);
			}
		}
	}

	// Token: 0x06005CA6 RID: 23718 RVA: 0x0021F60C File Offset: 0x0021D80C
	private void LateUpdatePart2()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.entries[i].bars_go != null)
			{
				this.entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
				foreach (KCollider2D kcollider2D in this.workingList)
				{
					kcollider2D.MarkDirty(false);
				}
			}
		}
	}

	// Token: 0x06005CA7 RID: 23719 RVA: 0x0021F6AC File Offset: 0x0021D8AC
	public void UpdateName(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " character overlay";
		if (entry.nameLabel != null)
		{
			entry.nameLabel.text = component.GetProperName();
			if (representedObject.GetComponent<RocketModule>() != null)
			{
				entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
			}
		}
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x0021F72C File Offset: 0x0021D92C
	public void SetDream(GameObject minion_go, Dream dream)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.SetDream(dream);
		entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
		entry.dreamBubble.gameObject.SetActive(true);
		entry.dreamBubble.SetVisibility(true);
	}

	// Token: 0x06005CA9 RID: 23721 RVA: 0x0021F794 File Offset: 0x0021D994
	public void StopDreaming(GameObject minion_go)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.StopDreaming();
		entry.dreamBubble.gameObject.SetActive(false);
	}

	// Token: 0x06005CAA RID: 23722 RVA: 0x0021F7D8 File Offset: 0x0021D9D8
	public void DreamTick(GameObject minion_go, float dt)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.Tick(dt);
	}

	// Token: 0x06005CAB RID: 23723 RVA: 0x0021F80C File Offset: 0x0021DA0C
	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
		entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubble.gameObject.SetActive(bVisible);
	}

	// Token: 0x06005CAC RID: 23724 RVA: 0x0021F87C File Offset: 0x0021DA7C
	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
		entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
	}

	// Token: 0x06005CAD RID: 23725 RVA: 0x0021F8FE File Offset: 0x0021DAFE
	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	// Token: 0x06005CAE RID: 23726 RVA: 0x0021F910 File Offset: 0x0021DB10
	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.gameplayEventDisplay == null)
		{
			return;
		}
		entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
		entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
		entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
	}

	// Token: 0x06005CAF RID: 23727 RVA: 0x0021F970 File Offset: 0x0021DB70
	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.breathBar == null)
		{
			return;
		}
		entry.breathBar.SetUpdateFunc(updatePercentFull);
		entry.breathBar.SetVisibility(bVisible);
	}

	// Token: 0x06005CB0 RID: 23728 RVA: 0x0021F9B0 File Offset: 0x0021DBB0
	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.healthBar == null)
		{
			return;
		}
		entry.healthBar.OnChange();
		entry.healthBar.SetUpdateFunc(updatePercentFull);
		if (entry.healthBar.gameObject.activeSelf != bVisible)
		{
			entry.healthBar.SetVisibility(bVisible);
		}
	}

	// Token: 0x06005CB1 RID: 23729 RVA: 0x0021FA10 File Offset: 0x0021DC10
	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBar == null)
		{
			return;
		}
		entry.suitBar.SetUpdateFunc(updatePercentFull);
		entry.suitBar.SetVisibility(bVisible);
	}

	// Token: 0x06005CB2 RID: 23730 RVA: 0x0021FA50 File Offset: 0x0021DC50
	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitFuelBar == null)
		{
			return;
		}
		entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
		entry.suitFuelBar.SetVisibility(bVisible);
	}

	// Token: 0x06005CB3 RID: 23731 RVA: 0x0021FA90 File Offset: 0x0021DC90
	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBatteryBar == null)
		{
			return;
		}
		entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
		entry.suitBatteryBar.SetVisibility(bVisible);
	}

	// Token: 0x06005CB4 RID: 23732 RVA: 0x0021FAD0 File Offset: 0x0021DCD0
	private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
	{
		return this.entries.Find((NameDisplayScreen.Entry entry) => entry.world_go == worldObject);
	}

	// Token: 0x04003E3F RID: 15935
	[SerializeField]
	private float HideDistance;

	// Token: 0x04003E40 RID: 15936
	public static NameDisplayScreen Instance;

	// Token: 0x04003E41 RID: 15937
	[SerializeField]
	private Canvas nameDisplayCanvas;

	// Token: 0x04003E42 RID: 15938
	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	// Token: 0x04003E43 RID: 15939
	public GameObject nameAndBarsPrefab;

	// Token: 0x04003E44 RID: 15940
	public GameObject barsPrefab;

	// Token: 0x04003E45 RID: 15941
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04003E46 RID: 15942
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04003E47 RID: 15943
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04003E48 RID: 15944
	public int fontsize_min = 14;

	// Token: 0x04003E49 RID: 15945
	public int fontsize_max = 32;

	// Token: 0x04003E4A RID: 15946
	public float cameraDistance_fontsize_min = 6f;

	// Token: 0x04003E4B RID: 15947
	public float cameraDistance_fontsize_max = 4f;

	// Token: 0x04003E4C RID: 15948
	public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();

	// Token: 0x04003E4D RID: 15949
	public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();

	// Token: 0x04003E4E RID: 15950
	public bool worldSpace = true;

	// Token: 0x04003E4F RID: 15951
	private int updateSectionIndex;

	// Token: 0x04003E50 RID: 15952
	private List<System.Action> lateUpdateSections = new List<System.Action>();

	// Token: 0x04003E51 RID: 15953
	private int worldChangeEventHandle;

	// Token: 0x04003E52 RID: 15954
	private bool isOverlayChangeBound;

	// Token: 0x04003E53 RID: 15955
	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	// Token: 0x04003E54 RID: 15956
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x02001ACE RID: 6862
	[Serializable]
	public class Entry
	{
		// Token: 0x04007A93 RID: 31379
		public string Name;

		// Token: 0x04007A94 RID: 31380
		public GameObject world_go;

		// Token: 0x04007A95 RID: 31381
		public GameObject display_go;

		// Token: 0x04007A96 RID: 31382
		public GameObject bars_go;

		// Token: 0x04007A97 RID: 31383
		public KAnimControllerBase world_go_anim_controller;

		// Token: 0x04007A98 RID: 31384
		public RectTransform display_go_rect;

		// Token: 0x04007A99 RID: 31385
		public LocText nameLabel;

		// Token: 0x04007A9A RID: 31386
		public HealthBar healthBar;

		// Token: 0x04007A9B RID: 31387
		public ProgressBar breathBar;

		// Token: 0x04007A9C RID: 31388
		public ProgressBar suitBar;

		// Token: 0x04007A9D RID: 31389
		public ProgressBar suitFuelBar;

		// Token: 0x04007A9E RID: 31390
		public ProgressBar suitBatteryBar;

		// Token: 0x04007A9F RID: 31391
		public DreamBubble dreamBubble;

		// Token: 0x04007AA0 RID: 31392
		public HierarchyReferences thoughtBubble;

		// Token: 0x04007AA1 RID: 31393
		public HierarchyReferences thoughtBubbleConvo;

		// Token: 0x04007AA2 RID: 31394
		public HierarchyReferences gameplayEventDisplay;

		// Token: 0x04007AA3 RID: 31395
		public HierarchyReferences refs;
	}

	// Token: 0x02001ACF RID: 6863
	public class TextEntry
	{
		// Token: 0x04007AA4 RID: 31396
		public Guid guid;

		// Token: 0x04007AA5 RID: 31397
		public GameObject display_go;
	}
}
