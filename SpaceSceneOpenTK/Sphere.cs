using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceSceneOpenTK
{
	public class Sphere : DrawableObject
	{
		private int _vertexBufferObject;
		private int _elementBufferObject;
		private int _vertexArray;


		public Sphere() : base()
		{
			CalcGeometry();
			CalcIndices();

			_texture = new Texture("container.png");
			//_texture = new Texture("checkboard.jpg");

			_vertexBufferObject = GL.GenBuffer();
			_elementBufferObject = GL.GenBuffer();
			_vertexArray = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		}
		public override void Draw()
		{
			base.Draw();
			
			GL.BindVertexArray(_vertexArray);
			GL.Color3(Color.White);
			GL.EnableVertexAttribArray(0);
			GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
			GL.DisableVertexAttribArray(0);
		}
		
		private void CalcGeometry()
		{
			List<float> vertices = new List<float>();
			List<float> normals = new List<float>();
			List<float> texCoords = new List<float>();

			uint sectorCount = 10;
			uint stackCount = 10;
			float radius = 1.0f;
			float x, y, z, xy;
			float nx, ny, nz, lengthInv = 1.0f / radius;
			float s, t;

			const float PI = (float)Math.PI;
			float sectorStep = 2.0f * PI / sectorCount;
			float stackStep = PI / stackCount;
			float sectorAngle, stackAngle;

			for (int i = 0; i <= stackCount; i++)
			{
				stackAngle = PI / 2.0f - i * stackStep;
				xy = radius * (float)Math.Cos((float)stackAngle);
				z = radius * (float)Math.Sin((float)stackAngle);
				for (int j = 0; j <= sectorCount; j++)
				{
					sectorAngle = j * sectorStep;

					x = xy * (float)Math.Cos((float)sectorAngle);
					y = xy * (float)Math.Sin((float)sectorAngle);
					vertices.AddRange(new float[] { x, y, z });

					nx = x * lengthInv;
					ny = y * lengthInv;
					nz = z * lengthInv;
					normals.AddRange(new float[] { nx, ny, nz });

					s = (float)j / sectorCount;
					t = (float)i / stackCount;

					texCoords.AddRange(new float[] { s, t, 0.0f });
				}
			}

			this._vertices = vertices.ToArray();
			this._normals = normals.ToArray();
			this._texCoords = texCoords.ToArray();
		}
		private void CalcIndices()
		{
			List<uint> indices = new List<uint>();

			uint sectorCount = 10;
			uint stackCount = 10;
			uint k1, k2;
			for (uint i = 0; i < stackCount; ++i)
			{
				k1 = i * (sectorCount + 1);
				k2 = k1 + sectorCount + 1;

				for (uint j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (i != 0)
					{
						indices.AddRange(new uint[] { k1, k2, k1 + 1 });
					}

					if (i != stackCount - 1)
					{
						indices.AddRange(new uint[] { k1 + 1, k2, k2 + 1 });
					}
				}

			}

			this._indices = indices.ToArray();
		}
		public void DrawSphere()
		{
			float x, y, z, nx, ny, nz, s, t;

			uint index;
			GL.Begin(PrimitiveType.Triangles);
			for (int i = 0; i < _indices.Length; i++)
			{
				index = _indices[i] * 3;
				x = _vertices[index];
				y = _vertices[index + 1];
				z = _vertices[index + 2];

				nx = _normals[index];
				ny = _normals[index + 1];
				nz = _normals[index + 2];

				s = _texCoords[index];
				t = _texCoords[index + 1];

				//GL.Normal3(nx, ny, nz);
				GL.TexCoord2(s, t);
				GL.Vertex3(x, y, z);

			}
			GL.End();

		}
	}
}
