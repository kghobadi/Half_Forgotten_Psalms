using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Guitar Playing 
/// </summary>
public class GuitarController : RhythmProducer
{
    [Header("Guitar Sounds")] 
    public AudioClip[] openNotes;
    public AudioClip[] Echords;
    public AudioClip[] Fchords;
    public AudioClip[] Gchords;
    
    [Header("Guitar Inputs")] 
    public bool playerInputting;

    public Chords currentChord;
    public enum Chords
    {
        E, F, G, 
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        GuitarInputs();
        
        if (showRhythm)
        {
            if (playerInputting)
            {
                PlayChords(currentChord);
            }
            else
            {
                PlayOpenNotes();
            }
        }
    }

    void GuitarInputs()
    {
        //no key presses 
        int keyPresses = 0;
        
        //inputs
        if (Input.GetKey(KeyCode.T))
        {
            currentChord = Chords.E;

            keyPresses++;
        }
        //inputs
        if (Input.GetKey(KeyCode.Y))
        {
            currentChord = Chords.F;

            keyPresses++;
        }
        //inputs
        if (Input.GetKey(KeyCode.U))
        {
            currentChord = Chords.G;

            keyPresses++;
        }
        
        //are we inputting?
        if (keyPresses > 0)
        {
            playerInputting = true;
        }
        else
        {
            playerInputting = false;
        }
    }

    void PlayChords(Chords chordCombo)
    {
        switch (chordCombo)
        {
            case Chords.E:
                PlayRandomSound(Echords, 1f);
                break;
            case Chords.F:
                PlayRandomSound(Fchords, 1f);
                break;
            case Chords.G:
                PlayRandomSound(Gchords, 1f);
                break;
        }

        showRhythm = false;
    }

    void PlayOpenNotes()
    {
        PlayRandomSound(openNotes, 1f);
        showRhythm = false;
    }
}
