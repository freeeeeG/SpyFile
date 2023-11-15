using System;
using flanne.Player;
using flanne.UI;
using flanne.UIExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.Core
{
	// Token: 0x020001ED RID: 493
	public abstract class GameState : State
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00029BAC File Offset: 0x00027DAC
		protected PlayerInput playerInput
		{
			get
			{
				return this.owner.playerInput;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00029BB9 File Offset: 0x00027DB9
		protected PauseController pauseController
		{
			get
			{
				return this.owner.pauseController;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00029BC6 File Offset: 0x00027DC6
		protected PowerupGenerator powerupGenerator
		{
			get
			{
				return this.owner.powerupGenerator;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00029BD3 File Offset: 0x00027DD3
		protected int numPowerupChoices
		{
			get
			{
				return this.owner.numPowerupChoices;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00029BE0 File Offset: 0x00027DE0
		protected GameObject player
		{
			get
			{
				return this.owner.player;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00029BED File Offset: 0x00027DED
		protected PlayerHealth playerHealth
		{
			get
			{
				return this.owner.playerHealth;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00029BFA File Offset: 0x00027DFA
		protected PlayerXP playerXP
		{
			get
			{
				return this.owner.playerXP;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00029C07 File Offset: 0x00027E07
		protected GameObject playerFogRevealer
		{
			get
			{
				return this.owner.playerFogRevealer;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00029C14 File Offset: 0x00027E14
		protected CameraRig playerCameraRig
		{
			get
			{
				return this.owner.playerCameraRig;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00029C21 File Offset: 0x00027E21
		protected AudioClip battleMusic
		{
			get
			{
				return this.owner.battleMusic;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00029C2E File Offset: 0x00027E2E
		protected SoundEffectSO youSurvivedSFX
		{
			get
			{
				return this.owner.youSurvivedSFX;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00029C3B File Offset: 0x00027E3B
		protected SoundEffectSO youDiedSFX
		{
			get
			{
				return this.owner.youDiedSFX;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00029C48 File Offset: 0x00027E48
		protected SoundEffectSO powerupMenuSFX
		{
			get
			{
				return this.owner.powerupMenuSFX;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00029C55 File Offset: 0x00027E55
		protected SoundEffectSO levelUpSFX
		{
			get
			{
				return this.owner.levelUpSFX;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00029C62 File Offset: 0x00027E62
		protected SoundEffectSO gunEvoStartSFX
		{
			get
			{
				return this.owner.gunEvoStartSFX;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00029C6F File Offset: 0x00027E6F
		protected SoundEffectSO gunEvoMenuSFX
		{
			get
			{
				return this.owner.gunEvoMenuSFX;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00029C7C File Offset: 0x00027E7C
		protected SoundEffectSO gunEvoEndSFX
		{
			get
			{
				return this.owner.gunEvoEndSFX;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00029C89 File Offset: 0x00027E89
		protected Animator levelupAnimator
		{
			get
			{
				return this.owner.levelupAnimator;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00029C96 File Offset: 0x00027E96
		protected ScreenFlash screenFlash
		{
			get
			{
				return this.owner.screenFlash;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00029CA3 File Offset: 0x00027EA3
		protected ShootingCursor shootingCursor
		{
			get
			{
				return this.owner.shootingCursor;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x00029CB0 File Offset: 0x00027EB0
		protected flanne.UIExtensions.Menu optionsMenu
		{
			get
			{
				return this.owner.optionsMenu;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00029CBD File Offset: 0x00027EBD
		protected flanne.UI.Panel hud
		{
			get
			{
				return this.owner.hud;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00029CCA File Offset: 0x00027ECA
		protected flanne.UI.Panel powerupMenuPanel
		{
			get
			{
				return this.owner.powerupMenuPanel;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00029CD7 File Offset: 0x00027ED7
		protected PowerupMenu powerupMenu
		{
			get
			{
				return this.owner.powerupMenu;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00029CE4 File Offset: 0x00027EE4
		protected Button powerupRerollButton
		{
			get
			{
				return this.owner.powerupRerollButton;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00029CF1 File Offset: 0x00027EF1
		protected flanne.UI.Panel devilDealMenuPanel
		{
			get
			{
				return this.owner.devilDealMenuPanel;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00029CFE File Offset: 0x00027EFE
		protected PowerupMenu devilDealMenu
		{
			get
			{
				return this.owner.devilDealMenu;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00029D0B File Offset: 0x00027F0B
		protected Button devilDealLeaveButton
		{
			get
			{
				return this.owner.devilDealLeaveButton;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00029D18 File Offset: 0x00027F18
		protected ChestUIController chestUIController
		{
			get
			{
				return this.owner.chestUIController;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00029D25 File Offset: 0x00027F25
		protected EndScreenUIC endScreenUIC
		{
			get
			{
				return this.owner.endScreenUIC;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00029D32 File Offset: 0x00027F32
		protected Button retryRunButton
		{
			get
			{
				return this.owner.retryRunButton;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00029D3F File Offset: 0x00027F3F
		protected Button quitToTitleButton
		{
			get
			{
				return this.owner.quitToTitleButton;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00029D4C File Offset: 0x00027F4C
		protected flanne.UI.Panel pauseMenu
		{
			get
			{
				return this.owner.pauseMenu;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00029D59 File Offset: 0x00027F59
		protected Button pauseResumeButton
		{
			get
			{
				return this.owner.pauseResumeButton;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00029D66 File Offset: 0x00027F66
		protected Button optionsButton
		{
			get
			{
				return this.owner.optionsButton;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00029D73 File Offset: 0x00027F73
		protected Button synergiesButton
		{
			get
			{
				return this.owner.synergiesButton;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00029D80 File Offset: 0x00027F80
		protected Button giveupButton
		{
			get
			{
				return this.owner.giveupButton;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00029D8D File Offset: 0x00027F8D
		protected flanne.UI.Panel mouseAmmoUI
		{
			get
			{
				return this.owner.mouseAmmoUI;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00029D9A File Offset: 0x00027F9A
		protected flanne.UI.Panel powerupListUI
		{
			get
			{
				return this.owner.powerupListUI;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00029DA7 File Offset: 0x00027FA7
		protected flanne.UI.Panel loadoutUI
		{
			get
			{
				return this.owner.loadoutUI;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00029DB4 File Offset: 0x00027FB4
		protected flanne.UI.Panel gunEvoPanel
		{
			get
			{
				return this.owner.gunEvoPanel;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00029DC1 File Offset: 0x00027FC1
		protected flanne.UI.Menu gunEvoMenu
		{
			get
			{
				return this.owner.gunEvoMenu;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00029DCE File Offset: 0x00027FCE
		protected flanne.UI.Panel haloUIPanel
		{
			get
			{
				return this.owner.haloUIPanel;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00029DDB File Offset: 0x00027FDB
		protected PowerupDescription haloUI
		{
			get
			{
				return this.owner.haloUI;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00029DE8 File Offset: 0x00027FE8
		protected Button takeHaloButton
		{
			get
			{
				return this.owner.takeHaloButton;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00029DF5 File Offset: 0x00027FF5
		protected flanne.UI.Panel synergiesUIPanel
		{
			get
			{
				return this.owner.synergiesUIPanel;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00029E02 File Offset: 0x00028002
		protected Button synergiesUIBackButton
		{
			get
			{
				return this.owner.synergiesUIBackButton;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00029E0F File Offset: 0x0002800F
		protected ShootDetector shootDetector
		{
			get
			{
				return this.owner.shootDetector;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00029E1C File Offset: 0x0002801C
		protected HitlessDetector hitlessDetector
		{
			get
			{
				return this.owner.hitlessDetector;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00029E29 File Offset: 0x00028029
		protected flanne.UI.Panel leaderBoardPanel
		{
			get
			{
				return this.owner.leaderBoardPanel;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00029E36 File Offset: 0x00028036
		protected LeaderboardUI leaderboardUI
		{
			get
			{
				return this.owner.leaderboardUI;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00029E43 File Offset: 0x00028043
		protected flanne.UI.Panel hasturUnlockedPanel
		{
			get
			{
				return this.owner.hasturUnlockedPanel;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00029E50 File Offset: 0x00028050
		protected Button hasturUnlockedConfirmButton
		{
			get
			{
				return this.owner.hasturUnlockedConfirmButton;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00029E5D File Offset: 0x0002805D
		protected flanne.UI.Panel ravenUnlockedPanel
		{
			get
			{
				return this.owner.ravenUnlockedPanel;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00029E6A File Offset: 0x0002806A
		protected Button ravenUnlockedConfirmButton
		{
			get
			{
				return this.owner.ravenUnlockedConfirmButton;
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00029E77 File Offset: 0x00028077
		private void Awake()
		{
			this.owner = base.GetComponentInParent<GameController>();
		}

		// Token: 0x040007E1 RID: 2017
		protected GameController owner;
	}
}
