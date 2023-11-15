using System;
using System.Collections;
using UnityEngine.SceneManagement;

// Token: 0x02000683 RID: 1667
public class RestartQuitSplashFlowroutine
{
	// Token: 0x06002001 RID: 8193 RVA: 0x0009B983 File Offset: 0x00099D83
	public RestartQuitSplashFlowroutine(PauseMenuManager _pauseManager, string _frontEndName, PopupGUI _popupGUI)
	{
		this.m_pauseManager = _pauseManager;
		this.m_frontEndName = _frontEndName;
		this.m_popupGUI = _popupGUI;
		this.m_resetButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.ResetButton, PlayerInputLookup.Player.One);
		this.m_quitButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.QuitButton, PlayerInputLookup.Player.One);
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x0009B9BC File Offset: 0x00099DBC
	public IEnumerator Run()
	{
		this.m_pauseManager.enabled = false;
		this.m_popupGUI.enabled = true;
		for (;;)
		{
			if (this.m_resetButton.IsDown())
			{
				GameUtils.LoadScene(this.m_frontEndName, LoadSceneMode.Single);
			}
			if (this.m_quitButton.IsDown())
			{
				GameUtils.LoadScene(this.m_frontEndName, LoadSceneMode.Single);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400185E RID: 6238
	private PauseMenuManager m_pauseManager;

	// Token: 0x0400185F RID: 6239
	private PopupGUI m_popupGUI;

	// Token: 0x04001860 RID: 6240
	private string m_frontEndName;

	// Token: 0x04001861 RID: 6241
	private ILogicalButton m_resetButton;

	// Token: 0x04001862 RID: 6242
	private ILogicalButton m_quitButton;
}
