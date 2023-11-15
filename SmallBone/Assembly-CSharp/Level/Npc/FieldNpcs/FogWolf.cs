using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Abilities.Savable;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005E8 RID: 1512
	public class FogWolf : FieldNpc
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x00018EC5 File Offset: 0x000170C5
		protected override NpcType _type
		{
			get
			{
				return NpcType.FogWolf;
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0005BE18 File Offset: 0x0005A018
		protected override void Start()
		{
			base.Start();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 2028506624 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0005BE7C File Offset: 0x0005A07C
		protected override void Interact(Character character)
		{
			base.Interact(character);
			FieldNpc.Phase phase = this._phase;
			if (phase <= FieldNpc.Phase.Greeted)
			{
				base.StartCoroutine(this.CGiveBuff(character));
				return;
			}
			if (phase != FieldNpc.Phase.Gave)
			{
				return;
			}
			base.StartCoroutine(base.CChat());
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0005BEBC File Offset: 0x0005A0BC
		private IEnumerator CGiveBuff(Character character)
		{
			yield return LetterBox.instance.CAppear(0.4f);
			yield return base.CGreeting();
			int buffIndex = this._random.Next(0, FogWolfBuff.buffCount);
			character.playerComponents.savableAbilityManager.Apply(SavableAbilityManager.Name.FogWolfBuff, buffIndex);
			this._phase = FieldNpc.Phase.Gave;
			this._npcConversation.skippable = true;
			this._givingBuffEffect.Spawn(base.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._givingBuffSound, base.transform.position);
			yield return this._npcConversation.visible = false;
			yield return new WaitForSeconds(this._effectShowingDuration);
			yield return this._npcConversation.visible = true;
			Vector2 v = new Vector2(character.collider.bounds.center.x, character.collider.bounds.max.y + 0.5f);
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(Localization.GetLocalizedString(this._floatingKeyArray[buffIndex]), v, "#F2F2F2");
			this._takingBuffEffect.Spawn(character.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._takingBuffSound, character.transform.position);
			yield return this._npcConversation.CConversation(new string[]
			{
				base._confirmed[buffIndex]
			});
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x04001981 RID: 6529
		private const int _randomSeed = 2028506624;

		// Token: 0x04001982 RID: 6530
		[Header("Effects")]
		[Tooltip("버프 부여 후 이 시간 동안 대화창이 잠시 사라집니다.")]
		[SerializeField]
		private float _effectShowingDuration;

		// Token: 0x04001983 RID: 6531
		[SerializeField]
		private EffectInfo _givingBuffEffect;

		// Token: 0x04001984 RID: 6532
		[SerializeField]
		private SoundInfo _givingBuffSound;

		// Token: 0x04001985 RID: 6533
		[SerializeField]
		private EffectInfo _takingBuffEffect;

		// Token: 0x04001986 RID: 6534
		[SerializeField]
		private SoundInfo _takingBuffSound;

		// Token: 0x04001987 RID: 6535
		[SerializeField]
		[Header("Buffs")]
		private string[] _floatingKeyArray = new string[5];

		// Token: 0x04001988 RID: 6536
		private System.Random _random;
	}
}
