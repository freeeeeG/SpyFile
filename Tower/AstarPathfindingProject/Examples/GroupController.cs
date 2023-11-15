using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000E1 RID: 225
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_group_controller.php")]
	public class GroupController : MonoBehaviour
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x0003EEC8 File Offset: 0x0003D0C8
		public void Start()
		{
			this.cam = Camera.main;
			RVOSimulator active = RVOSimulator.active;
			if (active == null)
			{
				base.enabled = false;
				throw new Exception("No RVOSimulator in the scene. Please add one");
			}
			this.sim = active.GetSimulator();
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003EF10 File Offset: 0x0003D110
		public void Update()
		{
			if (this.adjustCamera)
			{
				List<Agent> agents = this.sim.GetAgents();
				float num = 0f;
				for (int i = 0; i < agents.Count; i++)
				{
					float num2 = Mathf.Max(Mathf.Abs(agents[i].Position.x), Mathf.Abs(agents[i].Position.y));
					if (num2 > num)
					{
						num = num2;
					}
				}
				float a = num / Mathf.Tan(this.cam.fieldOfView * 0.017453292f / 2f);
				float b = num / Mathf.Tan(Mathf.Atan(Mathf.Tan(this.cam.fieldOfView * 0.017453292f / 2f) * this.cam.aspect));
				float num3 = Mathf.Max(a, b) * 1.1f;
				num3 = Mathf.Max(num3, 20f);
				num3 = Mathf.Min(num3, this.cam.farClipPlane - 1f);
				this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, new Vector3(0f, num3, 0f), Time.smoothDeltaTime * 2f);
			}
			if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.Mouse0))
			{
				this.Order();
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0003F06C File Offset: 0x0003D26C
		private void OnGUI()
		{
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && !Input.GetKey(KeyCode.A))
			{
				this.Select(this.start, this.end);
				this.wasDown = false;
			}
			if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
			{
				this.end = Event.current.mousePosition;
				if (!this.wasDown)
				{
					this.start = this.end;
					this.wasDown = true;
				}
			}
			if (Input.GetKey(KeyCode.A))
			{
				this.wasDown = false;
			}
			if (this.wasDown)
			{
				Rect position = Rect.MinMaxRect(Mathf.Min(this.start.x, this.end.x), Mathf.Min(this.start.y, this.end.y), Mathf.Max(this.start.x, this.end.x), Mathf.Max(this.start.y, this.end.y));
				if (position.width > 4f && position.height > 4f)
				{
					GUI.Box(position, "", this.selectionBox);
				}
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003F1B4 File Offset: 0x0003D3B4
		public void Order()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				float num = 0f;
				for (int i = 0; i < this.selection.Count; i++)
				{
					num += this.selection[i].GetComponent<RVOController>().radius;
				}
				float num2 = num / 3.1415927f;
				num2 *= 2f;
				for (int j = 0; j < this.selection.Count; j++)
				{
					float num3 = 6.2831855f * (float)j / (float)this.selection.Count;
					Vector3 target = raycastHit.point + new Vector3(Mathf.Cos(num3), 0f, Mathf.Sin(num3)) * num2;
					this.selection[j].SetTarget(target);
					this.selection[j].SetColor(this.GetColor(num3));
					this.selection[j].RecalculatePath();
				}
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003F2C8 File Offset: 0x0003D4C8
		public void Select(Vector2 _start, Vector2 _end)
		{
			_start.y = (float)Screen.height - _start.y;
			_end.y = (float)Screen.height - _end.y;
			Vector2 vector = Vector2.Min(_start, _end);
			Vector2 vector2 = Vector2.Max(_start, _end);
			if ((vector2 - vector).sqrMagnitude < 16f)
			{
				return;
			}
			this.selection.Clear();
			RVOExampleAgent[] array = Object.FindObjectsOfType(typeof(RVOExampleAgent)) as RVOExampleAgent[];
			for (int i = 0; i < array.Length; i++)
			{
				Vector2 vector3 = this.cam.WorldToScreenPoint(array[i].transform.position);
				if (vector3.x > vector.x && vector3.y > vector.y && vector3.x < vector2.x && vector3.y < vector2.y)
				{
					this.selection.Add(array[i]);
				}
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0003F3C1 File Offset: 0x0003D5C1
		public Color GetColor(float angle)
		{
			return AstarMath.HSVToRGB(angle * 57.295776f, 0.8f, 0.6f);
		}

		// Token: 0x040005BE RID: 1470
		public GUIStyle selectionBox;

		// Token: 0x040005BF RID: 1471
		public bool adjustCamera = true;

		// Token: 0x040005C0 RID: 1472
		private Vector2 start;

		// Token: 0x040005C1 RID: 1473
		private Vector2 end;

		// Token: 0x040005C2 RID: 1474
		private bool wasDown;

		// Token: 0x040005C3 RID: 1475
		private List<RVOExampleAgent> selection = new List<RVOExampleAgent>();

		// Token: 0x040005C4 RID: 1476
		private Simulator sim;

		// Token: 0x040005C5 RID: 1477
		private Camera cam;

		// Token: 0x040005C6 RID: 1478
		private const float rad2Deg = 57.295776f;
	}
}
