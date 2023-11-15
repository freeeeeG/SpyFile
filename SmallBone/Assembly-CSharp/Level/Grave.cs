using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Data;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004E6 RID: 1254
	public class Grave : InteractiveObject, ILootable
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x0004CA5E File Offset: 0x0004AC5E
		public Rarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001884 RID: 6276 RVA: 0x0004CA66 File Offset: 0x0004AC66
		// (set) Token: 0x06001885 RID: 6277 RVA: 0x0004CA6E File Offset: 0x0004AC6E
		public bool looted { get; private set; }

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001886 RID: 6278 RVA: 0x0004CA78 File Offset: 0x0004AC78
		// (remove) Token: 0x06001887 RID: 6279 RVA: 0x0004CAB0 File Offset: 0x0004ACB0
		public event Action onLoot;

		// Token: 0x06001888 RID: 6280 RVA: 0x0004CAE8 File Offset: 0x0004ACE8
		protected override void Awake()
		{
			base.Awake();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 925585528 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0004CB4C File Offset: 0x0004AD4C
		private void Start()
		{
			this._rarity = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.gearPossibilities.Evaluate(this._random);
			switch (this._rarity)
			{
			case Rarity.Common:
				this._animator.runtimeAnimatorController = this._common;
				break;
			case Rarity.Rare:
				this._animator.runtimeAnimatorController = this._rare;
				break;
			case Rarity.Unique:
				this._animator.runtimeAnimatorController = this._unique;
				break;
			case Rarity.Legendary:
				this._animator.runtimeAnimatorController = this._legendary;
				break;
			}
			this.Load();
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0004CBF2 File Offset: 0x0004ADF2
		private void OnDestroy()
		{
			WeaponRequest weaponRequest = this._weaponRequest;
			if (weaponRequest == null)
			{
				return;
			}
			weaponRequest.Release();
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0004CC04 File Offset: 0x0004AE04
		private void Load()
		{
			do
			{
				this.EvaluateGearRarity();
				this._weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, this._gearRarity);
			}
			while (this._weaponToDrop == null);
			this._weaponRequest = this._weaponToDrop.LoadAsync();
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0004CC51 File Offset: 0x0004AE51
		private void EvaluateGearRarity()
		{
			this._gearRarity = Settings.instance.containerPossibilities[this._rarity].Evaluate(this._random);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0004CC79 File Offset: 0x0004AE79
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
			this._effect.Play(this._rarity);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0004CCA2 File Offset: 0x0004AEA2
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0004CCBA File Offset: 0x0004AEBA
		public override void InteractWith(Character character)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(this.<InteractWith>g__CDrop|29_0());
			base.Deactivate();
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0004CCEB File Offset: 0x0004AEEB
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDrop|29_0()
		{
			float delay = 0.4f;
			delay += Time.unscaledTime;
			while (!this._weaponRequest.isDone)
			{
				yield return null;
			}
			delay -= Time.unscaledTime;
			if (delay > 0f)
			{
				yield return Chronometer.global.WaitForSeconds(delay);
			}
			Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest, base.transform.position);
			Action action = this.onLoot;
			if (action != null)
			{
				action();
			}
			this.looted = true;
			yield break;
		}

		// Token: 0x04001550 RID: 5456
		private const int _randomSeed = 925585528;

		// Token: 0x04001551 RID: 5457
		private const float _delayToDrop = 0.4f;

		// Token: 0x04001552 RID: 5458
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x04001553 RID: 5459
		[SerializeField]
		private RuntimeAnimatorController _common;

		// Token: 0x04001554 RID: 5460
		[SerializeField]
		private RuntimeAnimatorController _rare;

		// Token: 0x04001555 RID: 5461
		[SerializeField]
		private RuntimeAnimatorController _unique;

		// Token: 0x04001556 RID: 5462
		[SerializeField]
		private RuntimeAnimatorController _legendary;

		// Token: 0x04001557 RID: 5463
		[SerializeField]
		private RewardEffect _effect;

		// Token: 0x04001558 RID: 5464
		private Rarity _rarity;

		// Token: 0x04001559 RID: 5465
		private Rarity _gearRarity;

		// Token: 0x0400155A RID: 5466
		private WeaponReference _weaponToDrop;

		// Token: 0x0400155B RID: 5467
		private WeaponRequest _weaponRequest;

		// Token: 0x0400155C RID: 5468
		private System.Random _random;
	}
}
