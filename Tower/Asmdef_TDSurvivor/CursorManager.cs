using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CursorManager : MonoBehaviour
{
	// Token: 0x060001FB RID: 507 RVA: 0x00008C66 File Offset: 0x00006E66
	private void Awake()
	{
		this.cursorTex = (Resources.Load("Cursor/Cursor") as Texture2D);
		Cursor.SetCursor(this.cursorTex, Vector2.zero, CursorMode.ForceSoftware);
	}

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private Texture2D cursorTex;

	// Token: 0x020001E7 RID: 487
	public class CursorData
	{
		// Token: 0x040009E0 RID: 2528
		public string cursorType;

		// Token: 0x040009E1 RID: 2529
		public Texture2D tex_128;

		// Token: 0x040009E2 RID: 2530
		public Texture2D tex_64;

		// Token: 0x040009E3 RID: 2531
		public Texture2D tex_32;
	}
}
