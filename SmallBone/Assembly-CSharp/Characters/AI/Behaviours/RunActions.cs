using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012FE RID: 4862
	public class RunActions : Behaviour
	{
		// Token: 0x06006026 RID: 24614 RVA: 0x0011951E File Offset: 0x0011771E
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.AttachValues(this._stoppingResistanceStat);
			}
			if (this._actions == null)
			{
				throw new NullReferenceException();
			}
			if (this._actions.Length == 0)
			{
				Debug.LogError("The number of actions is 0");
			}
			foreach (Characters.Actions.Action action in this._actions)
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
			Characters.Actions.Action[] array = null;
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.DetachValues(this._stoppingResistanceStat);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004D5A RID: 19802
		[SerializeField]
		private Characters.Actions.Action[] _actions;

		// Token: 0x04004D5B RID: 19803
		private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0)
		});
	}
}
