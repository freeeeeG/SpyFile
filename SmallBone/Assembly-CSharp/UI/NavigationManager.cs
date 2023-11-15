using System;
using System.Collections.Generic;
using Characters.Controllers;
using Singletons;
using UnityEngine;
using UserInput;

namespace UI
{
	// Token: 0x020003B3 RID: 947
	public class NavigationManager : PersistentSingleton<NavigationManager>
	{
		// Token: 0x06001180 RID: 4480 RVA: 0x00033F1B File Offset: 0x0003211B
		public void Push(INavigatable navigatable)
		{
			this._navigatables.Push(navigatable);
			if (this._navigatables.Count == 1)
			{
				PlayerInput.blocked.Attach(this);
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00033F42 File Offset: 0x00032142
		public void Pop()
		{
			this._navigatables.Pop();
			if (this._navigatables.Count == 0)
			{
				PlayerInput.blocked.Detach(this);
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00033F6C File Offset: 0x0003216C
		private void Update()
		{
			if (this._navigatables.Count == 0)
			{
				return;
			}
			INavigatable navigatable = this._navigatables.Peek();
			if (KeyMapper.Map.Submit.IsPressed)
			{
				navigatable.Submit();
				return;
			}
			if (KeyMapper.Map.Cancel.IsPressed)
			{
				navigatable.Cancel();
				return;
			}
			this._remainNavigatingInterval -= Time.deltaTime;
			if (this._remainNavigatingInterval > 0f)
			{
				return;
			}
			if (KeyMapper.Map.Up.IsPressed)
			{
				this._remainNavigatingInterval = 0.3f;
				navigatable.Up();
				return;
			}
			if (KeyMapper.Map.Down.IsPressed)
			{
				this._remainNavigatingInterval = 0.3f;
				navigatable.Down();
				return;
			}
		}

		// Token: 0x04000E85 RID: 3717
		private const float _navigatingInterval = 0.3f;

		// Token: 0x04000E86 RID: 3718
		private float _remainNavigatingInterval;

		// Token: 0x04000E87 RID: 3719
		private readonly Stack<INavigatable> _navigatables = new Stack<INavigatable>();
	}
}
