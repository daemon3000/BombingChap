//	Distributed under the terms of an MIT-style license:
//
//	The MIT License
//
//	Copyright (c) 2014-2016 Cristian Alexandru Geambasu(daemon3000@hotmail.com)
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//	and associated documentation files (the "Software"), to deal in the Software without restriction, 
//	including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all copies or substantial 
//	portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

//	Generates a type safe representation of the tags and layers you can set up in the project settings.
//
//	How To Install:
//		Put this file somewhere in an 'Editor' folder.
//
//	How To Use:
//		Go to the 'Window/Tags and Layers' and select one of the two options. The tool will then generate
//		two new scripts in the plugins folder which contain two static classes.
//	
//	Scripting:
//		if(collider.tag == Tags.Player) {
//			//	do something
//		}
//	
//		if(gameObject.layer == Layers.Enemy) {
//			//	do something
//		}
public static class TagsAndLayersGenerator
{
	private const string INSTALL_FOLDER = "/Plugins/Misc/";

	[MenuItem("Window/Tags and Layers/Generate Tags File")]
	public static void GenerateTags()
	{
		StringBuilder builder = new StringBuilder();
		string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

		builder.Append("using System;\r\n\r\npublic static class Tags\r\n{\r\n\t");
		for(int i = 0; i < tags.Length; i++)
		{
			builder.Append("public const string ");
			builder.Append(tags[i].Replace(' ', '_'));
			builder.Append(" = \"");
			builder.Append(tags[i]);
			builder.Append("\";\r\n\t");
		}
		builder.Append("\r\n}");

		try
		{
			string installFolder = Application.dataPath + INSTALL_FOLDER;
			if(!Directory.Exists(installFolder))
				Directory.CreateDirectory(installFolder);

			using(var writer = System.IO.File.CreateText(installFolder + "Tags.cs"))
			{
				writer.Write(builder.ToString());
			}
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Success", "Tags file has been generated successfully!", "OK");
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
			EditorUtility.DisplayDialog("Error", "Failed to generate Tags file!", "OK");
		}
	}

	[MenuItem("Window/Tags and Layers/Generate Layers File")]
	public static void GenerateLayers()
	{
		StringBuilder builder = new StringBuilder();
		string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
		
		builder.Append("using System;\r\n\r\npublic static class Layers\r\n{\r\n\t");
		for(int i = 0; i < layers.Length; i++)
		{
			builder.Append("public const int ");
			builder.Append(layers[i].Replace(' ', '_'));
			builder.Append(" = ");
			builder.Append(LayerMask.NameToLayer(layers[i]));
			builder.Append(";\r\n\t");
		}
		builder.Append("\r\n}");

		try
		{
			string installFolder = Application.dataPath + INSTALL_FOLDER;
			if(!Directory.Exists(installFolder))
				Directory.CreateDirectory(installFolder);
			
			using(var writer = System.IO.File.CreateText(installFolder + "Layers.cs"))
			{
				writer.Write(builder.ToString());
			}
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Success", "Layers file has been generated successfully!", "OK");
		}
		catch(System.Exception ex)
		{
			Debug.LogException(ex);
			EditorUtility.DisplayDialog("Error", "Failed to generate Layers file!", "OK");
		}
	}
}