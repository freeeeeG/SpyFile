using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012D6 RID: 4822
	public sealed class MassSacrifice : Behaviour
	{
		// Token: 0x06005F62 RID: 24418 RVA: 0x00117788 File Offset: 0x00115988
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (!this._action.TryStart())
			{
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			while (this._action.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06005F63 RID: 24419 RVA: 0x00117798 File Offset: 0x00115998
		public bool CanUse(AIController aiController)
		{
			if (!this._action.canUse)
			{
				return false;
			}
			this._range.transform.position = aiController.target.transform.position;
			List<Character> list = aiController.FindEnemiesInRange(this._range);
			if (list == null || list.Count <= 0)
			{
				return false;
			}
			using (List<Character>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current.GetComponent<SacrificeCharacter>() == null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004CA9 RID: 19625
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004CAA RID: 19626
		[SerializeField]
		private Collider2D _range;
	}
}
