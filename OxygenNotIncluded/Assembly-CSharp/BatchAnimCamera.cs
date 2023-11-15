using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class BatchAnimCamera : MonoBehaviour
{
	// Token: 0x060016FF RID: 5887 RVA: 0x00076F66 File Offset: 0x00075166
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x00076F74 File Offset: 0x00075174
	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		this.ClampToBounds();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.do_pan = true;
				this.last_pan = KInputManager.GetMousePos();
			}
			else if (Input.GetMouseButton(0) && this.do_pan)
			{
				Vector3 vector = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
				Vector3 translation = new Vector3(vector.x * BatchAnimCamera.pan_speed, vector.y * BatchAnimCamera.pan_speed, 0f);
				base.transform.Translate(translation, Space.World);
				this.ClampToBounds();
				this.last_pan = KInputManager.GetMousePos();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.do_pan = false;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x00077178 File Offset: 0x00075378
	private void ClampToBounds()
	{
		Vector3 position = base.transform.GetPosition();
		position.x = Mathf.Clamp(base.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x);
		position.y = Mathf.Clamp(base.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y);
		position.z = Mathf.Clamp(base.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z);
		base.transform.SetPosition(position);
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x0007724C File Offset: 0x0007544C
	private void OnDrawGizmosSelected()
	{
		DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red, 0f, true);
	}

	// Token: 0x04000CC2 RID: 3266
	private static readonly float pan_speed = 5f;

	// Token: 0x04000CC3 RID: 3267
	private static readonly float zoom_speed = 5f;

	// Token: 0x04000CC4 RID: 3268
	public static Bounds bounds = new Bounds(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));

	// Token: 0x04000CC5 RID: 3269
	private float zoom_min = 1f;

	// Token: 0x04000CC6 RID: 3270
	private float zoom_max = 100f;

	// Token: 0x04000CC7 RID: 3271
	private Camera cam;

	// Token: 0x04000CC8 RID: 3272
	private bool do_pan;

	// Token: 0x04000CC9 RID: 3273
	private Vector3 last_pan;
}
