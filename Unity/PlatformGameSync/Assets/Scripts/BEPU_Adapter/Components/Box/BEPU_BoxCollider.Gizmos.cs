using System;
using UnityEngine;

public partial class BEPU_BoxCollider {
#if UNITY_EDITOR
    private Mesh _gizmosMesh = null;

    private Vector3[] vertices = new Vector3[24];
    private Vector3[] normals = new Vector3[24];
    private Vector2[] uvs = new Vector2[24]; // UV坐标 (可选,但推荐)
    private int[] triangles = new int[36];

    private Mesh GizmosMesh {
        get {
            if (_gizmosMesh == null) {
                _gizmosMesh = new();
            }
            return _gizmosMesh;
        }
    }

    private void OnDrawGizmos() {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.green;
        var center = this.entity.Position.ToUnityVector3();
        var size = new Vector3((float)boxShape.Width, (float)boxShape.Height, (float)boxShape.Length);
        UpdateGizmosMesh(GizmosMesh, (float)boxShape.Width, (float)boxShape.Height, (float)boxShape.Length);
        Gizmos.DrawWireMesh(GizmosMesh, 0, center, entity.Orientation.ToUnityQuaternion());
        Gizmos.color = oldColor;
    }

    private void UpdateGizmosMesh(Mesh mesh, float w, float h, float length) {
        mesh.Clear(); // 清除旧的网格数据

        float halfW = w / 2f;
        float halfH = h / 2f;
        float halfL = length / 2f;

        // 定义长方体的8个唯一角点
        // 这些点将作为构建面的基础
        //   y
        //   |  p7---p6
        //   | /|   /|
        //   p4---p5 |
        //   |  p3--|-p2
        //   | /   | /
        //   p0---p1-- x
        //  /
        // z

        Vector3 p0 = new Vector3(-halfW, -halfH, -halfL); // 后-下-左 (Back-Bottom-Left)
        Vector3 p1 = new Vector3(halfW, -halfH, -halfL); // 后-下-右 (Back-Bottom-Right)
        Vector3 p2 = new Vector3(halfW, -halfH, halfL); // 前-下-右 (Front-Bottom-Right)
        Vector3 p3 = new Vector3(-halfW, -halfH, halfL); // 前-下-左 (Front-Bottom-Left)

        Vector3 p4 = new Vector3(-halfW, halfH, -halfL); // 后-上-左 (Back-Top-Left)
        Vector3 p5 = new Vector3(halfW, halfH, -halfL); // 后-上-右 (Back-Top-Right)
        Vector3 p6 = new Vector3(halfW, halfH, halfL); // 前-上-右 (Front-Top-Right)
        Vector3 p7 = new Vector3(-halfW, halfH, halfL); // 前-上-左 (Front-Top-Left)

        int vIndex = 0; // 当前顶点数组的索引
        int tIndex = 0; // 当前三角形数组的索引

        // 辅助方法，用于添加一个面 (由4个顶点定义的四边形)
        // 顶点顺序 (vA, vB, vC, vD) 需要是逆时针方向 (当从面的外部观察时)
        System.Action<Vector3, Vector3, Vector3, Vector3, Vector3> AddFace =
            (vA, vB, vC, vD, normal) => {
                vertices[vIndex + 0] = vA;
                vertices[vIndex + 1] = vB;
                vertices[vIndex + 2] = vC;
                vertices[vIndex + 3] = vD;

                for (int i = 0; i < 4; i++) {
                    normals[vIndex + i] = normal;
                }

                // 为这个面设置标准的UV坐标
                uvs[vIndex + 0] = new Vector2(0, 0); // 左下
                uvs[vIndex + 1] = new Vector2(1, 0); // 右下
                uvs[vIndex + 2] = new Vector2(1, 1); // 右上
                uvs[vIndex + 3] = new Vector2(0, 1); // 左上

                // 四边形的两个三角形 (逆时针顺序)
                // 三角形1: vA, vB, vC
                // 三角形2: vA, vC, vD
                triangles[tIndex + 0] = vIndex + 0;
                triangles[tIndex + 1] = vIndex + 1;
                triangles[tIndex + 2] = vIndex + 2;

                triangles[tIndex + 3] = vIndex + 0;
                triangles[tIndex + 4] = vIndex + 2;
                triangles[tIndex + 5] = vIndex + 3;

                vIndex += 4; // 为下一个面移动顶点索引
                tIndex += 6; // 为下一个面移动三角形索引
            };

        // 构建长方体的6个面
        // 确保顶点顺序是逆时针 (从外部看)

        // 前面 (+Z)
        AddFace(p3, p2, p6, p7, Vector3.forward);
        // 后面 (-Z)
        AddFace(p1, p0, p4, p5, Vector3.back);
        // 上面 (+Y)
        AddFace(p7, p6, p5, p4, Vector3.up);
        // 下面 (-Y)
        AddFace(p0, p1, p2, p3, Vector3.down);
        // 右面 (+X)
        AddFace(p2, p1, p5, p6, Vector3.right);
        // 左面 (-X)
        AddFace(p0, p3, p7, p4, Vector3.left);


        // 将计算好的数据赋值给Mesh
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }

#endif
}