using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class ClientCookingEffectsCosmeticDecisions : ClientSynchroniserBase, IClientCookingNotifed
{
	// Token: 0x06001195 RID: 4501 RVA: 0x000649A0 File Offset: 0x00062DA0
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookingEffectsCosmeticDecisions = (CookingEffectsCosmeticDecisions)synchronisedObject;
		ClientIngredientContainer clientIngredientContainer = base.gameObject.RequestInterface<ClientIngredientContainer>();
		if (clientIngredientContainer)
		{
			clientIngredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentChanged));
		}
		IClientAttachment clientAttachment = base.gameObject.RequestInterface<IClientAttachment>();
		if (clientAttachment != null)
		{
			clientAttachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		}
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x00064A06 File Offset: 0x00062E06
	public void OnCookingStarted()
	{
		if (this.m_cookingEffectsCosmeticDecisions.m_animator.IsActive() && this.m_cookingEffectsCosmeticDecisions.m_hasOnParam)
		{
			this.m_cookingEffectsCosmeticDecisions.m_animator.SetBool(CookingEffectsCosmeticDecisions.c_onParam, true);
		}
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x00064A44 File Offset: 0x00062E44
	public void OnCookingFinished()
	{
		if (this.m_cookingEffectsCosmeticDecisions.m_animator.IsActive() && this.m_cookingEffectsCosmeticDecisions.m_hasOnParam)
		{
			this.m_cookingEffectsCosmeticDecisions.m_animator.SetBool(CookingEffectsCosmeticDecisions.c_onParam, false);
		}
		if (this.m_cookingProp < 1f)
		{
			this.m_cookingEffectsCosmeticDecisions.SetColor(Color.white);
			this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(0f);
		}
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00064ABC File Offset: 0x00062EBC
	public void OnCookingPropChanged(float _newProp)
	{
		this.m_cookingProp = _newProp;
		if (this.m_flammable == null || !this.m_flammable.OnFire())
		{
			if (_newProp < 1f)
			{
				this.m_cookingEffectsCosmeticDecisions.SetColor(Color.white);
				this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(0f);
			}
			else if (_newProp >= 1f && _newProp < 2f)
			{
				this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(20f);
			}
			else if (_newProp >= 2f)
			{
				this.m_cookingEffectsCosmeticDecisions.SetColor(Color.black);
				this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(15f);
			}
		}
		else
		{
			this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(0f);
		}
		this.m_cookingProp = _newProp;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00064B93 File Offset: 0x00062F93
	private void OnIgnitionChange(bool _state)
	{
		this.OnCookingPropChanged(this.m_cookingProp);
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00064BA4 File Offset: 0x00062FA4
	private void OnAttachmentChanged(IParentable _parentable)
	{
		if (this.m_flammable)
		{
			this.m_flammable.UnregisterIgnitionCallback(new ClientFlammable.IgnitionCallback(this.OnIgnitionChange));
		}
		this.m_flammable = null;
		if (_parentable != null)
		{
			this.m_flammable = _parentable.GetAttachPoint(base.gameObject).parent.gameObject.GetComponent<ClientFlammable>();
		}
		if (this.m_flammable)
		{
			this.m_flammable.RegisterIgnitionCallback(new ClientFlammable.IgnitionCallback(this.OnIgnitionChange));
		}
		this.OnCookingPropChanged(this.m_cookingProp);
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00064C3C File Offset: 0x0006303C
	private void OnContentChanged(AssembledDefinitionNode[] _contents)
	{
		if (_contents.Length == 0)
		{
			if (this.m_cookingEffectsCosmeticDecisions.m_animator.IsActive() && this.m_cookingEffectsCosmeticDecisions.m_hasCookingParam)
			{
				this.m_cookingEffectsCosmeticDecisions.m_animator.SetBool(CookingEffectsCosmeticDecisions.c_cookingParam, false);
			}
			this.m_cookingEffectsCosmeticDecisions.SetEmissionRate(0f);
		}
		else if (this.m_cookingEffectsCosmeticDecisions.m_animator.IsActive() && this.m_cookingEffectsCosmeticDecisions.m_hasCookingParam)
		{
			this.m_cookingEffectsCosmeticDecisions.m_animator.SetBool(CookingEffectsCosmeticDecisions.c_cookingParam, true);
		}
	}

	// Token: 0x04000DB1 RID: 3505
	private CookingEffectsCosmeticDecisions m_cookingEffectsCosmeticDecisions;

	// Token: 0x04000DB2 RID: 3506
	private ClientFlammable m_flammable;

	// Token: 0x04000DB3 RID: 3507
	private float m_cookingProp;
}
