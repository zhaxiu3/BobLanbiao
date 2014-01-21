using UnityEngine;
using System.Collections;

public class GLLineRenderer {

    static Material lineMaterial;
    public static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }
    public static void DrawLine(Vector3[] points)
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(new Color(1, 1, 0, 1));
        for (int i = 0; i < points.Length; i++)
        {
            GL.Vertex(points[i]);
        }
            GL.End();
    }
}
