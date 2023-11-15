using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000992 RID: 2450
public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	// Token: 0x06004853 RID: 18515 RVA: 0x00197D48 File Offset: 0x00195F48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x00197E07 File Offset: 0x00196007
	public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x04002FEC RID: 12268
	public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;

	// Token: 0x04002FED RID: 12269
	public ClusterMapMeteorShower.TravelingState traveling;

	// Token: 0x04002FEE RID: 12270
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;

	// Token: 0x02001807 RID: 6151
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009048 RID: 36936 RVA: 0x00324E98 File Offset: 0x00323098
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
			List<Descriptor> list = new List<Descriptor>();
			ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
			if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
				{
					num += bombardmentInfo.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo2 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(bombardmentInfo2.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent((float)Mathf.RoundToInt(bombardmentInfo2.weight / num * 100f), GameUtil.TimeSlice.None);
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP, Descriptor.DescriptorType.Effect, false);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x040070BE RID: 28862
		public string name;

		// Token: 0x040070BF RID: 28863
		public string description;

		// Token: 0x040070C0 RID: 28864
		public string description_Hidden;

		// Token: 0x040070C1 RID: 28865
		public string name_Hidden;

		// Token: 0x040070C2 RID: 28866
		public string eventID;

		// Token: 0x040070C3 RID: 28867
		public int destinationWorldID;

		// Token: 0x040070C4 RID: 28868
		public float arrivalTime;
	}

	// Token: 0x02001808 RID: 6152
	public class TravelingState : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
	{
		// Token: 0x040070C5 RID: 28869
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;

		// Token: 0x040070C6 RID: 28870
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
	}

	// Token: 0x02001809 RID: 6153
	public new class Instance : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600904B RID: 36939 RVA: 0x00324FF4 File Offset: 0x003231F4
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600904C RID: 36940 RVA: 0x00325006 File Offset: 0x00323206
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identify";
				}
				return "Dev Hide";
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600904D RID: 36941 RVA: 0x00325030 File Offset: 0x00323230
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identifies the meteor shower";
				}
				return "Dev unidentify back";
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600904E RID: 36942 RVA: 0x0032505A File Offset: 0x0032325A
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600904F RID: 36943 RVA: 0x0032506D File Offset: 0x0032326D
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x06009050 RID: 36944 RVA: 0x00325075 File Offset: 0x00323275
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x06009051 RID: 36945 RVA: 0x00325082 File Offset: 0x00323282
		public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x06009052 RID: 36946 RVA: 0x003250C1 File Offset: 0x003232C1
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x06009053 RID: 36947 RVA: 0x003250D3 File Offset: 0x003232D3
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x06009054 RID: 36948 RVA: 0x003250E8 File Offset: 0x003232E8
		public void Identify()
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress = 1f;
				base.sm.IsIdentified.Set(true, this, false);
				Game.Instance.Trigger(1427028915, this);
				this.RefreshVisuals(true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal", false));
				}
			}
		}

		// Token: 0x06009055 RID: 36949 RVA: 0x00325150 File Offset: 0x00323350
		public void ProgressIdentifiction(float points)
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress += points;
				this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0f, 1f);
				if (this.identifyingProgress == 1f)
				{
					this.Identify();
				}
			}
		}

		// Token: 0x06009056 RID: 36950 RVA: 0x003251A1 File Offset: 0x003233A1
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			this.RefreshVisuals(false);
		}

		// Token: 0x06009057 RID: 36951 RVA: 0x003251D8 File Offset: 0x003233D8
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (this.HasBeenIdentified)
			{
				this.selectable.SetName(base.def.name);
				this.descriptor.description = base.def.description;
				this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				this.selectable.SetName(base.def.name_Hidden);
				this.descriptor.description = base.def.description_Hidden;
				this.visualizer.PlayHideAnimation();
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x06009058 RID: 36952 RVA: 0x0032526C File Offset: 0x0032346C
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x06009059 RID: 36953 RVA: 0x003252DF File Offset: 0x003234DF
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600905A RID: 36954 RVA: 0x003252E7 File Offset: 0x003234E7
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x0600905B RID: 36955 RVA: 0x003252F9 File Offset: 0x003234F9
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600905C RID: 36956 RVA: 0x00325300 File Offset: 0x00323500
		public bool SidescreenEnabled()
		{
			return false;
		}

		// Token: 0x0600905D RID: 36957 RVA: 0x00325303 File Offset: 0x00323503
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x0600905E RID: 36958 RVA: 0x00325306 File Offset: 0x00323506
		public void OnSidescreenButtonPressed()
		{
			this.Identify();
		}

		// Token: 0x0600905F RID: 36959 RVA: 0x0032530E File Offset: 0x0032350E
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x00325311 File Offset: 0x00323511
		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}

		// Token: 0x040070C7 RID: 28871
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x040070C8 RID: 28872
		[Serialize]
		public float ArrivalTime;

		// Token: 0x040070C9 RID: 28873
		[Serialize]
		private float Speed;

		// Token: 0x040070CA RID: 28874
		[Serialize]
		private float identifyingProgress;

		// Token: 0x040070CB RID: 28875
		public System.Action OnDestinationReached;

		// Token: 0x040070CC RID: 28876
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x040070CD RID: 28877
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x040070CE RID: 28878
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x040070CF RID: 28879
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x040070D0 RID: 28880
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
