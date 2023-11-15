using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007E1 RID: 2017
	[RequireComponent(typeof(PlateStation))]
	[RequireComponent(typeof(Interactable))]
	public class HordeTarget : MonoBehaviour
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000B8870 File Offset: 0x000B6C70
		public Transform TargetTransform
		{
			get
			{
				return (!(this.m_targetTransform != null)) ? base.transform : this.m_targetTransform;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x000B8894 File Offset: 0x000B6C94
		public float TargetRadius
		{
			get
			{
				return this.m_targetRadius;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x000B889C File Offset: 0x000B6C9C
		public Transform SpawnTransform
		{
			get
			{
				return (!(this.m_spawnTransform != null)) ? base.transform : this.m_spawnTransform;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x000B88C0 File Offset: 0x000B6CC0
		public float SpawnRadius
		{
			get
			{
				return this.m_spawnRadius;
			}
		}

		// Token: 0x04001EC1 RID: 7873
		[SerializeField]
		private Transform m_targetTransform;

		// Token: 0x04001EC2 RID: 7874
		[Range(0.5f, 5f)]
		[SerializeField]
		private float m_targetRadius = 0.5f;

		// Token: 0x04001EC3 RID: 7875
		[SerializeField]
		private Transform m_spawnTransform;

		// Token: 0x04001EC4 RID: 7876
		[Range(0.5f, 5f)]
		[SerializeField]
		private float m_spawnRadius = 0.5f;
	}
}
