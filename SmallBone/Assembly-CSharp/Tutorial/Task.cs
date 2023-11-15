using System;
using System.Collections;
using Characters;
using Characters.Controllers;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorial
{
	// Token: 0x020000C8 RID: 200
	public abstract class Task : MonoBehaviour
	{
		// Token: 0x060003DD RID: 989 RVA: 0x0000D60D File Offset: 0x0000B80D
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000D624 File Offset: 0x0000B824
		public IEnumerator Play()
		{
			yield return null;
			yield break;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000D62C File Offset: 0x0000B82C
		private IEnumerator ProcessOption()
		{
			if (this._option.blockInput)
			{
				PlayerInput.blocked.Attach(this);
			}
			if (this._option.letterBox)
			{
				Scene<GameBase>.instance.uiManager.letterBox.Appear(1.7f);
			}
			this._option.reservedPosition != null;
			if (this._option.finalDirection != Task.Option.LookingDirectionOption.None)
			{
				this._player.lookingDirection = ((this._option.finalDirection == Task.Option.LookingDirectionOption.Right) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			}
			yield return null;
			yield break;
		}

		// Token: 0x040002FD RID: 765
		[SerializeField]
		private Message[] messages;

		// Token: 0x040002FE RID: 766
		[SerializeField]
		private Task.Option _option;

		// Token: 0x040002FF RID: 767
		private Character _player;

		// Token: 0x020000C9 RID: 201
		[Serializable]
		private class Option
		{
			// Token: 0x04000300 RID: 768
			[SerializeField]
			internal bool letterBox;

			// Token: 0x04000301 RID: 769
			[SerializeField]
			internal bool blockInput;

			// Token: 0x04000302 RID: 770
			[SerializeField]
			internal Transform reservedPosition;

			// Token: 0x04000303 RID: 771
			[SerializeField]
			internal Task.Option.LookingDirectionOption finalDirection;

			// Token: 0x020000CA RID: 202
			internal enum LookingDirectionOption
			{
				// Token: 0x04000305 RID: 773
				None,
				// Token: 0x04000306 RID: 774
				Right,
				// Token: 0x04000307 RID: 775
				Left
			}
		}

		// Token: 0x020000CB RID: 203
		private enum StartCondition
		{
			// Token: 0x04000309 RID: 777
			TimeOutAfterSpawn,
			// Token: 0x0400030A RID: 778
			RemainMonsters,
			// Token: 0x0400030B RID: 779
			EnterZone
		}
	}
}
