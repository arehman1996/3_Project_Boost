using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0,1)]
    [SerializeField]
    float movementFactor; //0 for not moved. 1 for fully moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //set movement factor

        //to do - set protection against period being 0
        if (period <= Mathf.Epsilon) { return; } //mathf.epsilon means smallest value possible
        float cycles = Time.time / period; //grows continously from 0

        const float tau = Mathf.PI * 2f; //about 6.28. tau value explained in lecture
        float rawSinWave = Mathf.Sin(cycles * tau); //Sin is the motion around a circle. explained in lecture. goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
