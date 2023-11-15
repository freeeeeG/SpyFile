using System;
using flanne.Player;
using flanne.UI;
using flanne.UIExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.Core
{
	// Token: 0x020001EC RID: 492
	public class GameController : StateMachine
	{
		// Token: 0x06000AE3 RID: 2787 RVA: 0x00029B84 File Offset: 0x00027D84
		private void Awake()
		{
			LeanTween.init(5000000);
			TagLayerUtil.Init();
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00029B95 File Offset: 0x00027D95
		private void Start()
		{
			this.ChangeState<InitState>();
		}

		// Token: 0x040007AA RID: 1962
		public PlayerInput playerInput;

		// Token: 0x040007AB RID: 1963
		public PauseController pauseController;

		// Token: 0x040007AC RID: 1964
		public PowerupGenerator powerupGenerator;

		// Token: 0x040007AD RID: 1965
		public int numPowerupChoices = 5;

		// Token: 0x040007AE RID: 1966
		public GameObject player;

		// Token: 0x040007AF RID: 1967
		public PlayerHealth playerHealth;

		// Token: 0x040007B0 RID: 1968
		public PlayerXP playerXP;

		// Token: 0x040007B1 RID: 1969
		public GameObject playerFogRevealer;

		// Token: 0x040007B2 RID: 1970
		public CameraRig playerCameraRig;

		// Token: 0x040007B3 RID: 1971
		public Animator levelupAnimator;

		// Token: 0x040007B4 RID: 1972
		public ScreenFlash screenFlash;

		// Token: 0x040007B5 RID: 1973
		public ShootingCursor shootingCursor;

		// Token: 0x040007B6 RID: 1974
		[Header("Audio")]
		public AudioClip battleMusic;

		// Token: 0x040007B7 RID: 1975
		public SoundEffectSO youSurvivedSFX;

		// Token: 0x040007B8 RID: 1976
		public SoundEffectSO youDiedSFX;

		// Token: 0x040007B9 RID: 1977
		public SoundEffectSO powerupMenuSFX;

		// Token: 0x040007BA RID: 1978
		public SoundEffectSO levelUpSFX;

		// Token: 0x040007BB RID: 1979
		public SoundEffectSO gunEvoStartSFX;

		// Token: 0x040007BC RID: 1980
		public SoundEffectSO gunEvoMenuSFX;

		// Token: 0x040007BD RID: 1981
		public SoundEffectSO gunEvoEndSFX;

		// Token: 0x040007BE RID: 1982
		[Header("UI")]
		public flanne.UIExtensions.Menu optionsMenu;

		// Token: 0x040007BF RID: 1983
		public flanne.UI.Panel hud;

		// Token: 0x040007C0 RID: 1984
		public ChestUIController chestUIController;

		// Token: 0x040007C1 RID: 1985
		public flanne.UI.Panel powerupMenuPanel;

		// Token: 0x040007C2 RID: 1986
		public PowerupMenu powerupMenu;

		// Token: 0x040007C3 RID: 1987
		public Button powerupRerollButton;

		// Token: 0x040007C4 RID: 1988
		public flanne.UI.Panel devilDealMenuPanel;

		// Token: 0x040007C5 RID: 1989
		public PowerupMenu devilDealMenu;

		// Token: 0x040007C6 RID: 1990
		public Button devilDealLeaveButton;

		// Token: 0x040007C7 RID: 1991
		public EndScreenUIC endScreenUIC;

		// Token: 0x040007C8 RID: 1992
		public Button retryRunButton;

		// Token: 0x040007C9 RID: 1993
		public Button quitToTitleButton;

		// Token: 0x040007CA RID: 1994
		public flanne.UI.Panel pauseMenu;

		// Token: 0x040007CB RID: 1995
		public Button pauseResumeButton;

		// Token: 0x040007CC RID: 1996
		public Button optionsButton;

		// Token: 0x040007CD RID: 1997
		public Button synergiesButton;

		// Token: 0x040007CE RID: 1998
		public Button giveupButton;

		// Token: 0x040007CF RID: 1999
		public flanne.UI.Panel mouseAmmoUI;

		// Token: 0x040007D0 RID: 2000
		public flanne.UI.Panel powerupListUI;

		// Token: 0x040007D1 RID: 2001
		public flanne.UI.Panel loadoutUI;

		// Token: 0x040007D2 RID: 2002
		public flanne.UI.Panel gunEvoPanel;

		// Token: 0x040007D3 RID: 2003
		public flanne.UI.Menu gunEvoMenu;

		// Token: 0x040007D4 RID: 2004
		public flanne.UI.Panel haloUIPanel;

		// Token: 0x040007D5 RID: 2005
		public PowerupDescription haloUI;

		// Token: 0x040007D6 RID: 2006
		public Button takeHaloButton;

		// Token: 0x040007D7 RID: 2007
		public flanne.UI.Panel synergiesUIPanel;

		// Token: 0x040007D8 RID: 2008
		public Button synergiesUIBackButton;

		// Token: 0x040007D9 RID: 2009
		public ShootDetector shootDetector;

		// Token: 0x040007DA RID: 2010
		public HitlessDetector hitlessDetector;

		// Token: 0x040007DB RID: 2011
		public flanne.UI.Panel leaderBoardPanel;

		// Token: 0x040007DC RID: 2012
		public LeaderboardUI leaderboardUI;

		// Token: 0x040007DD RID: 2013
		public flanne.UI.Panel hasturUnlockedPanel;

		// Token: 0x040007DE RID: 2014
		public Button hasturUnlockedConfirmButton;

		// Token: 0x040007DF RID: 2015
		public flanne.UI.Panel ravenUnlockedPanel;

		// Token: 0x040007E0 RID: 2016
		public Button ravenUnlockedConfirmButton;
	}
}
