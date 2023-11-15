using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x0200000F RID: 15
	public class Camera_Controller : MonoBehaviour
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00002D34 File Offset: 0x00000F34
		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				this.freeze = !this.freeze;
			}
			if (this.freeze)
			{
				return;
			}
			if (Input.GetKey(KeyCode.M))
			{
				this.freezeMouse = !this.freezeMouse;
			}
			if (!this.freezeMouse)
			{
				this.Mouse_Location = Input.mousePosition - this.Mouse_Location;
				this.Mouse_Location = new Vector3(-this.Mouse_Location.y * this.Camera_Sensitivity, this.Mouse_Location.x * this.Camera_Sensitivity, 0f);
				this.Mouse_Location = new Vector3(base.transform.eulerAngles.x + this.Mouse_Location.x, base.transform.eulerAngles.y + this.Mouse_Location.y, 0f);
				base.transform.eulerAngles = this.Mouse_Location;
				this.Mouse_Location = Input.mousePosition;
			}
			Vector3 vector = this.GetBaseInput();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.Total_Speed += Time.deltaTime;
				vector = vector * this.Total_Speed * this.Shift_Speed;
				vector.x = Mathf.Clamp(vector.x, -this.Speed_Cap, this.Speed_Cap);
				vector.y = Mathf.Clamp(vector.y, -this.Speed_Cap, this.Speed_Cap);
				vector.z = Mathf.Clamp(vector.z, -this.Speed_Cap, this.Speed_Cap);
			}
			else
			{
				this.Total_Speed = Mathf.Clamp(this.Total_Speed * 0.5f, 1f, 1000f);
				vector *= this.Normal_Speed;
			}
			vector *= Time.deltaTime;
			Vector3 position = base.transform.position;
			if (Input.GetKey(KeyCode.Space))
			{
				base.transform.Translate(vector);
				position.x = base.transform.position.x;
				position.z = base.transform.position.z;
				base.transform.position = position;
				return;
			}
			base.transform.Translate(vector);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002F78 File Offset: 0x00001178
		private Vector3 GetBaseInput()
		{
			Vector3 a = default(Vector3);
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			return a + new Vector3(axis, 0f, 0f) + new Vector3(0f, 0f, axis2);
		}

		// Token: 0x0400002C RID: 44
		public float Normal_Speed = 25f;

		// Token: 0x0400002D RID: 45
		public float Shift_Speed = 54f;

		// Token: 0x0400002E RID: 46
		public float Speed_Cap = 54f;

		// Token: 0x0400002F RID: 47
		public float Camera_Sensitivity = 0.6f;

		// Token: 0x04000030 RID: 48
		private Vector3 Mouse_Location = new Vector3(255f, 255f, 255f);

		// Token: 0x04000031 RID: 49
		private float Total_Speed = 1f;

		// Token: 0x04000032 RID: 50
		public bool freeze;

		// Token: 0x04000033 RID: 51
		public bool freezeMouse = true;
	}
}
