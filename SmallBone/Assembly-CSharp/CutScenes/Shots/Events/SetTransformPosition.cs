using System;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000218 RID: 536
	public class SetTransformPosition : Event
	{
		// Token: 0x06000A9E RID: 2718 RVA: 0x0001CE6F File Offset: 0x0001B06F
		public override void Run()
		{
			this._target.position = this._destinationTransform.position + this._offset;
		}

		// Token: 0x040008A7 RID: 2215
		[SerializeField]
		private Transform _target;

		// Token: 0x040008A8 RID: 2216
		[SerializeField]
		private Transform _destinationTransform;

		// Token: 0x040008A9 RID: 2217
		[SerializeField]
		private Vector3 _offset;
	}
}
