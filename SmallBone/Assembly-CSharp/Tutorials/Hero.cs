using System;
using System.Collections;
using Characters;
using Characters.Actions;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000DE RID: 222
	public class Hero : MonoBehaviour
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0000E5D3 File Offset: 0x0000C7D3
		public IEnumerator CAppear()
		{
			this._caharacter.ForceToLookAt(Character.LookingDirection.Left);
			this._landing.TryStart();
			while (this._landing.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000E5E2 File Offset: 0x0000C7E2
		public IEnumerator CAttack()
		{
			this._attack.TryStart();
			yield return Chronometer.global.WaitForSeconds(1f);
			Scene<GameBase>.instance.cameraController.Shake(0.1f, 2.5f);
			yield return Chronometer.global.WaitForSeconds(2.5f);
			if (!this._darkOgre.health.dead)
			{
				this._darkOgre.health.Kill();
			}
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.white);
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.black);
			while (this._attack.running)
			{
				yield return null;
			}
			yield return Chronometer.global.WaitForSeconds(2f);
			yield break;
		}

		// Token: 0x0400034D RID: 845
		[SerializeField]
		private Character _caharacter;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		private Character _darkOgre;

		// Token: 0x0400034F RID: 847
		[SerializeField]
		private Characters.Actions.Action _landing;

		// Token: 0x04000350 RID: 848
		[SerializeField]
		private Characters.Actions.Action _attack;
	}
}
