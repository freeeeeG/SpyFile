using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000E1 RID: 225
	public class JumpTutorial : Tutorial
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		public override void Activate()
		{
			base.Activate();
			Scene<GameBase>.instance.cameraController.StartTrack(this._trackPoint);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000E815 File Offset: 0x0000CA15
		public override void Deactivate()
		{
			base.StartCoroutine(this.<Deactivate>g__CDeactivate|3_0());
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000E824 File Offset: 0x0000CA24
		protected override IEnumerator Process()
		{
			yield return this.Converse(2f);
			yield break;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000E833 File Offset: 0x0000CA33
		[CompilerGenerated]
		private IEnumerator <Deactivate>g__CDeactivate|3_0()
		{
			base.state = Tutorial.State.Done;
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			yield return LetterBox.instance.CDisappear(0.4f);
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
			yield break;
		}

		// Token: 0x04000357 RID: 855
		[SerializeField]
		private Transform _skeletone;

		// Token: 0x04000358 RID: 856
		[SerializeField]
		private Transform _trackPoint;
	}
}
