using System;
using System.Collections;
using Characters;
using Characters.Operations.Fx;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005FC RID: 1532
	public class SleepySkeleton : FieldNpc
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000076D4 File Offset: 0x000058D4
		protected override NpcType _type
		{
			get
			{
				return NpcType.SleepySkeleton;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0005D431 File Offset: 0x0005B631
		private int _healthPercentToTake
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHealthPercentCost;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0005AE76 File Offset: 0x00059076
		private RarityPossibilities _headPossibilities
		{
			get
			{
				return Singleton<Service>.Instance.levelManager.currentChapter.currentStage.fieldNpcSettings.sleepySekeletonHeadPossibilities;
			}
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0005D454 File Offset: 0x0005B654
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this.Load();
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged += this.Load;
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0005D4D6 File Offset: 0x0005B6D6
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= this.Load;
			WeaponRequest weaponRequest = this._weaponRequest;
			if (weaponRequest == null)
			{
				return;
			}
			weaponRequest.Release();
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0005D50C File Offset: 0x0005B70C
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

		// Token: 0x06001EBF RID: 7871 RVA: 0x0005D560 File Offset: 0x0005B760
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

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0005D5A1 File Offset: 0x0005B7A1
		private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			string[] scripts = (this._phase == FieldNpc.Phase.Initial) ? base._greeting : base._regreeting;
			this._phase = FieldNpc.Phase.Greeted;
			this._npcConversation.skippable = true;
			int lastIndex = scripts.Length - 1;
			int num;
			for (int i = 0; i < lastIndex; i = num + 1)
			{
				yield return this._npcConversation.CConversation(new string[]
				{
					scripts[i]
				});
				num = i;
			}
			this._npcConversation.skippable = true;
			this._npcConversation.body = ((confirmArg == null) ? scripts[lastIndex] : string.Format(scripts[lastIndex], confirmArg));
			yield return this._npcConversation.CType();
			yield return new WaitForSecondsRealtime(0.3f);
			this._npcConversation.OpenConfirmSelector(delegate
			{
				this.OnConfirmed(character);
			}, new Action(base.Close));
			yield break;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0005D5C0 File Offset: 0x0005B7C0
		private void OnConfirmed(Character character)
		{
			SleepySkeleton.<>c__DisplayClass20_0 CS$<>8__locals1 = new SleepySkeleton.<>c__DisplayClass20_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.character = character;
			base.StartCoroutine(CS$<>8__locals1.<OnConfirmed>g__CDropHead|0());
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0005D5EE File Offset: 0x0005B7EE
		private IEnumerator CDropWeapon()
		{
			while (!this._weaponRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.gearManager.onWeaponInstanceChanged -= this.Load;
			Singleton<Service>.Instance.levelManager.DropWeapon(this._weaponRequest, this._dropPosition.position);
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, base.transform.position);
			yield break;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0005D600 File Offset: 0x0005B800
		private void GiveDamage(Character character)
		{
			double num = character.health.maximumHealth * (double)this._healthPercentToTake * 0.01;
			if (Math.Floor(character.health.currentHealth) <= num)
			{
				num = character.health.currentHealth - 1.0;
			}
			this._vignette.Run(character);
			this._shaderEffect.Run(character);
			character.health.TakeHealth(num);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(num, character.transform.position);
		}

		// Token: 0x040019F0 RID: 6640
		private const int _randomSeed = 2028506624;

		// Token: 0x040019F1 RID: 6641
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x040019F2 RID: 6642
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x040019F3 RID: 6643
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x040019F4 RID: 6644
		[SerializeField]
		private Characters.Operations.Fx.Vignette _vignette;

		// Token: 0x040019F5 RID: 6645
		[SerializeField]
		private ShaderEffect _shaderEffect;

		// Token: 0x040019F6 RID: 6646
		private WeaponReference _weaponToDrop;

		// Token: 0x040019F7 RID: 6647
		private WeaponRequest _weaponRequest;

		// Token: 0x040019F8 RID: 6648
		private System.Random _random;
	}
}
