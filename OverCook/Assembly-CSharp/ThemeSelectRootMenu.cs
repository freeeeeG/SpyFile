using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ADC RID: 2780
[RequireComponent(typeof(T17GridLayoutGroup))]
public class ThemeSelectRootMenu : CarouselRootMenu
{
	// Token: 0x06003834 RID: 14388 RVA: 0x00108A4C File Offset: 0x00106E4C
	protected override void Start()
	{
		base.Start();
		this.m_dlcManager = GameUtils.RequireManager<DLCManager>();
		DLCManagerBase.DLCUpdatedEvent = (GenericVoid)Delegate.Combine(DLCManagerBase.DLCUpdatedEvent, new GenericVoid(this.OnDLCUpdated));
		this.RefreshDLCButtons();
	}

	// Token: 0x06003835 RID: 14389 RVA: 0x00108A85 File Offset: 0x00106E85
	protected override void OnDestroy()
	{
		base.OnDestroy();
		DLCManagerBase.DLCUpdatedEvent = (GenericVoid)Delegate.Remove(DLCManagerBase.DLCUpdatedEvent, new GenericVoid(this.OnDLCUpdated));
	}

	// Token: 0x06003836 RID: 14390 RVA: 0x00108AB0 File Offset: 0x00106EB0
	protected override CarouselButton GetInitialButton()
	{
		CarouselButton buttonForTheme = this.GetButtonForTheme(SceneDirectoryData.LevelTheme.Random);
		return (!(buttonForTheme != null)) ? base.GetInitialButton() : buttonForTheme;
	}

	// Token: 0x06003837 RID: 14391 RVA: 0x00108AE0 File Offset: 0x00106EE0
	protected override void OnButtonFocusChanged(CarouselButton _button)
	{
		base.OnButtonFocusChanged(_button);
		if (_button != null)
		{
			Navigation navigation = _button.Button.navigation;
			if (this.m_playerRootMenu != null && this.m_playerRootMenu.CanFocusOnFirstPlayer(true))
			{
				navigation.selectOnDown = this.m_BorderSelectables.selectOnDown;
			}
			else
			{
				navigation.selectOnDown = null;
			}
			_button.Button.navigation = navigation;
		}
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x00108B5C File Offset: 0x00106F5C
	public Sprite GetSpriteForTheme(SceneDirectoryData.LevelTheme _theme)
	{
		ThemeSelectButton buttonForTheme = this.GetButtonForTheme(_theme);
		if (buttonForTheme != null)
		{
			return buttonForTheme.ThemeSprite;
		}
		return null;
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x00108B88 File Offset: 0x00106F88
	public ThemeSelectButton GetButtonForTheme(SceneDirectoryData.LevelTheme _theme)
	{
		CarouselButton[] buttons = base.Buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			ThemeSelectButton themeSelectButton = buttons[i] as ThemeSelectButton;
			if (themeSelectButton != null && themeSelectButton.Theme == _theme)
			{
				return themeSelectButton;
			}
		}
		return null;
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x00108BD4 File Offset: 0x00106FD4
	public ThemeSelectButton GetRandomTheme()
	{
		ThemeSelectButton[] array = base.Buttons.ConvertAll((CarouselButton x) => (ThemeSelectButton)x);
		return array.FindAll((ThemeSelectButton x) => x.Theme != SceneDirectoryData.LevelTheme.Random).GetRandomElement<ThemeSelectButton>();
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x00108C34 File Offset: 0x00107034
	public void DisallowTheme(SceneDirectoryData.LevelTheme _theme)
	{
		ThemeSelectButton buttonForTheme = this.GetButtonForTheme(_theme);
		base.DisallowButton(buttonForTheme);
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00108C50 File Offset: 0x00107050
	protected override bool IsButtonInteractable(CarouselButton _button)
	{
		DlcThemeSelectButton dlcThemeSelectButton = _button as DlcThemeSelectButton;
		return (dlcThemeSelectButton == null || dlcThemeSelectButton.Purchased) && base.IsButtonInteractable(_button);
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x00108C84 File Offset: 0x00107084
	private void OnDLCUpdated()
	{
		this.RefreshDLCButtons();
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x00108C8C File Offset: 0x0010708C
	private void RefreshDLCButtons()
	{
		CarouselButton[] buttons = base.Buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			DlcThemeSelectButton dlcThemeSelectButton = buttons[i] as DlcThemeSelectButton;
			if (dlcThemeSelectButton != null)
			{
				dlcThemeSelectButton.Purchased = (dlcThemeSelectButton.DLCData == null || this.m_dlcManager.IsDLCAvailable(dlcThemeSelectButton.DLCData));
			}
		}
	}

	// Token: 0x04002CF2 RID: 11506
	[SerializeField]
	private UIPlayerRootMenu m_playerRootMenu;

	// Token: 0x04002CF3 RID: 11507
	private DLCManager m_dlcManager;
}
