using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000650 RID: 1616
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06002A95 RID: 10901 RVA: 0x000E2F95 File Offset: 0x000E1195
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x000E2FBB File Offset: 0x000E11BB
	protected override void OnCompleteWork(Worker worker)
	{
		worker.workable.GetComponent<Edible>().CompleteWork(worker);
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000E2FCE File Offset: 0x000E11CE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new MessStation.MessStationSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000E2FF0 File Offset: 0x000E11F0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06002A99 RID: 10905 RVA: 0x000E305A File Offset: 0x000E125A
	public bool HasSalt
	{
		get
		{
			return this.smi.HasSalt;
		}
	}

	// Token: 0x040018E3 RID: 6371
	private MessStation.MessStationSM.Instance smi;

	// Token: 0x02001328 RID: 4904
	public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
	{
		// Token: 0x06007FFE RID: 32766 RVA: 0x002EF714 File Offset: 0x002ED914
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.salt.none;
			this.salt.none.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("off");
			this.salt.salty.Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, null);
			this.eating.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).PlayAnim("off");
		}

		// Token: 0x040061A3 RID: 24995
		public MessStation.MessStationSM.SaltState salt;

		// Token: 0x040061A4 RID: 24996
		public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

		// Token: 0x020020FC RID: 8444
		public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
		{
			// Token: 0x040093B8 RID: 37816
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;

			// Token: 0x040093B9 RID: 37817
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
		}

		// Token: 0x020020FD RID: 8445
		public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
		{
			// Token: 0x0600A83A RID: 43066 RVA: 0x00370421 File Offset: 0x0036E621
			public Instance(MessStation master) : base(master)
			{
				this.saltStorage = master.GetComponent<Storage>();
				this.assigned = master.GetComponent<Assignable>();
			}

			// Token: 0x17000A76 RID: 2678
			// (get) Token: 0x0600A83B RID: 43067 RVA: 0x00370442 File Offset: 0x0036E642
			public bool HasSalt
			{
				get
				{
					return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
				}
			}

			// Token: 0x0600A83C RID: 43068 RVA: 0x0037045C File Offset: 0x0036E65C
			public bool IsEating()
			{
				if (this.assigned == null || this.assigned.assignee == null)
				{
					return false;
				}
				Ownables soleOwner = this.assigned.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return false;
				}
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject == null)
				{
					return false;
				}
				ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
				return component != null && component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
			}

			// Token: 0x040093BA RID: 37818
			private Storage saltStorage;

			// Token: 0x040093BB RID: 37819
			private Assignable assigned;
		}
	}
}
