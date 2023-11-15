using System;
using UnityEngine;

// Token: 0x020003E5 RID: 997
[Serializable]
public class PipeAnimatorData : MonoBehaviour
{
	// Token: 0x04000E57 RID: 3671
	[SerializeField]
	public LinearPath m_path;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	[Range(0f, 1f)]
	public float m_position;

	// Token: 0x04000E59 RID: 3673
	[SerializeField]
	public float m_displacement;

	// Token: 0x04000E5A RID: 3674
	[SerializeField]
	public float m_falloff;
}
