using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000677 RID: 1655
[SerializationConfig(MemberSerialization.OptIn)]
public class RailGunPayload : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>
{
	// Token: 0x06002BF7 RID: 11255 RVA: 0x000E96C0 File Offset: 0x000E78C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded.idle;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.grounded.DefaultState(this.grounded.idle).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(true, smi, false);
		}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailgunpayloadNeedsEmptying, null).ToggleTag(GameTags.RailGunPayloadEmptyable).ToggleTag(GameTags.ClusterEntityGrounded).EventHandler(GameHashes.DroppedAll, delegate(RailGunPayload.StatesInstance smi)
		{
			smi.OnDroppedAll();
		}).OnSignal(this.launch, this.takeoff);
		this.grounded.idle.PlayAnim("idle");
		this.grounded.crater.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = true;
		}).Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = false;
		}).PlayAnim("landed", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.grounded.idle, null);
		this.takeoff.DefaultState(this.takeoff.launch).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).PlayAnim("launching").OnSignal(this.beginTravelling, this.travel);
		this.takeoff.launch.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartTakeoff();
		}).GoTo(this.takeoff.airborne);
		this.takeoff.airborne.Update("Launch", delegate(RailGunPayload.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (RailGunPayload.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.DefaultState(this.landing.landing).ParamTransition<bool>(this.onSurface, this.grounded.crater, GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IsTrue).ParamTransition<int>(this.destinationWorld, this.takeoff, (RailGunPayload.StatesInstance smi, int p) => p != -1).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.landing.PlayAnim("falling", KAnim.PlayMode.Loop).UpdateTransition(this.landing.impact, (RailGunPayload.StatesInstance smi, float dt) => smi.UpdateLanding(dt), UpdateRate.SIM_200ms, false).ToggleGravity(this.landing.impact);
		this.landing.impact.PlayAnim("land").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.crater);
	}

	// Token: 0x040019E5 RID: 6629
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter destinationWorld = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter(-1);

	// Token: 0x040019E6 RID: 6630
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter onSurface = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter(false);

	// Token: 0x040019E7 RID: 6631
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal beginTravelling;

	// Token: 0x040019E8 RID: 6632
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal launch;

	// Token: 0x040019E9 RID: 6633
	public RailGunPayload.TakeoffStates takeoff;

	// Token: 0x040019EA RID: 6634
	public RailGunPayload.TravelStates travel;

	// Token: 0x040019EB RID: 6635
	public RailGunPayload.LandingStates landing;

	// Token: 0x040019EC RID: 6636
	public RailGunPayload.GroundedStates grounded;

	// Token: 0x02001371 RID: 4977
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400626B RID: 25195
		public bool attractToBeacons;

		// Token: 0x0400626C RID: 25196
		public string clusterAnimSymbolSwapTarget;

		// Token: 0x0400626D RID: 25197
		public List<string> randomClusterSymbolSwaps;

		// Token: 0x0400626E RID: 25198
		public string worldAnimSymbolSwapTarget;

		// Token: 0x0400626F RID: 25199
		public List<string> randomWorldSymbolSwaps;
	}

	// Token: 0x02001372 RID: 4978
	public class TakeoffStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006270 RID: 25200
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State launch;

		// Token: 0x04006271 RID: 25201
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State airborne;
	}

	// Token: 0x02001373 RID: 4979
	public class TravelStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006272 RID: 25202
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State travelling;

		// Token: 0x04006273 RID: 25203
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State transferWorlds;
	}

	// Token: 0x02001374 RID: 4980
	public class LandingStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006274 RID: 25204
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State landing;

		// Token: 0x04006275 RID: 25205
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State impact;
	}

	// Token: 0x02001375 RID: 4981
	public class GroundedStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04006276 RID: 25206
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State crater;

		// Token: 0x04006277 RID: 25207
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State idle;
	}

	// Token: 0x02001376 RID: 4982
	public class StatesInstance : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.GameInstance
	{
		// Token: 0x0600810F RID: 33039 RVA: 0x002F39AC File Offset: 0x002F1BAC
		public StatesInstance(IStateMachineTarget master, RailGunPayload.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			DebugUtil.Assert(def.clusterAnimSymbolSwapTarget == null == (def.worldAnimSymbolSwapTarget == null), "Must specify both or neither symbol swap targets!");
			DebugUtil.Assert((def.randomClusterSymbolSwaps == null && def.randomWorldSymbolSwaps == null) || def.randomClusterSymbolSwaps.Count == def.randomWorldSymbolSwaps.Count, "Must specify the same number of swaps for both world and cluster!");
			if (def.clusterAnimSymbolSwapTarget != null && def.worldAnimSymbolSwapTarget != null)
			{
				if (this.randomSymbolSwapIndex == -1)
				{
					this.randomSymbolSwapIndex = UnityEngine.Random.Range(0, def.randomClusterSymbolSwaps.Count);
					global::Debug.Log(string.Format("Rolling a random symbol: {0}", this.randomSymbolSwapIndex), base.gameObject);
				}
				base.GetComponent<BallisticClusterGridEntity>().SwapSymbolFromSameAnim(def.clusterAnimSymbolSwapTarget, def.randomClusterSymbolSwaps[this.randomSymbolSwapIndex]);
				KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(def.randomWorldSymbolSwaps[this.randomSymbolSwapIndex]);
				this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(def.worldAnimSymbolSwapTarget, symbol, 0);
			}
		}

		// Token: 0x06008110 RID: 33040 RVA: 0x002F3AF0 File Offset: 0x002F1CF0
		public void Launch(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.takeoff);
		}

		// Token: 0x06008111 RID: 33041 RVA: 0x002F3B38 File Offset: 0x002F1D38
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06008112 RID: 33042 RVA: 0x002F3B7E File Offset: 0x002F1D7E
		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x002F3BA4 File Offset: 0x002F1DA4
		public void StartLand()
		{
			WorldContainer worldContainer = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			if (worldContainer == null)
			{
				worldContainer = ClusterManager.Instance.GetStartWorld();
			}
			int num = Grid.InvalidCell;
			if (base.def.attractToBeacons)
			{
				num = ClusterManager.Instance.GetLandingBeaconLocation(worldContainer.id);
			}
			int num4;
			if (num != Grid.InvalidCell)
			{
				int num2;
				int num3;
				Grid.CellToXY(num, out num2, out num3);
				int minInclusive = Mathf.Max(num2 - 3, (int)worldContainer.minimumBounds.x);
				int maxExclusive = Mathf.Min(num2 + 3, (int)worldContainer.maximumBounds.x);
				num4 = Mathf.RoundToInt((float)UnityEngine.Random.Range(minInclusive, maxExclusive));
			}
			else
			{
				num4 = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + 3f, worldContainer.maximumBounds.x - 3f));
			}
			Vector3 position = new Vector3((float)num4 + 0.5f, worldContainer.maximumBounds.y - 1f, Grid.GetLayerZ(Grid.SceneLayer.Front));
			base.transform.SetPosition(position);
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			GameComps.Fallers.Add(base.gameObject, new Vector2(0f, -10f));
			base.sm.destinationWorld.Set(-1, this, false);
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x002F3D0C File Offset: 0x002F1F0C
		public void UpdateLaunch(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition() + new Vector3(0f, this.takeoffVelocity * dt, 0f);
				base.transform.SetPosition(position);
				return;
			}
			base.sm.beginTravelling.Trigger(this);
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			if (ClusterGrid.Instance.GetAsteroidAtCell(component.Location) != null)
			{
				base.GetComponent<ClusterTraveler>().AdvancePathOneStep();
			}
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x002F3DA0 File Offset: 0x002F1FA0
		public bool UpdateLanding(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition();
				position.y -= 0.5f;
				int cell = Grid.PosToCell(position);
				if (Grid.IsWorldValidCell(cell) && Grid.IsSolidCell(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008116 RID: 33046 RVA: 0x002F3DF6 File Offset: 0x002F1FF6
		public void OnDroppedAll()
		{
			base.gameObject.DeleteObject();
		}

		// Token: 0x06008117 RID: 33047 RVA: 0x002F3E03 File Offset: 0x002F2003
		public bool IsTraveling()
		{
			return base.IsInsideState(base.sm.travel.travelling);
		}

		// Token: 0x06008118 RID: 33048 RVA: 0x002F3E1C File Offset: 0x002F201C
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, 0f));
		}

		// Token: 0x06008119 RID: 33049 RVA: 0x002F3E68 File Offset: 0x002F2068
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(true);
			}
		}

		// Token: 0x04006278 RID: 25208
		[Serialize]
		public float takeoffVelocity;

		// Token: 0x04006279 RID: 25209
		[Serialize]
		private int randomSymbolSwapIndex = -1;

		// Token: 0x0400627A RID: 25210
		public KBatchedAnimController animController;
	}
}
