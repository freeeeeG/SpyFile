using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF1 RID: 2801
public class PlayerButtonImage : MonoBehaviour
{
	// Token: 0x060038BB RID: 14523 RVA: 0x0010B8BC File Offset: 0x00109CBC
	private void Awake()
	{
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.RefreshImage();
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshImage));
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x0010B911 File Offset: 0x00109D11
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _b, GamepadUser _a)
	{
		this.RefreshImage();
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x0010B919 File Offset: 0x00109D19
	private void OnDestroy()
	{
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshImage));
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x0010B954 File Offset: 0x00109D54
	public static bool IsKeyBoard(PlayerManager _playerManger, PlayerInputLookup.Player _player)
	{
		ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(_player);
		GamepadUser user = _playerManger.GetUser((EngagementSlot)padForPlayer);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x0010B988 File Offset: 0x00109D88
	public static ControllerIconLookup.DeviceContext GetDevice(PlayerManager _playerManger, PlayerInputLookup.Player _player)
	{
		bool flag = PlayerButtonImage.IsKeyBoard(_playerManger, _player);
		ControllerIconLookup.DeviceContext result = ControllerIconLookup.DeviceContext.Pad;
		if (flag)
		{
			result = ControllerIconLookup.DeviceContext.Keyboard;
			if (StandardActionSet.IsKeyboardSplit())
			{
				result = ControllerIconLookup.DeviceContext.SplitKeyboard;
			}
		}
		return result;
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x0010B9B4 File Offset: 0x00109DB4
	private void RefreshImage()
	{
		ControllerIconLookup controllerIconLookup = GameUtils.RequireManager<ControllerIconLookup>();
		ControllerIconLookup.DeviceContext device = PlayerButtonImage.GetDevice(this.m_playerManager, this.m_player);
		Sprite sprite = null;
		float num = 1f;
		ControlPadInput.Button? button = PlayerButtonImage.GetControlPadButton<ControlPadInput.Button>(this.m_control, this.m_player, device);
		if (button != null)
		{
			if (PlayerManagerShared<PCPlayerManager.PCPlayerProfile>.AcceptAndCancelButtonsInverted)
			{
				if (button.Value == ControlPadInput.Button.A)
				{
					button = new ControlPadInput.Button?(ControlPadInput.Button.B);
				}
				else if (button.Value == ControlPadInput.Button.B)
				{
					button = new ControlPadInput.Button?(ControlPadInput.Button.A);
				}
			}
			sprite = controllerIconLookup.GetIcon(button.Value, this.m_context, device);
			num = controllerIconLookup.GetIconScale(button.Value, this.m_context, device);
		}
		this.m_image.sprite = sprite;
		if (sprite != null && this.m_lookupScale)
		{
			base.transform.localScale = new Vector3(num, num, 1f);
		}
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x0010BAA0 File Offset: 0x00109EA0
	public static T GetButtonComponent<T>(ILogicalButton _compositeButton) where T : LogicalButtonBase
	{
		AcyclicGraph<ILogicalElement, LogicalLinkInfo> acyclicGraph;
		AcyclicGraph<ILogicalElement, LogicalLinkInfo>.Node node;
		_compositeButton.GetLogicTreeData(out acyclicGraph, out node);
		AcyclicGraphEnumerator<ILogicalElement, LogicalLinkInfo> enumerator = acyclicGraph.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ILogicalElement logicalElement = enumerator.Current;
				T t = logicalElement as T;
				if (t != null)
				{
					return t;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return (T)((object)null);
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x0010BB2C File Offset: 0x00109F2C
	public static T? GetControlPadButton<T>(ILogicalButton _iLogicalButton, ControllerIconLookup.DeviceContext device) where T : struct
	{
		if (device == ControllerIconLookup.DeviceContext.Pad)
		{
			LogicalPadButtonBase<T> buttonComponent = PlayerButtonImage.GetButtonComponent<LogicalPadButtonBase<T>>(_iLogicalButton);
			if (buttonComponent != null)
			{
				return new T?(buttonComponent.GetControlpadButton());
			}
		}
		else
		{
			LogicalKeycodeButtonBase<T> buttonComponent2 = PlayerButtonImage.GetButtonComponent<LogicalKeycodeButtonBase<T>>(_iLogicalButton);
			if (buttonComponent2 != null)
			{
				return new T?(buttonComponent2.GetControlButton());
			}
		}
		return null;
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x0010BB80 File Offset: 0x00109F80
	public static T? GetControlPadButton<T>(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _player, ControllerIconLookup.DeviceContext device) where T : struct
	{
		ILogicalButton button = PlayerInputLookup.GetButton(_id, _player);
		return PlayerButtonImage.GetControlPadButton<T>(button, device);
	}

	// Token: 0x04002D60 RID: 11616
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Image m_image;

	// Token: 0x04002D61 RID: 11617
	[SerializeField]
	private PlayerInputLookup.LogicalButtonID m_control = PlayerInputLookup.LogicalButtonID.UISelect;

	// Token: 0x04002D62 RID: 11618
	[SerializeField]
	private PlayerInputLookup.Player m_player;

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	private ControllerIconLookup.IconContext m_context;

	// Token: 0x04002D64 RID: 11620
	[SerializeField]
	private bool m_lookupScale = true;

	// Token: 0x04002D65 RID: 11621
	private PlayerManager m_playerManager;
}
