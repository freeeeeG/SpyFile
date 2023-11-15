using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF9 RID: 3577
	public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06006DE1 RID: 28129 RVA: 0x002B5582 File Offset: 0x002B3782
		public bool canStarTravel
		{
			get
			{
				return this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
			}
		}

		// Token: 0x06006DE2 RID: 28130 RVA: 0x002B5593 File Offset: 0x002B3793
		public string GetClusterMapMeteorShowerID()
		{
			return this.clusterMapMeteorShowerID;
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x002B559B File Offset: 0x002B379B
		public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
		{
			return new List<MeteorShowerEvent.BombardmentInfo>(this.bombardmentInfo);
		}

		// Token: 0x06006DE4 RID: 28132 RVA: 0x002B55A8 File Offset: 0x002B37A8
		public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true) : base(id, 0, 0)
		{
			this.allowMultipleEventInstances = true;
			this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
			this.duration = duration;
			this.secondsPerMeteor = secondsPerMeteor;
			this.secondsBombardmentOff = secondsBombardmentOff;
			this.secondsBombardmentOn = secondsBombardmentOn;
			this.affectedByDifficulty = affectedByDifficulty;
			this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
			this.tags.Add(GameTags.SpaceDanger);
		}

		// Token: 0x06006DE5 RID: 28133 RVA: 0x002B5620 File Offset: 0x002B3820
		public MeteorShowerEvent AddMeteor(string prefab, float weight)
		{
			this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo
			{
				prefab = prefab,
				weight = weight
			});
			return this;
		}

		// Token: 0x06006DE6 RID: 28134 RVA: 0x002B5652 File Offset: 0x002B3852
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x002B565C File Offset: 0x002B385C
		public override bool IsAllowed()
		{
			return base.IsAllowed() && (!this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies");
		}

		// Token: 0x0400526A RID: 21098
		private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;

		// Token: 0x0400526B RID: 21099
		private MathUtil.MinMax secondsBombardmentOff;

		// Token: 0x0400526C RID: 21100
		private MathUtil.MinMax secondsBombardmentOn;

		// Token: 0x0400526D RID: 21101
		private float secondsPerMeteor = 0.33f;

		// Token: 0x0400526E RID: 21102
		private float duration;

		// Token: 0x0400526F RID: 21103
		private string clusterMapMeteorShowerID;

		// Token: 0x04005270 RID: 21104
		private bool affectedByDifficulty = true;

		// Token: 0x02001F7C RID: 8060
		public struct BombardmentInfo
		{
			// Token: 0x04008E41 RID: 36417
			public string prefab;

			// Token: 0x04008E42 RID: 36418
			public float weight;
		}

		// Token: 0x02001F7D RID: 8061
		public class States : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
		{
			// Token: 0x0600A295 RID: 41621 RVA: 0x00365380 File Offset: 0x00363580
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi, false);
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
					if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
					{
						smi.GoTo(smi.sm.starMap);
						return;
					}
					smi.GoTo(smi.sm.running);
				});
				this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
				this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
				this.starMap.arrive.GoTo(this.running.bombarding);
				this.running.DefaultState(this.running.snoozing).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.runTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.running.bombarding.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
				}).Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StartBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.bombardTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				}, UpdateRate.SIM_200ms, false);
				this.running.snoozing.Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.snoozeTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600A296 RID: 41622 RVA: 0x003655B9 File Offset: 0x003637B9
			public static void TriggerMeteorGlobalEvent(MeteorShowerEvent.StatesInstance smi, GameHashes hash)
			{
				Game.Instance.Trigger((int)hash, smi.eventInstance.worldId);
			}

			// Token: 0x0600A297 RID: 41623 RVA: 0x003655D8 File Offset: 0x003637D8
			public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
			{
				if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
				{
					GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
					float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
					AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
					GameObject gameObject = Util.KInstantiate(prefab, null, null);
					gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
					ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
					def.destinationWorldID = smi.eventInstance.worldId;
					def.arrivalTime = arrivalTime;
					gameObject.SetActive(true);
					smi.sm.clusterMapMeteorShower.Set(gameObject, smi, false);
				}
				GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
				go.GetDef<ClusterMapMeteorShower.Def>();
				go.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
			}

			// Token: 0x04008E43 RID: 36419
			public MeteorShowerEvent.States.ClusterMapStates starMap;

			// Token: 0x04008E44 RID: 36420
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x04008E45 RID: 36421
			public MeteorShowerEvent.States.RunningStates running;

			// Token: 0x04008E46 RID: 36422
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x04008E47 RID: 36423
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;

			// Token: 0x04008E48 RID: 36424
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;

			// Token: 0x04008E49 RID: 36425
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;

			// Token: 0x04008E4A RID: 36426
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;

			// Token: 0x04008E4B RID: 36427
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

			// Token: 0x02002F48 RID: 12104
			public class ClusterMapStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400C172 RID: 49522
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;

				// Token: 0x0400C173 RID: 49523
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
			}

			// Token: 0x02002F49 RID: 12105
			public class RunningStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400C174 RID: 49524
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;

				// Token: 0x0400C175 RID: 49525
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
			}
		}

		// Token: 0x02001F7E RID: 8062
		public class StatesInstance : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A29F RID: 41631 RVA: 0x003657AA File Offset: 0x003639AA
			public float GetSleepTimerValue()
			{
				return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
			}

			// Token: 0x0600A2A0 RID: 41632 RVA: 0x003657D4 File Offset: 0x003639D4
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent) : base(master, eventInstance, meteorShowerEvent)
			{
				this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
				this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
				this.m_worldId = eventInstance.worldId;
				Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
			}

			// Token: 0x0600A2A1 RID: 41633 RVA: 0x00365848 File Offset: 0x00363A48
			public void OnClusterMapDestinationReached(object obj)
			{
				base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
			}

			// Token: 0x0600A2A2 RID: 41634 RVA: 0x00365860 File Offset: 0x00363A60
			private void OnActiveWorldChanged(object data)
			{
				int first = ((global::Tuple<int, int>)data).first;
				if (this.activeMeteorBackground != null)
				{
					this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = (first == this.m_worldId);
				}
			}

			// Token: 0x0600A2A3 RID: 41635 RVA: 0x003658A0 File Offset: 0x00363AA0
			public override void StopSM(string reason)
			{
				this.StopBackgroundEffects();
				base.StopSM(reason);
			}

			// Token: 0x0600A2A4 RID: 41636 RVA: 0x003658AF File Offset: 0x00363AAF
			protected override void OnCleanUp()
			{
				Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
				this.DestroyClusterMapMeteorShowerObject();
				base.OnCleanUp();
			}

			// Token: 0x0600A2A5 RID: 41637 RVA: 0x003658D8 File Offset: 0x00363AD8
			private void DestroyClusterMapMeteorShowerObject()
			{
				if (base.sm.clusterMapMeteorShower.Get(this) != null)
				{
					ClusterMapMeteorShower.Instance smi = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
					if (smi != null)
					{
						smi.StopSM("Event is being aborted");
						Util.KDestroyGameObject(smi.gameObject);
					}
				}
			}

			// Token: 0x0600A2A6 RID: 41638 RVA: 0x00365930 File Offset: 0x00363B30
			public void StartBackgroundEffects()
			{
				if (this.activeMeteorBackground == null)
				{
					this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, null, null);
					float x = (this.world.maximumBounds.x + this.world.minimumBounds.x) / 2f;
					float y = this.world.maximumBounds.y;
					float z = 25f;
					this.activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
					this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}

			// Token: 0x0600A2A7 RID: 41639 RVA: 0x003659E4 File Offset: 0x00363BE4
			public void StopBackgroundEffects()
			{
				if (this.activeMeteorBackground != null)
				{
					ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
					component.main.stopAction = ParticleSystemStopAction.Destroy;
					component.Stop();
					if (!component.IsAlive())
					{
						UnityEngine.Object.Destroy(this.activeMeteorBackground);
					}
					this.activeMeteorBackground = null;
				}
			}

			// Token: 0x0600A2A8 RID: 41640 RVA: 0x00365A38 File Offset: 0x00363C38
			public float TimeUntilNextShower()
			{
				if (base.IsInsideState(base.sm.running.bombarding))
				{
					return 0f;
				}
				if (!base.IsInsideState(base.sm.starMap))
				{
					return base.sm.snoozeTimeRemaining.Get(this);
				}
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (num >= 0f)
				{
					return num;
				}
				return 0f;
			}

			// Token: 0x0600A2A9 RID: 41641 RVA: 0x00365AC8 File Offset: 0x00363CC8
			public void Bombarding(float dt)
			{
				this.nextMeteorTime -= dt;
				while (this.nextMeteorTime < 0f)
				{
					if (this.GetSleepTimerValue() <= 0f)
					{
						this.DoBombardment(this.gameplayEvent.bombardmentInfo);
					}
					this.nextMeteorTime += this.GetNextMeteorTime();
				}
			}

			// Token: 0x0600A2AA RID: 41642 RVA: 0x00365B28 File Offset: 0x00363D28
			private void DoBombardment(List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
			{
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
				{
					num += bombardmentInfo.weight;
				}
				num = UnityEngine.Random.Range(0f, num);
				MeteorShowerEvent.BombardmentInfo bombardmentInfo2 = bombardment_info[0];
				int num2 = 0;
				while (num - bombardmentInfo2.weight > 0f)
				{
					num -= bombardmentInfo2.weight;
					bombardmentInfo2 = bombardment_info[++num2];
				}
				Game.Instance.Trigger(-84771526, null);
				this.SpawnBombard(bombardmentInfo2.prefab);
			}

			// Token: 0x0600A2AB RID: 41643 RVA: 0x00365BDC File Offset: 0x00363DDC
			private GameObject SpawnBombard(string prefab)
			{
				WorldContainer worldContainer = ClusterManager.Instance.GetWorld(this.m_worldId);
				float x = (float)(worldContainer.Width - 1) * UnityEngine.Random.value + (float)worldContainer.WorldOffset.x;
				float y = (float)(worldContainer.Height + worldContainer.WorldOffset.y - 1);
				float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				Vector3 position = new Vector3(x, y, layerZ);
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(prefab), position, Quaternion.identity, null, null, true, 0);
				Comet component = gameObject.GetComponent<Comet>();
				if (component != null)
				{
					component.spawnWithOffset = true;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			// Token: 0x0600A2AC RID: 41644 RVA: 0x00365C7B File Offset: 0x00363E7B
			public float BombardTimeRemaining()
			{
				return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
			}

			// Token: 0x0600A2AD RID: 41645 RVA: 0x00365CA4 File Offset: 0x00363EA4
			public float GetBombardOffTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOff.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (id != null)
					{
						if (!(id == "Infrequent"))
						{
							if (!(id == "Intense"))
							{
								if (id == "Doomed")
								{
									num *= 0.5f;
								}
							}
							else
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
				}
				return num;
			}

			// Token: 0x0600A2AE RID: 41646 RVA: 0x00365D2C File Offset: 0x00363F2C
			public float GetBombardOnTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOn.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (id != null)
					{
						if (!(id == "Infrequent"))
						{
							if (!(id == "Intense"))
							{
								if (id == "Doomed")
								{
									num *= 1f;
								}
							}
							else
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
				}
				return num;
			}

			// Token: 0x0600A2AF RID: 41647 RVA: 0x00365DB4 File Offset: 0x00363FB4
			private float GetNextMeteorTime()
			{
				float num = this.gameplayEvent.secondsPerMeteor;
				num *= 256f / (float)this.world.Width;
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (id != null)
					{
						if (!(id == "Infrequent"))
						{
							if (!(id == "Intense"))
							{
								if (id == "Doomed")
								{
									num *= 0.5f;
								}
							}
							else
							{
								num *= 1.5f;
							}
						}
						else
						{
							num *= 1.5f;
						}
					}
				}
				return num;
			}

			// Token: 0x04008E4C RID: 36428
			public GameObject activeMeteorBackground;

			// Token: 0x04008E4D RID: 36429
			[Serialize]
			public float clusterTravelDuration = -1f;

			// Token: 0x04008E4E RID: 36430
			[Serialize]
			private float nextMeteorTime;

			// Token: 0x04008E4F RID: 36431
			[Serialize]
			private int m_worldId;

			// Token: 0x04008E50 RID: 36432
			private WorldContainer world;

			// Token: 0x04008E51 RID: 36433
			private SettingLevel difficultyLevel;
		}
	}
}
