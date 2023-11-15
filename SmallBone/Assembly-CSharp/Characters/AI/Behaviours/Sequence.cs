using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012C8 RID: 4808
	public class Sequence : Behaviour
	{
		// Token: 0x06005F22 RID: 24354 RVA: 0x00116B2D File Offset: 0x00114D2D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			foreach (BehaviourInfo child in this._children.components)
			{
				yield return child.CRun(controller);
				if (child.result == Behaviour.Result.Fail)
				{
					base.result = Behaviour.Result.Fail;
					yield break;
				}
				child = null;
			}
			BehaviourInfo[] array = null;
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004C6C RID: 19564
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BehaviourInfo))]
		private BehaviourInfo.Subcomponents _children;
	}
}
