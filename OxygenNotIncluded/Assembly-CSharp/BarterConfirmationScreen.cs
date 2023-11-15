using System;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAB RID: 2731
public class BarterConfirmationScreen : KModalScreen
{
	// Token: 0x06005364 RID: 21348 RVA: 0x001DEA53 File Offset: 0x001DCC53
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.cancelButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x06005365 RID: 21349 RVA: 0x001DEA8C File Offset: 0x001DCC8C
	public void Present(PermitResource permit, bool isPurchase)
	{
		this.Show(true);
		this.ShowContentContainer(true);
		this.ShowLoadingPanel(false);
		this.HideResultPanel();
		if (isPurchase)
		{
			this.itemIcon.transform.SetAsLastSibling();
			this.filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			this.itemIcon.transform.SetAsFirstSibling();
			this.filamentIcon.transform.SetAsLastSibling();
		}
		KleiItems.ResponseCallback <>9__1;
		KleiItems.ResponseCallback <>9__2;
		this.confirmButton.onClick += delegate()
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit == null)
			{
				return;
			}
			this.ShowContentContainer(false);
			this.HideResultPanel();
			this.ShowLoadingPanel(true);
			if (isPurchase)
			{
				string itemType = serverTypeFromPermit;
				KleiItems.ResponseCallback cb;
				if ((cb = <>9__1) == null)
				{
					cb = (<>9__1 = delegate(KleiItems.Result result)
					{
						if (this.IsNullOrDestroyed())
						{
							return;
						}
						this.ShowContentContainer(false);
						this.ShowLoadingPanel(false);
						if (!result.Success)
						{
							this.ShowResultPanel(permit, true, false);
							return;
						}
						this.ShowResultPanel(permit, true, true);
					});
				}
				KleiItems.AddRequestBarterGainItem(itemType, cb);
				return;
			}
			ulong itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
			KleiItems.ResponseCallback cb2;
			if ((cb2 = <>9__2) == null)
			{
				cb2 = (<>9__2 = delegate(KleiItems.Result result)
				{
					if (this.IsNullOrDestroyed())
					{
						return;
					}
					this.ShowContentContainer(false);
					this.ShowLoadingPanel(false);
					if (!result.Success)
					{
						this.ShowResultPanel(permit, false, false);
						return;
					}
					this.ShowResultPanel(permit, false, true);
				});
			}
			KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
		};
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		this.itemLabel.SetText(permit.Name);
		this.transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		this.panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		this.confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		this.confirmButtonFilamentLabel.SetText(isPurchase ? num.ToString() : (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix));
		this.largeCostLabel.SetText(isPurchase ? ("x" + num.ToString()) : ("x" + num2.ToString()));
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x001DEC57 File Offset: 0x001DCE57
	private void Update()
	{
		if (this.shouldCloseScreen)
		{
			this.ShowContentContainer(false);
			this.ShowLoadingPanel(false);
			this.HideResultPanel();
			this.Show(false);
		}
	}

	// Token: 0x06005367 RID: 21351 RVA: 0x001DEC7C File Offset: 0x001DCE7C
	private void ShowContentContainer(bool show)
	{
		this.contentContainer.SetActive(show);
	}

	// Token: 0x06005368 RID: 21352 RVA: 0x001DEC8C File Offset: 0x001DCE8C
	private void ShowLoadingPanel(bool show)
	{
		this.loadingContainer.SetActive(show);
		this.resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			this.loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			this.loadingAnimation.Stop();
		}
		if (!show)
		{
			this.shouldCloseScreen = false;
		}
	}

	// Token: 0x06005369 RID: 21353 RVA: 0x001DECF4 File Offset: 0x001DCEF4
	private void HideResultPanel()
	{
		this.resultContainer.SetActive(false);
	}

	// Token: 0x0600536A RID: 21354 RVA: 0x001DED04 File Offset: 0x001DCF04
	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		this.resultContainer.SetActive(true);
		if (!transationResult)
		{
			this.resultIcon.sprite = Assets.GetSprite("error_message");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			this.resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed", false));
			return;
		}
		this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			this.resultIcon.sprite = permitPresentationInfo.sprite;
			this.resultFilamentLabel.SetText("");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed", false));
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		this.resultIcon.sprite = Assets.GetSprite("filament");
		this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2.ToString());
		this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed", false));
	}

	// Token: 0x0400379C RID: 14236
	[SerializeField]
	private GameObject itemIcon;

	// Token: 0x0400379D RID: 14237
	[SerializeField]
	private GameObject filamentIcon;

	// Token: 0x0400379E RID: 14238
	[SerializeField]
	private LocText largeCostLabel;

	// Token: 0x0400379F RID: 14239
	[SerializeField]
	private LocText itemLabel;

	// Token: 0x040037A0 RID: 14240
	[SerializeField]
	private LocText transactionDescriptionLabel;

	// Token: 0x040037A1 RID: 14241
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x040037A2 RID: 14242
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x040037A3 RID: 14243
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040037A4 RID: 14244
	[SerializeField]
	private LocText panelHeaderLabel;

	// Token: 0x040037A5 RID: 14245
	[SerializeField]
	private LocText confirmButtonActionLabel;

	// Token: 0x040037A6 RID: 14246
	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	// Token: 0x040037A7 RID: 14247
	[SerializeField]
	private LocText resultLabel;

	// Token: 0x040037A8 RID: 14248
	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	// Token: 0x040037A9 RID: 14249
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x040037AA RID: 14250
	[SerializeField]
	private GameObject loadingContainer;

	// Token: 0x040037AB RID: 14251
	[SerializeField]
	private GameObject resultContainer;

	// Token: 0x040037AC RID: 14252
	[SerializeField]
	private Image resultIcon;

	// Token: 0x040037AD RID: 14253
	[SerializeField]
	private LocText mainResultLabel;

	// Token: 0x040037AE RID: 14254
	[SerializeField]
	private LocText resultFilamentLabel;

	// Token: 0x040037AF RID: 14255
	private bool shouldCloseScreen;
}
