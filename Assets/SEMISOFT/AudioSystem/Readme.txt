SEMISOFT Custom AudioSystem
v1.2

made to accelerate and simplify sound effect and bgm integration for casual and hypercasual games

installation:
Unpack the UnityPackage to your project, it is recommended to use the default directory and not move the AudioSystem folder. If you don't use the default directory, then you will need to adjust the const fields for paths in AudioSystemEditor.cs

how to use:
1. Register your audio asset to the audiosystem database
	a. put your audio assets to Assets/Resources/AudioAssets
	b. go to Tools menu on the menu bar, SEMISOFT->AudioSystem->Generate Audio Events
	c. this will create AudioDB in Resources and AudioEventEnum for each of your audio assets
2. Load the audio clips to the memory on the start of the game
	call the coroutine AudioSystem.LoadAllAudio(), don't forget to add listener to AudioSystem.onAudioLoadFinished to do something after the audio is loaded
3. Play the sound
	call the functions AudioSystem.PlayAudioxxxxx and pass the sound you want to play as the AudioEventEnum parameter, pass the GameObject that will be audio source or use default. It is highly recommended that you use different audio source for BGM and SFX so that playing SFX doesn't interrupt the BGM

supported extensions:
For now, this AudioSystem can work with Unity's default supported audio extensions, which are: .aif, .mp3, .ogg, .wav
