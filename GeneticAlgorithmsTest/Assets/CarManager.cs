using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{

    public GameObject carGO;

    public float[] carsFitness;
    public GameObject[] cars;

    public bool isSetUp;
    public bool haveCarsCollided;

    public int NumStillAlive;

    private int population;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void setUp(int pop) {
        population = pop;
        cars = new GameObject[population];
        carsFitness = new float[population];
    }

    public void setUpCars(List<NeuralNetwork> carNeuralNetworks) {
        for (int i = 0; i < population; i++) {
            cars[i] = Instantiate(carGO, this.transform);
            cars[i].GetComponent<CarController>().carNeuralNetwork = carNeuralNetworks[i];
            carsFitness[i] = 0f;
        }
        isSetUp = true;
        haveCarsCollided = false;
    }

    public void cleanUp() {
        for (int i = 0; i < population; i++) {
            Destroy(cars[i]);
            carsFitness[i] = 0f;
        }
        haveCarsCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetUp) {
            NumStillAlive = population;
            for (int i = 0; i < population; i++) {
                if (cars[i].GetComponent<CarController>().hasCollided) {
                    NumStillAlive -= 1;
                    carsFitness[i] = cars[i].GetComponent<CarController>().TotalDistance;
                }
            }

            if (NumStillAlive == 0) {
                haveCarsCollided = true;
                isSetUp = false;
            }
        }
    }
}
