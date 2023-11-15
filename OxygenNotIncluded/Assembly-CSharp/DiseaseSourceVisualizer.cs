using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200076F RID: 1903
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
	// Token: 0x06003494 RID: 13460 RVA: 0x0011A8DD File Offset: 0x00118ADD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisibility();
		Components.DiseaseSourceVisualizers.Add(this);
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0011A8F8 File Offset: 0x00118AF8
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnViewModeChanged));
		base.OnCleanUp();
		Components.DiseaseSourceVisualizers.Remove(this);
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0011A95C File Offset: 0x00118B5C
	private void CreateVisualizer()
	{
		if (this.visualizer != null)
		{
			return;
		}
		if (GameScreenManager.Instance.worldSpaceCanvas == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0011A9AC File Offset: 0x00118BAC
	public void UpdateVisibility()
	{
		this.CreateVisualizer();
		if (string.IsNullOrEmpty(this.alwaysShowDisease))
		{
			this.visible = false;
		}
		else
		{
			Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
			if (disease != null)
			{
				this.SetVisibleDisease(disease);
			}
		}
		if (OverlayScreen.Instance != null)
		{
			this.Show(OverlayScreen.Instance.GetMode());
		}
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0011AA14 File Offset: 0x00118C14
	private void SetVisibleDisease(Disease disease)
	{
		Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
		Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
		Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
		component.sprite = overlaySprite;
		component.color = colorByName;
		this.visible = true;
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x0011AA76 File Offset: 0x00118C76
	private void Update()
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.visualizer.transform.SetPosition(base.transform.GetPosition() + this.offset);
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x0011AAAE File Offset: 0x00118CAE
	private void OnViewModeChanged(HashedString mode)
	{
		this.Show(mode);
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0011AAB7 File Offset: 0x00118CB7
	public void Show(HashedString mode)
	{
		base.enabled = (this.visible && mode == OverlayModes.Disease.ID);
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(base.enabled);
		}
	}

	// Token: 0x04001FC0 RID: 8128
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04001FC1 RID: 8129
	private GameObject visualizer;

	// Token: 0x04001FC2 RID: 8130
	private bool visible;

	// Token: 0x04001FC3 RID: 8131
	public string alwaysShowDisease;
}
