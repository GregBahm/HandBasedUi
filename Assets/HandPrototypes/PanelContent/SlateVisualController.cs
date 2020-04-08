using System.Collections.Generic;
using UnityEngine;

public class SlateVisualController : MonoBehaviour 
{
    private float borderBrightness;

    [Range(0, .005f)]
    [SerializeField]
    private float lineWidth;

    [Range(0, .05f)]
    [SerializeField]
    private float rounding;
    public float Rounding => rounding;

    [Range(1, 16)]
    [SerializeField]
    private int cornerVerts;

    private float highlightness;

    private LineRenderer lineRenderer;
    private Material borderMaterial;
    private MeshFilter meshFilter;
    public Material FillMaterial { get; private set; }

    public bool DoHighlightBorder;

    private AudioSource audioSource;

    private static readonly int LeftThumbShaderProp = Shader.PropertyToID("_LeftThumbTip");
    private static readonly int LeftIndexShaderProp = Shader.PropertyToID("_LeftIndexTip");
    private static readonly int RightThumbShaderProp = Shader.PropertyToID("_RightThumbTip");
    private static readonly int RightIndexShaderProp = Shader.PropertyToID("_RightIndexTip");
    private static readonly int HighlightShaderProp = Shader.PropertyToID("_Highlight");

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        FillMaterial = GetComponent<MeshRenderer>().material;
        lineRenderer = GetComponent<LineRenderer>();
        borderMaterial = lineRenderer.material;
        audioSource = GetComponent<AudioSource>();
        UpdateMesh();
    }

    private void Update()
    {
        lineRenderer.widthMultiplier = lineWidth;
        UpdateHighlight();
        UpdateShader();
    }

    private void UpdateShader()
    {
        Vector3 leftThumb = HandPrototypeProxies.Instance.LeftThumb.position;
        Vector3 leftIndex = HandPrototypeProxies.Instance.LeftIndex.position;
        Shader.SetGlobalVector(LeftThumbShaderProp, leftThumb);
        Shader.SetGlobalVector(LeftIndexShaderProp, leftIndex);

        Vector3 rightThumb = HandPrototypeProxies.Instance.RightThumb.position;
        Vector3 rightIndex = HandPrototypeProxies.Instance.RightIndex.position;
        Shader.SetGlobalVector(RightThumbShaderProp, rightThumb);
        Shader.SetGlobalVector(RightIndexShaderProp, rightIndex);
    }
        
    private void UpdateMesh()
    {
        Vector3[] verts = GetVerts();
        UpdateVerts(verts);
        UpdateInteriorMesh(verts);
    }

    private void UpdateInteriorMesh(Vector3[] verts)
    {
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verts;
        mesh.normals = GetNormals(verts);
        mesh.triangles = GetTriangles(verts);
    }

    private Vector3[] GetNormals(Vector3[] verts)
    {
        Vector3[] ret = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            ret[i] = Vector3.forward;
        }
        return ret;
    }

    private int[] GetTriangles(Vector3[] verts)
    {
        List<int> ret = new List<int>();
        for (int i = 1; i < verts.Length - 1; i++)
        {
            ret.Add(0);
            ret.Add(i);
            ret.Add(i + 1);
        }
        return ret.ToArray();
    }

    private static readonly Vector3 cornerA = new Vector3(.5f, -.5f, 0);
    private static readonly Vector3 cornerB = new Vector3(.5f, .5f, 0);
    private static readonly Vector3 cornerC = new Vector3(-.5f, .5f, 0);
    private static readonly Vector3 cornerD = new Vector3(-.5f, -.5f, 0);

    private Vector3[] GetVerts()
    {
        List<Vector3> points = new List<Vector3>();

        points.AddRange(DoCorner(cornerD, cornerA, cornerB));
        points.AddRange(DoCorner(cornerA, cornerB, cornerC));
        points.AddRange(DoCorner(cornerB, cornerC, cornerD));
        points.AddRange(DoCorner(cornerC, cornerD, cornerA));
        return points.ToArray();
    }

    private void UpdateVerts(Vector3[] verts)
    {
        lineRenderer.positionCount = verts.Length;
        lineRenderer.SetPositions(verts);
    }
    
    private IEnumerable<Vector3> DoCorner(Vector3 fromCorner, Vector3 corner, Vector3 toCorner)
    {
        Vector3 startOffset = (corner - fromCorner).normalized * rounding;
        Vector3 endOffset = (corner - toCorner).normalized * rounding;

        Vector3 start = corner - startOffset;
        Vector3 end = corner - endOffset;
        Vector3 center = corner - startOffset - endOffset;

        for (int i = 0; i < cornerVerts; i++)
        {
            float param = (float)i / (cornerVerts - 1);
            yield return SweepAboutCircle(center, start, end, param);
        }
    }

    private Vector3 SweepAboutCircle(Vector3 center,
       Vector3 start,
       Vector3 end,
       float param)
    {
        Vector3 toStart = start - center;
        Vector3 toEnd = end - center;
        Vector3 toRet = Vector3.Slerp(toStart, toEnd, param);
        return toRet + center;
    }

    private float grabbedness;

    private void UpdateHighlight()
    {
        float highlightTarget = DoHighlightBorder ? 1 : 0;
        highlightness = Mathf.Lerp(highlightness, highlightTarget, Time.deltaTime * 10);
        borderMaterial.SetFloat(HighlightShaderProp, highlightness);
    }
}