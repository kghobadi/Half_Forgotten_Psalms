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
    public AudioClip[] Bchords;
    public AudioClip[] Cchords;
    
    [Header("Guitar Inputs")] 
    public bool playerInputting;
    int keyPresses = 0;
    public KeyCode[] inputKeys;
    private List<KeyCode> currentKeys = new List<KeyCode>();
    
    public Chords currentChord;
    public enum Chords
    {
        E, F, G, B, C,
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        GuitarInputs();
        
        //on beat
        if (showRhythm)
        {
            //check inputting
            if (playerInputting)
            {
                AnalyzeInputs();
            }
            else
            {
                PlayOpenNotes();
            }
        }
    }

    /// <summary>
    /// Gather the guitar's inputs and tell us if player is inputting
    /// </summary>
    void GuitarInputs()
    {
        //no key presses  -- clear keys list
        keyPresses = 0;
        currentKeys.Clear();

        //loop through all input keys
        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKey(inputKeys[i]))
            {
                currentKeys.Add(inputKeys[i]);
                keyPresses++;
            }
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

    /// <summary>
    /// Decide how to respond to player's inputs
    /// </summary>
    void AnalyzeInputs()
    {
        //loop through currently press keys
        for (int i = 0; i < currentKeys.Count; i++)
        {
            //play the chord array according to the pressed key
            switch (currentKeys[i])
            {
                case KeyCode.T :
                    PlayChords(Chords.E);
                    break;
                case KeyCode.Y :
                    PlayChords(Chords.F);
                    break;
                case KeyCode.U :
                    PlayChords(Chords.E);
                    break;
                case KeyCode.G :
                    PlayChords(Chords.G);
                    break;
                case KeyCode.H :
                    PlayChords(Chords.B);
                    break;
                case KeyCode.J :
                    PlayChords(Chords.C);
                    break;
            }
            
        }
    }

    /// <summary>
    /// Plays the correct audio array according to the passed Chord type. 
    /// </summary>
    /// <param name="chordCombo"></param>
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
            case Chords.B:
                PlayRandomSound(Bchords, 1f);
                break;
            case Chords.C:
                PlayRandomSound(Cchords, 1f);
                break;
        }

        currentChord = chordCombo;
        showRhythm = false;
    }

    void PlayOpenNotes()
    {
        PlayRandomSound(openNotes, 1f);
        showRhythm = false;
    }
}
