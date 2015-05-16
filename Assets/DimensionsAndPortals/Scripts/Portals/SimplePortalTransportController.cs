using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* -----------------------------------------------------------------------------------------------------
	-- SimplePortalTransportController --
	Requires -- SimplePortalGateDetector -- 
 
	This an EXAMPLE scipt that shows a setup for a simple portal Transport Controller that works for 
	portals in the most	basic setups.
	
	To achieve real physic effect you will have to use a physics based character controller (e.g. rigidbodies
	and custom control scripts). Since most of the 1st person projects are based on the unity3d character 
	controller, a script has been made to create portal like effects around this controller. Do note that this
	is not a 1:1 implementation from the portals used by Valve!
	
	So again: This has a basic setup for you to experiment with. This is NOT a full portal Implementation!
	This means you CAN trigger unwanted behaviour e.g. by shooting other portals while standing in one.
   ------------------------------------------------------------------------------------------------------ */

public class SimplePortalTransportController : MonoBehaviour {
	
	public PortalController portalController;
	[HideInInspector]
	public SimplePortalTransportController otherSPTController;
	private DimensionsAndPortalsController DAPController;
	[HideInInspector]
	public List<Transform> ignoreObjects = new List<Transform>();
	public float portalExitVelocity = 1f;
	
	// Use this for initialization
	void Awake () {
		if (portalController == null) {
			portalController = GetComponent<PortalController>();
			if (portalController == null) {
				Debug.Log("No PortalController Present in this GameObject: " + gameObject.name);
			}
		}
		
		getDAPController();
	}
	
	/**
	 * Get the DimensionsAndPortalsController in a lazy way
	 */
	private DimensionsAndPortalsController getDAPController() {
		if (!DAPController) {
			GameObject DAPGO = (GameObject) GameObject.Find("DimensionsAndPortalsController (Portals)");
			if (DAPGO) {
				DAPController = DAPGO.GetComponent<DimensionsAndPortalsController>();
			}
		}
		
		return DAPController;
	}
	
	/**
	 * Get the SimplePortalTransportController in a lazy way
	 */
	private SimplePortalTransportController getOtherSPTController() {
		if (!otherSPTController) {
			getDAPController();
			if(portalController != null) {
				otherSPTController = portalController.otherController.GetComponent<SimplePortalTransportController>();
				if (otherSPTController == null) {
					Debug.Log("The other portal does not have a SimplePortalTransportController!");
				}
			}
		}
		
		return otherSPTController;
	}
	
	/**
	 * When we enter the portal collider
	 */
	public void ManualTriggerEnter(Collider other) {
		// Is the collider portable?
		if (other.tag == DAPController.portableTag) {
			// Is the collider not within the bounds of the portal? If it is, this is a second trigger (after transport)
			if (!portalController.portalBoxCollider.bounds.Contains(other.transform.position)) {
				// Should this object be ignored?
				if (!ignoreObjects.Contains(other.transform)) {
					// Disable the attached objects for the object
					portalController.attachedToObject.gameObject.layer = DAPController.portalDisabledLayerInt;
					portalController.otherController.attachedToObject.gameObject.layer = DAPController.portalDisabledLayerInt;
					
					//Notify the other portal to ignore the incoming object if it comes in contact the first time (teleport)
					getOtherSPTController().addIgnoreObject(other.transform);
					//addIgnoreObject(other.transform); // Could be possible in your setup
					
					// If the collider is a player, enable the bounding box.
					if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
						portalController.portalBoxCollider.gameObject.SetActive(true);
						portalController.portalBoxCollider.renderer.enabled = true;
						
					}
				} else {
					StartCoroutine(removeIgnoredObject(other.transform));
					StartCoroutine(getOtherSPTController().removeIgnoredObject(other.transform));
				}
			}	
		}
	}
	
	
	/**
	 * When we exit the portal collider
	 */
	public void ManualTriggerExit(Collider other) {
		
		// Is the object portable?
		if (other.tag == DAPController.portableTag) {
			// Is the object in the ignore objects list?
			
			if (!ignoreObjects.Contains(other.transform)) {
				
				portalController.attachedToObject.gameObject.layer = DAPController.defaultLayerInt;
				portalController.otherController.attachedToObject.gameObject.layer = DAPController.defaultLayerInt;
				if (portalController.portalBoxCollider.bounds.Contains(other.transform.position)) {					// We are leaving the portal
					
					Vector3 currentForward = other.transform.forward;
					
					// Do we have a player?
					if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
						HandlePlayerThroughPortal(other);
					// We have an object without a player
					} else {
						HandleOtherThroughPortal(other);
					}
					
					// In case of a rigidbody maintain velocity
					if (other.rigidbody) {
						other.rigidbody.velocity = Quaternion.FromToRotation(currentForward,other.transform.forward) * other.rigidbody.velocity;
					}
					
					//Remove ignore status
					StartCoroutine(removeIgnoredObject(other.transform));
					StartCoroutine(getOtherSPTController().removeIgnoredObject(other.transform));
					
					portalController.portalBoxCollider.gameObject.SetActive(false);
					portalController.otherController.portalBoxCollider.gameObject.SetActive(false);
				}
				
				if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
					portalController.portalBoxCollider.gameObject.renderer.enabled = false;
				} else if (other.gameObject.layer == LayerMask.NameToLayer("PortableObject")) {
					other.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				
				// Remove in case the object moves back
			} else {
				StartCoroutine(removeIgnoredObject(other.transform));
				StartCoroutine(getOtherSPTController().removeIgnoredObject(other.transform));
			}
		} 
	}
	
	/**
	 * Normal movement through portal (e.g. bullets)
	 */
	public void HandleOtherThroughPortal(Collider other) {
		Vector3 relativePos = transform.InverseTransformPoint(other.transform.position);
		Vector3 newPos = portalController.otherController.transform.TransformPoint(relativePos);
		
		Vector3 relativeDir = transform.InverseTransformDirection(other.transform.forward);
		Vector3 newDir = portalController.otherController.transform.TransformDirection(-relativeDir);
		
		other.transform.position = newPos;
		other.transform.forward = newDir;
	}
	
	/**
	 * Player movement through portal considering the camera and charactermotor
	 */
	public void HandlePlayerThroughPortal(Collider other) {
		PlayerPortalController pController = other.GetComponent<PlayerPortalController>();
		if (pController) {
			Vector3 currentForward = other.transform.forward;
			// Change position of the player (accounting for the camera)
			Vector3 originalPosition = other.transform.position;
			other.transform.position = portalController.otherController.ownCameraTrans.position - pController.playerCameraHolder.localPosition;
			
			// Update the player rotation (Yaw)
			Vector3 eulerDir = Quaternion.LookRotation(portalController.otherController.ownCameraTrans.forward).eulerAngles;
			Vector3 eulerY = eulerDir;
			eulerY.x = 0;
			eulerY.z = 0;
			other.transform.localEulerAngles = eulerY;
			
			// Update the player camera Pitch and Roll
			MouseLookPortal pmLook = pController.playerCameraHolder.GetComponent<MouseLookPortal>();
			pmLook.modifyRotation(eulerDir.x,portalController.otherController.ownCameraTrans.eulerAngles.z);
			
			// In case of a character controller (CharacterMotor) maintain velocity
			CharacterMotor cController = other.GetComponent<CharacterMotor>();
			if (cController) {
				float exitAngle = Vector3.Angle(portalController.otherController.transform.up,Vector3.up);
				//exitAngle = 0;
				Vector3 exitVelocityBoost = portalController.otherController.transform.forward * (Mathf.Sin (Mathf.Deg2Rad * exitAngle) * portalExitVelocity);
				cController.movement.velocity = Quaternion.FromToRotation(currentForward,other.transform.forward) *cController.movement.velocity;
				cController.movement.velocity += exitVelocityBoost;
			}
			
		} else {
			Debug.Log("Trying to portal a player without PlayerPortalController!");
		}	
	}
	
	/**
	 * Add an object to temporary ignore with OnTriggerEnter
	 */
	public void addIgnoreObject(Transform object2ignore) {
		if (!ignoreObjects.Contains(object2ignore)) {
			ignoreObjects.Add(object2ignore);
		}
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
	
}
