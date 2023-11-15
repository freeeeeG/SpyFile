using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B65 RID: 2917
public class T17DialogBox : MonoBehaviour, IT17EventHelper
{
	// Token: 0x06003B4E RID: 15182 RVA: 0x0011A24C File Offset: 0x0011864C
	public void Initialize(string title, string message, string confirmBtn = "", string declineBtn = "", string cancelBtn = "", T17DialogBox.Symbols symbol = T17DialogBox.Symbols.Warning, bool bLocalizeTitle = true, bool bLocalizeMessage = true, bool bCanPause = false)
	{
		this.m_hasConfirm = !string.IsNullOrEmpty(confirmBtn);
		this.m_hasDecline = !string.IsNullOrEmpty(declineBtn);
		this.m_hasCancel = !string.IsNullOrEmpty(cancelBtn);
		this.m_canPause = bCanPause;
		if (this.m_animator != null)
		{
			if (!TimeManager.IsPaused(base.gameObject) || this.ShouldUnpause())
			{
				this.m_animator.speed = 1f;
			}
			else
			{
				this.m_animator.speed = 0f;
			}
		}
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null)
		{
			FrontendRootMenu rootmenu = instance.m_Rootmenu;
			if (rootmenu != null)
			{
				rootmenu.SetLegendText("Text.Menu.LegendAccept");
			}
		}
		if (this.m_ConfirmButton != null)
		{
			this.m_ConfirmButton.gameObject.SetActive(this.m_hasConfirm);
			Navigation navigation = this.m_ConfirmButton.navigation;
			if (this.m_hasDecline)
			{
				navigation.selectOnRight = this.m_DeclineButton;
			}
			else if (this.m_hasCancel)
			{
				navigation.selectOnRight = this.m_CancelButton;
			}
			else
			{
				navigation.selectOnRight = null;
			}
			this.m_ConfirmButton.navigation = navigation;
			if (string.IsNullOrEmpty(confirmBtn))
			{
				confirmBtn = "Text.Dialog.Prompt.Yes";
			}
			this.SetButtonText(this.m_ConfirmButton, confirmBtn);
		}
		this.OnConfirm = null;
		this.OnUpdate = null;
		if (this.m_DeclineButton != null)
		{
			this.m_DeclineButton.gameObject.SetActive(this.m_hasDecline);
			Navigation navigation2 = this.m_DeclineButton.navigation;
			if (this.m_hasConfirm)
			{
				navigation2.selectOnLeft = this.m_ConfirmButton;
			}
			else
			{
				navigation2.selectOnLeft = null;
			}
			if (this.m_hasCancel)
			{
				navigation2.selectOnRight = this.m_CancelButton;
			}
			else
			{
				navigation2.selectOnRight = null;
			}
			this.m_DeclineButton.navigation = navigation2;
			if (string.IsNullOrEmpty(declineBtn))
			{
				declineBtn = "Text.Dialog.Prompt.No";
			}
			this.SetButtonText(this.m_DeclineButton, declineBtn);
		}
		this.OnDecline = null;
		if (this.m_CancelButton != null)
		{
			this.m_CancelButton.gameObject.SetActive(this.m_hasCancel);
			Navigation navigation3 = this.m_CancelButton.navigation;
			if (this.m_hasDecline)
			{
				navigation3.selectOnLeft = this.m_DeclineButton;
			}
			else if (this.m_hasConfirm)
			{
				navigation3.selectOnLeft = this.m_ConfirmButton;
			}
			else
			{
				navigation3.selectOnLeft = null;
			}
			this.m_CancelButton.navigation = navigation3;
			if (string.IsNullOrEmpty(cancelBtn))
			{
				cancelBtn = "Text.Dialog.Prompt.Cancel";
			}
			this.SetButtonText(this.m_CancelButton, cancelBtn);
		}
		this.OnCancel = null;
		if (this.m_Message != null)
		{
			this.m_Message.text = message;
			this.m_Message.m_bNeedsLocalization = bLocalizeMessage;
			this.m_Message.SetNewPlaceHolder(message);
			this.m_Message.SetNewLocalizationTag(message);
		}
		if (this.m_Title != null)
		{
			this.m_Title.text = title;
			this.m_Title.m_bNeedsLocalization = bLocalizeTitle;
			this.m_Title.SetNewPlaceHolder(title);
			this.m_Title.SetNewLocalizationTag(title);
		}
		this.SetSymbol(symbol);
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x0011A5A8 File Offset: 0x001189A8
	public void SetSymbol(T17DialogBox.Symbols symbol)
	{
		if (this.m_activeSpinner != null)
		{
			this.m_activeSpinner.Release();
			this.m_activeSpinner = null;
		}
		switch (symbol)
		{
		case T17DialogBox.Symbols.Warning:
			this.m_activeSpinner = SpinnerIconManager.Instance.Show(SpinnerIconManager.SpinnerIconType.Warning, this, true);
			break;
		case T17DialogBox.Symbols.Error:
			this.m_activeSpinner = SpinnerIconManager.Instance.Show(SpinnerIconManager.SpinnerIconType.Error, this, true);
			break;
		case T17DialogBox.Symbols.Spinner:
			this.m_activeSpinner = SpinnerIconManager.Instance.Show(SpinnerIconManager.SpinnerIconType.SpinnerDialog, this, true);
			break;
		}
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0011A63C File Offset: 0x00118A3C
	public void SetMessage(string strMessage, bool bLocalizeMessage)
	{
		if (this.m_Message != null)
		{
			this.m_Message.text = strMessage;
			this.m_Message.m_bNeedsLocalization = bLocalizeMessage;
			this.m_Message.SetNewPlaceHolder(strMessage);
			this.m_Message.SetNewLocalizationTag(strMessage);
		}
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x0011A68A File Offset: 0x00118A8A
	public void Show()
	{
		T17DialogBoxManager.RequestDialogShow(this, new T17DialogBoxManager.AllowedToShowEvent(this.OnAllowedToShow));
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x0011A6A0 File Offset: 0x00118AA0
	private void OnAllowedToShow()
	{
		base.gameObject.SetActive(true);
		this.IsActive = true;
		if (null != this.m_EventSystemForGamer)
		{
			this.m_ObjectSelectedBeforeShow = this.m_EventSystemForGamer.GetPendingSelectedGameObject();
			if (this.m_ObjectSelectedBeforeShow == null)
			{
				this.m_ObjectSelectedBeforeShow = this.m_EventSystemForGamer.GetLastRequestedSelectedGameobject();
			}
			if (this.m_ObjectSelectedBeforeShow == null)
			{
				this.m_ObjectSelectedBeforeShow = this.m_EventSystemForGamer.currentSelectedGameObject;
			}
		}
		this.Focus();
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x0011A734 File Offset: 0x00118B34
	public void Focus()
	{
		if (null != this.m_EventSystemForGamer)
		{
			if (!base.gameObject.activeInHierarchy || !this.IsActive)
			{
				this.Hide();
			}
			else
			{
				GameObject gameObject = this.m_EventSystemForGamer.GetPendingSelectedGameObject();
				if (gameObject == null || gameObject.IsInHierarchyOf(base.gameObject))
				{
					gameObject = this.m_EventSystemForGamer.GetLastRequestedSelectedGameobject();
					if (gameObject == null || gameObject.IsInHierarchyOf(base.gameObject))
					{
						gameObject = null;
					}
				}
				if (gameObject != null && this.m_ObjectSelectedBeforeShow != gameObject)
				{
					this.m_ObjectSelectedBeforeShow = gameObject;
				}
				GameObject gameObject2 = null;
				this.m_EventSystemForGamer.SetSelectedGameObject(null);
				if (this.m_DeclineButton != null && this.m_DeclineButton.gameObject.activeSelf)
				{
					gameObject2 = this.m_DeclineButton.gameObject;
				}
				else if (this.m_CancelButton != null && this.m_CancelButton.gameObject.activeSelf)
				{
					gameObject2 = this.m_CancelButton.gameObject;
				}
				else if (this.m_ConfirmButton != null && this.m_ConfirmButton.gameObject.activeSelf)
				{
					gameObject2 = this.m_ConfirmButton.gameObject;
				}
				if (gameObject2 != null)
				{
					bool flag = true;
					T17StandaloneInputModule t17StandaloneInputModule = (T17StandaloneInputModule)this.m_EventSystemForGamer.currentInputModule;
					if (t17StandaloneInputModule != null && t17StandaloneInputModule.WasUsingMouse && (this.m_EventSystemForGamer.currentSelectedGameObject == null || this.m_EventSystemForGamer.currentSelectedGameObject == gameObject2))
					{
						flag = false;
					}
					if (flag)
					{
						this.m_EventSystemForGamer.SetSelectedGameObject(gameObject2.gameObject);
					}
				}
			}
		}
		else
		{
			PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
			GamepadUser user = playerManager.GetUser(EngagementSlot.One);
			if (user != null)
			{
				T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
				if (eventSystemForGamepadUser != null)
				{
					this.SetEventSystem(eventSystemForGamepadUser);
				}
			}
		}
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0011A962 File Offset: 0x00118D62
	public bool HasButtons()
	{
		return this.m_hasConfirm || this.m_hasDecline || this.m_hasCancel;
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x0011A984 File Offset: 0x00118D84
	public bool HasFocus()
	{
		bool result = false;
		if (null != this.m_EventSystemForGamer)
		{
			GameObject lastRequestedSelectedGameobject = this.m_EventSystemForGamer.GetLastRequestedSelectedGameobject();
			if (null != lastRequestedSelectedGameobject)
			{
				result = (lastRequestedSelectedGameobject == this.m_DeclineButton.gameObject || lastRequestedSelectedGameobject == this.m_CancelButton.gameObject || lastRequestedSelectedGameobject == this.m_ConfirmButton.gameObject);
			}
		}
		return result;
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x0011A9FE File Offset: 0x00118DFE
	public void ReparentToOnHide(Transform parent)
	{
		this.m_ParentToOnHide = parent;
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x0011AA08 File Offset: 0x00118E08
	public void Hide()
	{
		base.gameObject.SetActive(false);
		if (this.m_activeSpinner != null)
		{
			this.m_activeSpinner.Release();
			this.m_activeSpinner = null;
		}
		this.IsActive = false;
		if (this.m_ParentToOnHide != null)
		{
			if (base.transform.parent.childCount <= 2)
			{
				base.transform.parent.gameObject.SetActive(false);
			}
			base.transform.SetParent(this.m_ParentToOnHide, false);
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = Vector3.zero;
		}
		this.SetSelectedGameobjectToPrepopup();
		T17DialogBoxManager.ReleaseMe(this);
		this.m_ObjectSelectedBeforeShow = null;
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x0011AAC8 File Offset: 0x00118EC8
	public void SetSelectedGameobjectToPrepopup()
	{
		if (this.m_EventSystemForGamer != null)
		{
			this.m_EventSystemForGamer.ForceDeselectSelectionObject();
			if (this.m_ObjectSelectedBeforeShow != null && this.m_ObjectSelectedBeforeShow.activeInHierarchy)
			{
				this.m_EventSystemForGamer.SetSelectedGameObject(this.m_ObjectSelectedBeforeShow);
			}
		}
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0011AB24 File Offset: 0x00118F24
	private void SetButtonText(T17Button button, string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			T17Text componentInChildren = button.GetComponentInChildren<T17Text>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetNewPlaceHolder(text);
				componentInChildren.SetNewLocalizationTag(text);
			}
		}
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x0011AB5E File Offset: 0x00118F5E
	private bool IsPaused()
	{
		return this.m_animator != null && this.m_animator.speed < 0.001f;
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0011AB85 File Offset: 0x00118F85
	public void Confirm()
	{
		this.Hide();
		if (this.OnConfirm != null)
		{
			this.OnConfirm();
		}
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x0011ABA3 File Offset: 0x00118FA3
	public void Decline()
	{
		this.Hide();
		if (this.OnDecline != null)
		{
			this.OnDecline();
		}
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0011ABC1 File Offset: 0x00118FC1
	public void Cancel()
	{
		this.Hide();
		if (this.OnCancel != null)
		{
			this.OnCancel();
		}
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0011ABE0 File Offset: 0x00118FE0
	private void Update()
	{
		if (this.ShouldUnpause() && this.m_animator != null)
		{
			this.m_animator.speed = 1f;
		}
		if (this.OnUpdate != null)
		{
			this.OnUpdate(this);
		}
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x0011AC30 File Offset: 0x00119030
	private bool ShouldUnpause()
	{
		return this.IsPaused() && (!TimeManager.IsPaused(base.gameObject) || !this.m_canPause);
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x0011AC5C File Offset: 0x0011905C
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x0011AC64 File Offset: 0x00119064
	public void SetEventSystem(T17EventSystem gamersEventSystem = null)
	{
		this.m_EventSystemForGamer = gamersEventSystem;
		if (this.m_ConfirmButton != null)
		{
			this.m_ConfirmButton.SetEventSystem(gamersEventSystem);
		}
		if (this.m_DeclineButton != null)
		{
			this.m_DeclineButton.SetEventSystem(gamersEventSystem);
		}
		if (this.m_CancelButton != null)
		{
			this.m_CancelButton.SetEventSystem(gamersEventSystem);
		}
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0011ACCF File Offset: 0x001190CF
	public T17EventSystem GetDomain()
	{
		return this.m_EventSystemForGamer;
	}

	// Token: 0x04003038 RID: 12344
	public T17DialogBox.DialogEvent OnConfirm;

	// Token: 0x04003039 RID: 12345
	public T17DialogBox.DialogEvent OnDecline;

	// Token: 0x0400303A RID: 12346
	public T17DialogBox.DialogEvent OnCancel;

	// Token: 0x0400303B RID: 12347
	public T17DialogBox.DialogEventInfo OnUpdate;

	// Token: 0x0400303C RID: 12348
	public T17Button m_ConfirmButton;

	// Token: 0x0400303D RID: 12349
	public T17Button m_DeclineButton;

	// Token: 0x0400303E RID: 12350
	public T17Button m_CancelButton;

	// Token: 0x0400303F RID: 12351
	public T17Text m_Title;

	// Token: 0x04003040 RID: 12352
	public T17Text m_Message;

	// Token: 0x04003041 RID: 12353
	public bool IsActive;

	// Token: 0x04003042 RID: 12354
	private Transform m_ParentToOnHide;

	// Token: 0x04003043 RID: 12355
	public T17EventSystem m_EventSystemForGamer;

	// Token: 0x04003044 RID: 12356
	private GameObject m_ObjectSelectedBeforeShow;

	// Token: 0x04003045 RID: 12357
	private bool m_hasConfirm;

	// Token: 0x04003046 RID: 12358
	private bool m_hasDecline;

	// Token: 0x04003047 RID: 12359
	private bool m_hasCancel;

	// Token: 0x04003048 RID: 12360
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04003049 RID: 12361
	private bool m_canPause;

	// Token: 0x0400304A RID: 12362
	private Suppressor m_activeSpinner;

	// Token: 0x02000B66 RID: 2918
	// (Invoke) Token: 0x06003B64 RID: 15204
	public delegate void DialogEvent();

	// Token: 0x02000B67 RID: 2919
	// (Invoke) Token: 0x06003B68 RID: 15208
	public delegate void DialogEventInfo(T17DialogBox dialog = null);

	// Token: 0x02000B68 RID: 2920
	public enum Symbols
	{
		// Token: 0x0400304C RID: 12364
		Unassigned,
		// Token: 0x0400304D RID: 12365
		Warning,
		// Token: 0x0400304E RID: 12366
		Error,
		// Token: 0x0400304F RID: 12367
		Spinner
	}
}
