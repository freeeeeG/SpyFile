using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020009DF RID: 2527
public class FrontendChef : MonoBehaviour
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06003160 RID: 12640 RVA: 0x000E773D File Offset: 0x000E5B3D
	public GameObject ChefModel
	{
		get
		{
			return (!(this.m_chefMeshReplacer != null)) ? null : this.m_chefMeshReplacer.ChefModel;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06003161 RID: 12641 RVA: 0x000E7761 File Offset: 0x000E5B61
	public FrontendChef.ShaderMode CurrentShaderMode
	{
		get
		{
			return this.m_CurrentShaderMode;
		}
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000E7769 File Offset: 0x000E5B69
	private void Awake()
	{
		this.m_chefMeshReplacer = base.gameObject.RequireComponent<ChefMeshReplacer>();
		this.m_ChefCustomisation = base.gameObject.RequestComponent<FrontendChefCustomisation>();
		this.m_HatMeshVisibility = base.gameObject.RequestComponent<HatMeshVisibility>();
		this.SetChefData();
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000E77A4 File Offset: 0x000E5BA4
	public void SetChefData(GameSession.SelectedChefData chefData, bool force = false)
	{
		this.m_SelectedChefData = chefData;
		this.m_bForce = force;
		this.SetChefData();
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x000E77BC File Offset: 0x000E5BBC
	private void SetChefData()
	{
		if (this.m_chefMeshReplacer != null && this.m_SelectedChefData != null)
		{
			GameObject chefModel = this.m_chefMeshReplacer.ChefModel;
			this.m_chefMeshReplacer.SetChefData(this.m_SelectedChefData, this.m_bForce);
			if (chefModel != this.m_chefMeshReplacer.ChefModel)
			{
				this.m_ChefRenderers.Clear();
				for (int i = 0; i < this.m_chefRendererMats.Count; i++)
				{
					if (this.m_chefRendererMats._items[i] != null)
					{
						this.m_chefRendererMats._items[i].Clear();
					}
				}
			}
			if (this.m_chefMeshReplacer.ChefModel != null)
			{
				if (chefModel != this.m_chefMeshReplacer.ChefModel)
				{
					this.UpdateChefRenderers();
					this.UpdateHatRenderers();
				}
				this.SetCorrectHat();
				this.m_ChefAnimator = this.m_chefMeshReplacer.ChefModel.RequireComponent<Animator>();
				if (this.m_ChefCustomisation != null)
				{
					this.m_bHasAnimationSet = this.m_ChefAnimator.HasParameter(this.m_AnimationSetVariable);
					this.m_ChefCustomisation.SetChefAnimator(this.m_ChefAnimator);
				}
				this.SetCorrectAnimations();
			}
			if (chefModel != this.m_chefMeshReplacer.ChefModel)
			{
				this.HideProps(true);
				this.SetCorrectShaders();
			}
		}
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x000E7920 File Offset: 0x000E5D20
	protected void HideProps(bool _updateAnimator)
	{
		if (this.FindProps())
		{
			for (int i = 0; i < this.m_props.Length; i++)
			{
				this.m_props[i].gameObject.SetActive(false);
			}
			if (_updateAnimator)
			{
				if (this.m_ChefAnimator == null && this.m_chefMeshReplacer.ChefModel != null)
				{
					this.m_ChefAnimator = this.m_chefMeshReplacer.ChefModel.RequireComponent<Animator>();
					this.m_bHasAnimationSet = this.m_ChefAnimator.HasParameter(this.m_AnimationSetVariable);
				}
				if (this.m_ChefAnimator != null)
				{
					this.m_ChefAnimator.Update(Time.deltaTime);
				}
			}
		}
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x000E79DF File Offset: 0x000E5DDF
	public void SetChefHat(HatMeshVisibility.VisState _hat)
	{
		this.m_HatState = _hat;
		this.SetCorrectHat();
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x000E79EE File Offset: 0x000E5DEE
	public void SetAnimationSet(FrontendChef.AnimationSet _animSet)
	{
		this.m_AnimationSet = _animSet;
		this.SetCorrectAnimations();
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x000E79FD File Offset: 0x000E5DFD
	public void SetShaderMode(FrontendChef.ShaderMode eShaderMode)
	{
		this.m_CurrentShaderMode = eShaderMode;
		this.SetCorrectShaders();
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x000E7A0C File Offset: 0x000E5E0C
	public void SetUIChefAmbientLighting(Color ambientColor)
	{
		this.m_CurrentUIAmbientColor = ambientColor;
		this.ApplyAmbientLighting();
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x000E7A1C File Offset: 0x000E5E1C
	private bool FindProps()
	{
		this.m_props = null;
		Transform transform = base.transform.FindChildRecursive("Props");
		if (transform != null)
		{
			int childCount = transform.childCount;
			if (childCount > 0)
			{
				this.m_props = new Transform[childCount];
				for (int i = 0; i < childCount; i++)
				{
					this.m_props[i] = transform.GetChild(i);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x000E7A8C File Offset: 0x000E5E8C
	private void UpdateChefRenderers()
	{
		if (this.m_chefMeshReplacer.ChefModel != null)
		{
			Renderer[] collection = this.m_chefMeshReplacer.ChefModel.RequestComponentsRecursive<Renderer>();
			this.m_ChefRenderers.Clear();
			this.m_ChefRenderers.AddRange(collection);
			int count = this.m_ChefRenderers.Count;
			int count2 = this.m_chefRendererMats.Count;
			if (count2 > count)
			{
				this.m_chefRendererMats.RemoveRange(count - 1, count2 - count);
			}
			for (int i = 0; i < count; i++)
			{
				Material[] materials = this.m_ChefRenderers._items[i].materials;
				if (i > this.m_chefRendererMats.Count - 1)
				{
					FastList<Material> fastList = new FastList<Material>(materials.Length);
					fastList.AddRange(materials);
					this.m_chefRendererMats.Add(fastList);
				}
				else
				{
					FastList<Material> fastList2 = this.m_chefRendererMats._items[i];
					if (fastList2 != null)
					{
						fastList2.Clear();
					}
					else
					{
						fastList2 = new FastList<Material>(materials.Length);
						this.m_chefRendererMats._items[i] = fastList2;
					}
					fastList2.AddRange(materials);
				}
			}
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000E7BAB File Offset: 0x000E5FAB
	private void UpdateHatRenderers()
	{
		if (this.m_chefMeshReplacer.ChefModel != null && this.m_HatMeshVisibility != null)
		{
			this.m_HatMeshVisibility.Setup(this.m_HatState);
		}
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000E7BE5 File Offset: 0x000E5FE5
	private void SetCorrectHat()
	{
		if (this.m_chefMeshReplacer.ChefModel != null && this.m_HatMeshVisibility != null)
		{
			this.m_HatMeshVisibility.SetState(this.m_HatState);
		}
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x000E7C20 File Offset: 0x000E6020
	private void SetCorrectAnimations()
	{
		if (this.m_ChefAnimator == null && this.m_chefMeshReplacer.ChefModel != null)
		{
			this.m_ChefAnimator = this.m_chefMeshReplacer.ChefModel.RequireComponent<Animator>();
			this.m_bHasAnimationSet = this.m_ChefAnimator.HasParameter(this.m_AnimationSetVariable);
		}
		if (this.m_ChefAnimator != null && this.m_bHasAnimationSet)
		{
			int value = (int)(this.m_AnimationSet + 1);
			this.m_ChefAnimator.SetInteger(this.m_AnimationSetVariable, value);
		}
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x000E7CB8 File Offset: 0x000E60B8
	private void SetCorrectShaders()
	{
		if (this.m_StandardSkinShader == null || this.m_StandardClothesShader == null || this.m_UIClothesShader == null || this.m_UISkinShader == null)
		{
			return;
		}
		if (this.m_ChefRenderers == null)
		{
			this.UpdateChefRenderers();
			this.UpdateHatRenderers();
		}
		if (this.m_ChefRenderers != null)
		{
			for (int i = 0; i < this.m_ChefRenderers.Count; i++)
			{
				Renderer renderer = this.m_ChefRenderers._items[i];
				if (renderer != null)
				{
					FastList<Material> fastList = this.m_chefRendererMats._items[i];
					for (int j = 0; j < fastList.Count; j++)
					{
						Material material = fastList._items[j];
						if (this.m_CurrentShaderMode == FrontendChef.ShaderMode.eStandard)
						{
							renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
							renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
							if (material.shader == this.m_UIClothesShader)
							{
								material.shader = this.m_StandardClothesShader;
							}
							else if (material.shader == this.m_UISkinShader)
							{
								material.shader = this.m_StandardSkinShader;
							}
						}
						else if (this.m_CurrentShaderMode == FrontendChef.ShaderMode.eUI)
						{
							renderer.lightProbeUsage = LightProbeUsage.Off;
							renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
							if (material.shader == this.m_StandardClothesShader)
							{
								material.shader = this.m_UIClothesShader;
							}
							else if (material.shader == this.m_StandardSkinShader)
							{
								material.shader = this.m_UISkinShader;
							}
						}
					}
				}
			}
		}
		if (this.m_CurrentShaderMode == FrontendChef.ShaderMode.eUI)
		{
			this.SetUIChefAmbientLighting(this.m_CurrentUIAmbientColor);
		}
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x000E7E7C File Offset: 0x000E627C
	private void ApplyAmbientLighting()
	{
		if (this.m_ChefRenderers != null)
		{
			Color currentUIAmbientColor = this.m_CurrentUIAmbientColor;
			for (int i = 0; i < this.m_ChefRenderers.Count; i++)
			{
				FastList<Material> fastList = this.m_chefRendererMats._items[i];
				for (int j = 0; j < fastList.Count; j++)
				{
					fastList._items[j].SetColor(FrontendChef.Uniforms._AmbientLighting, this.m_CurrentUIAmbientColor);
				}
			}
		}
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x000E7EF4 File Offset: 0x000E62F4
	private void OnEnable()
	{
		this.SetCorrectAnimations();
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x000E7EFC File Offset: 0x000E62FC
	private void OnDisable()
	{
		AutoDestructParticleSystem[] array = base.gameObject.RequestComponentsRecursive<AutoDestructParticleSystem>();
		for (int i = array.Length - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
	}

	// Token: 0x040027A5 RID: 10149
	[SerializeField]
	private Shader m_StandardSkinShader;

	// Token: 0x040027A6 RID: 10150
	[SerializeField]
	private Shader m_StandardClothesShader;

	// Token: 0x040027A7 RID: 10151
	[SerializeField]
	private Shader m_UISkinShader;

	// Token: 0x040027A8 RID: 10152
	[SerializeField]
	private Shader m_UIClothesShader;

	// Token: 0x040027A9 RID: 10153
	private ChefMeshReplacer m_chefMeshReplacer;

	// Token: 0x040027AA RID: 10154
	private Transform[] m_props;

	// Token: 0x040027AB RID: 10155
	private FrontendChefCustomisation m_ChefCustomisation;

	// Token: 0x040027AC RID: 10156
	private FastList<Renderer> m_ChefRenderers = new FastList<Renderer>(30);

	// Token: 0x040027AD RID: 10157
	private FastList<FastList<Material>> m_chefRendererMats = new FastList<FastList<Material>>(30);

	// Token: 0x040027AE RID: 10158
	private Animator m_ChefAnimator;

	// Token: 0x040027AF RID: 10159
	private HatMeshVisibility m_HatMeshVisibility;

	// Token: 0x040027B0 RID: 10160
	private GameSession.SelectedChefData m_SelectedChefData;

	// Token: 0x040027B1 RID: 10161
	private bool m_bForce;

	// Token: 0x040027B2 RID: 10162
	private int m_AnimationSetVariable = Animator.StringToHash("AnimationSet");

	// Token: 0x040027B3 RID: 10163
	private bool m_bHasAnimationSet;

	// Token: 0x040027B4 RID: 10164
	private FrontendChef.AnimationSet m_AnimationSet;

	// Token: 0x040027B5 RID: 10165
	private HatMeshVisibility.VisState m_HatState;

	// Token: 0x040027B6 RID: 10166
	private FrontendChef.ShaderMode m_CurrentShaderMode;

	// Token: 0x040027B7 RID: 10167
	private Color m_CurrentUIAmbientColor = Color.black;

	// Token: 0x020009E0 RID: 2528
	public enum AnimationSet
	{
		// Token: 0x040027B9 RID: 10169
		One,
		// Token: 0x040027BA RID: 10170
		Two,
		// Token: 0x040027BB RID: 10171
		Three,
		// Token: 0x040027BC RID: 10172
		Four
	}

	// Token: 0x020009E1 RID: 2529
	private static class Uniforms
	{
		// Token: 0x040027BD RID: 10173
		internal static readonly int _AmbientLighting = Shader.PropertyToID("_AmbientLighting");
	}

	// Token: 0x020009E2 RID: 2530
	public enum ShaderMode
	{
		// Token: 0x040027BF RID: 10175
		eStandard,
		// Token: 0x040027C0 RID: 10176
		eUI
	}
}
