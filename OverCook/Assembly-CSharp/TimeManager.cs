using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000259 RID: 601
public class TimeManager : Manager
{
	// Token: 0x06000B15 RID: 2837 RVA: 0x0003B80C File Offset: 0x00039C0C
	private void Awake()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_paused[i] = false;
			this.m_arbitrationSupressors[i] = new List<object>();
		}
		this.m_timeMultiplier = new float[32];
		for (int j = 0; j < this.m_timeMultiplier.Length; j++)
		{
			this.m_timeMultiplier[j] = 1f;
		}
		TimeManager.s_timeManager = this;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0003B87A File Offset: 0x00039C7A
	private void Update()
	{
		Time.timeScale = this.m_debugtimeScale;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0003B887 File Offset: 0x00039C87
	private void OnDestroy()
	{
		TimeManager.s_timeManager = null;
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0003B890 File Offset: 0x00039C90
	private bool UpdateArbitratedPause(TimeManager.PauseLayer _layer, bool _pause, object _arbitrationKey)
	{
		List<object> list = this.m_arbitrationSupressors[(int)_layer];
		if (_pause)
		{
			list.Add(_arbitrationKey);
		}
		else
		{
			list.RemoveAll(new Predicate<object>(_arbitrationKey.Equals));
		}
		return list.Count != 0;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0003B8D8 File Offset: 0x00039CD8
	public static bool IsPaused(TimeManager.PauseLayer _layer)
	{
		return TimeManager.s_timeManager != null && TimeManager.s_timeManager.IsLayerPaused(_layer);
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0003B8F7 File Offset: 0x00039CF7
	protected bool IsLayerPaused(TimeManager.PauseLayer _layer)
	{
		return this.m_arbitrationSupressors[(int)_layer].Count != 0;
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0003B90C File Offset: 0x00039D0C
	public void SetPaused(TimeManager.PauseLayer _layer, bool _pause, object _arbitrationKey)
	{
		bool flag = this.UpdateArbitratedPause(_layer, _pause, _arbitrationKey);
		bool flag2 = this.m_paused[(int)_layer];
		if (flag2 != flag)
		{
			this.OnPauseStatusChange(_layer, flag);
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0003B93C File Offset: 0x00039D3C
	private LayerMask GetLayerMask(TimeManager.PauseLayer _layer)
	{
		switch (_layer)
		{
		case TimeManager.PauseLayer.Main:
			return this.m_mainLayer;
		case TimeManager.PauseLayer.UI:
			return this.m_uiLayer;
		case TimeManager.PauseLayer.Camera:
			return this.m_cameraLayer;
		case TimeManager.PauseLayer.System:
			return this.m_systemLayer;
		case TimeManager.PauseLayer.Network:
			return this.m_networkLayer;
		default:
			return default(LayerMask);
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0003B994 File Offset: 0x00039D94
	private void OnPauseStatusChange(TimeManager.PauseLayer _layer, bool _pause)
	{
		LayerMask pauseMask = this.m_pauseMask;
		this.m_paused[(int)_layer] = _pause;
		LayerMask layerMask = 0;
		for (int i = 0; i < this.m_paused.Length; i++)
		{
			if (this.m_paused[i])
			{
				layerMask |= this.GetLayerMask((TimeManager.PauseLayer)i);
			}
		}
		this.m_pauseMask = layerMask;
		if (_layer == TimeManager.PauseLayer.Network)
		{
			return;
		}
		if (_pause)
		{
			LayerMask mask = layerMask & ~pauseMask;
			this.OnPauseChange(mask, true);
		}
		else
		{
			LayerMask mask2 = ~layerMask & pauseMask;
			this.OnPauseChange(mask2, false);
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0003BA4C File Offset: 0x00039E4C
	private void OnPauseChange(LayerMask _mask, bool _pause)
	{
		float num = (!_pause) ? 1f : 0f;
		for (int i = 0; i < this.m_timeMultiplier.Length; i++)
		{
			if (TimeManager.InMask(_mask, i))
			{
				this.m_timeMultiplier[i] = num;
			}
		}
		Animator[] array = UnityEngine.Object.FindObjectsOfType<Animator>();
		for (int j = 0; j < array.Length; j++)
		{
			if (TimeManager.InMask(_mask, array[j].gameObject.layer))
			{
				array[j].speed = num;
			}
		}
		Animation[] array2 = UnityEngine.Object.FindObjectsOfType<Animation>();
		for (int k = 0; k < array2.Length; k++)
		{
			if (TimeManager.InMask(_mask, array2[k].gameObject.layer))
			{
				foreach (object obj in array2[k])
				{
					AnimationState animationState = obj as AnimationState;
					if (animationState != null)
					{
						animationState.speed = num;
					}
				}
			}
		}
		ParticleSystem[] array3 = GameObjectUtils.FindComponentsOfTypeInScene<ParticleSystem>();
		for (int l = 0; l < array3.Length; l++)
		{
			if (TimeManager.InMask(_mask, array3[l].gameObject.layer))
			{
				array3[l].main.simulationSpeed = num;
			}
		}
		AudioSource[] array4 = UnityEngine.Object.FindObjectsOfType<AudioSource>();
		for (int m = 0; m < array4.Length; m++)
		{
			if (TimeManager.InMask(_mask, array4[m].gameObject.layer))
			{
				if (_pause)
				{
					array4[m].Pause();
				}
				else
				{
					array4[m].UnPause();
				}
			}
		}
		if (_pause)
		{
			Rigidbody[] array5 = UnityEngine.Object.FindObjectsOfType<Rigidbody>();
			array5 = array5.AllRemoved_Predicate((Rigidbody x) => !TimeManager.InMask(_mask, x.gameObject.layer));
			for (int n = 0; n < array5.Length; n++)
			{
				this.m_frozenPhysics.Add(new TimeManager.FrozenPhysicsData(array5[n], array5[n].gameObject.layer));
			}
		}
		else
		{
			Predicate<TimeManager.FrozenPhysicsData> match = delegate(TimeManager.FrozenPhysicsData _fpd)
			{
				if (TimeManager.InMask(_mask, _fpd.Layer))
				{
					_fpd.Unfreeze();
					return true;
				}
				return false;
			};
			this.m_frozenPhysics.RemoveAll(match);
		}
		PlayableDirector[] array6 = UnityEngine.Object.FindObjectsOfType<PlayableDirector>();
		for (int num2 = 0; num2 < array6.Length; num2++)
		{
			PlayableGraph playableGraph = array6[num2].playableGraph;
			if (playableGraph.IsValid())
			{
				int rootPlayableCount = playableGraph.GetRootPlayableCount();
				for (int num3 = 0; num3 < rootPlayableCount; num3++)
				{
					playableGraph.GetRootPlayable(num3).SetSpeed((double)((!_pause) ? 1 : 0));
				}
			}
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0003BD1C File Offset: 0x0003A11C
	public static bool IsPaused(int _layer)
	{
		if (TimeManager.s_timeManager != null)
		{
			for (int i = 0; i < 5; i++)
			{
				LayerMask layerMask = TimeManager.s_timeManager.GetLayerMask((TimeManager.PauseLayer)i);
				if (TimeManager.InMask(layerMask, _layer))
				{
					return TimeManager.s_timeManager.m_paused[i];
				}
			}
		}
		return false;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0003BD71 File Offset: 0x0003A171
	public static bool IsPaused(GameObject _object)
	{
		return TimeManager.IsPaused(_object.layer);
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0003BD7E File Offset: 0x0003A17E
	public static float GetDeltaTime(int _layer)
	{
		if (TimeManager.s_timeManager != null)
		{
			return TimeManager.s_timeManager.m_timeMultiplier[_layer] * Time.deltaTime;
		}
		return Time.deltaTime;
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0003BDA8 File Offset: 0x0003A1A8
	public static float GetFixedDeltaTime(int _layer)
	{
		if (TimeManager.s_timeManager != null)
		{
			return TimeManager.s_timeManager.m_timeMultiplier[_layer] * Time.fixedDeltaTime;
		}
		return Time.fixedDeltaTime;
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0003BDD2 File Offset: 0x0003A1D2
	public static float GetDeltaTime(GameObject _object)
	{
		return TimeManager.GetDeltaTime(_object.layer);
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0003BDDF File Offset: 0x0003A1DF
	public static float GetFixedDeltaTime(GameObject _object)
	{
		return TimeManager.GetFixedDeltaTime(_object.layer);
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0003BDEC File Offset: 0x0003A1EC
	private static bool InMask(LayerMask _layerMask, int _layerId)
	{
		return (_layerMask.value & 1 << _layerId) != 0;
	}

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private LayerMask m_mainLayer;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private LayerMask m_uiLayer;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private LayerMask m_cameraLayer;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private LayerMask m_networkLayer;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private LayerMask m_systemLayer = -1;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private float m_debugtimeScale = 1f;

	// Token: 0x040008B3 RID: 2227
	private List<object>[] m_arbitrationSupressors = new List<object>[5];

	// Token: 0x040008B4 RID: 2228
	private List<TimeManager.FrozenPhysicsData> m_frozenPhysics = new List<TimeManager.FrozenPhysicsData>();

	// Token: 0x040008B5 RID: 2229
	private bool[] m_paused = new bool[5];

	// Token: 0x040008B6 RID: 2230
	private const int c_layerCount = 32;

	// Token: 0x040008B7 RID: 2231
	private float[] m_timeMultiplier = new float[0];

	// Token: 0x040008B8 RID: 2232
	private LayerMask m_pauseMask = 0;

	// Token: 0x040008B9 RID: 2233
	private static TimeManager s_timeManager;

	// Token: 0x0200025A RID: 602
	public enum PauseLayer
	{
		// Token: 0x040008BB RID: 2235
		Main,
		// Token: 0x040008BC RID: 2236
		UI,
		// Token: 0x040008BD RID: 2237
		Camera,
		// Token: 0x040008BE RID: 2238
		System,
		// Token: 0x040008BF RID: 2239
		Network,
		// Token: 0x040008C0 RID: 2240
		Count
	}

	// Token: 0x0200025B RID: 603
	private class FrozenPhysicsData
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x0003BE04 File Offset: 0x0003A204
		public FrozenPhysicsData(Rigidbody _rigidBody, int _layer)
		{
			this.m_frozenBody = _rigidBody;
			this.m_linearVelocity = _rigidBody.velocity;
			this.m_angularVelocity = _rigidBody.angularVelocity;
			this.m_isKinematic = _rigidBody.isKinematic;
			this.m_useGravity = _rigidBody.useGravity;
			this.m_layer = _layer;
			this.m_frozenBody.velocity = Vector3.zero;
			this.m_frozenBody.angularVelocity = Vector3.zero;
			this.m_frozenBody.isKinematic = true;
			this.m_frozenBody.useGravity = false;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0003BE8D File Offset: 0x0003A28D
		public int Layer
		{
			get
			{
				return this.m_layer;
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003BE98 File Offset: 0x0003A298
		public void Unfreeze()
		{
			if (this.m_frozenBody != null)
			{
				this.m_frozenBody.velocity = this.m_linearVelocity;
				this.m_frozenBody.angularVelocity = this.m_angularVelocity;
				this.m_frozenBody.isKinematic = this.m_isKinematic;
				this.m_frozenBody.useGravity = this.m_useGravity;
				this.m_frozenBody = null;
			}
		}

		// Token: 0x040008C1 RID: 2241
		private int m_layer;

		// Token: 0x040008C2 RID: 2242
		private Rigidbody m_frozenBody;

		// Token: 0x040008C3 RID: 2243
		private Vector3 m_linearVelocity;

		// Token: 0x040008C4 RID: 2244
		private Vector3 m_angularVelocity;

		// Token: 0x040008C5 RID: 2245
		private bool m_isKinematic;

		// Token: 0x040008C6 RID: 2246
		private bool m_useGravity;
	}
}
