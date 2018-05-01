using UnityEngine;
using System.Collections;

public class FlashingLightScript : MonoBehaviour {

	Light pLight;
	public float maxIntensity, minIntesity;
	bool isInc, isDec;
	// Use this for initialization
	void Start () {
		pLight = GetComponent<Light>();
		pLight.intensity = 0.5f;
		isInc = true; isDec = false;
	}
	
	// Update is called once per frame
	void Update () {
		IntensityManagement();
	}

	void IntensityManagement(){
		float currIntensity = pLight.intensity;
		if(currIntensity >= maxIntensity){
			isDec = true; isInc = false;
		}
		else if(currIntensity <= minIntesity){
			isDec = false; isInc = true;
		}
		else{
			//in the middle
		}

		if(isInc)
			pLight.intensity += Time.deltaTime;
		if(isDec)
			pLight.intensity -= Time.deltaTime;

	}
}
