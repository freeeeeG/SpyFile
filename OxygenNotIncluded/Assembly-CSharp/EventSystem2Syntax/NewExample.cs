using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000CC3 RID: 3267
	internal class NewExample : KMonoBehaviour2
	{
		// Token: 0x0600688A RID: 26762 RVA: 0x00278F50 File Offset: 0x00277150
		protected override void OnPrefabInit()
		{
			base.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
			base.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent
			{
				parameter = false
			});
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x00278F86 File Offset: 0x00277186
		private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
		{
		}

		// Token: 0x02001C0C RID: 7180
		private struct ObjectDestroyedEvent : IEventData
		{
			// Token: 0x04007EB8 RID: 32440
			public bool parameter;
		}
	}
}
