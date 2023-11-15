using System;
using UnityEngine;

namespace BT
{
	// Token: 0x020013FD RID: 5117
	public class BehaviourTree : MonoBehaviour
	{
		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x060064C4 RID: 25796 RVA: 0x0012421B File Offset: 0x0012241B
		public Node node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x00124223 File Offset: 0x00122423
		public NodeState Tick(Context context)
		{
			return this._node.Tick(context);
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x00124231 File Offset: 0x00122431
		public void ResetState()
		{
			this._node.ResetState();
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x0012423E File Offset: 0x0012243E
		public override string ToString()
		{
			if (this._tag != null && this._tag.Length != 0)
			{
				return string.Format("({0}) {1}", this.node.GetType(), this._tag);
			}
			return base.ToString();
		}

		// Token: 0x0400513F RID: 20799
		[SerializeField]
		private string _tag;

		// Token: 0x04005140 RID: 20800
		[Node.SubcomponentAttribute(true)]
		[SerializeField]
		private Node _node;

		// Token: 0x020013FE RID: 5118
		[Serializable]
		public class Subcomponents : SubcomponentArray<BehaviourTree>
		{
		}
	}
}
