using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear;
using Characters.Operations.Fx;
using Data;
using FX;
using GameResources;
using Hardmode.Darktech;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level
{
	// Token: 0x020004EA RID: 1258
	public class HardmodeChest : InteractiveObject, ILootable
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060018A9 RID: 6313 RVA: 0x0004CFFC File Offset: 0x0004B1FC
		// (remove) Token: 0x060018AA RID: 6314 RVA: 0x0004D034 File Offset: 0x0004B234
		public event Action onLoot;

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x0004D069 File Offset: 0x0004B269
		// (set) Token: 0x060018AC RID: 6316 RVA: 0x0004D071 File Offset: 0x0004B271
		public bool looted { get; private set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0004D07A File Offset: 0x0004B27A
		public bool isOmenChest
		{
			get
			{
				return this._isOmenChest;
			}
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0004D084 File Offset: 0x0004B284
		public bool TryToChangeOmenChest()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			if (Singleton<DarktechManager>.Instance.IsActivated(DarktechData.Type.OmenAmplifier))
			{
				int stageIndex = currentChapter.stageIndex;
				float value = Singleton<DarktechManager>.Instance.setting.흉조증폭기확률[new ValueTuple<Chapter.Type, float>(currentChapter.type, (float)stageIndex)].value;
				this._isOmenChest = MMMaths.Chance(this._random, value);
			}
			if (this._isOmenChest)
			{
				this._cat.transform.parent = Map.Instance.transform;
			}
			this._alreadyTryToChangeOmenChest = true;
			return this._isOmenChest;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0004D15B File Offset: 0x0004B35B
		private void Start()
		{
			if (!this._alreadyTryToChangeOmenChest)
			{
				this.TryToChangeOmenChest();
			}
			this.Initialize();
			this.Load();
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.Load;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0004D194 File Offset: 0x0004B394
		private void Initialize()
		{
			this._rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(this._random);
			switch (this._rarity)
			{
			case Rarity.Common:
				if (this._isOmenChest)
				{
					this._craw.transform.position = this._commonCrawPosition.position;
					this._animator.runtimeAnimatorController = this._commonOmenChest;
				}
				else
				{
					this._animator.runtimeAnimatorController = this._commonChest;
				}
				break;
			case Rarity.Rare:
				if (this._isOmenChest)
				{
					this._craw.transform.position = this._rareCrawPosition.position;
					this._animator.runtimeAnimatorController = this._rareOmenChest;
				}
				else
				{
					this._animator.runtimeAnimatorController = this._rareChest;
				}
				break;
			case Rarity.Unique:
				if (this._isOmenChest)
				{
					this._craw.transform.position = this._uniqueCrawPosition.position;
					this._animator.runtimeAnimatorController = this._uniqueOmenChest;
				}
				else
				{
					this._animator.runtimeAnimatorController = this._uniqueChest;
				}
				break;
			case Rarity.Legendary:
				if (this._isOmenChest)
				{
					this._craw.transform.position = this._legendaryCrawPosition.position;
					this._animator.runtimeAnimatorController = this._legendaryOmenChest;
				}
				else
				{
					this._animator.runtimeAnimatorController = this._legendaryChest;
				}
				break;
			}
			this._omenObejct.SetActive(this._isOmenChest);
			this._rewardCount = (this._isOmenChest ? this._omenChestItemCount : this._normalChestItemCount);
			this._toDropRequests = new List<GearRequest>(this._rewardCount);
			this._toDropReferences = new List<GearReference>(this._rewardCount);
			this._rewards = new Gear[this._rewardCount];
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0004D37C File Offset: 0x0004B57C
		private void ReleaseRequests()
		{
			if (this._toDropRequests == null)
			{
				return;
			}
			foreach (GearRequest gearRequest in this._toDropRequests)
			{
				if (gearRequest != null)
				{
					gearRequest.Release();
				}
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0004D3DC File Offset: 0x0004B5DC
		private void Load()
		{
			this._toDropReferences.Clear();
			this.ReleaseRequests();
			this._toDropRequests.Clear();
			int num = this._rewardCount;
			int num2 = 50;
			int num3 = 0;
			if (this._isOmenChest)
			{
				num--;
				GearReference omenItems;
				do
				{
					this.EvaluateGearRarity();
					omenItems = Singleton<Service>.Instance.gearManager.GetOmenItems(this._random);
					num3++;
					if (num3 >= num2)
					{
						Debug.LogError("######## 이 에러가 나오면 개발팀에 알려주세요. 흉조");
					}
				}
				while (omenItems == null && num3 < num2);
				if (omenItems != null)
				{
					this._toDropRequests.Add(omenItems.LoadAsync());
				}
			}
			num3 = 0;
			for (int i = 0; i < num; i++)
			{
				GearReference gearReference;
				do
				{
					num3++;
					this.EvaluateGearRarity();
					gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._gearRarity);
					if (gearReference != null)
					{
						foreach (GearReference gearReference2 in this._toDropReferences)
						{
							if (gearReference2 == null || gearReference == null)
							{
								Debug.Log("gear is null");
							}
							else if (gearReference2.name.Equals(gearReference.name))
							{
								gearReference = null;
								break;
							}
						}
						if (num3 >= num2)
						{
							Debug.LogError("######## 이 에러가 나오면 개발팀에 알려주세요. 일반");
						}
					}
				}
				while (gearReference == null && num3 < num2);
				this._toDropReferences.Add(gearReference);
				this._toDropRequests.Add(gearReference.LoadAsync());
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0004D550 File Offset: 0x0004B750
		private void EvaluateGearRarity()
		{
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0004D578 File Offset: 0x0004B778
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
			if (this._isOmenChest)
			{
				this._omenChestSound.Run(Singleton<Service>.Instance.levelManager.player);
			}
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0004D5B4 File Offset: 0x0004B7B4
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
			if (this._isOmenChest)
			{
				this._flySound.Run(Singleton<Service>.Instance.levelManager.player);
			}
			this._craw.enabled = true;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0004D608 File Offset: 0x0004B808
		public override void InteractWith(Character character)
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(this.<InteractWith>g__CDelayedDrop|50_0());
			base.Deactivate();
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0004D660 File Offset: 0x0004B860
		private void OnDiscard(Gear gear)
		{
			Array.Sort<Gear>(this._rewards, delegate(Gear gear1, Gear gear2)
			{
				if (gear1.type != gear2.type)
				{
					if (gear1.type == Gear.Type.Item)
					{
						return 1;
					}
					if (gear2.type == Gear.Type.Item)
					{
						return -1;
					}
				}
				if (gear1.rarity < gear2.rarity)
				{
					return -1;
				}
				return 1;
			});
			bool flag = false;
			for (int i = 0; i < this._rewardCount; i++)
			{
				if (!flag)
				{
					this._discardCount++;
					flag = true;
				}
				if (this._discardCount >= 2)
				{
					this._rewards[i].destructible = false;
					this._rewards[i].onDiscard -= this.OnDiscard;
				}
				this._rewards[i].dropped.onLoot -= this.OnLoot;
				if (this._rewards[i].state == Gear.State.Dropped && this._rewards[i] != null && this._rewards[i].gameObject != null)
				{
					this._rewards[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0004D758 File Offset: 0x0004B958
		private void OnLoot(Character character)
		{
			for (int i = 0; i < this._rewardCount; i++)
			{
				this._rewards[i].dropped.onLoot -= this.OnLoot;
				this._rewards[i].onDiscard -= this.OnDiscard;
				if (this._rewards[i].state == Gear.State.Dropped)
				{
					this._rewards[i].destructible = false;
					this._rewards[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0004D7DD File Offset: 0x0004B9DD
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			this.ReleaseRequests();
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0004D808 File Offset: 0x0004BA08
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDelayedDrop|50_0()
		{
			List<DropMovement> dropMovements = new List<DropMovement>();
			float elapsed = 0f;
			int num;
			for (int i = 0; i < this._rewardCount; i = num + 1)
			{
				GearRequest request = this._toDropRequests[i];
				while (!request.isDone)
				{
					elapsed += Chronometer.global.deltaTime;
					yield return null;
				}
				request = null;
				num = i;
			}
			while (elapsed < 0.5f)
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			for (int j = 0; j < this._rewardCount; j++)
			{
				GearRequest gearRequest = this._toDropRequests[j];
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequest, this._dropPoint.transform.position);
				gear.dropped.dropMovement.Pause();
				gear.dropped.onLoot += this.OnLoot;
				gear.onDiscard += this.OnDiscard;
				dropMovements.Add(gear.dropped.dropMovement);
				this._rewards[j] = gear;
				gear.dropped.gameObject.AddComponent<BossRewardEffect>();
				gear.dropped.additionalPopupUIOffsetY = float.MaxValue;
				gear.dropped.dropMovement.SetMultipleRewardMovement(1f);
			}
			foreach (DropMovement dropMovement in dropMovements)
			{
				dropMovement.Move();
			}
			DropMovement.SetMultiDropHorizontalInterval(dropMovements);
			Action action = this.onLoot;
			if (action != null)
			{
				action();
			}
			this.looted = true;
			yield break;
		}

		// Token: 0x0400156F RID: 5487
		private const int _randomSeed = 2028506624;

		// Token: 0x04001570 RID: 5488
		private const float _delayToDrop = 0.5f;

		// Token: 0x04001571 RID: 5489
		[Header("흉조 아이템")]
		[SerializeField]
		private int _normalChestItemCount;

		// Token: 0x04001572 RID: 5490
		[SerializeField]
		private int _omenChestItemCount;

		// Token: 0x04001573 RID: 5491
		[SerializeField]
		private RuntimeAnimatorController _commonOmenChest;

		// Token: 0x04001574 RID: 5492
		[SerializeField]
		private RuntimeAnimatorController _rareOmenChest;

		// Token: 0x04001575 RID: 5493
		[SerializeField]
		private RuntimeAnimatorController _uniqueOmenChest;

		// Token: 0x04001576 RID: 5494
		[SerializeField]
		private RuntimeAnimatorController _legendaryOmenChest;

		// Token: 0x04001577 RID: 5495
		[SerializeField]
		private Transform _commonCrawPosition;

		// Token: 0x04001578 RID: 5496
		[SerializeField]
		private Transform _rareCrawPosition;

		// Token: 0x04001579 RID: 5497
		[SerializeField]
		private Transform _uniqueCrawPosition;

		// Token: 0x0400157A RID: 5498
		[SerializeField]
		private Transform _legendaryCrawPosition;

		// Token: 0x0400157B RID: 5499
		[SerializeField]
		private GameObject _omenObejct;

		// Token: 0x0400157C RID: 5500
		[SerializeField]
		private GameObject _cat;

		// Token: 0x0400157D RID: 5501
		[SerializeField]
		private ReactiveProp _craw;

		// Token: 0x0400157E RID: 5502
		[Header("기본 설정")]
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x0400157F RID: 5503
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001580 RID: 5504
		[SerializeField]
		private RuntimeAnimatorController _commonChest;

		// Token: 0x04001581 RID: 5505
		[SerializeField]
		private RuntimeAnimatorController _rareChest;

		// Token: 0x04001582 RID: 5506
		[SerializeField]
		private RuntimeAnimatorController _uniqueChest;

		// Token: 0x04001583 RID: 5507
		[SerializeField]
		private RuntimeAnimatorController _legendaryChest;

		// Token: 0x04001584 RID: 5508
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _omenChestSound;

		// Token: 0x04001585 RID: 5509
		[SerializeField]
		[Subcomponent(typeof(PlaySound))]
		private PlaySound _flySound;

		// Token: 0x04001586 RID: 5510
		private Rarity _rarity;

		// Token: 0x04001587 RID: 5511
		private Rarity _gearRarity;

		// Token: 0x04001588 RID: 5512
		private Gear[] _rewards;

		// Token: 0x04001589 RID: 5513
		private List<GearRequest> _toDropRequests;

		// Token: 0x0400158A RID: 5514
		private List<GearReference> _toDropReferences;

		// Token: 0x0400158B RID: 5515
		private System.Random _random;

		// Token: 0x0400158E RID: 5518
		private int _rewardCount;

		// Token: 0x0400158F RID: 5519
		private int _discardCount;

		// Token: 0x04001590 RID: 5520
		private bool _isOmenChest;

		// Token: 0x04001591 RID: 5521
		private bool _alreadyTryToChangeOmenChest;
	}
}
