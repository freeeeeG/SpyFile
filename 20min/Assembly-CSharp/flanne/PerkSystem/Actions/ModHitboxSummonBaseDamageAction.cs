using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001BD RID: 445
	public class ModHitboxSummonBaseDamageAction : Action
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x00027AAC File Offset: 0x00025CAC
		public override void Activate(GameObject target)
		{
			foreach (SetHitboxActiveSummon setHitboxActiveSummon in this.FindObjectsOfTypeAll<SetHitboxActiveSummon>())
			{
				if (setHitboxActiveSummon.SummonTypeID == this.SummonTypeID)
				{
					setHitboxActiveSummon.baseDamage += this.baseDamageMod;
				}
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00027B20 File Offset: 0x00025D20
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

		// Token: 0x04000722 RID: 1826
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x04000723 RID: 1827
		[SerializeField]
		private int baseDamageMod;
	}
}
