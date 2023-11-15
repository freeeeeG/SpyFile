using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012C6 RID: 4806
	public class Selector : Decorator
	{
		// Token: 0x06005F1A RID: 24346 RVA: 0x00116A2C File Offset: 0x00114C2C
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			foreach (BehaviourInfo child in this._children.components)
			{
				yield return child.CRun(controller);
				if (child.result == Behaviour.Result.Success)
				{
					base.result = Behaviour.Result.Success;
					yield break;
				}
				child = null;
			}
			BehaviourInfo[] array = null;
			base.result = Behaviour.Result.Fail;
			yield break;
		}

		// Token: 0x04004C64 RID: 19556
		[UnityEditor.Subcomponent(typeof(BehaviourInfo))]
		[SerializeField]
		private BehaviourInfo.Subcomponents _children;
	}
}
