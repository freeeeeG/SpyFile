using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004CB RID: 1227
[RequireComponent(typeof(IngredientContainer))]
public class ServerIngredientCatcher : ServerSynchroniserBase, IHandleCatch
{
	// Token: 0x060016AA RID: 5802 RVA: 0x00076BE0 File Offset: 0x00074FE0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_catcher = (IngredientCatcher)synchronisedObject;
		this.m_attachment = base.gameObject.RequestInterface<IAttachment>();
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x00076C08 File Offset: 0x00075008
	public bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		if (!_object.AllowCatch(this, _directionXZ))
		{
			return false;
		}
		if (this.m_catcher.m_requireAttached)
		{
			if (this.m_attachment == null)
			{
				return false;
			}
			if (!this.m_attachment.IsAttached())
			{
				return false;
			}
		}
		if (!this.m_allowCatchingCallback(_object.AccessGameObject()))
		{
			return false;
		}
		IOrderDefinition orderDefinition = _object.AccessGameObject().RequestInterface<IOrderDefinition>();
		if (orderDefinition != null && orderDefinition.GetOrderComposition().Simpilfy() != AssembledDefinitionNode.NullNode)
		{
			IContainerTransferBehaviour containerTransferBehaviour = _object.AccessGameObject().RequestInterface<IContainerTransferBehaviour>();
			ServerIngredientContainer component = base.gameObject.GetComponent<ServerIngredientContainer>();
			if (containerTransferBehaviour != null && component != null && containerTransferBehaviour.CanTransferToContainer(component))
			{
				return component.CanAddIngredient(orderDefinition.GetOrderComposition());
			}
		}
		return false;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x00076CD8 File Offset: 0x000750D8
	public void HandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		GameObject gameObject = _object.AccessGameObject();
		IThrowable throwable = gameObject.RequireInterface<IThrowable>();
		IThrower thrower = throwable.GetThrower();
		if (thrower != null)
		{
			GameObject gameObject2 = (thrower as MonoBehaviour).gameObject;
			ServerMessenger.Achievement(gameObject2, 12, 1);
		}
		IOrderDefinition orderDefinition = gameObject.RequireInterface<IOrderDefinition>();
		if (orderDefinition.GetOrderComposition().Simpilfy() != AssembledDefinitionNode.NullNode)
		{
			IContainerTransferBehaviour containerTransferBehaviour = gameObject.RequireInterface<IContainerTransferBehaviour>();
			ServerIngredientContainer component = base.gameObject.GetComponent<ServerIngredientContainer>();
			if (containerTransferBehaviour != null && component != null && containerTransferBehaviour.CanTransferToContainer(component))
			{
				containerTransferBehaviour.TransferToContainer(null, component, true);
				component.InformOfInternalChange();
				gameObject.SetActive(false);
				NetworkUtils.DestroyObject(gameObject);
			}
		}
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x00076D8A File Offset: 0x0007518A
	public void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ)
	{
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x00076D8C File Offset: 0x0007518C
	public int GetCatchingPriority()
	{
		return 0;
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x00076D8F File Offset: 0x0007518F
	public void RegisterAllowItemCatching(QueryForCatching _allowCatchingCallback)
	{
		this.m_allowCatchingCallback = (QueryForCatching)Delegate.Combine(this.m_allowCatchingCallback, _allowCatchingCallback);
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x00076DA8 File Offset: 0x000751A8
	public void UnregisterAllowItemCatching(QueryForCatching _allowCatchingCallback)
	{
		this.m_allowCatchingCallback = (QueryForCatching)Delegate.Remove(this.m_allowCatchingCallback, _allowCatchingCallback);
	}

	// Token: 0x040010F4 RID: 4340
	private IngredientCatcher m_catcher;

	// Token: 0x040010F5 RID: 4341
	private IAttachment m_attachment;

	// Token: 0x040010F6 RID: 4342
	private QueryForCatching m_allowCatchingCallback = (GameObject _object) => true;
}
