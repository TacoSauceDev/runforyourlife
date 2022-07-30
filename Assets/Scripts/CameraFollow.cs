using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera _cam;

    public GameObject[] targets;

    float speed = 10f;

    public float minSize = 5;
    public float maxSize = 20;

    //public Tether tether;

	// Use this for initialization
	void Awake ()
    {
        _cam = GetComponent<Camera>();
	}
    
    // Update is called once per frame
    void Update()
    {
        
        if (targets == null || targets.Length == 0)
            return;

        var tmp = Vector3.zero;
        for (int i = 0; i < targets.Length; i++)
        {
            tmp += targets[i].transform.position;
        }

        tmp /= targets.Length;

        if (tmp.x < 0)
            tmp.x = 0;

        tmp.y = _cam.transform.position.y;
        tmp.z = _cam.transform.position.z;

        //  _cam.orthographicSize = Mathf.Lerp(minSize, maxSize, tether.Distance);

        _cam.transform.position = Vector3.Lerp(_cam.transform.position, 
                                               tmp, 
                                               Time.deltaTime);
    }
}