using UnityEngine;
using System.Collections;

/// <summary>
/// Metronome class as sample accurate clock to run instruments.
/// </summary>

public class Metronome : MonoBehaviour
{
	//instantiate static object to access from other scripts
	public static Metronome metro;

    public static double startTime;

    [Range(1, 480)]
    public float BPM = 120.0f;
	[HideInInspector]
	public volatile int currentTick, currentQuarter, currentBar, samplesPerTick, ticksPerBar;
    public NotationTime currentTime;
	[HideInInspector]
	public bool ready;

	private int  samplesPerBar, samplesPerQuarter, phasor, numberOfHits, sampleRate, ticksPerQuarter, quartersPerBar;

    public double secondsPerQuarter, secondsPerTick;
    public double lastTickTime, nextTickTime;

    private NotationTime notationTick;

    void Awake () 
	{
		//wait for instrument set up
		ready = false;
		metro = this;
        startTime = AudioSettings.dspTime;
		//set up audio DSP buffer size and sample rate for whole program
		sampleRate = AudioSettings.outputSampleRate;

		ticksPerQuarter = 8;
		quartersPerBar = 4;
		ticksPerBar = ticksPerQuarter * quartersPerBar;
		//set up basic subdivision
		samplesPerBar = (int)(sampleRate / (BPM / 60.0f)) * 4;
        samplesPerQuarter = samplesPerBar / 4;
        samplesPerTick = samplesPerQuarter / ticksPerQuarter;
        secondsPerTick = (double)samplesPerTick / (double)sampleRate;
        lastTickTime = startTime;
        nextTickTime = startTime + secondsPerTick;

        currentTick = 0;
        currentQuarter = 0;
		currentBar = 0;
        currentTime = new NotationTime(0, 0, 0);
        notationTick = new NotationTime(0, 0, 1);
    }

    void Update()
    {
       // BPM++;
        samplesPerBar = (int)(sampleRate / (BPM / 60.0f)) * 4;
        samplesPerQuarter = samplesPerBar / 4;
        samplesPerTick = samplesPerQuarter / ticksPerQuarter;
        secondsPerTick = (double)samplesPerTick / (double)sampleRate;
        while (AudioSettings.dspTime > nextTickTime)
        {
            lastTickTime = nextTickTime;
            nextTickTime = lastTickTime + secondsPerTick;
            currentTick++;
            currentTime.Add(notationTick);
            if (currentTick == ticksPerQuarter)
            {
                currentTick = 0;
                currentQuarter++;
            }
            if (currentQuarter == quartersPerBar)
            {
                currentQuarter = 0;
                currentBar++;
            }
        }
    }

    public double GetFutureTime(int bar, int quarter, int tick)
    {
        int tickTime =  (bar * ticksPerBar) + (quarter * ticksPerQuarter) + tick ;
        int currentTickTime = (currentBar * ticksPerBar) + (currentQuarter * ticksPerQuarter) + currentTick;
        if (currentTickTime > tickTime) return -1;
        int futureTicks = tickTime - currentTickTime;
        return lastTickTime + (futureTicks * secondsPerTick);

    }

    public double GetFutureTime(NotationTime time)
    {
        int tickTime = (time.bar * ticksPerBar) + (time.quarter * ticksPerQuarter) + time.tick;
        int currentTickTime = (currentBar * ticksPerBar) + (currentQuarter * ticksPerQuarter) + currentTick;
		if (currentTickTime > tickTime){
			return -1;
		}
        int futureTicks = tickTime - currentTickTime;
        return lastTickTime + (futureTicks * secondsPerTick);

    }

}
