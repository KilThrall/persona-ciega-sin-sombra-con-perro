using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

[RequireComponent(typeof(CompositeCollider2D))]
public class AutoTilemapShadows : MonoBehaviour
{

    [Space]
    [SerializeField]
    private bool selfShadows = true;

    private CompositeCollider2D tilemapCollider;

    static readonly FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D)
            .Assembly
            .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
            .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);

    [ContextMenu("Generate")]
    public void Generate()
    {
        tilemapCollider = GetComponent<CompositeCollider2D>();

        print(tilemapCollider.pathCount);
        if (tilemapCollider.pathCount == 0)
        {
            print("Shadow must be used in one closed tiles. Please erase the other tiles to other Tilemap.");
            return;
        }

        DestroyAllChildren();


        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];

            tilemapCollider.GetPath(i, pathVertices);
            CreateShadowsForPath(pathVertices);
        }

        Debug.Log("Shadow Generated");

    }


    private void CreateShadowsForPath(Vector2[] pathVertices)
    {
        var shadowCaster = new GameObject("ShadowCaster");
        shadowCaster.transform.parent = gameObject.transform;
        
        ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
        shadowCasterComponent.selfShadows = this.selfShadows;

        var centroid = Vector2.zero;
        foreach (var path in pathVertices)
        {
            centroid += path;
        }
        centroid /= pathVertices.Length;

        var finalPath = new Vector3[pathVertices.Length];
        var j = 0;
        foreach (var path in pathVertices)
        {
            finalPath[j++] = path-centroid;
        }

        shadowCaster.transform.position = centroid;

        shapePathField.SetValue(shadowCasterComponent, finalPath);
        meshField.SetValue(shadowCasterComponent, new Mesh());
        generateShadowMeshMethod.Invoke(shadowCasterComponent, new object[] { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
    }

    public void DestroyAllChildren()
    {

        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }

    }

}