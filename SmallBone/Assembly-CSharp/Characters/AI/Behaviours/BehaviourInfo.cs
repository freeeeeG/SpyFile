using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012A3 RID: 4771
	public class BehaviourInfo : MonoBehaviour
	{
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06005E90 RID: 24208 RVA: 0x00115CA3 File Offset: 0x00113EA3
		public Behaviour.Result result
		{
			get
			{
				return this._behaviour.result;
			}
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x00115CB0 File Offset: 0x00113EB0
		public IEnumerator CRun(AIController controller)
		{
			yield return this._behaviour.CRun(controller);
			yield break;
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x00115CC6 File Offset: 0x00113EC6
		public override string ToString()
		{
			if (this._tag != null && this._tag.Length != 0)
			{
				return this._tag;
			}
			return this.GetAutoName();
		}

		// Token: 0x04004BFC RID: 19452
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _behaviour;

		// Token: 0x04004BFD RID: 19453
		[SerializeField]
		private string _tag;

		// Token: 0x020012A4 RID: 4772
		[Serializable]
		internal class Subcomponents : SubcomponentArray<BehaviourInfo>
		{
		}
	}
}
