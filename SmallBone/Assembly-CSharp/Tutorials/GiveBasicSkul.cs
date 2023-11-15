using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000DC RID: 220
	public class GiveBasicSkul : MonoBehaviour
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x0000E50A File Offset: 0x0000C70A
		private IEnumerator Start()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.CancelAction();
			yield return new WaitForEndOfFrame();
			player.playerComponents.inventory.weapon.ForceEquipAt(this._basicSkul.Instantiate(), 0);
			player.animationController.ForceUpdate();
			UnityEngine.Object.Destroy(this);
			yield break;
		}

		// Token: 0x04000348 RID: 840
		[SerializeField]
		private Weapon _basicSkul;
	}
}
