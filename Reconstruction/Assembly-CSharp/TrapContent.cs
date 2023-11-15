using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class TrapContent : GameTileContent
{
	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06000C45 RID: 3141 RVA: 0x000200AD File Offset: 0x0001E2AD
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.Trap;
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06000C46 RID: 3142 RVA: 0x000200B0 File Offset: 0x0001E2B0
	public TrapAttribute TrapAttribute
	{
		get
		{
			return this.trapAttribute;
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06000C47 RID: 3143 RVA: 0x000200B8 File Offset: 0x0001E2B8
	// (set) Token: 0x06000C48 RID: 3144 RVA: 0x000200C0 File Offset: 0x0001E2C0
	public virtual long DamageAnalysis
	{
		get
		{
			return this.damageAnalysis;
		}
		set
		{
			this.damageAnalysis = value;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06000C49 RID: 3145 RVA: 0x000200C9 File Offset: 0x0001E2C9
	// (set) Token: 0x06000C4A RID: 3146 RVA: 0x000200D1 File Offset: 0x0001E2D1
	public virtual int CoinAnalysis
	{
		get
		{
			return this.coinAnalysis;
		}
		set
		{
			this.coinAnalysis = value;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06000C4B RID: 3147 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual int DieProtect
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000200DD File Offset: 0x0001E2DD
	// (set) Token: 0x06000C4D RID: 3149 RVA: 0x000200EB File Offset: 0x0001E2EB
	public float TrapIntensify
	{
		get
		{
			return this.trapIntensify + GameRes.TrapItensify;
		}
		set
		{
			this.trapIntensify = value;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06000C4E RID: 3150 RVA: 0x000200F4 File Offset: 0x0001E2F4
	// (set) Token: 0x06000C4F RID: 3151 RVA: 0x000200FC File Offset: 0x0001E2FC
	public bool IsReveal
	{
		get
		{
			return this.isReveal;
		}
		set
		{
			this.isReveal = value;
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00020105 File Offset: 0x0001E305
	public void Initialize(TrapAttribute att)
	{
		this.trapAttribute = att;
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00020110 File Offset: 0x0001E310
	protected virtual void Awake()
	{
		this.trapGFX = base.transform.Find("Sprite").GetComponent<SpriteRenderer>();
		this.initSprite = this.trapGFX.sprite;
		this.unrevealSprite = Singleton<StaticData>.Instance.UnrevealTrap;
		this.trapGFX.sprite = this.unrevealSprite;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0002016C File Offset: 0x0001E36C
	public override void ContentLanded()
	{
		base.ContentLanded();
		base.m_GameTile.tag = StaticData.UndropablePoint;
		StaticData.SetNodeWalkable(base.m_GameTile, true, true);
		Collider2D col = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.ConcreteTileMask
		}));
		this.ContentLandedCheck(col);
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x000201D1 File Offset: 0x0001E3D1
	public override void SaveContent(out ContentStruct contentStruct)
	{
		base.SaveContent(out contentStruct);
		contentStruct = this.m_ContentStruct;
		contentStruct.TrapRevealed = this.IsReveal;
		this.m_ContentStruct.ContentName = this.TrapAttribute.Name;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00020205 File Offset: 0x0001E405
	public override void CorretRotation()
	{
		base.CorretRotation();
		if (this.needReset)
		{
			base.transform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00020225 File Offset: 0x0001E425
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, null, 0);
		enemy.PassedTraps.Add((content == null) ? this : ((TrapContent)content));
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0002024D File Offset: 0x0001E44D
	public override void OnContentSelected(bool value)
	{
		base.OnContentSelected(value);
		if (value)
		{
			Singleton<TipsManager>.Instance.ShowTrapContentTips(this, StaticData.LeftTipsPos);
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00020269 File Offset: 0x0001E469
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.DamageAnalysis = 0L;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0002027C File Offset: 0x0001E47C
	protected override void ContentLandedCheck(Collider2D col)
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
		{
			Singleton<GameManager>.Instance.ConfirmChoice();
			TechSelectPanel.PlacingChoice = false;
		}
		if (col != null)
		{
			GameTile component = col.GetComponent<GameTile>();
			Singleton<ObjectPool>.Instance.UnSpawn(component);
		}
		base.IsSwitching = false;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x000202D4 File Offset: 0x0001E4D4
	public void RevealTrap()
	{
		if (!this.IsReveal)
		{
			if (!this.needReset)
			{
				base.m_GameTile.SetRandomRotation(-1);
			}
			this.trapGFX.sprite = this.initSprite;
			this.IsReveal = true;
			GameRes.MaxMark++;
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00020321 File Offset: 0x0001E521
	public virtual void ClearTurnData()
	{
		this.CoinGainThisTurn = 0;
		this.BlinkedEnemy.Clear();
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00020335 File Offset: 0x0001E535
	public override void OnSwitch()
	{
		base.OnSwitch();
		GameRes.SwitchTrapCost += Singleton<StaticData>.Instance.SwitchTrapCostMultiply;
		GameTile normalTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
		normalTile.transform.position = base.transform.position;
		normalTile.TileLanded();
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00020373 File Offset: 0x0001E573
	protected override void UndoSwitching()
	{
		base.UndoSwitching();
		GameRes.SwitchTrapCost -= Singleton<StaticData>.Instance.SwitchTrapCostMultiply;
		Singleton<TipsManager>.Instance.ShowTrapContentTips(this, StaticData.LeftTipsPos);
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x000203A0 File Offset: 0x0001E5A0
	protected override void UndoUnSwitching()
	{
		base.UndoUnSwitching();
		Singleton<GameManager>.Instance.ShowChoices(true, false, false);
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x04000621 RID: 1569
	public List<Enemy> BlinkedEnemy = new List<Enemy>();

	// Token: 0x04000622 RID: 1570
	private TrapAttribute trapAttribute;

	// Token: 0x04000623 RID: 1571
	public bool needReset;

	// Token: 0x04000624 RID: 1572
	public int CoinGainThisTurn;

	// Token: 0x04000625 RID: 1573
	private long damageAnalysis;

	// Token: 0x04000626 RID: 1574
	private int coinAnalysis;

	// Token: 0x04000627 RID: 1575
	private float trapIntensify;

	// Token: 0x04000628 RID: 1576
	private bool isReveal;

	// Token: 0x04000629 RID: 1577
	[HideInInspector]
	public bool Important;

	// Token: 0x0400062A RID: 1578
	private SpriteRenderer trapGFX;

	// Token: 0x0400062B RID: 1579
	private Sprite initSprite;

	// Token: 0x0400062C RID: 1580
	private Sprite unrevealSprite;
}
