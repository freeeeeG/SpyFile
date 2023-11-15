using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class TriggerQueue : TimedQueue, ITriggerReceiver
{
	// Token: 0x060006DA RID: 1754 RVA: 0x0002DCC6 File Offset: 0x0002C0C6
	protected virtual void Awake()
	{
		this.m_queue.InitHashs();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002DCD3 File Offset: 0x0002C0D3
	public void RegisterFinishedCallback(CallbackVoid _callback)
	{
		this.m_finishedCallback = (CallbackVoid)Delegate.Combine(this.m_finishedCallback, _callback);
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002DCEC File Offset: 0x0002C0EC
	public void DeregisterFinishedCallback(CallbackVoid _callback)
	{
		this.m_finishedCallback = (CallbackVoid)Delegate.Remove(this.m_finishedCallback, _callback);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0002DD05 File Offset: 0x0002C105
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_finishedTrigger)
		{
			this.m_finishedCallback();
		}
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0002DD23 File Offset: 0x0002C123
	public override float GetQueueLength()
	{
		return (float)this.m_queue.m_triggers.Length;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0002DD33 File Offset: 0x0002C133
	public override float GetDelay(int _index)
	{
		return this.m_queue.m_delays[_index];
	}

	// Token: 0x040005AB RID: 1451
	[Space]
	[SerializeField]
	public TriggerQueue.TriggerType m_targetType = TriggerQueue.TriggerType.Animator;

	// Token: 0x040005AC RID: 1452
	[HideInInspectorTest("m_targetType", TriggerQueue.TriggerType.Animator)]
	[SerializeField]
	public Animator m_animator;

	// Token: 0x040005AD RID: 1453
	[HideInInspectorTest("m_targetType", TriggerQueue.TriggerType.Object)]
	[SerializeField]
	public GameObject m_targetObject;

	// Token: 0x040005AE RID: 1454
	[Space]
	[HideInInspectorTest("m_targetType", TriggerQueue.TriggerType.Animator)]
	[SerializeField]
	[ReadOnly]
	public string m_finishedTrigger = "AnimationFinished";

	// Token: 0x040005AF RID: 1455
	[HideInInspectorTest("m_targetType", TriggerQueue.TriggerType.Animator)]
	[SerializeField]
	public bool m_waitForFinished = true;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	public TriggerQueue.Queue m_queue = new TriggerQueue.Queue();

	// Token: 0x040005B1 RID: 1457
	private CallbackVoid m_finishedCallback = delegate()
	{
	};

	// Token: 0x02000192 RID: 402
	public enum TriggerType
	{
		// Token: 0x040005B4 RID: 1460
		Object,
		// Token: 0x040005B5 RID: 1461
		Animator
	}

	// Token: 0x02000193 RID: 403
	[Serializable]
	public class Queue
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0002DD70 File Offset: 0x0002C170
		public void InitHashs()
		{
			this.m_triggerHashs = new int[this.m_triggers.Length];
			for (int i = 0; i < this.m_triggerHashs.Length; i++)
			{
				this.m_triggerHashs[i] = Animator.StringToHash(this.m_triggers[i]);
			}
		}

		// Token: 0x040005B6 RID: 1462
		[SerializeField]
		public string[] m_triggers = new string[0];

		// Token: 0x040005B7 RID: 1463
		[SerializeField]
		public float[] m_delays = new float[0];

		// Token: 0x040005B8 RID: 1464
		[NonSerialized]
		public int[] m_triggerHashs = new int[0];
	}
}
