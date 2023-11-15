using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000994 RID: 2452
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x0600485D RID: 18525 RVA: 0x001982E5 File Offset: 0x001964E5
	public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600485E RID: 18526 RVA: 0x001982ED File Offset: 0x001964ED
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x0600485F RID: 18527 RVA: 0x001982F0 File Offset: 0x001964F0
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06004860 RID: 18528 RVA: 0x001982F3 File Offset: 0x001964F3
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06004861 RID: 18529 RVA: 0x001982F8 File Offset: 0x001964F8
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = this.AnimName,
					animPlaySpeedModifier = 0.5f
				},
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("shower_identify_kanim"),
					initialAnim = "identify_off",
					playMode = KAnim.PlayMode.Once
				},
				this.questionMarkAnimConfig
			};
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06004862 RID: 18530 RVA: 0x0019838E File Offset: 0x0019658E
	public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			return ClusterGrid.Instance.GetCellRevealLevel(base.Location);
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06004863 RID: 18531 RVA: 0x001983A0 File Offset: 0x001965A0
	public string AnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return "unknown";
			}
			return "idle_loop";
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06004864 RID: 18532 RVA: 0x001983BE File Offset: 0x001965BE
	public string QuestionMarkAnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return this.questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x001983E4 File Offset: 0x001965E4
	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(origin, parent);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.SwapAnims(new KAnimFile[]
		{
			this.questionMarkAnimConfig.animFile
		});
		kbatchedAnimController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kbatchedAnimController;
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x00198448 File Offset: 0x00196648
	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(false);
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06004867 RID: 18535 RVA: 0x00198489 File Offset: 0x00196689
	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x00198498 File Offset: 0x00196698
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x0019849B File Offset: 0x0019669B
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x0019849E File Offset: 0x0019669E
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x001984AC File Offset: 0x001966AC
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			this.Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[]
				{
					this.AnimConfigs[0].animFile
				});
				KBatchedAnimController externalAnimController = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			this.RefreshVisuals();
			this.Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			this.RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !this.revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x00198569 File Offset: 0x00196769
	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x00198584 File Offset: 0x00196784
	public void RefreshVisuals()
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim != null)
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.Play(this.AnimName, KAnim.PlayMode.Loop, 1f, 0f);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x00198600 File Offset: 0x00196800
	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		this.revealed = true;
		this.RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x0019865B File Offset: 0x0019685B
	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x04002FF1 RID: 12273
	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	// Token: 0x04002FF2 RID: 12274
	public string p_name;

	// Token: 0x04002FF3 RID: 12275
	public string clusterAnimName;

	// Token: 0x04002FF4 RID: 12276
	public bool revealed;
}
