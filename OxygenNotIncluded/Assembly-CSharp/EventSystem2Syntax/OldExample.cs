using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000CC2 RID: 3266
	internal class OldExample : KMonoBehaviour2
	{
		// Token: 0x06006887 RID: 26759 RVA: 0x00278F00 File Offset: 0x00277100
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
			bool flag = false;
			base.Trigger(0, flag);
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x00278F35 File Offset: 0x00277135
		private void OnObjectDestroyed(object data)
		{
			Debug.Log((bool)data);
		}
	}
}
