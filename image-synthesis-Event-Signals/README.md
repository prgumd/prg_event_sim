# Image Synthesis repository with Event Signals component
Original repository for ML-ImageSynthesis can be found [here](https://bitbucket.org/Unity-Technologies/ml-imagesynthesis/src/master/)

## Event Signals [in-progress]
A Unity component for Camera objects which calculates the log value of absolute pixel differences between subsequent frames in real-time to then only display those pixels whose associated value passes a threshold, thus simulating data that could be generated from a Dynamic Vision Sensor (DVS).  

Found in Assets/EventSignals

Note that this component is not integral to the project as a whole, as [DAVIS simulator](https://github.com/uzh-rpg/rpg_davis_simulator) can be used to obtain a simulated DVS dataset

![Alt Text](https://media.giphy.com/media/J14JURlOBcTcQs51pp/giphy.gif)
