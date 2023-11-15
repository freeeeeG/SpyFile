using System;
using UnityEngine;

// Token: 0x02000377 RID: 887
[AddComponentMenu("Scripts/Core/UnitTests/TransformHelperUnitTests")]
public class TransformHelperUnitTests : MonoBehaviour
{
	// Token: 0x060010E3 RID: 4323 RVA: 0x00060D4E File Offset: 0x0005F14E
	private void Start()
	{
		this.DoTests();
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x00060D58 File Offset: 0x0005F158
	private void DoTests()
	{
		Quaternion quaternion = Quaternion.AngleAxis(60f, Vector3.up);
		Quaternion quaternion2 = TransformHelper.Inverted(quaternion);
		Quaternion quaternion3 = quaternion * quaternion2;
		Quaternion quaternion4 = quaternion2 * quaternion;
		Quaternion quaternion5 = Quaternion.AngleAxis(60f, Vector3.right);
		Quaternion quaternion6 = TransformHelper.Inverted(quaternion5);
		Quaternion quaternion7 = quaternion5 * quaternion6;
		Quaternion quaternion8 = quaternion6 * quaternion5;
		Quaternion quaternion9 = Quaternion.AngleAxis(60f, Vector3.forward);
		Quaternion quaternion10 = TransformHelper.Inverted(quaternion9);
		Quaternion quaternion11 = quaternion9 * quaternion10;
		Quaternion quaternion12 = quaternion10 * quaternion9;
		TransformHelper transformHelper = new TransformHelper(Quaternion.AngleAxis(45f, Vector3.up), Vector3.zero);
		TransformHelper trans = new TransformHelper(Quaternion.AngleAxis(135f, Vector3.up), Vector3.zero);
		float num;
		Vector3 vector;
		transformHelper.ToLocal(trans).Rotation.ToAngleAxis(out num, out vector);
		TransformHelper transformHelper2 = new TransformHelper(Quaternion.AngleAxis(45f, Vector3.right), Vector3.zero);
		TransformHelper trans2 = new TransformHelper(Quaternion.AngleAxis(135f, Vector3.right), Vector3.zero);
		float num2;
		Vector3 vector2;
		transformHelper2.ToLocal(trans2).Rotation.ToAngleAxis(out num2, out vector2);
		TransformHelper transformHelper3 = new TransformHelper(Quaternion.AngleAxis(45f, Vector3.forward), Vector3.zero);
		TransformHelper trans3 = new TransformHelper(Quaternion.AngleAxis(135f, Vector3.forward), Vector3.zero);
		float num3;
		Vector3 vector3;
		transformHelper3.ToLocal(trans3).Rotation.ToAngleAxis(out num3, out vector3);
		TransformHelper transformHelper4 = new TransformHelper(Quaternion.AngleAxis(45f, Vector3.forward), new Vector3(1f, 1f, 0f));
		TransformHelper trans4 = new TransformHelper(Quaternion.AngleAxis(-45f, Vector3.forward), new Vector3(2f, -2f, 0f));
		TransformHelper transformHelper5 = transformHelper4.ToLocal(trans4);
		float sqrMagnitude = (transformHelper5.R() - new Vector3(0f, -1f, 0f)).sqrMagnitude;
		float sqrMagnitude2 = (transformHelper5.U() - new Vector3(1f, 0f, 0f)).sqrMagnitude;
		float sqrMagnitude3 = (transformHelper5.Position - new Vector3(-Mathf.Sqrt(2f), -Mathf.Sqrt(8f), 0f)).sqrMagnitude;
		TransformHelper trans5 = new TransformHelper(Quaternion.AngleAxis(135f, new Vector3(0.707f, 0f, 0.707f)), Vector3.zero);
		TransformHelper trans6 = new TransformHelper(Quaternion.AngleAxis(45f, Vector3.forward), Vector3.zero);
		TransformHelper transformHelper6 = trans6.ToWorld(trans6.ToLocal(trans5));
		TransformHelper transformHelper7 = trans5.ToWorld(trans5.ToLocal(trans6));
		float sqrMagnitude4 = (transformHelper6.F() - trans5.F()).sqrMagnitude;
		float sqrMagnitude5 = (transformHelper7.F() - trans6.F()).sqrMagnitude;
	}

	// Token: 0x04000D09 RID: 3337
	[SerializeField]
	private float m_testEpsilon = 0.01f;
}
