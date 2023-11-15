using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public static class GameRes
{
	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x00012F70 File Offset: 0x00011170
	public static GameResStruct SaveRes
	{
		get
		{
			return GameRes.SetSaveRes();
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x00012F77 File Offset: 0x00011177
	// (set) Token: 0x060006EE RID: 1774 RVA: 0x00012F7E File Offset: 0x0001117E
	public static int MaxPath
	{
		get
		{
			return GameRes.maxPath;
		}
		set
		{
			GameRes.maxPath = ((value > GameRes.maxPath) ? value : GameRes.maxPath);
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x00012F95 File Offset: 0x00011195
	// (set) Token: 0x060006F0 RID: 1776 RVA: 0x00012FAC File Offset: 0x000111AC
	public static float TurnSpeedAdjust
	{
		get
		{
			return GameRes.turnSpeedAdjust + 0.3f * (float)(GameRes.CurrentWave / 30);
		}
		set
		{
			GameRes.turnSpeedAdjust = value;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00012FB4 File Offset: 0x000111B4
	// (set) Token: 0x060006F2 RID: 1778 RVA: 0x00012FBB File Offset: 0x000111BB
	public static int GameSpeed
	{
		get
		{
			return GameRes.gameSpeed;
		}
		set
		{
			if (value > 3)
			{
				GameRes.gameSpeed = 1;
			}
			else
			{
				GameRes.gameSpeed = value;
			}
			Time.timeScale = (float)GameRes.gameSpeed;
			GameRes.m_MainUI.GameSpeed = GameRes.gameSpeed;
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00012FE9 File Offset: 0x000111E9
	// (set) Token: 0x060006F4 RID: 1780 RVA: 0x00012FF0 File Offset: 0x000111F0
	public static ShapeInfo[] PreSetShape
	{
		get
		{
			return GameRes.preSetShape;
		}
		set
		{
			GameRes.preSetShape = value;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00012FF8 File Offset: 0x000111F8
	// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00012FFF File Offset: 0x000111FF
	public static ForcePlace ForcePlace
	{
		get
		{
			return GameRes.forcePlace;
		}
		set
		{
			GameRes.forcePlace = value;
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00013007 File Offset: 0x00011207
	// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0001300E File Offset: 0x0001120E
	public static int PerfectElementCount
	{
		get
		{
			return GameRes.perfectElementCount;
		}
		set
		{
			GameRes.perfectElementCount = value;
			GameRes.m_BluePrintShop.PerfectElementCount = GameRes.perfectElementCount;
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00013025 File Offset: 0x00011225
	// (set) Token: 0x060006FA RID: 1786 RVA: 0x0001302C File Offset: 0x0001122C
	public static int NextRefreshTurn
	{
		get
		{
			return GameRes.nextRefreshTurn;
		}
		set
		{
			if (value <= 0)
			{
				GameRes.nextRefreshTurn = GameRes.AutoRefreshInterval;
				Singleton<GameManager>.Instance.RefreshShop(0);
			}
			else
			{
				GameRes.nextRefreshTurn = value;
			}
			GameRes.m_BluePrintShop.NextRefreshTrun = GameRes.nextRefreshTurn;
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001305E File Offset: 0x0001125E
	// (set) Token: 0x060006FC RID: 1788 RVA: 0x00013065 File Offset: 0x00011265
	public static bool DrawThisTurn
	{
		get
		{
			return GameRes.drawThisTurn;
		}
		set
		{
			GameRes.drawThisTurn = value;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001306D File Offset: 0x0001126D
	// (set) Token: 0x060006FE RID: 1790 RVA: 0x00013074 File Offset: 0x00011274
	public static int Coin
	{
		get
		{
			return GameRes.coin;
		}
		set
		{
			GameRes.coin = Mathf.Max(0, value);
			GameRes.m_MainUI.Coin = GameRes.coin;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x00013091 File Offset: 0x00011291
	// (set) Token: 0x06000700 RID: 1792 RVA: 0x00013098 File Offset: 0x00011298
	public static int Life
	{
		get
		{
			return GameRes.life;
		}
		set
		{
			if (value <= 0)
			{
				if (GameRes.life <= 0 || GameRes.WontDieThisTurn)
				{
					return;
				}
				if (GameRes.DieProtect > 0)
				{
					GameRes.DieProtect--;
					GameRes.WontDieThisTurn = true;
					GameRes.life = 1;
					GameRes.m_MainUI.Life = GameRes.life;
					GameRes.EnemyRemain++;
					Singleton<GameManager>.Instance.DieProtect();
					return;
				}
				Singleton<GameManager>.Instance.GameEnd(false);
			}
			GameRes.life = Mathf.Clamp(value, 0, Singleton<LevelManager>.Instance.CurrentLevel.PlayerHealth);
			GameRes.m_MainUI.Life = GameRes.life;
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x00013134 File Offset: 0x00011334
	// (set) Token: 0x06000702 RID: 1794 RVA: 0x0001313B File Offset: 0x0001133B
	public static int EnemyRemain
	{
		get
		{
			return GameRes.enemyRemain;
		}
		set
		{
			GameRes.enemyRemain = value;
			if (GameRes.enemyRemain <= 0 && !GameRes.m_WaveSystem.RunningSpawn)
			{
				GameRes.enemyRemain = 0;
				Singleton<GameManager>.Instance.PrepareNextWave();
			}
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x00013167 File Offset: 0x00011367
	// (set) Token: 0x06000704 RID: 1796 RVA: 0x0001316E File Offset: 0x0001136E
	public static int CurrentWave
	{
		get
		{
			return GameRes.currentWave;
		}
		set
		{
			GameRes.currentWave = value;
			GameRes.m_MainUI.CurrentWave = GameRes.currentWave;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000705 RID: 1797 RVA: 0x00013185 File Offset: 0x00011385
	// (set) Token: 0x06000706 RID: 1798 RVA: 0x0001318C File Offset: 0x0001138C
	public static int BuildCost
	{
		get
		{
			return GameRes.buildCost;
		}
		set
		{
			GameRes.buildCost = Mathf.Max(0, value);
			GameRes.m_FuncUI.BuyShapeCost = GameRes.buildCost;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x000131A9 File Offset: 0x000113A9
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x000131B0 File Offset: 0x000113B0
	public static int RefreshShopCost
	{
		get
		{
			return GameRes.refreshShopCost;
		}
		set
		{
			GameRes.refreshShopCost = value;
			GameRes.m_BluePrintShop.RefreshShopCost = GameRes.refreshShopCost;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x000131C7 File Offset: 0x000113C7
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x000131EA File Offset: 0x000113EA
	public static int SwitchTrapCost
	{
		get
		{
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Challenge)
			{
				return Mathf.Min(120, GameRes.switchTrapCost);
			}
			return 30;
		}
		set
		{
			GameRes.switchTrapCost = value;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x0600070B RID: 1803 RVA: 0x000131F2 File Offset: 0x000113F2
	// (set) Token: 0x0600070C RID: 1804 RVA: 0x000131F9 File Offset: 0x000113F9
	public static int SwitchTurretCost
	{
		get
		{
			return GameRes.switchTurretCost;
		}
		set
		{
			GameRes.switchTurretCost = value;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x0600070D RID: 1805 RVA: 0x00013201 File Offset: 0x00011401
	// (set) Token: 0x0600070E RID: 1806 RVA: 0x00013208 File Offset: 0x00011408
	public static int SystemLevel
	{
		get
		{
			return GameRes.systemLevel;
		}
		set
		{
			GameRes.systemLevel = Mathf.Clamp(value, 1, 6);
			GameRes.SystemUpgradeCost = Singleton<StaticData>.Instance.LevelUpMoney[GameRes.systemLevel];
			GameRes.m_FuncUI.SystemLevel = GameRes.systemLevel;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001323B File Offset: 0x0001143B
	// (set) Token: 0x06000710 RID: 1808 RVA: 0x00013242 File Offset: 0x00011442
	public static int SystemUpgradeCost
	{
		get
		{
			return GameRes.systemUpgradeCost;
		}
		set
		{
			GameRes.systemUpgradeCost = value;
			GameRes.m_FuncUI.SystemUpgradeCost = GameRes.systemUpgradeCost;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000711 RID: 1809 RVA: 0x00013259 File Offset: 0x00011459
	// (set) Token: 0x06000712 RID: 1810 RVA: 0x00013260 File Offset: 0x00011460
	public static float BuildDiscount
	{
		get
		{
			return GameRes.discountRate;
		}
		set
		{
			GameRes.discountRate = Mathf.Min(0.5f, value);
			GameRes.m_FuncUI.DiscountRate = GameRes.discountRate;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x00013281 File Offset: 0x00011481
	// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001328E File Offset: 0x0001148E
	public static int ShopCapacity
	{
		get
		{
			return Mathf.Min(6, GameRes.shopCapacity);
		}
		set
		{
			GameRes.shopCapacity = value;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000715 RID: 1813 RVA: 0x00013296 File Offset: 0x00011496
	// (set) Token: 0x06000716 RID: 1814 RVA: 0x000132A3 File Offset: 0x000114A3
	public static int LockCount
	{
		get
		{
			return Mathf.Max(0, GameRes.lockCount);
		}
		set
		{
			GameRes.lockCount = value;
			GameRes.m_BluePrintShop.ShowAllLock(GameRes.lockCount);
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000717 RID: 1815 RVA: 0x000132BA File Offset: 0x000114BA
	// (set) Token: 0x06000718 RID: 1816 RVA: 0x000132DD File Offset: 0x000114DD
	public static int BuyGroundCost
	{
		get
		{
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Challenge)
			{
				return Mathf.Min(100, GameRes.buyGroundCost);
			}
			return 50;
		}
		set
		{
			GameRes.buyGroundCost = value;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x000132E5 File Offset: 0x000114E5
	// (set) Token: 0x0600071A RID: 1818 RVA: 0x000132EC File Offset: 0x000114EC
	public static float AbnormalRate
	{
		get
		{
			return GameRes.abnormalRate;
		}
		set
		{
			GameRes.abnormalRate = Mathf.Clamp(value, 0f, 1f);
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x0600071B RID: 1819 RVA: 0x00013303 File Offset: 0x00011503
	// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001330A File Offset: 0x0001150A
	public static int SkillChip
	{
		get
		{
			return GameRes.skillChip;
		}
		set
		{
			GameRes.skillChip = value;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x00013312 File Offset: 0x00011512
	// (set) Token: 0x0600071E RID: 1822 RVA: 0x00013319 File Offset: 0x00011519
	public static int RefreashTechCost { get; set; }

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x0600071F RID: 1823 RVA: 0x00013321 File Offset: 0x00011521
	// (set) Token: 0x06000720 RID: 1824 RVA: 0x00013328 File Offset: 0x00011528
	public static int FreeRefreshTech { get; set; }

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000721 RID: 1825 RVA: 0x00013330 File Offset: 0x00011530
	// (set) Token: 0x06000722 RID: 1826 RVA: 0x00013337 File Offset: 0x00011537
	public static bool Reverse { get; set; }

	// Token: 0x06000723 RID: 1827 RVA: 0x00013340 File Offset: 0x00011540
	public static void Initialize(MainUI mainUI, FuncUI funcUI, WaveSystem waveSystem, BluePrintShopUI bluePrintShop)
	{
		GameRes.m_MainUI = mainUI;
		GameRes.m_FuncUI = funcUI;
		GameRes.m_WaveSystem = waveSystem;
		GameRes.m_BluePrintShop = bluePrintShop;
		GameRes.Reverse = false;
		GameRes.GroundSize = ((Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge) ? 15 : 25);
		GameRes.TrapDistanceAdjust = 0;
		GameRes.EnemyFrostResist = 0f;
		GameRes.TurretFrostResist = 0f;
		GameRes.EnemyAmoundAdjust = 1f;
		GameRes.EnemyIntensifyAdjust = 1f;
		GameRes.TurnSpeedAdjust = 1f;
		GameRes.DamageAdjust = 1f;
		GameRes.CoinAdjust = 1f;
		GameRes.DieProtect = 1;
		GameRes.DrawThisTurn = true;
		GameRes.TotalRefactor = 0;
		GameRes.TotalDamage = 0L;
		GameRes.NextRefreshTurn = 4;
		GameRes.BuildDiscount = 0.1f;
		GameRes.ShopCapacity = 3;
		GameRes.SystemLevel = 1;
		GameRes.CurrentWave = 0;
		GameRes.SwitchTrapCost = Singleton<StaticData>.Instance.SwitchTrapCost;
		GameRes.SwitchTurretCost = Singleton<StaticData>.Instance.SwitchTurretCost;
		GameRes.Coin = Singleton<LevelManager>.Instance.CurrentLevel.StartCoin;
		GameRes.Life = Singleton<LevelManager>.Instance.CurrentLevel.PlayerHealth;
		GameRes.BuildCost = Singleton<StaticData>.Instance.BaseShapeCost;
		GameRes.RefreshShopCost = Singleton<StaticData>.Instance.ShopRefreshCost;
		GameRes.BuyGroundCost = Singleton<StaticData>.Instance.BuyGroundCost;
		GameRes.AutoRefreshInterval = 3;
		GameRes.PerfectElementCount = 0;
		GameRes.LevelStart = DateTime.Now;
		GameRes.MaxPath = 0;
		GameRes.MaxMark = 0;
		GameRes.GainGold = 0;
		GameRes.enemyRemain = 0;
		GameRes.SkipTimes = 0;
		GameRes.LockCount = 1;
		GameRes.FreeGroundTileCount = 0;
		GameRes.GameGoldIntensify = Singleton<StaticData>.Instance.GoldAttackIntensify;
		GameRes.GameWoodIntensify = Singleton<StaticData>.Instance.WoodFirerateIntensify;
		GameRes.GameWaterIntensify = Singleton<StaticData>.Instance.WaterSlowIntensify;
		GameRes.GameFireIntensify = Singleton<StaticData>.Instance.FireCritIntensify;
		GameRes.GameDustIntensify = Singleton<StaticData>.Instance.DustSplashIntensify;
		GameRes.EnemyFrostTime = 3f;
		GameRes.TotalLandedRefactor = 0;
		GameRes.AbnormalRate = 0.4f;
		GameRes.RefreashTechCost = 100;
		GameRes.FreeRefreshTech = 1;
		GameRes.GainGoldBattleTurn = 0;
		GameRes.GainPerfectBattleTurn = 0;
		GameRes.LostLifeBattleTurn = 0;
		GameRes.PreSetShape = new ShapeInfo[3];
		GameRes.ForcePlace = null;
		GameRes.SkillChip = 0;
		GameRes.SkillChipInterval = 20;
		GameRes.IntentLineID = 0;
		GameRes.ChallengeChoicePicked = false;
		GameRes.TurretUpgradeDiscount = 0f;
		Hamster.isFirstHamster = false;
		StrategyBase.CoordinatorMaxIntensify = 1f;
		StrategyBase.MaxElementCount = 99;
		GameRes.TrapItensify = 0f;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0001359C File Offset: 0x0001179C
	private static GameResStruct SetSaveRes()
	{
		return new GameResStruct
		{
			Mode = Singleton<LevelManager>.Instance.CurrentLevel.ModeID,
			Coin = GameRes.Coin - GameRes.GainGoldBattleTurn,
			Wave = GameRes.CurrentWave,
			CurrentLife = GameRes.Life + GameRes.LostLifeBattleTurn,
			MaxLife = Singleton<LevelManager>.Instance.CurrentLevel.PlayerHealth,
			BuildCost = GameRes.BuildCost,
			SwitchTrapCost = GameRes.SwitchTrapCost,
			SystemLevel = GameRes.SystemLevel,
			SystemUpgradeCost = GameRes.systemUpgradeCost,
			TotalDamage = GameRes.TotalDamage,
			TotalRefactor = GameRes.TotalRefactor,
			ShopCapacity = GameRes.ShopCapacity,
			NextRefreshTurn = GameRes.NextRefreshTurn,
			PefectElementCount = GameRes.PerfectElementCount - GameRes.GainPerfectBattleTurn,
			DrawThisTurn = GameRes.DrawThisTurn,
			DieProtect = GameRes.DieProtect,
			BuyGroundCost = GameRes.BuyGroundCost,
			GainGold = GameRes.GainGold,
			StartTime = GameRes.LevelStart,
			RefreashTechCost = GameRes.RefreashTechCost,
			SkillChip = GameRes.SkillChip,
			FreeRefreshTech = GameRes.FreeRefreshTech,
			SkipTimes = GameRes.SkipTimes
		};
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x000136D4 File Offset: 0x000118D4
	public static void LoadSaveRes()
	{
		GameResStruct saveRes = Singleton<LevelManager>.Instance.LastGameSave.SaveRes;
		GameRes.Coin = saveRes.Coin;
		GameRes.CurrentWave = saveRes.Wave;
		GameRes.Life = saveRes.CurrentLife;
		GameRes.BuildCost = saveRes.BuildCost;
		GameRes.SwitchTrapCost = saveRes.SwitchTrapCost;
		GameRes.SystemLevel = saveRes.SystemLevel;
		GameRes.SystemUpgradeCost = saveRes.SystemUpgradeCost;
		GameRes.TotalRefactor = saveRes.TotalRefactor;
		GameRes.ShopCapacity = saveRes.ShopCapacity;
		GameRes.NextRefreshTurn = saveRes.NextRefreshTurn;
		GameRes.PerfectElementCount = saveRes.PefectElementCount;
		GameRes.DrawThisTurn = saveRes.DrawThisTurn;
		GameRes.DieProtect = saveRes.DieProtect;
		GameRes.BuyGroundCost = saveRes.BuyGroundCost;
		GameRes.GainGold = saveRes.GainGold;
		GameRes.TotalDamage = saveRes.TotalDamage;
		GameRes.LevelStart = saveRes.StartTime;
		GameRes.RefreashTechCost = saveRes.RefreashTechCost;
		GameRes.FreeRefreshTech = saveRes.FreeRefreshTech;
		GameRes.SkillChip = saveRes.SkillChip;
		GameRes.SkipTimes = saveRes.SkipTimes;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x000137D8 File Offset: 0x000119D8
	public static bool CheckForcePlacement(Vector2 pos, Vector2 dir)
	{
		return GameRes.ForcePlace == null || (Vector2.SqrMagnitude(pos - GameRes.ForcePlace.ForcePos) < 0.1f && (GameRes.ForcePlace.ForceDir == Vector2.zero || Vector2.Dot(dir, GameRes.ForcePlace.ForceDir) > 0.99f));
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0001383C File Offset: 0x00011A3C
	public static void PrepareNextWave()
	{
		GameRes.CurrentWave++;
		GameRes.GainGoldBattleTurn = 0;
		GameRes.GainPerfectBattleTurn = 0;
		GameRes.LostLifeBattleTurn = 0;
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Endless && GameRes.CurrentWave % GameRes.SkillChipInterval == 0)
		{
			GameRes.GainSkillChip(1);
		}
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Challenge)
		{
			GameRes.NextRefreshTurn--;
			GameRes.DrawThisTurn = false;
		}
		GameRes.WontDieThisTurn = false;
		Singleton<GameManager>.Instance.GainMoney(Mathf.Min(300, Singleton<StaticData>.Instance.BaseWaveIncome + Singleton<StaticData>.Instance.WaveMultiplyIncome * (GameRes.CurrentWave - 1)));
		GameRes.BuildCost = Mathf.RoundToInt((float)GameRes.BuildCost * (1f - GameRes.BuildDiscount));
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00013903 File Offset: 0x00011B03
	public static void GainSkillChip(int amount)
	{
		GameRes.SkillChip += amount;
		Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("GAINCHIP"));
	}

	// Token: 0x0400034C RID: 844
	private static MainUI m_MainUI;

	// Token: 0x0400034D RID: 845
	private static FuncUI m_FuncUI;

	// Token: 0x0400034E RID: 846
	private static BluePrintShopUI m_BluePrintShop;

	// Token: 0x0400034F RID: 847
	private static WaveSystem m_WaveSystem;

	// Token: 0x04000350 RID: 848
	[Header("动态数据")]
	public static int FreeGroundTileCount = 0;

	// Token: 0x04000351 RID: 849
	public static Action<StrategyBase> NextCompositeCallback = null;

	// Token: 0x04000352 RID: 850
	public static int DieProtect;

	// Token: 0x04000353 RID: 851
	[Header("统计数据")]
	public static DateTime LevelStart;

	// Token: 0x04000354 RID: 852
	public static DateTime LevelEnd;

	// Token: 0x04000355 RID: 853
	public static int TotalRefactor = 0;

	// Token: 0x04000356 RID: 854
	public static long TotalDamage = 0L;

	// Token: 0x04000357 RID: 855
	private static int maxPath = 0;

	// Token: 0x04000358 RID: 856
	public static int MaxMark = 0;

	// Token: 0x04000359 RID: 857
	public static int GainGold = 0;

	// Token: 0x0400035A RID: 858
	public static int SkipTimes = 0;

	// Token: 0x0400035B RID: 859
	[Header("全局动态数据")]
	public static float GameGoldIntensify = 0f;

	// Token: 0x0400035C RID: 860
	public static float GameWoodIntensify = 0f;

	// Token: 0x0400035D RID: 861
	public static float GameWaterIntensify = 0f;

	// Token: 0x0400035E RID: 862
	public static float GameFireIntensify = 0f;

	// Token: 0x0400035F RID: 863
	public static float GameDustIntensify = 0f;

	// Token: 0x04000360 RID: 864
	public static float EnemyFrostTime;

	// Token: 0x04000361 RID: 865
	public static float TurretUpgradeDiscount = 0f;

	// Token: 0x04000362 RID: 866
	public static int SkillChipInterval = 20;

	// Token: 0x04000363 RID: 867
	private static float turnSpeedAdjust = 1f;

	// Token: 0x04000364 RID: 868
	public static float EnemyFrostResist = 0f;

	// Token: 0x04000365 RID: 869
	public static float TurretFrostResist = 0f;

	// Token: 0x04000366 RID: 870
	public static float EnemyAmoundAdjust;

	// Token: 0x04000367 RID: 871
	public static float EnemyIntensifyAdjust;

	// Token: 0x04000368 RID: 872
	public static int TotalLandedRefactor = 0;

	// Token: 0x04000369 RID: 873
	public static int GainGoldBattleTurn = 0;

	// Token: 0x0400036A RID: 874
	public static int GainPerfectBattleTurn = 0;

	// Token: 0x0400036B RID: 875
	public static int LostLifeBattleTurn = 0;

	// Token: 0x0400036C RID: 876
	[Header("场地数据")]
	public static int GroundSize = 25;

	// Token: 0x0400036D RID: 877
	public static int TrapDistanceAdjust = 0;

	// Token: 0x0400036E RID: 878
	public static float TrapItensify = 0f;

	// Token: 0x0400036F RID: 879
	private static int gameSpeed = 1;

	// Token: 0x04000370 RID: 880
	private static ShapeInfo[] preSetShape;

	// Token: 0x04000371 RID: 881
	private static ForcePlace forcePlace;

	// Token: 0x04000372 RID: 882
	private static int perfectElementCount;

	// Token: 0x04000373 RID: 883
	public static int AutoRefreshInterval;

	// Token: 0x04000374 RID: 884
	private static int nextRefreshTurn;

	// Token: 0x04000375 RID: 885
	private static bool drawThisTurn;

	// Token: 0x04000376 RID: 886
	private static int coin;

	// Token: 0x04000377 RID: 887
	private static bool WontDieThisTurn;

	// Token: 0x04000378 RID: 888
	private static int life;

	// Token: 0x04000379 RID: 889
	private static int enemyRemain;

	// Token: 0x0400037A RID: 890
	public static int currentWave;

	// Token: 0x0400037B RID: 891
	private static int buildCost;

	// Token: 0x0400037C RID: 892
	private static int refreshShopCost;

	// Token: 0x0400037D RID: 893
	private static int switchTrapCost;

	// Token: 0x0400037E RID: 894
	private static int switchTurretCost;

	// Token: 0x0400037F RID: 895
	private static int systemLevel = 1;

	// Token: 0x04000380 RID: 896
	private static int systemUpgradeCost;

	// Token: 0x04000381 RID: 897
	private static float discountRate;

	// Token: 0x04000382 RID: 898
	private static int shopCapacity;

	// Token: 0x04000383 RID: 899
	private static int lockCount = 1;

	// Token: 0x04000384 RID: 900
	private static int buyGroundCost;

	// Token: 0x04000385 RID: 901
	private static float abnormalRate;

	// Token: 0x04000386 RID: 902
	private static int skillChip;

	// Token: 0x04000387 RID: 903
	public static int IntentLineID;

	// Token: 0x0400038B RID: 907
	public static SwitchInfo SwitchInfo;

	// Token: 0x0400038C RID: 908
	public static float DamageAdjust;

	// Token: 0x0400038D RID: 909
	public static float CoinAdjust;

	// Token: 0x0400038E RID: 910
	public static bool ChallengeChoicePicked;
}
