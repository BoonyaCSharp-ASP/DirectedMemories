using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamSwitcher : MonoBehaviour {
	
	public DimensionsAndPortalsController DAPController;
	public Transform playerTransform;
	public List<GameObject> GameObjectsToSwitchOn = new List<GameObject>();
	public List<GameObject> GameObjectsToSwitchOff = new List<GameObject>();
	
	void Awake () {
		getDAPController();
	}
	
	void Update () {
		setCameraNear();
	}
	
	/**
	 * Get the DimensionsAndPortalsController in a lazy way
	 */
	private DimensionsAndPortalsController getDAPController() {
		if (!DAPController) {
			GameObject DAPGO = (GameObject) GameObject.Find("DimensionsAndPortalsController (Dimensions)");
			if (DAPGO) {
				DAPController = DAPGO.GetComponent<DimensionsAndPortalsController>();
			}
		}
		
		return DAPController;
	}
	
	/**
	 * Create the list of items that are required to be switched.
	 */
	public void listSwitcher() {
		GameObjectsToSwitchOn.Clear();
		GameObjectsToSwitchOff.Clear();
		DepthSwitcher[] switchers = Object.FindSceneObjectsOfType(typeof(DepthSwitcher)) as DepthSwitcher[];
		foreach (DepthSwitcher switcher in switchers) {
			bool found = false;
			if ((camera.cullingMask & switcher.renderInLayers.value) != 0) {
				GameObjectsToSwitchOn.Add(switcher.gameObject);
			} else {
				GameObjectsToSwitchOff.Add(switcher.gameObject);
			}
		}
	}
	
	/**
	 * Switch specific gameobjects that are lighting or effect related
	 */
	void OnPreCull () {
		if (DAPController.renderSettingsHandler != null) {
			if (DAPController.useOculurRift == true) {	//OVR Support
				if (camera.transform.parent.camera != null) {
					if (camera.transform.parent.tag == "CamNormal") {
						DAPController.renderSettingsHandler.SwitchToDefault();
					} else if (camera.transform.parent.tag == "CamAlternate") {
						DAPController.renderSettingsHandler.SwitchToAlternate();
					}
				}
			} else {
				if (camera == DAPController.rCam) {
					DAPController.renderSettingsHandler.SwitchToDefault();
				} else {
					DAPController.renderSettingsHandler.SwitchToAlternate();
				}
			}
		}

		foreach(GameObject GO in GameObjectsToSwitchOn) {
			GO.SetActive(true);
		}
		foreach(GameObject GO in GameObjectsToSwitchOff) {
			GO.SetActive(false);
		}
	}

	/**
	 * Unity 4 fix
	 */
	void OnPostRender() {
		if (DAPController.useOculurRift == true) {	//OVR Support
			if (camera.depth == 1 || camera.depth == 2) {
				GL.ClearWithSkybox(false,this.camera);
			}
		} else {
			if (camera.depth == 1) {
				GL.ClearWithSkybox(false,this.camera);
			}
		}
	}
	
	/**
	 * Detect if a certain renderen is visible from a camera
	 */
	public bool IsVisibleFrom(Renderer renderer, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
	
	/**
	 * Set the camera near rendering as close as possible to the portal to prevent objects occluding if you are far away from a portal or dimensional rift
	 */
	void setCameraNear() {
		if (camera.depth == 0) {
			float closest = 1000f;
			foreach(DimensionalGateController dimGate in DAPController.activeGates) {
				//if (IsVisibleFrom(portal.renderer,camera)) {
					float distance = Vector3.Distance(transform.position,dimGate.transform.position);
				
					closest = Mathf.Min(distance-dimGate.renderOffset,closest);
				//}
			}
			closest = Mathf.Max(0.001f,closest);
			camera.nearClipPlane = closest;
		} else {
			camera.nearClipPlane = 0.3f;
		}
	}
}
