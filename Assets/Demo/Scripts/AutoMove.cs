using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public float Speed = 1.0f;

    public Transform PointStart;
    public Transform PointEnd;

	void Start ()
    {
        transform.position = PointStart.position;
    }
	
	void Update ()
    {
        float deltaTime = Mathf.PingPong(Time.time*Speed, 1);

        transform.position = Vector3.Lerp(
            PointStart.position, 
            PointEnd.position, 
            deltaTime
        );
    }
}
