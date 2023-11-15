using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIExtensions
{
	// Token: 0x020000EE RID: 238
	[Serializable]
	public class ParameterTexture
	{
		// Token: 0x06000376 RID: 886 RVA: 0x0000F728 File Offset: 0x0000D928
		public ParameterTexture(int channels, int instanceLimit, string propertyName)
		{
			this._propertyName = propertyName;
			this._channels = ((channels - 1) / 4 + 1) * 4;
			this._instanceLimit = ((instanceLimit - 1) / 2 + 1) * 2;
			this._data = new byte[this._channels * this._instanceLimit];
			this._stack = new Stack<int>(this._instanceLimit);
			for (int i = 1; i < this._instanceLimit + 1; i++)
			{
				this._stack.Push(i);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		public void Register(IParameterTexture target)
		{
			this.Initialize();
			if (target.parameterIndex <= 0 && 0 < this._stack.Count)
			{
				target.parameterIndex = this._stack.Pop();
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000F7D8 File Offset: 0x0000D9D8
		public void Unregister(IParameterTexture target)
		{
			if (0 < target.parameterIndex)
			{
				this._stack.Push(target.parameterIndex);
				target.parameterIndex = 0;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		public void SetData(IParameterTexture target, int channelId, byte value)
		{
			int num = (target.parameterIndex - 1) * this._channels + channelId;
			if (0 < target.parameterIndex && this._data[num] != value)
			{
				this._data[num] = value;
				this._needUpload = true;
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F83F File Offset: 0x0000DA3F
		public void SetData(IParameterTexture target, int channelId, float value)
		{
			this.SetData(target, channelId, (byte)(Mathf.Clamp01(value) * 255f));
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F856 File Offset: 0x0000DA56
		public void RegisterMaterial(Material mat)
		{
			if (this._propertyId == 0)
			{
				this._propertyId = Shader.PropertyToID(this._propertyName);
			}
			if (mat)
			{
				mat.SetTexture(this._propertyId, this._texture);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000F88B File Offset: 0x0000DA8B
		public float GetNormalizedIndex(IParameterTexture target)
		{
			return ((float)target.parameterIndex - 0.5f) / (float)this._instanceLimit;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
		private void Initialize()
		{
			if (ParameterTexture.updates == null)
			{
				ParameterTexture.updates = new List<Action>();
				Canvas.willRenderCanvases += delegate()
				{
					int count = ParameterTexture.updates.Count;
					for (int i = 0; i < count; i++)
					{
						ParameterTexture.updates[i]();
					}
				};
			}
			if (!this._texture)
			{
				this._texture = new Texture2D(this._channels / 4, this._instanceLimit, TextureFormat.RGBA32, false, false);
				this._texture.filterMode = FilterMode.Point;
				this._texture.wrapMode = TextureWrapMode.Clamp;
				ParameterTexture.updates.Add(new Action(this.UpdateParameterTexture));
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000F93D File Offset: 0x0000DB3D
		private void UpdateParameterTexture()
		{
			if (this._needUpload && this._texture)
			{
				this._needUpload = false;
				this._texture.LoadRawTextureData(this._data);
				this._texture.Apply(false, false);
			}
		}

		// Token: 0x04000351 RID: 849
		private Texture2D _texture;

		// Token: 0x04000352 RID: 850
		private bool _needUpload;

		// Token: 0x04000353 RID: 851
		private int _propertyId;

		// Token: 0x04000354 RID: 852
		private readonly string _propertyName;

		// Token: 0x04000355 RID: 853
		private readonly int _channels;

		// Token: 0x04000356 RID: 854
		private readonly int _instanceLimit;

		// Token: 0x04000357 RID: 855
		private readonly byte[] _data;

		// Token: 0x04000358 RID: 856
		private readonly Stack<int> _stack;

		// Token: 0x04000359 RID: 857
		private static List<Action> updates;
	}
}
