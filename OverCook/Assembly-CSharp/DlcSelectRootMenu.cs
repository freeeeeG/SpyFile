using System;

// Token: 0x02000AD6 RID: 2774
public class DlcSelectRootMenu : CarouselRootMenu
{
	// Token: 0x0600381D RID: 14365 RVA: 0x0010884C File Offset: 0x00106C4C
	protected override bool IsButtonInteractable(CarouselButton _button)
	{
		DlcSelectButton dlcSelectButton = _button as DlcSelectButton;
		return !(dlcSelectButton != null) || dlcSelectButton.m_flipState == DlcSelectButton.FlipState.Front;
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x00108877 File Offset: 0x00106C77
	protected override CarouselButton GetInitialButton()
	{
		return base.Buttons[0];
	}
}
