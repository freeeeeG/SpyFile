using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
[ExecutionDependency(typeof(IOrderDefinition))]
public abstract class MealCosmeticDecisions : MonoBehaviour
{
	// Token: 0x06001224 RID: 4644
	protected abstract IClientOrderDefinition FindOrderDefinition();

	// Token: 0x06001225 RID: 4645
	protected abstract void UpdateAppearance(AssembledDefinitionNode _contents);

	// Token: 0x06001226 RID: 4646 RVA: 0x00061FFC File Offset: 0x000603FC
	protected virtual void Start()
	{
		this.m_iOrderDefinition = this.FindOrderDefinition();
		this.m_iOrderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		this.m_container = GameObjectUtils.CreateOnParent(base.gameObject, "IngredientContainer");
		this.m_rendererSceneInfo = base.gameObject.RequestComponent<RendererSceneInfo>();
		if (this.m_rendererSceneInfo == null)
		{
			this.m_rendererSceneInfo = base.gameObject.AddComponent<RendererSceneInfo>();
			this.m_rendererSceneInfo.m_rendererClass = RendererSceneSettings.RendererClass.MealCosmetic;
		}
		this.OnOrderCompositionChanged(this.m_iOrderDefinition.GetOrderComposition());
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00062092 File Offset: 0x00060492
	public void ForceCopositionUpdate(AssembledDefinitionNode _contents)
	{
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00062094 File Offset: 0x00060494
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		this.UpdateAppearance(_contents);
		if (this.m_rendererSceneInfo != null)
		{
			this.m_rendererSceneInfo.ApplySettings(true);
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x000620BA File Offset: 0x000604BA
	protected virtual void OnDestroy()
	{
		if (this.m_iOrderDefinition != null)
		{
			this.m_iOrderDefinition.UnregisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		}
	}

	// Token: 0x04000E3A RID: 3642
	protected GameObject m_container;

	// Token: 0x04000E3B RID: 3643
	protected IClientOrderDefinition m_iOrderDefinition;

	// Token: 0x04000E3C RID: 3644
	private RendererSceneInfo m_rendererSceneInfo;
}
