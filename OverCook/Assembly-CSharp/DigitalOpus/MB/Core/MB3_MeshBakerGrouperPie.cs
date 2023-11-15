using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000060 RID: 96
	[Serializable]
	public class MB3_MeshBakerGrouperPie : MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0001B83E File Offset: 0x00019C3E
		public MB3_MeshBakerGrouperPie(GrouperData data)
		{
			this.d = data;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0001B850 File Offset: 0x00019C50
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			if (this.d.pieNumSegments == 0)
			{
				Debug.LogError("pieNumSegments must be greater than zero.");
				return dictionary;
			}
			if (this.d.pieAxis.magnitude <= 1E-06f)
			{
				Debug.LogError("Pie axis must have length greater than zero.");
				return dictionary;
			}
			this.d.pieAxis.Normalize();
			Quaternion rotation = Quaternion.FromToRotation(this.d.pieAxis, Vector3.up);
			Debug.Log("Collecting renderers in each cell");
			foreach (GameObject gameObject in selection)
			{
				if (!(gameObject == null))
				{
					GameObject gameObject2 = gameObject;
					Renderer component = gameObject2.GetComponent<Renderer>();
					if (component is MeshRenderer || component is SkinnedMeshRenderer)
					{
						Vector3 point = component.bounds.center - this.d.origin;
						point.Normalize();
						point = rotation * point;
						float num;
						if (Mathf.Abs(point.x) < 0.0001f && Mathf.Abs(point.z) < 0.0001f)
						{
							num = 0f;
						}
						else
						{
							num = Mathf.Atan2(point.x, point.z) * 57.29578f;
							if (num < 0f)
							{
								num = 360f + num;
							}
						}
						int num2 = Mathf.FloorToInt(num / 360f * (float)this.d.pieNumSegments);
						string key = "seg_" + num2;
						List<Renderer> list;
						if (dictionary.ContainsKey(key))
						{
							list = dictionary[key];
						}
						else
						{
							list = new List<Renderer>();
							dictionary.Add(key, list);
						}
						if (!list.Contains(component))
						{
							list.Add(component);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0001BA74 File Offset: 0x00019E74
		public override void DrawGizmos(Bounds sourceObjectBounds)
		{
			if (this.d.pieAxis.magnitude < 0.1f)
			{
				return;
			}
			if (this.d.pieNumSegments < 1)
			{
				return;
			}
			float magnitude = sourceObjectBounds.extents.magnitude;
			MB3_MeshBakerGrouperPie.DrawCircle(this.d.pieAxis, this.d.origin, magnitude, 24);
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, this.d.pieAxis);
			Quaternion rotation2 = Quaternion.AngleAxis(180f / (float)this.d.pieNumSegments, Vector3.up);
			Vector3 point = Vector3.forward;
			for (int i = 0; i < this.d.pieNumSegments; i++)
			{
				Vector3 a = rotation * point;
				Gizmos.DrawLine(this.d.origin, this.d.origin + a * magnitude);
				point = rotation2 * point;
				point = rotation2 * point;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0001BB7C File Offset: 0x00019F7C
		public static void DrawCircle(Vector3 axis, Vector3 center, float radius, int subdiv)
		{
			Quaternion rotation = Quaternion.AngleAxis((float)(360 / subdiv), axis);
			Vector3 vector = new Vector3(axis.y, -axis.x, axis.z);
			vector.Normalize();
			vector *= radius;
			for (int i = 0; i < subdiv + 1; i++)
			{
				Vector3 vector2 = rotation * vector;
				Gizmos.DrawLine(center + vector, center + vector2);
				vector = vector2;
			}
		}
	}
}
