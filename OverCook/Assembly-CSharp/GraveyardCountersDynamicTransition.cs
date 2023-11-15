using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public class GraveyardCountersDynamicTransition : DynamicTransitionBase
{
	// Token: 0x06001F86 RID: 8070 RVA: 0x00099D16 File Offset: 0x00098116
	private void OnDestroy()
	{
		GraveyardCountersDynamicTransition.ms_counterAttachments.Clear();
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x00099D24 File Offset: 0x00098124
	public override void Setup(CallbackVoid _endTransitionCallback)
	{
		this.m_endTransitionCallback = _endTransitionCallback;
		this.m_animatedTransition = base.gameObject.RequestComponent<AnimatedDynamicTransition>();
		this.m_outAnimators.Clear();
		this.m_inAnimators.Clear();
		for (int i = 0; i < this.m_countersGoingOut.Length; i++)
		{
			foreach (Animator animator in this.m_countersGoingOut[i].RequestComponentsRecursive<Animator>())
			{
				if (animator.transform.name.Equals("Counters"))
				{
					this.m_outAnimators.Add(animator);
				}
			}
		}
		for (int k = 0; k < this.m_countersComingIn.Length; k++)
		{
			foreach (Animator animator2 in this.m_countersComingIn[k].RequestComponentsRecursive<Animator>())
			{
				if (animator2.transform.name.Equals("Counters"))
				{
					this.m_inAnimators.Add(animator2);
				}
			}
		}
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			this.m_countersLeavingCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersLeavingCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				base.gameObject.SendTrigger(this.m_goingOutTrigger);
			}));
			this.m_countersEnteringCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersEnteringCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				base.gameObject.SendTrigger(this.m_comingInTrigger);
			}));
			this.m_countersOffscreenCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersOffscreenCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				for (int m = 0; m < _animators.Count; m++)
				{
					this.RespawnAllUtensils(_animators[m].gameObject);
				}
			}));
			this.m_countersEnteringCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersEnteringCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				for (int m = 0; m < _animators.Count; m++)
				{
					this.SetupAllUtensils(_animators[m].gameObject);
				}
			}));
		}
		else
		{
			this.m_countersEnteringCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersEnteringCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				for (int m = 0; m < _animators.Count; m++)
				{
					this.SetupAllUtensils(_animators[m].gameObject);
				}
			}));
			this.m_countersOffscreenCallback = (GenericVoid<List<Animator>>)Delegate.Combine(this.m_countersOffscreenCallback, new GenericVoid<List<Animator>>(delegate(List<Animator> _animators)
			{
				for (int m = 0; m < _animators.Count; m++)
				{
					this.RespawnAllUtensils_Client(_animators[m].gameObject);
				}
			}));
		}
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x00099F20 File Offset: 0x00098320
	private void SetupAllUtensils(GameObject _root)
	{
		IRespawnBehaviour[] array = _root.RequestInterfacesRecursive<IRespawnBehaviour>();
		for (int i = 0; i < array.Length; i++)
		{
			GraveyardCountersDynamicTransition.ms_counterAttachments.Add(((MonoBehaviour)array[i]).gameObject);
		}
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x00099F60 File Offset: 0x00098360
	private void RespawnAllUtensils(GameObject _root)
	{
		IRespawnBehaviour[] array = _root.RequestInterfacesRecursive<IRespawnBehaviour>();
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = ((MonoBehaviour)array[i]).gameObject;
			if (!this.m_respawnObjects || GraveyardCountersDynamicTransition.ms_counterAttachments.IndexOf(gameObject) >= 0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				ServerPlayerRespawnManager.KillOrRespawn(gameObject, null);
			}
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x00099FC8 File Offset: 0x000983C8
	private void RespawnAllUtensils_Client(GameObject _root)
	{
		IRespawnBehaviour[] array = _root.RequestInterfacesRecursive<IRespawnBehaviour>();
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = ((MonoBehaviour)array[i]).gameObject;
			if (!this.m_respawnObjects || GraveyardCountersDynamicTransition.ms_counterAttachments.IndexOf(gameObject) >= 0)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0009A024 File Offset: 0x00098424
	public override IEnumerator Run()
	{
		for (int i = 0; i < this.m_outAnimators.Count; i++)
		{
			this.m_outAnimators[i].SetBool(GraveyardCountersDynamicTransition.m_InScene, false);
		}
		this.m_countersLeavingCallback(this.m_outAnimators);
		yield return null;
		for (;;)
		{
			if (!this.m_outAnimators.Exists((Animator x) => x.GetBool(GraveyardCountersDynamicTransition.m_IsTransitioning)))
			{
				break;
			}
			yield return null;
		}
		this.m_countersOffscreenCallback(this.m_outAnimators);
		IEnumerator timerRoutine = CoroutineUtils.TimerRoutine(this.m_waitTime, base.gameObject.layer);
		while (timerRoutine.MoveNext())
		{
			yield return null;
		}
		if (this.m_animatedTransition != null)
		{
			this.m_animatedTransition.Setup(delegate
			{
			});
			IEnumerator subRoutine = this.m_animatedTransition.Run();
			while (subRoutine.MoveNext())
			{
				object obj = subRoutine.Current;
				yield return obj;
			}
			timerRoutine = CoroutineUtils.TimerRoutine(this.m_waitTime, base.gameObject.layer);
			while (timerRoutine.MoveNext())
			{
				yield return null;
			}
		}
		for (int j = 0; j < this.m_inAnimators.Count; j++)
		{
			this.m_inAnimators[j].SetBool(GraveyardCountersDynamicTransition.m_InScene, true);
		}
		this.m_countersEnteringCallback(this.m_inAnimators);
		yield return null;
		for (;;)
		{
			if (!this.m_inAnimators.Exists((Animator x) => !x.GetBool(GraveyardCountersDynamicTransition.m_IsTransitioning)))
			{
				break;
			}
			yield return null;
		}
		this.Shutdown();
		yield break;
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x0009A03F File Offset: 0x0009843F
	private void Shutdown()
	{
		this.m_endTransitionCallback();
	}

	// Token: 0x04001807 RID: 6151
	[SerializeField]
	private GameObject[] m_countersGoingOut;

	// Token: 0x04001808 RID: 6152
	[SerializeField]
	private float m_waitTime;

	// Token: 0x04001809 RID: 6153
	[SerializeField]
	private GameObject[] m_countersComingIn;

	// Token: 0x0400180A RID: 6154
	[SerializeField]
	private bool m_respawnObjects = true;

	// Token: 0x0400180B RID: 6155
	[Space]
	[SerializeField]
	private string m_goingOutTrigger = string.Empty;

	// Token: 0x0400180C RID: 6156
	[SerializeField]
	private string m_comingInTrigger = string.Empty;

	// Token: 0x0400180D RID: 6157
	private AnimatedDynamicTransition m_animatedTransition;

	// Token: 0x0400180E RID: 6158
	private List<Animator> m_outAnimators = new List<Animator>();

	// Token: 0x0400180F RID: 6159
	private List<Animator> m_inAnimators = new List<Animator>();

	// Token: 0x04001810 RID: 6160
	private static List<GameObject> ms_counterAttachments = new List<GameObject>();

	// Token: 0x04001811 RID: 6161
	private GenericVoid<List<Animator>> m_countersLeavingCallback = delegate(List<Animator> _animators)
	{
	};

	// Token: 0x04001812 RID: 6162
	private GenericVoid<List<Animator>> m_countersEnteringCallback = delegate(List<Animator> _animators)
	{
	};

	// Token: 0x04001813 RID: 6163
	private GenericVoid<List<Animator>> m_countersOffscreenCallback = delegate(List<Animator> _animators)
	{
	};

	// Token: 0x04001814 RID: 6164
	private CallbackVoid m_endTransitionCallback = delegate()
	{
	};

	// Token: 0x04001815 RID: 6165
	private static int m_InScene = Animator.StringToHash("InScene");

	// Token: 0x04001816 RID: 6166
	private static int m_IsTransitioning = Animator.StringToHash("IsTransitioning");
}
