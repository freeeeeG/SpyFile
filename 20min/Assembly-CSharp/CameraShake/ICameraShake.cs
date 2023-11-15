using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000059 RID: 89
	public interface ICameraShake
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000445 RID: 1093
		Displacement CurrentDisplacement { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000446 RID: 1094
		bool IsFinished { get; }

		// Token: 0x06000447 RID: 1095
		void Initialize(Vector3 cameraPosition, Quaternion cameraRotation);

		// Token: 0x06000448 RID: 1096
		void Update(float deltaTime, Vector3 cameraPosition, Quaternion cameraRotation);
	}
}
