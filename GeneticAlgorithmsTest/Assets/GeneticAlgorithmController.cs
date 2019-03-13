using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithmController : MonoBehaviour {

    public int totalPopulation = 10;
    public CarManager cm;

    public float bestDistanceTillNow = 0;
    public float bestDistance = 0.0f;
    public int bestIndex = 0;
    public float secondBestDistance = 0.0f;
    public int secondBestIndex = 0;
    int generation = 0;
    
    public GameObject Car;
    public bool bestTwo = true;

    List<NeuralNetwork> children = new List<NeuralNetwork>();

    //For GUI
    public Text Populationtext;
    public Text NeuralNetworkInfo;

	// Use this for initialization
	void Start () {
        cm.setUp(totalPopulation);
            
        //Create initial population
        for (int i = 0; i < totalPopulation; i++)
        {
            children.Add(new NeuralNetwork());
        }
        StartCoroutine(Simulate());      
	}

	// Update is called once per frame
	void Update () {
        Populationtext.text = "Generation: " + generation
            + "\nBest: "+ bestDistanceTillNow;
	}

    void ResetCar()
    {
        Car.transform.position = new Vector3(0, 0.51f, -239);
        Car.transform.eulerAngles = new Vector3(0, 0, 0);
        Car.GetComponent<CarController>().TotalDistance = 0;
        Car.GetComponent<CarController>().hasCollided = false;
        Car.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Car.gameObject.GetComponent<CarController>().carLastLocation = new Vector3(0, 0.51f, -239);
    }

    IEnumerator Simulate()
    {
        while (true)
        {
            generation++;
            bestDistance = 0;
            secondBestDistance = 0;
            cm.setUpCars(children);

            yield return new WaitUntil(()=>cm.haveCarsCollided);

            for (int count = 0; count < totalPopulation; count++)
            {
                float dist = cm.carsFitness[count];
                if (dist > bestDistanceTillNow)
                    bestDistanceTillNow = dist;

                if (dist > bestDistance)
                {
                    secondBestDistance = bestDistance;
                    secondBestIndex = bestIndex;
                    bestDistance = dist;
                    bestIndex = count;
                }
                else if (dist > secondBestDistance)
                {
                    secondBestDistance = dist;
                    secondBestIndex = count;
                }

            }

            NeuralNetwork A = new NeuralNetwork(children[bestIndex]);
            NeuralNetwork B = new NeuralNetwork(children[secondBestIndex]);
                
            children.Clear();

            if (bestTwo) {
                children.Add(A);
                children.Add(B);
                for (int i=2; i<totalPopulation; i++)
                {
                    children.Add(new NeuralNetwork(A, B));
                }
            } else {
                for (int i=0; i<totalPopulation; i++)
                {
                    children.Add(new NeuralNetwork(A, B));
                }
            }
            

            cm.cleanUp();

        }
    }
}
