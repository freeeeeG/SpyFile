using System;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000209 RID: 521
	public class SetPositionToCamera : Event
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0001CC57 File Offset: 0x0001AE57
		public override void Run()
		{
			this._target.position = Camera.main.transform.position + this._offset;
		}

		// Token: 0x04000897 RID: 2199
		[SerializeField]
		private Transform _target;

		// Token: 0x04000898 RID: 2200
		[SerializeField]
		private Vector3 _offset;
	}
}
