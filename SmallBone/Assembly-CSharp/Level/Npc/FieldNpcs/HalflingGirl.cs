using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Data;
using FX;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005EA RID: 1514
	public class HalflingGirl : FieldNpc
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x0005C182 File Offset: 0x0005A382
		protected override NpcType _type
		{
			get
			{
				return this._npcType;
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0005C18C File Offset: 0x0005A38C
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 338118185 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0005C1F0 File Offset: 0x0005A3F0
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGiveFood());
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0005C22F File Offset: 0x0005A42F
		private IEnumerator CGiveFood()
		{
			yield return LetterBox.instance.CAppear(0.4f);
			yield return base.CGreeting();
			this._npcConversation.skippable = true;
			this._npcConversation.visible = false;
			this._animator.Play(HalflingGirl._castingHash, 0, 0f);
			this._dropEffect.Spawn(this._dropPosition.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dropSound, this._dropPosition.position);
			yield return new WaitForSeconds(this._givingAnimatonDuration1);
			this.PlaceFood();
			this._phase = FieldNpc.Phase.Gave;
			yield return new WaitForSeconds(this._givingAnimatonDuration2);
			this._animator.Play(FieldNpc._idleHash, 0, 0f);
			this._npcConversation.visible = true;
			yield return this._npcConversation.CConversation(base._confirmed);
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0005C240 File Offset: 0x0005A440
		private void PlaceFood()
		{
			Rarity rarity = this._abilityPossiblities.Evaluate();
			AbilityBuff abilityBuff = this._abilityBuffList.Take(this._random, rarity);
			AbilityBuff abilityBuff2 = UnityEngine.Object.Instantiate<AbilityBuff>(abilityBuff, this._dropPosition);
			abilityBuff2.name = abilityBuff.name;
			abilityBuff2.price = 0;
			abilityBuff2.Initialize();
		}

		// Token: 0x0400198E RID: 6542
		private const int _randomSeed = 338118185;

		// Token: 0x0400198F RID: 6543
		protected static readonly int _castingHash = Animator.StringToHash("Casting");

		// Token: 0x04001990 RID: 6544
		[SerializeField]
		private NpcType _npcType;

		// Token: 0x04001991 RID: 6545
		[SerializeField]
		[Space]
		private float _givingAnimatonDuration1;

		// Token: 0x04001992 RID: 6546
		[SerializeField]
		private float _givingAnimatonDuration2;

		// Token: 0x04001993 RID: 6547
		[SerializeField]
		[Header("Herb")]
		[FormerlySerializedAs("_foodList")]
		private AbilityBuffList _abilityBuffList;

		// Token: 0x04001994 RID: 6548
		[SerializeField]
		[FormerlySerializedAs("_foodPossibilities")]
		private RarityPossibilities _abilityPossiblities;

		// Token: 0x04001995 RID: 6549
		[Header("Drop")]
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x04001996 RID: 6550
		[SerializeField]
		private EffectInfo _dropEffect;

		// Token: 0x04001997 RID: 6551
		[SerializeField]
		private SoundInfo _dropSound;

		// Token: 0x04001998 RID: 6552
		private System.Random _random;
	}
}
