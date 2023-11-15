using System;
using Level.BlackMarket;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class Settings : ScriptableObject
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A687 File Offset: 0x00008887
	// (set) Token: 0x06000293 RID: 659 RVA: 0x0000A68F File Offset: 0x0000888F
	public EnumArray<Rarity, RarityPossibilities> containerPossibilities { get; private set; }

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000294 RID: 660 RVA: 0x0000A698 File Offset: 0x00008898
	public GoldPossibility smallPropGoldPossibility
	{
		get
		{
			return this._smallPropGoldPossibility;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000295 RID: 661 RVA: 0x0000A6A0 File Offset: 0x000088A0
	public GoldPossibility largePropGoldPossibility
	{
		get
		{
			return this._largePropGoldPossibility;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000296 RID: 662 RVA: 0x0000A6A8 File Offset: 0x000088A8
	public RarityPrices bonesByDiscard
	{
		get
		{
			return this._bonesByDiscard;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000297 RID: 663 RVA: 0x0000A6B0 File Offset: 0x000088B0
	public RarityPrices bonesToUpgrade
	{
		get
		{
			return this._bonesToUpgrade;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000298 RID: 664 RVA: 0x0000A6B8 File Offset: 0x000088B8
	public GlobalSettings marketSettings
	{
		get
		{
			return this._marketSettings;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A6C0 File Offset: 0x000088C0
	public static Settings instance
	{
		get
		{
			if (Settings._instance == null)
			{
				Settings._instance = Resources.Load<Settings>("Settings");
				Settings._instance.Initialize();
			}
			return Settings._instance;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000A6F0 File Offset: 0x000088F0
	public void Initialize()
	{
		this.containerPossibilities = new EnumArray<Rarity, RarityPossibilities>();
		this.containerPossibilities[Rarity.Common] = this._commonContainerPossibilities;
		this.containerPossibilities[Rarity.Rare] = this._rareContainerPossibilities;
		this.containerPossibilities[Rarity.Unique] = this._uniqueContainerPossibilities;
		this.containerPossibilities[Rarity.Legendary] = this._legendaryContainerPossibilities;
	}

	// Token: 0x04000222 RID: 546
	[SerializeField]
	[Header("Container")]
	private RarityPossibilities _commonContainerPossibilities;

	// Token: 0x04000223 RID: 547
	[SerializeField]
	private RarityPossibilities _rareContainerPossibilities;

	// Token: 0x04000224 RID: 548
	[SerializeField]
	private RarityPossibilities _uniqueContainerPossibilities;

	// Token: 0x04000225 RID: 549
	[SerializeField]
	private RarityPossibilities _legendaryContainerPossibilities;

	// Token: 0x04000226 RID: 550
	[Header("Prop Gold")]
	[SerializeField]
	private GoldPossibility _smallPropGoldPossibility;

	// Token: 0x04000227 RID: 551
	[SerializeField]
	private GoldPossibility _largePropGoldPossibility;

	// Token: 0x04000228 RID: 552
	[Header("Head")]
	[SerializeField]
	private RarityPrices _bonesByDiscard;

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private RarityPrices _bonesToUpgrade;

	// Token: 0x0400022A RID: 554
	[Header("Blackmarket")]
	[SerializeField]
	private GlobalSettings _marketSettings;

	// Token: 0x0400022C RID: 556
	private static Settings _instance;
}
