using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000671 RID: 1649
public class CampsiteAnimatedDynamicTransition : AnimatedDynamicTransition
{
	// Token: 0x06001F77 RID: 8055 RVA: 0x0009934F File Offset: 0x0009774F
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x00099358 File Offset: 0x00097758
	public override void Setup(CallbackVoid _endTransitionCallback)
	{
		base.Setup(_endTransitionCallback);
		this.m_respawnCollider = base.gameObject.RequestComponent<ServerRespawnCollider>();
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			this.m_raftLeftCallback = (CallbackVoid)Delegate.Combine(this.m_raftLeftCallback, new CallbackVoid(delegate()
			{
				this.DestroyAndRespawnObjects(this.m_leavingRaftLayoutRoot, this.m_leavingRaftRespawnCollider);
			}));
		}
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000993B4 File Offset: 0x000977B4
	private void DestroyAndRespawnObjects(Transform _attachmentRoot, Collider _looseDestroyCollider)
	{
		Bounds bounds = _looseDestroyCollider.bounds;
		foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			IRespawnBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<IRespawnBehaviour>(true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j] != null)
				{
					MonoBehaviour monoBehaviour = componentsInChildren[j] as MonoBehaviour;
					if (!(monoBehaviour == null) && !(monoBehaviour.gameObject == null))
					{
						GameObject gameObject2 = monoBehaviour.gameObject;
						if (!(gameObject2 == null) && bounds.Contains(gameObject2.transform.position))
						{
							IAttachment attachment = gameObject2.RequestInterface<IAttachment>();
							if (attachment == null || !attachment.IsAttached())
							{
								ServerPlayerRespawnManager.KillOrRespawn(gameObject2, this.m_respawnCollider);
							}
						}
					}
				}
			}
		}
		ServerWashingStation[] array = _attachmentRoot.gameObject.RequestComponentsRecursive<ServerWashingStation>();
		for (int k = 0; k < array.Length; k++)
		{
			array[k].WashAllPlates();
		}
		ServerAttachStation[] array2 = _attachmentRoot.gameObject.RequestComponentsRecursive<ServerAttachStation>();
		for (int l = 0; l < array2.Length; l++)
		{
			if (array2[l].HasItem())
			{
				GameObject gameObject3 = array2[l].TakeItem();
				if (gameObject3 != null)
				{
					ServerPlayerRespawnManager.KillOrRespawn(gameObject3, this.m_respawnCollider);
				}
			}
		}
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x00099540 File Offset: 0x00097940
	public override IEnumerator Run()
	{
		if (this.m_lakeAnimator != null)
		{
			this.m_lakeAnimator.SetBool(CampsiteAnimatedDynamicTransition.m_RaftLeavingHash, true);
			yield return null;
			while (this.m_lakeAnimator.GetBool(CampsiteAnimatedDynamicTransition.m_AreRaftsInMotionHash))
			{
				yield return null;
			}
		}
		IEnumerator transitionRoutine = base.Run();
		while (transitionRoutine.MoveNext())
		{
			object obj = transitionRoutine.Current;
			yield return obj;
		}
		this.m_raftLeftCallback();
		if (this.m_lakeAnimator != null)
		{
			this.m_lakeAnimator.SetBool(CampsiteAnimatedDynamicTransition.m_RaftLeavingHash, false);
			yield return null;
			while (this.m_lakeAnimator.GetBool(CampsiteAnimatedDynamicTransition.m_AreRaftsInMotionHash))
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040017FB RID: 6139
	[Space]
	[Header("Raft References")]
	[SerializeField]
	private Transform m_leavingRaftLayoutRoot;

	// Token: 0x040017FC RID: 6140
	[SerializeField]
	private Collider m_leavingRaftRespawnCollider;

	// Token: 0x040017FD RID: 6141
	[Space]
	[SerializeField]
	private Transform m_enteringRaftLayoutRoot;

	// Token: 0x040017FE RID: 6142
	[Space]
	[Header("Lake References")]
	[SerializeField]
	private Animator m_lakeAnimator;

	// Token: 0x040017FF RID: 6143
	private ServerRespawnCollider m_respawnCollider;

	// Token: 0x04001800 RID: 6144
	private CallbackVoid m_raftLeftCallback = delegate()
	{
	};

	// Token: 0x04001801 RID: 6145
	private static int m_RaftLeavingHash = Animator.StringToHash("RaftsAreChanging");

	// Token: 0x04001802 RID: 6146
	private static int m_AreRaftsInMotionHash = Animator.StringToHash("IsArtInMotion");
}
