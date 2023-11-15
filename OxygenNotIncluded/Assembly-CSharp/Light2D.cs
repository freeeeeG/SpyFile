using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000910 RID: 2320
[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600433C RID: 17212 RVA: 0x00178610 File Offset: 0x00176810
	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x0600433D RID: 17213 RVA: 0x00178626 File Offset: 0x00176826
	// (set) Token: 0x0600433E RID: 17214 RVA: 0x00178633 File Offset: 0x00176833
	public global::LightShape shape
	{
		get
		{
			return this.pending_emitter_state.shape;
		}
		set
		{
			this.pending_emitter_state.shape = this.MaybeDirty<global::LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x0600433F RID: 17215 RVA: 0x00178658 File Offset: 0x00176858
	// (set) Token: 0x06004340 RID: 17216 RVA: 0x00178660 File Offset: 0x00176860
	public LightGridManager.LightGridEmitter emitter { get; private set; }

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06004341 RID: 17217 RVA: 0x00178669 File Offset: 0x00176869
	// (set) Token: 0x06004342 RID: 17218 RVA: 0x00178676 File Offset: 0x00176876
	public Color Color
	{
		get
		{
			return this.pending_emitter_state.colour;
		}
		set
		{
			this.pending_emitter_state.colour = value;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06004343 RID: 17219 RVA: 0x00178684 File Offset: 0x00176884
	// (set) Token: 0x06004344 RID: 17220 RVA: 0x00178691 File Offset: 0x00176891
	public int Lux
	{
		get
		{
			return this.pending_emitter_state.intensity;
		}
		set
		{
			this.pending_emitter_state.intensity = value;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06004345 RID: 17221 RVA: 0x0017869F File Offset: 0x0017689F
	// (set) Token: 0x06004346 RID: 17222 RVA: 0x001786AC File Offset: 0x001768AC
	public float Range
	{
		get
		{
			return this.pending_emitter_state.radius;
		}
		set
		{
			this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06004347 RID: 17223 RVA: 0x001786D1 File Offset: 0x001768D1
	// (set) Token: 0x06004348 RID: 17224 RVA: 0x001786DE File Offset: 0x001768DE
	private int origin
	{
		get
		{
			return this.pending_emitter_state.origin;
		}
		set
		{
			this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06004349 RID: 17225 RVA: 0x00178703 File Offset: 0x00176903
	// (set) Token: 0x0600434A RID: 17226 RVA: 0x00178710 File Offset: 0x00176910
	public float FalloffRate
	{
		get
		{
			return this.pending_emitter_state.falloffRate;
		}
		set
		{
			this.pending_emitter_state.falloffRate = this.MaybeDirty<float>(this.pending_emitter_state.falloffRate, value, ref this.dirty_falloff);
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x0600434B RID: 17227 RVA: 0x00178735 File Offset: 0x00176935
	// (set) Token: 0x0600434C RID: 17228 RVA: 0x0017873D File Offset: 0x0017693D
	public float IntensityAnimation { get; set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x0600434D RID: 17229 RVA: 0x00178746 File Offset: 0x00176946
	// (set) Token: 0x0600434E RID: 17230 RVA: 0x0017874E File Offset: 0x0017694E
	public Vector2 Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			if (this._offset != value)
			{
				this._offset = value;
				this.origin = Grid.PosToCell(base.transform.GetPosition() + this._offset);
			}
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x0600434F RID: 17231 RVA: 0x0017878B File Offset: 0x0017698B
	private bool isRegistered
	{
		get
		{
			return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x001787A0 File Offset: 0x001769A0
	public Light2D()
	{
		this.emitter = new LightGridManager.LightGridEmitter();
		this.Range = 5f;
		this.Lux = 1000;
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x001787F5 File Offset: 0x001769F5
	protected override void OnPrefabInit()
	{
		base.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
		this.IntensityAnimation = 1f;
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x00178814 File Offset: 0x00176A14
	protected override void OnCmpEnable()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMoved), "Light2D.OnMoved");
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x00178880 File Offset: 0x00176A80
	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMoved));
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		this.FullRemove();
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x001788B8 File Offset: 0x00176AB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.origin = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (base.isActiveAndEnabled)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x00178912 File Offset: 0x00176B12
	protected override void OnCleanUp()
	{
		this.FullRemove();
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x0017891A File Offset: 0x00176B1A
	private void OnMoved()
	{
		if (base.isSpawned)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x0017892A File Offset: 0x00176B2A
	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, new Action<object>(this.OnWorldChanged));
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x00178950 File Offset: 0x00176B50
	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(this.origin);
		int num = (int)this.Range;
		Vector2I vector2I2 = new Vector2I(vector2I.x - num, vector2I.y - num);
		int width = 2 * num;
		int height = (this.shape == global::LightShape.Circle) ? (2 * num) : num;
		return new Extents(vector2I2.x, vector2I2.y, width, height);
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x001789B0 File Offset: 0x00176BB0
	private void AddToScenePartitioner()
	{
		Extents ext = this.ComputeExtents();
		this.solidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		this.liquidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x001789F2 File Offset: 0x00176BF2
	private void RemoveFromScenePartitioner()
	{
		if (this.isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
		}
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x00178A1C File Offset: 0x00176C1C
	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.ComputeExtents());
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x00178A4A File Offset: 0x00176C4A
	private void EmitterRefresh()
	{
		this.emitter.Refresh(this.pending_emitter_state, true);
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x00178A5F File Offset: 0x00176C5F
	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (!base.isSpawned || !base.isActiveAndEnabled)
		{
			return;
		}
		DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled", null);
		this.RefreshShapeAndPosition();
		this.EmitterRefresh();
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x00178A90 File Offset: 0x00176C90
	public void FullRemove()
	{
		this.RemoveFromScenePartitioner();
		this.emitter.RemoveFromGrid();
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00178AA4 File Offset: 0x00176CA4
	public Light2D.RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return Light2D.RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		int num = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (!Grid.IsValidCell(num))
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		this.origin = num;
		if (this.dirty_shape)
		{
			this.RemoveFromScenePartitioner();
			this.AddToScenePartitioner();
		}
		else if (this.dirty_position)
		{
			this.MoveInScenePartitioner();
		}
		if (this.dirty_falloff)
		{
			this.EmitterRefresh();
		}
		this.dirty_shape = false;
		this.dirty_position = false;
		this.dirty_falloff = false;
		return Light2D.RefreshResult.Updated;
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x00178B4B File Offset: 0x00176D4B
	private void OnWorldChanged(object data)
	{
		this.FullRefresh();
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x00178B54 File Offset: 0x00176D54
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04002BD1 RID: 11217
	private bool dirty_shape;

	// Token: 0x04002BD2 RID: 11218
	private bool dirty_position;

	// Token: 0x04002BD3 RID: 11219
	private bool dirty_falloff;

	// Token: 0x04002BD4 RID: 11220
	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	// Token: 0x04002BD7 RID: 11223
	public float Angle;

	// Token: 0x04002BD8 RID: 11224
	public Vector2 Direction;

	// Token: 0x04002BD9 RID: 11225
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x04002BDA RID: 11226
	public bool drawOverlay;

	// Token: 0x04002BDB RID: 11227
	public Color overlayColour;

	// Token: 0x04002BDC RID: 11228
	public MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04002BDD RID: 11229
	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04002BDE RID: 11230
	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04002BDF RID: 11231
	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		light.enabled = (bool)data;
	});

	// Token: 0x0200175F RID: 5983
	public enum RefreshResult
	{
		// Token: 0x04006E86 RID: 28294
		None,
		// Token: 0x04006E87 RID: 28295
		Removed,
		// Token: 0x04006E88 RID: 28296
		Updated
	}
}
