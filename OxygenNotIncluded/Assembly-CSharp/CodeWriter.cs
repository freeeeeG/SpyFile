using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200036F RID: 879
public class CodeWriter
{
	// Token: 0x060011FB RID: 4603 RVA: 0x000613CF File Offset: 0x0005F5CF
	public CodeWriter(string path)
	{
		this.Path = path;
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x000613E9 File Offset: 0x0005F5E9
	public void Comment(string text)
	{
		this.Lines.Add("// " + text);
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x00061404 File Offset: 0x0005F604
	public void BeginPartialClass(string class_name, string parent_name = null)
	{
		string text = "public partial class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00061450 File Offset: 0x0005F650
	public void BeginClass(string class_name, string parent_name = null)
	{
		string text = "public class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x00061499 File Offset: 0x0005F699
	public void EndClass()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000614B4 File Offset: 0x0005F6B4
	public void BeginNameSpace(string name)
	{
		this.Line("namespace " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x000614E0 File Offset: 0x0005F6E0
	public void EndNameSpace()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x000614FB File Offset: 0x0005F6FB
	public void BeginArrayStructureInitialization(string name)
	{
		this.Line("new " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x00061527 File Offset: 0x0005F727
	public void EndArrayStructureInitialization(bool last_item)
	{
		this.Indent--;
		if (!last_item)
		{
			this.Line("},");
			return;
		}
		this.Line("}");
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x00061551 File Offset: 0x0005F751
	public void BeginArraArrayInitialization(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00061583 File Offset: 0x0005F783
	public void EndArrayArrayInitialization(bool last_item)
	{
		this.Indent--;
		if (last_item)
		{
			this.Line("}");
			return;
		}
		this.Line("},");
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x000615AD File Offset: 0x0005F7AD
	public void BeginConstructor(string name)
	{
		this.Line("public " + name + "()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x000615DE File Offset: 0x0005F7DE
	public void EndConstructor()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x000615F9 File Offset: 0x0005F7F9
	public void BeginArrayAssignment(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0006162B File Offset: 0x0005F82B
	public void EndArrayAssignment()
	{
		this.Indent--;
		this.Line("};");
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x00061646 File Offset: 0x0005F846
	public void FieldAssignment(string field_name, string value)
	{
		this.Line(field_name + " = " + value + ";");
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0006165F File Offset: 0x0005F85F
	public void BeginStructureDelegateFieldInitializer(string name)
	{
		this.Line(name + "=delegate()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0006168B File Offset: 0x0005F88B
	public void EndStructureDelegateFieldInitializer()
	{
		this.Indent--;
		this.Line("},");
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000616A6 File Offset: 0x0005F8A6
	public void BeginIf(string condition)
	{
		this.Line("if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x000616D8 File Offset: 0x0005F8D8
	public void BeginElseIf(string condition)
	{
		this.Indent--;
		this.Line("}");
		this.Line("else if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0006172D File Offset: 0x0005F92D
	public void EndIf()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x00061748 File Offset: 0x0005F948
	public void BeginFunctionDeclaration(string name, string parameter, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"(",
			parameter,
			")"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x000617AC File Offset: 0x0005F9AC
	public void BeginFunctionDeclaration(string name, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"()"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00061803 File Offset: 0x0005FA03
	public void EndFunctionDeclaration()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00061820 File Offset: 0x0005FA20
	private void InternalNamedParameter(string name, string value, bool last_parameter)
	{
		string str = "";
		if (!last_parameter)
		{
			str = ",";
		}
		this.Line(name + ":" + value + str);
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x0006184F File Offset: 0x0005FA4F
	public void NamedParameterBool(string name, bool value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString().ToLower(), last_parameter);
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00061865 File Offset: 0x0005FA65
	public void NamedParameterInt(string name, int value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString(), last_parameter);
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00061876 File Offset: 0x0005FA76
	public void NamedParameterFloat(string name, float value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString() + "f", last_parameter);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x00061891 File Offset: 0x0005FA91
	public void NamedParameterString(string name, string value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value, last_parameter);
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x0006189C File Offset: 0x0005FA9C
	public void BeginFunctionCall(string name)
	{
		this.Line(name);
		this.Line("(");
		this.Indent++;
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x000618BE File Offset: 0x0005FABE
	public void EndFunctionCall()
	{
		this.Indent--;
		this.Line(");");
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x000618DC File Offset: 0x0005FADC
	public void FunctionCall(string function_name, params string[] parameters)
	{
		string str = function_name + "(";
		for (int i = 0; i < parameters.Length; i++)
		{
			str += parameters[i];
			if (i != parameters.Length - 1)
			{
				str += ", ";
			}
		}
		this.Line(str + ");");
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00061932 File Offset: 0x0005FB32
	public void StructureFieldInitializer(string field, string value)
	{
		this.Line(field + " = " + value + ",");
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0006194C File Offset: 0x0005FB4C
	public void StructureArrayFieldInitializer(string field, string field_type, params string[] values)
	{
		string text = field + " = new " + field_type + "[]{ ";
		for (int i = 0; i < values.Length; i++)
		{
			text += values[i];
			if (i < values.Length - 1)
			{
				text += ", ";
			}
		}
		text += " },";
		this.Line(text);
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x000619AC File Offset: 0x0005FBAC
	public void Line(string text = "")
	{
		for (int i = 0; i < this.Indent; i++)
		{
			text = "\t" + text;
		}
		this.Lines.Add(text);
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000619E3 File Offset: 0x0005FBE3
	public void Flush()
	{
		File.WriteAllLines(this.Path, this.Lines.ToArray());
	}

	// Token: 0x040009C6 RID: 2502
	private List<string> Lines = new List<string>();

	// Token: 0x040009C7 RID: 2503
	private string Path;

	// Token: 0x040009C8 RID: 2504
	private int Indent;
}
