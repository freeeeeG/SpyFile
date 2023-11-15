using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000CD RID: 205
	public class OutlineSetter : MonoBehaviour
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001D274 File Offset: 0x0001B474
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0001D298 File Offset: 0x0001B498
		public static bool isOn
		{
			get
			{
				return OutlineSetter.Instance.outlineMaterial.GetColor("_OutlineColor").a == 1f;
			}
			set
			{
				if (OutlineSetter.Instance == null)
				{
					return;
				}
				Color color = OutlineSetter.Instance.outlineMaterial.GetColor("_OutlineColor");
				if (value)
				{
					color.a = 1f;
				}
				else
				{
					color.a = 0f;
				}
				OutlineSetter.Instance.outlineMaterial.SetColor("_OutlineColor", color);
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001D2FA File Offset: 0x0001B4FA
		private void Awake()
		{
			if (OutlineSetter.Instance != null)
			{
				Object.Destroy(base.gameObject);
			}
			OutlineSetter.Instance = this;
		}

		// Token: 0x04000438 RID: 1080
		public static OutlineSetter Instance;

		// Token: 0x04000439 RID: 1081
		[SerializeField]
		private Material outlineMaterial;
	}
}
