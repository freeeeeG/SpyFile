using System;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001482 RID: 5250
	[Serializable]
	public class SetPlayerAsTarget : Action
	{
		// Token: 0x06006650 RID: 26192 RVA: 0x00127FBD File Offset: 0x001261BD
		public override TaskStatus OnUpdate()
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return TaskStatus.Failure;
			}
			this._target.SetValue(Singleton<Service>.Instance.levelManager.player);
			return TaskStatus.Success;
		}

		// Token: 0x04005265 RID: 21093
		[SerializeField]
		private SharedCharacter _target;
	}
}
