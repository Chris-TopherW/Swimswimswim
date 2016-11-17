using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;

//[ExecuteInEditMode]
//public class FillBuffers : MonoBehaviour
//{
//
//}

/// <summary>
/// Instrument voice class for audio sequencer.
/// </summary>

public class SequencerInstrument : MonoBehaviour
{	
	public AudioClip		 	audioClip;
	public AudioSource 			audioSource;
//	public AudioMixer 			mixer;
	[HideInInspector]
	public bool[] 				score;
	public float 				pan;
	private int 				numSamples, DSPBufferingSize, playhead, phasor;
	private float 				leftGain, rightGain;
	private float[] 			sampleBuffer;
	private bool 				processAudio;

	void Awake() {
		audioSource = GetComponent<AudioSource> ();
		score = new bool[Metronome.metro.ticksPerBar];
	}

	void Start () {
		numSamples = audioClip.samples;
		DSPBufferingSize = Metronome.metro.getDSPBufferSize ();

		//setup mixer output
//		string _OutputMixer = ("Channel" + this.name);       
//		audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(_OutputMixer)[0];
		//setup input buffer to store clip of floats and set to end of buffer
		sampleBuffer = new float[ numSamples ];
		audioClip.GetData (sampleBuffer, 0);
		playhead = numSamples - 1;
		//pan = -0.5f;
		audioSource.panStereo = pan;
		leftGain = scale(-1.0f, 1.0f, 1.0f, 0.0f, pan);
		rightGain = scale(-1.0f, 1.0f, 0.0f, 1.0f, pan);
		//wait for everything to be set up then start metro
		Metronome.metro.ready = true;
	}

	//to place samples in specific channels
	// i % channels == x, where x is channel from 0 to channels -1,
	// For example if audio is stereo, then channel 0 will be left, channel 1 will be right so
	// i % channels == 0 will hold true on left channel.
	void OnAudioFilterRead( float[] samples, int channels ) {
		for ( int i = 0; i < DSPBufferingSize; i++ )  {
			phasor++;
			if ( playhead < numSamples - 1 ) {
				playhead++;
			}
			if ( phasor == Metronome.metro.samplesPerTick ) {
				phasor = 0;
				if( score[ Metronome.metro.currentTick ] == true ) {
					playhead = 0;
				}
			}
			if ( playhead < numSamples -1 ) {
				if( i % 2 == 0 ) {
				samples[ i ] = sampleBuffer[ playhead ] * leftGain;
				} else {
					samples[ i ] = sampleBuffer[ playhead ] * rightGain;
				}
			} else {
				samples[ i ] = 0;
			}
		}
	}

	public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

		return(NewValue);
	}

		public float dBToVolume( float dB ){
		return (float)Math.Pow( 10.0f, 0.05f * dB );
		}
		public float VolumeTodB( float volume ) {
		return 20.0f * (float)Math.Log10( volume );
		}
}
