using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012B6 RID: 4790
	public class RandomBehaviour : Behaviour
	{
		// Token: 0x06005EDC RID: 24284 RVA: 0x001162EA File Offset: 0x001144EA
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._behaviours == null)
			{
				Debug.LogError("Behaviours length is null");
				yield break;
			}
			if (this._behaviours.components.Length == 0)
			{
				Debug.LogError("Behaviours length is 0");
				yield break;
			}
			BehaviourInfo behaviour = this._behaviours.components.Random<BehaviourInfo>();
			yield return behaviour.CRun(controller);
			base.result = behaviour.result;
			yield break;
		}

		// Token: 0x04004C32 RID: 19506
		[UnityEditor.Subcomponent(typeof(BehaviourInfo))]
		[SerializeField]
		private BehaviourInfo.Subcomponents _behaviours;
	}
}
