using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AEA RID: 2794
public class SliderOption : BaseUIOption<IQuantizedOption>
{
	// Token: 0x0600389D RID: 14493 RVA: 0x0010B394 File Offset: 0x00109794
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
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x0010B41C File Offset: 0x0010981C
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

	// Token: 0x0600389F RID: 14495 RVA: 0x0010B4E4 File Offset: 0x001098E4
	public override void SyncUIWithOption()
	{
		if (this.m_Option != null)
		{
			int num = this.m_Option.GetOption();
			num = Mathf.Clamp(num, 0, this.m_StepImages.Length);
			for (int i = 0; i < this.m_StepImages.Length; i++)
			{
				if (i < num)
				{
					this.m_StepImages[i].sprite = this.m_EnabledSprite;
				}
				else
				{
					this.m_StepImages[i].sprite = this.m_DisabledSprite;
				}
			}
		}
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x0010B564 File Offset: 0x00109964
	private void OnLeftPressed()
	{
		int num = this.m_Option.GetOption() - 1;
		num = Mathf.Clamp(num, 0, this.m_Option.Quanta);
		this.m_Option.SetOption(num);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIHighlight, base.gameObject.layer);
		this.SyncUIWithOption();
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x0010B5B8 File Offset: 0x001099B8
	private void OnRightPressed()
	{
		int num = this.m_Option.GetOption() + 1;
		num = Mathf.Clamp(num, 0, this.m_Option.Quanta);
		this.m_Option.SetOption(num);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIHighlight, base.gameObject.layer);
		this.SyncUIWithOption();
	}

	// Token: 0x04002D4C RID: 11596
	[SerializeField]
	private T17Image[] m_StepImages;

	// Token: 0x04002D4D RID: 11597
	[SerializeField]
	private Sprite m_EnabledSprite;

	// Token: 0x04002D4E RID: 11598
	[SerializeField]
	private Sprite m_DisabledSprite;

	// Token: 0x04002D4F RID: 11599
	[SerializeField]
	private T17Button m_ButtonToHaveFocusOnForLeftRight;

	// Token: 0x04002D50 RID: 11600
	[SerializeField]
	private T17Button m_LeftButton;

	// Token: 0x04002D51 RID: 11601
	[SerializeField]
	private T17Button m_RightButton;

	// Token: 0x04002D52 RID: 11602
	private ILogicalButton m_LeftInput;

	// Token: 0x04002D53 RID: 11603
	private ILogicalButton m_RightInput;
}
