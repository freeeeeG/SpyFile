using System;
using Data;
using FX;
using GameResources;
using Hardmode.Darktech;
using Level;
using Singletons;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class MapReward : MonoBehaviour
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000190 RID: 400 RVA: 0x00007894 File Offset: 0x00005A94
	// (remove) Token: 0x06000191 RID: 401 RVA: 0x000078CC File Offset: 0x00005ACC
	public event Action onLoot;

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000192 RID: 402 RVA: 0x00007901 File Offset: 0x00005B01
	public bool hasReward
	{
		get
		{
			return this._rewardPrefabs[this.type] != null;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000193 RID: 403 RVA: 0x0000791A File Offset: 0x00005B1A
	// (set) Token: 0x06000194 RID: 404 RVA: 0x00007922 File Offset: 0x00005B22
	public bool activated { get; set; }

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000195 RID: 405 RVA: 0x0000792B File Offset: 0x00005B2B
	public bool isHardmodeItem
	{
		get
		{
			return this.type == MapReward.Type.Item && GameData.HardmodeProgress.hardmode;
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000793D File Offset: 0x00005B3D
	private void Awake()
	{
		UnityEngine.Object.Destroy(this._preview);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000794C File Offset: 0x00005B4C
	public void LoadReward()
	{
		GameObject gameObject = this._rewardPrefabs[this.type];
		if (this.isHardmodeItem)
		{
			gameObject = CommonResource.instance.hardmodeChest;
		}
		if (!(gameObject == null))
		{
			this._reward = UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position, Quaternion.identity, base.transform);
			this._reward.gameObject.SetActive(false);
			if (this.isHardmodeItem)
			{
				HardmodeChest component = this._reward.GetComponent<HardmodeChest>();
				if (component != null)
				{
					component.TryToChangeOmenChest();
				}
				if (component.isOmenChest)
				{
					this._spawnEffect.animation = Singleton<DarktechManager>.Instance.resource.omenChestSpawnEffect;
				}
			}
			this._reward.GetComponent<ILootable>().onLoot += delegate()
			{
				Action action2 = this.onLoot;
				if (action2 == null)
				{
					return;
				}
				action2();
			};
			return;
		}
		Action action = this.onLoot;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00007A30 File Offset: 0x00005C30
	public bool Activate()
	{
		this.activated = true;
		if (this._reward == null)
		{
			return false;
		}
		this._reward.gameObject.SetActive(true);
		this._reward.GetComponent<ILootable>().Activate();
		this._spawnEffect.Spawn(this._spawnEffectPosition.position, 0f, 1f);
		return true;
	}

	// Token: 0x04000154 RID: 340
	[NonSerialized]
	public MapReward.Type type;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private SpriteRenderer _preview;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private MapReward.RewardTypeGameObjectArray _rewardPrefabs;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private EffectInfo _spawnEffect;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private Transform _spawnEffectPosition;

	// Token: 0x0400015A RID: 346
	private GameObject _reward;

	// Token: 0x02000054 RID: 84
	[Serializable]
	public class RewardTypeGameObjectArray : EnumArray<MapReward.Type, GameObject>
	{
	}

	// Token: 0x02000055 RID: 85
	public enum Type
	{
		// Token: 0x0400015C RID: 348
		None,
		// Token: 0x0400015D RID: 349
		Gold,
		// Token: 0x0400015E RID: 350
		Head,
		// Token: 0x0400015F RID: 351
		Item,
		// Token: 0x04000160 RID: 352
		Adventurer,
		// Token: 0x04000161 RID: 353
		Boss
	}
}
