using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000210 RID: 528
	public class ResetGame : Event
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x0001CDAF File Offset: 0x0001AFAF
		public override void Run()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			levelManager.skulSpawnAnimaionEnable = this._skulSpawnAnimation;
			levelManager.ResetGame(this._type);
		}

		// Token: 0x040008A1 RID: 2209
		[SerializeField]
		private bool _skulSpawnAnimation = true;

		// Token: 0x040008A2 RID: 2210
		[SerializeField]
		private Chapter.Type _type;
	}
}
