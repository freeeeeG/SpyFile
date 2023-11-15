using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000CC4 RID: 3268
	internal class KMonoBehaviour2
	{
		// Token: 0x0600688D RID: 26765 RVA: 0x00278F90 File Offset: 0x00277190
		protected virtual void OnPrefabInit()
		{
		}

		// Token: 0x0600688E RID: 26766 RVA: 0x00278F92 File Offset: 0x00277192
		public void Subscribe(int evt, Action<object> cb)
		{
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x00278F94 File Offset: 0x00277194
		public void Trigger(int evt, object data)
		{
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x00278F96 File Offset: 0x00277196
		public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
		{
		}

		// Token: 0x06006891 RID: 26769 RVA: 0x00278F98 File Offset: 0x00277198
		public void Trigger<EventType>(EventType evt) where EventType : IEventData
		{
		}
	}
}
