using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class DevToolSceneInspector : DevTool
{
	// Token: 0x060021A3 RID: 8611 RVA: 0x000B68E4 File Offset: 0x000B4AE4
	public DevToolSceneInspector()
	{
		this.drawFlags = ImGuiWindowFlags.MenuBar;
		this.CustomTypeViews = new Dictionary<Type, DevToolSceneInspector.ViewInfo>
		{
			{
				typeof(GameObject),
				new DevToolSceneInspector.ViewInfo("Components", delegate(object o, string f)
				{
					this.CustomGameObjectDisplay(o, f);
				})
			},
			{
				typeof(KPrefabID),
				new DevToolSceneInspector.ViewInfo("Prefab tags", delegate(object o, string f)
				{
					this.CustomPrefabTagView(o, f);
				})
			}
		};
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x000B696B File Offset: 0x000B4B6B
	public static void Inspect(object obj)
	{
		DevToolManager.Instance.panels.AddOrGetDevTool<DevToolSceneInspector>().PushObject(obj);
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000B6984 File Offset: 0x000B4B84
	public void PushObject(object obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.StackIndex >= 0 && this.StackIndex < this.Stack.Count && obj == this.Stack[this.StackIndex].Obj)
		{
			return;
		}
		if (this.Stack.Count > this.StackIndex + 1)
		{
			this.Stack.RemoveRange(this.StackIndex + 1, this.Stack.Count - (this.StackIndex + 1));
		}
		DevToolSceneInspector.StackItem stackItem = new DevToolSceneInspector.StackItem();
		stackItem.Obj = obj;
		stackItem.Filter = "";
		this.Stack.Add(stackItem);
		this.StackIndex++;
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000B6A3C File Offset: 0x000B4C3C
	protected override void RenderTo(DevPanel panel)
	{
		for (int i = this.Stack.Count - 1; i >= 0; i--)
		{
			if (this.Stack[i].Obj.IsNullOrDestroyed())
			{
				this.Stack.RemoveAt(i);
				if (this.StackIndex >= i)
				{
					this.StackIndex--;
				}
			}
		}
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("Utils"))
			{
				if (ImGui.MenuItem("Goto current selection"))
				{
					SelectTool instance = SelectTool.Instance;
					UnityEngine.Object x;
					if (instance == null)
					{
						x = null;
					}
					else
					{
						KSelectable selected = instance.selected;
						x = ((selected != null) ? selected.gameObject : null);
					}
					if (x != null)
					{
						SelectTool instance2 = SelectTool.Instance;
						object obj;
						if (instance2 == null)
						{
							obj = null;
						}
						else
						{
							KSelectable selected2 = instance2.selected;
							obj = ((selected2 != null) ? selected2.gameObject : null);
						}
						this.PushObject(obj);
					}
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenuBar();
		}
		if (ImGui.Button(" < ") && this.StackIndex > 0)
		{
			this.StackIndex--;
		}
		ImGui.SameLine();
		if (ImGui.Button(" > ") && this.StackIndex + 1 < this.Stack.Count)
		{
			this.StackIndex++;
		}
		if (this.Stack.Count == 0)
		{
			ImGui.Text("No Selection.");
			return;
		}
		DevToolSceneInspector.StackItem stackItem = this.Stack[this.StackIndex];
		object obj2 = stackItem.Obj;
		Type type = obj2.GetType();
		ImGui.LabelText("Type", type.Name);
		if (ImGui.Button("Clear"))
		{
			stackItem.Filter = "";
		}
		ImGui.SameLine();
		ImGui.InputText("Filter", ref stackItem.Filter, 64U);
		ImGui.PushID(this.StackIndex);
		if (ImGui.BeginTabBar("##tabs", ImGuiTabBarFlags.None))
		{
			DevToolSceneInspector.ViewInfo viewInfo;
			if (this.CustomTypeViews.TryGetValue(type, out viewInfo) && ImGui.BeginTabItem(viewInfo.Name))
			{
				viewInfo.Callback(obj2, stackItem.Filter);
				ImGui.EndTabItem();
			}
			if (ImGui.BeginTabItem("Raw view"))
			{
				ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), true, ImGuiWindowFlags.None);
				if (obj2 is IEnumerable)
				{
					IEnumerator enumerator = (obj2 as IEnumerable).GetEnumerator();
					int num = 0;
					while (enumerator.MoveNext())
					{
						object obj3 = enumerator.Current;
						this.DisplayField("[" + num.ToString() + "]", obj3.GetType(), ref obj3);
						num++;
					}
				}
				else
				{
					foreach (FieldInfo fieldInfo in type.GetFields())
					{
						object value = fieldInfo.GetValue(obj2);
						Type fieldType = fieldInfo.FieldType;
						if (fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0 && (!(stackItem.Filter != "") || fieldInfo.Name.IndexOf(stackItem.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1 || fieldType.Name.IndexOf(stackItem.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1) && this.DisplayField(fieldInfo.Name, fieldType, ref value) && !fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
						{
							fieldInfo.SetValue(obj2, value);
						}
					}
					BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
					foreach (PropertyInfo propertyInfo in type.GetProperties(bindingAttr))
					{
						if (!propertyInfo.CanRead)
						{
							ImGui.LabelText(propertyInfo.Name, "Unreadable");
						}
						else if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0)
						{
							Type propertyType = propertyInfo.PropertyType;
							object value2 = propertyInfo.GetValue(obj2);
							if ((!(stackItem.Filter != "") || propertyInfo.Name.IndexOf(stackItem.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1 || propertyType.Name.IndexOf(stackItem.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1) && this.DisplayField(propertyInfo.Name, propertyType, ref value2) && propertyInfo.CanWrite)
							{
								propertyInfo.SetValue(obj2, value2);
							}
						}
					}
				}
				ImGui.EndChild();
				ImGui.EndTabItem();
			}
			ImGui.EndTabBar();
		}
		ImGui.PopID();
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000B6E70 File Offset: 0x000B5070
	private bool DisplayField(string name, Type ft, ref object obj)
	{
		bool result = false;
		if (obj == null)
		{
			ImGui.LabelText(name, "null");
		}
		else if (ft == typeof(int))
		{
			int num = (int)obj;
			if (ImGui.InputInt(name, ref num))
			{
				obj = num;
				result = true;
			}
		}
		else if (ft == typeof(uint))
		{
			int val = (int)((uint)obj);
			if (ImGui.InputInt(name, ref val))
			{
				obj = (uint)Math.Max(val, 0);
				result = true;
			}
		}
		else if (ft == typeof(bool))
		{
			bool flag = (bool)obj;
			if (ImGui.Checkbox(name, ref flag))
			{
				obj = flag;
				result = true;
			}
		}
		else if (ft == typeof(float))
		{
			float num2 = (float)obj;
			if (ImGui.InputFloat(name, ref num2))
			{
				obj = num2;
				result = true;
			}
		}
		else if (ft == typeof(Vector2))
		{
			Vector2 vector = (Vector2)obj;
			if (ImGui.InputFloat2(name, ref vector))
			{
				obj = vector;
				result = true;
			}
		}
		else if (ft == typeof(Vector3))
		{
			Vector3 vector2 = (Vector3)obj;
			if (ImGui.InputFloat3(name, ref vector2))
			{
				obj = vector2;
				result = true;
			}
		}
		else if (ft == typeof(string))
		{
			string text = (string)obj;
			if (ImGui.InputText(name, ref text, 256U))
			{
				obj = text;
				result = true;
			}
		}
		else if (ImGui.Selectable(name + " (" + ft.Name + ")", false, ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
		{
			this.PushObject(obj);
		}
		return result;
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000B7040 File Offset: 0x000B5240
	private void CustomGameObjectDisplay(object obj, string filter)
	{
		GameObject gameObject = (GameObject)obj;
		ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), true, ImGuiWindowFlags.None);
		int num = 0;
		foreach (Behaviour behaviour in gameObject.GetComponents<Behaviour>())
		{
			Type type = behaviour.GetType();
			if (!(filter != "") || type.Name.IndexOf(filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				ImGui.PushID(num++);
				bool enabled = behaviour.enabled;
				if (ImGui.Checkbox("", ref enabled))
				{
					behaviour.enabled = enabled;
				}
				ImGui.PopID();
				ImGui.SameLine();
				if (ImGui.Selectable(type.Name, false, ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
				{
					this.PushObject(behaviour);
				}
			}
		}
		ImGui.EndChild();
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000B710C File Offset: 0x000B530C
	private void CustomPrefabTagView(object obj, string filter)
	{
		KPrefabID kprefabID = (KPrefabID)obj;
		ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), true, ImGuiWindowFlags.None);
		string name = kprefabID.PrefabTag.Name;
		ImGui.InputText("PrefabID: ", ref name, 128U);
		int num = 0;
		foreach (Tag tag in kprefabID.Tags)
		{
			string name2 = tag.Name;
			if (!(filter != "") || name2.IndexOf(filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				ImGui.InputText("[" + num.ToString() + "]", ref name2, 128U);
				num++;
			}
		}
		ImGui.EndChild();
	}

	// Token: 0x04001314 RID: 4884
	private List<DevToolSceneInspector.StackItem> Stack = new List<DevToolSceneInspector.StackItem>();

	// Token: 0x04001315 RID: 4885
	private int StackIndex = -1;

	// Token: 0x04001316 RID: 4886
	private Dictionary<Type, DevToolSceneInspector.ViewInfo> CustomTypeViews;

	// Token: 0x020011F9 RID: 4601
	private class StackItem
	{
		// Token: 0x04005E18 RID: 24088
		public object Obj;

		// Token: 0x04005E19 RID: 24089
		public string Filter;
	}

	// Token: 0x020011FA RID: 4602
	private class ViewInfo
	{
		// Token: 0x06007B58 RID: 31576 RVA: 0x002DDC90 File Offset: 0x002DBE90
		public ViewInfo(string s, Action<object, string> a)
		{
			this.Name = s;
			this.Callback = a;
		}

		// Token: 0x04005E1A RID: 24090
		public string Name;

		// Token: 0x04005E1B RID: 24091
		public Action<object, string> Callback;
	}
}
