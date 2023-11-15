using System;
using UnityEngine.UI;

// Token: 0x02000B7C RID: 2940
public interface ITextProcessor
{
	// Token: 0x06003BF7 RID: 15351
	bool HasEmbeddedImages(string inputString);

	// Token: 0x06003BF8 RID: 15352
	bool ProcessText(ref string inputString);

	// Token: 0x06003BF9 RID: 15353
	bool OnPopulateMesh(VertexHelper _helper);
}
