using System;
using UnityEngine;

namespace PixelArsenal
{
	// Token: 0x020002B0 RID: 688
	public class PixelArsenalBeamStatic : MonoBehaviour
	{
		// Token: 0x060010F4 RID: 4340 RVA: 0x0002FC2D File Offset: 0x0002DE2D
		private void Start()
		{
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0002FC2F File Offset: 0x0002DE2F
		private void OnEnable()
		{
			if (this.alwaysOn)
			{
				this.SpawnBeam();
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0002FC3F File Offset: 0x0002DE3F
		private void OnDisable()
		{
			this.RemoveBeam();
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0002FC48 File Offset: 0x0002DE48
		private void FixedUpdate()
		{
			if (this.beam)
			{
				this.line.SetPosition(0, base.transform.position);
				RaycastHit raycastHit;
				Vector3 vector;
				if (this.beamCollides && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit))
				{
					vector = raycastHit.point - base.transform.forward * this.beamEndOffset;
				}
				else
				{
					vector = base.transform.position + base.transform.forward * this.beamLength;
				}
				this.line.SetPosition(1, vector);
				if (this.beamStart)
				{
					this.beamStart.transform.position = base.transform.position;
					this.beamStart.transform.LookAt(vector);
				}
				if (this.beamEnd)
				{
					this.beamEnd.transform.position = vector;
					this.beamEnd.transform.LookAt(this.beamStart.transform.position);
				}
				float num = Vector3.Distance(base.transform.position, vector);
				this.line.material.mainTextureScale = new Vector2(num / this.textureLengthScale, 1f);
				this.line.material.mainTextureOffset -= new Vector2(Time.deltaTime * this.textureScrollSpeed, 0f);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0002FDD8 File Offset: 0x0002DFD8
		public void SpawnBeam()
		{
			if (this.beamLineRendererPrefab)
			{
				if (this.beamStartPrefab)
				{
					this.beamStart = Object.Instantiate<GameObject>(this.beamStartPrefab);
				}
				if (this.beamEndPrefab)
				{
					this.beamEnd = Object.Instantiate<GameObject>(this.beamEndPrefab);
				}
				this.beam = Object.Instantiate<GameObject>(this.beamLineRendererPrefab);
				this.beam.transform.position = base.transform.position;
				this.beam.transform.parent = base.transform;
				this.beam.transform.rotation = base.transform.rotation;
				this.line = this.beam.GetComponent<LineRenderer>();
				this.line.useWorldSpace = true;
				this.line.positionCount = 2;
				return;
			}
			MonoBehaviour.print("Add a hecking prefab with a line renderer to the PixelArsenalBeamStatic script on " + base.gameObject.name + "! Heck!");
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0002FED8 File Offset: 0x0002E0D8
		public void RemoveBeam()
		{
			if (this.beam)
			{
				Object.Destroy(this.beam);
			}
			if (this.beamStart)
			{
				Object.Destroy(this.beamStart);
			}
			if (this.beamEnd)
			{
				Object.Destroy(this.beamEnd);
			}
		}

		// Token: 0x0400092D RID: 2349
		[Header("Prefabs")]
		public GameObject beamLineRendererPrefab;

		// Token: 0x0400092E RID: 2350
		public GameObject beamStartPrefab;

		// Token: 0x0400092F RID: 2351
		public GameObject beamEndPrefab;

		// Token: 0x04000930 RID: 2352
		private GameObject beamStart;

		// Token: 0x04000931 RID: 2353
		private GameObject beamEnd;

		// Token: 0x04000932 RID: 2354
		private GameObject beam;

		// Token: 0x04000933 RID: 2355
		private LineRenderer line;

		// Token: 0x04000934 RID: 2356
		[Header("Beam Options")]
		public bool alwaysOn = true;

		// Token: 0x04000935 RID: 2357
		public bool beamCollides = true;

		// Token: 0x04000936 RID: 2358
		public float beamLength = 100f;

		// Token: 0x04000937 RID: 2359
		public float beamEndOffset;

		// Token: 0x04000938 RID: 2360
		public float textureScrollSpeed;

		// Token: 0x04000939 RID: 2361
		public float textureLengthScale = 1f;
	}
}
