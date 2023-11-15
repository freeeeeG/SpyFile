using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D8 RID: 2520
[ExecutionDependency(typeof(PlayerIDProvider))]
[ExecutionDependency(typeof(KitchenBootstrapManager))]
[ExecutionDependency(typeof(PlayerControls))]
[ExecutionDependency(typeof(CampaignKitchenLoaderManager))]
[ExecutionDependency(typeof(CompetitiveKitchenLoaderManager))]
public class ChefMeshReplacer : MonoBehaviour
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x0600314B RID: 12619 RVA: 0x000E6E4E File Offset: 0x000E524E
	public GameObject ChefModel
	{
		get
		{
			return this.m_cachedChefModel;
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x000E6E56 File Offset: 0x000E5256
	public GameSession.SelectedChefData GetChefData()
	{
		return this.chefDataCopy;
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x000E6E5E File Offset: 0x000E525E
	private void Awake()
	{
		this.m_maskParamID = Shader.PropertyToID("_MaskColor");
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x000E6E70 File Offset: 0x000E5270
	public void SetChefData(GameSession.SelectedChefData chefData, bool force = false)
	{
		if (chefData != null)
		{
			string headName = chefData.Character.HeadName;
			ChefColourData colour = chefData.Colour;
			this.chefDataCopy = chefData;
			bool flag = headName != this.m_currentHeadName;
			if (force || flag || colour != this.m_currentColour)
			{
				this.ReplaceModel(chefData, force);
				if (force || flag)
				{
					this.SetHeadVisibility(chefData.Character.HeadName);
				}
				this.m_currentHeadName = headName;
				this.m_currentColour = colour;
			}
		}
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x000E6EFC File Offset: 0x000E52FC
	private void SetHeadVisibility(string _headName)
	{
		if (this.m_chefParts.Count == 0)
		{
			this.CacheChefParts();
		}
		ChefMeshReplacer.ChefParts chefParts = this.m_chefParts.SafeGet(_headName, default(ChefMeshReplacer.ChefParts));
		SkinnedMeshRenderer head = chefParts.m_head;
		List<SkinnedMeshRenderer> hands = chefParts.m_hands;
		for (int i = 0; i < this.m_allHeads.Count; i++)
		{
			this.m_allHeads._items[i].gameObject.SetActive(false);
		}
		if (head != null)
		{
			head.gameObject.SetActive(true);
			for (int j = 0; j < hands.Count; j++)
			{
				hands[j].sharedMaterial = head.sharedMaterial;
			}
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x000E6FC4 File Offset: 0x000E53C4
	private void ReplaceModel(GameSession.SelectedChefData _chefData, bool _force)
	{
		if (this.m_cachedChefModel == null)
		{
			this.m_cachedChefModel = base.transform.FindChildRecursive("Chef").gameObject;
		}
		if (this.m_currentHeadName == _chefData.Character.HeadName && this.m_currentColour != _chefData.Colour && !_force)
		{
			this.AssignBodyColour(this.m_cachedChefModel, _chefData);
			return;
		}
		GameObject gameObject = null;
		ChefMeshReplacer.ChefModelType modelType = this.m_modelType;
		if (modelType != ChefMeshReplacer.ChefModelType.InGame)
		{
			if (modelType != ChefMeshReplacer.ChefModelType.FrontEnd)
			{
				if (modelType == ChefMeshReplacer.ChefModelType.UI)
				{
					gameObject = _chefData.Character.UIModelPrefab;
				}
			}
			else
			{
				gameObject = _chefData.Character.FrontendModelPrefab;
			}
		}
		else
		{
			gameObject = _chefData.Character.ModelPrefab;
		}
		if (this.m_lastModelPrefab != gameObject)
		{
			this.m_lastModelPrefab = gameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.SetObjectLayer(base.gameObject.layer);
			gameObject2.name = this.m_cachedChefModel.name;
			gameObject2.transform.SetParent(this.m_cachedChefModel.transform.parent, false);
			gameObject2.transform.localPosition = this.m_cachedChefModel.transform.localPosition;
			gameObject2.transform.localRotation = this.m_cachedChefModel.transform.localRotation;
			gameObject2.transform.localScale = this.m_cachedChefModel.transform.localScale;
			Animator animator = this.m_chefTemplatePrefab.RequireComponent<Animator>();
			Animator animator2 = gameObject2.RequireComponent<Animator>();
			animator2.runtimeAnimatorController = animator.runtimeAnimatorController;
			AnimationEventData other = this.m_chefTemplatePrefab.RequireComponent<AnimationEventData>();
			AnimationEventData animationEventData = gameObject2.RequestComponent<AnimationEventData>();
			if (animationEventData == null)
			{
				animationEventData = gameObject2.AddComponent<AnimationEventData>();
			}
			animationEventData.Copy(other);
			if (gameObject2.RequestComponent<AnimatorRumbleComponent>() == null)
			{
				gameObject2.AddComponent<AnimatorRumbleComponent>();
			}
			if (gameObject2.RequestComponent<AnimatorCommunications>() == null)
			{
				gameObject2.AddComponent<AnimatorCommunications>();
			}
			if (gameObject2.RequestComponent<AnimatorAudioComponent>() == null)
			{
				gameObject2.AddComponent<AnimatorAudioComponent>();
			}
			if (gameObject2.RequestComponent<ForwardTriggersToParent>() == null)
			{
				gameObject2.AddComponent<ForwardTriggersToParent>();
			}
			if (gameObject2.RequestComponent<RendererSceneInfo>() == null)
			{
				RendererSceneInfo rendererSceneInfo = gameObject2.AddComponent<RendererSceneInfo>();
				rendererSceneInfo.m_rendererClass = RendererSceneSettings.RendererClass.Avatar;
			}
			this.AssignBodyColour(gameObject2, _chefData);
			if (this.m_modelType == ChefMeshReplacer.ChefModelType.InGame)
			{
				this.CreateAttachPointFromTemplate(gameObject2, "Attachment");
				this.CreateAttachPointFromTemplate(gameObject2, "Slip_Particles");
				this.CreateAttachPointFromTemplate(gameObject2, "Attachment_Backpack");
				Transform[] componentsInChildren = gameObject2.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ComponentCacheRegistry.UpdateObject(componentsInChildren[i].gameObject);
				}
			}
			Animator animator3 = this.m_cachedChefModel.RequestComponent<Animator>();
			if (animator3 != null && animator3.runtimeAnimatorController != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator3.GetCurrentAnimatorStateInfo(0);
				animator2.Play(currentAnimatorStateInfo.fullPathHash, 0, currentAnimatorStateInfo.normalizedTime);
				animator2.Update(0f);
			}
			UnityEngine.Object.DestroyImmediate(this.m_cachedChefModel);
			GameObject gameObject3 = gameObject2.transform.FindChildRecursive("Body").gameObject;
			this.m_cachedChefModel = gameObject2;
			this.m_cachedBody = gameObject3;
			this.CacheChefParts();
			return;
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000E731C File Offset: 0x000E571C
	private void AssignBodyColour(GameObject _root, GameSession.SelectedChefData _data)
	{
		GameObject gameObject = _root.transform.FindChildRecursive("Body").gameObject;
		if (_data.Character.ColourisationMode == ChefMeshReplacer.ChefColourisationMode.SwapMaterial)
		{
			gameObject.RequireComponent<SkinnedMeshRenderer>().material = _data.Colour.ChefMaterial;
		}
		else
		{
			Material material = gameObject.RequireComponent<SkinnedMeshRenderer>().material;
			material.SetColor(this.m_maskParamID, _data.Colour.MaskColour);
		}
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000E7390 File Offset: 0x000E5790
	private GameObject CreateAttachPointFromTemplate(GameObject _root, string _templateObjectName)
	{
		Transform transform = this.m_chefTemplatePrefab.transform.FindChildRecursive(_templateObjectName);
		Transform transform2 = _root.transform.FindChildRecursive(transform.parent.name);
		GameObject gameObject = GameObjectUtils.CreateOnParent(transform2.gameObject, transform.gameObject.name);
		gameObject.transform.localPosition = transform.transform.localPosition;
		gameObject.transform.localRotation = transform.transform.localRotation;
		gameObject.transform.localScale = transform.transform.localScale;
		return gameObject;
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000E7420 File Offset: 0x000E5820
	private void CacheChefParts()
	{
		this.m_chefParts.Clear();
		this.m_allHeads.Clear();
		GameObject gameObject = this.m_cachedChefModel.RequestChild("Mesh");
		if (gameObject != null)
		{
			SkinnedMeshRenderer[] array = gameObject.gameObject.RequestComponentsInImmediateChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < array.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = array[i];
				if (skinnedMeshRenderer.name.Contains("Chef_"))
				{
					ChefMeshReplacer.ChefParts value = default(ChefMeshReplacer.ChefParts);
					value.m_hands = new List<SkinnedMeshRenderer>(2);
					value.m_head = skinnedMeshRenderer;
					SkinnedMeshRenderer[] array2 = skinnedMeshRenderer.gameObject.RequestComponentsRecursive<SkinnedMeshRenderer>();
					for (int j = 0; j < array2.Length; j++)
					{
						SkinnedMeshRenderer skinnedMeshRenderer2 = array[i];
						if (skinnedMeshRenderer2.name.Contains("Hand_"))
						{
							value.m_hands.Add(skinnedMeshRenderer2);
						}
					}
					this.m_chefParts.Add(value.m_head.name, value);
					this.m_allHeads.Add(value.m_head);
				}
			}
		}
	}

	// Token: 0x04002788 RID: 10120
	private const string c_headPrefix = "Chef_";

	// Token: 0x04002789 RID: 10121
	private const string c_handPrefix = "Hand_";

	// Token: 0x0400278A RID: 10122
	[SerializeField]
	private GameObject m_chefTemplatePrefab;

	// Token: 0x0400278B RID: 10123
	[SerializeField]
	private ChefMeshReplacer.ChefModelType m_modelType;

	// Token: 0x0400278C RID: 10124
	private string m_currentHeadName;

	// Token: 0x0400278D RID: 10125
	private ChefColourData m_currentColour;

	// Token: 0x0400278E RID: 10126
	private Dictionary<string, ChefMeshReplacer.ChefParts> m_chefParts = new Dictionary<string, ChefMeshReplacer.ChefParts>(20);

	// Token: 0x0400278F RID: 10127
	private FastList<SkinnedMeshRenderer> m_allHeads = new FastList<SkinnedMeshRenderer>(20);

	// Token: 0x04002790 RID: 10128
	private GameSession.SelectedChefData chefDataCopy;

	// Token: 0x04002791 RID: 10129
	private GameObject m_lastModelPrefab;

	// Token: 0x04002792 RID: 10130
	private GameObject m_cachedBody;

	// Token: 0x04002793 RID: 10131
	private GameObject m_cachedChefModel;

	// Token: 0x04002794 RID: 10132
	private PlayerIDProvider m_playerIDProvider;

	// Token: 0x04002795 RID: 10133
	public const string c_maskParamName = "_MaskColor";

	// Token: 0x04002796 RID: 10134
	private int m_maskParamID;

	// Token: 0x020009D9 RID: 2521
	public enum ChefColourisationMode
	{
		// Token: 0x04002798 RID: 10136
		SwapMaterial,
		// Token: 0x04002799 RID: 10137
		SwapColourValue
	}

	// Token: 0x020009DA RID: 2522
	public enum ChefModelType
	{
		// Token: 0x0400279B RID: 10139
		InGame,
		// Token: 0x0400279C RID: 10140
		FrontEnd,
		// Token: 0x0400279D RID: 10141
		UI
	}

	// Token: 0x020009DB RID: 2523
	private struct ChefParts
	{
		// Token: 0x0400279E RID: 10142
		public SkinnedMeshRenderer m_head;

		// Token: 0x0400279F RID: 10143
		public List<SkinnedMeshRenderer> m_hands;
	}
}
