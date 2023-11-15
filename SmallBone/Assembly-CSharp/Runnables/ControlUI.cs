using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D3 RID: 723
	public sealed class ControlUI : Runnable
	{
		// Token: 0x06000E89 RID: 3721 RVA: 0x0002D695 File Offset: 0x0002B895
		public override void Run()
		{
			this._commands.Run();
		}

		// Token: 0x04000C10 RID: 3088
		[SerializeField]
		[UICommands.SubcomponentAttribute]
		private UICommands _commands;
	}
}
