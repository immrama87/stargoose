using UnityEngine;

public static class RenderExtensions{
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera){
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes (camera);
		return GeometryUtility.TestPlanesAABB (planes, renderer.bounds);
	}
}
