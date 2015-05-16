using UnityEngine;
using System.Collections;

public static class DAPPhysics : object {

	public static Ray transformRay(Ray ray, Vector3 hit, DimensionalGateController DGC) {
		Transform sourceGate = DGC.transform;
		Transform targetGate = DGC.otherGate.transform;
		
		Vector3 targetPos = targetGate.TransformPoint(sourceGate.InverseTransformPoint(hit));
		Vector3 targetDir = targetGate.TransformDirection(sourceGate.InverseTransformDirection(ray.direction));
		
		return new Ray (targetPos, targetDir);
	}

	/* Not yet Implemented */
	public static Ray transformRay(Ray ray, Vector3 hit, PortalController portalController) {
		return ray;
	}

	public static bool Raycast (Ray ray) {
		RaycastHit hitInfo;
		return Raycast(ray, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Ray ray, float distance) {
		RaycastHit hitInfo;
		return Raycast(ray, out hitInfo,Mathf.Infinity, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo) {
		return Raycast(ray, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction) {
		RaycastHit hitInfo;
		return Raycast(new Ray(origin,direction), out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, float distance) {
		RaycastHit hitInfo;
		return Raycast(new Ray(origin,direction), out hitInfo, distance, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Ray ray, float distance, int layerMask) {
		RaycastHit hitInfo;
		return Raycast(ray, out hitInfo, distance, layerMask);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance) {
		return Raycast(ray, out hitInfo, distance, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo) {
		return Raycast(new Ray(origin,direction), out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask) {
		RaycastHit hitInfo;
		return Raycast(new Ray(origin,direction), out hitInfo, distance, layerMask);
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float distance) {
		return Raycast(new Ray(origin,direction), out hitInfo, distance, Physics.DefaultRaycastLayers);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance = Mathf.Infinity , int layerMask = Physics.DefaultRaycastLayers) {
		bool gotHit = Physics.Raycast(ray,out hitInfo,distance,layerMask);
		/* Enable for debug */
		//Vector3 dir = hitInfo.point-ray.origin;
		//Debug.DrawRay(ray.origin,dir,Color.red,1f);
		if (gotHit == true) {
			if (hitInfo.transform.gameObject.tag == "Portal") {
				DimensionalGateController DGC = hitInfo.transform.GetComponent<DimensionalGateController>();
				if (DGC != null) { // TODO: Also check for direction!!!!
					if (distance != Mathf.Infinity) {
						distance -= Vector3.Distance(ray.origin,hitInfo.point);
					}

					Ray newRay = transformRay(ray,hitInfo.point, DGC);
					return SecondCast(newRay, out hitInfo, distance, layerMask);
				} else {
					PortalController PC = hitInfo.transform.parent.GetComponent<PortalController>();
					if (PC != null) {
						if (distance != Mathf.Infinity) {
							distance -= Vector3.Distance(ray.origin,hitInfo.point);
						}

						Ray newRay = transformRay(ray,hitInfo.point, PC);
						return SecondCast(newRay, out hitInfo, distance, layerMask);
					}

					return false; // Faulty setup dimensional gate!
				}
			} else {
				return true; // Non dimensional gate hit
			}
		} else {
			return false; // No hit
		}
	}

	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float distance, int layerMask) {
		return Raycast(new Ray(origin,direction), out hitInfo, distance, layerMask);
	}

	private static bool SecondCast(Ray newRay, out RaycastHit hitInfo,float distance,int layerMask) {
		bool gotHit = Physics.Raycast(newRay,out hitInfo,distance,layerMask);
		if (gotHit == true) {
			/* Enable for debug */
			//Vector3 dir = hitInfo.point - newRay.origin;
			//Debug.DrawRay(newRay.origin,dir,Color.red,1f);
			if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("DAP_Portal")) {
				return false; // No portal in portal raycast allowed!
			} else {
				return true; // Hit a non-portal object
			}
		} else {
			/* Enable for debug */
			//Debug.DrawRay(newRay.origin,newRay.direction*Mathf.Min(1000f,distance),Color.red,1f);
			return false; // No hit after gate
		}
	}
}
