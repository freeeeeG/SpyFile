using System;
using Data;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000213 RID: 531
	public class SaveCutSceneData : Event
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x0001CE01 File Offset: 0x0001B001
		public override void Run()
		{
			GameData.Progress.cutscene.SetData(this._key, true);
		}

		// Token: 0x040008A4 RID: 2212
		[SerializeField]
		private Key _key;
	}
}
