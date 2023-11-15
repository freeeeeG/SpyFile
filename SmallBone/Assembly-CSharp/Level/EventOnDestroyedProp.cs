using System;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
	// Token: 0x020004D8 RID: 1240
	[RequireComponent(typeof(Prop))]
	public class EventOnDestroyedProp : MonoBehaviour
	{
		// Token: 0x0600182D RID: 6189 RVA: 0x0004BD24 File Offset: 0x00049F24
		public void Start()
		{
			this._prop.onDestroy += this._events.Invoke;
		}

		// Token: 0x0400150C RID: 5388
		[GetComponent]
		[SerializeField]
		private Prop _prop;

		// Token: 0x0400150D RID: 5389
		[SerializeField]
		private UnityEvent _events;
	}
}
