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
    public AudioClip[] Achords;
    
    [Header("Guitar Inputs")] 
    public bool playerInputting;
    public int keyPresses = 0;
    public KeyCode[] inputKeys;
    public List<KeyCode> currentKeys = new List<KeyCode>();
    
    public Chords currentChord;
    public enum Chords
    {
        E, F, G, B, C, A
    }

    void Update()
    {
        //take inputs
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
            
            //clear the current keys list
            currentKeys.Clear();
        }
    }

    /// <summary>
    /// Gather the guitar's inputs and tell us if player is inputting
    /// </summary>
    void GuitarInputs()
    {
        //no key presses 
        keyPresses = 0;

        //loop through all input keys
        for (int i = 0; i < inputKeys.Length; i++)
        {
            //check if key is just pressed down
            if (Input.GetKeyDown(inputKeys[i]))
            {
                //only add if not already added
                if(currentKeys.Contains(inputKeys[i]) == false)
                    currentKeys.Add(inputKeys[i]);
                keyPresses++;
            }
            
            //check if key held
            if (Input.GetKey(inputKeys[i]))
            {   
                //only add if not already added
                if(currentKeys.Contains(inputKeys[i]) == false)
                    currentKeys.Add(inputKeys[i]);
                keyPresses++;
            }
            
            //check if key released
            if (Input.GetKeyUp(inputKeys[i]))
            {
                //remove the key since its being released
                if(currentKeys.Contains(inputKeys[i]))
                    currentKeys.Remove(inputKeys[i]);
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
                    PlayChords(Chords.A);
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
                PlaySoundMultipleAudioSources(Echords);
                break;
            case Chords.F:
                PlaySoundMultipleAudioSources(Fchords);
                break;
            case Chords.G:
                PlaySoundMultipleAudioSources(Gchords);
                break;
            case Chords.B:
                PlaySoundMultipleAudioSources(Bchords);
                break;
            case Chords.C:
                PlaySoundMultipleAudioSources(Cchords);
                break;
            case Chords.A:
                PlaySoundMultipleAudioSources(Achords);
                break;
        }

        currentChord = chordCombo;
        showRhythm = false;
    }

    void PlayOpenNotes()
    {
        PlaySoundMultipleAudioSources(openNotes);
        showRhythm = false;
    }
}
