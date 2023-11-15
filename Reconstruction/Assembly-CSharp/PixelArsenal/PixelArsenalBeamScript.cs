using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelArsenal
{
	// Token: 0x020002AB RID: 683
	public class PixelArsenalBeamScript : MonoBehaviour
	{
		// Token: 0x060010DA RID: 4314 RVA: 0x0002EFAC File Offset: 0x0002D1AC
		private void Start()
		{
			if (this.textBeamName)
			{
				this.textBeamName.text = this.beamLineRendererPrefab[this.currentBeam].name;
			}
			if (this.endOffSetSlider)
			{
				this.endOffSetSlider.value = this.beamEndOffset;
			}
			if (this.scrollSpeedSlider)
			{
				this.scrollSpeedSlider.value = this.textureScrollSpeed;
			}
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0002F020 File Offset: 0x0002D220
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.beamStart = Object.Instantiate<GameObject>(this.beamStartPrefab[this.currentBeam], new Vector3(0f, 0f, 0f), Quaternion.identity);
				this.beamEnd = Object.Instantiate<GameObject>(this.beamEndPrefab[this.currentBeam], new Vector3(0f, 0f, 0f), Quaternion.identity);
				this.beam = Object.Instantiate<GameObject>(this.beamLineRendererPrefab[this.currentBeam], new Vector3(0f, 0f, 0f), Quaternion.identity);
				this.line = this.beam.GetComponent<LineRenderer>();
			}
			if (Input.GetMouseButtonUp(0))
			{
				Object.Destroy(this.beamStart);
				Object.Destroy(this.beamEnd);
				Object.Destroy(this.beam);
			}
			if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray.origin, ray.direction, out raycastHit))
				{
					Vector3 dir = raycastHit.point - base.transform.position;
					this.ShootBeamInDir(base.transform.position, dir);
				}
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.nextBeam();
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				this.nextBeam();
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				this.previousBeam();
				return;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.previousBeam();
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0002F1B0 File Offset: 0x0002D3B0
		public void nextBeam()
		{
			if (this.currentBeam < this.beamLineRendererPrefab.Length - 1)
			{
				this.currentBeam++;
			}
			else
			{
				this.currentBeam = 0;
			}
			if (this.textBeamName)
			{
				this.textBeamName.text = this.beamLineRendererPrefab[this.currentBeam].name;
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0002F210 File Offset: 0x0002D410
		public void previousBeam()
		{
			if (this.currentBeam > 0)
			{
				this.currentBeam--;
			}
			else
			{
				this.currentBeam = this.beamLineRendererPrefab.Length - 1;
			}
			if (this.textBeamName)
			{
				this.textBeamName.text = this.beamLineRendererPrefab[this.currentBeam].name;
			}
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0002F270 File Offset: 0x0002D470
		public void UpdateEndOffset()
		{
			this.beamEndOffset = this.endOffSetSlider.value;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0002F283 File Offset: 0x0002D483
		public void UpdateScrollSpeed()
		{
			this.textureScrollSpeed = this.scrollSpeedSlider.value;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0002F298 File Offset: 0x0002D498
		private void ShootBeamInDir(Vector3 start, Vector3 dir)
		{
			this.line.positionCount = 2;
			this.line.SetPosition(0, start);
			this.beamStart.transform.position = start;
			Vector3 vector = Vector3.zero;
			RaycastHit raycastHit;
			if (Physics.Raycast(start, dir, out raycastHit))
			{
				vector = raycastHit.point - dir.normalized * this.beamEndOffset;
			}
			else
			{
				vector = base.transform.position + dir * 100f;
			}
			this.beamEnd.transform.position = vector;
			this.line.SetPosition(1, vector);
			this.beamStart.transform.LookAt(this.beamEnd.transform.position);
			this.beamEnd.transform.LookAt(this.beamStart.transform.position);
			float num = Vector3.Distance(start, vector);
			this.line.sharedMaterial.mainTextureScale = new Vector2(num / this.textureLengthScale, 1f);
			this.line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * this.textureScrollSpeed, 0f);
		}

		// Token: 0x040008FD RID: 2301
		[Header("Prefabs")]
		public GameObject[] beamLineRendererPrefab;

		// Token: 0x040008FE RID: 2302
		public GameObject[] beamStartPrefab;

		// Token: 0x040008FF RID: 2303
		public GameObject[] beamEndPrefab;

		// Token: 0x04000900 RID: 2304
		private int currentBeam;

		// Token: 0x04000901 RID: 2305
		private GameObject beamStart;

		// Token: 0x04000902 RID: 2306
		private GameObject beamEnd;

		// Token: 0x04000903 RID: 2307
		private GameObject beam;

		// Token: 0x04000904 RID: 2308
		private LineRenderer line;

		// Token: 0x04000905 RID: 2309
		[Header("Adjustable Variables")]
		public float beamEndOffset = 1f;

		// Token: 0x04000906 RID: 2310
		public float textureScrollSpeed = 8f;

		// Token: 0x04000907 RID: 2311
		public float textureLengthScale = 3f;

		// Token: 0x04000908 RID: 2312
		[Header("Put Sliders here (Optional)")]
		public Slider endOffSetSlider;

		// Token: 0x04000909 RID: 2313
		public Slider scrollSpeedSlider;

		// Token: 0x0400090A RID: 2314
		[Header("Put UI Text object here to show beam name")]
		public Text textBeamName;
	}
}
