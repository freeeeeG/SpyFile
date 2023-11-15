using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIExtensions
{
	// Token: 0x020000EF RID: 239
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	public abstract class UIEffectBase : BaseMeshEffect, IParameterTexture
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000F979 File Offset: 0x0000DB79
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000F981 File Offset: 0x0000DB81
		public int parameterIndex { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000F98A File Offset: 0x0000DB8A
		public virtual ParameterTexture ptex
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000F98D File Offset: 0x0000DB8D
		public Graphic targetGraphic
		{
			get
			{
				return base.graphic;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000F995 File Offset: 0x0000DB95
		public Material effectMaterial
		{
			get
			{
				return this.m_EffectMaterial;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F99D File Offset: 0x0000DB9D
		public virtual void ModifyMaterial()
		{
			this.targetGraphic.material = (base.isActiveAndEnabled ? this.m_EffectMaterial : null);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F9BB File Offset: 0x0000DBBB
		protected override void OnEnable()
		{
			if (this.ptex != null)
			{
				this.ptex.Register(this);
			}
			this.ModifyMaterial();
			this.targetGraphic.SetVerticesDirty();
			this.SetDirty();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
		protected override void OnDisable()
		{
			this.ModifyMaterial();
			this.targetGraphic.SetVerticesDirty();
			if (this.ptex != null)
			{
				this.ptex.Unregister(this);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000FA0F File Offset: 0x0000DC0F
		protected virtual void SetDirty()
		{
			this.targetGraphic.SetVerticesDirty();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x0400035A RID: 858
		protected static readonly Vector2[] splitedCharacterPosition = new Vector2[]
		{
			Vector2.up,
			Vector2.one,
			Vector2.right,
			Vector2.zero
		};

		// Token: 0x0400035B RID: 859
		protected static readonly List<UIVertex> tempVerts = new List<UIVertex>();

		// Token: 0x0400035C RID: 860
		[HideInInspector]
		[SerializeField]
		private int m_Version;

		// Token: 0x0400035D RID: 861
		[SerializeField]
		protected Material m_EffectMaterial;
	}
}
