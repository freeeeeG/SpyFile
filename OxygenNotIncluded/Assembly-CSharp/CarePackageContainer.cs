using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A56 RID: 2646
public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06004FBB RID: 20411 RVA: 0x001C36B6 File Offset: 0x001C18B6
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06004FBC RID: 20412 RVA: 0x001C36BE File Offset: 0x001C18BE
	public CarePackageInfo Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x001C36C6 File Offset: 0x001C18C6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06004FBE RID: 20414 RVA: 0x001C36E1 File Offset: 0x001C18E1
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001C36E8 File Offset: 0x001C18E8
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (this.controller != null)
		{
			this.GenerateCharacter(this.controller.IsStarterMinion);
		}
		yield break;
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001C36F7 File Offset: 0x001C18F7
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001C3724 File Offset: 0x001C1924
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001C37BA File Offset: 0x001C19BA
	private void Initialize()
	{
		this.professionIconMap = new Dictionary<string, Sprite>();
		this.professionIcons.ForEach(delegate(CarePackageContainer.ProfessionIcon ic)
		{
			this.professionIconMap.Add(ic.professionName, ic.iconImg);
		});
		if (CarePackageContainer.containers == null)
		{
			CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
		}
		CarePackageContainer.containers.Add(this);
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001C37FC File Offset: 0x001C19FC
	private void GenerateCharacter(bool is_starter)
	{
		int num = 0;
		do
		{
			this.info = Immigration.Instance.RandomCarePackage();
			num++;
		}
		while (this.IsCharacterRedundant() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.carePackageInstanceData = new CarePackageContainer.CarePackageInstanceData();
		this.carePackageInstanceData.info = this.info;
		if (this.info.facadeID == "SELECTRANDOM")
		{
			this.carePackageInstanceData.facadeID = Db.GetEquippableFacades().resources.FindAll((EquippableFacadeResource match) => match.DefID == this.info.id).GetRandom<EquippableFacadeResource>().Id;
		}
		else
		{
			this.carePackageInstanceData.facadeID = this.info.facadeID;
		}
		this.SetAnimator();
		this.SetInfoText();
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001C3908 File Offset: 0x001C1B08
	private void SetAnimator()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
		int num;
		if (ElementLoader.FindElementByName(this.info.id) != null)
		{
			num = 1;
		}
		else if (foodInfo != null)
		{
			num = (int)(this.info.quantity % foodInfo.CaloriesPerUnit);
		}
		else
		{
			num = (int)this.info.quantity;
		}
		if (prefab != null)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				Image component = gameObject.GetComponent<Image>();
				global::Tuple<Sprite, Color> uisprite;
				if (!this.carePackageInstanceData.facadeID.IsNullOrWhiteSpace())
				{
					uisprite = Def.GetUISprite(prefab.PrefabID(), this.carePackageInstanceData.facadeID);
				}
				else
				{
					uisprite = Def.GetUISprite(prefab, "ui", false);
				}
				component.sprite = uisprite.first;
				component.color = uisprite.second;
				this.entryIcons.Add(gameObject);
				if (num > 1)
				{
					int num2;
					int num3;
					int num4;
					if (num % 2 == 1)
					{
						num2 = Mathf.CeilToInt((float)(num / 2));
						num3 = num2 - i;
						num4 = ((num3 > 0) ? 1 : -1);
						num3 = Mathf.Abs(num3);
					}
					else
					{
						num2 = num / 2 - 1;
						if (i <= num2)
						{
							num3 = Mathf.Abs(num2 - i);
							num4 = -1;
						}
						else
						{
							num3 = Mathf.Abs(num2 + 1 - i);
							num4 = 1;
						}
					}
					int num5 = 0;
					if (num % 2 == 0)
					{
						num5 = ((i <= num2) ? -6 : 6);
						gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float)num5, 0f, 0f));
					}
					gameObject.transform.localScale = new Vector3(1f - (float)num3 * 0.1f, 1f - (float)num3 * 0.1f, 1f);
					gameObject.transform.Rotate(0f, 0f, 3f * (float)num3 * (float)num4);
					gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float)num3 * (float)num4, 5f * (float)num3) + new Vector3((float)num5, 0f, 0f));
					gameObject.GetComponent<Canvas>().sortingOrder = num - num3;
				}
			}
			return;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
		gameObject2.SetActive(true);
		Image component2 = gameObject2.GetComponent<Image>();
		component2.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim, "ui", false, "");
		component2.color = ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
		this.entryIcons.Add(gameObject2);
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001C3C38 File Offset: 0x001C1E38
	private string GetSpawnableName()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			Element element = ElementLoader.FindElementByName(this.info.id);
			if (element != null)
			{
				return element.substance.name;
			}
			return "";
		}
		else
		{
			if (string.IsNullOrEmpty(this.carePackageInstanceData.facadeID))
			{
				return prefab.GetProperName();
			}
			return EquippableFacade.GetNameOverride(this.carePackageInstanceData.info.id, this.carePackageInstanceData.facadeID);
		}
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001C3CC4 File Offset: 0x001C1EC4
	private string GetSpawnableQuantityOnly()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true));
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, this.info.quantity.ToString());
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x001C3D78 File Offset: 0x001C1F78
	private string GetCurrentQuantity(WorldInventory inventory)
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			float amount = inventory.GetAmount(this.info.id.ToTag(), false);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			float calories = RationTracker.Get().CountRationsByFoodType(this.info.id, inventory, true);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
		}
		float amount2 = inventory.GetAmount(this.info.id.ToTag(), false);
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, amount2.ToString());
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x001C3E44 File Offset: 0x001C2044
	private string GetSpawnableQuantity()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), Assets.GetPrefab(this.info.id).GetProperName());
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true), Assets.GetPrefab(this.info.id).GetProperName());
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, Assets.GetPrefab(this.info.id).GetProperName(), this.info.quantity.ToString());
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x001C3F44 File Offset: 0x001C2144
	private string GetSpawnableDescription()
	{
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		if (element != null)
		{
			return element.Description();
		}
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		InfoDescription component = prefab.GetComponent<InfoDescription>();
		if (component != null)
		{
			return component.description;
		}
		return prefab.GetProperName();
	}

	// Token: 0x06004FCA RID: 20426 RVA: 0x001C3FB4 File Offset: 0x001C21B4
	private void SetInfoText()
	{
		this.characterName.SetText(this.GetSpawnableName());
		this.description.SetText(this.GetSpawnableDescription());
		this.itemName.SetText(this.GetSpawnableName());
		this.quantity.SetText(this.GetSpawnableQuantityOnly());
		this.currentQuantity.SetText(this.GetCurrentQuantity(ClusterManager.Instance.activeWorld.worldInventory));
	}

	// Token: 0x06004FCB RID: 20427 RVA: 0x001C4028 File Offset: 0x001C2228
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.carePackageInstanceData);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
	}

	// Token: 0x06004FCC RID: 20428 RVA: 0x001C40D0 File Offset: 0x001C22D0
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.carePackageInstanceData);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x001C4156 File Offset: 0x001C2356
	private void OnReplacedEvent(ITelepadDeliverable stats)
	{
		if (stats == this.carePackageInstanceData)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x001C4168 File Offset: 0x001C2368
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x06004FCF RID: 20431 RVA: 0x001C41DE File Offset: 0x001C23DE
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06004FD0 RID: 20432 RVA: 0x001C41F0 File Offset: 0x001C23F0
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x06004FD1 RID: 20433 RVA: 0x001C4214 File Offset: 0x001C2414
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x001C4265 File Offset: 0x001C2465
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x001C4278 File Offset: 0x001C2478
	private void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			this.DeselectDeliverable();
		}
		this.ClearEntryIcons();
		this.GenerateCharacter(is_starter);
	}

	// Token: 0x06004FD4 RID: 20436 RVA: 0x001C42B0 File Offset: 0x001C24B0
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x001C4370 File Offset: 0x001C2570
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x001C43D0 File Offset: 0x001C25D0
	private bool IsCharacterRedundant()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in CarePackageContainer.containers)
		{
			if (telepadDeliverableContainer != this)
			{
				CarePackageContainer carePackageContainer = telepadDeliverableContainer as CarePackageContainer;
				if (carePackageContainer != null && carePackageContainer.info == this.info)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004FD7 RID: 20439 RVA: 0x001C4444 File Offset: 0x001C2644
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x001C4454 File Offset: 0x001C2654
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			this.controller.OnPressBack();
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001C4478 File Offset: 0x001C2678
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06004FDA RID: 20442 RVA: 0x001C4488 File Offset: 0x001C2688
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.info == null)
		{
			return;
		}
		this.ClearEntryIcons();
		this.SetAnimator();
		this.SetInfoText();
	}

	// Token: 0x06004FDB RID: 20443 RVA: 0x001C44AC File Offset: 0x001C26AC
	private void ClearEntryIcons()
	{
		for (int i = 0; i < this.entryIcons.Count; i++)
		{
			UnityEngine.Object.Destroy(this.entryIcons[i]);
		}
	}

	// Token: 0x04003425 RID: 13349
	[Header("UI References")]
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04003426 RID: 13350
	[SerializeField]
	private LocText characterName;

	// Token: 0x04003427 RID: 13351
	public GameObject selectedBorder;

	// Token: 0x04003428 RID: 13352
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003429 RID: 13353
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x0400342A RID: 13354
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x0400342B RID: 13355
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x0400342C RID: 13356
	private KBatchedAnimController animController;

	// Token: 0x0400342D RID: 13357
	[SerializeField]
	private LocText itemName;

	// Token: 0x0400342E RID: 13358
	[SerializeField]
	private LocText quantity;

	// Token: 0x0400342F RID: 13359
	[SerializeField]
	private LocText currentQuantity;

	// Token: 0x04003430 RID: 13360
	[SerializeField]
	private LocText description;

	// Token: 0x04003431 RID: 13361
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003432 RID: 13362
	private CarePackageInfo info;

	// Token: 0x04003433 RID: 13363
	public CarePackageContainer.CarePackageInstanceData carePackageInstanceData;

	// Token: 0x04003434 RID: 13364
	private CharacterSelectionController controller;

	// Token: 0x04003435 RID: 13365
	private static List<ITelepadDeliverableContainer> containers;

	// Token: 0x04003436 RID: 13366
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04003437 RID: 13367
	[SerializeField]
	private List<CarePackageContainer.ProfessionIcon> professionIcons;

	// Token: 0x04003438 RID: 13368
	private Dictionary<string, Sprite> professionIconMap;

	// Token: 0x04003439 RID: 13369
	public float baseCharacterScale = 0.38f;

	// Token: 0x0400343A RID: 13370
	private List<GameObject> entryIcons = new List<GameObject>();

	// Token: 0x020018E7 RID: 6375
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04007352 RID: 29522
		public string professionName;

		// Token: 0x04007353 RID: 29523
		public Sprite iconImg;
	}

	// Token: 0x020018E8 RID: 6376
	public class CarePackageInstanceData : ITelepadDeliverable
	{
		// Token: 0x0600932A RID: 37674 RVA: 0x0032E3CE File Offset: 0x0032C5CE
		public GameObject Deliver(Vector3 position)
		{
			GameObject gameObject = this.info.Deliver(position);
			gameObject.GetComponent<CarePackage>().SetFacade(this.facadeID);
			return gameObject;
		}

		// Token: 0x04007354 RID: 29524
		public CarePackageInfo info;

		// Token: 0x04007355 RID: 29525
		public string facadeID;
	}
}
