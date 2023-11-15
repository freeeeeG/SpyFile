using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class ClientContentsCosmeticDecisions : ClientSynchroniserBase, IClientCookingNotifed, IClientMixingNotifed
{
	// Token: 0x06001174 RID: 4468 RVA: 0x00063D4C File Offset: 0x0006214C
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_contentsCosmeticDecisions = (ContentsCosmeticDecisions)synchronisedObject;
		if (this.m_contentsCosmeticDecisions.m_animator == null)
		{
		}
		this.m_iIngredientContents = this.m_contentsCosmeticDecisions.m_gameObject.RequireComponent<ClientIngredientContainer>();
		this.m_iIngredientContents.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentChanged));
		this.m_IngredientCapacity = this.m_iIngredientContents.gameObject.RequireComponent<IngredientContainer>().m_capacity;
		this.m_contentsCosmeticDecisions.m_contentsObject.SetActive(false);
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x00063DD4 File Offset: 0x000621D4
	public void OnCookingStarted()
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasOnParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, true);
		}
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x00063E11 File Offset: 0x00062211
	public void OnCookingFinished()
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasOnParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, false);
		}
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00063E50 File Offset: 0x00062250
	public void OnCookingPropChanged(float newProp)
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasProgressParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetFloat(ContentsCosmeticDecisions.c_progressParam, newProp);
		}
		if (newProp > 1f)
		{
			float t = Mathf.Min(2f, newProp) - 1f;
			if (!this.m_validRecipe)
			{
				t = 1f;
			}
			Color b = Color.Lerp(this.m_uncookedSurfaceColour, Color.black, 0.8f);
			Color b2 = Color.Lerp(this.m_uncookedBubbleColour, Color.black, 0.8f);
			Color surfaceColour = Color.Lerp(this.m_uncookedSurfaceColour, b, t);
			Color bubbleColour = Color.Lerp(this.m_uncookedBubbleColour, b2, t);
			this.SetRendererColourRecursive(this.m_contentsCosmeticDecisions.m_contentsObject, surfaceColour, bubbleColour);
		}
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00063F23 File Offset: 0x00062323
	public void OnMixingStarted()
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasOnParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, true);
		}
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x00063F60 File Offset: 0x00062360
	public void OnMixingFinished()
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasOnParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, false);
		}
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x00063FA0 File Offset: 0x000623A0
	public void OnMixingPropChanged(float newProp)
	{
		if (this.m_contentsCosmeticDecisions.m_animator.IsActive() && this.m_contentsCosmeticDecisions.m_hasProgressParam)
		{
			this.m_contentsCosmeticDecisions.m_animator.SetFloat(ContentsCosmeticDecisions.c_progressParam, newProp);
		}
		if (this.m_contentsCosmeticDecisions.m_prefabLookup != null)
		{
			MixedCompositeAssembledNode mixedCompositeAssembledNode = new MixedCompositeAssembledNode();
			mixedCompositeAssembledNode.m_composition = this.m_iIngredientContents.GetContents();
			mixedCompositeAssembledNode.m_recordedProgress = new float?(newProp);
			if (newProp >= 1f)
			{
				if (newProp < 2f)
				{
					mixedCompositeAssembledNode.m_progress = MixedCompositeOrderNode.MixingProgress.Mixed;
				}
				else
				{
					mixedCompositeAssembledNode.m_progress = MixedCompositeOrderNode.MixingProgress.OverMixed;
				}
			}
			else
			{
				mixedCompositeAssembledNode.m_progress = MixedCompositeOrderNode.MixingProgress.Unmixed;
			}
			this.m_validRecipe = (this.m_contentsCosmeticDecisions.m_prefabLookup.GetPrefabForNode(mixedCompositeAssembledNode) != null);
		}
		if (newProp > 1f)
		{
			float t = Mathf.Min(2f, newProp) - 1f;
			if (!this.m_validRecipe)
			{
				t = 1f;
			}
			Color b = Color.Lerp(this.m_uncookedSurfaceColour, Color.black, 0.8f);
			Color b2 = Color.Lerp(this.m_uncookedBubbleColour, Color.black, 0.8f);
			Color surfaceColour = Color.Lerp(this.m_uncookedSurfaceColour, b, t);
			Color bubbleColour = Color.Lerp(this.m_uncookedBubbleColour, b2, t);
			this.SetRendererColourRecursive(this.m_contentsCosmeticDecisions.m_contentsObject, surfaceColour, bubbleColour);
		}
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x00064100 File Offset: 0x00062500
	private void OnContentChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_uncookedSurfaceColour = this.GetNewColour(_contents, false);
		this.m_uncookedBubbleColour = this.GetNewColour(_contents, true);
		this.SetRendererColourRecursive(this.m_contentsCosmeticDecisions.m_contentsObject, this.m_uncookedSurfaceColour, this.m_uncookedBubbleColour);
		if (_contents.Length == 0)
		{
			this.m_contentsCosmeticDecisions.m_contentsObject.SetActive(false);
		}
		else
		{
			this.m_contentsCosmeticDecisions.m_contentsObject.SetActive(true);
			float num = Mathf.Clamp01((float)_contents.Length / (float)this.m_IngredientCapacity);
			float y = num * (this.m_contentsCosmeticDecisions.m_contentsYPositionWhenFull - this.m_contentsCosmeticDecisions.m_contentsYPositionWhenEmpty) + this.m_contentsCosmeticDecisions.m_contentsYPositionWhenEmpty;
			this.m_contentsCosmeticDecisions.m_contentsObject.transform.localPosition = this.m_contentsCosmeticDecisions.m_contentsObject.transform.localPosition.WithY(y);
			this.m_contentsCosmeticDecisions.m_contentsObject.transform.localScale = Vector3.Lerp(this.m_contentsCosmeticDecisions.m_contentsScale.m_empty, this.m_contentsCosmeticDecisions.m_contentsScale.m_full, this.m_contentsCosmeticDecisions.m_contentsScale.m_curve.Evaluate(num));
		}
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x00064230 File Offset: 0x00062630
	public void SetRendererColourRecursive(GameObject _object, Color _surfaceColour, Color _bubbleColour)
	{
		Renderer component = _object.GetComponent<Renderer>();
		if (component)
		{
			if (component.material.name == this.m_contentsCosmeticDecisions.m_surfaceMaterialName + " (Instance)")
			{
				component.material.SetColor("_MaskColor", _surfaceColour);
			}
			else if (component.material.name == this.m_contentsCosmeticDecisions.m_bubbleMaterialName + " (Instance)")
			{
				component.material.SetColor("_MaskColor", _bubbleColour);
			}
		}
		for (int i = 0; i < _object.transform.childCount; i++)
		{
			this.SetRendererColourRecursive(_object.transform.GetChild(i).gameObject, _surfaceColour, _bubbleColour);
		}
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x00064300 File Offset: 0x00062700
	private Color GetNewColour(AssembledDefinitionNode[] _contents, bool bubble)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		for (int i = 0; i < _contents.Length; i++)
		{
			IngredientAssembledNode ingredientAssembledNode = _contents[i] as IngredientAssembledNode;
			if (ingredientAssembledNode != null)
			{
				num += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.r;
				num2 += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.g;
				num3 += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.b;
				num4++;
			}
		}
		if (num4 > 0)
		{
			num /= (float)num4;
			num2 /= (float)num4;
			num3 /= (float)num4;
		}
		float num5 = 0.25f;
		if (bubble)
		{
			num += num5;
			num2 += num5;
			num3 += num5;
		}
		return new Color(num, num2, num3, 1f);
	}

	// Token: 0x04000D99 RID: 3481
	private ContentsCosmeticDecisions m_contentsCosmeticDecisions;

	// Token: 0x04000D9A RID: 3482
	private ClientIngredientContainer m_iIngredientContents;

	// Token: 0x04000D9B RID: 3483
	private int m_IngredientCapacity;

	// Token: 0x04000D9C RID: 3484
	private Color m_uncookedSurfaceColour = Color.white;

	// Token: 0x04000D9D RID: 3485
	private Color m_uncookedBubbleColour = Color.white;

	// Token: 0x04000D9E RID: 3486
	private bool m_validRecipe = true;
}
