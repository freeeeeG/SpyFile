using System;
using Characters;
using UnityEngine;

namespace FX
{
	// Token: 0x02000264 RID: 612
	public abstract class VisualEffect : MonoBehaviour
	{
		// Token: 0x06000BFB RID: 3067 RVA: 0x00002191 File Offset: 0x00000391
		public static void PostProcess(PoolObject poolObject, Character target, float scale, float angle, Vector3 offset, EffectInfo.AttachInfo attachInfo, bool relativeScaleToTargetSize, bool overwrite = false)
		{
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00020DB0 File Offset: 0x0001EFB0
		public static void PostProcess(PoolObject poolObject, Character target, float scale, float angle, Vector3 offset, bool attachToTarget, bool relativeScaleToTargetSize, bool overwrite = false)
		{
			float num = 1f;
			if (relativeScaleToTargetSize)
			{
				Vector3 size = target.collider.bounds.size;
				num = Mathf.Min(size.x, size.y);
			}
			if (attachToTarget)
			{
				poolObject.transform.parent = target.transform;
			}
			VisualEffect.PostProcess(poolObject, scale * num, angle, offset, overwrite);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00020E10 File Offset: 0x0001F010
		public static void PostProcess(PoolObject poolObject, float scale, float angle, Vector3 offset, bool overwrite = false)
		{
			if (overwrite)
			{
				poolObject.transform.localScale = Vector3.one * scale;
				poolObject.transform.eulerAngles = new Vector3(0f, 0f, angle);
			}
			else
			{
				poolObject.transform.localScale *= scale;
				poolObject.transform.eulerAngles += new Vector3(0f, 0f, angle);
			}
			poolObject.transform.localPosition += offset;
		}
	}
}
