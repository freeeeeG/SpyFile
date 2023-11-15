using System;

// Token: 0x02000639 RID: 1593
public class ExceptionDialogHandler : IExceptionDisplayer
{
	// Token: 0x06001E55 RID: 7765 RVA: 0x00092D63 File Offset: 0x00091163
	public void Initialize()
	{
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x00092D65 File Offset: 0x00091165
	public void OnGUI()
	{
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x00092D68 File Offset: 0x00091168
	public void Display(string exceptionString, string stackTrace, bool bJustOccured)
	{
		if (!bJustOccured)
		{
			return;
		}
		if (this.m_dialogBox != null)
		{
			return;
		}
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		GamepadUser user = playerManager.GetUser(EngagementSlot.One);
		if (T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user) == null)
		{
			return;
		}
		this.m_dialogBox = T17DialogBoxManager.GetDialog(false);
		if (this.m_dialogBox != null)
		{
			string str = stackTrace.Substring(0, stackTrace.IndexOf('\n'));
			this.m_dialogBox.Initialize("Exception", exceptionString + "\n" + str, "OK", null, null, T17DialogBox.Symbols.Error, false, false, false);
			T17DialogBox dialogBox = this.m_dialogBox;
			dialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(dialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.OnConfirmDialog));
			this.m_dialogBox.Show();
			this.SetGamePaused(true);
		}
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x00092E40 File Offset: 0x00091240
	private void OnConfirmDialog()
	{
		this.m_dialogBox = null;
		this.SetGamePaused(false);
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x00092E50 File Offset: 0x00091250
	private void SetGamePaused(bool bSetPaused)
	{
		if (ConnectionStatus.IsInSession())
		{
			return;
		}
		TimeManager timeManager = GameUtils.RequestManager<TimeManager>();
		if (timeManager != null)
		{
			timeManager.SetPaused(TimeManager.PauseLayer.Main, bSetPaused, this);
		}
	}

	// Token: 0x0400175F RID: 5983
	private T17DialogBox m_dialogBox;
}
