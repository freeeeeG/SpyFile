using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class ClientTriggerColourCycle : ClientSynchroniserBase
{
	// Token: 0x06000696 RID: 1686 RVA: 0x0002D3AC File Offset: 0x0002B7AC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerColourCycle = (TriggerColourCycle)synchronisedObject;
		this.SetMaterial(0);
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0002D3C8 File Offset: 0x0002B7C8
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerColourCycle;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002D3CC File Offset: 0x0002B7CC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		int colourIndex = ((TriggerColourCycleMessage)serialisable).m_colourIndex;
		this.SetMaterial(colourIndex);
		this.SetGlowEffect(colourIndex);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002D3F4 File Offset: 0x0002B7F4
	private void SetGlowEffect(int index)
	{
		for (int i = 0; i < this.m_triggerColourCycle.m_glowEffects.Length; i++)
		{
			if (i == index)
			{
				this.m_triggerColourCycle.m_glowEffects[i].gameObject.SetActive(true);
			}
			else
			{
				this.m_triggerColourCycle.m_glowEffects[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002D45B File Offset: 0x0002B85B
	private void SetMaterial(int index)
	{
		this.m_triggerColourCycle.m_renderer.material = this.m_triggerColourCycle.m_materials[index];
	}

	// Token: 0x04000580 RID: 1408
	private TriggerColourCycle m_triggerColourCycle;
}
