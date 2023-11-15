using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A0 RID: 1184
	public class Chest : InteractiveObject, ILootable
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060016A2 RID: 5794 RVA: 0x00047504 File Offset: 0x00045704
		// (remove) Token: 0x060016A3 RID: 5795 RVA: 0x0004753C File Offset: 0x0004573C
		public event Action onLoot;

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00047571 File Offset: 0x00045771
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x00047579 File Offset: 0x00045779
		public bool looted { get; private set; }

		// Token: 0x060016A6 RID: 5798 RVA: 0x00047584 File Offset: 0x00045784
		protected override void Awake()
		{
			base.Awake();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x000475E5 File Offset: 0x000457E5
		private void Start()
		{
			this.Initialize();
			this.Load();
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.Load;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00047610 File Offset: 0x00045810
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

		// Token: 0x060016A9 RID: 5801 RVA: 0x00047670 File Offset: 0x00045870
		private void Load()
		{
			this._toDropReferences.Clear();
			this.ReleaseRequests();
			this._toDropRequests.Clear();
			int num = 50;
			int num2 = 0;
			for (int i = 0; i < 3; i++)
			{
				GearReference gearReference;
				do
				{
					num2++;
					this.EvaluateGearRarity();
					gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._gearRarity);
					if (gearReference != null)
					{
						using (List<GearReference>.Enumerator enumerator = this._toDropReferences.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.name.Equals(gearReference.name))
								{
									gearReference = null;
									break;
								}
							}
						}
						if (num2 >= num)
						{
							Debug.LogError("######## 이 에러가 나오면 개발팀에 알려주세요.");
						}
					}
				}
				while (gearReference == null && num2 < num);
				this._toDropReferences.Add(gearReference);
				this._toDropRequests.Add(gearReference.LoadAsync());
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00047764 File Offset: 0x00045964
		private void Initialize()
		{
			this._rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(this._random);
			switch (this._rarity)
			{
			case Rarity.Common:
				this._animator.runtimeAnimatorController = this._commonChest;
				break;
			case Rarity.Rare:
				this._animator.runtimeAnimatorController = this._rareChest;
				break;
			case Rarity.Unique:
				this._animator.runtimeAnimatorController = this._uniqueChest;
				break;
			case Rarity.Legendary:
				this._animator.runtimeAnimatorController = this._legendaryChest;
				break;
			}
			this._toDropRequests = new List<GearRequest>(3);
			this._toDropReferences = new List<GearReference>(3);
			this._rewards = new Gear[3];
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00047828 File Offset: 0x00045A28
		private void EvaluateGearRarity()
		{
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00047850 File Offset: 0x00045A50
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00047868 File Offset: 0x00045A68
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00047880 File Offset: 0x00045A80
		public override void InteractWith(Character character)
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(this.<InteractWith>g__CDelayedDrop|32_0());
			base.Deactivate();
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000478D8 File Offset: 0x00045AD8
		private void OnDiscard(Gear gear)
		{
			Array.Sort<Gear>(this._rewards, delegate(Gear gear1, Gear gear2)
			{
				if (gear1.rarity < gear2.rarity)
				{
					return -1;
				}
				return 1;
			});
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (!flag && this._rewards[i].type != Gear.Type.Quintessence)
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
				if (this._rewards[i].gameObject != null && this._rewards[i].state == Gear.State.Dropped)
				{
					this._rewards[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x000479CC File Offset: 0x00045BCC
		private void OnLoot(Character character)
		{
			for (int i = 0; i < 3; i++)
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

		// Token: 0x060016B1 RID: 5809 RVA: 0x00047A4C File Offset: 0x00045C4C
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.Load;
			this.ReleaseRequests();
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00047A77 File Offset: 0x00045C77
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDelayedDrop|32_0()
		{
			List<DropMovement> dropMovements = new List<DropMovement>();
			float elapsed = 0f;
			int num;
			for (int i = 0; i < 3; i = num + 1)
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
			while (elapsed <= 0.5f)
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
			}
			for (int j = 0; j < 3; j++)
			{
				GearRequest gearRequest = this._toDropRequests[j];
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(gearRequest, base.transform.position);
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

		// Token: 0x040013DB RID: 5083
		private const int rewardCount = 3;

		// Token: 0x040013DC RID: 5084
		private const int _randomSeed = 2028506624;

		// Token: 0x040013DD RID: 5085
		private const float _delayToDrop = 0.5f;

		// Token: 0x040013DE RID: 5086
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x040013DF RID: 5087
		[SerializeField]
		private RuntimeAnimatorController _commonChest;

		// Token: 0x040013E0 RID: 5088
		[SerializeField]
		private RuntimeAnimatorController _rareChest;

		// Token: 0x040013E1 RID: 5089
		[SerializeField]
		private RuntimeAnimatorController _uniqueChest;

		// Token: 0x040013E2 RID: 5090
		[SerializeField]
		private RuntimeAnimatorController _legendaryChest;

		// Token: 0x040013E3 RID: 5091
		private Gear[] _rewards;

		// Token: 0x040013E4 RID: 5092
		private List<GearRequest> _toDropRequests;

		// Token: 0x040013E5 RID: 5093
		private List<GearReference> _toDropReferences;

		// Token: 0x040013E6 RID: 5094
		private Rarity _rarity;

		// Token: 0x040013E7 RID: 5095
		private Rarity _gearRarity;

		// Token: 0x040013E8 RID: 5096
		private ItemReference _itemToDrop;

		// Token: 0x040013E9 RID: 5097
		private ItemRequest _itemRequest;

		// Token: 0x040013EA RID: 5098
		private int _discardCount;

		// Token: 0x040013EB RID: 5099
		private System.Random _random;
	}
}
