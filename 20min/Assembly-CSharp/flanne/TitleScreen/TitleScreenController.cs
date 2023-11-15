using System;
using flanne.UI;
using flanne.UIExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.TitleScreen
{
	// Token: 0x020001EA RID: 490
	public class TitleScreenController : StateMachine
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x00029819 File Offset: 0x00027A19
		private void Awake()
		{
			LeanTween.init(500000);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00029825 File Offset: 0x00027A25
		private void Start()
		{
			this.ChangeState<InitState>();
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00029830 File Offset: 0x00027A30
		public void Save()
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

		// Token: 0x04000785 RID: 1925
		public InputActionAsset input;

		// Token: 0x04000786 RID: 1926
		public flanne.UIExtensions.Panel leavesPanel;

		// Token: 0x04000787 RID: 1927
		public flanne.UIExtensions.Panel logoPanel;

		// Token: 0x04000788 RID: 1928
		public flanne.UIExtensions.Panel selectPanel;

		// Token: 0x04000789 RID: 1929
		public flanne.UIExtensions.Panel runeMenuPanel;

		// Token: 0x0400078A RID: 1930
		public flanne.UIExtensions.Panel modeSelectPanel;

		// Token: 0x0400078B RID: 1931
		public flanne.UIExtensions.Panel mapSelectPanel;

		// Token: 0x0400078C RID: 1932
		public flanne.UIExtensions.Panel emberpathPanel;

		// Token: 0x0400078D RID: 1933
		public flanne.UIExtensions.Menu mainMenu;

		// Token: 0x0400078E RID: 1934
		public flanne.UIExtensions.Menu languageMenu;

		// Token: 0x0400078F RID: 1935
		public flanne.UIExtensions.Menu optionsMenu;

		// Token: 0x04000790 RID: 1936
		public CharacterMenu characterMenu;

		// Token: 0x04000791 RID: 1937
		public GunMenu gunMenu;

		// Token: 0x04000792 RID: 1938
		public Button runesButton;

		// Token: 0x04000793 RID: 1939
		public Button runeConfirmButton;

		// Token: 0x04000794 RID: 1940
		public Animator eyes;

		// Token: 0x04000795 RID: 1941
		public Image screenCover;

		// Token: 0x04000796 RID: 1942
		public Image checkRunesPromptArrow;

		// Token: 0x04000797 RID: 1943
		public GameModeMenu modeSelectMenu;

		// Token: 0x04000798 RID: 1944
		public GameModeMenu mapSelectMenu;

		// Token: 0x04000799 RID: 1945
		public AudioClip titleScreenMusic;

		// Token: 0x0400079A RID: 1946
		public UnlockablesManager characterUnlocker;

		// Token: 0x0400079B RID: 1947
		public UnlockablesManager gunUnlocker;

		// Token: 0x0400079C RID: 1948
		public TieredUnlockManager runeUnlocker;

		// Token: 0x0400079D RID: 1949
		public VictoryAchievedUI characterVictories;

		// Token: 0x0400079E RID: 1950
		public VictoryAchievedUI gunVictories;

		// Token: 0x0400079F RID: 1951
		public DifficultyController difficultyController;

		// Token: 0x040007A0 RID: 1952
		public UnlockAtDarkness templeUnlocker;

		// Token: 0x040007A1 RID: 1953
		public UnlockAtDarkness pumpkinPatchUnlocker;

		// Token: 0x040007A2 RID: 1954
		public RuneTreeUI swordRuneTree;

		// Token: 0x040007A3 RID: 1955
		public RuneTreeUI shieldRuneTree;

		// Token: 0x040007A4 RID: 1956
		public Button loadoutBackButton;

		// Token: 0x040007A5 RID: 1957
		public Button modeSelectStartButton;

		// Token: 0x040007A6 RID: 1958
		public Button modeSelectBackButton;

		// Token: 0x040007A7 RID: 1959
		public Button mapSelectStartButton;

		// Token: 0x040007A8 RID: 1960
		public Button mapSelectBackButton;
	}
}
