using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D3 RID: 467
	public class TargetSummonsAction : Action
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x000285B2 File Offset: 0x000267B2
		public override void Init()
		{
			this.action.Init();
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000285C0 File Offset: 0x000267C0
		public override void Activate(GameObject target)
		{
			List<Summon> list = this.FindObjectsOfTypeAll<Summon>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.activeInHierarchy)
				{
					this.action.Activate(list[i].gameObject);
				}
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00028610 File Offset: 0x00026810
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

		// Token: 0x0400075D RID: 1885
		[SerializeReference]
		private Action action;
	}
}
