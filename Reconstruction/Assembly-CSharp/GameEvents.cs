using System;

// Token: 0x02000114 RID: 276
public class GameEvents : Singleton<GameEvents>
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060006C4 RID: 1732 RVA: 0x000128BC File Offset: 0x00010ABC
	// (remove) Token: 0x060006C5 RID: 1733 RVA: 0x000128F4 File Offset: 0x00010AF4
	public event Action onGuideObjCollect;

	// Token: 0x060006C6 RID: 1734 RVA: 0x00012929 File Offset: 0x00010B29
	public void GuideObjCollect()
	{
		Action action = this.onGuideObjCollect;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060006C7 RID: 1735 RVA: 0x0001293C File Offset: 0x00010B3C
	// (remove) Token: 0x060006C8 RID: 1736 RVA: 0x00012974 File Offset: 0x00010B74
	public event Action<TutorialType> onTutorialTrigger;

	// Token: 0x060006C9 RID: 1737 RVA: 0x000129A9 File Offset: 0x00010BA9
	public void TutorialTrigger(TutorialType tutorialType)
	{
		Action<TutorialType> action = this.onTutorialTrigger;
		if (action == null)
		{
			return;
		}
		action(tutorialType);
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060006CA RID: 1738 RVA: 0x000129BC File Offset: 0x00010BBC
	// (remove) Token: 0x060006CB RID: 1739 RVA: 0x000129F4 File Offset: 0x00010BF4
	public event Action<TempWord> onTempWord;

	// Token: 0x060006CC RID: 1740 RVA: 0x00012A29 File Offset: 0x00010C29
	public void TempWordTrigger(TempWord word)
	{
		Action<TempWord> action = this.onTempWord;
		if (action == null)
		{
			return;
		}
		action(word);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x060006CD RID: 1741 RVA: 0x00012A3C File Offset: 0x00010C3C
	// (remove) Token: 0x060006CE RID: 1742 RVA: 0x00012A74 File Offset: 0x00010C74
	public event Action onSeekPath;

	// Token: 0x060006CF RID: 1743 RVA: 0x00012AA9 File Offset: 0x00010CA9
	public void SeekPath()
	{
		Action action = this.onSeekPath;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060006D0 RID: 1744 RVA: 0x00012ABC File Offset: 0x00010CBC
	// (remove) Token: 0x060006D1 RID: 1745 RVA: 0x00012AF4 File Offset: 0x00010CF4
	public event Action onTileClick;

	// Token: 0x060006D2 RID: 1746 RVA: 0x00012B29 File Offset: 0x00010D29
	public void TileClick()
	{
		Action action = this.onTileClick;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060006D3 RID: 1747 RVA: 0x00012B3C File Offset: 0x00010D3C
	// (remove) Token: 0x060006D4 RID: 1748 RVA: 0x00012B74 File Offset: 0x00010D74
	public event Action<TileBase> onTileUp;

	// Token: 0x060006D5 RID: 1749 RVA: 0x00012BA9 File Offset: 0x00010DA9
	public void TileUp(TileBase tile)
	{
		Action<TileBase> action = this.onTileUp;
		if (action == null)
		{
			return;
		}
		action(tile);
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060006D6 RID: 1750 RVA: 0x00012BBC File Offset: 0x00010DBC
	// (remove) Token: 0x060006D7 RID: 1751 RVA: 0x00012BF4 File Offset: 0x00010DF4
	public event Action<Enemy> onEnemyReach;

	// Token: 0x060006D8 RID: 1752 RVA: 0x00012C29 File Offset: 0x00010E29
	public void EnemyReach(Enemy enemy)
	{
		Action<Enemy> action = this.onEnemyReach;
		if (action == null)
		{
			return;
		}
		action(enemy);
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060006D9 RID: 1753 RVA: 0x00012C3C File Offset: 0x00010E3C
	// (remove) Token: 0x060006DA RID: 1754 RVA: 0x00012C74 File Offset: 0x00010E74
	public event Action<Enemy> onEnemyDie;

	// Token: 0x060006DB RID: 1755 RVA: 0x00012CA9 File Offset: 0x00010EA9
	public void EnemyDie(Enemy enemy)
	{
		Action<Enemy> action = this.onEnemyDie;
		if (action == null)
		{
			return;
		}
		action(enemy);
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060006DC RID: 1756 RVA: 0x00012CBC File Offset: 0x00010EBC
	// (remove) Token: 0x060006DD RID: 1757 RVA: 0x00012CF4 File Offset: 0x00010EF4
	public event Action<bool> onShowDamageIntensify;

	// Token: 0x060006DE RID: 1758 RVA: 0x00012D29 File Offset: 0x00010F29
	public void ShowDamageIntensify(bool value)
	{
		Action<bool> action = this.onShowDamageIntensify;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060006DF RID: 1759 RVA: 0x00012D3C File Offset: 0x00010F3C
	// (remove) Token: 0x060006E0 RID: 1760 RVA: 0x00012D74 File Offset: 0x00010F74
	public event Action<bool> onEndlessLeaderBoardGet;

	// Token: 0x060006E1 RID: 1761 RVA: 0x00012DA9 File Offset: 0x00010FA9
	public void EndlessLeaderboardGet(bool value)
	{
		Action<bool> action = this.onEndlessLeaderBoardGet;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060006E2 RID: 1762 RVA: 0x00012DBC File Offset: 0x00010FBC
	// (remove) Token: 0x060006E3 RID: 1763 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public event Action<bool> onChallengeLeaderBoardGet;

	// Token: 0x060006E4 RID: 1764 RVA: 0x00012E29 File Offset: 0x00011029
	public void ChallengeLeaderboardGet(bool value)
	{
		Action<bool> action = this.onChallengeLeaderBoardGet;
		if (action == null)
		{
			return;
		}
		action(value);
	}
}
