using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DEF RID: 3567
	public class PeriodicEmoteSickness : Sickness.SicknessComponent
	{
		// Token: 0x06006D96 RID: 28054 RVA: 0x002B39B4 File Offset: 0x002B1BB4
		public PeriodicEmoteSickness(Emote emote, float cooldown)
		{
			this.emote = emote;
			this.cooldown = cooldown;
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x002B39CA File Offset: 0x002B1BCA
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
			statesInstance.StartSM();
			return statesInstance;
		}

		// Token: 0x06006D98 RID: 28056 RVA: 0x002B39D9 File Offset: 0x002B1BD9
		public override void OnCure(GameObject go, object instance_data)
		{
			((PeriodicEmoteSickness.StatesInstance)instance_data).StopSM("Cured");
		}

		// Token: 0x04005228 RID: 21032
		private Emote emote;

		// Token: 0x04005229 RID: 21033
		private float cooldown;

		// Token: 0x02001F6A RID: 8042
		public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600A25D RID: 41565 RVA: 0x00364104 File Offset: 0x00362304
			public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness) : base(master)
			{
				this.periodicEmoteSickness = periodicEmoteSickness;
			}

			// Token: 0x0600A25E RID: 41566 RVA: 0x00364114 File Offset: 0x00362314
			public Reactable GetReactable()
			{
				return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity, 0f).SetEmote(this.periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
			}

			// Token: 0x04008E0D RID: 36365
			public PeriodicEmoteSickness periodicEmoteSickness;
		}

		// Token: 0x02001F6B RID: 8043
		public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600A25F RID: 41567 RVA: 0x0036417E File Offset: 0x0036237E
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.root.ToggleReactable((PeriodicEmoteSickness.StatesInstance smi) => smi.GetReactable());
			}
		}
	}
}
