using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Store if we hit a ray
    RaycastHit rhFront, rhvecLeft, rhvecRight, rhvecvecLeft45, rhvecvecRight45; 

    //All the vectors we use for the program for detection
    Vector3 vecvecLeft45 = new Vector3(-0.5f,0, 0.5f);
    Vector3 vecvecRight45 = new Vector3(0.5f, 0, 0.5f);
    Vector3 vecLeft = new Vector3(-1, 0, 0);
    Vector3 vecRight = new Vector3(1, 0, 0);

    //floats to store distances from hit checks
    public float d_front = 10f, d_left45 = 10f, d_right45 = 10f, d_left = 10f, d_right = 10f;

    //Length of Collision Checking
    public float maxDistanceSeen = 10f;

    //Simple holders
    Rigidbody rb;
    SimpleCarController scc;

    //Finish GameObject
    public GameObject finishGO;

    //NeuralNetwork association
    public NeuralNetwork carNeuralNetwork;

    //Variables
    double steering, acceleration;
    public float TotalDistance = 0;
    public bool hasCollided = false;
    public float carSpeed = 0f;
    public Vector3 carLastLocation;

    //Time Waiting
    public double newTime;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scc = GetComponent<SimpleCarController>();
        carLastLocation = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 thisPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Vector3.forward), out rhFront, maxDistanceSeen))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Vector3.forward) * rhFront.distance, Color.yellow);
            d_front = rhFront.distance;
        } else { d_front = maxDistanceSeen; }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(vecvecLeft45), out rhvecvecLeft45, maxDistanceSeen))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(vecvecLeft45) * rhvecvecLeft45.distance, Color.yellow);
            d_left45 = rhvecvecLeft45.distance;
        } else { d_left45 = maxDistanceSeen; }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(vecvecRight45), out rhvecvecRight45, maxDistanceSeen))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(vecvecRight45) * rhvecvecRight45.distance, Color.yellow);
            d_right45 = rhvecvecRight45.distance;
        } else { d_right45 = maxDistanceSeen; }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(vecLeft), out rhvecLeft, maxDistanceSeen))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(vecLeft) * rhvecLeft.distance, Color.yellow);
            d_left = rhvecLeft.distance;
        } else { d_left = maxDistanceSeen; }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(vecRight), out rhvecRight, maxDistanceSeen))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(vecRight) * rhvecRight.distance, Color.yellow);
            d_right = rhvecRight.distance;
        } else { d_right = maxDistanceSeen; }
    }

    void FixedUpdate() {

        newTime += Time.deltaTime;
        if (newTime > 60f) {
            newTime = 0f;
            TotalDistance = Vector3.Distance( this.transform.position, new Vector3(0, 0.51f, -239) );
            hasCollided = true;
        }
        
        if ((!hasCollided) || carNeuralNetwork != null) {
            //This is the total distance covered by the car; We are not using this heuristic anymore.
            //TotalDistance += Vector3.Distance(this.transform.position, carLastLocation);
            carSpeed = Vector3.Distance(this.transform.position, carLastLocation) / Time.deltaTime;
            carLastLocation = transform.position;

            float[] Inputs = { carSpeed, d_front, d_left, d_right, d_left45 , d_right45 };
            carNeuralNetwork.predict(Inputs, ref steering, ref acceleration);
            //Debug.Log(steering + " " + acceleration + " " + inSpeed + " " + inFrontDist + " " + inLeftDistance + " " + inRightDistance);

            scc.move((float)acceleration, (float)steering);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish") {
            Debug.Log("FINISH");
            carNeuralNetwork.save();
            //Application.Quit();
        }

        if (other.tag == "Wall") {
            //Debug.Log("Collided!");
            //Debug.Log( Vector3.Distance(finishGO.transform.position, transform.position) );
            newTime = 0f;
            TotalDistance = Vector3.Distance( this.transform.position, new Vector3(0, 0.51f, -239) );
            hasCollided = true;
        }
    }
}
