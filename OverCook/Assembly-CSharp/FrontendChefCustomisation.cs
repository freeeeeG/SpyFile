using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAE RID: 2734
public class FrontendChefCustomisation : MonoBehaviour
{
	// Token: 0x06003633 RID: 13875 RVA: 0x000FE070 File Offset: 0x000FC470
	public bool IsActive()
	{
		return this.m_assignedInput != null;
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x000FE080 File Offset: 0x000FC480
	private int SortAvatarsByDlc(ChefAvatarData a, ChefAvatarData b)
	{
		int num = (!(a.ForDlc != null)) ? -1 : a.ForDlc.m_DLCID;
		int value = (!(b.ForDlc != null)) ? -1 : b.ForDlc.m_DLCID;
		int num2 = num.CompareTo(value);
		if (num2 == 0)
		{
			int num3 = this.m_avatarDirectory.FindIndex_Predicate((ChefAvatarData x) => x == a);
			int value2 = this.m_avatarDirectory.FindIndex_Predicate((ChefAvatarData x) => x == b);
			num2 = num3.CompareTo(value2);
		}
		return num2;
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x000FE147 File Offset: 0x000FC547
	public void RebindInput(PlayerGameInput _input)
	{
		if (base.enabled)
		{
			this.m_assignedInput = _input;
			this.m_leftButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UILeftPlayerSpecific, _input);
			this.m_rightButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UIRightPlayerSpecific, _input);
		}
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x000FE178 File Offset: 0x000FC578
	public void Activate(PlayerGameInput _input, GameSession.SelectedChefData _default = null)
	{
		this.m_assignedInput = _input;
		this.m_chefData = _default;
		this.m_leftButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UILeftPlayerSpecific, _input);
		this.m_rightButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UIRightPlayerSpecific, _input);
		this.m_avatarDirectory = GameUtils.GetAvatarDirectoryData().Avatars;
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		this.m_unlockedAvatars = metaGameProgress.GetUnlockedAvatars();
		Array.Sort<ChefAvatarData>(this.m_unlockedAvatars, new Comparison<ChefAvatarData>(this.SortAvatarsByDlc));
		if (_default != null)
		{
			if (T17FrontendFlow.Instance != null && T17FrontendFlow.Instance.AutoOpenFrontendDlcData != null)
			{
				int dlcID = T17FrontendFlow.Instance.AutoOpenFrontendDlcData.m_DLCID;
				this.SetChefSelection(this.m_unlockedAvatars.FindIndex_Predicate((ChefAvatarData x) => x.ForDlc != null && x.ForDlc.m_DLCID == dlcID));
			}
			else
			{
				this.m_chefSelection = this.m_unlockedAvatars.FindIndex_Predicate((ChefAvatarData x) => x == _default.Character);
			}
		}
		if (this.leftArrow != null)
		{
			this.leftArrow.transform.parent.gameObject.SetActive(true);
			this.leftArrow.enabled = true;
			this.leftArrowAnimator = this.leftArrow.GetComponent<Animator>();
		}
		if (this.rightArrow != null)
		{
			this.rightArrow.transform.parent.gameObject.SetActive(true);
			this.rightArrow.enabled = true;
			this.rightArrowAnimator = this.rightArrow.GetComponent<Animator>();
		}
		this.m_adminLayerMask = LayerMask.NameToLayer("Administration");
		base.enabled = true;
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x000FE330 File Offset: 0x000FC730
	public void ActivateLayoutOnly()
	{
		this.leftArrow.transform.parent.gameObject.SetActive(true);
		this.leftArrow.enabled = false;
		this.rightArrow.transform.parent.gameObject.SetActive(true);
		this.rightArrow.enabled = false;
		this.UnbindInput();
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x000FE394 File Offset: 0x000FC794
	public void Deactivate()
	{
		if (this.m_chefData != null)
		{
			Analytics.LogEvent("Chef Picked", "Chef " + this.m_chefData.Character.HeadName, (long)this.m_chefSelection, (Analytics.Flags)0);
		}
		this.UnbindInput();
		this.m_chefData = null;
		if (this.leftArrow != null)
		{
			this.leftArrow.transform.parent.gameObject.SetActive(false);
			this.leftArrow.enabled = false;
		}
		if (this.rightArrow != null)
		{
			this.rightArrow.transform.parent.gameObject.SetActive(false);
			this.rightArrow.enabled = false;
		}
		base.enabled = false;
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.AutoOpenChefSelectionMenu(null);
		}
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x000FE477 File Offset: 0x000FC877
	private void UnbindInput()
	{
		this.m_assignedInput = null;
		this.m_leftButton = null;
		this.m_rightButton = null;
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x000FE48E File Offset: 0x000FC88E
	public GameSession.SelectedChefData GetCurrentSelection()
	{
		return this.m_chefData;
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x000FE496 File Offset: 0x000FC896
	public PlayerGameInput GetAssignedInput()
	{
		return this.m_assignedInput;
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x000FE4A0 File Offset: 0x000FC8A0
	private void Update()
	{
		if (this.m_leftButton == null || this.m_rightButton == null)
		{
			return;
		}
		if (T17DialogBoxManager.HasAnyOpenDialogs())
		{
			this.m_leftButton.ClaimPressEvent();
			this.m_rightButton.ClaimPressEvent();
			return;
		}
		if (this.m_leftButton.JustPressed())
		{
			this.OnClickLeftButton();
		}
		if (this.m_rightButton.JustPressed())
		{
			this.OnClickRightButton();
		}
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x000FE511 File Offset: 0x000FC911
	public void OnClickLeftButton()
	{
		this.SetChefSelection(this.m_chefSelection - 1);
		this.leftArrowAnimator.SetTrigger(FrontendChefCustomisation.m_iPressed);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIButtonDown, this.m_adminLayerMask);
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x000FE53F File Offset: 0x000FC93F
	public void OnClickRightButton()
	{
		this.SetChefSelection(this.m_chefSelection + 1);
		this.rightArrowAnimator.SetTrigger(FrontendChefCustomisation.m_iPressed);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIButtonDown, this.m_adminLayerMask);
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x000FE570 File Offset: 0x000FC970
	private void SetChefSelection(int _selection)
	{
		if (this.m_unlockedAvatars.Length > 0)
		{
			this.m_chefSelection = MathUtils.Wrap(_selection, 0, this.m_unlockedAvatars.Length);
			ChefAvatarData newAvatar = this.m_unlockedAvatars[this.m_chefSelection];
			this.m_chefData.Character = newAvatar;
			int num = this.m_avatarDirectory.FindIndex_Predicate((ChefAvatarData x) => x == newAvatar);
			uint chefAvatar;
			if (num < 0)
			{
				chefAvatar = 127U;
			}
			else
			{
				chefAvatar = (uint)num;
			}
			FastList<User> users = ClientUserSystem.m_Users;
			if (this.m_actualPlayer < (PlayerInputLookup.Player)users.Count)
			{
				ClientMessenger.ChefAvatar(chefAvatar, users._items[(int)this.m_actualPlayer]);
			}
		}
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x000FE61F File Offset: 0x000FCA1F
	public void SetChefMeshReplacer(ChefMeshReplacer _mesh)
	{
		this.m_chef = _mesh;
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x000FE628 File Offset: 0x000FCA28
	public void SetChefAnimator(Animator animator)
	{
		this.m_ChefAnimator = animator;
		if (this.m_bPlaySelecting)
		{
			this.PlaySelectingAnimation();
			this.m_bPlaySelecting = false;
		}
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x000FE649 File Offset: 0x000FCA49
	public void PlaySelectedAnimation()
	{
		if (this.m_ChefAnimator != null)
		{
			this.m_ChefAnimator.SetTrigger(FrontendChefCustomisation.m_iTrigChefChosen);
		}
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x000FE66C File Offset: 0x000FCA6C
	public void PlaySelectingAnimation()
	{
		if (this.m_ChefAnimator != null)
		{
			this.m_ChefAnimator.SetTrigger(FrontendChefCustomisation.m_iTrigChooseChef);
		}
	}

	// Token: 0x04002B99 RID: 11161
	[SerializeField]
	public PlayerInputLookup.Player m_actualPlayer;

	// Token: 0x04002B9A RID: 11162
	[SerializeField]
	private ChefMeshReplacer m_chef;

	// Token: 0x04002B9B RID: 11163
	[SerializeField]
	private Image leftArrow;

	// Token: 0x04002B9C RID: 11164
	[SerializeField]
	private Image rightArrow;

	// Token: 0x04002B9D RID: 11165
	private PlayerGameInput m_assignedInput;

	// Token: 0x04002B9E RID: 11166
	private ILogicalButton m_leftButton;

	// Token: 0x04002B9F RID: 11167
	private ILogicalButton m_rightButton;

	// Token: 0x04002BA0 RID: 11168
	private ChefAvatarData[] m_unlockedAvatars;

	// Token: 0x04002BA1 RID: 11169
	private ChefAvatarData[] m_avatarDirectory;

	// Token: 0x04002BA2 RID: 11170
	private int m_chefSelection = -1;

	// Token: 0x04002BA3 RID: 11171
	private GameSession.SelectedChefData m_chefData;

	// Token: 0x04002BA4 RID: 11172
	private Animator leftArrowAnimator;

	// Token: 0x04002BA5 RID: 11173
	private Animator rightArrowAnimator;

	// Token: 0x04002BA6 RID: 11174
	private Animator m_ChefAnimator;

	// Token: 0x04002BA7 RID: 11175
	private bool m_bPlaySelecting;

	// Token: 0x04002BA8 RID: 11176
	private static readonly int m_iTrigChooseChef = Animator.StringToHash("TrigChooseChef");

	// Token: 0x04002BA9 RID: 11177
	private static readonly int m_iTrigChefChosen = Animator.StringToHash("TrigChefChosen");

	// Token: 0x04002BAA RID: 11178
	private static readonly int m_iPressed = Animator.StringToHash("Pressed");

	// Token: 0x04002BAB RID: 11179
	private int m_adminLayerMask;
}
