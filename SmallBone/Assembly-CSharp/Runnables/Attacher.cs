using System;
using CutScenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002CF RID: 719
	public class Attacher : Runnable
	{
		// Token: 0x06000E83 RID: 3715 RVA: 0x0002D631 File Offset: 0x0002B831
		public override void Run()
		{
			if (this._type == Attacher.Type.Attach)
			{
				this._state.Attach();
				return;
			}
			this._state.Detach();
		}

		// Token: 0x04000C08 RID: 3080
		[SerializeField]
		private Attacher.Type _type;

		// Token: 0x04000C09 RID: 3081
		[SerializeField]
		[State.SubcomponentAttribute]
		private State _state;

		// Token: 0x020002D0 RID: 720
		private enum Type
		{
			// Token: 0x04000C0B RID: 3083
			Attach,
			// Token: 0x04000C0C RID: 3084
			Detach
		}
	}
}
