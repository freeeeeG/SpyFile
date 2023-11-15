using System;
using System.Linq;
using CutScenes.Shots.Events;
using Runnables;
using UnityEditor;

namespace CutScenes.Shots
{
	// Token: 0x020001BE RID: 446
	public abstract class Event : Runnable
	{
		// Token: 0x040007CC RID: 1996
		public new static readonly Type[] types = new Type[]
		{
			typeof(Attacher),
			typeof(CancelAction),
			typeof(CameraMoveTo),
			typeof(CameraMoveToCharacter),
			typeof(ChangeCameraTrackSpeed),
			typeof(ControlLetterBox),
			typeof(ControlUI),
			typeof(DestroyObject),
			typeof(Knockback),
			typeof(PlayAnimation),
			typeof(ResetGame),
			typeof(ResetGameScene),
			typeof(RenderEndingCut),
			typeof(RunSequences),
			typeof(SaveGameData),
			typeof(SaveCutSceneData),
			typeof(SaveRescueNPCData),
			typeof(SaveTutorialData),
			typeof(SetFadeColor),
			typeof(PlayAdventurerMusic),
			typeof(ForceToUpdateCameraPosition),
			typeof(SetTransformPosition)
		};

		// Token: 0x020001BF RID: 447
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000971 RID: 2417 RVA: 0x0001AFD4 File Offset: 0x000191D4
			public SubcomponentAttribute() : base(true, Runnable.types.Concat(Event.types).ToArray<Type>())
			{
			}
		}

		// Token: 0x020001C0 RID: 448
		[Serializable]
		public new class Subcomponents : SubcomponentArray<Runnable>
		{
			// Token: 0x06000972 RID: 2418 RVA: 0x0001AFF4 File Offset: 0x000191F4
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
