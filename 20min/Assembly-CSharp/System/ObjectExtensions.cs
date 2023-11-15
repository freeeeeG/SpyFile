using System;
using System.ArrayExtensions;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	// Token: 0x0200004E RID: 78
	public static class ObjectExtensions
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0001565F File Offset: 0x0001385F
		public static bool IsPrimitive(this Type type)
		{
			return type == typeof(string) || (type.IsValueType & type.IsPrimitive);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015682 File Offset: 0x00013882
		public static object Copy(this object originalObject)
		{
			return ObjectExtensions.InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015694 File Offset: 0x00013894
		private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
		{
			if (originalObject == null)
			{
				return null;
			}
			Type type = originalObject.GetType();
			if (type.IsPrimitive())
			{
				return originalObject;
			}
			if (visited.ContainsKey(originalObject))
			{
				return visited[originalObject];
			}
			if (typeof(Delegate).IsAssignableFrom(type))
			{
				return null;
			}
			object obj = ObjectExtensions.CloneMethod.Invoke(originalObject, null);
			if (type.IsArray && !type.GetElementType().IsPrimitive())
			{
				Array clonedArray = (Array)obj;
				clonedArray.ForEach(delegate(Array array, int[] indices)
				{
					array.SetValue(ObjectExtensions.InternalCopy(clonedArray.GetValue(indices), visited), indices);
				});
			}
			visited.Add(originalObject, obj);
			ObjectExtensions.CopyFields(originalObject, visited, obj, type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null);
			ObjectExtensions.RecursiveCopyBaseTypePrivateFields(originalObject, visited, obj, type);
			return obj;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015774 File Offset: 0x00013974
		private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if (typeToReflect.BaseType != null)
			{
				ObjectExtensions.RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
				ObjectExtensions.CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, (FieldInfo info) => info.IsPrivate);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000157CC File Offset: 0x000139CC
		private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
			{
				if ((filter == null || filter(fieldInfo)) && !fieldInfo.FieldType.IsPrimitive())
				{
					object value = ObjectExtensions.InternalCopy(fieldInfo.GetValue(originalObject), visited);
					fieldInfo.SetValue(cloneObject, value);
				}
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00015825 File Offset: 0x00013A25
		public static T Copy<T>(this T original)
		{
			return (T)((object)original.Copy());
		}

		// Token: 0x04000218 RID: 536
		private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
