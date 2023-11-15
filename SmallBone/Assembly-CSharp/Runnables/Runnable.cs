using System;
using UnityEditor;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002E7 RID: 743
	public abstract class Runnable : MonoBehaviour
	{
		// Token: 0x06000EB7 RID: 3767
		public abstract void Run();

		// Token: 0x04000C2E RID: 3118
		public static readonly Type[] types = new Type[]
		{
			typeof(Attacher),
			typeof(CharacterSetPositionTo),
			typeof(ChangeCameraZone),
			typeof(ChangeBackground),
			typeof(ClearStatus),
			typeof(ControlUI),
			typeof(ConsumeCurrency),
			typeof(Branch),
			typeof(DestroyObject),
			typeof(DropCurrency),
			typeof(DropCustomGear),
			typeof(DropGear),
			typeof(InvokeUnityEvent),
			typeof(LoadNextMap),
			typeof(LoadChapter),
			typeof(OpenUpgradePanel),
			typeof(PrintDebugLog),
			typeof(RunOperations),
			typeof(SetSoundEffectVolume),
			typeof(ShowStageInfo),
			typeof(ShowLineText),
			typeof(SpawnBuffFloatingText),
			typeof(TransitTo),
			typeof(TakeHealth),
			typeof(KillAllEnemy),
			typeof(Zoom),
			typeof(RunOperationInfos),
			typeof(RunAction),
			typeof(ControlHardmodeLevel),
			typeof(GameFadeInOut),
			typeof(SetAcheivement),
			typeof(StopPlayerStuckResolver)
		};

		// Token: 0x020002E8 RID: 744
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000EBA RID: 3770 RVA: 0x0002DBF8 File Offset: 0x0002BDF8
			public SubcomponentAttribute() : base(true, Runnable.types)
			{
			}
		}

		// Token: 0x020002E9 RID: 745
		[Serializable]
		public class Subcomponents : SubcomponentArray<Runnable>
		{
			// Token: 0x06000EBB RID: 3771 RVA: 0x0002DC08 File Offset: 0x0002BE08
			public void Run()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Run();
				}
			}
		}
	}
}
