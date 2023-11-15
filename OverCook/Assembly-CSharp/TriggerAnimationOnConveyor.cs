using System;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
[RequireComponent(typeof(ConveyorStation))]
public class TriggerAnimationOnConveyor : MonoBehaviour, ITriggerReceiver
{
	// Token: 0x06001BC9 RID: 7113 RVA: 0x000881A0 File Offset: 0x000865A0
	public void Awake()
	{
		this.m_animationStartTriggerHash = Animator.StringToHash(this.m_animationStartTrigger);
		TriggerConveyorAdjacentUpdate triggerConveyorAdjacentUpdate = base.gameObject.RequestComponent<TriggerConveyorAdjacentUpdate>();
		if (triggerConveyorAdjacentUpdate == null)
		{
			triggerConveyorAdjacentUpdate = base.gameObject.AddComponent<TriggerConveyorAdjacentUpdate>();
			triggerConveyorAdjacentUpdate.hideFlags = HideFlags.NotEditable;
			triggerConveyorAdjacentUpdate.m_updateTrigger = this.m_animationFinishedTrigger;
		}
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x000881F5 File Offset: 0x000865F5
	public void RegisterOnTriggerCallback(GenericVoid<string> _callback)
	{
		this.m_onTriggerCallback = (GenericVoid<string>)Delegate.Combine(this.m_onTriggerCallback, _callback);
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x0008820E File Offset: 0x0008660E
	public void UnregisterOnTriggerCallback(GenericVoid<string> _callback)
	{
		this.m_onTriggerCallback = (GenericVoid<string>)Delegate.Remove(this.m_onTriggerCallback, _callback);
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00088227 File Offset: 0x00086627
	public void OnTrigger(string _trigger)
	{
		this.m_onTriggerCallback(_trigger);
	}

	// Token: 0x040015C6 RID: 5574
	[SerializeField]
	public string m_startTrigger;

	// Token: 0x040015C7 RID: 5575
	[SerializeField]
	[ReadOnly]
	public string m_animationStartTrigger = "Animate";

	// Token: 0x040015C8 RID: 5576
	[SerializeField]
	[ReadOnly]
	public string m_animationFinishedTrigger = "AnimationFinished";

	// Token: 0x040015C9 RID: 5577
	[SerializeField]
	public bool m_stopWhileAnimating = true;

	// Token: 0x040015CA RID: 5578
	public int m_animationStartTriggerHash;

	// Token: 0x040015CB RID: 5579
	private GenericVoid<string> m_onTriggerCallback = delegate(string _trigger)
	{
	};

	// Token: 0x020005B6 RID: 1462
	public enum State
	{
		// Token: 0x040015CE RID: 5582
		Idle,
		// Token: 0x040015CF RID: 5583
		Pending,
		// Token: 0x040015D0 RID: 5584
		Animating
	}
}
