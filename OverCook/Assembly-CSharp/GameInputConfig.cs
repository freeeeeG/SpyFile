using System;
using Team17.Online;

// Token: 0x02000826 RID: 2086
[Serializable]
public class GameInputConfig
{
	// Token: 0x060027FD RID: 10237 RVA: 0x000BB80C File Offset: 0x000B9C0C
	public GameInputConfig(GameInputConfig.ConfigEntry[] _entries)
	{
		this.m_playerConfigs = _entries;
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000BB8C8 File Offset: 0x000B9CC8
	public ControlPadInput.ButtonIdentifier[] GetRealButtons(PlayerInputLookup.Player _player, AmbiPadButton _gamepadButton)
	{
		PlayerGameInput inputData = this.GetInputData(_player);
		if (inputData == null || inputData.AmbiControlsMapping == null)
		{
			return new ControlPadInput.ButtonIdentifier[0];
		}
		return this.GetRealButtons(inputData, _gamepadButton);
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000BB904 File Offset: 0x000B9D04
	public ControlPadInput.ButtonIdentifier[] GetRealButtons(PlayerGameInput _playerGameInput, AmbiPadButton _gamepadButton)
	{
		ControlPadInput.Button[] realButtons = _playerGameInput.AmbiControlsMapping.GetRealButtons(_gamepadButton);
		ControlPadInput.Button[] array = PadSidednessDefinition.FilterForSide(_playerGameInput.Side, realButtons);
		Converter<ControlPadInput.Button, ControlPadInput.ButtonIdentifier> converter = (ControlPadInput.Button input) => new ControlPadInput.ButtonIdentifier(_playerGameInput.Pad, input);
		return Array.ConvertAll<ControlPadInput.Button, ControlPadInput.ButtonIdentifier>(array, converter);
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x000BB958 File Offset: 0x000B9D58
	public ControlPadInput.ValueIdentifier[] GetRealValues(PlayerInputLookup.Player _player, AmbiPadValue _gamepadValue)
	{
		PlayerGameInput inputData = this.GetInputData(_player);
		if (inputData == null)
		{
			return new ControlPadInput.ValueIdentifier[0];
		}
		return this.GetRealValues(inputData, _gamepadValue);
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000BB984 File Offset: 0x000B9D84
	public ControlPadInput.ValueIdentifier[] GetRealValues(PlayerGameInput _playerGameInput, AmbiPadValue _gamepadValue)
	{
		ControlPadInput.Value[] realValues = _playerGameInput.AmbiControlsMapping.GetRealValues(_gamepadValue);
		ControlPadInput.Value[] array = PadSidednessDefinition.FilterForSide(_playerGameInput.Side, realValues);
		Converter<ControlPadInput.Value, ControlPadInput.ValueIdentifier> converter = (ControlPadInput.Value input) => new ControlPadInput.ValueIdentifier(_playerGameInput.Pad, input);
		return Array.ConvertAll<ControlPadInput.Value, ControlPadInput.ValueIdentifier>(array, converter);
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000BB9D8 File Offset: 0x000B9DD8
	public PlayerGameInput GetInputData(PlayerInputLookup.Player _player)
	{
		GameInputConfig.ConfigEntry configEntry = Array.Find<GameInputConfig.ConfigEntry>(this.m_playerConfigs, (GameInputConfig.ConfigEntry x) => x.Player == _player);
		if (configEntry != null)
		{
			return new PlayerGameInput(configEntry.Pad, configEntry.Side, configEntry.AmbiControlsMapping);
		}
		return null;
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000BBA2C File Offset: 0x000B9E2C
	public GameInputConfig.ConfigEntry GetInputConfigEntry(PlayerInputLookup.Player _player)
	{
		return Array.Find<GameInputConfig.ConfigEntry>(this.m_playerConfigs, (GameInputConfig.ConfigEntry x) => x.Player == _player && x.IsLocal());
	}

	// Token: 0x04001F8C RID: 8076
	public GameInputConfig.ConfigEntry[] m_playerConfigs = new GameInputConfig.ConfigEntry[]
	{
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.One, ControlPadInput.PadNum.One, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Two, ControlPadInput.PadNum.Two, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Three, ControlPadInput.PadNum.Three, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Four, ControlPadInput.PadNum.Four, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Five, ControlPadInput.PadNum.Five, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Six, ControlPadInput.PadNum.Six, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Seven, ControlPadInput.PadNum.Seven, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Eight, ControlPadInput.PadNum.Eight, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Nine, ControlPadInput.PadNum.Nine, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Ten, ControlPadInput.PadNum.Ten, PadSide.Both, User.MachineID.Count, null),
		new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Eleven, ControlPadInput.PadNum.Eleven, PadSide.Both, User.MachineID.Count, null)
	};

	// Token: 0x02000827 RID: 2087
	[Serializable]
	public class ConfigEntry
	{
		// Token: 0x06002804 RID: 10244 RVA: 0x000BBA5D File Offset: 0x000B9E5D
		public ConfigEntry(PlayerInputLookup.Player _player, ControlPadInput.PadNum _pad, PadSide _side = PadSide.Both, User.MachineID _machineId = User.MachineID.Count, AmbiControlsMappingData _mappingData = null)
		{
			this.Player = _player;
			this.Pad = _pad;
			this.Side = _side;
			this.UIHandedness = _side;
			this.MachineId = _machineId;
			this.AmbiControlsMapping = _mappingData;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000BBA98 File Offset: 0x000B9E98
		public bool IsLocal()
		{
			return this.MachineId == ClientUserSystem.s_LocalMachineId;
		}

		// Token: 0x04001F8D RID: 8077
		public PlayerInputLookup.Player Player;

		// Token: 0x04001F8E RID: 8078
		public ControlPadInput.PadNum Pad;

		// Token: 0x04001F8F RID: 8079
		public PadSide Side;

		// Token: 0x04001F90 RID: 8080
		public User.MachineID MachineId;

		// Token: 0x04001F91 RID: 8081
		public AmbiControlsMappingData AmbiControlsMapping;

		// Token: 0x04001F92 RID: 8082
		public PadSide UIHandedness = PadSide.Both;
	}
}
