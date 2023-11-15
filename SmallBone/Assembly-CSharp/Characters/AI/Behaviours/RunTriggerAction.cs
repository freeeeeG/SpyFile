using System;
using System.Collections;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001302 RID: 4866
	public sealed class RunTriggerAction : Behaviour
	{
		// Token: 0x06006037 RID: 24631 RVA: 0x0011984B File Offset: 0x00117A4B
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.AttachValues(this._stoppingResistanceStat);
			}
			Characters.Actions.Action action;
			if (this._trigger.IsSatisfied())
			{
				action = this._successAction;
			}
			else
			{
				action = this._failAction;
			}
			if (action != null)
			{
				if (!action.TryStart())
				{
					base.result = Behaviour.Result.Fail;
					yield break;
				}
				while (action.running && base.result == Behaviour.Result.Doing)
				{
					yield return null;
				}
				if (character.type == Character.Type.Adventurer)
				{
					character.stat.DetachValues(this._stoppingResistanceStat);
				}
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004D6E RID: 19822
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04004D6F RID: 19823
		[SerializeField]
		private Characters.Actions.Action _successAction;

		// Token: 0x04004D70 RID: 19824
		[SerializeField]
		private Characters.Actions.Action _failAction;

		// Token: 0x04004D71 RID: 19825
		private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0)
		});
	}
}
