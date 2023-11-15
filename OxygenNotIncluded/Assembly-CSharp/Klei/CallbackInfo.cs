using System;

namespace Klei
{
	// Token: 0x02000DBF RID: 3519
	public struct CallbackInfo
	{
		// Token: 0x06006C72 RID: 27762 RVA: 0x002AD359 File Offset: 0x002AB559
		public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
		{
			this.handle = h;
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x002AD364 File Offset: 0x002AB564
		public void Release()
		{
			if (this.handle.IsValid())
			{
				Game.CallbackInfo item = Game.Instance.callbackManager.GetItem(this.handle);
				System.Action cb = item.cb;
				if (!item.manuallyRelease)
				{
					Game.Instance.callbackManager.Release(this.handle);
				}
				cb();
			}
		}

		// Token: 0x04005155 RID: 20821
		private HandleVector<Game.CallbackInfo>.Handle handle;
	}
}
