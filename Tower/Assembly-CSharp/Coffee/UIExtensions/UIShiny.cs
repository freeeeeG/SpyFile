using System;
using Helios.GUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coffee.UIExtensions
{
	// Token: 0x020000F0 RID: 240
	[AddComponentMenu("UI/UIEffect/UIShiny", 2)]
	public class UIShiny : UIEffectBase
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000FA7E File Offset: 0x0000DC7E
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000FA86 File Offset: 0x0000DC86
		[Obsolete("Use effectFactor instead (UnityUpgradable) -> effectFactor")]
		public float location
		{
			get
			{
				return this.m_EffectFactor;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_EffectFactor, value))
				{
					this.m_EffectFactor = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000FAB5 File Offset: 0x0000DCB5
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000FABD File Offset: 0x0000DCBD
		public float effectFactor
		{
			get
			{
				return this.m_EffectFactor;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_EffectFactor, value))
				{
					this.m_EffectFactor = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000FAEC File Offset: 0x0000DCEC
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000FAF4 File Offset: 0x0000DCF4
		public float width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_Width, value))
				{
					this.m_Width = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000FB23 File Offset: 0x0000DD23
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000FB2B File Offset: 0x0000DD2B
		public float softness
		{
			get
			{
				return this.m_Softness;
			}
			set
			{
				value = Mathf.Clamp(value, 0.01f, 1f);
				if (!Mathf.Approximately(this.m_Softness, value))
				{
					this.m_Softness = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0000FB62 File Offset: 0x0000DD62
		[Obsolete("Use brightness instead (UnityUpgradable) -> brightness")]
		public float alpha
		{
			get
			{
				return this.m_Brightness;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_Brightness, value))
				{
					this.m_Brightness = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000FB91 File Offset: 0x0000DD91
		// (set) Token: 0x06000396 RID: 918 RVA: 0x0000FB99 File Offset: 0x0000DD99
		public float brightness
		{
			get
			{
				return this.m_Brightness;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_Brightness, value))
				{
					this.m_Brightness = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
		[Obsolete("Use gloss instead (UnityUpgradable) -> gloss")]
		public float highlight
		{
			get
			{
				return this.m_Gloss;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_Gloss, value))
				{
					this.m_Gloss = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000FBFF File Offset: 0x0000DDFF
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000FC07 File Offset: 0x0000DE07
		public float gloss
		{
			get
			{
				return this.m_Gloss;
			}
			set
			{
				value = Mathf.Clamp(value, 0f, 1f);
				if (!Mathf.Approximately(this.m_Gloss, value))
				{
					this.m_Gloss = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600039B RID: 923 RVA: 0x0000FC36 File Offset: 0x0000DE36
		// (set) Token: 0x0600039C RID: 924 RVA: 0x0000FC3E File Offset: 0x0000DE3E
		public float rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				if (!Mathf.Approximately(this.m_Rotation, value))
				{
					this.m_Rotation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000FC5B File Offset: 0x0000DE5B
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000FC63 File Offset: 0x0000DE63
		public EffectArea effectArea
		{
			get
			{
				return this.m_EffectArea;
			}
			set
			{
				if (this.m_EffectArea != value)
				{
					this.m_EffectArea = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000FC7B File Offset: 0x0000DE7B
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000FC88 File Offset: 0x0000DE88
		public bool play
		{
			get
			{
				return this._player.play;
			}
			set
			{
				this._player.play = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000FC96 File Offset: 0x0000DE96
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x0000FCA3 File Offset: 0x0000DEA3
		public bool loop
		{
			get
			{
				return this._player.loop;
			}
			set
			{
				this._player.loop = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000FCB1 File Offset: 0x0000DEB1
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x0000FCBE File Offset: 0x0000DEBE
		public float duration
		{
			get
			{
				return this._player.duration;
			}
			set
			{
				this._player.duration = Mathf.Max(value, 0.1f);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000FCD6 File Offset: 0x0000DED6
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000FCE3 File Offset: 0x0000DEE3
		public float loopDelay
		{
			get
			{
				return this._player.loopDelay;
			}
			set
			{
				this._player.loopDelay = Mathf.Max(value, 0f);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000FCFB File Offset: 0x0000DEFB
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0000FD08 File Offset: 0x0000DF08
		public AnimatorUpdateMode updateMode
		{
			get
			{
				return this._player.updateMode;
			}
			set
			{
				this._player.updateMode = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000FD16 File Offset: 0x0000DF16
		public override ParameterTexture ptex
		{
			get
			{
				return UIShiny._ptex;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FD1D File Offset: 0x0000DF1D
		protected override void OnEnable()
		{
			base.OnEnable();
			this._player.OnEnable(delegate(float f)
			{
				this.effectFactor = f;
			});
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		protected override void OnDisable()
		{
			base.OnDisable();
			this._player.OnDisable();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FD50 File Offset: 0x0000DF50
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			float normalizedIndex = this.ptex.GetNormalizedIndex(this);
			Rect effectArea = this.m_EffectArea.GetEffectArea(vh, base.graphic, -1f);
			float f = this.m_Rotation * 0.017453292f;
			Vector2 normalized = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			normalized.x *= effectArea.height / effectArea.width;
			normalized = normalized.normalized;
			bool flag = base.graphic is Text && this.m_EffectArea == EffectArea.Character;
			UIVertex uivertex = default(UIVertex);
			Matrix2x3 m = new Matrix2x3(effectArea, normalized.x, normalized.y);
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref uivertex, i);
				Vector2 vector;
				if (flag)
				{
					vector = m * UIEffectBase.splitedCharacterPosition[i % 4];
				}
				else
				{
					vector = m * uivertex.position;
				}
				uivertex.uv0 = new Vector2(Packer.ToFloat(uivertex.uv0.x, uivertex.uv0.y), Packer.ToFloat(vector.y, normalizedIndex));
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		public void Play()
		{
			this._player.Play(null);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000FEAE File Offset: 0x0000E0AE
		public void Stop()
		{
			this._player.Stop();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		protected override void SetDirty()
		{
			this.ptex.RegisterMaterial(base.targetGraphic.material);
			this.ptex.SetData(this, 0, this.m_EffectFactor);
			this.ptex.SetData(this, 1, this.m_Width);
			this.ptex.SetData(this, 2, this.m_Softness);
			this.ptex.SetData(this, 3, this.m_Brightness);
			this.ptex.SetData(this, 4, this.m_Gloss);
			if (!Mathf.Approximately(this._lastRotation, this.m_Rotation) && base.targetGraphic)
			{
				this._lastRotation = this.m_Rotation;
				base.targetGraphic.SetVerticesDirty();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000FF78 File Offset: 0x0000E178
		private EffectPlayer _player
		{
			get
			{
				EffectPlayer result;
				if ((result = this.m_Player) == null)
				{
					result = (this.m_Player = new EffectPlayer());
				}
				return result;
			}
		}

		// Token: 0x0400035F RID: 863
		public const string shaderName = "UI/Hidden/UI-Effect-Shiny";

		// Token: 0x04000360 RID: 864
		private static readonly ParameterTexture _ptex = new ParameterTexture(8, 128, "_ParamTex");

		// Token: 0x04000361 RID: 865
		[Tooltip("Location for shiny effect.")]
		[FormerlySerializedAs("m_Location")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_EffectFactor;

		// Token: 0x04000362 RID: 866
		[Tooltip("Width for shiny effect.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Width = 0.25f;

		// Token: 0x04000363 RID: 867
		[Tooltip("Rotation for shiny effect.")]
		[SerializeField]
		[Range(-180f, 180f)]
		private float m_Rotation;

		// Token: 0x04000364 RID: 868
		[Tooltip("Softness for shiny effect.")]
		[SerializeField]
		[Range(0.01f, 1f)]
		private float m_Softness = 1f;

		// Token: 0x04000365 RID: 869
		[Tooltip("Brightness for shiny effect.")]
		[FormerlySerializedAs("m_Alpha")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Brightness = 1f;

		// Token: 0x04000366 RID: 870
		[Tooltip("Gloss factor for shiny effect.")]
		[FormerlySerializedAs("m_Highlight")]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Gloss = 1f;

		// Token: 0x04000367 RID: 871
		[Tooltip("The area for effect.")]
		[SerializeField]
		protected EffectArea m_EffectArea;

		// Token: 0x04000368 RID: 872
		[SerializeField]
		private EffectPlayer m_Player;

		// Token: 0x04000369 RID: 873
		[Obsolete]
		[HideInInspector]
		[SerializeField]
		private bool m_Play;

		// Token: 0x0400036A RID: 874
		[Obsolete]
		[HideInInspector]
		[SerializeField]
		private bool m_Loop;

		// Token: 0x0400036B RID: 875
		[Obsolete]
		[HideInInspector]
		[SerializeField]
		[Range(0.1f, 10f)]
		private float m_Duration = 1f;

		// Token: 0x0400036C RID: 876
		[Obsolete]
		[HideInInspector]
		[SerializeField]
		[Range(0f, 10f)]
		private float m_LoopDelay = 1f;

		// Token: 0x0400036D RID: 877
		[Obsolete]
		[HideInInspector]
		[SerializeField]
		private AnimatorUpdateMode m_UpdateMode;

		// Token: 0x0400036E RID: 878
		private float _lastRotation;
	}
}
