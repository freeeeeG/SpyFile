using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000670 RID: 1648
public class BeachAnimatedDynamicTransition : AnimatedDynamicTransition
{
	// Token: 0x06001F6F RID: 8047 RVA: 0x00098FB4 File Offset: 0x000973B4
	public override void Setup(CallbackVoid _endTransitionCallback)
	{
		base.Setup(_endTransitionCallback);
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			this.m_tideInCallback = (CallbackVoid)Delegate.Combine(this.m_tideInCallback, new CallbackVoid(delegate()
			{
				this.DestroyAndRespawnObjects(this.m_leavingCounterLayoutRoot, this.m_tideCollider);
			}));
		}
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x00098FF4 File Offset: 0x000973F4
	private void DestroyAndRespawnObjects(Transform _attachmentRoot, Collider _looseDestroyCollider)
	{
		Bounds bounds = _looseDestroyCollider.bounds;
		foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			IAttachment[] componentsInChildren = gameObject.GetComponentsInChildren<IAttachment>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j] != null && !(componentsInChildren[j].AccessGameObject() == null))
				{
					if (!componentsInChildren[j].IsAttached())
					{
						GameObject gameObject2 = componentsInChildren[j].AccessGameObject();
						if (!(gameObject2 == null) && bounds.Contains(gameObject2.transform.position))
						{
							ServerPlayerRespawnManager.KillOrRespawn(componentsInChildren[j].AccessGameObject(), null);
						}
					}
				}
			}
		}
		ServerAttachStation[] array = _attachmentRoot.gameObject.RequestComponentsRecursive<ServerAttachStation>();
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k].HasItem())
			{
				GameObject gameObject3 = array[k].TakeItem();
				if (gameObject3 != null)
				{
					ServerPlayerRespawnManager.KillOrRespawn(gameObject3, null);
				}
			}
		}
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x00099120 File Offset: 0x00097520
	public override IEnumerator Run()
	{
		this.m_tideAnimator.SetBool(BeachAnimatedDynamicTransition.m_TideInScene, true);
		yield return null;
		while (this.m_tideAnimator.GetBool(BeachAnimatedDynamicTransition.m_IsTideTransitioning))
		{
			yield return null;
		}
		this.m_tideInCallback();
		IEnumerator transitionRoutine = base.Run();
		while (transitionRoutine.MoveNext())
		{
			object obj = transitionRoutine.Current;
			yield return obj;
		}
		this.m_tideAnimator.SetBool(BeachAnimatedDynamicTransition.m_TideInScene, false);
		yield return null;
		while (this.m_tideAnimator.GetBool(BeachAnimatedDynamicTransition.m_IsTideTransitioning))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040017F4 RID: 6132
	[Space]
	[SerializeField]
	private Transform m_leavingCounterLayoutRoot;

	// Token: 0x040017F5 RID: 6133
	[Space]
	[Header("Tide References")]
	[SerializeField]
	private Animator m_tideAnimator;

	// Token: 0x040017F6 RID: 6134
	[SerializeField]
	private Collider m_tideCollider;

	// Token: 0x040017F7 RID: 6135
	private CallbackVoid m_tideInCallback = delegate()
	{
	};

	// Token: 0x040017F8 RID: 6136
	private static int m_TideInScene = Animator.StringToHash("TideInScene");

	// Token: 0x040017F9 RID: 6137
	private static int m_IsTideTransitioning = Animator.StringToHash("IsTideTransitioning");
}
