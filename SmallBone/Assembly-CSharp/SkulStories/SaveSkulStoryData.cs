using System;
using Data;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000119 RID: 281
	public sealed class SaveSkulStoryData : Event
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x00010AFB File Offset: 0x0000ECFB
		public override void Run()
		{
			GameData.Progress.skulstory.SetData(this._key, true);
		}

		// Token: 0x04000433 RID: 1075
		[SerializeField]
		private Key _key;
	}
}
