using System;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000217 RID: 535
	public class SetFadeColor : Event
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x0001CE58 File Offset: 0x0001B058
		public override void Run()
		{
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(this._color);
		}

		// Token: 0x040008A6 RID: 2214
		[SerializeField]
		private Color _color;
	}
}
