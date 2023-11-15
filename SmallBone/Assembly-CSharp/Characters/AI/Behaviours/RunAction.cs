using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012F6 RID: 4854
	public sealed class RunAction : Behaviour
	{
		// Token: 0x06005FFC RID: 24572 RVA: 0x00118FA2 File Offset: 0x001171A2
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.AttachValues(this._stoppingResistanceStat);
			}
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running && base.result == Behaviour.Result.Doing)
			{
				yield return null;
			}
			if (character.type == Character.Type.Adventurer)
			{
				character.stat.DetachValues(this._stoppingResistanceStat);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004D39 RID: 19769
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004D3A RID: 19770
		private Stat.Values _stoppingResistanceStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Percent, Stat.Kind.StoppingResistance, 0.0)
		});
	}
}
