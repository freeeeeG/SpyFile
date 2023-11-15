using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200038D RID: 909
public class ClientCakeTinContentsCosmeticDecisions : ClientSynchroniserBase, IClientCookingNotifed
{
	// Token: 0x0600112A RID: 4394 RVA: 0x000628E8 File Offset: 0x00060CE8
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_contentsCosmeticDecisions = (CakeTinContentsCosmeticDecisions)synchronisedObject;
		this.m_iIngredientContents = this.m_contentsCosmeticDecisions.m_gameObject.RequireComponent<ClientIngredientContainer>();
		this.m_iIngredientContents.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentChanged));
		this.m_IngredientCapacity = this.m_iIngredientContents.gameObject.RequireComponent<IngredientContainer>().m_capacity;
		this.m_contentsCosmeticDecisions.m_contentsObject.SetActive(false);
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0006295A File Offset: 0x00060D5A
	public void OnCookingStarted()
	{
		this.m_contentsCosmeticDecisions.m_animator.SetBool(ClientCakeTinContentsCosmeticDecisions.m_iCooking, true);
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x00062972 File Offset: 0x00060D72
	public void OnCookingFinished()
	{
		this.m_contentsCosmeticDecisions.m_animator.SetBool(ClientCakeTinContentsCosmeticDecisions.m_iCooking, false);
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x0006298C File Offset: 0x00060D8C
	public void OnCookingPropChanged(float newProp)
	{
		this.m_contentsCosmeticDecisions.m_animator.SetFloat(ClientCakeTinContentsCosmeticDecisions.m_iProgress, newProp);
		if (newProp > 1f && newProp < 2f)
		{
			float t = newProp - 1f;
			Color b = Color.Lerp(this.m_uncookedSurfaceColour, Color.black, 0.8f);
			Color b2 = Color.Lerp(this.m_uncookedBubbleColour, Color.black, 0.8f);
			Color surfaceColour = Color.Lerp(this.m_uncookedSurfaceColour, b, t);
			Color bubbleColour = Color.Lerp(this.m_uncookedBubbleColour, b2, t);
			this.SetRendererColourRecursive(this.m_contentsCosmeticDecisions.m_contentsObject, surfaceColour, bubbleColour);
		}
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x00062A2C File Offset: 0x00060E2C
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
			float y = MathUtils.ClampedRemap((float)_contents.Length, 0f, (float)this.m_IngredientCapacity, this.m_contentsCosmeticDecisions.m_contentsYPositionWhenEmpty, this.m_contentsCosmeticDecisions.m_contentsYPositionWhenFull);
			this.m_contentsCosmeticDecisions.m_contentsObject.transform.localPosition = this.m_contentsCosmeticDecisions.m_contentsObject.transform.localPosition.WithY(y);
		}
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x00062B00 File Offset: 0x00060F00
	public void SetRendererColourRecursive(GameObject _object, Color _surfaceColour, Color _bubbleColour)
	{
		Renderer component = _object.GetComponent<Renderer>();
		if (component)
		{
			if (component.material.name == this.m_contentsCosmeticDecisions.m_surfaceMaterialName + " (Instance)")
			{
				component.material.color = _surfaceColour;
			}
			else if (component.material.name == this.m_contentsCosmeticDecisions.m_bubbleMaterialName + " (Instance)")
			{
				component.material.color = _bubbleColour;
			}
		}
		for (int i = 0; i < _object.transform.childCount; i++)
		{
			this.SetRendererColourRecursive(_object.transform.GetChild(i).gameObject, _surfaceColour, _bubbleColour);
		}
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x00062BC8 File Offset: 0x00060FC8
	private Color GetNewColour(AssembledDefinitionNode[] _contents, bool bubble)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = 0;
		for (int i = 0; i < _contents.Length; i++)
		{
			MixedCompositeAssembledNode mixedCompositeAssembledNode = _contents[i] as MixedCompositeAssembledNode;
			for (int j = 0; j < mixedCompositeAssembledNode.m_composition.Length; j++)
			{
				IngredientAssembledNode ingredientAssembledNode = mixedCompositeAssembledNode.m_composition[j] as IngredientAssembledNode;
				if (ingredientAssembledNode != null)
				{
					num += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.r;
					num2 += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.g;
					num3 += ingredientAssembledNode.m_ingriedientOrderNode.m_colour.b;
					num4++;
				}
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

	// Token: 0x04000D4C RID: 3404
	private CakeTinContentsCosmeticDecisions m_contentsCosmeticDecisions;

	// Token: 0x04000D4D RID: 3405
	private ClientIngredientContainer m_iIngredientContents;

	// Token: 0x04000D4E RID: 3406
	private int m_IngredientCapacity;

	// Token: 0x04000D4F RID: 3407
	private Color m_uncookedSurfaceColour = Color.white;

	// Token: 0x04000D50 RID: 3408
	private Color m_uncookedBubbleColour = Color.white;

	// Token: 0x04000D51 RID: 3409
	private static int m_iCooking = Animator.StringToHash("Cooking");

	// Token: 0x04000D52 RID: 3410
	private static int m_iProgress = Animator.StringToHash("Progress");
}
