using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using GameResources;
using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Specials
{
	// Token: 0x0200062E RID: 1582
	public class TimeCostEventReward : InteractiveObject
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x000605A8 File Offset: 0x0005E7A8
		public Rarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000605B0 File Offset: 0x0005E7B0
		private void Start()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1281776104 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._rarity = this._rarityPossibilities.Evaluate(this._random);
			this.EvaluateGearRarity();
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
			this.Load();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00060697 File Offset: 0x0005E897
		private void OnDestroy()
		{
			ItemRequest itemRequest = this._itemRequest;
			if (itemRequest == null)
			{
				return;
			}
			itemRequest.Release();
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000606AC File Offset: 0x0005E8AC
		private void Load()
		{
			do
			{
				this.EvaluateGearRarity();
				this._itemToDrop = Singleton<Service>.Instance.gearManager.GetItemToTake(this._random, this._gearRarity);
			}
			while (this._itemToDrop == null);
			this._itemRequest = this._itemToDrop.LoadAsync();
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000606F9 File Offset: 0x0005E8F9
		private void EvaluateGearRarity()
		{
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00060721 File Offset: 0x0005E921
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00060739 File Offset: 0x0005E939
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x00060754 File Offset: 0x0005E954
		public override void InteractWith(Character character)
		{
			if (!this._trigger.IsSatisfied())
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(this.<InteractWith>g__CDelayedDrop|24_0());
			this._runnable.Run();
			base.Deactivate();
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000607A9 File Offset: 0x0005E9A9
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDelayedDrop|24_0()
		{
			float delay = 0.5f;
			delay += Time.unscaledTime;
			while (!this._itemRequest.isDone)
			{
				yield return null;
			}
			delay -= Time.unscaledTime;
			if (delay > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(delay);
			}
			Singleton<Service>.Instance.levelManager.DropItem(this._itemRequest, this._dropPoint.position);
			yield break;
		}

		// Token: 0x04001AE5 RID: 6885
		private const int _randomSeed = 1281776104;

		// Token: 0x04001AE6 RID: 6886
		private const float _delayToDrop = 0.5f;

		// Token: 0x04001AE7 RID: 6887
		[SerializeField]
		private Transform _dropPoint;

		// Token: 0x04001AE8 RID: 6888
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001AE9 RID: 6889
		[SerializeField]
		private RuntimeAnimatorController _commonChest;

		// Token: 0x04001AEA RID: 6890
		[SerializeField]
		private RuntimeAnimatorController _rareChest;

		// Token: 0x04001AEB RID: 6891
		[SerializeField]
		private RuntimeAnimatorController _uniqueChest;

		// Token: 0x04001AEC RID: 6892
		[SerializeField]
		private RuntimeAnimatorController _legendaryChest;

		// Token: 0x04001AED RID: 6893
		[SerializeField]
		private RarityPossibilities _rarityPossibilities;

		// Token: 0x04001AEE RID: 6894
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04001AEF RID: 6895
		[SerializeField]
		private Runnable _runnable;

		// Token: 0x04001AF0 RID: 6896
		private Rarity _rarity;

		// Token: 0x04001AF1 RID: 6897
		private Rarity _gearRarity;

		// Token: 0x04001AF2 RID: 6898
		private ItemReference _itemToDrop;

		// Token: 0x04001AF3 RID: 6899
		private ItemRequest _itemRequest;

		// Token: 0x04001AF4 RID: 6900
		private System.Random _random;
	}
}
