using System;

// Token: 0x020001E4 RID: 484
public static class ControlPadInput
{
	// Token: 0x020001E5 RID: 485
	public enum Button
	{
		// Token: 0x040006C5 RID: 1733
		Invalid,
		// Token: 0x040006C6 RID: 1734
		A,
		// Token: 0x040006C7 RID: 1735
		X,
		// Token: 0x040006C8 RID: 1736
		B,
		// Token: 0x040006C9 RID: 1737
		Y,
		// Token: 0x040006CA RID: 1738
		LB,
		// Token: 0x040006CB RID: 1739
		RB,
		// Token: 0x040006CC RID: 1740
		LTrigger,
		// Token: 0x040006CD RID: 1741
		RTrigger,
		// Token: 0x040006CE RID: 1742
		DPadLeft,
		// Token: 0x040006CF RID: 1743
		DPadRight,
		// Token: 0x040006D0 RID: 1744
		DPadUp,
		// Token: 0x040006D1 RID: 1745
		DPadDown,
		// Token: 0x040006D2 RID: 1746
		Back,
		// Token: 0x040006D3 RID: 1747
		Start,
		// Token: 0x040006D4 RID: 1748
		LeftAnalog,
		// Token: 0x040006D5 RID: 1749
		RightAnalog
	}

	// Token: 0x020001E6 RID: 486
	public enum Value
	{
		// Token: 0x040006D7 RID: 1751
		Invalid,
		// Token: 0x040006D8 RID: 1752
		LStickX,
		// Token: 0x040006D9 RID: 1753
		LStickY,
		// Token: 0x040006DA RID: 1754
		RStickX,
		// Token: 0x040006DB RID: 1755
		RStickY,
		// Token: 0x040006DC RID: 1756
		DPadX,
		// Token: 0x040006DD RID: 1757
		DPadY,
		// Token: 0x040006DE RID: 1758
		LTrigger,
		// Token: 0x040006DF RID: 1759
		RTrigger
	}

	// Token: 0x020001E7 RID: 487
	public enum PadNum
	{
		// Token: 0x040006E1 RID: 1761
		One,
		// Token: 0x040006E2 RID: 1762
		Two,
		// Token: 0x040006E3 RID: 1763
		Three,
		// Token: 0x040006E4 RID: 1764
		Four,
		// Token: 0x040006E5 RID: 1765
		Five,
		// Token: 0x040006E6 RID: 1766
		Six,
		// Token: 0x040006E7 RID: 1767
		Seven,
		// Token: 0x040006E8 RID: 1768
		Eight,
		// Token: 0x040006E9 RID: 1769
		Nine,
		// Token: 0x040006EA RID: 1770
		Ten,
		// Token: 0x040006EB RID: 1771
		Eleven,
		// Token: 0x040006EC RID: 1772
		Twelve,
		// Token: 0x040006ED RID: 1773
		Thriteen,
		// Token: 0x040006EE RID: 1774
		Forteen,
		// Token: 0x040006EF RID: 1775
		Fifteen,
		// Token: 0x040006F0 RID: 1776
		Count,
		// Token: 0x040006F1 RID: 1777
		Invalid = 15
	}

	// Token: 0x020001E8 RID: 488
	public struct ButtonIdentifier
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x00031978 File Offset: 0x0002FD78
		public ButtonIdentifier(ControlPadInput.PadNum _pad, ControlPadInput.Button _button)
		{
			this.pad = _pad;
			this.button = _button;
		}

		// Token: 0x040006F2 RID: 1778
		public ControlPadInput.PadNum pad;

		// Token: 0x040006F3 RID: 1779
		public ControlPadInput.Button button;
	}

	// Token: 0x020001E9 RID: 489
	public struct ValueIdentifier
	{
		// Token: 0x0600081A RID: 2074 RVA: 0x00031988 File Offset: 0x0002FD88
		public ValueIdentifier(ControlPadInput.PadNum _pad, ControlPadInput.Value _value)
		{
			this.pad = _pad;
			this.value = _value;
		}

		// Token: 0x040006F4 RID: 1780
		public ControlPadInput.PadNum pad;

		// Token: 0x040006F5 RID: 1781
		public ControlPadInput.Value value;
	}
}
