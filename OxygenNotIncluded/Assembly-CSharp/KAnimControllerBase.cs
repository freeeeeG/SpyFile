using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x0600171D RID: 5917 RVA: 0x00077F18 File Offset: 0x00076118
	protected KAnimControllerBase()
	{
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.PlaySpeedMultiplier = 1f;
		this.synchronizer = new KAnimSynchronizer(this);
		this.layering = new KAnimLayering(this, this.fgLayer);
		this.isVisible = true;
	}

	// Token: 0x0600171E RID: 5918
	public abstract KAnim.Anim GetAnim(int index);

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600171F RID: 5919 RVA: 0x00078021 File Offset: 0x00076221
	// (set) Token: 0x06001720 RID: 5920 RVA: 0x00078029 File Offset: 0x00076229
	public string debugName { get; private set; }

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06001721 RID: 5921 RVA: 0x00078032 File Offset: 0x00076232
	// (set) Token: 0x06001722 RID: 5922 RVA: 0x0007803A File Offset: 0x0007623A
	public KAnim.Build curBuild { get; protected set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06001723 RID: 5923 RVA: 0x00078044 File Offset: 0x00076244
	// (remove) Token: 0x06001724 RID: 5924 RVA: 0x0007807C File Offset: 0x0007627C
	public event Action<Color32> OnOverlayColourChanged;

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06001725 RID: 5925 RVA: 0x000780B1 File Offset: 0x000762B1
	// (set) Token: 0x06001726 RID: 5926 RVA: 0x000780B9 File Offset: 0x000762B9
	public new bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			if (!this.hasAwakeRun)
			{
				return;
			}
			if (this._enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06001727 RID: 5927 RVA: 0x000780E0 File Offset: 0x000762E0
	public bool HasBatchInstanceData
	{
		get
		{
			return this.batchInstanceData != null;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06001728 RID: 5928 RVA: 0x000780EB File Offset: 0x000762EB
	// (set) Token: 0x06001729 RID: 5929 RVA: 0x000780F3 File Offset: 0x000762F3
	public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600172A RID: 5930 RVA: 0x000780FC File Offset: 0x000762FC
	// (set) Token: 0x0600172B RID: 5931 RVA: 0x00078104 File Offset: 0x00076304
	public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x0600172C RID: 5932 RVA: 0x0007810D File Offset: 0x0007630D
	// (set) Token: 0x0600172D RID: 5933 RVA: 0x00078120 File Offset: 0x00076320
	public Color32 TintColour
	{
		get
		{
			return this.batchInstanceData.GetTintColour();
		}
		set
		{
			if (this.batchInstanceData != null && this.batchInstanceData.SetTintColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnTintChanged != null)
				{
					this.OnTintChanged(value);
				}
			}
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600172E RID: 5934 RVA: 0x0007816E File Offset: 0x0007636E
	// (set) Token: 0x0600172F RID: 5935 RVA: 0x00078180 File Offset: 0x00076380
	public Color32 HighlightColour
	{
		get
		{
			return this.batchInstanceData.GetHighlightcolour();
		}
		set
		{
			if (this.batchInstanceData.SetHighlightColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnHighlightChanged != null)
				{
					this.OnHighlightChanged(value);
				}
			}
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06001730 RID: 5936 RVA: 0x000781BB File Offset: 0x000763BB
	// (set) Token: 0x06001731 RID: 5937 RVA: 0x000781C8 File Offset: 0x000763C8
	public Color OverlayColour
	{
		get
		{
			return this.batchInstanceData.GetOverlayColour();
		}
		set
		{
			if (this.batchInstanceData.SetOverlayColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnOverlayColourChanged != null)
				{
					this.OnOverlayColourChanged(value);
				}
			}
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06001732 RID: 5938 RVA: 0x00078200 File Offset: 0x00076400
	// (remove) Token: 0x06001733 RID: 5939 RVA: 0x00078238 File Offset: 0x00076438
	public event KAnimControllerBase.KAnimEvent onAnimEnter;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06001734 RID: 5940 RVA: 0x00078270 File Offset: 0x00076470
	// (remove) Token: 0x06001735 RID: 5941 RVA: 0x000782A8 File Offset: 0x000764A8
	public event KAnimControllerBase.KAnimEvent onAnimComplete;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001736 RID: 5942 RVA: 0x000782E0 File Offset: 0x000764E0
	// (remove) Token: 0x06001737 RID: 5943 RVA: 0x00078318 File Offset: 0x00076518
	public event Action<int> onLayerChanged;

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06001738 RID: 5944 RVA: 0x0007834D File Offset: 0x0007654D
	// (set) Token: 0x06001739 RID: 5945 RVA: 0x00078355 File Offset: 0x00076555
	public int previousFrame { get; protected set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x0600173A RID: 5946 RVA: 0x0007835E File Offset: 0x0007655E
	// (set) Token: 0x0600173B RID: 5947 RVA: 0x00078366 File Offset: 0x00076566
	public int currentFrame { get; protected set; }

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600173C RID: 5948 RVA: 0x00078370 File Offset: 0x00076570
	public HashedString currentAnim
	{
		get
		{
			if (this.curAnim == null)
			{
				return default(HashedString);
			}
			return this.curAnim.hash;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600173E RID: 5950 RVA: 0x000783A3 File Offset: 0x000765A3
	// (set) Token: 0x0600173D RID: 5949 RVA: 0x0007839A File Offset: 0x0007659A
	public float PlaySpeedMultiplier { get; set; }

	// Token: 0x0600173F RID: 5951 RVA: 0x000783AB File Offset: 0x000765AB
	public void SetFGLayer(Grid.SceneLayer layer)
	{
		this.fgLayer = layer;
		this.GetLayering();
		if (this.layering != null)
		{
			this.layering.SetLayer(this.fgLayer);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06001740 RID: 5952 RVA: 0x000783D4 File Offset: 0x000765D4
	// (set) Token: 0x06001741 RID: 5953 RVA: 0x000783DC File Offset: 0x000765DC
	public KAnim.PlayMode PlayMode
	{
		get
		{
			return this.mode;
		}
		set
		{
			this.mode = value;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06001742 RID: 5954 RVA: 0x000783E5 File Offset: 0x000765E5
	// (set) Token: 0x06001743 RID: 5955 RVA: 0x000783ED File Offset: 0x000765ED
	public bool FlipX
	{
		get
		{
			return this.flipX;
		}
		set
		{
			this.flipX = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06001744 RID: 5956 RVA: 0x0007840F File Offset: 0x0007660F
	// (set) Token: 0x06001745 RID: 5957 RVA: 0x00078417 File Offset: 0x00076617
	public bool FlipY
	{
		get
		{
			return this.flipY;
		}
		set
		{
			this.flipY = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06001746 RID: 5958 RVA: 0x00078439 File Offset: 0x00076639
	// (set) Token: 0x06001747 RID: 5959 RVA: 0x00078441 File Offset: 0x00076641
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.DeRegister();
			this.Register();
			this.RefreshVisibilityListener();
			this.SetDirty();
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06001748 RID: 5960 RVA: 0x00078475 File Offset: 0x00076675
	// (set) Token: 0x06001749 RID: 5961 RVA: 0x0007847D File Offset: 0x0007667D
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600174A RID: 5962 RVA: 0x0007849F File Offset: 0x0007669F
	// (set) Token: 0x0600174B RID: 5963 RVA: 0x000784A7 File Offset: 0x000766A7
	public Vector3 Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600174C RID: 5964 RVA: 0x000784C9 File Offset: 0x000766C9
	public Vector3 PositionIncludingOffset
	{
		get
		{
			return base.transform.GetPosition() + this.Offset;
		}
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x000784E1 File Offset: 0x000766E1
	public KAnimBatchGroup.MaterialType GetMaterialType()
	{
		return this.materialType;
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000784EC File Offset: 0x000766EC
	public Vector3 GetWorldPivot()
	{
		Vector3 position = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			position.x += component.offset.x;
			position.y += component.offset.y - component.size.y / 2f;
		}
		return position;
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x00078554 File Offset: 0x00076754
	public KAnim.Anim GetCurrentAnim()
	{
		return this.curAnim;
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x0007855C File Offset: 0x0007675C
	public KAnimHashedString GetBuildHash()
	{
		if (this.curBuild == null)
		{
			return KAnimBatchManager.NO_BATCH;
		}
		return this.curBuild.fileHash;
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x0007857C File Offset: 0x0007677C
	protected float GetDuration()
	{
		if (this.curAnim != null)
		{
			return (float)this.curAnim.numFrames / this.curAnim.frameRate;
		}
		return 0f;
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000785A4 File Offset: 0x000767A4
	protected int GetFrameIdxFromOffset(int offset)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = offset + this.curAnim.firstFrameIdx;
		}
		return result;
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x000785CC File Offset: 0x000767CC
	public int GetFrameIdx(float time, bool absolute)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
		}
		return result;
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x00078609 File Offset: 0x00076809
	public bool IsStopped()
	{
		return this.stopped;
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06001755 RID: 5973 RVA: 0x00078611 File Offset: 0x00076811
	public KAnim.Anim CurrentAnim
	{
		get
		{
			return this.curAnim;
		}
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x00078619 File Offset: 0x00076819
	public KAnimSynchronizer GetSynchronizer()
	{
		return this.synchronizer;
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x00078621 File Offset: 0x00076821
	public KAnimLayering GetLayering()
	{
		if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
		{
			this.layering = new KAnimLayering(this, this.fgLayer);
		}
		return this.layering;
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0007864D File Offset: 0x0007684D
	public KAnim.PlayMode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x00078655 File Offset: 0x00076855
	public static string GetModeString(KAnim.PlayMode mode)
	{
		switch (mode)
		{
		case KAnim.PlayMode.Loop:
			return "Loop";
		case KAnim.PlayMode.Once:
			return "Once";
		case KAnim.PlayMode.Paused:
			return "Paused";
		default:
			return "Unknown";
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x00078682 File Offset: 0x00076882
	public float GetPlaySpeed()
	{
		return this.playSpeed;
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0007868A File Offset: 0x0007688A
	public void SetElapsedTime(float value)
	{
		this.elapsedTime = value;
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x00078693 File Offset: 0x00076893
	public float GetElapsedTime()
	{
		return this.elapsedTime;
	}

	// Token: 0x0600175D RID: 5981
	protected abstract void SuspendUpdates(bool suspend);

	// Token: 0x0600175E RID: 5982
	protected abstract void OnStartQueuedAnim();

	// Token: 0x0600175F RID: 5983
	public abstract void SetDirty();

	// Token: 0x06001760 RID: 5984
	protected abstract void RefreshVisibilityListener();

	// Token: 0x06001761 RID: 5985
	protected abstract void DeRegister();

	// Token: 0x06001762 RID: 5986
	protected abstract void Register();

	// Token: 0x06001763 RID: 5987
	protected abstract void OnAwake();

	// Token: 0x06001764 RID: 5988
	protected abstract void OnStart();

	// Token: 0x06001765 RID: 5989
	protected abstract void OnStop();

	// Token: 0x06001766 RID: 5990
	protected abstract void Enable();

	// Token: 0x06001767 RID: 5991
	protected abstract void Disable();

	// Token: 0x06001768 RID: 5992
	protected abstract void UpdateFrame(float t);

	// Token: 0x06001769 RID: 5993
	public abstract Matrix2x3 GetTransformMatrix();

	// Token: 0x0600176A RID: 5994
	public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

	// Token: 0x0600176B RID: 5995
	public abstract void UpdateAllHiddenSymbols();

	// Token: 0x0600176C RID: 5996
	public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

	// Token: 0x0600176D RID: 5997
	public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

	// Token: 0x0600176E RID: 5998
	public abstract void TriggerStop();

	// Token: 0x0600176F RID: 5999 RVA: 0x0007869B File Offset: 0x0007689B
	public virtual void SetLayer(int layer)
	{
		if (this.onLayerChanged != null)
		{
			this.onLayerChanged(layer);
		}
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x000786B4 File Offset: 0x000768B4
	public Vector3 GetPivotSymbolPosition()
	{
		bool flag = false;
		Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out flag);
		Vector3 position = base.transform.GetPosition();
		if (flag)
		{
			position = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
		}
		return position;
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x00078703 File Offset: 0x00076903
	public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		symbolVisible = false;
		return Matrix4x4.identity;
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x00078710 File Offset: 0x00076910
	private void Awake()
	{
		this.aem = Singleton<AnimEventManager>.Instance;
		this.debugName = base.name;
		this.SetFGLayer(this.fgLayer);
		this.OnAwake();
		if (!string.IsNullOrEmpty(this.initialAnim))
		{
			this.SetDirty();
			this.Play(this.initialAnim, this.initialMode, 1f, 0f);
		}
		this.hasAwakeRun = true;
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x00078781 File Offset: 0x00076981
	private void Start()
	{
		this.OnStart();
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x0007878C File Offset: 0x0007698C
	protected virtual void OnDestroy()
	{
		this.animFiles = null;
		this.curAnim = null;
		this.curBuild = null;
		this.synchronizer = null;
		this.layering = null;
		this.animQueue = null;
		this.overrideAnims = null;
		this.anims = null;
		this.synchronizer = null;
		this.layering = null;
		this.overrideAnimFiles = null;
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x000787E6 File Offset: 0x000769E6
	protected void AnimEnter(HashedString hashed_name)
	{
		if (this.onAnimEnter != null)
		{
			this.onAnimEnter(hashed_name);
		}
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x000787FC File Offset: 0x000769FC
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x00078818 File Offset: 0x00076A18
	public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		for (int i = 0; i < anim_names.Length - 1; i++)
		{
			this.Queue(anim_names[i], KAnim.PlayMode.Once, 1f, 0f);
		}
		global::Debug.Assert(anim_names.Length != 0, "Play was called with an empty anim array");
		this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0f);
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x00078888 File Offset: 0x00076A88
	public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.animQueue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		this.mode = ((mode == KAnim.PlayMode.Paused) ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once);
		if (this.aem != null)
		{
			this.aem.SetMode(this.eventManagerHandle, this.mode);
		}
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x00078913 File Offset: 0x00076B13
	public void QueueAndSyncTransition(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.SyncTransition();
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x00078926 File Offset: 0x00076B26
	public void SyncTransition()
	{
		this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x00078945 File Offset: 0x00076B45
	public void ClearQueue()
	{
		this.animQueue.Clear();
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x00078954 File Offset: 0x00076B54
	private void Restart(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.curBuild == null)
		{
			string[] array = new string[5];
			array[0] = "[";
			array[1] = base.gameObject.name;
			array[2] = "] Missing build while trying to play anim [";
			int num = 3;
			HashedString hashedString = anim_name;
			array[num] = hashedString.ToString();
			array[4] = "]";
			global::Debug.LogWarning(string.Concat(array), base.gameObject);
			return;
		}
		Queue<KAnimControllerBase.AnimData> queue = new Queue<KAnimControllerBase.AnimData>();
		queue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		while (this.animQueue.Count > 0)
		{
			queue.Enqueue(this.animQueue.Dequeue());
		}
		this.animQueue = queue;
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x00078A34 File Offset: 0x00076C34
	protected void StartQueuedAnim()
	{
		this.StopAnimEventSequence();
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.SuspendUpdates(false);
		this.stopped = false;
		this.OnStartQueuedAnim();
		KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
		while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
		{
			animData = this.animQueue.Dequeue();
		}
		KAnimControllerBase.AnimLookupData animLookupData;
		if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
		{
			if (!this.anims.TryGetValue(animData.anim, out animLookupData))
			{
				bool flag = true;
				if (this.showWhenMissing != null)
				{
					this.showWhenMissing.SetActive(true);
				}
				if (flag)
				{
					this.TriggerStop();
					return;
				}
			}
			else if (this.showWhenMissing != null)
			{
				this.showWhenMissing.SetActive(false);
			}
		}
		this.curAnim = this.GetAnim(animLookupData.animIndex);
		int num = 0;
		if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
		{
			num = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
		}
		this.prevAnimFrame = -1;
		this.curAnimFrameIdx = this.GetFrameIdxFromOffset(num);
		this.currentFrame = this.curAnimFrameIdx;
		this.mode = animData.mode;
		this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
		this.SetElapsedTime((float)num / this.curAnim.frameRate + animData.timeOffset);
		this.synchronizer.Sync();
		this.StartAnimEventSequence();
		this.AnimEnter(animData.anim);
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x00078BB8 File Offset: 0x00076DB8
	public bool GetSymbolVisiblity(KAnimHashedString symbol)
	{
		return !this.hiddenSymbolsSet.Contains(symbol);
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x00078BC9 File Offset: 0x00076DC9
	public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
	{
		if (is_visible)
		{
			this.hiddenSymbolsSet.Remove(symbol);
		}
		else if (!this.hiddenSymbolsSet.Contains(symbol))
		{
			this.hiddenSymbolsSet.Add(symbol);
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbol(symbol);
		}
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x00078C08 File Offset: 0x00076E08
	public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
	{
		foreach (KAnimHashedString item in symbols)
		{
			if (is_visible)
			{
				this.hiddenSymbolsSet.Remove(item);
			}
			else if (!this.hiddenSymbolsSet.Contains(item))
			{
				this.hiddenSymbolsSet.Add(item);
			}
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(symbols);
		}
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x00078C8C File Offset: 0x00076E8C
	public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0f)
	{
		global::Debug.Assert(kanim_file != null);
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.AddBuildOverride(kanim_file.GetData(), 0);
		}
		this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData
		{
			priority = priority,
			file = kanim_file
		});
		this.overrideAnimFiles.Sort((KAnimControllerBase.OverrideAnimFileData a, KAnimControllerBase.OverrideAnimFileData b) => b.priority.CompareTo(a.priority));
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x00078D40 File Offset: 0x00076F40
	public void RemoveAnimOverrides(KAnimFile kanim_file)
	{
		global::Debug.Assert(kanim_file != null);
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
		}
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			if (this.overrideAnimFiles[i].file == kanim_file)
			{
				this.overrideAnimFiles.RemoveAt(i);
				break;
			}
		}
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x00078DE0 File Offset: 0x00076FE0
	private void RebuildOverrides(KAnimFile kanim_file)
	{
		bool flag = false;
		this.overrideAnims.Clear();
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			KAnimControllerBase.OverrideAnimFileData overrideAnimFileData = this.overrideAnimFiles[i];
			KAnimFileData data = overrideAnimFileData.file.GetData();
			for (int j = 0; j < data.animCount; j++)
			{
				KAnim.Anim anim = data.GetAnim(j);
				if (anim.animFile.hashName != data.hashName)
				{
					global::Debug.LogError(string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", data.name, anim.animFile.name, j));
				}
				KAnimControllerBase.AnimLookupData value = default(KAnimControllerBase.AnimLookupData);
				value.animIndex = anim.index;
				HashedString hashedString = new HashedString(anim.name);
				if (!this.overrideAnims.ContainsKey(hashedString))
				{
					this.overrideAnims[hashedString] = value;
				}
				if (this.curAnim != null && this.curAnim.hash == hashedString && overrideAnimFileData.file == kanim_file)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Restart(this.curAnim.name, this.mode, this.playSpeed, 0f);
		}
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x00078F30 File Offset: 0x00077130
	public bool HasAnimation(HashedString anim_name)
	{
		bool flag = anim_name.IsValid;
		if (flag)
		{
			bool flag2 = this.anims.ContainsKey(anim_name);
			bool flag3 = !flag2 && this.overrideAnims.ContainsKey(anim_name);
			flag = (flag2 || flag3);
		}
		return flag;
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x00078F6C File Offset: 0x0007716C
	public bool HasAnimationFile(KAnimHashedString anim_file_name)
	{
		KAnimFile kanimFile = null;
		return this.TryGetAnimationFile(anim_file_name, out kanimFile);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x00078F84 File Offset: 0x00077184
	public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
	{
		match = null;
		if (!anim_file_name.IsValid())
		{
			return false;
		}
		KAnimFileData kanimFileData = null;
		int num = 0;
		int num2 = this.overrideAnimFiles.Count - 1;
		int num3 = (int)((float)this.overrideAnimFiles.Count * 0.5f);
		while (num3 > 0 && match == null && num < num3)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
				break;
			}
			if (this.overrideAnimFiles[num2].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num2].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num2].file;
			}
			num++;
			num2--;
		}
		if (match == null && this.overrideAnimFiles.Count % 2 != 0)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
			}
		}
		kanimFileData = null;
		if (match == null && this.animFiles != null)
		{
			num = 0;
			num2 = this.animFiles.Length - 1;
			num3 = (int)((float)this.animFiles.Length * 0.5f);
			while (num3 > 0 && match == null && num < num3)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
					break;
				}
				if (this.animFiles[num2] != null)
				{
					kanimFileData = this.animFiles[num2].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num2];
				}
				num++;
				num2--;
			}
			if (match == null && this.animFiles.Length % 2 != 0)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
				}
			}
		}
		return match != null;
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x00079260 File Offset: 0x00077460
	public void AddAnims(KAnimFile anim_file)
	{
		KAnimFileData data = anim_file.GetData();
		if (data == null)
		{
			global::Debug.LogError("AddAnims() Null animfile data");
			return;
		}
		this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.animFile.hashName != data.hashName)
			{
				global::Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", new object[]
				{
					data.name,
					anim.animFile.name,
					i
				});
			}
			this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData
			{
				animIndex = anim.index
			};
		}
		if (this.usingNewSymbolOverrideSystem && data.buildIndex != -1 && data.build.symbols != null && data.build.symbols.Length != 0)
		{
			base.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06001788 RID: 6024 RVA: 0x00079362 File Offset: 0x00077562
	// (set) Token: 0x06001789 RID: 6025 RVA: 0x0007936C File Offset: 0x0007756C
	public KAnimFile[] AnimFiles
	{
		get
		{
			return this.animFiles;
		}
		set
		{
			DebugUtil.AssertArgs(value.Length != 0, new object[]
			{
				"Controller has no anim files.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0] != null, new object[]
			{
				"First anim file needs to be non-null.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0].IsBuildLoaded, new object[]
			{
				"First anim file needs to be the build file.",
				base.gameObject
			});
			for (int i = 0; i < value.Length; i++)
			{
				DebugUtil.AssertArgs(value[i] != null, new object[]
				{
					"Anim file is null",
					base.gameObject
				});
			}
			this.animFiles = new KAnimFile[value.Length];
			for (int j = 0; j < value.Length; j++)
			{
				this.animFiles[j] = value[j];
			}
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600178A RID: 6026 RVA: 0x0007943D File Offset: 0x0007763D
	public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
	{
		get
		{
			return this.overrideAnimFiles;
		}
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x00079448 File Offset: 0x00077648
	public void Stop()
	{
		if (this.curAnim != null)
		{
			this.StopAnimEventSequence();
		}
		this.animQueue.Clear();
		this.stopped = true;
		if (this.onAnimComplete != null)
		{
			this.onAnimComplete((this.curAnim == null) ? HashedString.Invalid : this.curAnim.hash);
		}
		this.OnStop();
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000794A8 File Offset: 0x000776A8
	public void StopAndClear()
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.bounds.center = Vector3.zero;
		this.bounds.extents = Vector3.zero;
		if (this.OnUpdateBounds != null)
		{
			this.OnUpdateBounds(this.bounds);
		}
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000794FC File Offset: 0x000776FC
	public float GetPositionPercent()
	{
		return this.GetElapsedTime() / this.GetDuration();
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x0007950C File Offset: 0x0007770C
	public void SetPositionPercent(float percent)
	{
		if (this.curAnim == null)
		{
			return;
		}
		this.SetElapsedTime((float)this.curAnim.numFrames / this.curAnim.frameRate * percent);
		int frameIdx = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
		if (this.currentFrame != frameIdx)
		{
			this.SetDirty();
			this.UpdateAnimEventSequenceTime();
			this.SuspendUpdates(false);
		}
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x00079578 File Offset: 0x00077778
	protected void StartAnimEventSequence()
	{
		if (!this.layering.GetIsForeground() && this.aem != null)
		{
			this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
		}
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x000795C7 File Offset: 0x000777C7
	protected void UpdateAnimEventSequenceTime()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
		}
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x000795F8 File Offset: 0x000777F8
	protected void StopAnimEventSequence()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
			}
			this.aem.StopAnim(this.eventManagerHandle);
			this.eventManagerHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x0007965E File Offset: 0x0007785E
	protected void DestroySelf()
	{
		if (this.onDestroySelf != null)
		{
			this.onDestroySelf(base.gameObject);
			return;
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x00079685 File Offset: 0x00077885
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.hiddenSymbols.Clear();
		this.hiddenSymbols = new List<KAnimHashedString>(this.hiddenSymbolsSet);
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x000796A3 File Offset: 0x000778A3
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.hiddenSymbolsSet = new HashSet<KAnimHashedString>(this.hiddenSymbols);
		this.hiddenSymbols.Clear();
	}

	// Token: 0x04000CD3 RID: 3283
	[NonSerialized]
	public GameObject showWhenMissing;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	public KAnimBatchGroup.MaterialType materialType;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	public string initialAnim;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	protected KAnimFile[] animFiles = new KAnimFile[0];

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	protected Vector3 offset;

	// Token: 0x04000CD9 RID: 3289
	[SerializeField]
	protected Vector3 pivot;

	// Token: 0x04000CDA RID: 3290
	[SerializeField]
	protected float rotation;

	// Token: 0x04000CDB RID: 3291
	[SerializeField]
	public bool destroyOnAnimComplete;

	// Token: 0x04000CDC RID: 3292
	[SerializeField]
	public bool inactiveDisable;

	// Token: 0x04000CDD RID: 3293
	[SerializeField]
	protected bool flipX;

	// Token: 0x04000CDE RID: 3294
	[SerializeField]
	protected bool flipY;

	// Token: 0x04000CDF RID: 3295
	[SerializeField]
	public bool forceUseGameTime;

	// Token: 0x04000CE0 RID: 3296
	public string defaultAnim;

	// Token: 0x04000CE2 RID: 3298
	protected KAnim.Anim curAnim;

	// Token: 0x04000CE3 RID: 3299
	protected int curAnimFrameIdx = -1;

	// Token: 0x04000CE4 RID: 3300
	protected int prevAnimFrame = -1;

	// Token: 0x04000CE5 RID: 3301
	public bool usingNewSymbolOverrideSystem;

	// Token: 0x04000CE7 RID: 3303
	protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04000CE8 RID: 3304
	protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();

	// Token: 0x04000CE9 RID: 3305
	protected DeepProfiler DeepProfiler = new DeepProfiler(false);

	// Token: 0x04000CEA RID: 3306
	public bool randomiseLoopedOffset;

	// Token: 0x04000CEB RID: 3307
	protected float elapsedTime;

	// Token: 0x04000CEC RID: 3308
	protected float playSpeed = 1f;

	// Token: 0x04000CED RID: 3309
	protected KAnim.PlayMode mode = KAnim.PlayMode.Once;

	// Token: 0x04000CEE RID: 3310
	protected bool stopped = true;

	// Token: 0x04000CEF RID: 3311
	public float animHeight = 1f;

	// Token: 0x04000CF0 RID: 3312
	public float animWidth = 1f;

	// Token: 0x04000CF1 RID: 3313
	protected bool isVisible;

	// Token: 0x04000CF2 RID: 3314
	protected Bounds bounds;

	// Token: 0x04000CF3 RID: 3315
	public Action<Bounds> OnUpdateBounds;

	// Token: 0x04000CF4 RID: 3316
	public Action<Color> OnTintChanged;

	// Token: 0x04000CF5 RID: 3317
	public Action<Color> OnHighlightChanged;

	// Token: 0x04000CF7 RID: 3319
	protected KAnimSynchronizer synchronizer;

	// Token: 0x04000CF8 RID: 3320
	protected KAnimLayering layering;

	// Token: 0x04000CF9 RID: 3321
	[SerializeField]
	protected bool _enabled = true;

	// Token: 0x04000CFA RID: 3322
	protected bool hasEnableRun;

	// Token: 0x04000CFB RID: 3323
	protected bool hasAwakeRun;

	// Token: 0x04000CFC RID: 3324
	protected KBatchedAnimInstanceData batchInstanceData;

	// Token: 0x04000CFF RID: 3327
	public KAnimControllerBase.VisibilityType visibilityType;

	// Token: 0x04000D03 RID: 3331
	public Action<GameObject> onDestroySelf;

	// Token: 0x04000D06 RID: 3334
	[SerializeField]
	protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();

	// Token: 0x04000D07 RID: 3335
	[SerializeField]
	protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();

	// Token: 0x04000D08 RID: 3336
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04000D09 RID: 3337
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04000D0A RID: 3338
	protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();

	// Token: 0x04000D0B RID: 3339
	protected int maxSymbols;

	// Token: 0x04000D0D RID: 3341
	public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;

	// Token: 0x04000D0E RID: 3342
	protected AnimEventManager aem;

	// Token: 0x04000D0F RID: 3343
	private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

	// Token: 0x020010D0 RID: 4304
	public struct OverrideAnimFileData
	{
		// Token: 0x04005A2B RID: 23083
		public float priority;

		// Token: 0x04005A2C RID: 23084
		public KAnimFile file;
	}

	// Token: 0x020010D1 RID: 4305
	public struct AnimLookupData
	{
		// Token: 0x04005A2D RID: 23085
		public int animIndex;
	}

	// Token: 0x020010D2 RID: 4306
	public struct AnimData
	{
		// Token: 0x04005A2E RID: 23086
		public HashedString anim;

		// Token: 0x04005A2F RID: 23087
		public KAnim.PlayMode mode;

		// Token: 0x04005A30 RID: 23088
		public float speed;

		// Token: 0x04005A31 RID: 23089
		public float timeOffset;
	}

	// Token: 0x020010D3 RID: 4307
	public enum VisibilityType
	{
		// Token: 0x04005A33 RID: 23091
		Default,
		// Token: 0x04005A34 RID: 23092
		OffscreenUpdate,
		// Token: 0x04005A35 RID: 23093
		Always
	}

	// Token: 0x020010D4 RID: 4308
	// (Invoke) Token: 0x0600778E RID: 30606
	public delegate void KAnimEvent(HashedString name);
}
