using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class ClientFryingContentsCosmeticDecisions : ClientSynchroniserBase, IClientCookingNotifed
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060011BD RID: 4541 RVA: 0x00065277 File Offset: 0x00063677
	protected Renderer AccessRenderer
	{
		get
		{
			return this.m_renderer;
		}
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0006527F File Offset: 0x0006367F
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_fryingContentsCosmeticDecisions = (FryingContentsCosmeticDecisions)synchronisedObject;
		this.m_renderer = base.gameObject.RequireComponent<Renderer>();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0006529E File Offset: 0x0006369E
	public virtual void OnCookingStarted()
	{
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000652A0 File Offset: 0x000636A0
	public virtual void OnCookingFinished()
	{
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000652A4 File Offset: 0x000636A4
	public virtual void OnCookingPropChanged(float newProp)
	{
		if (this.m_renderer != null)
		{
			float value = MathUtils.ClampedRemap(newProp, this.m_fryingContentsCosmeticDecisions.m_lowerClampProp, 1f, 0f, 1f);
			this.m_renderer.material.SetFloat("Prop", value);
		}
	}

	// Token: 0x04000DD4 RID: 3540
	private FryingContentsCosmeticDecisions m_fryingContentsCosmeticDecisions;

	// Token: 0x04000DD5 RID: 3541
	private Renderer m_renderer;
}
