using System;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public static class CameraSaveData
{
	// Token: 0x06002F29 RID: 12073 RVA: 0x000F8B28 File Offset: 0x000F6D28
	public static void Load(FastReader reader)
	{
		CameraSaveData.position = reader.ReadVector3();
		CameraSaveData.localScale = reader.ReadVector3();
		CameraSaveData.rotation = reader.ReadQuaternion();
		CameraSaveData.orthographicsSize = reader.ReadSingle();
		CameraSaveData.valid = true;
	}

	// Token: 0x04001BF3 RID: 7155
	public static bool valid;

	// Token: 0x04001BF4 RID: 7156
	public static Vector3 position;

	// Token: 0x04001BF5 RID: 7157
	public static Vector3 localScale;

	// Token: 0x04001BF6 RID: 7158
	public static Quaternion rotation;

	// Token: 0x04001BF7 RID: 7159
	public static float orthographicsSize;
}
