using System;
using UnityEngine;

// Token: 0x02000A2A RID: 2602
public class NewContentPopup : FrontendMenuBehaviour
{
	// Token: 0x06003389 RID: 13193 RVA: 0x000F288E File Offset: 0x000F0C8E
	protected override void Awake()
	{
		base.Awake();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000F28A1 File Offset: 0x000F0CA1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x000F28C0 File Offset: 0x000F0CC0
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _p, GamepadUser _n)
	{
		if (base.CachedEventSystem != null)
		{
			base.CachedEventSystem.SetSelectedGameObject(this.m_confirmButton.GetGameobject());
		}
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x000F28EC File Offset: 0x000F0CEC
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		PopupData.Kind kind = this.m_popupData.m_kind;
		if (kind != PopupData.Kind.DLC)
		{
			if (kind == PopupData.Kind.Update)
			{
				this.m_name.SetLocalisedTextCatchAll(this.m_popupData.m_nameLocalisationKey);
				this.m_description.SetLocalisedTextCatchAll(this.m_popupData.m_descriptionLocalisationKey);
				this.m_image.sprite = this.m_popupData.m_image;
				this.m_storeButton.gameObject.SetActive(false);
				this.m_seasonPass.gameObject.SetActive(false);
			}
		}
		else
		{
			DLCFrontendData dlcData = this.m_popupData.m_dlcData;
			this.m_name.SetLocalisedTextCatchAll(dlcData.m_NameLocalizationKey);
			this.m_description.SetLocalisedTextCatchAll(dlcData.m_DescriptionLocalizationKey);
			this.m_image.sprite = dlcData.m_PopupImage;
			DLCManager dlcmanager = GameUtils.RequestManager<DLCManager>();
			this.m_storeButton.gameObject.SetActive(!dlcmanager.IsDLCAvailable(this.m_popupData.m_dlcData));
			this.m_seasonPass.gameObject.SetActive(dlcData.m_IsSeasonPassDLC);
		}
		return true;
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x000F2A34 File Offset: 0x000F0E34
	public void OnPopupConfirm()
	{
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		this.Hide(true, false);
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x000F2A58 File Offset: 0x000F0E58
	public void OnPopupStore()
	{
		DLCManager dlcmanager = GameUtils.RequireManager<DLCManager>();
		dlcmanager.ShowDLCStorePage(this.m_popupData.m_dlcData);
	}

	// Token: 0x04002977 RID: 10615
	[SerializeField]
	private T17Text m_name;

	// Token: 0x04002978 RID: 10616
	[SerializeField]
	private T17Text m_description;

	// Token: 0x04002979 RID: 10617
	[SerializeField]
	private T17Image m_image;

	// Token: 0x0400297A RID: 10618
	[SerializeField]
	private T17Button m_confirmButton;

	// Token: 0x0400297B RID: 10619
	[SerializeField]
	private T17Button m_storeButton;

	// Token: 0x0400297C RID: 10620
	[SerializeField]
	private RectTransform m_seasonPass;

	// Token: 0x0400297D RID: 10621
	[HideInInspector]
	public PopupData m_popupData;

	// Token: 0x0400297E RID: 10622
	private PlayerManager m_playerManager;
}
