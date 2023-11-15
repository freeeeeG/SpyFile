using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C9 RID: 5065
	public class BehaviourTemplate : Behaviour
	{
		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x060063D2 RID: 25554 RVA: 0x00121E88 File Offset: 0x00120088
		// (set) Token: 0x060063D3 RID: 25555 RVA: 0x00121E90 File Offset: 0x00120090
		public bool canUse { get; private set; } = true;

		// Token: 0x060063D4 RID: 25556 RVA: 0x00121E99 File Offset: 0x00120099
		public override IEnumerator CRun(AIController controller)
		{
			if (this._actions.Length < 0)
			{
				Debug.LogError("Action length is 0");
				yield break;
			}
			if (!this.canUse)
			{
				yield break;
			}
			if (this._coolTime > 0f)
			{
				base.StartCoroutine(this.CCoolDown(controller.character.chronometer.master));
			}
			foreach (Characters.Actions.Action action in this._actions)
			{
				action.TryStart();
				while (action.running)
				{
					yield return null;
				}
				action = null;
			}
			Characters.Actions.Action[] array = null;
			yield break;
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x00121EAF File Offset: 0x001200AF
		private IEnumerator CCoolDown(Chronometer chronometer)
		{
			this.canUse = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this.canUse = true;
			yield break;
		}

		// Token: 0x04005078 RID: 20600
		[SerializeField]
		private float _coolTime;

		// Token: 0x04005079 RID: 20601
		[SerializeField]
		private Characters.Actions.Action[] _actions;
	}
}
