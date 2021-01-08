using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10, 10f);
    [SerializeField] float period = 2f;
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon)
        {
            return;
        }
        float cycle = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycle * tau);
        movementFactor = rawSinWave / 2f + .5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;

    }
}
