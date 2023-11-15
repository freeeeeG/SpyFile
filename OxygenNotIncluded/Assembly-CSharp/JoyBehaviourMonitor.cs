using System;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class JoyBehaviourMonitor : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance>
{
	// Token: 0x06003F72 RID: 16242 RVA: 0x00162770 File Offset: 0x00160970
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.EventHandler(GameHashes.TagsChanged, delegate(JoyBehaviourMonitor.Instance smi, object data)
		{
			TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
			if (!tagChangedEventData.added)
			{
				return;
			}
			if (tagChangedEventData.tag == GameTags.PleasantConversation && UnityEngine.Random.Range(0f, 100f) <= 1f)
			{
				smi.GoToOverjoyed();
			}
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).EventHandler(GameHashes.SleepFinished, delegate(JoyBehaviourMonitor.Instance smi)
		{
			if (smi.ShouldBeOverjoyed())
			{
				smi.GoToOverjoyed();
			}
		});
		this.overjoyed.Transition(this.neutral, (JoyBehaviourMonitor.Instance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms).ToggleExpression((JoyBehaviourMonitor.Instance smi) => smi.happyExpression).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoAnim).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoWalkAnim).ToggleTag(GameTags.Overjoyed).Exit(delegate(JoyBehaviourMonitor.Instance smi)
		{
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).OnSignal(this.exitEarly, this.neutral);
	}

	// Token: 0x04002933 RID: 10547
	public StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Signal exitEarly;

	// Token: 0x04002934 RID: 10548
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04002935 RID: 10549
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State overjoyed;

	// Token: 0x0200168A RID: 5770
	public new class Instance : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B4D RID: 35661 RVA: 0x003162F0 File Offset: 0x003144F0
		public Instance(IStateMachineTarget master, string happy_loco_anim, string happy_loco_walk_anim, Expression happy_expression) : base(master)
		{
			this.happyLocoAnim = happy_loco_anim;
			this.happyLocoWalkAnim = happy_loco_walk_anim;
			this.happyExpression = happy_expression;
			Attributes attributes = base.gameObject.GetAttributes();
			this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
			this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
		}

		// Token: 0x06008B4E RID: 35662 RVA: 0x00316378 File Offset: 0x00314578
		public bool ShouldBeOverjoyed()
		{
			float totalValue = this.qolAttribute.GetTotalValue();
			float totalValue2 = this.expectationAttribute.GetTotalValue();
			float num = totalValue - totalValue2;
			if (num >= TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS)
			{
				float num2 = MathUtil.ReRange(num, TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MAX_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MIN_REACTION_CHANCE, TRAITS.JOY_REACTIONS.MAX_REACTION_CHANCE);
				return UnityEngine.Random.Range(0f, 100f) <= num2;
			}
			return false;
		}

		// Token: 0x06008B4F RID: 35663 RVA: 0x003163D9 File Offset: 0x003145D9
		public void GoToOverjoyed()
		{
			base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
			base.smi.GoTo(base.smi.sm.overjoyed);
		}

		// Token: 0x04006C0C RID: 27660
		public string happyLocoAnim = "";

		// Token: 0x04006C0D RID: 27661
		public string happyLocoWalkAnim = "";

		// Token: 0x04006C0E RID: 27662
		public Expression happyExpression;

		// Token: 0x04006C0F RID: 27663
		[Serialize]
		public float transitionTime;

		// Token: 0x04006C10 RID: 27664
		private AttributeInstance expectationAttribute;

		// Token: 0x04006C11 RID: 27665
		private AttributeInstance qolAttribute;
	}
}
