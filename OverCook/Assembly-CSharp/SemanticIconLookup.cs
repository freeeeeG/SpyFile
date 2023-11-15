using System;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
public class SemanticIconLookup : Manager
{
	// Token: 0x06003930 RID: 14640 RVA: 0x0010F89E File Offset: 0x0010DC9E
	private void Awake()
	{
		this.m_controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x0010F8B8 File Offset: 0x0010DCB8
	private bool IsSemantic()
	{
		GameInputConfig inputConfig = PlayerInputLookup.GetInputConfig();
		GameInputConfig.ConfigEntry[] playerConfigs = inputConfig.m_playerConfigs;
		playerConfigs = playerConfigs.AllRemoved_Predicate((GameInputConfig.ConfigEntry x) => this.GetUser(x) == null || !x.IsLocal());
		if (playerConfigs.Length == 0)
		{
			return true;
		}
		bool flag = playerConfigs.Contains((GameInputConfig.ConfigEntry x) => x.Side != playerConfigs[0].Side);
		bool flag2 = playerConfigs.Contains((GameInputConfig.ConfigEntry x) => this.GetUser(x).ControlType != this.GetUser(playerConfigs[0]).ControlType);
		bool flag3 = playerConfigs.Contains((GameInputConfig.ConfigEntry x) => this.GetUser(x).ControlType == GamepadUser.ControlTypeEnum.Keyboard);
		return flag || flag2 || flag3;
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x0010F96A File Offset: 0x0010DD6A
	private GamepadUser GetUser(GameInputConfig.ConfigEntry _entry)
	{
		return this.m_playerManager.GetUser((EngagementSlot)_entry.Pad);
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x0010F980 File Offset: 0x0010DD80
	public Sprite GetSemanticIcon(SemanticIconLookup.Semantic _semantic)
	{
		SemanticIconLookup.ButtonSet iconPack = this.m_platformSet.GetIconPack();
		return iconPack.GetSemanticIcon(_semantic);
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x0010F9A0 File Offset: 0x0010DDA0
	public Sprite GetIcon(SemanticIconLookup.Semantic _semantic, PlayerInputLookup.Player _player = PlayerInputLookup.Player.One, ControllerIconLookup.IconContext _context = ControllerIconLookup.IconContext.Bordered)
	{
		SemanticIconLookup.ButtonSet iconPack = this.m_platformSet.GetIconPack();
		if (this.IsSemantic())
		{
			return iconPack.GetSemanticIcon(_semantic);
		}
		PlayerInputLookup.LogicalButtonID button = iconPack.GetButton(_semantic);
		ControllerIconLookup.DeviceContext device = PlayerButtonImage.GetDevice(this.m_playerManager, _player);
		ControlPadInput.Button button2 = PlayerButtonImage.GetControlPadButton<ControlPadInput.Button>(button, _player, device).Value;
		if (PlayerManagerShared<PCPlayerManager.PCPlayerProfile>.AcceptAndCancelButtonsInverted)
		{
			if (button2 == ControlPadInput.Button.A)
			{
				button2 = ControlPadInput.Button.B;
			}
			else if (button2 == ControlPadInput.Button.B)
			{
				button2 = ControlPadInput.Button.A;
			}
		}
		return this.m_controllerIconLookup.GetIcon(button2, _context, device);
	}

	// Token: 0x04002DE9 RID: 11753
	[SerializeField]
	private SemanticIconLookup.PlatformSet m_platformSet;

	// Token: 0x04002DEA RID: 11754
	private PlayerManager m_playerManager;

	// Token: 0x04002DEB RID: 11755
	private ControllerIconLookup m_controllerIconLookup;

	// Token: 0x02000B09 RID: 2825
	[Serializable]
	private class ButtonSet
	{
		// Token: 0x06003936 RID: 14646 RVA: 0x0010FA30 File Offset: 0x0010DE30
		public Sprite GetSemanticIcon(SemanticIconLookup.Semantic _s)
		{
			switch (_s)
			{
			case SemanticIconLookup.Semantic.Pickup:
				return this.Pickup;
			case SemanticIconLookup.Semantic.Chop:
				return this.Chop;
			case SemanticIconLookup.Semantic.Button:
				return this.Button;
			case SemanticIconLookup.Semantic.Washing:
				return this.Washing;
			case SemanticIconLookup.Semantic.FireExtinguisher:
				return this.FireExtinguisher;
			case SemanticIconLookup.Semantic.Talk:
				return this.Talk;
			case SemanticIconLookup.Semantic.Portal:
				return this.Portal;
			case SemanticIconLookup.Semantic.Switch:
				return this.Switch;
			case SemanticIconLookup.Semantic.Whack:
				return this.Whack;
			case SemanticIconLookup.Semantic.Dash:
				return this.Dash;
			default:
				return this.Generic;
			}
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x0010FABC File Offset: 0x0010DEBC
		public PlayerInputLookup.LogicalButtonID GetButton(SemanticIconLookup.Semantic _s)
		{
			switch (_s)
			{
			case SemanticIconLookup.Semantic.Pickup:
			case SemanticIconLookup.Semantic.FireExtinguisher:
			case SemanticIconLookup.Semantic.Talk:
			case SemanticIconLookup.Semantic.Portal:
				return PlayerInputLookup.LogicalButtonID.PickupAndDrop;
			case SemanticIconLookup.Semantic.Chop:
			case SemanticIconLookup.Semantic.Button:
			case SemanticIconLookup.Semantic.Washing:
			case SemanticIconLookup.Semantic.Whack:
				return PlayerInputLookup.LogicalButtonID.WorkstationInteract;
			case SemanticIconLookup.Semantic.Switch:
				return PlayerInputLookup.LogicalButtonID.PlayerSwitch;
			case SemanticIconLookup.Semantic.Dash:
				return PlayerInputLookup.LogicalButtonID.Dash;
			default:
				return PlayerInputLookup.LogicalButtonID.WorkstationInteract;
			}
		}

		// Token: 0x04002DEC RID: 11756
		public Sprite Pickup;

		// Token: 0x04002DED RID: 11757
		public Sprite Chop;

		// Token: 0x04002DEE RID: 11758
		public Sprite Button;

		// Token: 0x04002DEF RID: 11759
		public Sprite Washing;

		// Token: 0x04002DF0 RID: 11760
		public Sprite FireExtinguisher;

		// Token: 0x04002DF1 RID: 11761
		public Sprite Talk;

		// Token: 0x04002DF2 RID: 11762
		public Sprite Portal;

		// Token: 0x04002DF3 RID: 11763
		public Sprite Switch;

		// Token: 0x04002DF4 RID: 11764
		public Sprite Whack;

		// Token: 0x04002DF5 RID: 11765
		public Sprite Dash;

		// Token: 0x04002DF6 RID: 11766
		public Sprite Generic;
	}

	// Token: 0x02000B0A RID: 2826
	[Serializable]
	private class PlatformSet
	{
		// Token: 0x06003939 RID: 14649 RVA: 0x0010FB03 File Offset: 0x0010DF03
		public SemanticIconLookup.ButtonSet GetIconPack()
		{
			if (KeyboardUtils.IsKeyboard(PlayerInputLookup.Player.One))
			{
				return this.PC;
			}
			return this.XboxOne;
		}

		// Token: 0x04002DF7 RID: 11767
		public SemanticIconLookup.ButtonSet XboxOne;

		// Token: 0x04002DF8 RID: 11768
		public SemanticIconLookup.ButtonSet PS4;

		// Token: 0x04002DF9 RID: 11769
		public SemanticIconLookup.ButtonSet Switch;

		// Token: 0x04002DFA RID: 11770
		public SemanticIconLookup.ButtonSet PC;
	}

	// Token: 0x02000B0B RID: 2827
	public enum Semantic
	{
		// Token: 0x04002DFC RID: 11772
		Pickup,
		// Token: 0x04002DFD RID: 11773
		Chop,
		// Token: 0x04002DFE RID: 11774
		Button,
		// Token: 0x04002DFF RID: 11775
		Washing,
		// Token: 0x04002E00 RID: 11776
		FireExtinguisher,
		// Token: 0x04002E01 RID: 11777
		Talk,
		// Token: 0x04002E02 RID: 11778
		Portal,
		// Token: 0x04002E03 RID: 11779
		Switch,
		// Token: 0x04002E04 RID: 11780
		Whack,
		// Token: 0x04002E05 RID: 11781
		Dash,
		// Token: 0x04002E06 RID: 11782
		Generic
	}
}
