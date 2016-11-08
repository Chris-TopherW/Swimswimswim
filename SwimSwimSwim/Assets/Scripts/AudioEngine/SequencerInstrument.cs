using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

/// <summary>
/// Instrument voice class for audio sequencer.
/// </summary>

public class SequencerInstrument : MonoBehaviour
{	
	public AudioClip audioClip;
	public AudioSource audioSource;
	public AudioMixer mixer;

	[HideInInspector]
	public bool[] score;

	private int numSamples, DSPBufferingSize, playhead, phasor;
	private float gain = 1.0f;
	private float[] sampleBuffer;
	private bool processAudio;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
		score = new bool[Metronome.metro.ticksPerBar];
	}

	void Start ()
	{
		numSamples = audioClip.samples;
		DSPBufferingSize = Metronome.metro.getDSPBufferSize ();

		//setup mixer output
		string _OutputMixer = ("Channel" + this.name);       
		audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];

		//setup input buffer to store clip of floats and set to end of buffer
		sampleBuffer = new float[numSamples];
		audioClip.GetData (sampleBuffer, 0);
		playhead = numSamples - 1;
		//new GameObject

		//wait for everything to be set up then start metro
		Metronome.metro.ready = true;
	}

	//to place samples in specific channels
	// i % channels == x, where x is channel from 0 to channels -1,
	// For example if audio is stereo, then channel 0 will be left, channel 1 will be right so
	// i % channels == 0 will hold true on left channel.
	void OnAudioFilterRead(float[] samples, int channels)
	{
		for (int i = 0; i < DSPBufferingSize; i++) 
		{
			phasor++;
			if (playhead < numSamples - 1)
				playhead++;

			if (phasor == Metronome.metro.samplesPerTick) {
				phasor = 0;

				if(score[Metronome.metro.currentTick])
					playhead = 0;
				}
			if (playhead < numSamples -1){
				samples [i] = sampleBuffer[playhead] * gain;
			} else {
				samples[i] = 0;
			}
		}
	}
}
