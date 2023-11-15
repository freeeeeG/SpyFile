using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class ClientBlenderCosmeticDecisions : ClientSynchroniserBase, IClientMixingNotifed
{
	// Token: 0x06001111 RID: 4369 RVA: 0x00061BB4 File Offset: 0x0005FFB4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_blenderCosmeticDecisions = (BlenderCosmeticDecisions)synchronisedObject;
		if (this.m_blenderCosmeticDecisions.m_animator == null)
		{
		}
		this.m_iIngredientContents = base.gameObject.RequireComponent<ClientIngredientContainer>();
		this.m_iIngredientContents.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentChanged));
		this.m_blenderCosmeticDecisions.m_contentsObject.SetActive(false);
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00061C1C File Offset: 0x0006001C
	public void OnMixingStarted()
	{
		if (this.m_blenderCosmeticDecisions.m_animator.IsActive() && this.m_blenderCosmeticDecisions.m_hasOnParam)
		{
			this.m_blenderCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, true);
		}
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x00061C59 File Offset: 0x00060059
	public void OnMixingFinished()
	{
		if (this.m_blenderCosmeticDecisions.m_animator.IsActive() && this.m_blenderCosmeticDecisions.m_hasOnParam)
		{
			this.m_blenderCosmeticDecisions.m_animator.SetBool(ContentsCosmeticDecisions.c_onParam, false);
		}
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x00061C98 File Offset: 0x00060098
	public void OnMixingPropChanged(float newProp)
	{
		if (this.m_blenderCosmeticDecisions.m_animator.IsActive() && this.m_blenderCosmeticDecisions.m_hasFillParam)
		{
			this.m_blenderCosmeticDecisions.m_animator.SetInteger(BlenderCosmeticDecisions.c_FillParam, this.m_iIngredientContents.GetContentsCount());
		}
		if (this.m_blenderCosmeticDecisions.m_prefabLookup != null)
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
			this.m_validRecipe = (this.m_blenderCosmeticDecisions.m_prefabLookup.GetPrefabForNode(mixedCompositeAssembledNode) != null);
		}
		if (newProp > 1f)
		{
			float t = Mathf.Min(2f, newProp) - 1f;
			Color b = Color.Lerp(this.m_colour, Color.black, 0.8f);
			Color surfaceColour = Color.Lerp(this.m_colour, b, t);
			this.SetRendererColourRecursive(this.m_blenderCosmeticDecisions.m_contentsObject, surfaceColour);
		}
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x00061DC8 File Offset: 0x000601C8
	private void OnContentChanged(AssembledDefinitionNode[] _contents)
	{
		if (this.m_blenderCosmeticDecisions.m_animator.IsActive() && this.m_blenderCosmeticDecisions.m_hasFillParam)
		{
			this.m_blenderCosmeticDecisions.m_animator.SetInteger(BlenderCosmeticDecisions.c_FillParam, this.m_iIngredientContents.GetContentsCount());
		}
		this.m_colour = this.GetNewColour(_contents, false);
		this.SetRendererColourRecursive(this.m_blenderCosmeticDecisions.m_contentsObject, this.m_colour);
		if (_contents.Length == 0)
		{
			this.m_blenderCosmeticDecisions.m_contentsObject.SetActive(false);
		}
		else
		{
			this.m_blenderCosmeticDecisions.m_contentsObject.SetActive(true);
			if (this.m_blenderCosmeticDecisions.m_hasFillParam)
			{
				this.m_blenderCosmeticDecisions.m_animator.SetInteger(BlenderCosmeticDecisions.c_FillParam, _contents.Length);
			}
		}
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00061E98 File Offset: 0x00060298
	public void SetRendererColourRecursive(GameObject _object, Color _surfaceColour)
	{
		Renderer component = _object.GetComponent<Renderer>();
		if (component && component.material.name == this.m_blenderCosmeticDecisions.m_surfaceMaterialName + " (Instance)")
		{
			component.material.SetColor("_MaskColor", _surfaceColour);
		}
		for (int i = 0; i < _object.transform.childCount; i++)
		{
			this.SetRendererColourRecursive(_object.transform.GetChild(i).gameObject, _surfaceColour);
		}
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x00061F28 File Offset: 0x00060328
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

	// Token: 0x04000D3A RID: 3386
	private BlenderCosmeticDecisions m_blenderCosmeticDecisions;

	// Token: 0x04000D3B RID: 3387
	private ClientIngredientContainer m_iIngredientContents;

	// Token: 0x04000D3C RID: 3388
	private Color m_colour = Color.white;

	// Token: 0x04000D3D RID: 3389
	private bool m_validRecipe = true;
}
