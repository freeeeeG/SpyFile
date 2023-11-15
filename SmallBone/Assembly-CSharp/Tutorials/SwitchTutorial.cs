using System;
using System.Collections;
using Characters;
using Level;
using Scenes;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000E7 RID: 231
	public class SwitchTutorial : Tutorial
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0000EB8F File Offset: 0x0000CD8F
		protected override IEnumerator Process()
		{
			yield return base.MoveTo(this._conversationPoint.position);
			this._player.lookingDirection = Character.LookingDirection.Right;
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			yield return base.MoveTo(this._interactiveGravePoint.position);
			yield return Chronometer.global.WaitForSeconds(0.3f);
			this._grave.InteractWith(this._player);
			yield return Chronometer.global.WaitForSeconds(1.7f);
			yield return base.MoveTo(this._getHeadPoint.position);
			yield return Chronometer.global.WaitForSeconds(0.3f);
			this._grave.droppedWeapon.dropped.InteractWith(this._player);
			this.Deactivate();
			yield break;
		}

		// Token: 0x0400036D RID: 877
		[SerializeField]
		private DeterminedGrave _grave;

		// Token: 0x0400036E RID: 878
		[SerializeField]
		private Transform _conversationPoint;

		// Token: 0x0400036F RID: 879
		[SerializeField]
		private Transform _interactiveGravePoint;

		// Token: 0x04000370 RID: 880
		[SerializeField]
		private Transform _getHeadPoint;
	}
}
