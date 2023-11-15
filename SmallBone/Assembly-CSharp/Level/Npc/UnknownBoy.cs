using System;
using System.Collections;
using Characters;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005C0 RID: 1472
	public class UnknownBoy : InteractiveObject
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001D43 RID: 7491 RVA: 0x0005984A File Offset: 0x00057A4A
		private string displayName
		{
			get
			{
				return Localization.GetLocalizedString("npc/essence/unknownboy/name");
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x00059856 File Offset: 0x00057A56
		private string[] questScripts
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/essence/unknownboy/quest/0");
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00059862 File Offset: 0x00057A62
		private string[] rewardScripts
		{
			get
			{
				return Localization.GetLocalizedStringArray("npc/essence/unknownboy/reward/0");
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001D46 RID: 7494 RVA: 0x0005986E File Offset: 0x00057A6E
		private string[] chatScripts
		{
			get
			{
				return Localization.GetLocalizedStringArrays("npc/essence/unknownboy/chat").Random<string[]>();
			}
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x00059880 File Offset: 0x00057A80
		protected override void Awake()
		{
			base.Awake();
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._npcConversation.name = this.displayName;
			this._npcConversation.skippable = true;
			this._npcConversation.portrait = null;
			this._questState = UnknownBoy.QuestState.Wait;
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000598D8 File Offset: 0x00057AD8
		private void Start()
		{
			this._essenceRequest = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(this._essencePossibilities.Evaluate()).LoadAsync();
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000598FF File Offset: 0x00057AFF
		private void OnDestroy()
		{
			EssenceRequest essenceRequest = this._essenceRequest;
			if (essenceRequest == null)
			{
				return;
			}
			essenceRequest.Release();
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00059914 File Offset: 0x00057B14
		public override void InteractWith(Character character)
		{
			if (this._questState == UnknownBoy.QuestState.Cleared)
			{
				base.StartCoroutine(this.CRewardConversation());
				return;
			}
			if (this._questState == UnknownBoy.QuestState.Wait)
			{
				base.StartCoroutine(this.CAcceptQuest());
				return;
			}
			if (this._questState == UnknownBoy.QuestState.GaveReward)
			{
				base.StartCoroutine(this.CChat());
			}
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00059964 File Offset: 0x00057B64
		private IEnumerator CAcceptQuest()
		{
			LetterBox.instance.Appear(0.4f);
			yield return this._npcConversation.CConversation(this.questScripts);
			LetterBox.instance.Disappear(0.4f);
			this._questState = UnknownBoy.QuestState.Accepted;
			this._startWave.Spawn(true);
			base.StartCoroutine(this.CCheckWaveAllCleared());
			yield break;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00059973 File Offset: 0x00057B73
		private void OnDisable()
		{
			this._npcConversation.portrait = null;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00059981 File Offset: 0x00057B81
		private IEnumerator CCheckWaveAllCleared()
		{
			EnemyWave[] waves = Map.Instance.waveContainer.enemyWaves;
			Transform player = Singleton<Service>.Instance.levelManager.player.transform;
			this._collider.enabled = false;
			for (;;)
			{
				if (base.transform.position.x < player.position.x)
				{
					this._body.localScale = new Vector2(-1f, 1f);
				}
				else
				{
					this._body.localScale = new Vector2(1f, 1f);
				}
				bool flag = true;
				EnemyWave[] array = waves;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].state != Wave.State.Cleared)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				yield return null;
			}
			this._questState = UnknownBoy.QuestState.Cleared;
			this._inEffectInfo.Spawn(base.transform.position, 0f, 1f);
			base.transform.position = this._rewardConversationPoint.transform.position;
			this._outEffectInfo.Spawn(base.transform.position, 0f, 1f);
			this._body.localScale = Vector3.one;
			this._collider.enabled = true;
			this._spriteRenderer.sortingLayerName = "Enemy";
			this._lineText.SetActive(false);
			yield break;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00059990 File Offset: 0x00057B90
		private IEnumerator CChat()
		{
			LetterBox.instance.Appear(0.4f);
			yield return this._npcConversation.CConversation(new string[]
			{
				this.chatScripts.Random<string>()
			});
			LetterBox.instance.Disappear(0.4f);
			yield break;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0005999F File Offset: 0x00057B9F
		private IEnumerator CRewardConversation()
		{
			LetterBox.instance.Appear(0.4f);
			yield return this._npcConversation.CConversation(new string[]
			{
				this.rewardScripts.Random<string>()
			});
			LetterBox.instance.Disappear(0.4f);
			while (!this._essenceRequest.isDone)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.DropQuintessence(this._essenceRequest, this._dropPosition.position);
			this._questState = UnknownBoy.QuestState.GaveReward;
			yield break;
		}

		// Token: 0x040018CC RID: 6348
		[SerializeField]
		private Transform _body;

		// Token: 0x040018CD RID: 6349
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x040018CE RID: 6350
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040018CF RID: 6351
		[SerializeField]
		private EnemyWave _startWave;

		// Token: 0x040018D0 RID: 6352
		[SerializeField]
		private Transform _rewardConversationPoint;

		// Token: 0x040018D1 RID: 6353
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x040018D2 RID: 6354
		[SerializeField]
		private GameObject _lineText;

		// Token: 0x040018D3 RID: 6355
		[SerializeField]
		private RarityPossibilities _essencePossibilities;

		// Token: 0x040018D4 RID: 6356
		[SerializeField]
		private EffectInfo _outEffectInfo;

		// Token: 0x040018D5 RID: 6357
		[SerializeField]
		private EffectInfo _inEffectInfo;

		// Token: 0x040018D6 RID: 6358
		private UnknownBoy.QuestState _questState;

		// Token: 0x040018D7 RID: 6359
		private NpcConversation _npcConversation;

		// Token: 0x040018D8 RID: 6360
		private EssenceRequest _essenceRequest;

		// Token: 0x020005C1 RID: 1473
		private enum QuestState
		{
			// Token: 0x040018DA RID: 6362
			Wait,
			// Token: 0x040018DB RID: 6363
			Accepted,
			// Token: 0x040018DC RID: 6364
			Cleared,
			// Token: 0x040018DD RID: 6365
			GaveReward
		}
	}
}
