using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class TriggerDestroy : MonoBehaviour, ITriggerReceiver
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x0002D640 File Offset: 0x0002BA40
	public void OnTrigger(string _name)
	{
		if (_name == this.m_trigger && EntitySerialisationRegistry.GetEntry(base.gameObject) == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400058D RID: 1421
	[SerializeField]
	public string m_trigger;
}
