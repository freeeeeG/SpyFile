using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
public class JettisonableCargoModule : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>
{
	// Token: 0x06004957 RID: 18775 RVA: 0x0019D498 File Offset: 0x0019B698
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").Update(delegate(JettisonableCargoModule.StatesInstance smi, float dt)
		{
			if (smi.CheckReadyForFinalDeploy())
			{
				smi.FinalDeploy();
				smi.GoTo(smi.sm.not_grounded.empty);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ClusterLocationChanged, (JettisonableCargoModule.StatesInstance smi) => Game.Instance, this.not_grounded, null).Exit(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CancelPendingDeploy();
		});
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
	}

	// Token: 0x0400303D RID: 12349
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x0400303E RID: 12350
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Signal emptyCargo;

	// Token: 0x0400303F RID: 12351
	public JettisonableCargoModule.GroundedStates grounded;

	// Token: 0x04003040 RID: 12352
	public JettisonableCargoModule.NotGroundedStates not_grounded;

	// Token: 0x0200181C RID: 6172
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007107 RID: 28935
		public DefComponent<Storage> landerContainer;

		// Token: 0x04007108 RID: 28936
		public Tag landerPrefabID;

		// Token: 0x04007109 RID: 28937
		public Vector3 cargoDropOffset;

		// Token: 0x0400710A RID: 28938
		public string clusterMapFXPrefabID;
	}

	// Token: 0x0200181D RID: 6173
	public class GroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x0400710B RID: 28939
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x0400710C RID: 28940
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x0200181E RID: 6174
	public class NotGroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x0400710D RID: 28941
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x0400710E RID: 28942
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State emptying;

		// Token: 0x0400710F RID: 28943
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x0200181F RID: 6175
	public class StatesInstance : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x060090A2 RID: 37026 RVA: 0x00325C13 File Offset: 0x00323E13
		public StatesInstance(IStateMachineTarget master, JettisonableCargoModule.Def def) : base(master, def)
		{
			this.landerContainer = def.landerContainer.Get(this);
		}

		// Token: 0x060090A3 RID: 37027 RVA: 0x00325C30 File Offset: 0x00323E30
		private void ChooseLanderLocation()
		{
			ClusterGridEntity stableOrbitAsteroid = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid();
			if (stableOrbitAsteroid != null)
			{
				WorldContainer component = stableOrbitAsteroid.GetComponent<WorldContainer>();
				Placeable component2 = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
				component2.restrictWorldId = component.id;
				component.LookAtSurface();
				ClusterManager.Instance.SetActiveWorld(component.id);
				ManagementMenu.Instance.CloseAll();
				PlaceTool.Instance.Activate(component2, new Action<Placeable, int>(this.OnLanderPlaced));
			}
		}

		// Token: 0x060090A4 RID: 37028 RVA: 0x00325CC8 File Offset: 0x00323EC8
		private void OnLanderPlaced(Placeable lander, int cell)
		{
			this.landerPlaced = true;
			this.landerPlacementCell = cell;
			if (lander.GetComponent<MinionStorage>() != null)
			{
				this.OpenMoveChoreForChosenDuplicant();
			}
			ManagementMenu.Instance.ToggleClusterMap();
			base.sm.emptyCargo.Trigger(base.smi);
			ClusterMapScreen.Instance.SelectEntity(base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<ClusterGridEntity>(), true);
		}

		// Token: 0x060090A5 RID: 37029 RVA: 0x00325D34 File Offset: 0x00323F34
		private void OpenMoveChoreForChosenDuplicant()
		{
			RocketModuleCluster component = base.master.GetComponent<RocketModuleCluster>();
			Clustercraft craft = component.CraftInterface.GetComponent<Clustercraft>();
			MinionStorage storage = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			this.EnableTeleport(true);
			this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().SetModuleDeployChore(this.landerPlacementCell, delegate(Chore obj)
			{
				Game.Instance.assignmentManager.RemoveFromWorld(this.ChosenDuplicant.assignableProxy.Get(), craft.ModuleInterface.GetInteriorWorld().id);
				craft.ModuleInterface.GetPassengerModule().RemoveRocketPassenger(this.ChosenDuplicant);
				storage.SerializeMinion(this.ChosenDuplicant.gameObject);
				this.EnableTeleport(false);
			});
		}

		// Token: 0x060090A6 RID: 37030 RVA: 0x00325DB8 File Offset: 0x00323FB8
		private void EnableTeleport(bool enable)
		{
			ClustercraftExteriorDoor component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponent<ClustercraftExteriorDoor>();
			ClustercraftInteriorDoor interiorDoor = component.GetInteriorDoor();
			AccessControl component2 = component.GetInteriorDoor().GetComponent<AccessControl>();
			NavTeleporter component3 = base.GetComponent<NavTeleporter>();
			if (enable)
			{
				component3.SetOverrideCell(this.landerPlacementCell);
				interiorDoor.GetComponent<NavTeleporter>().SetTarget(component3);
				component3.SetTarget(interiorDoor.GetComponent<NavTeleporter>());
				using (List<MinionIdentity>.Enumerator enumerator = Components.MinionIdentities.GetWorldItems(interiorDoor.GetMyWorldId(), false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MinionIdentity minionIdentity = enumerator.Current;
						component2.SetPermission(minionIdentity.assignableProxy.Get(), (minionIdentity == this.ChosenDuplicant) ? AccessControl.Permission.Both : AccessControl.Permission.Neither);
					}
					return;
				}
			}
			component3.SetOverrideCell(-1);
			interiorDoor.GetComponent<NavTeleporter>().SetTarget(null);
			component3.SetTarget(null);
			component2.SetPermission(this.ChosenDuplicant.assignableProxy.Get(), AccessControl.Permission.Neither);
		}

		// Token: 0x060090A7 RID: 37031 RVA: 0x00325ED0 File Offset: 0x003240D0
		public void FinalDeploy()
		{
			this.landerPlaced = false;
			Placeable component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
			this.landerContainer.FindFirst(base.def.landerPrefabID);
			this.landerContainer.Drop(component.gameObject, true);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			TreeFilterable component3 = component.GetComponent<TreeFilterable>();
			if (component3 != null)
			{
				component3.UpdateFilters(component2.AcceptedTags);
			}
			Storage component4 = component.GetComponent<Storage>();
			if (component4 != null)
			{
				Storage[] components = base.gameObject.GetComponents<Storage>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].Transfer(component4, false, true);
				}
			}
			Vector3 position = Grid.CellToPosCBC(this.landerPlacementCell, Grid.SceneLayer.Building);
			component.transform.SetPosition(position);
			component.gameObject.SetActive(true);
			base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().gameObject.Trigger(1792516731, component);
			component.Trigger(1792516731, base.gameObject);
			GameObject gameObject = Assets.TryGetPrefab(base.smi.def.clusterMapFXPrefabID);
			if (gameObject != null)
			{
				this.clusterMapFX = GameUtil.KInstantiate(gameObject, Grid.SceneLayer.Background, null, 0);
				this.clusterMapFX.SetActive(true);
				this.clusterMapFX.GetComponent<ClusterFXEntity>().Init(component.GetMyWorldLocation(), Vector3.zero);
				component.Subscribe(1969584890, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
				component.Subscribe(1591811118, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
			}
		}

		// Token: 0x060090A8 RID: 37032 RVA: 0x00326078 File Offset: 0x00324278
		public bool CheckReadyForFinalDeploy()
		{
			MinionStorage component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			return !(component != null) || component.GetStoredMinionInfo().Count > 0;
		}

		// Token: 0x060090A9 RID: 37033 RVA: 0x003260BA File Offset: 0x003242BA
		public void CancelPendingDeploy()
		{
			this.landerPlaced = false;
			if (this.ChosenDuplicant != null && this.CheckIfLoaded())
			{
				this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().CancelModuleDeployChore();
			}
		}

		// Token: 0x060090AA RID: 37034 RVA: 0x003260EC File Offset: 0x003242EC
		public bool CheckIfLoaded()
		{
			bool flag = false;
			using (List<GameObject>.Enumerator enumerator = this.landerContainer.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PrefabID() == base.def.landerPrefabID)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x00326184 File Offset: 0x00324384
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid() != null;
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060090AC RID: 37036 RVA: 0x003261A1 File Offset: 0x003243A1
		// (set) Token: 0x060090AD RID: 37037 RVA: 0x003261A9 File Offset: 0x003243A9
		public bool AutoDeploy { get; set; }

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060090AE RID: 37038 RVA: 0x003261B2 File Offset: 0x003243B2
		public bool CanAutoDeploy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060090AF RID: 37039 RVA: 0x003261B5 File Offset: 0x003243B5
		public void EmptyCargo()
		{
			this.ChooseLanderLocation();
		}

		// Token: 0x060090B0 RID: 37040 RVA: 0x003261C0 File Offset: 0x003243C0
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation() && (!this.ChooseDuplicant || this.ChosenDuplicant != null) && !this.landerPlaced;
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060090B1 RID: 37041 RVA: 0x00326210 File Offset: 0x00324410
		public bool ChooseDuplicant
		{
			get
			{
				GameObject gameObject = this.landerContainer.FindFirst(base.def.landerPrefabID);
				return !(gameObject == null) && gameObject.GetComponent<MinionStorage>() != null;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060090B2 RID: 37042 RVA: 0x0032624B File Offset: 0x0032444B
		public bool ModuleDeployed
		{
			get
			{
				return this.landerPlaced;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060090B3 RID: 37043 RVA: 0x00326253 File Offset: 0x00324453
		// (set) Token: 0x060090B4 RID: 37044 RVA: 0x0032625B File Offset: 0x0032445B
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return this.chosenDuplicant;
			}
			set
			{
				this.chosenDuplicant = value;
			}
		}

		// Token: 0x04007110 RID: 28944
		private Storage landerContainer;

		// Token: 0x04007111 RID: 28945
		private bool landerPlaced;

		// Token: 0x04007112 RID: 28946
		private MinionIdentity chosenDuplicant;

		// Token: 0x04007113 RID: 28947
		private int landerPlacementCell;

		// Token: 0x04007114 RID: 28948
		public GameObject clusterMapFX;
	}
}
