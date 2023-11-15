using System;
using UnityEngine;

// Token: 0x02000987 RID: 2439
public class ArtifactHarvestModule : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>
{
	// Token: 0x060047FD RID: 18429 RVA: 0x001964DC File Offset: 0x001946DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		});
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.not_grounded.DefaultState(this.not_grounded.not_harvesting).EventHandler(GameHashes.ClusterLocationChanged, (ArtifactHarvestModule.StatesInstance smi) => Game.Instance, delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).EventHandler(GameHashes.OnStorageChange, delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.not_harvesting.PlayAnim("loaded").ParamTransition<bool>(this.canHarvest, this.not_grounded.harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsTrue);
		this.not_grounded.harvesting.PlayAnim("deploying").Update(delegate(ArtifactHarvestModule.StatesInstance smi, float dt)
		{
			smi.HarvestFromPOI(dt);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<bool>(this.canHarvest, this.not_grounded.not_harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsFalse);
	}

	// Token: 0x04002FB9 RID: 12217
	public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.BoolParameter canHarvest;

	// Token: 0x04002FBA RID: 12218
	public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State grounded;

	// Token: 0x04002FBB RID: 12219
	public ArtifactHarvestModule.NotGroundedStates not_grounded;

	// Token: 0x020017F4 RID: 6132
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017F5 RID: 6133
	public class NotGroundedStates : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State
	{
		// Token: 0x04007075 RID: 28789
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State not_harvesting;

		// Token: 0x04007076 RID: 28790
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State harvesting;
	}

	// Token: 0x020017F6 RID: 6134
	public class StatesInstance : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameInstance
	{
		// Token: 0x06008FF9 RID: 36857 RVA: 0x00324472 File Offset: 0x00322672
		public StatesInstance(IStateMachineTarget master, ArtifactHarvestModule.Def def) : base(master, def)
		{
		}

		// Token: 0x06008FFA RID: 36858 RVA: 0x0032447C File Offset: 0x0032267C
		public void HarvestFromPOI(float dt)
		{
			ClusterGridEntity poiatCurrentLocation = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetPOIAtCurrentLocation();
			if (poiatCurrentLocation.IsNullOrDestroyed())
			{
				return;
			}
			ArtifactPOIStates.Instance smi = poiatCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
			if ((poiatCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()) && !smi.IsNullOrDestroyed())
			{
				bool flag = false;
				string artifactToHarvest = smi.GetArtifactToHarvest();
				if (artifactToHarvest != null)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(artifactToHarvest), base.transform.position);
					gameObject.SetActive(true);
					this.receptacle.ForceDeposit(gameObject);
					this.storage.Store(gameObject, false, false, true, false);
					smi.HarvestArtifact();
					if (smi.configuration.DestroyOnHarvest())
					{
						flag = true;
					}
					if (flag)
					{
						poiatCurrentLocation.gameObject.DeleteObject();
					}
				}
			}
		}

		// Token: 0x06008FFB RID: 36859 RVA: 0x00324544 File Offset: 0x00322744
		public bool CheckIfCanHarvest()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component == null)
			{
				return false;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			if (poiatCurrentLocation != null && (poiatCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()))
			{
				ArtifactPOIStates.Instance smi = poiatCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.CanHarvestArtifact() && this.receptacle.Occupant == null)
				{
					base.sm.canHarvest.Set(true, this, false);
					return true;
				}
			}
			base.sm.canHarvest.Set(false, this, false);
			return false;
		}

		// Token: 0x04007077 RID: 28791
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04007078 RID: 28792
		[MyCmpReq]
		private SingleEntityReceptacle receptacle;
	}
}
