using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DimensionalGateController : MonoBehaviour {
	public enum DimensionalGateDirection {Both = 0, Real2Alternate = 1,Alternate2Real=2,OtherBoth=3,OtherReal2Alternate = 4,OtherAlternate2Real=5};
	public bool canTravenTrough = false;
	public bool displaced = false;
	public float renderOffset = 0f;
	public DimensionalGateController otherGate;
	public DimensionalGateDirection travelDirection = DimensionalGateDirection.Both;
	public bool matchOtherGateRotationAndSize = false;
	public LayerMask otherReflectsLayer;
	
	private List<Transform> ignoreObjects = new List<Transform>();
	private DimensionsAndPortalsController DAPController;
	
	// Use this for initialization
	void Awake () {
		if (canTravenTrough == true && !collider) {
			Debug.Log("Warning! You can't travel trough Dimensional Gates if there is no collider in the gate!");
		}
		getDAPController();
	}
	
	// Update is called once per frame
	void Update () {
		if (displaced == true) {
			if (otherGate == false) {
				renderer.enabled = false;
			} else {
				renderer.enabled = true;
			}
		} else {
			renderer.enabled = true;
		}
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
	 * Detect if this is a solitary portal (overlapping scenes) or one that is connected to another one
	 */
	public bool isConnectedToOther() {
		if (displaced && otherGate && (travelDirection == DimensionalGateDirection.OtherBoth || travelDirection == DimensionalGateDirection.OtherAlternate2Real || travelDirection == DimensionalGateDirection.OtherReal2Alternate)) {
			return true;
		} else {
			return false;	
		}
	}
	
	/**
	 * Add an object to temporary ignore with OnTriggerEnter
	 */
	public void addIgnoreObject(Transform object2ignore) {
		ignoreObjects.Add(object2ignore);
	}
	
	/**
	 * Ignore the object for 1 seconds so OnTriggerEnter is not called after teleportation
	 */	
	IEnumerator removeIgnoredObject(Transform object2remove) {
		yield return new WaitForSeconds(1f);
		if (ignoreObjects.Contains(object2remove)) {
			ignoreObjects.Remove(object2remove);
		}
		yield return null;
	}
	
	/**
	 * Detect if a object that is portable passed through the portal
	 */
    void OnTriggerEnter(Collider other) {
		if (canTravenTrough == true) {
	    	if (other.tag == DAPController.portableTag) {
				if (!ignoreObjects.Contains(other.transform)) {
					if(DAPController.PortableSwitchesRealm(other.transform,this)) {	//Did the object travel through?
						//We can do something here if you like
						SendMessage("TransformWentThroughPortal",other.transform,SendMessageOptions.DontRequireReceiver);
					}
				} else {
					StartCoroutine(removeIgnoredObject(other.transform));
				}
			}
		}
    }

}
