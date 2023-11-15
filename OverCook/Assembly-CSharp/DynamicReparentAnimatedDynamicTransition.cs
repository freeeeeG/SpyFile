using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000672 RID: 1650
public class DynamicReparentAnimatedDynamicTransition : AnimatedDynamicTransition
{
	// Token: 0x06001F80 RID: 8064 RVA: 0x00099778 File Offset: 0x00097B78
	public override IEnumerator Run()
	{
		foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				IAttachment[] componentsInChildren = gameObject.GetComponentsInChildren<IAttachment>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j] != null && !(componentsInChildren[j].AccessGameObject() == null))
					{
						if (!componentsInChildren[j].IsAttached())
						{
							ServerPlayerRespawnManager.KillOrRespawn(componentsInChildren[j].AccessGameObject(), null);
						}
					}
				}
				PlayerControls[] componentsInChildren2 = gameObject.GetComponentsInChildren<PlayerControls>();
				for (int k = 0; k < componentsInChildren2.Length; k++)
				{
					componentsInChildren2[k].transform.SetParent(null);
				}
			}
			DynamicLandscapeParenting[] componentsInChildren3 = gameObject.GetComponentsInChildren<DynamicLandscapeParenting>();
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				componentsInChildren3[l].enabled = false;
				componentsInChildren3[l].SetEnabled(false);
			}
		}
		IEnumerator routine = base.Run();
		while (routine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001804 RID: 6148
	[SerializeField]
	private GameObject dynamicParent;
}
