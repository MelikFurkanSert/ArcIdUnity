using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Vector3 distance;
    [SerializeField] private float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, character.transform.position + distance, cameraSpeed * Time.deltaTime);
    }
}
