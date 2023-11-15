using System;
using flanne.UI;
using flanne.UIExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.TitleScreen
{
	// Token: 0x020001EB RID: 491
	public abstract class TitleScreenState : State
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000298EC File Offset: 0x00027AEC
		protected InputActionAsset input
		{
			get
			{
				return this.owner.input;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000298F9 File Offset: 0x00027AF9
		protected flanne.UIExtensions.Panel leavesPanel
		{
			get
			{
				return this.owner.leavesPanel;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00029906 File Offset: 0x00027B06
		protected flanne.UIExtensions.Panel logoPanel
		{
			get
			{
				return this.owner.logoPanel;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00029913 File Offset: 0x00027B13
		protected flanne.UIExtensions.Panel selectPanel
		{
			get
			{
				return this.owner.selectPanel;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00029920 File Offset: 0x00027B20
		protected flanne.UIExtensions.Panel runeMenuPanel
		{
			get
			{
				return this.owner.runeMenuPanel;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0002992D File Offset: 0x00027B2D
		protected flanne.UIExtensions.Panel modeSelectPanel
		{
			get
			{
				return this.owner.modeSelectPanel;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0002993A File Offset: 0x00027B3A
		protected flanne.UIExtensions.Panel mapSelectPanel
		{
			get
			{
				return this.owner.mapSelectPanel;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00029947 File Offset: 0x00027B47
		protected flanne.UIExtensions.Panel emberpathPanel
		{
			get
			{
				return this.owner.emberpathPanel;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00029954 File Offset: 0x00027B54
		protected flanne.UIExtensions.Menu mainMenu
		{
			get
			{
				return this.owner.mainMenu;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00029961 File Offset: 0x00027B61
		protected flanne.UIExtensions.Menu languageMenu
		{
			get
			{
				return this.owner.languageMenu;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0002996E File Offset: 0x00027B6E
		protected flanne.UIExtensions.Menu optionsMenu
		{
			get
			{
				return this.owner.optionsMenu;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0002997B File Offset: 0x00027B7B
		protected CharacterMenu characterMenu
		{
			get
			{
				return this.owner.characterMenu;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00029988 File Offset: 0x00027B88
		protected GunMenu gunMenu
		{
			get
			{
				return this.owner.gunMenu;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00029995 File Offset: 0x00027B95
		protected Animator eyes
		{
			get
			{
				return this.owner.eyes;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x000299A2 File Offset: 0x00027BA2
		protected Image screenCover
		{
			get
			{
				return this.owner.screenCover;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x000299AF File Offset: 0x00027BAF
		protected Image checkRunesPromptArrow
		{
			get
			{
				return this.owner.checkRunesPromptArrow;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x000299BC File Offset: 0x00027BBC
		protected GameModeMenu modeSelectMenu
		{
			get
			{
				return this.owner.modeSelectMenu;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x000299C9 File Offset: 0x00027BC9
		protected GameModeMenu mapSelectMenu
		{
			get
			{
				return this.owner.mapSelectMenu;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x000299D6 File Offset: 0x00027BD6
		protected AudioClip titleScreenMusic
		{
			get
			{
				return this.owner.titleScreenMusic;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x000299E3 File Offset: 0x00027BE3
		protected UnlockablesManager characterUnlocker
		{
			get
			{
				return this.owner.characterUnlocker;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000299F0 File Offset: 0x00027BF0
		protected UnlockablesManager gunUnlocker
		{
			get
			{
				return this.owner.gunUnlocker;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000299FD File Offset: 0x00027BFD
		protected TieredUnlockManager runeUnlocker
		{
			get
			{
				return this.owner.runeUnlocker;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00029A0A File Offset: 0x00027C0A
		protected VictoryAchievedUI characterVictories
		{
			get
			{
				return this.owner.characterVictories;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00029A17 File Offset: 0x00027C17
		protected VictoryAchievedUI gunVictories
		{
			get
			{
				return this.owner.gunVictories;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00029A24 File Offset: 0x00027C24
		protected DifficultyController difficultyController
		{
			get
			{
				return this.owner.difficultyController;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00029A31 File Offset: 0x00027C31
		protected UnlockAtDarkness templeUnlocker
		{
			get
			{
				return this.owner.templeUnlocker;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00029A3E File Offset: 0x00027C3E
		protected UnlockAtDarkness pumpkinPatchUnlocker
		{
			get
			{
				return this.owner.pumpkinPatchUnlocker;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00029A4B File Offset: 0x00027C4B
		protected RuneTreeUI swordRuneTree
		{
			get
			{
				return this.owner.swordRuneTree;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00029A58 File Offset: 0x00027C58
		protected RuneTreeUI shieldRuneTree
		{
			get
			{
				return this.owner.shieldRuneTree;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00029A65 File Offset: 0x00027C65
		protected Button loadoutBackButton
		{
			get
			{
				return this.owner.loadoutBackButton;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00029A72 File Offset: 0x00027C72
		protected Button runesButton
		{
			get
			{
				return this.owner.runesButton;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00029A7F File Offset: 0x00027C7F
		protected Button runeConfirmButton
		{
			get
			{
				return this.owner.runeConfirmButton;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00029A8C File Offset: 0x00027C8C
		protected Button modeSelectStartButton
		{
			get
			{
				return this.owner.modeSelectStartButton;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00029A99 File Offset: 0x00027C99
		protected Button modeSelectBackButton
		{
			get
			{
				return this.owner.modeSelectBackButton;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00029AA6 File Offset: 0x00027CA6
		protected Button mapSelectStartButton
		{
			get
			{
				return this.owner.mapSelectStartButton;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00029AB3 File Offset: 0x00027CB3
		protected Button mapSelectBackButton
		{
			get
			{
				return this.owner.mapSelectBackButton;
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00029AC0 File Offset: 0x00027CC0
		private void Awake()
		{
			this.owner = base.GetComponentInParent<TitleScreenController>();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00029AD0 File Offset: 0x00027CD0
		protected void Save()
		{
			SaveSystem.data.points = PointsTracker.pts;
			SaveSystem.data.characterUnlocks = this.characterUnlocker.unlockData;
			SaveSystem.data.gunUnlocks = this.gunUnlocker.unlockData;
			SaveSystem.data.runeUnlocks = this.runeUnlocker.unlockData;
			SaveSystem.data.characterHighestClear = this.characterVictories.victories;
			SaveSystem.data.gunHighestClear = this.gunVictories.victories;
			SaveSystem.data.swordRuneSelections = this.swordRuneTree.GetSelections();
			SaveSystem.data.shieldRuneSelections = this.shieldRuneTree.GetSelections();
			SaveSystem.Save();
		}

		// Token: 0x040007A9 RID: 1961
		protected TitleScreenController owner;
	}
}
