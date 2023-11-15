using System;
using UnityEditor;
using UnityEngine;

namespace BT
{
	// Token: 0x0200140A RID: 5130
	public abstract class Decorator : Node
	{
		// Token: 0x060064F9 RID: 25849 RVA: 0x00124632 File Offset: 0x00122832
		protected override void DoReset(NodeState state)
		{
			this._subTree.node.ResetState();
		}

		// Token: 0x04005158 RID: 20824
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(BehaviourTree))]
		protected BehaviourTree _subTree;
	}
}
