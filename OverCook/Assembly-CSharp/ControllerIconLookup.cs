using System;
using UnityEngine;

// Token: 0x020001DA RID: 474
[AddComponentMenu("Scripts/Core/Input/ControllerIconLookup")]
public class ControllerIconLookup : Manager
{
	// Token: 0x0600080B RID: 2059 RVA: 0x00031768 File Offset: 0x0002FB68
	private ControllerIconLookup.IGetSprite GetIconPack(ControllerIconLookup.IconContext _context, ControllerIconLookup.DeviceContext _device)
	{
		return this.GetPlatformSet(_context).GetIconPack(_device);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00031777 File Offset: 0x0002FB77
	private ControllerIconLookup.PlatformSet GetPlatformSet(ControllerIconLookup.IconContext _context)
	{
		if (_context == ControllerIconLookup.IconContext.Borderless)
		{
			return this.m_borderlessIcons;
		}
		if (_context != ControllerIconLookup.IconContext.Bordered)
		{
			return null;
		}
		return this.m_borderedIcons;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0003179C File Offset: 0x0002FB9C
	public float GetIconScale(ControlPadInput.Button _button, ControllerIconLookup.IconContext _context = ControllerIconLookup.IconContext.Bordered, ControllerIconLookup.DeviceContext _device = ControllerIconLookup.DeviceContext.Pad)
	{
		ControllerIconLookup.IGetSprite iconPack = this.GetIconPack(_context, _device);
		return iconPack.GetIconScale(_button);
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x000317BC File Offset: 0x0002FBBC
	public Sprite GetIcon(ControlPadInput.Button _button, ControllerIconLookup.IconContext _context = ControllerIconLookup.IconContext.Bordered, ControllerIconLookup.DeviceContext _device = ControllerIconLookup.DeviceContext.Pad)
	{
		ControllerIconLookup.IGetSprite iconPack = this.GetIconPack(_context, _device);
		return iconPack.GetSprite(_button);
	}

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private ControllerIconLookup.PlatformSet m_borderlessIcons;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private ControllerIconLookup.PlatformSet m_borderedIcons;

	// Token: 0x020001DB RID: 475
	private interface IGetSprite
	{
		// Token: 0x0600080F RID: 2063
		Sprite GetSprite(ControlPadInput.Button _button);

		// Token: 0x06000810 RID: 2064
		float GetIconScale(ControlPadInput.Button _button);
	}

	// Token: 0x020001DC RID: 476
	[Serializable]
	private class ButtonIcons : ControllerIconLookup.IGetSprite
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x000317F8 File Offset: 0x0002FBF8
		public Sprite GetSprite(ControlPadInput.Button _button)
		{
			switch (_button)
			{
			case ControlPadInput.Button.A:
				return (!PlayerManagerShared<PCPlayerManager.PCPlayerProfile>.AcceptAndCancelButtonsInverted) ? this.Action1 : this.Action2;
			case ControlPadInput.Button.X:
				return this.Action3;
			case ControlPadInput.Button.B:
				return (!PlayerManagerShared<PCPlayerManager.PCPlayerProfile>.AcceptAndCancelButtonsInverted) ? this.Action2 : this.Action1;
			case ControlPadInput.Button.Y:
				return this.Action4;
			case ControlPadInput.Button.LB:
				return this.LeftBumper;
			case ControlPadInput.Button.RB:
				return this.RightBumper;
			case ControlPadInput.Button.LTrigger:
				return this.LeftTrigger;
			case ControlPadInput.Button.RTrigger:
				return this.RightTrigger;
			case ControlPadInput.Button.DPadLeft:
				return this.DPadLeft;
			case ControlPadInput.Button.DPadRight:
				return this.DPadRight;
			case ControlPadInput.Button.DPadUp:
				return this.DPadUp;
			case ControlPadInput.Button.LeftAnalog:
				return this.LeftStick;
			case ControlPadInput.Button.RightAnalog:
				return this.RightStick;
			}
			return null;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000318D8 File Offset: 0x0002FCD8
		public float GetIconScale(ControlPadInput.Button _button)
		{
			if (_button == ControlPadInput.Button.LeftAnalog)
			{
				return this.LeftStickScale;
			}
			if (_button != ControlPadInput.Button.RightAnalog)
			{
				return 1f;
			}
			return this.RightStickScale;
		}

		// Token: 0x04000668 RID: 1640
		public Sprite Action1;

		// Token: 0x04000669 RID: 1641
		public Sprite Action2;

		// Token: 0x0400066A RID: 1642
		public Sprite Action3;

		// Token: 0x0400066B RID: 1643
		public Sprite Action4;

		// Token: 0x0400066C RID: 1644
		public Sprite LeftBumper;

		// Token: 0x0400066D RID: 1645
		public Sprite RightBumper;

		// Token: 0x0400066E RID: 1646
		public Sprite LeftTrigger;

		// Token: 0x0400066F RID: 1647
		public Sprite RightTrigger;

		// Token: 0x04000670 RID: 1648
		public Sprite LeftStick;

		// Token: 0x04000671 RID: 1649
		public float LeftStickScale = 1f;

		// Token: 0x04000672 RID: 1650
		public Sprite RightStick;

		// Token: 0x04000673 RID: 1651
		public float RightStickScale = 1f;

		// Token: 0x04000674 RID: 1652
		public Sprite DPadUp;

		// Token: 0x04000675 RID: 1653
		public Sprite DPadRight;

		// Token: 0x04000676 RID: 1654
		public Sprite DPadLeft;
	}

	// Token: 0x020001DD RID: 477
	[Serializable]
	private class NXButtonIcons : ControllerIconLookup.IGetSprite
	{
		// Token: 0x06000815 RID: 2069 RVA: 0x0003192B File Offset: 0x0002FD2B
		public Sprite GetSprite(ControlPadInput.Button _button)
		{
			return this.A;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00031933 File Offset: 0x0002FD33
		public float GetIconScale(ControlPadInput.Button _button)
		{
			return 1f;
		}

		// Token: 0x04000677 RID: 1655
		public Sprite A;

		// Token: 0x04000678 RID: 1656
		public Sprite B;

		// Token: 0x04000679 RID: 1657
		public Sprite X;

		// Token: 0x0400067A RID: 1658
		public Sprite Y;

		// Token: 0x0400067B RID: 1659
		public Sprite LeftBumper;

		// Token: 0x0400067C RID: 1660
		public Sprite RightBumper;

		// Token: 0x0400067D RID: 1661
		public Sprite LeftTrigger;

		// Token: 0x0400067E RID: 1662
		public Sprite RightTrigger;

		// Token: 0x0400067F RID: 1663
		public Sprite LeftStick;

		// Token: 0x04000680 RID: 1664
		public float LeftStickScale = 1f;

		// Token: 0x04000681 RID: 1665
		public Sprite RightStick;

		// Token: 0x04000682 RID: 1666
		public float RightStickScale = 1f;

		// Token: 0x04000683 RID: 1667
		public Sprite DPadUp;

		// Token: 0x04000684 RID: 1668
		public Sprite DPadDown;

		// Token: 0x04000685 RID: 1669
		public Sprite DPadLeft;

		// Token: 0x04000686 RID: 1670
		public Sprite DPadRight;

		// Token: 0x04000687 RID: 1671
		public Sprite LeftSL;

		// Token: 0x04000688 RID: 1672
		public Sprite LeftSR;

		// Token: 0x04000689 RID: 1673
		public Sprite RightSL;

		// Token: 0x0400068A RID: 1674
		public Sprite RightSR;

		// Token: 0x0400068B RID: 1675
		public Sprite LRButton;

		// Token: 0x0400068C RID: 1676
		public Sprite TopFaceButton;

		// Token: 0x0400068D RID: 1677
		public Sprite LeftFaceButton;

		// Token: 0x0400068E RID: 1678
		public Sprite RightFaceButton;

		// Token: 0x0400068F RID: 1679
		public Sprite BottomFaceButton;

		// Token: 0x04000690 RID: 1680
		public Sprite AmbiStick;

		// Token: 0x04000691 RID: 1681
		public float AmbipadScale = 0.5f;
	}

	// Token: 0x020001DE RID: 478
	[Serializable]
	private class PlatformSet
	{
		// Token: 0x06000818 RID: 2072 RVA: 0x00031942 File Offset: 0x0002FD42
		public ControllerIconLookup.IGetSprite GetIconPack(ControllerIconLookup.DeviceContext _device)
		{
			if (_device == ControllerIconLookup.DeviceContext.Pad)
			{
				return this.XboxOne;
			}
			if (_device == ControllerIconLookup.DeviceContext.Keyboard)
			{
				return this.Keyboard;
			}
			if (_device != ControllerIconLookup.DeviceContext.SplitKeyboard)
			{
				return this.XboxOne;
			}
			return this.KeyboardSplit;
		}

		// Token: 0x04000692 RID: 1682
		public ControllerIconLookup.ButtonIcons XboxOne;

		// Token: 0x04000693 RID: 1683
		public ControllerIconLookup.ButtonIcons PS4;

		// Token: 0x04000694 RID: 1684
		public ControllerIconLookup.NXButtonIcons NX;

		// Token: 0x04000695 RID: 1685
		public ControllerIconLookup.ButtonIcons Keyboard;

		// Token: 0x04000696 RID: 1686
		public ControllerIconLookup.ButtonIcons KeyboardSplit;
	}

	// Token: 0x020001DF RID: 479
	public enum IconContext
	{
		// Token: 0x04000698 RID: 1688
		Borderless,
		// Token: 0x04000699 RID: 1689
		Bordered
	}

	// Token: 0x020001E0 RID: 480
	public enum DeviceContext
	{
		// Token: 0x0400069B RID: 1691
		Keyboard,
		// Token: 0x0400069C RID: 1692
		Pad,
		// Token: 0x0400069D RID: 1693
		SplitKeyboard
	}
}
