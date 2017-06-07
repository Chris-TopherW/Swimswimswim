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
	public int NextNote(int previousNote)
	{
		int sum = 0;
		int randomiser;
		for(int i = 0; i < 12; i++)
		{
			sum += transitionMatrix[previousNote,i];
		}
		randomiser = Random.Range(0, sum);
		sum = 0;
		for(int i = 0; i < 12; i++)
		{
			sum += transitionMatrix[previousNote,i];
			if(sum >= randomiser)
			{
				return transitionMatrix[previousNote,i];
			}
		}
		Debug.Log("Should never see this! Markov overflow");
		return 0;
	}
}
