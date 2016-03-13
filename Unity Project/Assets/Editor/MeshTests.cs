using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Assets.Editor;
using System.Collections.Generic;

public class MeshTests
{
    MeshGenerator mesh;
    MeshGenerator.Node[] nodes1;
    MeshGenerator.Node[] nodes2;
    MeshGenerator.Node[] nodes3;
    MeshGenerator.Node[] nodes4;
    MeshGenerator.Node[] nodes5;
    MeshGenerator.Node[] nodes6;

    [SetUp]
    public void Init()
    {
        mesh = new MeshGenerator();

        nodes1 = new MeshGenerator.Node[3] { new MeshGenerator.Node(new Vector3(1, 2, 3)),
                                             new MeshGenerator.Node(new Vector3(4, 5, 6)),
                                             new MeshGenerator.Node(new Vector3(7, 8, 9)) };

        nodes1[0].vertexIndex = 1;
        nodes1[1].vertexIndex = 2;
        nodes1[2].vertexIndex = 3;

        nodes2 = new MeshGenerator.Node[3] { new MeshGenerator.Node(new Vector3(9, 8, 7)),
                                             new MeshGenerator.Node(new Vector3(6, 5, 4)),
                                             new MeshGenerator.Node(new Vector3(3, 2, 1)) };

        nodes2[0].vertexIndex = 4;
        nodes2[1].vertexIndex = 5;
        nodes2[2].vertexIndex = 6;

        nodes3 = new MeshGenerator.Node[3] { nodes1[0], nodes2[1], nodes2[0] };

        nodes4 = new MeshGenerator.Node[4] { new MeshGenerator.Node(new Vector3(0, 0, 0)),
                                             new MeshGenerator.Node(new Vector3(1, 2, 3)),
                                             new MeshGenerator.Node(new Vector3(9, 9, 9)),
                                             new MeshGenerator.Node(new Vector3(0, 5, 9))};

        nodes4[0].vertexIndex = 7;
        nodes4[1].vertexIndex = 8;
        nodes4[2].vertexIndex = 9;
        nodes4[3].vertexIndex = 10;

        nodes5 = new MeshGenerator.Node[5] { new MeshGenerator.Node(new Vector3(0, 0, 0)),
                                             new MeshGenerator.Node(new Vector3(1, 2, 3)),
                                             new MeshGenerator.Node(new Vector3(9, 9, 9)),
                                             new MeshGenerator.Node(new Vector3(0, 5, 9)),
                                             new MeshGenerator.Node(new Vector3(10, 10, 10))};

        nodes5[0].vertexIndex = 11;
        nodes5[1].vertexIndex = 12;
        nodes5[2].vertexIndex = 13;
        nodes5[3].vertexIndex = 14;
        nodes5[4].vertexIndex = 15;

        nodes6 = new MeshGenerator.Node[6] { new MeshGenerator.Node(new Vector3(0, 0, 0)),
                                             new MeshGenerator.Node(new Vector3(1, 2, 3)),
                                             new MeshGenerator.Node(new Vector3(9, 9, 9)),
                                             new MeshGenerator.Node(new Vector3(0, 5, 9)),
                                             new MeshGenerator.Node(new Vector3(6, 6, 6)),
                                             new MeshGenerator.Node(new Vector3(5, 4, 3))};

        nodes6[0].vertexIndex = 16;
        nodes6[1].vertexIndex = 17;
        nodes6[2].vertexIndex = 18;
        nodes6[3].vertexIndex = 19;
        nodes6[4].vertexIndex = 20;
        nodes6[5].vertexIndex = 21;

    }


    [Test]
    public void IsOutlineEdgeTest()
    {
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes1);
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes2);


        Assert.IsFalse((bool) UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "IsOutlineEdge", mesh, new object[2] { 1, 4 }));

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes3);

        Assert.IsTrue((bool)UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "IsOutlineEdge", mesh, new object[2] { 1, 4 }));
    }

    [Test]
    public void GetConnectedOutlineVertexTest()
    {
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes1);
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes2);
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes3);

        Assert.AreEqual(2, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 1 }));
        Assert.AreEqual(1, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 2 }));
        Assert.AreEqual(1, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 3 }));
        Assert.AreEqual(6, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 4 }));
        Assert.AreEqual(6, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 5 }));
        Assert.AreEqual(4, UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "GetConnectedOutlineVertex", mesh, new object[] { 6 }));

    }

    [Test]
    public void CreateTriangeTest()
    {
        Dictionary<int, List<Triangle>> triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.IsFalse(triangles.ContainsKey(1));
        Assert.IsFalse(triangles.ContainsKey(2));
        Assert.IsFalse(triangles.ContainsKey(3));

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "CreateTriangle", mesh, nodes1);


        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.IsTrue(triangles.ContainsKey(1));
        Assert.IsTrue(triangles.ContainsKey(2));
        Assert.IsTrue(triangles.ContainsKey(3));
    }

    [Test]
    public void MeshFromPointsTest()
    {
        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { new MeshGenerator.Node[0] });
        Dictionary<int, List<Triangle>> triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");

        Assert.AreEqual(0, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes1 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(3, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes2 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(6, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes4 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(10, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes5 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(15, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes6 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(21, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes6 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(21, triangles.Keys.Count);

        UnitTestUtilities.RunInstanceMethod(typeof(MeshGenerator), "MeshFromPoints", mesh, new object[] { nodes3 });
        triangles = (Dictionary<int, List<Triangle>>)UnitTestUtilities.GetInstanceField(typeof(MeshGenerator), mesh, "triangleDictionary");
        Assert.AreEqual(21, triangles.Keys.Count);



    }
}
