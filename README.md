# MLAgentsUnityBees
Bee agents trained to gather pollen in 3 dimensions in Unity. 
![Work please](https://user-images.githubusercontent.com/35940692/235979819-cc5743ab-edee-4182-987b-42f495d59219.gif)

Built on Pytorch and Unity ML Agents

The most technically intresting part of theis project was the reward system. I had started with a curriculum where I was pretraining the model to touch the pollen, then running a second training to bring the polen to the hive, but I ended up having much faster trainings by bundling all the rewards into the same training, where each step in the training gives 100x the rewards of the previose step. 



Final Training output logs  (Will update to include logs of other versions and TenserBoard outputs by 5/14)
![image](https://user-images.githubusercontent.com/35940692/235981029-a6a6d01b-156e-4b71-9050-c41e5ed05e09.png)

