using System;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
	// Token: 0x06001A7F RID: 6783 RVA: 0x0008D520 File Offset: 0x0008B720
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (ClimbableTreeMonitor.Instance smi) => smi.UpdateHasClimbable(), delegate(ClimbableTreeMonitor.Instance smi)
		{
			smi.OnClimbComplete();
		});
	}

	// Token: 0x04000EB9 RID: 3769
	private const int MAX_NAV_COST = 2147483647;

	// Token: 0x02001121 RID: 4385
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B62 RID: 23394
		public float searchMinInterval = 60f;

		// Token: 0x04005B63 RID: 23395
		public float searchMaxInterval = 120f;
	}

	// Token: 0x02001122 RID: 4386
	public new class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
	{
		// Token: 0x06007866 RID: 30822 RVA: 0x002D6466 File Offset: 0x002D4666
		public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x002D6476 File Offset: 0x002D4676
		private void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x002D64A4 File Offset: 0x002D46A4
		public bool UpdateHasClimbable()
		{
			if (this.climbTarget == null)
			{
				if (Time.time < this.nextSearchTime)
				{
					return false;
				}
				this.FindClimbableTree();
				this.RefreshSearchTime();
			}
			return this.climbTarget != null;
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x002D64DC File Offset: 0x002D46DC
		private void FindClimbableTree()
		{
			this.climbTarget = null;
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList pooledList2 = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.plants, pooledList);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, pooledList);
			Navigator component = base.GetComponent<Navigator>();
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KMonoBehaviour kmonoBehaviour = scenePartitionerEntry.obj as KMonoBehaviour;
				if (!kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature))
				{
					int cell = Grid.PosToCell(kmonoBehaviour);
					if (component.CanReach(cell))
					{
						BuddingTrunk component2 = kmonoBehaviour.GetComponent<BuddingTrunk>();
						StorageLocker component3 = kmonoBehaviour.GetComponent<StorageLocker>();
						if (component2 != null)
						{
							if (!component2.ExtraSeedAvailable)
							{
								continue;
							}
						}
						else
						{
							if (!(component3 != null))
							{
								continue;
							}
							Storage component4 = component3.GetComponent<Storage>();
							if (!component4.allowItemRemoval || component4.IsEmpty())
							{
								continue;
							}
						}
						pooledList2.Add(kmonoBehaviour);
					}
				}
			}
			if (pooledList2.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, pooledList2.Count);
				KMonoBehaviour kmonoBehaviour2 = pooledList2[index];
				this.climbTarget = kmonoBehaviour2.gameObject;
			}
			pooledList.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x002D664C File Offset: 0x002D484C
		public void OnClimbComplete()
		{
			this.climbTarget = null;
		}

		// Token: 0x04005B64 RID: 23396
		public GameObject climbTarget;

		// Token: 0x04005B65 RID: 23397
		public float nextSearchTime;
	}
}
