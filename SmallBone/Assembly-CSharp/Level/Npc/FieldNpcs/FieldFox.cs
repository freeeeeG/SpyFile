using System;
using System.Collections;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005D6 RID: 1494
	public sealed class FieldFox : FieldNpc
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x0005AE72 File Offset: 0x00059072
		protected override NpcType _type
		{
			get
			{
				return NpcType.FieldFox;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x0005AE76 File Offset: 0x00059076
		private RarityPossibilities _headPossibilities
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHeadPossibilities;
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0005AE98 File Offset: 0x00059098
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this.Load();
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged += this.Load;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0005AF1A File Offset: 0x0005911A
		protected override void OnDestroy()
		{
			base.OnDestroy();
			WeaponRequest weaponRequest = this._weaponRequest;
			if (weaponRequest != null)
			{
				weaponRequest.Release();
			}
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= this.Load;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0005AF50 File Offset: 0x00059150
		private void Load()
		{
			do
			{
				Rarity rarity = this._headPossibilities.Evaluate(this._random);
				this._weaponToDrop = Singleton<Service>.Instance.gearManager.GetWeaponToTake(this._random, rarity);
			}
			while (this._weaponToDrop == null);
			this._weaponRequest = this._weaponToDrop.LoadAsync();
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0005AFA4 File Offset: 0x000591A4
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGreetingAndConfirm(character, null));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0005AFE5 File Offset: 0x000591E5
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			this._npcConversation.skippable = true;
			int lastIndex = 3;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			base.StartCoroutine(this.CDropWeapon());
			this._phase = FieldNpc.Phase.Gave;
			for (int i = lastIndex; i < base._greeting.Length; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					base._greeting[i]
				});
				num = i;
			}
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0005AFF4 File Offset: 0x000591F4
		private IEnumerator CDropWeapon()
		{
			while (!this._weaponRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= this.Load;
			Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest, this._dropPosition.position);
			int num = (int)this._currenyAmount.value;
			Singleton<Service>.Instance.levelManager.DropBone(num, Mathf.Min(num, 10));
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, base.transform.position);
			yield break;
		}

		// Token: 0x0400193C RID: 6460
		private const int _randomSeed = 2028506624;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x0400193E RID: 6462
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private CustomFloat _currenyAmount;

		// Token: 0x04001941 RID: 6465
		private WeaponReference _weaponToDrop;

		// Token: 0x04001942 RID: 6466
		private WeaponRequest _weaponRequest;

		// Token: 0x04001943 RID: 6467
		private System.Random _random;
	}
}
