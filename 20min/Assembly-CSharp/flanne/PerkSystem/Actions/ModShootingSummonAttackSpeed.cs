using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C1 RID: 449
	public class ModShootingSummonAttackSpeed : Action
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x00027C30 File Offset: 0x00025E30
		public override void Activate(GameObject target)
		{
			PlayerController instance = PlayerController.Instance;
			foreach (AttackingSummon attackingSummon in this.FindObjectsOfTypeAll<AttackingSummon>())
			{
				if (attackingSummon.SummonTypeID == this.SummonTypeID)
				{
					attackingSummon.attackSpeedMod.AddMultiplierBonus(this.attackSpeedMod);
				}
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00027CA8 File Offset: 0x00025EA8
		private List<T> FindObjectsOfTypeAll<T>()
		{
			List<T> list = new List<T>();
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					foreach (GameObject gameObject in sceneAt.GetRootGameObjects())
					{
						list.AddRange(gameObject.GetComponentsInChildren<T>(true));
					}
				}
			}
			return list;
		}

		// Token: 0x04000727 RID: 1831
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x04000728 RID: 1832
		[SerializeField]
		private float attackSpeedMod;
	}
}
