﻿	using UnityEngine;

	public static class SphereCreator
  {

		private static Vector3[] directions = {
			Vector3.left,
			Vector3.back,
			Vector3.right,
			Vector3.forward
		};

		public static Mesh Create (int subdivisions, float radius, float vertexChange)
    {
			int resolution = 1 << subdivisions;
			Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1) * 4 - (resolution * 2 - 1) * 3];
			int[] triangles = new int[(1 << (subdivisions * 2 + 3)) * 3];
			CreateOctahedron(vertices, triangles, resolution);

			Vector3[] normals = new Vector3[vertices.Length];
			Normalize(vertices, normals);

			Vector2[] uv = new Vector2[vertices.Length];
			// CreateUV(vertices, uv);

			Vector4[] tangents = new Vector4[vertices.Length];
			CreateTangents(vertices, tangents);

			if (radius != 1f) {
				for (int i = 0; i < vertices.Length; i++) {
					vertices[i] *= radius;
				}
			}

      for (int i = 0; i < vertices.Length; i++)
      {
            vertices[i] *= vertexChange* Random.Range(1f, 1.8f);
        }

			Mesh mesh = new Mesh();
			mesh.name = "Octahedron Sphere";
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.tangents = tangents;
			mesh.triangles = triangles;
			return mesh;
		}

		private static void CreateOctahedron (Vector3[] vertices, int[] triangles, int resolution) {
			int v = 0, vBottom = 0, t = 0;

			for (int i = 0; i < 4; i++) {
				vertices[v++] = Vector3.down;
			}

			for (int i = 1; i <= resolution; i++) {
				float progress = (float)i / resolution;
				Vector3 from, to;
				vertices[v++] = to = Vector3.Lerp(Vector3.down, Vector3.forward, progress);
				for (int d = 0; d < 4; d++) {
					from = to;
					to = Vector3.Lerp(Vector3.down, directions[d], progress);
					t = CreateLowerStrip(i, v, vBottom, t, triangles);
					v = CreateVertexLine(from, to, i, v, vertices);
					vBottom += i > 1 ? (i - 1) : 1;
				}
				vBottom = v - 1 - i * 4;
			}

			for (int i = resolution - 1; i >= 1; i--) {
				float progress = (float)i / resolution;
				Vector3 from, to;
				vertices[v++] = to = Vector3.Lerp(Vector3.up, Vector3.forward, progress);
				for (int d = 0; d < 4; d++) {
					from = to;
					to = Vector3.Lerp(Vector3.up, directions[d], progress);
					t = CreateUpperStrip(i, v, vBottom, t, triangles);
					v = CreateVertexLine(from, to, i, v, vertices);
					vBottom += i + 1;
				}
				vBottom = v - 1 - i * 4;
			}

			for (int i = 0; i < 4; i++) {
				triangles[t++] = vBottom;
				triangles[t++] = v;
				triangles[t++] = ++vBottom;
				vertices[v++] = Vector3.up;
			}
		}

		private static int CreateVertexLine (Vector3 from, Vector3 to, int steps, int v, Vector3[] vertices) {
			for (int i = 1; i <= steps; i++) {
				vertices[v++] = Vector3.Lerp(from, to, (float)i / steps);
			}
			return v;
		}

		private static int CreateLowerStrip (int steps, int vTop, int vBottom, int t, int[] triangles) {
			for (int i = 1; i < steps; i++) {
				triangles[t++] = vBottom;
				triangles[t++] = vTop - 1;
				triangles[t++] = vTop;

				triangles[t++] = vBottom++;
				triangles[t++] = vTop++;
				triangles[t++] = vBottom;
			}
			triangles[t++] = vBottom;
			triangles[t++] = vTop - 1;
			triangles[t++] = vTop;
			return t;
		}

		private static int CreateUpperStrip (int steps, int vTop, int vBottom, int t, int[] triangles) {
			triangles[t++] = vBottom;
			triangles[t++] = vTop - 1;
			triangles[t++] = ++vBottom;
			for (int i = 1; i <= steps; i++) {
				triangles[t++] = vTop - 1;
				triangles[t++] = vTop;
				triangles[t++] = vBottom;

				triangles[t++] = vBottom;
				triangles[t++] = vTop++;
				triangles[t++] = ++vBottom;
			}
			return t;
		}

		private static void Normalize (Vector3[] vertices, Vector3[] normals) {
			for (int i = 0; i < vertices.Length; i++) {
				normals[i] = vertices[i] = vertices[i].normalized;
			}
		}
		private static void CreateTangents (Vector3[] vertices, Vector4[] tangents) {
			for (int i = 0; i < vertices.Length; i++) {
				Vector3 v = vertices[i];
				v.y = 0f;
				v = v.normalized;
				Vector4 tangent;
				tangent.x = -v.z;
				tangent.y = 0f;
				tangent.z = v.x;
				tangent.w = -1f;
				tangents[i] = tangent;
			}

			tangents[vertices.Length - 4] = tangents[0] = new Vector3(-1f, 0, -1f).normalized;
			tangents[vertices.Length - 3] = tangents[1] = new Vector3(1f, 0f, -1f).normalized;
			tangents[vertices.Length - 2] = tangents[2] = new Vector3(1f, 0f, 1f).normalized;
			tangents[vertices.Length - 1] = tangents[3] = new Vector3(-1f, 0f, 1f).normalized;
			for (int i = 0; i < 4; i++) {
				tangents[vertices.Length - 1 - i].w = tangents[i].w = -1f;
			}
		}
	}
