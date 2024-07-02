# ENG
# AvatarFacial
The repository has implemented some novel interation design in VR. Using the SDK Movement.
## Functions implemented

I have used the official SDK for [Oculus Movements](https://github.com/oculus-samples/Unity-Movement) and added several features on top of it:

1. **Data Collection**: Gathering facial, body, and eye movement data, which can be written to files at a specified frame rate.

2. **VR World Playback**: The ability to replay VR world experiences by reading previously recorded data and decoding audio stored in WAV files, allowing for a mirror-like review of past experiences.

3. **Face-to-Face Communication**: Enabling face-to-face communication within the VR world, allowing for interaction between users with multiple VR devices in a virtual environment.

4. **Facial Expression Fine-Tuning**: Making fine adjustments to facial expressions by modifying blendshapes in facial modeling to control and enhance the range of facial expressions.

5. **Virtual Cinema**: Creating a virtual cinema within the VR world, providing users with a cinematic experience.
   
## Update 2023.12.29
### DEMOs:

1. VRFace2Face Chat: [view here]()
2. MirrorPlayback: [view here](https://www.youtube.com/watch?v=puNORFzl48w)
3. Virtual Theater: a immersive cinema, [view here](https://www.youtube.com/watch?v=2zthpene_yg)

### More function:

1. blendshape(AU) multiplier: multipliers that can change the amplitude of different facial blendshapes. the implementation of them varies a lot bet different emotions.
2. Gaze Cursor: A cursor motivated by your gaze direction.
