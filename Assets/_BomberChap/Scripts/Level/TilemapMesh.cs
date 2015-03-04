using UnityEngine;
using System;
using System.Collections;

namespace BomberChap
{
	public class TilemapMesh : IDisposable
	{
		public Vector3[] vertices;
		public Vector2[] uvs;
		public int[] triangles;

		private MeshFilter m_meshFilter;
		private Mesh m_mesh;
		private bool m_isDisposed;

		public TilemapMesh(MeshFilter meshFilter)
		{
			m_meshFilter = meshFilter;
			m_mesh = new Mesh();
			m_mesh.name = "Tilemap Mesh";
			m_meshFilter.sharedMesh = m_mesh;
			m_isDisposed = false;
		}

		public void UpdateMesh()
		{
			if(!m_isDisposed)
			{
				m_mesh.Clear();
				m_mesh.vertices = vertices;
				m_mesh.uv = uvs;
				m_mesh.triangles = triangles;
				m_mesh.RecalculateNormals();
				m_mesh.RecalculateBounds();
			}
		}

		public void Dispose()
		{
			if(!m_isDisposed)
			{
#if UNITY_EDITOR
				if(UnityEditor.EditorApplication.isPlaying)
					Mesh.Destroy(m_mesh);
				else
					Mesh.DestroyImmediate(m_mesh);
#else
				Mesh.Destroy(m_mesh);
#endif
				m_mesh = null;
				m_meshFilter.sharedMesh = null;
				m_isDisposed = true;
			}
		}
	}
}