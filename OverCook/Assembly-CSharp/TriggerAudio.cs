using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
[AddComponentMenu("Scripts/Core/Components/TriggerAudio")]
public class TriggerAudio : MonoBehaviour
{
	// Token: 0x0600067F RID: 1663 RVA: 0x0002D173 File Offset: 0x0002B573
	private void Awake()
	{
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0002D175 File Offset: 0x0002B575
	private void OnEnable()
	{
		if (this.m_onEnable)
		{
			GameUtils.TriggerAudio(this.m_oneShotTag, base.gameObject.layer);
		}
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0002D199 File Offset: 0x0002B599
	private void OnDisable()
	{
		if (this.m_onDisable)
		{
			GameUtils.TriggerAudio(this.m_oneShotTag, base.gameObject.layer);
		}
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002D1BD File Offset: 0x0002B5BD
	private void OnTrigger(string _name)
	{
		if (_name == this.m_trigger)
		{
			GameUtils.TriggerAudio(this.m_oneShotTag, base.gameObject.layer);
		}
	}

	// Token: 0x04000570 RID: 1392
	[SerializeField]
	private GameOneShotAudioTag m_oneShotTag;

	// Token: 0x04000571 RID: 1393
	[SerializeField]
	private string m_trigger;

	// Token: 0x04000572 RID: 1394
	[SerializeField]
	private bool m_onEnable;

	// Token: 0x04000573 RID: 1395
	[SerializeField]
	private bool m_onDisable;
}
