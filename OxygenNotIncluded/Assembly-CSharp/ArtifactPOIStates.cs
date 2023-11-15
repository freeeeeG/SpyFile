using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200098A RID: 2442
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
	// Token: 0x0600480C RID: 18444 RVA: 0x00196804 File Offset: 0x00194A04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(ArtifactPOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
				smi.PickNewArtifactToHarvest();
				smi.poiCharge = 1f;
			}
		});
		this.idle.ParamTransition<float>(this.poiCharge, this.recharging, (ArtifactPOIStates.Instance smi, float f) => f < 1f);
		this.recharging.ParamTransition<float>(this.poiCharge, this.idle, (ArtifactPOIStates.Instance smi, float f) => f >= 1f).EventHandler(GameHashes.NewDay, (ArtifactPOIStates.Instance smi) => GameClock.Instance, delegate(ArtifactPOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		});
	}

	// Token: 0x04002FC3 RID: 12227
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State idle;

	// Token: 0x04002FC4 RID: 12228
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;

	// Token: 0x04002FC5 RID: 12229
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

	// Token: 0x020017FB RID: 6139
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017FC RID: 6140
	public new class Instance : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600900D RID: 36877 RVA: 0x00324777 File Offset: 0x00322977
		// (set) Token: 0x0600900E RID: 36878 RVA: 0x0032477F File Offset: 0x0032297F
		public float poiCharge
		{
			get
			{
				return this._poiCharge;
			}
			set
			{
				this._poiCharge = value;
				base.smi.sm.poiCharge.Set(value, base.smi, false);
			}
		}

		// Token: 0x0600900F RID: 36879 RVA: 0x003247A6 File Offset: 0x003229A6
		public Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x06009010 RID: 36880 RVA: 0x003247B0 File Offset: 0x003229B0
		public void PickNewArtifactToHarvest()
		{
			if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
			{
				this.artifactToHarvest = this.configuration.GetArtifactID();
				ArtifactSelector.Instance.ReserveArtifactID(this.artifactToHarvest, ArtifactType.Any);
				return;
			}
			this.artifactToHarvest = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
		}

		// Token: 0x06009011 RID: 36881 RVA: 0x0032480C File Offset: 0x00322A0C
		public string GetArtifactToHarvest()
		{
			if (this.CanHarvestArtifact())
			{
				if (string.IsNullOrEmpty(this.artifactToHarvest))
				{
					this.PickNewArtifactToHarvest();
				}
				return this.artifactToHarvest;
			}
			return null;
		}

		// Token: 0x06009012 RID: 36882 RVA: 0x00324831 File Offset: 0x00322A31
		public void HarvestArtifact()
		{
			if (this.CanHarvestArtifact())
			{
				this.numHarvests++;
				this.poiCharge = 0f;
				this.artifactToHarvest = null;
				this.PickNewArtifactToHarvest();
			}
		}

		// Token: 0x06009013 RID: 36883 RVA: 0x00324864 File Offset: 0x00322A64
		public void RechargePOI(float dt)
		{
			float delta = dt / this.configuration.GetRechargeTime();
			this.DeltaPOICharge(delta);
		}

		// Token: 0x06009014 RID: 36884 RVA: 0x00324886 File Offset: 0x00322A86
		public float RechargeTimeRemaining()
		{
			return (float)Mathf.CeilToInt((this.configuration.GetRechargeTime() - this.configuration.GetRechargeTime() * this.poiCharge) / 600f) * 600f;
		}

		// Token: 0x06009015 RID: 36885 RVA: 0x003248B8 File Offset: 0x00322AB8
		public void DeltaPOICharge(float delta)
		{
			this.poiCharge += delta;
			this.poiCharge = Mathf.Min(1f, this.poiCharge);
		}

		// Token: 0x06009016 RID: 36886 RVA: 0x003248DE File Offset: 0x00322ADE
		public bool CanHarvestArtifact()
		{
			return this.poiCharge >= 1f;
		}

		// Token: 0x06009017 RID: 36887 RVA: 0x003248F0 File Offset: 0x00322AF0
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x0400708C RID: 28812
		[Serialize]
		public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;

		// Token: 0x0400708D RID: 28813
		[Serialize]
		private float _poiCharge;

		// Token: 0x0400708E RID: 28814
		[Serialize]
		public string artifactToHarvest;

		// Token: 0x0400708F RID: 28815
		[Serialize]
		private int numHarvests;
	}
}
