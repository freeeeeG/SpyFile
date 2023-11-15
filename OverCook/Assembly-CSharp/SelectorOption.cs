using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AE9 RID: 2793
public class SelectorOption : BaseUIOption<INameListOption>
{
	// Token: 0x06003896 RID: 14486 RVA: 0x0010AFFC File Offset: 0x001093FC
	protected override void Awake()
	{
		base.Awake();
		if (this.m_LeftButton != null)
		{
			this.m_LeftButton.onClick.AddListener(new UnityAction(this.OnLeftPressed));
		}
		if (this.m_RightButton != null)
		{
			this.m_RightButton.onClick.AddListener(new UnityAction(this.OnRightPressed));
		}
		this.m_LeftInput = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UILeft, PlayerInputLookup.Player.One);
		this.m_RightInput = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIRight, PlayerInputLookup.Player.One);
		this.m_optionsMenu = base.gameObject.RequestComponentUpwardsRecursive<FrontendOptionsMenu>();
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x0010B098 File Offset: 0x00109498
	private void Update()
	{
		if (this.m_ButtonToHaveFocusOnForLeftRight != null && this.m_ButtonToHaveFocusOnForLeftRight.GetDomain() != null)
		{
			Navigation navigation = this.m_ButtonToHaveFocusOnForLeftRight.navigation;
			navigation.selectOnLeft = null;
			navigation.selectOnRight = null;
			this.m_ButtonToHaveFocusOnForLeftRight.navigation = navigation;
			if (this.m_ButtonToHaveFocusOnForLeftRight.GetDomain().currentSelectedGameObject == this.m_ButtonToHaveFocusOnForLeftRight.gameObject)
			{
				if (this.m_LeftInput.JustPressed())
				{
					this.m_LeftInput.ClaimPressEvent();
					this.OnLeftPressed();
				}
				else if (this.m_RightInput.JustPressed())
				{
					this.m_RightInput.ClaimPressEvent();
					this.OnRightPressed();
				}
			}
		}
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x0010B160 File Offset: 0x00109560
	public override void SyncUIWithOption()
	{
		if (this.m_Option != null && this.m_ValueText != null)
		{
			if (this.m_LocalizeOption)
			{
				this.m_ValueText.SetLocalisedTextCatchAll(this.m_Option.GetNames()[this.m_Option.GetOption()]);
			}
			else
			{
				this.m_ValueText.SetNonLocalizedText(this.m_Option.GetNames()[this.m_Option.GetOption()]);
			}
		}
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x0010B1E0 File Offset: 0x001095E0
	private void OnLeftPressed()
	{
		int num = this.m_Option.GetOption() - 1;
		string[] names = this.m_Option.GetNames();
		if (num < 0)
		{
			num = names.Length - 1;
		}
		this.m_Option.SetOption(num);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIHighlight, base.gameObject.layer);
		base.StartCoroutine(this.UpdateUIAtEndOfFrame());
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x0010B240 File Offset: 0x00109640
	private void OnRightPressed()
	{
		int num = this.m_Option.GetOption() + 1;
		string[] names = this.m_Option.GetNames();
		if (num >= names.Length)
		{
			num = 0;
		}
		this.m_Option.SetOption(num);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIHighlight, base.gameObject.layer);
		base.StartCoroutine(this.UpdateUIAtEndOfFrame());
	}

	// Token: 0x0600389B RID: 14491 RVA: 0x0010B2A0 File Offset: 0x001096A0
	private IEnumerator UpdateUIAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (this.m_optionsMenu != null)
		{
			this.m_optionsMenu.SyncAllOptions();
		}
		yield break;
	}

	// Token: 0x04002D44 RID: 11588
	[SerializeField]
	private T17Button m_ButtonToHaveFocusOnForLeftRight;

	// Token: 0x04002D45 RID: 11589
	[SerializeField]
	private T17Button m_LeftButton;

	// Token: 0x04002D46 RID: 11590
	[SerializeField]
	private T17Button m_RightButton;

	// Token: 0x04002D47 RID: 11591
	[SerializeField]
	private T17Text m_ValueText;

	// Token: 0x04002D48 RID: 11592
	private FrontendOptionsMenu m_optionsMenu;

	// Token: 0x04002D49 RID: 11593
	[SerializeField]
	private bool m_LocalizeOption = true;

	// Token: 0x04002D4A RID: 11594
	private ILogicalButton m_LeftInput;

	// Token: 0x04002D4B RID: 11595
	private ILogicalButton m_RightInput;
}
