using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020009CA RID: 2506
public class ClusterMapVisualizer : KMonoBehaviour
{
	// Token: 0x06004AFD RID: 19197 RVA: 0x001A6280 File Offset: 0x001A4480
	public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
	{
		this.entity = entity;
		this.pathDrawer = pathDrawer;
		this.animControllers = new List<KBatchedAnimController>();
		if (this.animContainer == null)
		{
			GameObject gameObject = new GameObject("AnimContainer", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform component = base.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.SetParent(component, false);
			component2.SetLocalPosition(new Vector3(0f, 0f, 0f));
			component2.sizeDelta = component.sizeDelta;
			component2.localScale = Vector3.one;
			this.animContainer = component2;
		}
		Vector3 position = ClusterGrid.Instance.GetPosition(entity);
		this.rectTransform().SetLocalPosition(position);
		this.RefreshPathDrawing();
		entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x001A6356 File Offset: 0x001A4556
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.doesTransitionAnimation)
		{
			new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
		}
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x001A6378 File Offset: 0x001A4578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.entity != null)
		{
			if (this.doesTransitionAnimation)
			{
				base.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
			}
			if (this.entity is Clustercraft)
			{
				new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is BallisticClusterGridEntity)
			{
				new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity.Layer == EntityLayer.FX)
			{
				new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
			}
		}
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x001A641C File Offset: 0x001A461C
	protected override void OnCleanUp()
	{
		if (this.entity != null)
		{
			this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001A644E File Offset: 0x001A464E
	private void OnClusterDestinationChanged(object data)
	{
		this.RefreshPathDrawing();
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001A6458 File Offset: 0x001A4658
	public void Select(bool selected)
	{
		if (this.animControllers == null || this.animControllers.Count == 0)
		{
			return;
		}
		if (!selected == this.isSelected)
		{
			this.isSelected = selected;
			this.RefreshPathDrawing();
		}
		this.GetFirstAnimController().SetSymbolVisiblity("selected", selected);
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x001A64AA File Offset: 0x001A46AA
	public void PlayAnim(string animName, KAnim.PlayMode playMode)
	{
		if (this.animControllers.Count > 0)
		{
			this.GetFirstAnimController().Play(animName, playMode, 1f, 0f);
		}
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x001A64D6 File Offset: 0x001A46D6
	public KBatchedAnimController GetFirstAnimController()
	{
		return this.GetAnimController(0);
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x001A64DF File Offset: 0x001A46DF
	public KBatchedAnimController GetAnimController(int index)
	{
		if (index < this.animControllers.Count)
		{
			return this.animControllers[index];
		}
		return null;
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x001A64FD File Offset: 0x001A46FD
	public void ManualAddAnimController(KBatchedAnimController externalAnimController)
	{
		this.animControllers.Add(externalAnimController);
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x001A650C File Offset: 0x001A470C
	public void Show(ClusterRevealLevel level)
	{
		if (!this.entity.IsVisible)
		{
			level = ClusterRevealLevel.Hidden;
		}
		if (level == this.lastRevealLevel)
		{
			return;
		}
		this.lastRevealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			base.gameObject.SetActive(false);
			break;
		case ClusterRevealLevel.Peeked:
		{
			this.ClearAnimControllers();
			KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
			kbatchedAnimController.gameObject.SetActive(true);
			this.animControllers.Add(kbatchedAnimController);
			base.gameObject.SetActive(true);
			break;
		}
		case ClusterRevealLevel.Visible:
			this.ClearAnimControllers();
			if (this.animControllerPrefab != null && this.entity.AnimConfigs != null)
			{
				foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
				{
					KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
					kbatchedAnimController2.AnimFiles = new KAnimFile[]
					{
						animConfig.animFile
					};
					kbatchedAnimController2.initialMode = animConfig.playMode;
					kbatchedAnimController2.initialAnim = animConfig.initialAnim;
					kbatchedAnimController2.Offset = animConfig.animOffset;
					kbatchedAnimController2.gameObject.AddComponent<LoopingSounds>();
					if (animConfig.animPlaySpeedModifier != 0f)
					{
						kbatchedAnimController2.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
					}
					if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
					{
						SymbolOverrideController component = kbatchedAnimController2.GetComponent<SymbolOverrideController>();
						KAnim.Build.Symbol symbol = kbatchedAnimController2.AnimFiles[0].GetData().build.GetSymbol(animConfig.symbolSwapSymbol);
						component.AddSymbolOverride(animConfig.symbolSwapTarget, symbol, 0);
					}
					kbatchedAnimController2.gameObject.SetActive(true);
					this.animControllers.Add(kbatchedAnimController2);
				}
			}
			base.gameObject.SetActive(true);
			break;
		}
		this.entity.OnClusterMapIconShown(level);
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x001A670C File Offset: 0x001A490C
	public void RefreshPathDrawing()
	{
		if (this.entity == null)
		{
			return;
		}
		ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
		if (component == null)
		{
			return;
		}
		List<AxialI> list = (this.entity.IsVisible && component.IsTraveling()) ? component.CurrentPath : null;
		if (list != null && list.Count > 0)
		{
			if (this.mapPath == null)
			{
				this.mapPath = this.pathDrawer.AddPath();
			}
			this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(base.transform.GetLocalPosition(), list));
			Color color;
			if (this.isSelected)
			{
				color = ClusterMapScreen.Instance.rocketSelectedPathColor;
			}
			else if (this.entity.ShowPath())
			{
				color = ClusterMapScreen.Instance.rocketPathColor;
			}
			else
			{
				color = new Color(0f, 0f, 0f, 0f);
			}
			this.mapPath.SetColor(color);
			return;
		}
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
			this.mapPath = null;
		}
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x001A6826 File Offset: 0x001A4A26
	public void SetAnimRotation(float rotation)
	{
		this.animContainer.localRotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x001A6843 File Offset: 0x001A4A43
	public float GetPathAngle()
	{
		if (this.mapPath == null)
		{
			return 0f;
		}
		return this.mapPath.GetRotationForNextSegment();
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x001A6864 File Offset: 0x001A4A64
	private void ClearAnimControllers()
	{
		if (this.animControllers == null)
		{
			return;
		}
		foreach (KBatchedAnimController kbatchedAnimController in this.animControllers)
		{
			global::Util.KDestroyGameObject(kbatchedAnimController.gameObject);
		}
		this.animControllers.Clear();
	}

	// Token: 0x04003129 RID: 12585
	public KBatchedAnimController animControllerPrefab;

	// Token: 0x0400312A RID: 12586
	public KBatchedAnimController peekControllerPrefab;

	// Token: 0x0400312B RID: 12587
	public Transform nameTarget;

	// Token: 0x0400312C RID: 12588
	public AlertVignette alertVignette;

	// Token: 0x0400312D RID: 12589
	public bool doesTransitionAnimation;

	// Token: 0x0400312E RID: 12590
	[HideInInspector]
	public Transform animContainer;

	// Token: 0x0400312F RID: 12591
	private ClusterGridEntity entity;

	// Token: 0x04003130 RID: 12592
	private ClusterMapPathDrawer pathDrawer;

	// Token: 0x04003131 RID: 12593
	private ClusterMapPath mapPath;

	// Token: 0x04003132 RID: 12594
	private List<KBatchedAnimController> animControllers;

	// Token: 0x04003133 RID: 12595
	private bool isSelected;

	// Token: 0x04003134 RID: 12596
	private ClusterRevealLevel lastRevealLevel;

	// Token: 0x02001863 RID: 6243
	private class UpdateXPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060091AA RID: 37290 RVA: 0x00329FAE File Offset: 0x003281AE
		public UpdateXPositionParameter() : base("Starmap_Position_X")
		{
		}

		// Token: 0x060091AB RID: 37291 RVA: 0x00329FCC File Offset: 0x003281CC
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateXPositionParameter.Entry item = new ClusterMapVisualizer.UpdateXPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060091AC RID: 37292 RVA: 0x0032A024 File Offset: 0x00328224
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float)Screen.width, false);
				}
			}
		}

		// Token: 0x060091AD RID: 37293 RVA: 0x0032A0AC File Offset: 0x003282AC
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040071DC RID: 29148
		private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

		// Token: 0x020021FA RID: 8698
		private struct Entry
		{
			// Token: 0x04009815 RID: 38933
			public Transform transform;

			// Token: 0x04009816 RID: 38934
			public EventInstance ev;

			// Token: 0x04009817 RID: 38935
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001864 RID: 6244
	private class UpdateYPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060091AE RID: 37294 RVA: 0x0032A104 File Offset: 0x00328304
		public UpdateYPositionParameter() : base("Starmap_Position_Y")
		{
		}

		// Token: 0x060091AF RID: 37295 RVA: 0x0032A124 File Offset: 0x00328324
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateYPositionParameter.Entry item = new ClusterMapVisualizer.UpdateYPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060091B0 RID: 37296 RVA: 0x0032A17C File Offset: 0x0032837C
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float)Screen.height, false);
				}
			}
		}

		// Token: 0x060091B1 RID: 37297 RVA: 0x0032A204 File Offset: 0x00328404
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040071DD RID: 29149
		private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

		// Token: 0x020021FB RID: 8699
		private struct Entry
		{
			// Token: 0x04009818 RID: 38936
			public Transform transform;

			// Token: 0x04009819 RID: 38937
			public EventInstance ev;

			// Token: 0x0400981A RID: 38938
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001865 RID: 6245
	private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060091B2 RID: 37298 RVA: 0x0032A25C File Offset: 0x0032845C
		public UpdateZoomPercentageParameter() : base("Starmap_Zoom_Percentage")
		{
		}

		// Token: 0x060091B3 RID: 37299 RVA: 0x0032A27C File Offset: 0x0032847C
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry item = new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry
			{
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060091B4 RID: 37300 RVA: 0x0032A2C8 File Offset: 0x003284C8
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
			{
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			}
		}

		// Token: 0x060091B5 RID: 37301 RVA: 0x0032A334 File Offset: 0x00328534
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040071DE RID: 29150
		private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

		// Token: 0x020021FC RID: 8700
		private struct Entry
		{
			// Token: 0x0400981B RID: 38939
			public Transform transform;

			// Token: 0x0400981C RID: 38940
			public EventInstance ev;

			// Token: 0x0400981D RID: 38941
			public PARAMETER_ID parameterId;
		}
	}
}
