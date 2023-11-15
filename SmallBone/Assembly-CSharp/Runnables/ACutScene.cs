using System;
using CutScenes.Shots;
using UnityEditor;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002B7 RID: 695
	public class ACutScene : Runnable
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x0002CC82 File Offset: 0x0002AE82
		public override void Run()
		{
			ShotInfo.Subcomponents shotInfos = this._shotInfos;
			OnStartEnd onStartEnd = this._onStartEnd;
			Shot onStart = (onStartEnd != null) ? onStartEnd.onStart : null;
			OnStartEnd onStartEnd2 = this._onStartEnd;
			shotInfos.Run(onStart, (onStartEnd2 != null) ? onStartEnd2.onEnd : null);
		}

		// Token: 0x04000BC9 RID: 3017
		[SerializeField]
		private OnStartEnd _onStartEnd;

		// Token: 0x04000BCA RID: 3018
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		private ShotInfo.Subcomponents _shotInfos;
	}
}
