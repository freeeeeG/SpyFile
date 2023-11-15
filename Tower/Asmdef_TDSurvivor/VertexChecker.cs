using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class VertexChecker : MonoBehaviour
{
	// Token: 0x06000B7F RID: 2943 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
	[ContextMenu("Check Vertex Count")]
	public void CheckVertexCount()
	{
		List<KeyValuePair<Renderer, int>> list = new List<KeyValuePair<Renderer, int>>();
		foreach (Renderer renderer in Object.FindObjectsOfType<Renderer>())
		{
			if (renderer is MeshRenderer)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component && component.sharedMesh)
				{
					list.Add(new KeyValuePair<Renderer, int>(renderer, component.sharedMesh.vertexCount));
				}
			}
			else if (renderer is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				if (skinnedMeshRenderer.sharedMesh)
				{
					list.Add(new KeyValuePair<Renderer, int>(renderer, skinnedMeshRenderer.sharedMesh.vertexCount));
				}
			}
		}
		foreach (KeyValuePair<Renderer, int> keyValuePair in (from x in list
		orderby x.Value descending
		select x).Take(30).ToList<KeyValuePair<Renderer, int>>())
		{
			string arg = "";
			if (keyValuePair.Key is MeshRenderer)
			{
				MeshFilter component2 = keyValuePair.Key.GetComponent<MeshFilter>();
				if (component2 && component2.sharedMesh)
				{
					arg = component2.sharedMesh.name;
				}
			}
			else if (keyValuePair.Key is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedMeshRenderer2 = keyValuePair.Key as SkinnedMeshRenderer;
				if (skinnedMeshRenderer2.sharedMesh)
				{
					arg = skinnedMeshRenderer2.sharedMesh.name;
				}
			}
			Debug.Log(string.Format("Object Name: {0}, Vertex Count: {1}, Mesh Name: {2}", keyValuePair.Key.gameObject.name, keyValuePair.Value, arg));
		}
	}
}
