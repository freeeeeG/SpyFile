using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes.Horde
{
	// Token: 0x020007CD RID: 1997
	public class ServerHordeFlowController : ServerFlowControllerBase, IKitchenOrderHandler
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x000B5830 File Offset: 0x000B3C30
		// (set) Token: 0x0600263E RID: 9790 RVA: 0x000B5838 File Offset: 0x000B3C38
		public int MaxHealth { get; private set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x000B5841 File Offset: 0x000B3C41
		public int Health
		{
			get
			{
				return this.m_score.TotalHealth;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06002640 RID: 9792 RVA: 0x000B584E File Offset: 0x000B3C4E
		public int Money
		{
			get
			{
				return this.m_score.GetTotalMoney();
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000B585B File Offset: 0x000B3C5B
		public void RegisterOnMoneyChanged(object handle, GenericVoid<int> onMoneyChanged)
		{
			this.m_onMoneyChanged = (GenericVoid<int>)Delegate.Combine(this.m_onMoneyChanged, onMoneyChanged);
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000B5874 File Offset: 0x000B3C74
		public void UnregisterOnMoneyChanged(object handle, GenericVoid<int> onMoneyChanged)
		{
			this.m_onMoneyChanged = (GenericVoid<int>)Delegate.Remove(this.m_onMoneyChanged, onMoneyChanged);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000B588D File Offset: 0x000B3C8D
		public override EntityType GetEntityType()
		{
			return EntityType.HordeFlowController;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000B5894 File Offset: 0x000B3C94
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_flowController = (HordeFlowController)synchronisedObject;
			this.m_levelConfig = (GameUtils.GetLevelConfig() as HordeLevelConfig);
			this.m_flowLayerId = LayerMask.NameToLayer("Default");
			this.MaxHealth = this.m_levelConfig.m_health;
			this.m_score.TotalHealth = this.MaxHealth;
			PlateReturnController.PlateReturnControllerConfig plateReturnControllerConfig = new PlateReturnController.PlateReturnControllerConfig
			{
				m_plateReturnTime = this.m_levelConfig.m_plateReturnTime
			};
			this.m_plateReturnController = new PlateReturnController(ref plateReturnControllerConfig);
			this.m_plateReturnController.Init();
			List<GameObject> list = new List<GameObject>();
			SceneManager.GetActiveScene().GetRootGameObjects(list);
			List<ServerHordeTarget> list2 = new List<ServerHordeTarget>(8);
			for (int i = 0; i < list.Count; i++)
			{
				list2.AddRange(list[i].RequestComponentsRecursive<ServerHordeTarget>());
			}
			list2.Sort(default(ServerHordeTargetComparer));
			this.m_targets = list2.ToArray();
			this.m_enemies = new ServerHordeEnemy[this.m_targets.Length];
			this.m_entries = new RecipeList.Entry[this.m_targets.Length];
			for (int j = 0; j < this.m_levelConfig.m_waves.Count; j++)
			{
				HordeWaveData hordeWaveData = this.m_levelConfig.m_waves[j];
				for (int k = 0; k < hordeWaveData.m_spawns.Count; k++)
				{
					HordeSpawnData hordeSpawnData = hordeWaveData.m_spawns[k];
					NetworkUtils.RegisterSpawnablePrefab(base.gameObject, hordeSpawnData.m_prefab);
				}
			}
			this.m_runWaves = this.RunWaves(this.m_levelConfig.m_waves, this.m_flowController.m_waveNumberUIDelay);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000B5A5C File Offset: 0x000B3E5C
		public void Damage(int kitchenDamage)
		{
			this.m_score.TotalHealth = Mathf.Max(this.m_score.TotalHealth - kitchenDamage, 0);
			HordeFlowMessage.ScoreOnly(ref this.m_message, this.m_score);
			this.SendServerEvent(this.m_message);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000B5AAC File Offset: 0x000B3EAC
		public bool SpendMoney(int amount)
		{
			int totalMoney = this.m_score.GetTotalMoney();
			if (totalMoney >= amount)
			{
				this.m_score.TotalMoneySpent = this.m_score.TotalMoneySpent + amount;
				HordeFlowMessage.ScoreOnly(ref this.m_message, this.m_score);
				this.SendServerEvent(this.m_message);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				return true;
			}
			return false;
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000B5B1A File Offset: 0x000B3F1A
		protected override bool HasFinished()
		{
			return this.m_runWaves == null || base.HasFinished();
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000B5B30 File Offset: 0x000B3F30
		protected override void OnUpdateInRound()
		{
			base.OnUpdateInRound();
			this.m_waveTime += (double)TimeManager.GetDeltaTime(this.m_flowLayerId);
			if (this.m_runWaves != null)
			{
				this.RemoveDead();
				if (this.m_score.TotalHealth > 0 && this.m_runWaves.MoveNext())
				{
					if (this.m_plateReturnController != null)
					{
						this.m_plateReturnController.Update();
					}
				}
				else
				{
					this.RemoveDead();
					this.m_runWaves = null;
				}
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000B5BB8 File Offset: 0x000B3FB8
		public void RemoveDead()
		{
			for (int i = 0; i < this.m_enemies.Length; i++)
			{
				if (this.m_enemies[i] != null && !this.m_enemies[i].IsAlive)
				{
					NetworkUtils.DestroyObject(this.m_enemies[i].gameObject);
					this.m_enemies[i] = null;
				}
			}
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000B5C20 File Offset: 0x000B4020
		public bool AnyAlive()
		{
			bool flag = false;
			for (int i = 0; i < this.m_enemies.Length; i++)
			{
				flag |= (this.m_enemies[i] != null && this.m_enemies[i].IsAlive);
			}
			return flag;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000B5C70 File Offset: 0x000B4070
		public int NextTarget()
		{
			this.m_freeTargets.Clear();
			for (int i = 0; i < this.m_enemies.Length; i++)
			{
				if (this.m_enemies[i] == null || !this.m_enemies[i].IsAlive)
				{
					this.m_freeTargets.Add(i);
				}
			}
			return (this.m_freeTargets.Count <= 0) ? -1 : this.m_freeTargets[UnityEngine.Random.Range(0, this.m_freeTargets.Count)];
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000B5D08 File Offset: 0x000B4108
		public int NextSpawn(List<HordeSpawnData> spawns, double waveTime)
		{
			for (int i = 0; i < spawns.Count; i++)
			{
				if (spawns[i].CanSpawn(waveTime))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000B5D44 File Offset: 0x000B4144
		private IEnumerator RunWaves(HordeWavesData waves, float waveNumberUIDelay)
		{
			int layerId = LayerMask.NameToLayer("Default");
			while (this.m_waveIndex < waves.Count)
			{
				this.RemoveDead();
				HordeWaveData wave = waves[this.m_waveIndex];
				this.m_waveTime = 0.0;
				HordeFlowMessage.BeginWave(ref this.m_message, this.m_waveIndex, this.m_score);
				this.SendServerEvent(this.m_message);
				IEnumerator timer = CoroutineUtils.TimerRoutine(waveNumberUIDelay, this.m_flowLayerId);
				while (timer != null && timer.MoveNext())
				{
					yield return null;
				}
				List<HordeSpawnData> spawns = new List<HordeSpawnData>(wave.m_spawns.Count);
				spawns.AddRange(wave.m_spawns);
				while (spawns.Count > 0)
				{
					int targetIndex = -1;
					while ((targetIndex = this.NextTarget()) == -1)
					{
						yield return null;
					}
					int spawnIndex = -1;
					while ((spawnIndex = this.NextSpawn(spawns, this.m_waveTime)) == -1)
					{
						yield return null;
					}
					HordeSpawnData spawn = spawns[spawnIndex];
					spawns.RemoveAt(spawnIndex);
					ServerHordeTarget target = this.m_targets[targetIndex];
					Quaternion rotation;
					Vector3 position = target.GenerateSpawnPosition(out rotation);
					GameObject obj = NetworkUtils.ServerSpawnPrefab(target.gameObject, spawn.m_prefab, position, rotation);
					ServerHordeEnemy serverHordeEnemy = obj.RequireComponent<ServerHordeEnemy>();
					serverHordeEnemy.Setup(this, target);
					HordeFlowMessage.Spawn(ref this.m_message, targetIndex, serverHordeEnemy.gameObject);
					this.SendServerEvent(this.m_message);
					this.m_enemies[targetIndex] = serverHordeEnemy;
					RecipeList.Entry randomElement = wave.m_recipes.m_recipes.GetRandomElement<RecipeList.Entry>();
					HordeFlowMessage.EntryAdded(ref this.m_message, targetIndex, randomElement);
					this.SendServerEvent(this.m_message);
					this.m_entries[targetIndex] = randomElement;
				}
				while (this.AnyAlive())
				{
					yield return null;
				}
				HordeFlowMessage.EndWave(ref this.m_message, this.m_waveIndex, this.m_score);
				this.SendServerEvent(this.m_message);
				IEnumerator timer2 = CoroutineUtils.TimerRoutine((float)wave.m_intervalSeconds, this.m_flowLayerId);
				while (timer2 != null && timer2.MoveNext())
				{
					yield return null;
				}
				this.m_waveIndex++;
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000B5D6D File Offset: 0x000B416D
		public void OnLevelRestartRequested()
		{
			this.m_levelRestartRequested = true;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000B5D78 File Offset: 0x000B4178
		protected override string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
		{
			o_loadState = GameState.NotSet;
			o_loadEndState = GameState.NotSet;
			o_useLoadingScreen = true;
			if (this.m_levelRestartRequested)
			{
				o_loadState = GameState.LoadKitchen;
				o_loadEndState = GameState.RunLevelIntro;
				o_useLoadingScreen = true;
				return GameUtils.GetGameSession().LevelSettings.SceneDirectoryVarientEntry.SceneName;
			}
			if (ServerGameSetup.Mode == GameMode.Campaign)
			{
				o_loadState = GameState.CampaignMap;
				o_loadEndState = GameState.RunMapUnfoldRoutine;
				return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
			}
			if (ServerGameSetup.Mode == GameMode.Party)
			{
				o_loadState = GameState.PartyLobby;
				o_loadEndState = GameState.NotSet;
				return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
			}
			return string.Empty;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000B5E00 File Offset: 0x000B4200
		public void FoodDelivered(AssembledDefinitionNode definition, PlatingStepData plateType, ServerPlateStation station)
		{
			this.m_plateReturnController.FoodDelivered(definition, plateType, station);
			int num = Array.FindIndex<ServerHordeTarget>(this.m_targets, (ServerHordeTarget x) => x.gameObject == station.gameObject);
			ServerHordeTarget serverHordeTarget = this.m_targets[num];
			ServerHordeEnemy serverHordeEnemy = this.m_enemies[num];
			RecipeList.Entry entry = this.m_entries[num];
			if (serverHordeEnemy != null && entry != null && AssembledDefinitionNode.Matching(definition, entry.m_order))
			{
				bool flag = serverHordeEnemy.Feed(entry);
				if (flag)
				{
					this.m_score.TotalEnemiesDefeated = this.m_score.TotalEnemiesDefeated + 1;
				}
				this.m_score.TotalMoneyEarned = this.m_score.TotalMoneyEarned + this.m_levelConfig.m_recipeMoney.Get(entry.m_order);
				HordeFlowMessage.SuccessfulDelivery(ref this.m_message, num, this.m_score);
				this.SendServerEvent(this.m_message);
				this.m_onMoneyChanged(this.m_score.GetTotalMoney());
				if (!flag)
				{
					this.m_entries[num] = this.m_levelConfig.m_waves[this.m_waveIndex].m_recipes.m_recipes.GetRandomElement<RecipeList.Entry>();
					HordeFlowMessage.EntryAdded(ref this.m_message, num, this.m_entries[num]);
					this.SendServerEvent(this.m_message);
				}
				else
				{
					this.m_entries[num] = null;
				}
			}
			else
			{
				HordeFlowMessage.IncorrectDelivery(ref this.m_message, num, this.m_score);
				this.SendServerEvent(this.m_message);
			}
		}

		// Token: 0x04001E59 RID: 7769
		private HordeFlowController m_flowController;

		// Token: 0x04001E5A RID: 7770
		private HordeLevelConfig m_levelConfig;

		// Token: 0x04001E5B RID: 7771
		private HordeFlowMessage m_message = default(HordeFlowMessage);

		// Token: 0x04001E5C RID: 7772
		private IEnumerator m_runWaves;

		// Token: 0x04001E5D RID: 7773
		private int m_waveIndex;

		// Token: 0x04001E5E RID: 7774
		private double m_waveTime;

		// Token: 0x04001E5F RID: 7775
		private PlateReturnController m_plateReturnController;

		// Token: 0x04001E60 RID: 7776
		private ServerHordeTarget[] m_targets;

		// Token: 0x04001E61 RID: 7777
		private ServerHordeEnemy[] m_enemies;

		// Token: 0x04001E62 RID: 7778
		private RecipeList.Entry[] m_entries;

		// Token: 0x04001E63 RID: 7779
		private TeamScoreStats m_score = default(TeamScoreStats);

		// Token: 0x04001E64 RID: 7780
		private int m_flowLayerId = -1;

		// Token: 0x04001E66 RID: 7782
		private GenericVoid<int> m_onMoneyChanged;

		// Token: 0x04001E67 RID: 7783
		private List<int> m_freeTargets = new List<int>(8);

		// Token: 0x04001E68 RID: 7784
		private bool m_levelRestartRequested;
	}
}
