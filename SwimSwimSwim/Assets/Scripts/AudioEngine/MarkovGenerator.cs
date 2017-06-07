using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkovGenerator 
{
	private int[,] transitionMatrix = new int[12,12];
	RawMidiData rawMidiData = new RawMidiData();

	public MarkovGenerator()
	{
		PopulateTransitionMatrix();
	}

    //This is working @ 6/7/2017
	private void PopulateTransitionMatrix()
	{
		int previousNote;
		int currentNote;
		int noteIterator = 1;
		int numberOfNotes = rawMidiData.notes.Length;
		while(noteIterator < numberOfNotes)
		{
			previousNote = rawMidiData.notes[noteIterator - 1];
			currentNote = rawMidiData.notes[noteIterator];
			transitionMatrix[previousNote,currentNote]++; //this will add an instance to the prob tree
			noteIterator++;
		}
	}

    //this is returning incorrect results @ 6/7/2017
	public int NextNote(int previousNote)
	{
		int sumTransitions = 0;
		int randomiser;
        int iterator = 0;
		while(iterator < 12)
		{
            sumTransitions += transitionMatrix[previousNote, iterator]; //should sum all values in row so that we can chose one based on its weighting
            iterator++;

        }
        if(sumTransitions == 0)
        {
            //There is no translation prob for this note- go back to root note.
            Debug.Log("No transition prob for this note");
            return 0;
        }
        randomiser = Random.Range(1, sumTransitions + 1);
        sumTransitions = 0;
        iterator = 0;

        while (iterator < 12)
		{
            sumTransitions += transitionMatrix[previousNote, iterator];
            if (sumTransitions >= randomiser)
			{
                return iterator;
			}
            iterator++;
        }
		Debug.Log("Should never see this! Markov overflow: val " + iterator);
        Debug.Log("Randomiser = " + randomiser);
		return 0;
	}
}
