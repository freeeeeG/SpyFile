using System;
using Services;
using Singletons;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001468 RID: 5224
	[TaskDescription("현재 챕터의 Stage Index를 저장합니다.")]
	public sealed class GetStageIndex : Action
	{
		// Token: 0x060065FF RID: 26111 RVA: 0x00126C1D File Offset: 0x00124E1D
		public override TaskStatus OnUpdate()
		{
			this.storeResult.SetValue(Singleton<Service>.Instance.levelManager.currentChapter.stageIndex);
			return TaskStatus.Success;
		}

		// Token: 0x040051F9 RID: 20985
		public SharedInt storeResult;
	}
}
