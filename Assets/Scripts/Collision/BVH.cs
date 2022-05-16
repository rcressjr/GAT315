using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;
        List<Body> sorted = new List<Body>(bodies);

        // sort bodies along x-axis (position.x) 
        sorted.Sort((x, y) => x.position.x.CompareTo(y.position.x));
        // set sorted bodies to root bvh node 
        rootNode = new BVHNode(sorted);
    }

    public override void Query(AABB aabb, List<Body> results)
    {
        rootNode.Query(aabb, results);
        // update the number of potential collisions 
        queryResultCount += results.Count;
    }

    public override void Query(Body body, List<Body> results)
    {
        Query(body.shape.GetAABB(body.position), results);
    }

    public override void Draw()
    {
        rootNode?.Draw();
    }
}
