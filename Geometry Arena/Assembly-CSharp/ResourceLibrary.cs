using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ResourceLibrary : ScriptableObject
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000044 RID: 68 RVA: 0x00003D47 File Offset: 0x00001F47
	public static ResourceLibrary Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.resourceLibrary;
			}
			return Resources.Load<ResourceLibrary>("Assets/ResourceLibrary");
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000045 RID: 69 RVA: 0x00003D6B File Offset: 0x00001F6B
	public Sprite Sprite_Fragment
	{
		get
		{
			return this.spriteFragment;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000046 RID: 70 RVA: 0x00003D73 File Offset: 0x00001F73
	public Sprite Sprite_Score
	{
		get
		{
			return this.spriteScore;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00003D7B File Offset: 0x00001F7B
	public Sprite Sprite_Star
	{
		get
		{
			return this.spriteStar;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003D83 File Offset: 0x00001F83
	public Sprite Sprite_Circle
	{
		get
		{
			return this.polySprites[0];
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000049 RID: 73 RVA: 0x00003D8D File Offset: 0x00001F8D
	public Sprite Sprite_Circle100
	{
		get
		{
			return this.polySprites[1];
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600004A RID: 74 RVA: 0x00003D97 File Offset: 0x00001F97
	public Sprite Sprite_Triangle
	{
		get
		{
			return this.polySprites[2];
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600004B RID: 75 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public Sprite Sprite_Triangle100
	{
		get
		{
			return this.polySprites[3];
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00003DAB File Offset: 0x00001FAB
	public Sprite Sprite_Square
	{
		get
		{
			return this.polySprites[4];
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600004D RID: 77 RVA: 0x00003DB5 File Offset: 0x00001FB5
	public Sprite Sprite_Start
	{
		get
		{
			return this.polySprites[5];
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600004E RID: 78 RVA: 0x00003DBF File Offset: 0x00001FBF
	public GameObject[] Prefab_Enemys
	{
		get
		{
			return this.prefabEnemys;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600004F RID: 79 RVA: 0x00003DC7 File Offset: 0x00001FC7
	public GameObject[] Prefab_Players
	{
		get
		{
			return this.prefabPlayers;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000050 RID: 80 RVA: 0x00003DCF File Offset: 0x00001FCF
	public GameObject Prefab_Particle_UnitBlastBottom
	{
		get
		{
			return this.prefabParticleUnitBlastBottom;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000051 RID: 81 RVA: 0x00003DD7 File Offset: 0x00001FD7
	public GameObject Prefab_Particle_UnitBlastMiddle
	{
		get
		{
			return this.prefabParticleUnitBlastMiddle;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000052 RID: 82 RVA: 0x00003DDF File Offset: 0x00001FDF
	public GameObject Prefab_Particle_UnitBlastTop
	{
		get
		{
			return this.prefabParticleUnitBlastTop;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00003DE7 File Offset: 0x00001FE7
	public GameObject Prefab_Particle_BulletBlast
	{
		get
		{
			return this.prefabParticleBulletBlast;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000054 RID: 84 RVA: 0x00003DEF File Offset: 0x00001FEF
	public GameObject Prefab_Particle_BulletTrail
	{
		get
		{
			return this.prefabParticleBulletTrail;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000055 RID: 85 RVA: 0x00003DF7 File Offset: 0x00001FF7
	public GameObject Prefab_Particle_EnemyPreGene
	{
		get
		{
			return this.prefabParticleEnemyPreGene;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000056 RID: 86 RVA: 0x00003DFF File Offset: 0x00001FFF
	public GameObject Prefab_Laser
	{
		get
		{
			return this.prefabLaser;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000057 RID: 87 RVA: 0x00003E07 File Offset: 0x00002007
	public GameObject Prefab_Bullet_Tracking
	{
		get
		{
			return this.prefabBulletTracking;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000058 RID: 88 RVA: 0x00003E0F File Offset: 0x0000200F
	public GameObject Prefab_Bullet_Grenade
	{
		get
		{
			return this.prefabBulletGrenade;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000059 RID: 89 RVA: 0x00003E17 File Offset: 0x00002017
	public GameObject Prefab_Bullet_Mine
	{
		get
		{
			return this.prefabBulletMine;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600005A RID: 90 RVA: 0x00003E1F File Offset: 0x0000201F
	public GameObject Prefab_BlastWave
	{
		get
		{
			return this.prefabBlastWave;
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003E28 File Offset: 0x00002028
	public Sprite GetSprite_Shape(EnumShapeType shape, bool ifParticle)
	{
		switch (shape)
		{
		case EnumShapeType.UNINITED:
			Debug.LogError("Shape UNINITED!");
			if (!ifParticle)
			{
				return this.Sprite_Circle;
			}
			return this.Sprite_Circle100;
		case EnumShapeType.CIRCLE:
			if (!ifParticle)
			{
				return this.Sprite_Circle;
			}
			return this.Sprite_Circle100;
		case EnumShapeType.TRIANGLE:
			if (!ifParticle)
			{
				return this.Sprite_Triangle;
			}
			return this.Sprite_Triangle100;
		case EnumShapeType.SQUARE:
			return this.Sprite_Square;
		default:
			Debug.LogError("Shape WHAT??");
			return null;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00003E9E File Offset: 0x0000209E
	public GameObject Prefab_Fragment
	{
		get
		{
			return this.prefabFragment;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x0600005D RID: 93 RVA: 0x00003EA6 File Offset: 0x000020A6
	public GameObject Prefab_BattleItem
	{
		get
		{
			return this.prefabBattleItem;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600005E RID: 94 RVA: 0x00003EAE File Offset: 0x000020AE
	public float GlowIntensity_Unit
	{
		get
		{
			return this.glowIntensity_Unit;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600005F RID: 95 RVA: 0x00003EB6 File Offset: 0x000020B6
	public float GlowIntensity_Bullet
	{
		get
		{
			return this.glowIntensity_Bullet;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000060 RID: 96 RVA: 0x00003EBE File Offset: 0x000020BE
	public float GlowIntensity_OtherParticle
	{
		get
		{
			return this.glowIntensity_OtherParticle;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00003EC6 File Offset: 0x000020C6
	public float GlowIntensityItems
	{
		get
		{
			return this.glowIntensity_Items;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000062 RID: 98 RVA: 0x00003ECE File Offset: 0x000020CE
	public float GlowIntensityFragments
	{
		get
		{
			return this.glowIntensity_Fragments;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000063 RID: 99 RVA: 0x00003ED6 File Offset: 0x000020D6
	public float GlowIntensity_Wall
	{
		get
		{
			return this.glowIntensity_Wall;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00003EDE File Offset: 0x000020DE
	public float GlowBackStars
	{
		get
		{
			return this.glowBackStars;
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003EE8 File Offset: 0x000020E8
	public float GetFloat_GlowMulti()
	{
		int num = Setting.Inst.setInts[0];
		if (num <= 4)
		{
			return this.bloomIntensityInGrades[num];
		}
		Debug.LogError("Error_BloomIndexWrong");
		return this.bloomIntensityInGrades[0];
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000066 RID: 102 RVA: 0x00003F21 File Offset: 0x00002121
	public Rune Rune_Duanwu_Random
	{
		get
		{
			if (MyTool.DecimalToBool(0.5f))
			{
				return this.prefabRunes[0];
			}
			return this.prefabRunes[1];
		}
	}

	// Token: 0x04000080 RID: 128
	[Header("Logo")]
	public Sprite logoCN;

	// Token: 0x04000081 RID: 129
	[SerializeField]
	public Sprite logoENG;

	// Token: 0x04000082 RID: 130
	[Header("Sprite")]
	[SerializeField]
	private Sprite spriteFragment;

	// Token: 0x04000083 RID: 131
	[SerializeField]
	private Sprite spriteScore;

	// Token: 0x04000084 RID: 132
	[SerializeField]
	private Sprite spriteStar;

	// Token: 0x04000085 RID: 133
	[SerializeField]
	public Sprite sprite_GeometryCoin;

	// Token: 0x04000086 RID: 134
	[SerializeField]
	private Sprite[] polySprites = new Sprite[5];

	// Token: 0x04000087 RID: 135
	public Sprite sprite_Outline_Square;

	// Token: 0x04000088 RID: 136
	public Sprite sprite_Outline_Ring;

	// Token: 0x04000089 RID: 137
	[Header("SpriteList")]
	[SerializeField]
	public SpriteList Sprite_Icon_Jobs = new SpriteList();

	// Token: 0x0400008A RID: 138
	[SerializeField]
	public SpriteList SpList_Icon_Upgrades_Back = new SpriteList();

	// Token: 0x0400008B RID: 139
	[SerializeField]
	public SpriteList SpList_Icon_Upgrades_Front = new SpriteList();

	// Token: 0x0400008C RID: 140
	[SerializeField]
	public SpriteList SpList_Icon_DifficultyOptions = new SpriteList();

	// Token: 0x0400008D RID: 141
	[SerializeField]
	public SpriteList SpList_Icon_DifficultyOptionsOnBottomColor = new SpriteList();

	// Token: 0x0400008E RID: 142
	[SerializeField]
	public SpriteList SpList_Icon_DifficultyOptionsOnTopWhite = new SpriteList();

	// Token: 0x0400008F RID: 143
	[SerializeField]
	public SpriteList SpList_Icon_BattleItem = new SpriteList();

	// Token: 0x04000090 RID: 144
	[SerializeField]
	public SpriteList Splist_Icon_Modes = new SpriteList();

	// Token: 0x04000091 RID: 145
	[SerializeField]
	public SpriteList SpList_Icon_Rune = new SpriteList();

	// Token: 0x04000092 RID: 146
	[SerializeField]
	public SpriteList[] SpLists_Icon_SkillModules = new SpriteList[9];

	// Token: 0x04000093 RID: 147
	[Header("Prefab")]
	[SerializeField]
	private GameObject[] prefabEnemys = new GameObject[0];

	// Token: 0x04000094 RID: 148
	[SerializeField]
	private GameObject[] prefabPlayers = new GameObject[0];

	// Token: 0x04000095 RID: 149
	[Header("Particle")]
	[SerializeField]
	private GameObject prefabParticleUnitBlastBottom;

	// Token: 0x04000096 RID: 150
	[SerializeField]
	private GameObject prefabParticleUnitBlastMiddle;

	// Token: 0x04000097 RID: 151
	[SerializeField]
	private GameObject prefabParticleUnitBlastTop;

	// Token: 0x04000098 RID: 152
	[SerializeField]
	private GameObject prefabParticleBulletBlast;

	// Token: 0x04000099 RID: 153
	[SerializeField]
	private GameObject prefabParticleBulletTrail;

	// Token: 0x0400009A RID: 154
	[SerializeField]
	private GameObject prefabParticleEnemyPreGene;

	// Token: 0x0400009B RID: 155
	[Header("Skills")]
	[SerializeField]
	private GameObject prefabLaser;

	// Token: 0x0400009C RID: 156
	[SerializeField]
	private GameObject prefabBulletTracking;

	// Token: 0x0400009D RID: 157
	[SerializeField]
	private GameObject prefabBulletGrenade;

	// Token: 0x0400009E RID: 158
	[SerializeField]
	private GameObject prefabBulletMine;

	// Token: 0x0400009F RID: 159
	[SerializeField]
	private GameObject prefabBlastWave;

	// Token: 0x040000A0 RID: 160
	[Header("Items")]
	[SerializeField]
	private GameObject prefabFragment;

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	private GameObject prefabBattleItem;

	// Token: 0x040000A2 RID: 162
	[Header("Glow")]
	public Material matNormalUnlit;

	// Token: 0x040000A3 RID: 163
	public Material matAlphaUnlit;

	// Token: 0x040000A4 RID: 164
	public Material matGlowDefault;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	private float glowIntensity_Unit = 0.9f;

	// Token: 0x040000A6 RID: 166
	[SerializeField]
	private float glowIntensity_Bullet = 1.8f;

	// Token: 0x040000A7 RID: 167
	[SerializeField]
	private float glowIntensity_OtherParticle = 1.8f;

	// Token: 0x040000A8 RID: 168
	[SerializeField]
	private float glowIntensity_Items = 1f;

	// Token: 0x040000A9 RID: 169
	[SerializeField]
	private float glowIntensity_Fragments = 2.5f;

	// Token: 0x040000AA RID: 170
	[SerializeField]
	private float glowIntensity_Wall = 1.5f;

	// Token: 0x040000AB RID: 171
	[SerializeField]
	private float glowBackStars = 1f;

	// Token: 0x040000AC RID: 172
	[Header("Color")]
	public ColorSet colorSet_Unit = new ColorSet();

	// Token: 0x040000AD RID: 173
	public ColorSet colorSet_BulletNormal = new ColorSet();

	// Token: 0x040000AE RID: 174
	public ColorSet colorSet_BulletCrit = new ColorSet();

	// Token: 0x040000AF RID: 175
	public ColorSet colorSet_UI = new ColorSet();

	// Token: 0x040000B0 RID: 176
	public ColorSet colorSet_Enemy = new ColorSet();

	// Token: 0x040000B1 RID: 177
	[Header("Bloom")]
	[SerializeField]
	private float[] bloomIntensityInGrades = new float[]
	{
		0.6f,
		0.6f,
		0.8f,
		1f,
		1.2f
	};

	// Token: 0x040000B2 RID: 178
	[Header("Runes")]
	[SerializeField]
	private Rune[] prefabRunes = new Rune[0];
}
