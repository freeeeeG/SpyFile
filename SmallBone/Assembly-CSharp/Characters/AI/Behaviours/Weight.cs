using System;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012BF RID: 4799
	public class Weight : MonoBehaviour
	{
		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06005F04 RID: 24324 RVA: 0x001166E5 File Offset: 0x001148E5
		public Behaviour key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06005F05 RID: 24325 RVA: 0x001166ED File Offset: 0x001148ED
		public int value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x001166F8 File Offset: 0x001148F8
		public override string ToString()
		{
			string arg = (this._tag == null || this._tag.Length == 0) ? base.ToString() : this._tag;
			return string.Format("{0}, weight : {1}", arg, this._value);
		}

		// Token: 0x04004C50 RID: 19536
		[Behaviour.SubcomponentAttribute(true)]
		[SerializeField]
		private Behaviour _key;

		// Token: 0x04004C51 RID: 19537
		[SerializeField]
		private int _value = 1;

		// Token: 0x04004C52 RID: 19538
		[SerializeField]
		private string _tag;

		// Token: 0x020012C0 RID: 4800
		[Serializable]
		public class Subcomponents : SubcomponentArray<Weight>
		{
		}
	}
}
