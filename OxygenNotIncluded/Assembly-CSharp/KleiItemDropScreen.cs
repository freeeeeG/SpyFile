using System;
using System.Collections;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B34 RID: 2868
public class KleiItemDropScreen : KModalScreen
{
	// Token: 0x06005895 RID: 22677 RVA: 0x00207B0D File Offset: 0x00205D0D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KleiItemDropScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		if (string.IsNullOrEmpty(KleiAccount.KleiToken))
		{
			base.Show(false);
		}
	}

	// Token: 0x06005896 RID: 22678 RVA: 0x00207B45 File Offset: 0x00205D45
	protected override void OnActivate()
	{
		KleiItemDropScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06005897 RID: 22679 RVA: 0x00207B54 File Offset: 0x00205D54
	public override void Show(bool show = true)
	{
		this.serverRequestState.Reset();
		if (!show)
		{
			this.animatedLoadingIcon.gameObject.SetActive(false);
			if (this.activePresentationRoutine != null)
			{
				base.StopCoroutine(this.activePresentationRoutine);
			}
			if (this.shouldDoCloseRoutine)
			{
				this.closeButton.gameObject.SetActive(false);
				Updater.RunRoutine(this, this.AnimateScreenOutRoutine()).Then(delegate
				{
					base.Show(false);
				});
				this.shouldDoCloseRoutine = false;
			}
			else
			{
				base.Show(false);
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
		base.Show(true);
	}

	// Token: 0x06005898 RID: 22680 RVA: 0x00207C11 File Offset: 0x00205E11
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005899 RID: 22681 RVA: 0x00207C34 File Offset: 0x00205E34
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			return;
		}
		if (PermitItems.HasUnopenedItem())
		{
			this.PresentNextUnopenedItem(true);
			this.shouldDoCloseRoutine = true;
			return;
		}
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.PresentNoItemAvailablePrompt(true);
		this.shouldDoCloseRoutine = true;
	}

	// Token: 0x0600589A RID: 22682 RVA: 0x00207C88 File Offset: 0x00205E88
	public void PresentNextUnopenedItem(bool firstItemPresentation = true)
	{
		foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
		{
			if (!itemData.IsOpened)
			{
				this.PresentItem(itemData, firstItemPresentation);
				return;
			}
		}
		this.PresentNoItemAvailablePrompt(false);
	}

	// Token: 0x0600589B RID: 22683 RVA: 0x00207CE8 File Offset: 0x00205EE8
	public void PresentItem(KleiItems.ItemData item, bool firstItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
		this.giftAcknowledged = false;
		this.serverRequestState.revealConfirmedByServer = false;
		this.serverRequestState.revealRejectedByServer = false;
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentItemRoutine(item, firstItemPresentation));
		this.acceptButton.ClearOnClick();
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.serverRequestState.revealRequested = true;
			PermitItems.QueueRequestOpenOrUnboxItem(item, new KleiItems.ResponseCallback(this.OnOpenItemRequestResponse));
		};
		this.acknowledgeButton.onClick += delegate()
		{
			if (this.serverRequestState.revealConfirmedByServer)
			{
				this.giftAcknowledged = true;
			}
		};
	}

	// Token: 0x0600589C RID: 22684 RVA: 0x00207DC8 File Offset: 0x00205FC8
	public void OnOpenItemRequestResponse(KleiItems.Result result)
	{
		if (!this.serverRequestState.revealRequested)
		{
			return;
		}
		this.serverRequestState.revealRequested = false;
		if (result.Success)
		{
			this.serverRequestState.revealRejectedByServer = false;
			this.serverRequestState.revealConfirmedByServer = true;
			return;
		}
		this.serverRequestState.revealRejectedByServer = true;
		this.serverRequestState.revealConfirmedByServer = false;
	}

	// Token: 0x0600589D RID: 22685 RVA: 0x00207E28 File Offset: 0x00206028
	public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.noItemAvailableAcknowledged = false;
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.noItemAvailableAcknowledged = true;
		};
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentNoItemAvailableRoutine(firstItemPresentation));
	}

	// Token: 0x0600589E RID: 22686 RVA: 0x00207EBF File Offset: 0x002060BF
	private IEnumerator AnimateScreenInRoutine()
	{
		float scaleFactor = base.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
		float OPEN_WIDTH = (float)Screen.width / scaleFactor;
		float y = Mathf.Clamp((float)Screen.height / scaleFactor, 720f, 900f);
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
		this.userMessageLabel.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x0600589F RID: 22687 RVA: 0x00207ECE File Offset: 0x002060CE
	private IEnumerator AnimateScreenOutRoutine()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(8f, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, 0f), 0.25f, Easing.CircInOut);
		yield break;
	}

	// Token: 0x060058A0 RID: 22688 RVA: 0x00207EDD File Offset: 0x002060DD
	private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
	{
		yield return null;
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		this.acceptButtonRect.gameObject.SetActive(true);
		this.acceptButtonPosition.SetOn(this.acceptButtonRect);
		yield return Updater.WaitForSeconds(0.75f);
		yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => this.noItemAvailableAcknowledged);
		yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		this.Show(false);
		yield break;
	}

	// Token: 0x060058A1 RID: 22689 RVA: 0x00207EF3 File Offset: 0x002060F3
	private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem)
	{
		yield return null;
		if (item.ItemId == 0UL)
		{
			global::Debug.LogError("Could not find dropped item inventory.");
			yield break;
		}
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		this.permitVisualizer.ResetState();
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		this.acceptButtonRect.gameObject.SetActive(true);
		this.acceptButtonPosition.SetOn(this.acceptButtonRect);
		this.animatedPod.Play("powerup", KAnim.PlayMode.Once, 1f, 0f);
		this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		yield return Updater.WaitForSeconds(1.25f);
		yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => this.serverRequestState.revealRequested);
		yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		this.animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0f, -352f);
		if (this.animatedLoadingIcon.GetComponent<CanvasGroup>() != null)
		{
			this.animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		yield return new WaitForSecondsRealtime(0.3f);
		if (!this.serverRequestState.revealConfirmedByServer && !this.serverRequestState.revealRejectedByServer)
		{
			this.animatedLoadingIcon.gameObject.SetActive(true);
			this.animatedLoadingIcon.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.Until(() => this.serverRequestState.revealConfirmedByServer || this.serverRequestState.revealRejectedByServer);
			yield return new WaitForSecondsRealtime(2f);
			yield return PresUtil.OffsetFromAndFade(this.animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0f, -512f), 0f, 0.25f, Easing.SmoothStep);
			this.animatedLoadingIcon.gameObject.SetActive(false);
		}
		if (this.serverRequestState.revealRejectedByServer)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.errorMessage.gameObject.SetActive(true);
			yield return Updater.WaitForSeconds(3f);
			this.errorMessage.gameObject.SetActive(false);
		}
		else if (this.serverRequestState.revealConfirmedByServer)
		{
			this.animatedPod.Play("additional_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(1f);
			DropScreenPresentationInfo info;
			info.UseEquipmentVis = false;
			info.BuildOverride = null;
			info.Sprite = null;
			string name = "";
			string desc = "";
			PermitRarity rarity = PermitRarity.Unknown;
			string categoryString = "";
			string s;
			if (PermitItems.TryGetBoxInfo(item, out name, out desc, out s))
			{
				info.UseEquipmentVis = false;
				info.BuildOverride = null;
				info.Sprite = Assets.GetSprite(s);
				rarity = PermitRarity.Loyalty;
			}
			else
			{
				PermitResource permitResource = Db.Get().Permits.Get(item.Id);
				info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
				info.UseEquipmentVis = (permitResource.Category == PermitCategory.Equipment);
				if (permitResource is EquippableFacadeResource)
				{
					info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
				}
				name = permitResource.Name;
				desc = permitResource.Description;
				rarity = permitResource.Rarity;
				PermitCategory category = permitResource.Category;
				if (category != PermitCategory.Building)
				{
					if (category != PermitCategory.Artwork)
					{
						if (category != PermitCategory.JoyResponse)
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						}
						else
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
							if (permitResource is BalloonArtistFacadeResource)
							{
								categoryString = PermitCategories.GetDisplayName(permitResource.Category) + ": " + UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
							}
						}
					}
					else
					{
						categoryString = Assets.GetPrefab((permitResource as ArtableStage).prefabId).GetProperName();
					}
				}
				else
				{
					categoryString = Assets.GetPrefab((permitResource as BuildingFacadeResource).PrefabID).GetProperName();
				}
			}
			this.permitVisualizer.ConfigureWith(info);
			yield return this.permitVisualizer.AnimateIn();
			KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity", false), "GiftItemRarity", string.Format("{0}", rarity));
			this.itemNameLabel.SetText(name);
			this.itemDescriptionLabel.SetText(desc);
			this.itemRarityLabel.SetText(rarity.GetLocStringName());
			this.itemCategoryLabel.SetText(categoryString);
			this.itemTextContainerPosition.SetOn(this.itemTextContainer);
			yield return Updater.Parallel(new Updater[]
			{
				PresUtil.OffsetToAndFade(this.itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut)
			});
			yield return Updater.Until(() => this.giftAcknowledged);
			this.animatedPod.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.Parallel(new Updater[]
			{
				PresUtil.OffsetFromAndFade(this.itemTextContainer.rectTransform(), animate_offset, 0f, 0.125f, Easing.CircInOut)
			});
			this.itemNameLabel.SetText("");
			this.itemDescriptionLabel.SetText("");
			this.itemRarityLabel.SetText("");
			this.itemCategoryLabel.SetText("");
			yield return this.permitVisualizer.AnimateOut();
			name = null;
			desc = null;
			categoryString = null;
		}
		this.PresentNextUnopenedItem(false);
		yield break;
	}

	// Token: 0x060058A2 RID: 22690 RVA: 0x00207F10 File Offset: 0x00206110
	public static bool HasItemsToShow()
	{
		return PermitItems.HasUnopenedItem();
	}

	// Token: 0x04003BEF RID: 15343
	[SerializeField]
	private RectTransform shieldMaskRect;

	// Token: 0x04003BF0 RID: 15344
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003BF1 RID: 15345
	[Header("Animated Item")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis permitVisualizer;

	// Token: 0x04003BF2 RID: 15346
	[SerializeField]
	private KBatchedAnimController animatedPod;

	// Token: 0x04003BF3 RID: 15347
	[SerializeField]
	private LocText userMessageLabel;

	// Token: 0x04003BF4 RID: 15348
	[Header("Item Info")]
	[SerializeField]
	private RectTransform itemTextContainer;

	// Token: 0x04003BF5 RID: 15349
	[SerializeField]
	private LocText itemNameLabel;

	// Token: 0x04003BF6 RID: 15350
	[SerializeField]
	private LocText itemDescriptionLabel;

	// Token: 0x04003BF7 RID: 15351
	[SerializeField]
	private LocText itemRarityLabel;

	// Token: 0x04003BF8 RID: 15352
	[SerializeField]
	private LocText itemCategoryLabel;

	// Token: 0x04003BF9 RID: 15353
	[Header("Accept Button")]
	[SerializeField]
	private RectTransform acceptButtonRect;

	// Token: 0x04003BFA RID: 15354
	[SerializeField]
	private KButton acceptButton;

	// Token: 0x04003BFB RID: 15355
	[SerializeField]
	private KBatchedAnimController animatedLoadingIcon;

	// Token: 0x04003BFC RID: 15356
	[SerializeField]
	private KButton acknowledgeButton;

	// Token: 0x04003BFD RID: 15357
	[SerializeField]
	private LocText errorMessage;

	// Token: 0x04003BFE RID: 15358
	private Coroutine activePresentationRoutine;

	// Token: 0x04003BFF RID: 15359
	private KleiItemDropScreen.ServerRequestState serverRequestState;

	// Token: 0x04003C00 RID: 15360
	private bool giftAcknowledged;

	// Token: 0x04003C01 RID: 15361
	private bool noItemAvailableAcknowledged;

	// Token: 0x04003C02 RID: 15362
	public static KleiItemDropScreen Instance;

	// Token: 0x04003C03 RID: 15363
	private bool shouldDoCloseRoutine;

	// Token: 0x04003C04 RID: 15364
	private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;

	// Token: 0x04003C05 RID: 15365
	private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();

	// Token: 0x04003C06 RID: 15366
	private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

	// Token: 0x02001A47 RID: 6727
	private struct ServerRequestState
	{
		// Token: 0x0600968A RID: 38538 RVA: 0x0033CAB3 File Offset: 0x0033ACB3
		public void Reset()
		{
			this.revealRequested = false;
			this.revealConfirmedByServer = false;
			this.revealRejectedByServer = false;
		}

		// Token: 0x04007912 RID: 30994
		public bool revealRequested;

		// Token: 0x04007913 RID: 30995
		public bool revealConfirmedByServer;

		// Token: 0x04007914 RID: 30996
		public bool revealRejectedByServer;
	}
}
