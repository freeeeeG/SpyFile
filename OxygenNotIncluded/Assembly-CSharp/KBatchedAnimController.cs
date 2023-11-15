using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200044C RID: 1100
[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
	// Token: 0x060017BE RID: 6078 RVA: 0x0007A502 File Offset: 0x00078702
	public int GetCurrentFrameIndex()
	{
		return this.curAnimFrameIdx;
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x0007A50A File Offset: 0x0007870A
	public KBatchedAnimInstanceData GetBatchInstanceData()
	{
		return this.batchInstanceData;
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060017C0 RID: 6080 RVA: 0x0007A512 File Offset: 0x00078712
	// (set) Token: 0x060017C1 RID: 6081 RVA: 0x0007A51A File Offset: 0x0007871A
	protected bool forceRebuild
	{
		get
		{
			return this._forceRebuild;
		}
		set
		{
			this._forceRebuild = value;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060017C2 RID: 6082 RVA: 0x0007A523 File Offset: 0x00078723
	public bool IsMoving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x0007A52C File Offset: 0x0007872C
	public KBatchedAnimController()
	{
		this.batchInstanceData = new KBatchedAnimInstanceData(this);
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x0007A5AA File Offset: 0x000787AA
	public bool IsActive()
	{
		return base.isActiveAndEnabled && this._enabled;
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x0007A5BC File Offset: 0x000787BC
	public bool IsVisible()
	{
		return this.isVisible;
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x0007A5C4 File Offset: 0x000787C4
	public Vector4 GetPositionData()
	{
		if (this.getPositionDataFunctionInUse != null)
		{
			return this.getPositionDataFunctionInUse();
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0007A618 File Offset: 0x00078818
	public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x0007A660 File Offset: 0x00078860
	public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x0007A6A8 File Offset: 0x000788A8
	public Vector2I GetCellXY()
	{
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		if (Grid.CellSizeInMeters == 0f)
		{
			return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);
		}
		return Grid.PosToXY(positionIncludingOffset);
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x0007A6E2 File Offset: 0x000788E2
	public float GetZ()
	{
		return base.transform.GetPosition().z;
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x0007A6F4 File Offset: 0x000788F4
	public string GetName()
	{
		return base.name;
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x0007A6FC File Offset: 0x000788FC
	public override KAnim.Anim GetAnim(int index)
	{
		if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
		{
			global::Debug.LogError(base.name + " batch not ready");
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
		global::Debug.Assert(batchGroupData != null);
		return batchGroupData.GetAnim(index);
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x0007A760 File Offset: 0x00078960
	private void Initialize()
	{
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.DeRegister();
			this.Register();
		}
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x0007A79B File Offset: 0x0007899B
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving == this.moving)
		{
			return;
		}
		this.moving = is_moving;
		this.SetDirty();
		this.ConfigureUpdateListener();
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x0007A7BA File Offset: 0x000789BA
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x0007A7C8 File Offset: 0x000789C8
	private void SetBatchGroup(KAnimFileData kafd)
	{
		if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag)
		{
			return;
		}
		DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
		DebugUtil.Assert(kafd != null, "Null anim data!! For", base.name);
		base.curBuild = kafd.build;
		DebugUtil.Assert(base.curBuild != null, "Null build for anim!! ", base.name, kafd.name);
		KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(base.curBuild.batchTag);
		HashedString batchGroupID = kafd.build.batchTag;
		if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
		{
			bool isValid = group.swapTarget.IsValid;
			string str = "Invalid swap target fro group [";
			HashedString id = group.id;
			global::Debug.Assert(isValid, str + id.ToString() + "]");
			batchGroupID = group.swapTarget;
		}
		this.batchGroupID = batchGroupID;
		base.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
		base.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
		if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
		{
			global::Debug.LogError("Batch is not ready: " + base.name);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x0007A964 File Offset: 0x00078B64
	public void LoadAnims()
	{
		if (!KAnimBatchManager.Instance().isReady)
		{
			global::Debug.LogError("KAnimBatchManager is not ready when loading anim:" + base.name);
		}
		if (this.animFiles.Length == 0)
		{
			DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + base.name);
		}
		if (!this.animFiles[0].IsBuildLoaded)
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", this.animFiles[0].GetData().name)
			});
		}
		this.overrideAnims.Clear();
		this.anims.Clear();
		this.SetBatchGroup(this.animFiles[0].GetData());
		for (int i = 0; i < this.animFiles.Length; i++)
		{
			base.AddAnims(this.animFiles[i]);
		}
		this.forceRebuild = true;
		if (this.layering != null)
		{
			this.layering.HideSymbols();
		}
		if (this.usingNewSymbolOverrideSystem)
		{
			DebugUtil.Assert(base.GetComponent<SymbolOverrideController>() != null);
		}
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x0007AA70 File Offset: 0x00078C70
	public void SwapAnims(KAnimFile[] anims)
	{
		if (this.batchGroupID.IsValid)
		{
			this.DeRegister();
			this.batchGroupID = HashedString.Invalid;
		}
		base.AnimFiles = anims;
		this.LoadAnims();
		if (base.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(this.hiddenSymbolsSet);
		}
		this.Register();
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x0007AAC8 File Offset: 0x00078CC8
	public void UpdateAnim(float dt)
	{
		if (this.batch != null && base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.batch != null && this.batch.group.maxGroupSize == 1 && this.lastPos.z != base.transform.GetPosition().z)
			{
				this.batch.OverrideZ(base.transform.GetPosition().z);
			}
			Vector3 positionIncludingOffset = base.PositionIncludingOffset;
			this.lastPos = positionIncludingOffset;
			if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY(this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
			{
				this.DeRegister();
				this.Register();
			}
			this.SetDirty();
		}
		if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
		{
			return;
		}
		if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Once && this.curAnim != null && (this.elapsedTime > this.curAnim.totalTime || this.curAnim.totalTime <= 0f) && this.animQueue.Count == 0)))
		{
			this.SuspendUpdates(true);
		}
		if (!this.isVisible && !this.forceRebuild)
		{
			if (this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate && !this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
			}
			return;
		}
		this.curAnimFrameIdx = base.GetFrameIdx(this.elapsedTime, true);
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			float elapsedTime = this.aem.GetElapsedTime(this.eventManagerHandle);
			if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0)
			{
				base.UpdateAnimEventSequenceTime();
			}
		}
		this.UpdateFrame(this.elapsedTime);
		if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
		{
			base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
		}
		this.forceRebuild = false;
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0007ACF0 File Offset: 0x00078EF0
	protected override void UpdateFrame(float t)
	{
		base.previousFrame = base.currentFrame;
		if (!this.stopped || this.forceRebuild)
		{
			if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || this.elapsedTime <= base.GetDuration() || this.forceRebuild))
			{
				base.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
				if (base.currentFrame != base.previousFrame || this.forceRebuild)
				{
					this.SetDirty();
				}
			}
			else
			{
				this.TriggerStop();
			}
			if (!this.stopped && this.mode == KAnim.PlayMode.Loop && base.currentFrame == 0)
			{
				base.AnimEnter(this.curAnim.hash);
			}
		}
		if (this.synchronizer != null)
		{
			this.synchronizer.SyncTime();
		}
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x0007ADC0 File Offset: 0x00078FC0
	public override void TriggerStop()
	{
		if (this.animQueue.Count > 0)
		{
			base.StartQueuedAnim();
			return;
		}
		if (this.curAnim != null && this.mode == KAnim.PlayMode.Once)
		{
			base.currentFrame = this.curAnim.numFrames - 1;
			base.Stop();
			base.gameObject.Trigger(-1061186183, null);
			if (this.destroyOnAnimComplete)
			{
				base.DestroySelf();
			}
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x0007AE2C File Offset: 0x0007902C
	public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x0007AEB0 File Offset: 0x000790B0
	public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x0007AF34 File Offset: 0x00079134
	public override void UpdateAllHiddenSymbols()
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
			bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
			base.symbolInstanceGpuData.SetVisible(i, is_visible);
		}
		this.SetDirty();
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x0007AF9D File Offset: 0x0007919D
	public int GetMaxVisible()
	{
		return this.maxSymbols;
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060017DA RID: 6106 RVA: 0x0007AFA5 File Offset: 0x000791A5
	// (set) Token: 0x060017DB RID: 6107 RVA: 0x0007AFAD File Offset: 0x000791AD
	public HashedString batchGroupID { get; private set; }

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060017DC RID: 6108 RVA: 0x0007AFB6 File Offset: 0x000791B6
	// (set) Token: 0x060017DD RID: 6109 RVA: 0x0007AFBE File Offset: 0x000791BE
	public HashedString batchGroupIDOverride { get; private set; }

	// Token: 0x060017DE RID: 6110 RVA: 0x0007AFC8 File Offset: 0x000791C8
	public HashedString GetBatchGroupID(bool isEditorWindow = false)
	{
		global::Debug.Assert(isEditorWindow || this.animFiles == null || this.animFiles.Length == 0 || (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
		return this.batchGroupID;
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x0007B01D File Offset: 0x0007921D
	public HashedString GetBatchGroupIDOverride()
	{
		return this.batchGroupIDOverride;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x0007B025 File Offset: 0x00079225
	public void SetBatchGroupOverride(HashedString id)
	{
		this.batchGroupIDOverride = id;
		this.DeRegister();
		this.Register();
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x0007B03A File Offset: 0x0007923A
	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0007B047 File Offset: 0x00079247
	public KAnimBatch GetBatch()
	{
		return this.batch;
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x0007B050 File Offset: 0x00079250
	public void SetBatch(KAnimBatch new_batch)
	{
		this.batch = new_batch;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			KBatchedAnimCanvasRenderer kbatchedAnimCanvasRenderer = base.GetComponent<KBatchedAnimCanvasRenderer>();
			if (kbatchedAnimCanvasRenderer == null && new_batch != null)
			{
				kbatchedAnimCanvasRenderer = base.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
			}
			if (kbatchedAnimCanvasRenderer != null)
			{
				kbatchedAnimCanvasRenderer.SetBatch(this);
			}
		}
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x0007B09C File Offset: 0x0007929C
	public int GetCurrentNumFrames()
	{
		if (this.curAnim == null)
		{
			return 0;
		}
		return this.curAnim.numFrames;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x0007B0B3 File Offset: 0x000792B3
	public int GetFirstFrameIndex()
	{
		if (this.curAnim == null)
		{
			return -1;
		}
		return this.curAnim.firstFrameIdx;
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x0007B0CC File Offset: 0x000792CC
	private Canvas GetRootCanvas()
	{
		if (this.rt == null)
		{
			return null;
		}
		RectTransform component = this.rt.parent.GetComponent<RectTransform>();
		while (component != null)
		{
			Canvas component2 = component.GetComponent<Canvas>();
			if (component2 != null && component2.isRootCanvas)
			{
				return component2;
			}
			component = component.parent.GetComponent<RectTransform>();
		}
		return null;
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x0007B130 File Offset: 0x00079330
	public override Matrix2x3 GetTransformMatrix()
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x0007B550 File Offset: 0x00079750
	public Matrix2x3 GetTransformMatrix(Vector2 customScale)
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = customScale;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x0007B950 File Offset: 0x00079B50
	public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		if (this.curAnimFrameIdx != -1 && this.batch != null)
		{
			Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
			if (symbolVisible)
			{
				return this.GetTransformMatrix() * symbolLocalTransform;
			}
		}
		symbolVisible = false;
		return default(Matrix4x4);
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x0007B9A0 File Offset: 0x00079BA0
	public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible)
	{
		KAnim.Anim.Frame frame;
		if (this.curAnimFrameIdx != -1 && this.batch != null && this.batch.group.data.TryGetFrame(this.curAnimFrameIdx, out frame))
		{
			for (int i = 0; i < frame.numElements; i++)
			{
				int num = frame.firstElementIdx + i;
				if (num < this.batch.group.data.frameElements.Count)
				{
					KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[num];
					if (frameElement.symbol == symbol)
					{
						symbolVisible = true;
						return frameElement.transform;
					}
				}
			}
		}
		symbolVisible = false;
		return Matrix2x3.identity;
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x0007BA56 File Offset: 0x00079C56
	public override void SetLayer(int layer)
	{
		if (layer == base.gameObject.layer)
		{
			return;
		}
		base.SetLayer(layer);
		this.DeRegister();
		base.gameObject.layer = layer;
		this.Register();
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x0007BA86 File Offset: 0x00079C86
	public override void SetDirty()
	{
		if (this.batch != null)
		{
			this.batch.SetDirty(this);
		}
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x0007BA9C File Offset: 0x00079C9C
	protected override void OnStartQueuedAnim()
	{
		this.SuspendUpdates(false);
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x0007BAA8 File Offset: 0x00079CA8
	protected override void OnAwake()
	{
		this.LoadAnims();
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
		{
			this.visibilityType = ((this.materialType == KAnimBatchGroup.MaterialType.UI) ? KAnimControllerBase.VisibilityType.Always : this.visibilityType);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.UpdateAllHiddenSymbols();
		this.hasEnableRun = false;
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x0007BB18 File Offset: 0x00079D18
	protected override void OnStart()
	{
		if (this.batch == null)
		{
			this.Initialize();
		}
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			this.ConfigureUpdateListener();
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
			this.moving = instance.IsMoving(base.transform);
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.SetDirty();
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x0007BB90 File Offset: 0x00079D90
	protected override void OnStop()
	{
		this.SetDirty();
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x0007BB98 File Offset: 0x00079D98
	private void OnEnable()
	{
		if (this._enabled)
		{
			this.Enable();
		}
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x0007BBA8 File Offset: 0x00079DA8
	protected override void Enable()
	{
		if (this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = true;
		if (this.batch == null)
		{
			this.Initialize();
		}
		this.SetDirty();
		this.SuspendUpdates(false);
		this.ConfigureVisibilityListener(true);
		if (!this.stopped && this.curAnim != null && this.mode != KAnim.PlayMode.Paused && !this.eventManagerHandle.IsValid())
		{
			base.StartAnimEventSequence();
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x0007BC13 File Offset: 0x00079E13
	private void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0007BC1C File Offset: 0x00079E1C
	protected override void Disable()
	{
		if (App.IsExiting || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = false;
		this.SuspendUpdates(true);
		if (this.batch != null)
		{
			this.DeRegister();
		}
		this.ConfigureVisibilityListener(false);
		base.StopAnimEventSequence();
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x0007BC6C File Offset: 0x00079E6C
	protected override void OnDestroy()
	{
		if (App.IsExiting)
		{
			return;
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
		}
		KBatchedAnimUpdater instance2 = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance2 != null)
		{
			instance2.UpdateUnregister(this);
		}
		this.isVisible = false;
		this.DeRegister();
		this.stopped = true;
		base.StopAnimEventSequence();
		this.batchInstanceData = null;
		this.batch = null;
		base.OnDestroy();
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0007BCE0 File Offset: 0x00079EE0
	public void SetBlendValue(float value)
	{
		this.batchInstanceData.SetBlend(value);
		this.SetDirty();
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x0007BCF4 File Offset: 0x00079EF4
	public SymbolOverrideController SetupSymbolOverriding()
	{
		if (!this.symbolOverrideController.IsNullOrDestroyed())
		{
			return this.symbolOverrideController;
		}
		this.usingNewSymbolOverrideSystem = true;
		this.symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(base.gameObject);
		return this.symbolOverrideController;
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0007BD28 File Offset: 0x00079F28
	public bool ApplySymbolOverrides()
	{
		this.batch.atlases.Apply(this.batch.matProperties);
		if (this.symbolOverrideController != null)
		{
			if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
			{
				this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
				this.symbolOverrideController.ApplyOverrides();
			}
			this.symbolOverrideController.ApplyAtlases();
			return true;
		}
		return false;
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x0007BDA8 File Offset: 0x00079FA8
	public void SetSymbolOverrides(int symbol_start_idx, int symbol_num_frames, int atlas_idx, KBatchGroupData source_data, int source_start_idx, int source_num_frames)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx, symbol_num_frames, atlas_idx, source_data, source_start_idx, source_num_frames);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x0007BDBE File Offset: 0x00079FBE
	public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x0007BDD0 File Offset: 0x00079FD0
	protected override void Register()
	{
		if (!this.IsActive())
		{
			return;
		}
		if (this.batch != null)
		{
			return;
		}
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
			KAnimBatchManager.Instance().Register(this);
			this.forceRebuild = true;
			this.SetDirty();
		}
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x0007BE35 File Offset: 0x0007A035
	protected override void DeRegister()
	{
		if (this.batch != null)
		{
			this.batch.Deregister(this);
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x0007BE4C File Offset: 0x0007A04C
	private void ConfigureUpdateListener()
	{
		if ((this.IsActive() && !this.suspendUpdates && this.isVisible) || this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
		{
			Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
			return;
		}
		Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x0007BEA7 File Offset: 0x0007A0A7
	protected override void SuspendUpdates(bool suspend)
	{
		this.suspendUpdates = suspend;
		this.ConfigureUpdateListener();
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0007BEB6 File Offset: 0x0007A0B6
	public void SetVisiblity(bool is_visible)
	{
		if (is_visible != this.isVisible)
		{
			this.isVisible = is_visible;
			if (is_visible)
			{
				this.SuspendUpdates(false);
				this.SetDirty();
				base.UpdateAnimEventSequenceTime();
				return;
			}
			this.SuspendUpdates(true);
			this.SetDirty();
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0007BEEC File Offset: 0x0007A0EC
	private void ConfigureVisibilityListener(bool enabled)
	{
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			return;
		}
		if (enabled)
		{
			this.RegisterVisibilityListener();
			return;
		}
		this.UnregisterVisibilityListener();
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0007BF11 File Offset: 0x0007A111
	protected override void RefreshVisibilityListener()
	{
		if (!this.visibilityListenerRegistered)
		{
			return;
		}
		this.ConfigureVisibilityListener(false);
		this.ConfigureVisibilityListener(true);
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0007BF2A File Offset: 0x0007A12A
	private void RegisterVisibilityListener()
	{
		DebugUtil.Assert(!this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
		this.visibilityListenerRegistered = true;
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x0007BF4C File Offset: 0x0007A14C
	private void UnregisterVisibilityListener()
	{
		DebugUtil.Assert(this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
		this.visibilityListenerRegistered = false;
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x0007BF6C File Offset: 0x0007A16C
	public void SetSceneLayer(Grid.SceneLayer layer)
	{
		float layerZ = Grid.GetLayerZ(layer);
		this.sceneLayer = layer;
		Vector3 position = base.transform.GetPosition();
		position.z = layerZ;
		base.transform.SetPosition(position);
		this.DeRegister();
		this.Register();
	}

	// Token: 0x04000D25 RID: 3365
	[NonSerialized]
	protected bool _forceRebuild;

	// Token: 0x04000D26 RID: 3366
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x04000D27 RID: 3367
	private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;

	// Token: 0x04000D28 RID: 3368
	private KAnimBatch batch;

	// Token: 0x04000D29 RID: 3369
	public float animScale = 0.005f;

	// Token: 0x04000D2A RID: 3370
	private bool suspendUpdates;

	// Token: 0x04000D2B RID: 3371
	private bool visibilityListenerRegistered;

	// Token: 0x04000D2C RID: 3372
	private bool moving;

	// Token: 0x04000D2D RID: 3373
	private SymbolOverrideController symbolOverrideController;

	// Token: 0x04000D2E RID: 3374
	private int symbolOverrideControllerVersion;

	// Token: 0x04000D2F RID: 3375
	[NonSerialized]
	public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;

	// Token: 0x04000D30 RID: 3376
	public Grid.SceneLayer sceneLayer;

	// Token: 0x04000D31 RID: 3377
	private RectTransform rt;

	// Token: 0x04000D32 RID: 3378
	private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04000D33 RID: 3379
	public Matrix2x3 navMatrix = Matrix2x3.identity;

	// Token: 0x04000D34 RID: 3380
	private CanvasScaler scaler;

	// Token: 0x04000D35 RID: 3381
	public bool setScaleFromAnim = true;

	// Token: 0x04000D36 RID: 3382
	public Vector2 animOverrideSize = Vector2.one;

	// Token: 0x04000D37 RID: 3383
	private Canvas rootCanvas;

	// Token: 0x04000D38 RID: 3384
	public bool isMovable;

	// Token: 0x04000D39 RID: 3385
	public Func<Vector4> getPositionDataFunctionInUse;
}
