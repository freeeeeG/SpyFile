using System;
using System.Collections;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001304 RID: 4868
	public class RunTriggerActions : Behaviour
	{
		// Token: 0x0600603F RID: 24639 RVA: 0x001199BA File Offset: 0x00117BBA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.AttachValues(this._stoppingResistanceStat);
			}
			Characters.Actions.Action[] array;
			if (this._trigger.IsSatisfied())
			{
				array = this._successActions;
			}
			else
			{
				array = this._failActions;
			}
			if (array != null && array.Length != 0)
			{
				foreach (Characters.Actions.Action action in array)
				{
					while (controller.character.stunedOrFreezed)
					{
						yield return null;
					}
					action.TryStart();
					while (action.running)
					{
						yield return null;
					}
					action = null;
				}
				Characters.Actions.Action[] array2 = null;
				if (character.type == Character.Type.Adventurer)
				{
					character.stat.DetachValues(this._stoppingResistanceStat);
				}
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004D78 RID: 19832
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04004D79 RID: 19833
		[SerializeField]
		private Characters.Actions.Action[] _successActions;

		// Token: 0x04004D7A RID: 19834
		[SerializeField]
		private Characters.Actions.Action[] _failActions;

		// Token: 0x04004D7B RID: 19835
		private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0)
		});
	}
}
