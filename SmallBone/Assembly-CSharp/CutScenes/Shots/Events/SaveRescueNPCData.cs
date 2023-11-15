using System;
using Data;
using Level.Npc;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000215 RID: 533
	public class SaveRescueNPCData : Event
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x0001CE2A File Offset: 0x0001B02A
		public override void Run()
		{
			GameData.Progress.SetRescued(this._type, true);
		}

		// Token: 0x040008A5 RID: 2213
		[SerializeField]
		private NpcType _type;
	}
}
