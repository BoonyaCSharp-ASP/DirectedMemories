using UnityEngine;
using System.Collections;

public class SimplePortalGateDetector : MonoBehaviour {
	
	public SimplePortalTransportController transportController;
	
	void OnTriggerEnter(Collider other) {
		if (transportController) {
			transportController.ManualTriggerEnter(other);	
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (transportController) {
			transportController.ManualTriggerExit(other);	
		}
	}
}
