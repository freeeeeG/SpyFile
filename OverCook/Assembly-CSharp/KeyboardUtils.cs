using System;

// Token: 0x02000829 RID: 2089
public static class KeyboardUtils
{
	// Token: 0x06002807 RID: 10247 RVA: 0x000BBB24 File Offset: 0x000B9F24
	public static bool IsKeyboard(PlayerGameInput input)
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		GamepadUser user = playerManager.GetUser((EngagementSlot)input.Pad);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000BBB5C File Offset: 0x000B9F5C
	public static bool IsKeyboard(PlayerInputLookup.Player player)
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(player);
		GamepadUser user = playerManager.GetUser((EngagementSlot)padForPlayer);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x000BBB98 File Offset: 0x000B9F98
	public static bool KeyboardSide(PlayerGameInput input, out PadSide side)
	{
		side = input.Side;
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		GamepadUser user = playerManager.GetUser((EngagementSlot)input.Pad);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}
}
