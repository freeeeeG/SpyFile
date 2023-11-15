using System;
using UnityEngine;

namespace Characters.Operations.ObjectTransform
{
	// Token: 0x02000FB7 RID: 4023
	public class SetPositionsTo : CharacterOperation
	{
		// Token: 0x06004E03 RID: 19971 RVA: 0x000E9538 File Offset: 0x000E7738
		public override void Run(Character owner)
		{
			this._targets.Shuffle<Transform>();
			for (int i = 0; i < this._objects.Length; i++)
			{
				this._objects[i].position = this._targets[i].position;
			}
		}

		// Token: 0x04003DFE RID: 15870
		[SerializeField]
		private Transform[] _objects;

		// Token: 0x04003DFF RID: 15871
		[SerializeField]
		private Transform[] _targets;
	}
}
