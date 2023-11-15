using System;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000203 RID: 515
	public class PlaySkulAnimation : Event
	{
		// Token: 0x06000A76 RID: 2678 RVA: 0x0001CABC File Offset: 0x0001ACBC
		public override void Run()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			if (this._player == null)
			{
				return;
			}
			Skul component = this._player.GetComponent<WeaponInventory>().polymorphOrCurrent.GetComponent<Skul>();
			switch (this._type)
			{
			case PlaySkulAnimation.Type.EndPose:
				component.endPose.TryStart();
				return;
			case PlaySkulAnimation.Type.IntroIdle:
				component.introIdle.TryStart();
				return;
			case PlaySkulAnimation.Type.IntroWalk:
				component.introWalk.TryStart();
				return;
			default:
				return;
			}
		}

		// Token: 0x04000886 RID: 2182
		[SerializeField]
		private PlaySkulAnimation.Type _type = PlaySkulAnimation.Type.IntroIdle;

		// Token: 0x04000887 RID: 2183
		private Character _player;

		// Token: 0x02000204 RID: 516
		private enum Type
		{
			// Token: 0x04000889 RID: 2185
			EndPose,
			// Token: 0x0400088A RID: 2186
			IntroIdle,
			// Token: 0x0400088B RID: 2187
			IntroWalk
		}
	}
}
