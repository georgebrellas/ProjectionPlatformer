using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public GameObject objectToFollow;

    public float speed = 2.0f;

    void Update()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;

        
        position.y = Mathf.Clamp(Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation), 1.41f, 1000f);
        position.x = Mathf.Clamp(Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation), -38.39f, - 26.78f);

        this.transform.position = position;
    }
}
