using System;
using UnityEditor;
using UnityEngine;

namespace BT
{
	// Token: 0x02001401 RID: 5121
	public abstract class Composite : Node
	{
		// Token: 0x060064D6 RID: 25814 RVA: 0x001243D9 File Offset: 0x001225D9
		protected override void DoReset(NodeState state)
		{
			this.ResetChild();
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x001243E4 File Offset: 0x001225E4
		private void ResetChild()
		{
			for (int i = 0; i < this._child.components.Length; i++)
			{
				this._child.components[i].node.ResetState();
			}
		}

		// Token: 0x0400514A RID: 20810
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BehaviourTree))]
		protected BehaviourTree.Subcomponents _child;
	}
}
