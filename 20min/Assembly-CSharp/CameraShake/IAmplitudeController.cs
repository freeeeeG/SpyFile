using System;

namespace CameraShake
{
	// Token: 0x02000058 RID: 88
	public interface IAmplitudeController
	{
		// Token: 0x06000442 RID: 1090
		void SetTargetAmplitude(float value);

		// Token: 0x06000443 RID: 1091
		void Finish();

		// Token: 0x06000444 RID: 1092
		void FinishImmediately();
	}
}
