using System;
using UnityEngine;

namespace FlatKit
{
	// Token: 0x02000057 RID: 87
	public class Buoyancy : MonoBehaviour
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00004E68 File Offset: 0x00003068
		private void Start()
		{
			Renderer component = this.water.GetComponent<Renderer>();
			this._material = ((this.overrideWaterMaterial != null) ? this.overrideWaterMaterial : component.sharedMaterial);
			this._speed = this._material.GetFloat("_WaveSpeed");
			this._amplitude = this._material.GetFloat("_WaveAmplitude");
			this._frequency = this._material.GetFloat("_WaveFrequency");
			this._direction = this._material.GetFloat("_WaveDirection");
			Transform transform = base.transform;
			this._originalPosition = transform.position;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004F10 File Offset: 0x00003110
		private void Update()
		{
			Vector3 position = base.transform.position;
			Vector3 positionOS = this.water.InverseTransformPoint(position);
			position.y = this.GetHeightOS(positionOS) + this._originalPosition.y;
			base.transform.position = position;
			base.transform.up = this.GetNormalWS(positionOS);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004F70 File Offset: 0x00003170
		private Vector2 GradientNoiseDir(Vector2 p)
		{
			p = new Vector2(p.x % 289f, p.y % 289f);
			float num = (34f * p.x + 1f) * p.x % 289f + p.y;
			num = (34f * num + 1f) * num % 289f;
			num = num / 41f % 1f * 2f - 1f;
			return new Vector2(num - Mathf.Floor(num + 0.5f), Mathf.Abs(num) - 0.5f).normalized;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000501C File Offset: 0x0000321C
		private float GradientNoise(Vector2 p)
		{
			Vector2 vector = new Vector2(Mathf.Floor(p.x), Mathf.Floor(p.y));
			Vector2 vector2 = new Vector2(p.x % 1f, p.y % 1f);
			float a = Vector3.Dot(this.GradientNoiseDir(vector), vector2);
			float b = Vector3.Dot(this.GradientNoiseDir(vector + Vector2.up), vector2 - Vector2.up);
			float a2 = Vector3.Dot(this.GradientNoiseDir(vector + Vector2.right), vector2 - Vector2.right);
			float b2 = Vector3.Dot(this.GradientNoiseDir(vector + Vector2.one), vector2 - Vector2.one);
			vector2 = vector2 * vector2 * vector2 * (vector2 * (vector2 * 6f - Vector2.one * 15f) + Vector2.one * 10f);
			return Mathf.Lerp(Mathf.Lerp(a, b, vector2.y), Mathf.Lerp(a2, b2, vector2.y), vector2.x);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005174 File Offset: 0x00003374
		private Vector3 GetNormalWS(Vector3 positionOS)
		{
			Vector3 vector = positionOS + Vector3.forward * this.size;
			vector.y = this.GetHeightOS(vector);
			Vector3 a = positionOS + Vector3.right * this.size;
			a.y = this.GetHeightOS(vector);
			Vector3 normalized = Vector3.Cross(vector - positionOS, a - positionOS).normalized;
			return this.water.TransformDirection(normalized);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000051F4 File Offset: 0x000033F4
		private float SineWave(Vector3 positionOS, float offset)
		{
			float num = Time.timeSinceLevelLoad * 2f;
			float num2 = Mathf.Sin(offset + num * this._speed + (positionOS.x * Mathf.Sin(offset + this._direction) + positionOS.z * Mathf.Cos(offset + this._direction)) * this._frequency);
			if (this._material.IsKeywordEnabled("_WAVEMODE_POINTY"))
			{
				num2 = 1f - Mathf.Abs(num2);
			}
			return num2 * this._amplitude;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005278 File Offset: 0x00003478
		private float GetHeightOS(Vector3 positionOS)
		{
			float num = this.SineWave(positionOS, 0f);
			if (this._material.IsKeywordEnabled("_WAVEMODE_GRID"))
			{
				num *= this.SineWave(positionOS, 1.57f);
			}
			return num * this.amplitude;
		}

		// Token: 0x040000DE RID: 222
		[Tooltip("The object that contains a Water material.")]
		public Transform water;

		// Token: 0x040000DF RID: 223
		[Space]
		[Tooltip("Range of probing wave height for buoyancy rotation.")]
		public float size = 1f;

		// Token: 0x040000E0 RID: 224
		[Tooltip("Max height of buoyancy going up and down.")]
		public float amplitude = 1f;

		// Token: 0x040000E1 RID: 225
		[Space]
		[Tooltip("Optionally provide a separate material to get the wave parameters.")]
		public Material overrideWaterMaterial;

		// Token: 0x040000E2 RID: 226
		private Material _material;

		// Token: 0x040000E3 RID: 227
		private float _speed;

		// Token: 0x040000E4 RID: 228
		private float _amplitude;

		// Token: 0x040000E5 RID: 229
		private float _frequency;

		// Token: 0x040000E6 RID: 230
		private float _direction;

		// Token: 0x040000E7 RID: 231
		private Vector3 _originalPosition;
	}
}
