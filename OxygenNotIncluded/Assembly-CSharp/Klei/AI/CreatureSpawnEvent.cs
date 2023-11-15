using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF4 RID: 3572
	public class CreatureSpawnEvent : GameplayEvent<CreatureSpawnEvent.StatesInstance>
	{
		// Token: 0x06006DCF RID: 28111 RVA: 0x002B5058 File Offset: 0x002B3258
		public CreatureSpawnEvent() : base("HatchSpawnEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.DESCRIPTION;
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x002B5087 File Offset: 0x002B3287
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new CreatureSpawnEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400524A RID: 21066
		public const string ID = "HatchSpawnEvent";

		// Token: 0x0400524B RID: 21067
		public const float UPDATE_TIME = 4f;

		// Token: 0x0400524C RID: 21068
		public const float NUM_TO_SPAWN = 10f;

		// Token: 0x0400524D RID: 21069
		public const float duration = 40f;

		// Token: 0x0400524E RID: 21070
		public static List<string> CreatureSpawnEventIDs = new List<string>
		{
			"Hatch",
			"Squirrel",
			"Puft",
			"Crab",
			"Drecko",
			"Mole",
			"LightBug",
			"Pacu"
		};

		// Token: 0x02001F74 RID: 8052
		public class StatesInstance : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A278 RID: 41592 RVA: 0x00364B08 File Offset: 0x00362D08
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, CreatureSpawnEvent creatureEvent) : base(master, eventInstance, creatureEvent)
			{
			}

			// Token: 0x0600A279 RID: 41593 RVA: 0x00364B1E File Offset: 0x00362D1E
			private void PickCreatureToSpawn()
			{
				this.creatureID = CreatureSpawnEvent.CreatureSpawnEventIDs.GetRandom<string>();
			}

			// Token: 0x0600A27A RID: 41594 RVA: 0x00364B30 File Offset: 0x00362D30
			private void PickSpawnLocations()
			{
				Vector3 position = Components.Telepads.Items.GetRandom<Telepad>().transform.GetPosition();
				int num = 100;
				ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
				GameScenePartitioner.Instance.GatherEntries((int)position.x - num / 2, (int)position.y - num / 2, num, num, GameScenePartitioner.Instance.plants, pooledList);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
				{
					KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
					if (!kprefabID.GetComponent<TreeBud>())
					{
						base.smi.spawnPositions.Add(kprefabID.transform.GetPosition());
					}
				}
				pooledList.Recycle();
			}

			// Token: 0x0600A27B RID: 41595 RVA: 0x00364C04 File Offset: 0x00362E04
			public void InitializeEvent()
			{
				this.PickCreatureToSpawn();
				this.PickSpawnLocations();
			}

			// Token: 0x0600A27C RID: 41596 RVA: 0x00364C12 File Offset: 0x00362E12
			public void EndEvent()
			{
				this.creatureID = null;
				this.spawnPositions.Clear();
			}

			// Token: 0x0600A27D RID: 41597 RVA: 0x00364C28 File Offset: 0x00362E28
			public void SpawnCreature()
			{
				if (this.spawnPositions.Count > 0)
				{
					Vector3 random = this.spawnPositions.GetRandom<Vector3>();
					Util.KInstantiate(Assets.GetPrefab(this.creatureID), random).SetActive(true);
				}
			}

			// Token: 0x04008E2D RID: 36397
			[Serialize]
			private List<Vector3> spawnPositions = new List<Vector3>();

			// Token: 0x04008E2E RID: 36398
			[Serialize]
			private string creatureID;
		}

		// Token: 0x02001F75 RID: 8053
		public class States : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>
		{
			// Token: 0x0600A27E RID: 41598 RVA: 0x00364C6C File Offset: 0x00362E6C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.initialize_event;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.initialize_event.Enter(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.InitializeEvent();
					smi.GoTo(this.spawn_season);
				});
				this.start.DoNothing();
				this.spawn_season.Update(delegate(CreatureSpawnEvent.StatesInstance smi, float dt)
				{
					smi.SpawnCreature();
				}, UpdateRate.SIM_4000ms, false).Exit(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.EndEvent();
				});
			}

			// Token: 0x0600A27F RID: 41599 RVA: 0x00364D00 File Offset: 0x00362F00
			public override EventInfoData GenerateEventPopupData(CreatureSpawnEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x04008E2F RID: 36399
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State initialize_event;

			// Token: 0x04008E30 RID: 36400
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State spawn_season;

			// Token: 0x04008E31 RID: 36401
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State start;
		}
	}
}
