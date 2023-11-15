using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
[Serializable]
public struct SpriteSheet
{
	// Token: 0x040010F6 RID: 4342
	public string name;

	// Token: 0x040010F7 RID: 4343
	public int numFrames;

	// Token: 0x040010F8 RID: 4344
	public int numXFrames;

	// Token: 0x040010F9 RID: 4345
	public Vector2 uvFrameSize;

	// Token: 0x040010FA RID: 4346
	public int renderLayer;

	// Token: 0x040010FB RID: 4347
	public Material material;

	// Token: 0x040010FC RID: 4348
	public Texture2D texture;
}
