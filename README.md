# Using Genetic Algorithms to create a Self-Driving Car

This is a project I did to learn about Neural Networks and Genetic Algorithms. We use create a neural network to drive a car and then use genetic algorithms to train and find the best weights for the neural network. The network takes in 6 inputs and outputs the acceleration and steering of the car.

## Examples

Here, we see all the cars beginning to get initialized with a random neural network. They are pretty dumb.

![MLOG](https://github.com/AmritAmar/GeneticSelfDrivingCars/blob/master/Gifs/FirstGen.gif)

In the editor, this is the information that cars get (as shown by the yellow lines). 

![ML1](https://github.com/AmritAmar/GeneticSelfDrivingCars/blob/master/Gifs/NNView.gif)

By generation 22, we have a car that can finish the whole track (albeit not smoothly)!

![ML2](https://github.com/AmritAmar/GeneticSelfDrivingCars/blob/master/Gifs/BestCar.gif)

## Getting Started

Simply clone and open this Unity project. The scene used called SampleScene. Just hit play and watch the cars run. The initial population chosen is 10. On average, it takes about 22 generations to train the neural network fully.

The neural network has 6 input nodes:
- current speed
- distance of closest obstacle from the left side (max 10m)
- distance of closest obstacle from the right side (max 10m)
- distance of closest obstacle from the front (max 10m)
- distance of closest obstacle from the front-left side (max 10m)
- distance of closest obstacle from the front-right side (max 10m)

There is 1 hidden layer, consisting of 5 nodes.
There is 1 output layer, consisting of 2 nodes:
- Acceleration (-1 to 1)
- Steering (-1 to 1)

I use tanH as my activation function.

There are 3 main scripts:
- CarController: this is the script for a car. Has a neural network that drives the car.
- CarManager: manager for the population of cars
- NeuralNetwork: holds the neural network code
- GeneticAlgorithmController: performs the selection process of the algorithm

The program works as follows:

The input and hidden layer nodes have both a weight and a bias, that is randomly generated for the population of cars (default 10). They are then simulated as generation 1 on the race track. After all the cars crash or 60 seconds, the algorithm chooses the best 2 cars (based on displacement from starting point) and then creates the next generation of cars by combining the best 2 cars genes (randomly selecting between the 2 or a combination thereof). There is an option that allows us to keep the best 2 cars in the next generation or to generate all new cars in the population from the parents (i.e. the parents die). The next generation is then simulated and so on.

## Future Plans

In the future, I would like to clean up the NeuralNetwork class and make it less hard-coded and more dependent on variables. I would also like to move from Genetic Algorithms on to implementing self-driving cars using Q-Learning and Policy Iteration.

## Authors

* **Amrit Amar** - *Coding* - [AmritAmar](https://github.com/AmritAmar)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
