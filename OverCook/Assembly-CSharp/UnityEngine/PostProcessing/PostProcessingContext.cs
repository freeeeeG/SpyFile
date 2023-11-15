using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E6 RID: 230
	public class PostProcessingContext
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00026782 File Offset: 0x00024B82
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0002678A File Offset: 0x00024B8A
		public bool interrupted { get; private set; }

		// Token: 0x0600044C RID: 1100 RVA: 0x00026793 File Offset: 0x00024B93
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0002679C File Offset: 0x00024B9C
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x000267C2 File Offset: 0x00024BC2
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000267D2 File Offset: 0x00024BD2
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000267DF File Offset: 0x00024BDF
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x000267EC File Offset: 0x00024BEC
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000267F9 File Offset: 0x00024BF9
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x040003DA RID: 986
		public PostProcessingProfile profile;

		// Token: 0x040003DB RID: 987
		public Camera camera;

		// Token: 0x040003DC RID: 988
		public MaterialFactory materialFactory;

		// Token: 0x040003DD RID: 989
		public RenderTextureFactory renderTextureFactory;
	}
}
