using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes.Horde
{
	// Token: 0x020007CE RID: 1998
	public class ClientHordeFlowController : ClientFlowControllerBase, IRecipeListCache
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x000B6492 File Offset: 0x000B4892
		// (set) Token: 0x06002653 RID: 9811 RVA: 0x000B649A File Offset: 0x000B489A
		public int MaxHealth { get; private set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x000B64A3 File Offset: 0x000B48A3
		public int Health
		{
			get
			{
				return this.m_score.TotalHealth;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x000B64B0 File Offset: 0x000B48B0
		public int Money
		{
			get
			{
				return this.m_score.GetTotalMoney();
			}
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000B64C0 File Offset: 0x000B48C0
		public void RegisterOnSuccessfulDelivery(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onSuccessfulDelivery)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onSuccessfulDeliveryForTarget;
			int num2;
			(onSuccessfulDeliveryForTarget = this.m_onSuccessfulDeliveryForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Combine(onSuccessfulDeliveryForTarget[num2], onSuccessfulDelivery);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000B64F4 File Offset: 0x000B48F4
		public void UnregisterOnSuccessfulDelivery(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onSuccessfulDelivery)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onSuccessfulDeliveryForTarget;
			int num2;
			(onSuccessfulDeliveryForTarget = this.m_onSuccessfulDeliveryForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Remove(onSuccessfulDeliveryForTarget[num2], onSuccessfulDelivery);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000B6528 File Offset: 0x000B4928
		public void RegisterOnIncorrectDelivery(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onIncorrectDelivery)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onIncorrectDeliveryForTarget;
			int num2;
			(onIncorrectDeliveryForTarget = this.m_onIncorrectDeliveryForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Combine(onIncorrectDeliveryForTarget[num2], onIncorrectDelivery);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000B655C File Offset: 0x000B495C
		public void UnregisterOnIncorrectDelivery(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onIncorrectDelivery)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onIncorrectDeliveryForTarget;
			int num2;
			(onIncorrectDeliveryForTarget = this.m_onIncorrectDeliveryForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Remove(onIncorrectDeliveryForTarget[num2], onIncorrectDelivery);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000B6590 File Offset: 0x000B4990
		public void RegisterOnEntryAdded(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onOrderChanged)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onEntryAddedForTarget;
			int num2;
			(onEntryAddedForTarget = this.m_onEntryAddedForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Combine(onEntryAddedForTarget[num2], onOrderChanged);
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000B65C4 File Offset: 0x000B49C4
		public void UnregisterOnEntryAdded(object handle, ClientHordeTarget target, GenericVoid<RecipeList.Entry> onOrderChanged)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<RecipeList.Entry>[] onEntryAddedForTarget;
			int num2;
			(onEntryAddedForTarget = this.m_onEntryAddedForTarget)[num2 = num] = (GenericVoid<RecipeList.Entry>)Delegate.Remove(onEntryAddedForTarget[num2], onOrderChanged);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000B65F8 File Offset: 0x000B49F8
		public void RegisterOnEnemyApproaching(object handle, ClientHordeTarget target, GenericVoid<ClientHordeEnemy> onEnemyApproaching)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<ClientHordeEnemy>[] onEnemyApproachingTarget;
			int num2;
			(onEnemyApproachingTarget = this.m_onEnemyApproachingTarget)[num2 = num] = (GenericVoid<ClientHordeEnemy>)Delegate.Combine(onEnemyApproachingTarget[num2], onEnemyApproaching);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000B662C File Offset: 0x000B4A2C
		public void UnregisterOnEnemyApproaching(object handle, ClientHordeTarget target, GenericVoid<ClientHordeEnemy> onEnemyApproaching)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<ClientHordeEnemy>[] onEnemyApproachingTarget;
			int num2;
			(onEnemyApproachingTarget = this.m_onEnemyApproachingTarget)[num2 = num] = (GenericVoid<ClientHordeEnemy>)Delegate.Remove(onEnemyApproachingTarget[num2], onEnemyApproaching);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000B6660 File Offset: 0x000B4A60
		public void RegisterOnEnemyDespawning(object handle, ClientHordeTarget target, GenericVoid<ClientHordeEnemy> onEnemyDespawning)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<ClientHordeEnemy>[] onEnemyDespawningForTarget;
			int num2;
			(onEnemyDespawningForTarget = this.m_onEnemyDespawningForTarget)[num2 = num] = (GenericVoid<ClientHordeEnemy>)Delegate.Combine(onEnemyDespawningForTarget[num2], onEnemyDespawning);
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000B6694 File Offset: 0x000B4A94
		public void UnregisterOnEnemyDeath(object handle, ClientHordeTarget target, GenericVoid<ClientHordeEnemy> onEnemyDespawning)
		{
			int num = Array.IndexOf<ClientHordeTarget>(this.m_targets, target);
			GenericVoid<ClientHordeEnemy>[] onEnemyDespawningForTarget;
			int num2;
			(onEnemyDespawningForTarget = this.m_onEnemyDespawningForTarget)[num2 = num] = (GenericVoid<ClientHordeEnemy>)Delegate.Remove(onEnemyDespawningForTarget[num2], onEnemyDespawning);
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000B66C8 File Offset: 0x000B4AC8
		public void RegisterOnBeginWave(object handle, GenericVoid<ClientHordeFlowController, int> onBeginWave)
		{
			this.m_onBeginWave = (GenericVoid<ClientHordeFlowController, int>)Delegate.Combine(this.m_onBeginWave, onBeginWave);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000B66E1 File Offset: 0x000B4AE1
		public void UnregisterOnBeginWave(object handle, GenericVoid<ClientHordeFlowController, int> onBeginWave)
		{
			this.m_onBeginWave = (GenericVoid<ClientHordeFlowController, int>)Delegate.Remove(this.m_onBeginWave, onBeginWave);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000B66FA File Offset: 0x000B4AFA
		public void RegisterOnMoneyChanged(object handle, GenericVoid<int> onMoneyChanged)
		{
			this.m_onMoneyChanged = (GenericVoid<int>)Delegate.Combine(this.m_onMoneyChanged, onMoneyChanged);
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000B6713 File Offset: 0x000B4B13
		public void UnregisterOnMoneyChanged(object handle, GenericVoid<int> onMoneyChanged)
		{
			this.m_onMoneyChanged = (GenericVoid<int>)Delegate.Remove(this.m_onMoneyChanged, onMoneyChanged);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000B672C File Offset: 0x000B4B2C
		public override EntityType GetEntityType()
		{
			return EntityType.HordeFlowController;
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000B6730 File Offset: 0x000B4B30
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.CacheRecipeListData();
			this.m_flowController = (HordeFlowController)synchronisedObject;
			this.m_levelConfig = (GameUtils.GetLevelConfig() as HordeLevelConfig);
			GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_flowController.m_uiPrefab);
			this.MaxHealth = this.m_levelConfig.m_health;
			this.m_score.TotalHealth = this.MaxHealth;
			List<GameObject> list = new List<GameObject>();
			SceneManager.GetActiveScene().GetRootGameObjects(list);
			List<ClientHordeTarget> list2 = new List<ClientHordeTarget>(8);
			for (int i = 0; i < list.Count; i++)
			{
				list2.AddRange(list[i].RequestComponentsRecursive<ClientHordeTarget>());
			}
			list2.Sort(default(ClientHordeTargetComparer));
			this.m_targets = list2.ToArray();
			this.m_enemies = new ClientHordeEnemy[this.m_targets.Length];
			this.m_entries = new RecipeList.Entry[this.m_targets.Length];
			this.m_onSuccessfulDeliveryForTarget = new GenericVoid<RecipeList.Entry>[this.m_targets.Length];
			this.m_onIncorrectDeliveryForTarget = new GenericVoid<RecipeList.Entry>[this.m_targets.Length];
			this.m_onEntryAddedForTarget = new GenericVoid<RecipeList.Entry>[this.m_targets.Length];
			this.m_onEnemyApproachingTarget = new GenericVoid<ClientHordeEnemy>[this.m_targets.Length];
			this.m_onEnemyDespawningForTarget = new GenericVoid<ClientHordeEnemy>[this.m_targets.Length];
			for (int j = 0; j < this.m_levelConfig.m_waves.Count; j++)
			{
				HordeWaveData hordeWaveData = this.m_levelConfig.m_waves[j];
				for (int k = 0; k < hordeWaveData.m_spawns.Count; k++)
				{
					HordeSpawnData hordeSpawnData = hordeWaveData.m_spawns[k];
					for (int l = 0; l < this.m_targets.Length; l++)
					{
						NetworkUtils.RegisterSpawnablePrefab(this.m_targets[l].gameObject, hordeSpawnData.m_prefab);
					}
				}
			}
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000B6924 File Offset: 0x000B4D24
		private IEnumerator RunWaveIntro(GameObject waveNumberUIPrefab, float waveNumberUIDelay, string waveNumberUILocalisationTag, HordeWavesData waves, int waveIndex)
		{
			GameObject waveNumberUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(waveNumberUIPrefab);
			T17Text waveNumberText = waveNumberUIInstance.RequireComponentRecursive<T17Text>();
			waveNumberUIInstance.SetActive(false);
			string waveNumberLocalisedText = Localization.Get(waveNumberUILocalisationTag, new LocToken[]
			{
				new LocToken("[Number]", (waveIndex + 1).ToString()),
				new LocToken("[NumberMax]", waves.Count.ToString())
			});
			waveNumberText.SetNonLocalizedText(waveNumberLocalisedText);
			waveNumberUIInstance.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Wave_Incoming, waveNumberUIInstance.layer);
			this.m_onBeginWave(this, waveIndex + 1);
			int layerId = LayerMask.NameToLayer("Default");
			IEnumerator timerRoutine = CoroutineUtils.TimerRoutine(waveNumberUIDelay, layerId);
			while (timerRoutine.MoveNext())
			{
				yield return null;
			}
			waveNumberUIInstance.SetActive(false);
			UnityEngine.Object.Destroy(waveNumberUIInstance);
			yield return null;
			yield break;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000B6964 File Offset: 0x000B4D64
		private IEnumerator RunWaveOutro(HordeWavesData waves, int index)
		{
			IEnumerator timer = CoroutineUtils.TimerRoutine((float)waves[index].m_intervalSeconds, base.gameObject.layer);
			while (timer != null && timer.MoveNext())
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000B6990 File Offset: 0x000B4D90
		public override void UpdateSynchronising()
		{
			base.UpdateSynchronising();
			if (this.m_runWaveIntro != null && !this.m_runWaveIntro.MoveNext())
			{
				this.m_runWaveIntro = null;
			}
			if (this.m_runWaveOutro != null && !this.m_runWaveOutro.MoveNext())
			{
				this.m_runWaveOutro = null;
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000B69E8 File Offset: 0x000B4DE8
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			HordeFlowMessage hordeFlowMessage = (HordeFlowMessage)serialisable;
			switch (hordeFlowMessage.m_kind)
			{
			case HordeFlowMessage.Kind.BeginWave:
				this.m_score.Copy(hordeFlowMessage.m_score);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				this.m_runWaveIntro = this.RunWaveIntro(this.m_flowController.m_waveNumberUIPrefab, this.m_flowController.m_waveNumberUIDelay, this.m_flowController.m_waveNumberUILocalisationTag, this.m_levelConfig.m_waves, hordeFlowMessage.m_index);
				break;
			case HordeFlowMessage.Kind.EndWave:
				this.m_score.Copy(hordeFlowMessage.m_score);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				this.m_runWaveOutro = this.RunWaveOutro(this.m_levelConfig.m_waves, hordeFlowMessage.m_index);
				break;
			case HordeFlowMessage.Kind.Spawn:
			{
				ClientHordeTarget target = this.m_targets[hordeFlowMessage.m_index];
				ClientHordeEnemy clientHordeEnemy = hordeFlowMessage.m_enemy.RequireComponent<ClientHordeEnemy>();
				clientHordeEnemy.Setup(this, target);
				clientHordeEnemy.RegisterOnBeginState(this, new GenericVoid<ClientHordeEnemy, HordeEnemyBehaviorState, HordeEnemyBehaviorState>(this.OnEnemyBeginState));
				this.m_enemies[hordeFlowMessage.m_index] = clientHordeEnemy;
				break;
			}
			case HordeFlowMessage.Kind.EntryAdded:
			{
				this.m_entries[hordeFlowMessage.m_index] = hordeFlowMessage.m_entry;
				RecipeList.Entry param = this.m_entries[hordeFlowMessage.m_index];
				this.m_onEntryAddedForTarget[hordeFlowMessage.m_index](param);
				break;
			}
			case HordeFlowMessage.Kind.SuccessfulDelivery:
			{
				this.m_score.Copy(hordeFlowMessage.m_score);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				RecipeList.Entry param2 = this.m_entries[hordeFlowMessage.m_index];
				this.m_entries[hordeFlowMessage.m_index] = null;
				this.m_onSuccessfulDeliveryForTarget[hordeFlowMessage.m_index](param2);
				GameUtils.TriggerAudio(GameOneShotAudioTag.SuccessfulDelivery, base.gameObject.layer);
				break;
			}
			case HordeFlowMessage.Kind.IncorrectDelivery:
			{
				this.m_score.Copy(hordeFlowMessage.m_score);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				RecipeList.Entry param3 = this.m_entries[hordeFlowMessage.m_index];
				this.m_onIncorrectDeliveryForTarget[hordeFlowMessage.m_index](param3);
				GameUtils.TriggerAudio(GameOneShotAudioTag.RecipeTimeOut, base.gameObject.layer);
				break;
			}
			case HordeFlowMessage.Kind.ScoreOnly:
				this.m_score.Copy(hordeFlowMessage.m_score);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				break;
			}
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000B6C78 File Offset: 0x000B5078
		private void OnEnemyBeginState(ClientHordeEnemy enemy, HordeEnemyBehaviorState fromState, HordeEnemyBehaviorState state)
		{
			if (state != HordeEnemyBehaviorState.Move)
			{
				if (state == HordeEnemyBehaviorState.Despawn)
				{
					int num = Array.IndexOf<ClientHordeEnemy>(this.m_enemies, enemy);
					this.m_onEnemyDespawningForTarget[num](enemy);
				}
			}
			else
			{
				int num2 = Array.IndexOf<ClientHordeEnemy>(this.m_enemies, enemy);
				this.m_onEnemyApproachingTarget[num2](enemy);
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000B6CD8 File Offset: 0x000B50D8
		private void OnLevelRestartRequested()
		{
			bool flag = !ConnectionStatus.IsHost() && ConnectionStatus.IsInSession();
			bool flag2 = ClientGameSetup.Mode != GameMode.Campaign;
			if (flag || flag2)
			{
				return;
			}
			base.gameObject.RequireComponent<ServerHordeFlowController>().OnLevelRestartRequested();
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000B6D24 File Offset: 0x000B5124
		protected override IEnumerator RunLevelOutro()
		{
			int levelID = GameUtils.GetLevelID();
			OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
			if (overcookedAchievementManager != null)
			{
				bool flag = this.Health >= this.MaxHealth;
				if (flag)
				{
					overcookedAchievementManager.AddIDStat(700, levelID, ControlPadInput.PadNum.One);
				}
			}
			int num = 3;
			int starRating = 0;
			if (this.Health > 0 && this.Health < this.MaxHealth)
			{
				starRating = 1 + (int)Mathf.Round(MathUtils.ClampedRemap((float)this.Health, 0f, (float)this.MaxHealth, -0.49f, (float)(num - 2) - 0.51f));
			}
			else if (this.Health >= this.MaxHealth)
			{
				starRating = num;
			}
			int requiredBitCount = GameUtils.GetRequiredBitCount(65535);
			GameSession gameSession = GameUtils.GetGameSession();
			GameProgress.UnlockData[] unlocks = new GameProgress.UnlockData[0];
			gameSession.Progress.RecordLevelProgress(levelID, starRating, ref unlocks);
			gameSession.Progress.RecordLevelScore(new GameProgress.HighScores.Score
			{
				iLevelID = levelID,
				iHighScore = FloatUtils.ToUnorm((float)this.Health / (float)this.MaxHealth, requiredBitCount)
			});
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				Analytics.LogEvent("Level Score", (long)this.Health, Analytics.Flags.LevelName | Analytics.Flags.PlayerCount);
			}
			HordeOutroFlowroutine hordeOutroFlowroutine = new HordeOutroFlowroutine();
			HordeRatingUIController.ScoreData scoreData = new HordeRatingUIController.ScoreData
			{
				m_health = (float)this.m_score.TotalHealth / (float)this.MaxHealth,
				m_moneyEarned = this.m_score.TotalMoneyEarned,
				m_enemiesDefeated = this.m_score.TotalEnemiesDefeated
			};
			this.m_levelConfig.m_flowroutineData.m_scoreData = scoreData;
			this.m_levelConfig.m_flowroutineData.m_health = (float)this.m_score.TotalHealth;
			this.m_levelConfig.m_flowroutineData.m_success = ((float)this.m_score.TotalHealth > 0f);
			this.m_levelConfig.m_flowroutineData.m_unlocks = unlocks;
			hordeOutroFlowroutine.OnRestartRequest = new GenericVoid(this.OnLevelRestartRequested);
			return hordeOutroFlowroutine.BuildFlowroutine(this.m_levelConfig.m_flowroutineData);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000B6F45 File Offset: 0x000B5345
		public OrderDefinitionNode[] GetCachedRecipeList()
		{
			return this.m_cachedRecipeList.ToArray();
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000B6F52 File Offset: 0x000B5352
		public AssembledDefinitionNode[] GetCachedAssembledRecipes()
		{
			return this.m_cachedAssembledRecipes.ToArray();
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000B6F5F File Offset: 0x000B535F
		public CookingStepData[] GetCachedCookingSteps()
		{
			return this.m_cachedCookingStepList.ToArray();
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000B6F6C File Offset: 0x000B536C
		private void CacheRecipeListData()
		{
			LevelConfigBase levelConfig = base.GetLevelConfig();
			if (levelConfig != null && levelConfig.m_recipeMatchingList != null)
			{
				this.m_cachedRecipeList.Capacity = levelConfig.m_recipeMatchingList.m_recipes.Length;
				this.m_cachedCookingStepList.Capacity = levelConfig.m_recipeMatchingList.m_cookingSteps.Length;
				this.m_cachedRecipeList.AddRange(levelConfig.m_recipeMatchingList.m_recipes);
				this.m_cachedCookingStepList.AddRange(levelConfig.m_recipeMatchingList.m_cookingSteps);
				for (int i = 0; i < levelConfig.m_recipeMatchingList.m_includeLists.Length; i++)
				{
					this.m_cachedRecipeList.AddRange(levelConfig.m_recipeMatchingList.m_includeLists[i].m_recipes);
					this.m_cachedCookingStepList.AddRange(levelConfig.m_recipeMatchingList.m_includeLists[i].m_cookingSteps);
				}
				this.m_cachedAssembledRecipes.Capacity = this.m_cachedRecipeList.Count;
				for (int j = 0; j < this.m_cachedRecipeList.Count; j++)
				{
					this.m_cachedAssembledRecipes.Add(this.m_cachedRecipeList[j].Convert().Simpilfy());
				}
			}
		}

		// Token: 0x04001E69 RID: 7785
		private HordeFlowController m_flowController;

		// Token: 0x04001E6A RID: 7786
		private HordeLevelConfig m_levelConfig;

		// Token: 0x04001E6B RID: 7787
		private ClientHordeTarget[] m_targets;

		// Token: 0x04001E6C RID: 7788
		private ClientHordeEnemy[] m_enemies;

		// Token: 0x04001E6D RID: 7789
		private RecipeList.Entry[] m_entries;

		// Token: 0x04001E6E RID: 7790
		private TeamScoreStats m_score = default(TeamScoreStats);

		// Token: 0x04001E70 RID: 7792
		private IEnumerator m_runWaveIntro;

		// Token: 0x04001E71 RID: 7793
		private IEnumerator m_runWaveOutro;

		// Token: 0x04001E72 RID: 7794
		private GenericVoid<RecipeList.Entry>[] m_onEntryAddedForTarget;

		// Token: 0x04001E73 RID: 7795
		private GenericVoid<RecipeList.Entry>[] m_onSuccessfulDeliveryForTarget;

		// Token: 0x04001E74 RID: 7796
		private GenericVoid<RecipeList.Entry>[] m_onIncorrectDeliveryForTarget;

		// Token: 0x04001E75 RID: 7797
		private GenericVoid<ClientHordeEnemy>[] m_onEnemyApproachingTarget;

		// Token: 0x04001E76 RID: 7798
		private GenericVoid<ClientHordeEnemy>[] m_onEnemyDespawningForTarget;

		// Token: 0x04001E77 RID: 7799
		private GenericVoid<ClientHordeFlowController, int> m_onBeginWave;

		// Token: 0x04001E78 RID: 7800
		private GenericVoid<int> m_onMoneyChanged;

		// Token: 0x04001E79 RID: 7801
		private List<OrderDefinitionNode> m_cachedRecipeList = new List<OrderDefinitionNode>(8);

		// Token: 0x04001E7A RID: 7802
		private List<AssembledDefinitionNode> m_cachedAssembledRecipes = new List<AssembledDefinitionNode>(8);

		// Token: 0x04001E7B RID: 7803
		private List<CookingStepData> m_cachedCookingStepList = new List<CookingStepData>(8);
	}
}
