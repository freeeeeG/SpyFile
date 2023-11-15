using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000186 RID: 390
public class UI_SocialMediaButtons : MonoBehaviour
{
	// Token: 0x06000A57 RID: 2647 RVA: 0x000268AC File Offset: 0x00024AAC
	private void OnEnable()
	{
		this.button_Twitter.onClick.AddListener(new UnityAction(this.OnClickButton_Twitter));
		this.button_Feedback.onClick.AddListener(new UnityAction(this.OnClickButton_Feedback));
		this.button_BugReport.onClick.AddListener(new UnityAction(this.OnClickButton_BugReport));
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x00026910 File Offset: 0x00024B10
	private void OnDisable()
	{
		this.button_Twitter.onClick.RemoveListener(new UnityAction(this.OnClickButton_Twitter));
		this.button_Feedback.onClick.RemoveListener(new UnityAction(this.OnClickButton_Feedback));
		this.button_BugReport.onClick.RemoveListener(new UnityAction(this.OnClickButton_BugReport));
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00026974 File Offset: 0x00024B74
	private void OnClickButton_Feedback()
	{
		SystemLanguage currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();
		if (currentLanguage <= SystemLanguage.Japanese)
		{
			if (currentLanguage == SystemLanguage.English)
			{
				Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdef3m9thIGQnCEh-lg6K-sO2r2lonuXCdENa90sGN0SJgUAQ/viewform?usp=sf_link");
				return;
			}
			if (currentLanguage != SystemLanguage.Japanese)
			{
				return;
			}
			Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdirrrbu1wIb6e_3JaXAmKZQBhZxev4-GrQ6ybm7nL6R0hX6g/viewform?usp=sf_link");
			return;
		}
		else
		{
			if (currentLanguage == SystemLanguage.ChineseSimplified)
			{
				Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd7boAkahBN7d8Sfd9hunvP2DDbktqnV4t7VgsiGF_7r3bdLg/viewform?usp=sf_link");
				return;
			}
			if (currentLanguage != SystemLanguage.ChineseTraditional)
			{
				return;
			}
			Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeJFK7yMV469dhI6ketGiG9PXjvX64QsKQRlJzgeMcIyBgF-w/viewform?usp=sf_link");
			return;
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x000269D4 File Offset: 0x00024BD4
	private void OnClickButton_BugReport()
	{
		SystemLanguage currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();
		if (currentLanguage <= SystemLanguage.Japanese)
		{
			if (currentLanguage == SystemLanguage.English)
			{
				Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScA6K_5xLrq35N45MMmQyMuY2whppZ62UXeyJSksa6HQY87Og/viewform?usp=sf_link");
				return;
			}
			if (currentLanguage != SystemLanguage.Japanese)
			{
				return;
			}
			Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd58HXEkGOCHwYr12e0_bz43ihBp6F5rl8brmxqnaOZCuNrfQ/viewform?usp=sf_link");
			return;
		}
		else
		{
			if (currentLanguage == SystemLanguage.ChineseSimplified)
			{
				Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdGKAJNpBugHXZhQ2CI8FJkLLgQpATop8Zb0IjFT--T0LjvRA/viewform?usp=sf_link");
				return;
			}
			if (currentLanguage != SystemLanguage.ChineseTraditional)
			{
				return;
			}
			Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdskCJAk_tiULpkzKtPed2zNEIhw3aDWTkYzDiGZaTNgCAIIg/viewform?usp=sf_link");
			return;
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00026A32 File Offset: 0x00024C32
	private void OnClickButton_Twitter()
	{
		Application.OpenURL("https://twitter.com/RefiCHL");
	}

	// Token: 0x040007EE RID: 2030
	[SerializeField]
	private Button button_Twitter;

	// Token: 0x040007EF RID: 2031
	[SerializeField]
	private Button button_Feedback;

	// Token: 0x040007F0 RID: 2032
	[SerializeField]
	private Button button_BugReport;
}
