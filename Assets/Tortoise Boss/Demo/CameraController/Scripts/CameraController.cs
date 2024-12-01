using UnityEngine;
using System;

[Serializable]
public class CameraType {
	[Tooltip("A camera must be associated with this variable. The camera that is associated here, will receive the settings of this index.")]
	public Camera _camera;
	public enum TipoRotac{LookAtThePlayer, FirstPerson, FollowPlayer, Orbital, Stop, StraightStop, OrbitalThatFollows, ETS_StyleCamera}
	[Tooltip("Here you must select the type of rotation and movement that camera will possess.")]
	public TipoRotac rotationType = TipoRotac.LookAtThePlayer;
	[Range(0.0f,1.0f)][Tooltip("Here you must adjust the volume that the camera attached to this element can perceive. In this way, each camera can perceive a different volume.")]
	public float volume = 1.0f;
}
[Serializable]
public class CameraSetting {
	[Tooltip("In this variable you can configure the key responsible for switching cameras.")]
	public KeyCode cameraSwitchKey = KeyCode.C;
	[Range(1,20)][Tooltip("In this variable you can configure the sensitivity with which the script will perceive the movement of the mouse. This is applied to cameras that interact with mouse movement only.")]
	public float sensibility = 10.0f;
	[Range(0,360)][Tooltip("The highest horizontal angle that camera style 'FistPerson' camera can achieve.")]
	public float horizontalAngle = 65.0f;
	[Range(0,85)][Tooltip("The highest vertical angle that camera style 'FistPerson' camera can achieve.")]
	public float verticalAngle = 20.0f;
	[Range(1,20)][Tooltip("The speed at which the camera can follow the player.")]
	public float displacementSpeed = 5.0f;
	[Range(1,30)][Tooltip("The speed at which the camera rotates as it follows and looks at the player.")]
	public float spinSpeed = 15.0f;
	[Range(0.5f,3.0f)][Tooltip("The distance the camera will move to the left when the mouse is also shifted to the left. This option applies only to cameras that have the 'ETS_StyleCamera' option selected.")]
	public float ETS_CameraShift = 1.0f;
	[Space(15)][Tooltip("Here you can configure the preferences of cameras that orbit the player.")]
	public SettingsOrbitalCamera orbitalCamera;
}
[Serializable]
public class SettingsOrbitalCamera {
	[Range(0.01f,2.0f)][Tooltip("In this variable you can configure the sensitivity with which the script will perceive the movement of the mouse. ")]
	public float sensibility = 0.8f;
	[Range(0.01f,2.0f)][Tooltip("In this variable, you can configure the speed at which the orbital camera will approach or distance itself from the player when the mouse scrool is used.")]
	public float speedScrool = 1.0f;
	[Range(0.01f,2.0f)][Tooltip("In this variable, you can configure the speed at which the orbital camera moves up or down.")]
	public float speedYAxis = 0.5f;
	[Range(3.0f,20.0f)][Tooltip("In this variable, you can set the minimum distance that the orbital camera can stay from the player.")]
	public float minDistance = 5.0f;
	[Range(20.0f,1000.0f)][Tooltip("In this variable, you can set the maximum distance that the orbital camera can stay from the player.")]
	public float maxDistance = 50.0f;
}

public class CameraController : MonoBehaviour {

	[Tooltip("If this variable is checked, the script will automatically place the 'IgnoreRaycast' layer on the player when needed.")]
	public bool ajustTheLayers = true;

	[Space(10)][Tooltip("Here you must associate all the cameras that you want to control by this script, associating each one with an index and selecting your preferences.")]
	public CameraType[] cameras = new CameraType[0];
	[Tooltip("Here you can configure the cameras, deciding their speed of movement, rotation, zoom, among other options.")]
	public CameraSetting CameraSettings;

	bool orbitalAtiv;
	int index = 0;
	float rotacX = 0.0f;
	float rotacY = 0.0f;
	float tempoOrbit = 0.0f;
	float rotacXETS = 0.0f;
	float rotacYETS = 0.0f;

	GameObject[] objPosicStopCameras;
	Quaternion[] originalRotation;
	GameObject[] oritinalPosition;
	Vector3[] originalPositionETS;
	float[] xOrbit;
	float[] yOrbit;
	float[] distanceFromOrbitalCamera;

	void Awake(){

		GameObject temp = new GameObject ("PlayerCams");
		temp.transform.parent = transform;
		objPosicStopCameras = new GameObject[cameras.Length];
		originalRotation = new Quaternion[cameras.Length];
		oritinalPosition = new GameObject[cameras.Length];
		originalPositionETS = new Vector3[cameras.Length];
		xOrbit = new float[cameras.Length];
		yOrbit = new float[cameras.Length];

		distanceFromOrbitalCamera = new float[cameras.Length];

		for (int x = 0; x < cameras.Length; x++) {
			if (cameras [x]._camera) {
				distanceFromOrbitalCamera [x] = CameraSettings.orbitalCamera.minDistance;

				if (cameras [x].rotationType == CameraType.TipoRotac.FirstPerson) {
					cameras [x]._camera.transform.parent = temp.transform;
					originalRotation [x] = cameras [x]._camera.transform.localRotation;
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.FollowPlayer) {
					cameras [x]._camera.transform.parent = temp.transform;
					oritinalPosition [x] = new GameObject ("positionFollowPlayerCamera" + x);
					oritinalPosition [x].transform.parent = transform;
					oritinalPosition [x].transform.localPosition = cameras [x]._camera.transform.localPosition;
					if (ajustTheLayers) {
						transform.gameObject.layer = 2;
						foreach (Transform trans in this.gameObject.GetComponentsInChildren<Transform>(true)) {
							trans.gameObject.layer = 2;
						}
					}
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.Orbital) {
					cameras [x]._camera.transform.parent = temp.transform;
					xOrbit [x] = cameras [x]._camera.transform.eulerAngles.x;
					yOrbit [x] = cameras [x]._camera.transform.eulerAngles.y;
					if (ajustTheLayers) {
						transform.gameObject.layer = 2;
						foreach (Transform trans in this.gameObject.GetComponentsInChildren<Transform>(true)) {
							trans.gameObject.layer = 2;
						}
					}
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.Stop) {
					cameras [x]._camera.transform.parent = temp.transform;
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.StraightStop) {
					cameras [x]._camera.transform.parent = temp.transform;
					objPosicStopCameras [x] = new GameObject ("positionStraightStopCamera" + x);
					objPosicStopCameras [x].transform.parent = cameras [x]._camera.transform;
					objPosicStopCameras [x].transform.localPosition = new Vector3 (0, 0, 1.0f);
					objPosicStopCameras [x].transform.parent = transform;
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.OrbitalThatFollows) {
					cameras [x]._camera.transform.parent = temp.transform;
					xOrbit [x] = cameras [x]._camera.transform.eulerAngles.x;
					yOrbit [x] = cameras [x]._camera.transform.eulerAngles.y;

					oritinalPosition [x] = new GameObject ("posicaoDaCameraSeguir" + x);
					oritinalPosition [x].transform.parent = transform;
					oritinalPosition [x].transform.localPosition = cameras [x]._camera.transform.localPosition;

					if (ajustTheLayers) {
						transform.gameObject.layer = 2;
						foreach (Transform trans in this.gameObject.GetComponentsInChildren<Transform>(true)) {
							trans.gameObject.layer = 2;
						}
					}
				}

				if (cameras [x].rotationType == CameraType.TipoRotac.ETS_StyleCamera) {
					cameras [x]._camera.transform.parent = temp.transform;
					originalRotation [x] = cameras [x]._camera.transform.localRotation;
					originalPositionETS [x] = cameras [x]._camera.transform.localPosition;
				}

				AudioListener captadorDeSom = cameras [x]._camera.GetComponent<AudioListener> ();
				if (captadorDeSom == null) {
					cameras [x]._camera.transform.gameObject.AddComponent (typeof(AudioListener));
				}

			} else {
				Debug.LogWarning ("There is no camera associated with the index " + x);
			}
		}
	}

	void Start(){
		EnableCameras (index);
	}

	void EnableCameras (int indicePedido){
		if (cameras.Length > 0) {
			for (int x = 0; x < cameras.Length; x++) {
				if (cameras [x]._camera) {
					if (x == indicePedido) {
						cameras [x]._camera.gameObject.SetActive (true);
					} else {
						cameras [x]._camera.gameObject.SetActive (false);
					}
				}
			}
		}
	}

	void ManageCameras(){
		for (int x = 0; x < cameras.Length; x++) {
			if (cameras [x].rotationType == CameraType.TipoRotac.FollowPlayer) {
				if (cameras [x]._camera.isActiveAndEnabled) {
					cameras [x]._camera.transform.parent = null;
				} else {
					cameras [x]._camera.transform.parent = transform;
				}
			}
		}
		AudioListener.volume = cameras [index].volume;
		float velocidadeTimeScale = 1.0f / Time.timeScale;
		switch (cameras[index].rotationType ) {
		case CameraType.TipoRotac.Stop:
			//stop camera
			break;
		case CameraType.TipoRotac.StraightStop:
			var newRotationDest = Quaternion.LookRotation(objPosicStopCameras[index].transform.position - cameras [index]._camera.transform.position, Vector3.up);
			cameras [index]._camera.transform.rotation = Quaternion.Slerp(cameras [index]._camera.transform.rotation, newRotationDest, Time.deltaTime * 15.0f);
			break;
		case CameraType.TipoRotac.LookAtThePlayer:
			cameras [index]._camera.transform.LookAt (transform.position);
			break;
		case CameraType.TipoRotac.FirstPerson:
			rotacX += Input.GetAxis ("Mouse X") * CameraSettings.sensibility;
			rotacY += Input.GetAxis ("Mouse Y") * CameraSettings.sensibility;
			rotacX = ClampAngle (rotacX, -CameraSettings.horizontalAngle, CameraSettings.horizontalAngle);
			rotacY = ClampAngle (rotacY, -CameraSettings.verticalAngle, CameraSettings.verticalAngle);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotacX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotacY, -Vector3.right);
			Quaternion rotacFinal = originalRotation [index] * xQuaternion * yQuaternion;
			cameras [index]._camera.transform.localRotation = Quaternion.Lerp (cameras [index]._camera.transform.localRotation, rotacFinal, Time.deltaTime*10.0f*velocidadeTimeScale);
			break;
		case CameraType.TipoRotac.FollowPlayer:
			RaycastHit hit;
			float velocidade = CameraSettings.displacementSpeed;
			if (!Physics.Linecast (transform.position, oritinalPosition [index].transform.position)) {
				cameras [index]._camera.transform.position = Vector3.Lerp (cameras [index]._camera.transform.position, oritinalPosition [index].transform.position, Time.deltaTime * velocidade);
			}
			else if(Physics.Linecast(transform.position, oritinalPosition [index].transform.position,out hit)){
				cameras [index]._camera.transform.position = Vector3.Lerp(cameras [index]._camera.transform.position, hit.point,Time.deltaTime * velocidade);
			}
			float velocidadeGir = CameraSettings.spinSpeed;
			var newRotation = Quaternion.LookRotation(transform.position - cameras [index]._camera.transform.position, Vector3.up);
			cameras [index]._camera.transform.rotation = Quaternion.Slerp(cameras [index]._camera.transform.rotation, newRotation, Time.deltaTime * velocidadeGir);
			break;
		case CameraType.TipoRotac.Orbital:
			float sensibilidade = CameraSettings.orbitalCamera.sensibility;
			float distMin = CameraSettings.orbitalCamera.minDistance;
			float distMax = CameraSettings.orbitalCamera.maxDistance;
			float velocidadeScrool = CameraSettings.orbitalCamera.speedScrool * 50.0f;
			float sensYMouse = CameraSettings.orbitalCamera.speedYAxis * 10.0f;
			RaycastHit hit2;
			if (!Physics.Linecast (transform.position, cameras [index]._camera.transform.position)) {

			} else if (Physics.Linecast (transform.position, cameras [index]._camera.transform.position, out hit2)) {
				distanceFromOrbitalCamera [index] = Vector3.Distance (transform.position, hit2.point);
				distMin = Mathf.Clamp ((Vector3.Distance (transform.position, hit2.point)), distMin * 0.5f, distMax);
			}
			xOrbit [index] += Input.GetAxis ("Mouse X") * (sensibilidade * distanceFromOrbitalCamera [index])/(distanceFromOrbitalCamera [index]*0.5f);
			yOrbit [index] -= Input.GetAxis ("Mouse Y") * sensibilidade * sensYMouse;
			yOrbit [index] = ClampAngle (yOrbit [index], 0.0f, 85.0f);
			Quaternion rotation = Quaternion.Euler (yOrbit [index], xOrbit [index], 0);
			distanceFromOrbitalCamera [index] = Mathf.Clamp (distanceFromOrbitalCamera [index] - Input.GetAxis ("Mouse ScrollWheel") * velocidadeScrool, distMin, distMax);
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distanceFromOrbitalCamera [index]);
			Vector3 position = rotation * negDistance + transform.position;
			Vector3 posicAtual = cameras [index]._camera.transform.position;
			Quaternion rotacAtual = cameras [index]._camera.transform.rotation;
			cameras [index]._camera.transform.rotation = Quaternion.Lerp(rotacAtual,rotation,Time.deltaTime*5.0f*velocidadeTimeScale);
			cameras [index]._camera.transform.position = Vector3.Lerp(posicAtual,position,Time.deltaTime*5.0f*velocidadeTimeScale);
			break;
		case CameraType.TipoRotac.OrbitalThatFollows:
			float movX = Input.GetAxis ("Mouse X");
			float movY = Input.GetAxis ("Mouse Y");
			float movZ = Input.GetAxis ("Mouse ScrollWheel");
			if (movX > 0.0f || movY > 0.0f || movZ > 0.0f) {
				orbitalAtiv = true;
				tempoOrbit = 0.0f;
			} else {
				tempoOrbit += Time.deltaTime;
			}
			if (tempoOrbit > 3.0f) {
				tempoOrbit = 3.1f;
				orbitalAtiv = false;
			}
			if(orbitalAtiv == true){
				float _sensibilidade = CameraSettings.orbitalCamera.sensibility;
				float _distMin = CameraSettings.orbitalCamera.minDistance;
				float _distMax = CameraSettings.orbitalCamera.maxDistance;
				float _velocidadeScrool = CameraSettings.orbitalCamera.speedScrool * 50.0f;
				float _sensYMouse = CameraSettings.orbitalCamera.speedYAxis * 10.0f;
				RaycastHit _hit;
				if (!Physics.Linecast (transform.position, cameras [index]._camera.transform.position)) {

				} else if (Physics.Linecast (transform.position, cameras [index]._camera.transform.position, out _hit)) {
					distanceFromOrbitalCamera [index] = Vector3.Distance (transform.position, _hit.point);
					_distMin = Mathf.Clamp ((Vector3.Distance (transform.position, _hit.point)), _distMin * 0.5f, _distMax);
				}
				xOrbit [index] += movX * (_sensibilidade * distanceFromOrbitalCamera [index]) / (distanceFromOrbitalCamera [index] * 0.5f);
				yOrbit [index] -= movY * _sensibilidade * _sensYMouse;
				yOrbit [index] = ClampAngle (yOrbit [index], 0.0f, 85.0f);
				Quaternion _rotation = Quaternion.Euler (yOrbit [index], xOrbit [index], 0);
				distanceFromOrbitalCamera [index] = Mathf.Clamp (distanceFromOrbitalCamera [index] - movZ * _velocidadeScrool, _distMin, _distMax);
				Vector3 _negDistance = new Vector3 (0.0f, 0.0f, -distanceFromOrbitalCamera [index]);
				Vector3 _position = _rotation * _negDistance + transform.position;
				Vector3 _posicAtual = cameras [index]._camera.transform.position;
				Quaternion _rotacAtual = cameras [index]._camera.transform.rotation;
				cameras [index]._camera.transform.rotation = Quaternion.Lerp (_rotacAtual, _rotation, Time.deltaTime * 5.0f * velocidadeTimeScale);
				cameras [index]._camera.transform.position = Vector3.Lerp (_posicAtual, _position, Time.deltaTime * 5.0f * velocidadeTimeScale);
			} else {
				RaycastHit __hit;
				float __velocidade = CameraSettings.displacementSpeed;
				if (!Physics.Linecast (transform.position, oritinalPosition [index].transform.position)) {
					cameras [index]._camera.transform.position = Vector3.Lerp (cameras [index]._camera.transform.position, oritinalPosition [index].transform.position, Time.deltaTime * __velocidade);
				}
				else if(Physics.Linecast(transform.position, oritinalPosition [index].transform.position,out __hit)){
					cameras [index]._camera.transform.position = Vector3.Lerp(cameras [index]._camera.transform.position, __hit.point,Time.deltaTime * __velocidade);
				}
				float __velocidadeGir = CameraSettings.spinSpeed;
				var __newRotation = Quaternion.LookRotation(transform.position - cameras [index]._camera.transform.position, Vector3.up);
				cameras [index]._camera.transform.rotation = Quaternion.Slerp(cameras [index]._camera.transform.rotation, __newRotation, Time.deltaTime * __velocidadeGir);
			}
			break;
		case CameraType.TipoRotac.ETS_StyleCamera:
			rotacXETS += Input.GetAxis ("Mouse X") * CameraSettings.sensibility;
			rotacYETS += Input.GetAxis ("Mouse Y") * CameraSettings.sensibility;
			Vector3 novaPosicao = new Vector3 (originalPositionETS [index].x + Mathf.Clamp (rotacXETS / 50 + (CameraSettings.ETS_CameraShift/3.0f), -CameraSettings.ETS_CameraShift, 0), originalPositionETS [index].y, originalPositionETS [index].z);
			cameras [index]._camera.transform.localPosition = Vector3.Lerp (cameras [index]._camera.transform.localPosition, novaPosicao, Time.deltaTime * 10.0f);
			rotacXETS = ClampAngle (rotacXETS, -180, 80);
			rotacYETS = ClampAngle (rotacYETS, -60, 60);
			Quaternion _xQuaternionETS = Quaternion.AngleAxis (rotacXETS, Vector3.up);
			Quaternion _yQuaternionETS = Quaternion.AngleAxis (rotacYETS, -Vector3.right);
			Quaternion _rotacFinalETS = originalRotation [index] * _xQuaternionETS * _yQuaternionETS;
			cameras [index]._camera.transform.localRotation = Quaternion.Lerp (cameras [index]._camera.transform.localRotation, _rotacFinalETS, Time.deltaTime * 10.0f * velocidadeTimeScale);
			break;
		}
	}

	public static float ClampAngle (float angulo, float min, float max){
		if (angulo < -360F) { angulo += 360F; }
		if (angulo > 360F) { angulo -= 360F; }
		return Mathf.Clamp (angulo, min, max);
	}

	void Update(){
		if (Input.GetKeyDown (CameraSettings.cameraSwitchKey) && index < (cameras.Length - 1)) {
			index++;
			EnableCameras (index);
		} else if (Input.GetKeyDown (CameraSettings.cameraSwitchKey) && index >= (cameras.Length - 1)) {
			index = 0;
			EnableCameras (index);
		}
	}

	void LateUpdate(){
		if (cameras.Length > 0) {
			if (cameras [index]._camera) {
				ManageCameras ();
			}
		}
	}
}