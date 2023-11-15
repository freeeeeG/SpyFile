using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class StrategyBase
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x000235E2 File Offset: 0x000217E2
	// (set) Token: 0x06000D98 RID: 3480 RVA: 0x000235EA File Offset: 0x000217EA
	public int Quality
	{
		get
		{
			return this.quality;
		}
		set
		{
			this.quality = value;
		}
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x000235F4 File Offset: 0x000217F4
	public StrategyBase(TurretAttribute attribute, int quality)
	{
		this.Attribute = attribute;
		this.Quality = quality;
		this.RangeType = attribute.RangeType;
		this.SetQualityValue();
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0002369C File Offset: 0x0002189C
	// (set) Token: 0x06000D9B RID: 3483 RVA: 0x000236A4 File Offset: 0x000218A4
	public float InitAttack
	{
		get
		{
			return this.initAttack;
		}
		set
		{
			this.initAttack = value;
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06000D9C RID: 3484 RVA: 0x000236AD File Offset: 0x000218AD
	// (set) Token: 0x06000D9D RID: 3485 RVA: 0x000236B5 File Offset: 0x000218B5
	public float InitFireRate
	{
		get
		{
			return this.initSpeed;
		}
		set
		{
			this.initSpeed = value;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06000D9E RID: 3486 RVA: 0x000236BE File Offset: 0x000218BE
	// (set) Token: 0x06000D9F RID: 3487 RVA: 0x000236C6 File Offset: 0x000218C6
	public int InitRange
	{
		get
		{
			return this.initRange;
		}
		set
		{
			this.initRange = value;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000236CF File Offset: 0x000218CF
	// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x000236D7 File Offset: 0x000218D7
	public float InitCriticalRate
	{
		get
		{
			return this.initCriticalRate;
		}
		set
		{
			this.initCriticalRate = value;
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000236E0 File Offset: 0x000218E0
	// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x000236E8 File Offset: 0x000218E8
	public float InitSplashRange
	{
		get
		{
			return this.initSputteringRange;
		}
		set
		{
			this.initSputteringRange = value;
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000236F1 File Offset: 0x000218F1
	// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000236F9 File Offset: 0x000218F9
	public float InitSlowRate
	{
		get
		{
			return this.initSlowRate;
		}
		set
		{
			this.initSlowRate = value;
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00023702 File Offset: 0x00021902
	// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x0002370A File Offset: 0x0002190A
	public float InitDamageIntensify
	{
		get
		{
			return this.initDamageIntensify;
		}
		set
		{
			this.initDamageIntensify = value;
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00023713 File Offset: 0x00021913
	// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x0002371B File Offset: 0x0002191B
	public long TurnDamage
	{
		get
		{
			return this.turnDamage;
		}
		set
		{
			this.turnDamage = value;
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00023724 File Offset: 0x00021924
	// (set) Token: 0x06000DAB RID: 3499 RVA: 0x0002372C File Offset: 0x0002192C
	public long TotalDamage
	{
		get
		{
			return this.totalDamage;
		}
		set
		{
			this.totalDamage = value;
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00023735 File Offset: 0x00021935
	public int FinalExtraSlot
	{
		get
		{
			return this.PrivateExtraSlot;
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0002373D File Offset: 0x0002193D
	// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00023745 File Offset: 0x00021945
	public bool ContainTurretBuffSkill { get; set; }

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0002374E File Offset: 0x0002194E
	public int ElementSKillSlot
	{
		get
		{
			return Mathf.Min(5, this.InitSkillSLot + this.FinalExtraSlot);
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00023763 File Offset: 0x00021963
	// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x0002376C File Offset: 0x0002196C
	public RangeType RangeType
	{
		get
		{
			return this.rangeType;
		}
		set
		{
			this.rangeType = value;
			RangeType rangeType = this.rangeType;
			if (rangeType <= RangeType.HalfCircle)
			{
				this.CheckAngle = 10f;
				this.RotSpeed = 12f;
				return;
			}
			if (rangeType != RangeType.Line)
			{
				return;
			}
			this.CheckAngle = 45f;
			this.RotSpeed = 0f;
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x000237BD File Offset: 0x000219BD
	// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x000237C5 File Offset: 0x000219C5
	public float BulletSize
	{
		get
		{
			return this.bulletSize;
		}
		set
		{
			this.bulletSize = value;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000237CE File Offset: 0x000219CE
	// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x000237D6 File Offset: 0x000219D6
	public float UpgradeDiscount { get; set; }

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x000237DF File Offset: 0x000219DF
	// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x000237E7 File Offset: 0x000219E7
	public float TimeModify
	{
		get
		{
			return this.timeModify;
		}
		set
		{
			this.timeModify = value;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x000237F0 File Offset: 0x000219F0
	// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x000237F8 File Offset: 0x000219F8
	public int ShootTriggerCount
	{
		get
		{
			return this.shootTriggerCount;
		}
		set
		{
			this.shootTriggerCount = value;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00023801 File Offset: 0x00021A01
	// (set) Token: 0x06000DBB RID: 3515 RVA: 0x00023809 File Offset: 0x00021A09
	public float RotSpeed
	{
		get
		{
			return this.rotSpeed;
		}
		set
		{
			this.rotSpeed = value;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00023812 File Offset: 0x00021A12
	// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0002381A File Offset: 0x00021A1A
	public float CheckAngle
	{
		get
		{
			return this.checkAngle;
		}
		set
		{
			this.checkAngle = value;
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00023823 File Offset: 0x00021A23
	// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0002382B File Offset: 0x00021A2B
	public int ForbidRange
	{
		get
		{
			return this.forbidRange;
		}
		set
		{
			this.forbidRange = value;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00023834 File Offset: 0x00021A34
	// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0002383C File Offset: 0x00021A3C
	public float MaxFireRate
	{
		get
		{
			return this.maxFirerate;
		}
		set
		{
			this.maxFirerate = value;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00023845 File Offset: 0x00021A45
	// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0002384D File Offset: 0x00021A4D
	public float MaxSplash
	{
		get
		{
			return this.maxSplash;
		}
		set
		{
			this.maxSplash = value;
		}
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00023856 File Offset: 0x00021A56
	// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0002385E File Offset: 0x00021A5E
	public int MaxRange
	{
		get
		{
			return this.maxRange;
		}
		set
		{
			this.maxRange = value;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00023867 File Offset: 0x00021A67
	// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x0002386E File Offset: 0x00021A6E
	public static float CoordinatorMaxIntensify
	{
		get
		{
			return StrategyBase.coordinatorMaxIntensify;
		}
		set
		{
			StrategyBase.coordinatorMaxIntensify = value;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00023876 File Offset: 0x00021A76
	// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x0002387D File Offset: 0x00021A7D
	public static float CooporativeAttackIntensify
	{
		get
		{
			return StrategyBase.cooporativeAttackIntensify;
		}
		set
		{
			StrategyBase.cooporativeAttackIntensify = value;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00023885 File Offset: 0x00021A85
	public int TotalBaseCount
	{
		get
		{
			return this.BaseGoldCount + this.BaseWoodCount + this.BaseWaterCount + this.BaseFireCount + this.BaseDustCount;
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06000DCB RID: 3531 RVA: 0x000238A9 File Offset: 0x00021AA9
	public int GoldCount
	{
		get
		{
			return Mathf.Min(StrategyBase.MaxElementCount, this.BaseGoldCount + this.TempGoldCount);
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06000DCC RID: 3532 RVA: 0x000238C2 File Offset: 0x00021AC2
	public int WoodCount
	{
		get
		{
			return Mathf.Min(StrategyBase.MaxElementCount, this.BaseWoodCount + this.TempWoodCount);
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06000DCD RID: 3533 RVA: 0x000238DB File Offset: 0x00021ADB
	public int WaterCount
	{
		get
		{
			return Mathf.Min(StrategyBase.MaxElementCount, this.BaseWaterCount + this.TempWaterCount);
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06000DCE RID: 3534 RVA: 0x000238F4 File Offset: 0x00021AF4
	public int FireCount
	{
		get
		{
			return Mathf.Min(StrategyBase.MaxElementCount, this.BaseFireCount + this.TempFireCount);
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0002390D File Offset: 0x00021B0D
	public int DustCount
	{
		get
		{
			return Mathf.Min(StrategyBase.MaxElementCount, this.BaseDustCount + this.TempDustCount);
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00023926 File Offset: 0x00021B26
	public int TotalElementCount
	{
		get
		{
			return this.GoldCount + this.WoodCount + this.WaterCount + this.FireCount + this.DustCount;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0002394A File Offset: 0x00021B4A
	public float AttackPerGold
	{
		get
		{
			return Singleton<StaticData>.Instance.GoldAttackIntensify * StaticData.ElementBenefit[Mathf.Min((this.GoldCount - 1) / 2, 3)];
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0002396D File Offset: 0x00021B6D
	public float FireratePerWood
	{
		get
		{
			return Singleton<StaticData>.Instance.WoodFirerateIntensify * StaticData.ElementBenefit[Mathf.Min((this.WoodCount - 1) / 2, 3)];
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00023990 File Offset: 0x00021B90
	public float SlowPerWater
	{
		get
		{
			return Singleton<StaticData>.Instance.WaterSlowIntensify * StaticData.ElementBenefit[Mathf.Min((this.WaterCount - 1) / 2, 3)];
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x000239B3 File Offset: 0x00021BB3
	public float CritPerFire
	{
		get
		{
			return Singleton<StaticData>.Instance.FireCritIntensify * StaticData.ElementBenefit[Mathf.Min((this.FireCount - 1) / 2, 3)];
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x000239D6 File Offset: 0x00021BD6
	public float SplashPerDust
	{
		get
		{
			return Singleton<StaticData>.Instance.DustSplashIntensify * StaticData.ElementBenefit[Mathf.Min((this.DustCount - 1) / 2, 3)];
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x000239F9 File Offset: 0x00021BF9
	public float ElementAttackIntensify
	{
		get
		{
			return this.AttackPerGold * (float)this.GoldCount;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00023A09 File Offset: 0x00021C09
	public float ElementFirerateIntensify
	{
		get
		{
			return this.FireratePerWood * (float)this.WoodCount;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00023A19 File Offset: 0x00021C19
	public float ElementSlowIntensify
	{
		get
		{
			return this.SlowPerWater * (float)this.WaterCount;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00023A29 File Offset: 0x00021C29
	public float ElementCritIntensify
	{
		get
		{
			return this.CritPerFire * (float)this.FireCount;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00023A39 File Offset: 0x00021C39
	public float ElementSplashIntensify
	{
		get
		{
			return this.SplashPerDust * (float)this.DustCount;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00023A49 File Offset: 0x00021C49
	// (set) Token: 0x06000DDC RID: 3548 RVA: 0x00023A51 File Offset: 0x00021C51
	public float AttackAdjust { get; set; }

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00023A5A File Offset: 0x00021C5A
	// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00023A62 File Offset: 0x00021C62
	public float FireRateAdjust { get; set; }

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00023A6B File Offset: 0x00021C6B
	// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00023A73 File Offset: 0x00021C73
	public float SlowRateAdjust { get; set; }

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00023A7C File Offset: 0x00021C7C
	// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00023A84 File Offset: 0x00021C84
	public float SplashRangeAdjust { get; set; }

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00023A8D File Offset: 0x00021C8D
	// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x00023A95 File Offset: 0x00021C95
	public float BaseFixAttack
	{
		get
		{
			return this.baseFixAttack;
		}
		set
		{
			this.baseFixAttack = value;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00023A9E File Offset: 0x00021C9E
	// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x00023AA6 File Offset: 0x00021CA6
	public float BaseFixFirerate
	{
		get
		{
			return this.baseFixFirerate;
		}
		set
		{
			this.baseFixFirerate = value;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00023AAF File Offset: 0x00021CAF
	// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x00023AB7 File Offset: 0x00021CB7
	public float BaseFixSlow
	{
		get
		{
			return this.baseFixSlow;
		}
		set
		{
			this.baseFixSlow = value;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00023AC0 File Offset: 0x00021CC0
	// (set) Token: 0x06000DEA RID: 3562 RVA: 0x00023AC8 File Offset: 0x00021CC8
	public float BaseFixSplash
	{
		get
		{
			return this.baseFixSplash;
		}
		set
		{
			this.baseFixSplash = value;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00023AD1 File Offset: 0x00021CD1
	// (set) Token: 0x06000DEC RID: 3564 RVA: 0x00023AD9 File Offset: 0x00021CD9
	public float BaseFixCritRate
	{
		get
		{
			return this.baseFixCrit;
		}
		set
		{
			this.baseFixCrit = value;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06000DED RID: 3565 RVA: 0x00023AE2 File Offset: 0x00021CE2
	// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00023AEA File Offset: 0x00021CEA
	public int BaseFixRange
	{
		get
		{
			return this.baseFixRange;
		}
		set
		{
			this.baseFixRange = value;
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00023AF3 File Offset: 0x00021CF3
	// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00023AFB File Offset: 0x00021CFB
	public float BaseFixDamageIntensify
	{
		get
		{
			return this.baseFixDamageIntensify;
		}
		set
		{
			this.baseFixDamageIntensify = value;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00023B04 File Offset: 0x00021D04
	// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00023B0C File Offset: 0x00021D0C
	public float BaseFixFrostResist
	{
		get
		{
			return this.baseFixFrostResist;
		}
		set
		{
			this.baseFixFrostResist = value;
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00023B15 File Offset: 0x00021D15
	// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x00023B1D File Offset: 0x00021D1D
	public int BaseFixTargetCount
	{
		get
		{
			return this.baseFixTargetCount;
		}
		set
		{
			this.baseFixTargetCount = value;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x00023B26 File Offset: 0x00021D26
	// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x00023B2E File Offset: 0x00021D2E
	public float BaseFixBulletEffectIntensify
	{
		get
		{
			return this.baseFixBulletEffectIntensify;
		}
		set
		{
			this.baseFixBulletEffectIntensify = value;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x00023B37 File Offset: 0x00021D37
	public float BaseAttack
	{
		get
		{
			return this.InitAttack + this.BaseFixAttack + this.TurnFixAttack;
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00023B4D File Offset: 0x00021D4D
	public float BaseFirerate
	{
		get
		{
			return this.InitFireRate + this.BaseFixFirerate + this.TurnFixFirerate;
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00023B63 File Offset: 0x00021D63
	public int BaseRange
	{
		get
		{
			return this.InitRange + this.BaseFixRange + this.TurnFixRange;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06000DFA RID: 3578 RVA: 0x00023B79 File Offset: 0x00021D79
	public float BaseCriticalRate
	{
		get
		{
			return this.InitCriticalRate + this.BaseFixCritRate + this.ElementCritIntensify + this.TurnFixCriticalRate;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00023B96 File Offset: 0x00021D96
	public float BaseCriticalPercentage
	{
		get
		{
			return StaticData.DefaultCritDmg + this.TurnFixCriticalPercentage;
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06000DFC RID: 3580 RVA: 0x00023BA4 File Offset: 0x00021DA4
	public float BaseSplashRange
	{
		get
		{
			return this.InitSplashRange + this.BaseFixSplash + this.ElementSplashIntensify + this.TurnFixSplashRange;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00023BC1 File Offset: 0x00021DC1
	public float BaseSplashPercentage
	{
		get
		{
			return StaticData.DefaultSplashDmg + this.TurnFixSplashPercentage;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06000DFE RID: 3582 RVA: 0x00023BCF File Offset: 0x00021DCF
	public float BaseSlowPercentage
	{
		get
		{
			return this.initSlowPercentage + this.TurnFixSlowRatePercentage;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00023BDE File Offset: 0x00021DDE
	public float BaseSlowRate
	{
		get
		{
			return this.InitSlowRate + this.BaseFixSlow + this.ElementSlowIntensify + this.TurnFixSlowRate;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06000E00 RID: 3584 RVA: 0x00023BFB File Offset: 0x00021DFB
	public int BaseTargetCount
	{
		get
		{
			return this.initTargetCount + this.BaseFixTargetCount + this.TurnFixTargetCount;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00023C11 File Offset: 0x00021E11
	public float BaseDamageIntensify
	{
		get
		{
			return this.InitDamageIntensify + this.BaseFixDamageIntensify;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06000E02 RID: 3586 RVA: 0x00023C20 File Offset: 0x00021E20
	public float TotalAttackIntensify
	{
		get
		{
			return this.ElementAttackIntensify + this.AttackIntensify + this.TurnAttackIntensify + Mathf.Min(StrategyBase.CoordinatorMaxIntensify, StrategyBase.CooporativeAttackIntensify);
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00023C46 File Offset: 0x00021E46
	public float TotalFirerateIntensify
	{
		get
		{
			return this.ElementFirerateIntensify + this.FirerateIntensify + this.TurnFireRateIntensify;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06000E04 RID: 3588 RVA: 0x00023C5C File Offset: 0x00021E5C
	public virtual float FinalAttack
	{
		get
		{
			return Mathf.Max(0f, this.BaseAttack * (1f + this.TotalAttackIntensify));
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06000E05 RID: 3589 RVA: 0x00023C7B File Offset: 0x00021E7B
	public virtual float FinalFireRate
	{
		get
		{
			return Mathf.Min(this.MaxFireRate, this.BaseFirerate * (1f + this.TotalFirerateIntensify));
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06000E06 RID: 3590 RVA: 0x00023C9B File Offset: 0x00021E9B
	public virtual int FinalRange
	{
		get
		{
			return Mathf.Min(this.MaxRange, this.BaseRange);
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00023CAE File Offset: 0x00021EAE
	public float FinalCriticalRate
	{
		get
		{
			return this.BaseCriticalRate;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00023CB6 File Offset: 0x00021EB6
	public float FinalCriticalPercentage
	{
		get
		{
			return this.BaseCriticalPercentage + this.FinalCriticalRate;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00023CC5 File Offset: 0x00021EC5
	public virtual float FinalSplashRange
	{
		get
		{
			return Mathf.Min(this.MaxSplash, this.BaseSplashRange);
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06000E0A RID: 3594 RVA: 0x00023CD8 File Offset: 0x00021ED8
	public float FinalSplashPercentage
	{
		get
		{
			return this.BaseSplashPercentage + this.FinalSplashRange * 0.1f;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00023CED File Offset: 0x00021EED
	public float FinalSlowRate
	{
		get
		{
			return this.BaseSlowRate;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00023CF5 File Offset: 0x00021EF5
	public float FianlSlowPercentageOfSplash
	{
		get
		{
			return this.BaseSlowPercentage;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06000E0D RID: 3597 RVA: 0x00023CFD File Offset: 0x00021EFD
	public int FinalTargetCount
	{
		get
		{
			return this.BaseTargetCount;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06000E0E RID: 3598 RVA: 0x00023D05 File Offset: 0x00021F05
	public float FinalBulletDamageIntensify
	{
		get
		{
			return this.BaseDamageIntensify + this.TurnFixDamageBonus;
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06000E0F RID: 3599 RVA: 0x00023D14 File Offset: 0x00021F14
	public float FinalBulletSize
	{
		get
		{
			return this.BulletSize + this.TurnBulletSize;
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00023D23 File Offset: 0x00021F23
	public float FinalFrostResist
	{
		get
		{
			return this.TurnFrostResist + this.BaseFixFrostResist;
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00023D32 File Offset: 0x00021F32
	public float FinalBulletEffectIntensify
	{
		get
		{
			return this.BaseFixBulletEffectIntensify + this.TurnFixBulletEffectIntensify;
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00023D41 File Offset: 0x00021F41
	// (set) Token: 0x06000E13 RID: 3603 RVA: 0x00023D49 File Offset: 0x00021F49
	public float TurnFixDamageBonus
	{
		get
		{
			return this.turnFixDamageBonus;
		}
		set
		{
			this.turnFixDamageBonus = value;
		}
	}

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00023D52 File Offset: 0x00021F52
	// (set) Token: 0x06000E15 RID: 3605 RVA: 0x00023D5A File Offset: 0x00021F5A
	public float TurnFixAttack
	{
		get
		{
			return this.turnFixAttack;
		}
		set
		{
			this.turnFixAttack = value;
		}
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06000E16 RID: 3606 RVA: 0x00023D63 File Offset: 0x00021F63
	// (set) Token: 0x06000E17 RID: 3607 RVA: 0x00023D6B File Offset: 0x00021F6B
	public float TurnFixFirerate
	{
		get
		{
			return this.turnFixSpeed;
		}
		set
		{
			this.turnFixSpeed = value;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00023D74 File Offset: 0x00021F74
	// (set) Token: 0x06000E19 RID: 3609 RVA: 0x00023D7C File Offset: 0x00021F7C
	public int TurnFixRange
	{
		get
		{
			return this.turnFixRange;
		}
		set
		{
			this.turnFixRange = value;
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00023D85 File Offset: 0x00021F85
	// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00023D8D File Offset: 0x00021F8D
	public float TurnFixSplashRange
	{
		get
		{
			return this.turnFixSplashRange;
		}
		set
		{
			this.turnFixSplashRange = value;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00023D96 File Offset: 0x00021F96
	// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00023D9E File Offset: 0x00021F9E
	public float TurnFixSplashPercentage
	{
		get
		{
			return this.turnFixSplashPercentage;
		}
		set
		{
			this.turnFixSplashPercentage = value;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00023DA7 File Offset: 0x00021FA7
	// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00023DAF File Offset: 0x00021FAF
	public float TurnFixCriticalRate
	{
		get
		{
			return this.turnFixCriticalRate;
		}
		set
		{
			this.turnFixCriticalRate = value;
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00023DB8 File Offset: 0x00021FB8
	// (set) Token: 0x06000E21 RID: 3617 RVA: 0x00023DC0 File Offset: 0x00021FC0
	public float TurnFixCriticalPercentage
	{
		get
		{
			return this.turnFixCriticalPercentage;
		}
		set
		{
			this.turnFixCriticalPercentage = value;
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00023DC9 File Offset: 0x00021FC9
	// (set) Token: 0x06000E23 RID: 3619 RVA: 0x00023DD1 File Offset: 0x00021FD1
	public float TurnFixSlowRate
	{
		get
		{
			return this.turnFixSlowRate;
		}
		set
		{
			this.turnFixSlowRate = value;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00023DDA File Offset: 0x00021FDA
	// (set) Token: 0x06000E25 RID: 3621 RVA: 0x00023DE2 File Offset: 0x00021FE2
	public float TurnFixSlowRatePercentage
	{
		get
		{
			return this.turnFixSlowRatePercentage;
		}
		set
		{
			this.turnFixSlowRatePercentage = value;
		}
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00023DEB File Offset: 0x00021FEB
	// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00023DF3 File Offset: 0x00021FF3
	public int TurnFixTargetCount
	{
		get
		{
			return this.turnFixTargetCount;
		}
		set
		{
			this.turnFixTargetCount = value;
		}
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00023DFC File Offset: 0x00021FFC
	// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00023E04 File Offset: 0x00022004
	public float TurnFixBulletEffectIntensify
	{
		get
		{
			return this.turnFixBulletEffectIntensify;
		}
		set
		{
			this.turnFixBulletEffectIntensify = value;
		}
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00023E0D File Offset: 0x0002200D
	// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00023E15 File Offset: 0x00022015
	public float AttackIntensify
	{
		get
		{
			return this.attackIntensify;
		}
		set
		{
			this.attackIntensify = value;
		}
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00023E1E File Offset: 0x0002201E
	// (set) Token: 0x06000E2D RID: 3629 RVA: 0x00023E26 File Offset: 0x00022026
	public float FirerateIntensify
	{
		get
		{
			return this.firerateIntensify;
		}
		set
		{
			this.firerateIntensify = value;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06000E2E RID: 3630 RVA: 0x00023E2F File Offset: 0x0002202F
	// (set) Token: 0x06000E2F RID: 3631 RVA: 0x00023E37 File Offset: 0x00022037
	public float TurnFrostResist
	{
		get
		{
			return this.turnFrostResist;
		}
		set
		{
			this.turnFrostResist = value;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00023E40 File Offset: 0x00022040
	// (set) Token: 0x06000E31 RID: 3633 RVA: 0x00023E48 File Offset: 0x00022048
	public virtual float TurnAttackIntensify
	{
		get
		{
			return this.turnAttackIntensify;
		}
		set
		{
			this.turnAttackIntensify = value;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00023E51 File Offset: 0x00022051
	// (set) Token: 0x06000E33 RID: 3635 RVA: 0x00023E59 File Offset: 0x00022059
	public float TurnFireRateIntensify
	{
		get
		{
			return this.turnSpeedIntensify;
		}
		set
		{
			this.turnSpeedIntensify = value;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00023E62 File Offset: 0x00022062
	// (set) Token: 0x06000E35 RID: 3637 RVA: 0x00023E6A File Offset: 0x0002206A
	public float TurnBulletSize
	{
		get
		{
			return this.turnBulletSize;
		}
		set
		{
			this.turnBulletSize = value;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00023E73 File Offset: 0x00022073
	public float NextAttack
	{
		get
		{
			return (this.Attribute.TurretLevels[this.Quality].AttackDamage + this.TurnFixAttack + this.BaseFixAttack) * (1f + this.TotalAttackIntensify);
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00023EAB File Offset: 0x000220AB
	public float NextFirarate
	{
		get
		{
			return (this.Attribute.TurretLevels[this.Quality].AttackSpeed + this.TurnFixFirerate + this.BaseFixFirerate) * (1f + this.TotalFirerateIntensify);
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00023EE3 File Offset: 0x000220E3
	public float NextSplashRange
	{
		get
		{
			return this.Attribute.TurretLevels[this.Quality].SplashRange + this.ElementSplashIntensify;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00023F07 File Offset: 0x00022107
	public float NextCriticalRate
	{
		get
		{
			return this.Attribute.TurretLevels[this.Quality].CriticalRate + this.ElementCritIntensify;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00023F2B File Offset: 0x0002212B
	public float NextSlowRate
	{
		get
		{
			return this.Attribute.TurretLevels[this.Quality].SlowRate + this.ElementSlowIntensify;
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00023F4F File Offset: 0x0002214F
	public float NextDmgIntentisfy
	{
		get
		{
			return this.Attribute.TurretLevels[this.Quality].DamageIntensify;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00023F6C File Offset: 0x0002216C
	// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00023F74 File Offset: 0x00022174
	public TurretSkill TurretSkill { get; set; }

	// Token: 0x06000E3E RID: 3646 RVA: 0x00023F80 File Offset: 0x00022180
	public virtual void GetTurretSkills()
	{
		this.TurretSkills.Clear();
		TurretSkill initialSkill = TurretSkillFactory.GetInitialSkill((int)this.Attribute.RefactorName);
		this.TurretSkill = initialSkill;
		this.TurretSkill.strategy = this;
		this.TurretSkills.Add(initialSkill);
		this.TurretSkill.Build();
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00023FD4 File Offset: 0x000221D4
	public void GainRandomTempElement(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			switch (Random.Range(0, 5))
			{
			case 0:
				this.TempGoldCount++;
				break;
			case 1:
				this.TempWoodCount++;
				break;
			case 2:
				this.TempWaterCount++;
				break;
			case 3:
				this.TempFireCount++;
				break;
			case 4:
				this.TempDustCount++;
				break;
			}
		}
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x0002405F File Offset: 0x0002225F
	public void AddElementSkill(ElementSkill skill)
	{
		if (skill == null)
		{
			return;
		}
		skill.strategy = this;
		this.TurretSkills.Add(skill);
		skill.Build();
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0002407E File Offset: 0x0002227E
	public void AddGlobalSkill(GlobalSkill skill)
	{
		skill.strategy = this;
		this.GlobalSkills.Add(skill);
		skill.Build();
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x0002409C File Offset: 0x0002229C
	public void OnEquipSkill()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills.ToList<TurretSkill>())
		{
			turretSkill.OnEquip();
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x000240F4 File Offset: 0x000222F4
	public virtual void SetQualityValue()
	{
		this.InitAttack = this.Attribute.TurretLevels[this.Quality - 1].AttackDamage;
		this.InitFireRate = this.Attribute.TurretLevels[this.Quality - 1].AttackSpeed;
		this.InitRange = this.Attribute.TurretLevels[this.Quality - 1].AttackRange;
		this.InitCriticalRate = this.Attribute.TurretLevels[this.Quality - 1].CriticalRate;
		this.InitSplashRange = this.Attribute.TurretLevels[this.Quality - 1].SplashRange;
		this.InitSlowRate = this.Attribute.TurretLevels[this.Quality - 1].SlowRate;
		this.InitDamageIntensify = this.Attribute.TurretLevels[this.Quality - 1].DamageIntensify;
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x000241F8 File Offset: 0x000223F8
	public void StartTurnSkills()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.StartTurn();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.StartTurn();
		}
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00024288 File Offset: 0x00022488
	public void StartTurnSkill2()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.StartTurn2();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.StartTurn2();
		}
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00024318 File Offset: 0x00022518
	public void StartTurnSkill3()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.StartTurn3();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.StartTurn3();
		}
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x000243A8 File Offset: 0x000225A8
	public void ClearTurnAnalysis()
	{
		this.TurnDamage = 0L;
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x000243B4 File Offset: 0x000225B4
	public void ClearTurnIntensify()
	{
		this.TempGoldCount = 0;
		this.TempWoodCount = 0;
		this.TempWaterCount = 0;
		this.TempFireCount = 0;
		this.TempDustCount = 0;
		this.TurnFixAttack = 0f;
		this.TurnFixFirerate = 0f;
		this.TurnFixRange = 0;
		this.TurnFixSplashRange = 0f;
		this.TurnFixSplashPercentage = 0f;
		this.TurnFixCriticalRate = 0f;
		this.TurnFixCriticalPercentage = 0f;
		this.TurnFixSlowRate = 0f;
		this.TurnFixTargetCount = 0;
		this.TurnFrostResist = 0f;
		this.TurnFixSlowRatePercentage = 0f;
		this.TurnBulletSize = 0f;
		this.TurnFixDamageBonus = 0f;
		this.TurnFixBulletEffectIntensify = 0f;
		this.TurnAttackIntensify = 0f;
		this.TurnFireRateIntensify = 0f;
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.EndTurn();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.EndTurn();
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x00024510 File Offset: 0x00022710
	public void DetectSkills()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.Detect();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.Detect();
		}
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x000245A0 File Offset: 0x000227A0
	public void DrawTurretSkill()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.Draw();
		}
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x000245F0 File Offset: 0x000227F0
	public void OnEquippedSkill()
	{
		foreach (TurretSkill turretSkill in this.TurretSkills)
		{
			turretSkill.OnEquipped();
		}
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			globalSkill.OnEquipped();
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00024680 File Offset: 0x00022880
	public void CompositeSkill()
	{
		for (int i = 0; i < this.TurretSkills.Count; i++)
		{
			this.TurretSkills[i].Composite();
		}
		for (int j = 0; j < this.GlobalSkills.Count; j++)
		{
			this.GlobalSkills[j].Composite();
		}
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x000246DC File Offset: 0x000228DC
	public void EnterSkill(IDamage target)
	{
		for (int i = 0; i < this.TurretSkills.Count; i++)
		{
			this.TurretSkills[i].OnEnter(target);
		}
		for (int j = 0; j < this.GlobalSkills.Count; j++)
		{
			this.GlobalSkills[j].OnEnter(target);
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x0002473C File Offset: 0x0002293C
	public void ExitSkill(IDamage target)
	{
		for (int i = 0; i < this.TurretSkills.Count; i++)
		{
			this.TurretSkills[i].OnExit(target);
		}
		for (int j = 0; j < this.GlobalSkills.Count; j++)
		{
			this.GlobalSkills[j].OnExit(target);
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0002479C File Offset: 0x0002299C
	public List<ElementType> GetRandomElementsOfthisTurret(int amount)
	{
		List<ElementType> list = new List<ElementType>();
		List<ElementType> list2 = new List<ElementType>();
		for (int i = 0; i < this.GoldCount; i++)
		{
			list2.Add(ElementType.GOLD);
		}
		for (int j = 0; j < this.WoodCount; j++)
		{
			list2.Add(ElementType.WOOD);
		}
		for (int k = 0; k < this.WaterCount; k++)
		{
			list2.Add(ElementType.WATER);
		}
		for (int l = 0; l < this.FireCount; l++)
		{
			list2.Add(ElementType.FIRE);
		}
		for (int m = 0; m < this.DustCount; m++)
		{
			list2.Add(ElementType.DUST);
		}
		List<int> list3 = StaticData.SelectNoRepeat(amount, list2.Count);
		for (int n = 0; n < amount; n++)
		{
			list.Add(list2[list3[n]]);
		}
		return list;
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0002486F File Offset: 0x00022A6F
	public virtual void UndoStrategy()
	{
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x00024874 File Offset: 0x00022A74
	public void RemoveGlobalSkill(GlobalSkill skill)
	{
		foreach (GlobalSkill globalSkill in this.GlobalSkills)
		{
			if (globalSkill.GlobalSkillName == skill.GlobalSkillName)
			{
				this.GlobalSkills.Remove(globalSkill);
			}
		}
	}

	// Token: 0x04000689 RID: 1673
	public ConcreteContent Concrete;

	// Token: 0x0400068A RID: 1674
	public TurretAttribute Attribute;

	// Token: 0x0400068B RID: 1675
	private int quality;

	// Token: 0x0400068C RID: 1676
	private float initAttack;

	// Token: 0x0400068D RID: 1677
	private float initSpeed;

	// Token: 0x0400068E RID: 1678
	private int initRange;

	// Token: 0x0400068F RID: 1679
	private float initCriticalRate;

	// Token: 0x04000690 RID: 1680
	private float initSputteringRange;

	// Token: 0x04000691 RID: 1681
	private float initSlowRate;

	// Token: 0x04000692 RID: 1682
	private float initDamageIntensify;

	// Token: 0x04000693 RID: 1683
	private long turnDamage;

	// Token: 0x04000694 RID: 1684
	private long totalDamage;

	// Token: 0x04000695 RID: 1685
	public int InitSkillSLot = 3;

	// Token: 0x04000696 RID: 1686
	public int PrivateExtraSlot;

	// Token: 0x04000698 RID: 1688
	private RangeType rangeType;

	// Token: 0x04000699 RID: 1689
	private float initSlowPercentage = 0.5f;

	// Token: 0x0400069A RID: 1690
	private int initTargetCount = 1;

	// Token: 0x0400069B RID: 1691
	private float rotSpeed = 15f;

	// Token: 0x0400069C RID: 1692
	private float checkAngle = 10f;

	// Token: 0x0400069D RID: 1693
	private float timeModify = 1f;

	// Token: 0x0400069E RID: 1694
	private float bulletSize;

	// Token: 0x040006A0 RID: 1696
	private int shootTriggerCount = 1;

	// Token: 0x040006A1 RID: 1697
	private int forbidRange;

	// Token: 0x040006A2 RID: 1698
	private float maxFirerate = 30f;

	// Token: 0x040006A3 RID: 1699
	private float maxSplash = 25f;

	// Token: 0x040006A4 RID: 1700
	private int maxRange = 50;

	// Token: 0x040006A5 RID: 1701
	private static float coordinatorMaxIntensify = 1f;

	// Token: 0x040006A6 RID: 1702
	private static float cooporativeAttackIntensify;

	// Token: 0x040006A7 RID: 1703
	public int BaseGoldCount;

	// Token: 0x040006A8 RID: 1704
	public int BaseWoodCount;

	// Token: 0x040006A9 RID: 1705
	public int BaseWaterCount;

	// Token: 0x040006AA RID: 1706
	public int BaseFireCount;

	// Token: 0x040006AB RID: 1707
	public int BaseDustCount;

	// Token: 0x040006AC RID: 1708
	public static int MaxElementCount = 99;

	// Token: 0x040006AD RID: 1709
	public int TempGoldCount;

	// Token: 0x040006AE RID: 1710
	public int TempWoodCount;

	// Token: 0x040006AF RID: 1711
	public int TempWaterCount;

	// Token: 0x040006B0 RID: 1712
	public int TempFireCount;

	// Token: 0x040006B1 RID: 1713
	public int TempDustCount;

	// Token: 0x040006B6 RID: 1718
	private float baseFixAttack;

	// Token: 0x040006B7 RID: 1719
	private float baseFixFirerate;

	// Token: 0x040006B8 RID: 1720
	private float baseFixSlow;

	// Token: 0x040006B9 RID: 1721
	private float baseFixSplash;

	// Token: 0x040006BA RID: 1722
	private float baseFixCrit;

	// Token: 0x040006BB RID: 1723
	private int baseFixRange;

	// Token: 0x040006BC RID: 1724
	private float baseFixDamageIntensify;

	// Token: 0x040006BD RID: 1725
	private float baseFixFrostResist;

	// Token: 0x040006BE RID: 1726
	private int baseFixTargetCount;

	// Token: 0x040006BF RID: 1727
	private float baseFixBulletEffectIntensify;

	// Token: 0x040006C0 RID: 1728
	private float turnFixAttack;

	// Token: 0x040006C1 RID: 1729
	private float turnFixSpeed;

	// Token: 0x040006C2 RID: 1730
	private int turnFixRange;

	// Token: 0x040006C3 RID: 1731
	private float turnFixSplashRange;

	// Token: 0x040006C4 RID: 1732
	private float turnFixSplashPercentage;

	// Token: 0x040006C5 RID: 1733
	private float turnFixCriticalRate;

	// Token: 0x040006C6 RID: 1734
	private float turnFixCriticalPercentage;

	// Token: 0x040006C7 RID: 1735
	private float turnFixSlowRate;

	// Token: 0x040006C8 RID: 1736
	private float turnFixSlowRatePercentage;

	// Token: 0x040006C9 RID: 1737
	private int turnFixTargetCount;

	// Token: 0x040006CA RID: 1738
	private float turnFixDamageBonus;

	// Token: 0x040006CB RID: 1739
	private float turnFixBulletEffectIntensify;

	// Token: 0x040006CC RID: 1740
	private float attackIntensify;

	// Token: 0x040006CD RID: 1741
	private float firerateIntensify;

	// Token: 0x040006CE RID: 1742
	private float turnAttackIntensify;

	// Token: 0x040006CF RID: 1743
	private float turnSpeedIntensify;

	// Token: 0x040006D0 RID: 1744
	private float turnBulletSize;

	// Token: 0x040006D1 RID: 1745
	private float turnFrostResist;

	// Token: 0x040006D3 RID: 1747
	public List<TurretSkill> TurretSkills = new List<TurretSkill>();

	// Token: 0x040006D4 RID: 1748
	public List<GlobalSkill> GlobalSkills = new List<GlobalSkill>();
}
