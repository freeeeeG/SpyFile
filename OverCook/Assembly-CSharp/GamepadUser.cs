using System;

// Token: 0x0200071C RID: 1820
public abstract class GamepadUser
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06002298 RID: 8856 RVA: 0x000A6752 File Offset: 0x000A4B52
	public virtual GamepadUser.ControlTypeEnum ControlType
	{
		get
		{
			return GamepadUser.ControlTypeEnum.Pad;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06002299 RID: 8857
	public abstract string UID { get; }

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x0600229A RID: 8858
	public abstract string DisplayName { get; }

	// Token: 0x0600229B RID: 8859 RVA: 0x000A6755 File Offset: 0x000A4B55
	public override bool Equals(object obj)
	{
		return obj is GamepadUser && (obj as GamepadUser).UID == this.UID;
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000A677A File Offset: 0x000A4B7A
	public override int GetHashCode()
	{
		return this.UID.GetHashCode();
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000A6788 File Offset: 0x000A4B88
	public static bool operator ==(GamepadUser _a, GamepadUser _b)
	{
		if (_a == null || _b == null)
		{
			return _a == null && _b == null;
		}
		return _a.Equals(_b);
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000A67BA File Offset: 0x000A4BBA
	public static bool operator !=(GamepadUser _a, GamepadUser _b)
	{
		return !(_a == _b);
	}

	// Token: 0x04001A8A RID: 6794
	public PadSide Side = PadSide.Both;

	// Token: 0x04001A8B RID: 6795
	public bool StickyEngagement;

	// Token: 0x0200071D RID: 1821
	public enum ControlTypeEnum
	{
		// Token: 0x04001A8D RID: 6797
		Pad,
		// Token: 0x04001A8E RID: 6798
		Keyboard,
		// Token: 0x04001A8F RID: 6799
		Joycon
	}
}
