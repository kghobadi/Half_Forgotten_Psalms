using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GuitarInput
{
    public KeyCode inputKey;
    public GuitarController.Chords chordType;
    public AudioClip[] chords;
    public ParticleSystem chordParticles;
    public Color chordColor;
}

/// <summary>
/// Handles the Guitar Playing 
/// </summary>
public class GuitarController : RhythmProducer
{
    [Header("Guitar Sounds")] 
    public AudioClip[] openNotes;
    public ParticleSystem bassParticles;
    
    [Header("Guitar Inputs")] 
    public bool playerInputting;
    public int keyPresses = 0;
    public GuitarInput[] allGuitarInputs;
    public List<KeyCode> currentKeys = new List<KeyCode>();
    
    public enum Chords
    {
        E, F, G, B, C, A
    }

    [Header("UI")] 
    public FadeUI[] guitarUI;
    
    void Update()
    {
        //take inputs
        GuitarInputs();
        
        //on beat
        if (showRhythm)
        {
            //always play open notes
            PlayOpenNotes();
            
            //check inputting
            if (playerInputting)
            {
                AnalyzeInputs();

                //fade out guitar UI after input
                if (guitarUI[0].gameObject.activeSelf)
                {
                    for (int i = 0; i < guitarUI.Length; i++)
                    {
                        guitarUI[i].FadeOut();
                    }
                }
            }

            //show rhythm ends 
            showRhythm = false;
        }

        //click to strum -- this doesn't really sound good. feels a bit unnecessary for now?
        // if (Input.GetMouseButtonDown(0))
        // {
        //     AnalyzeInputs();
        // }
    }

    /// <summary>
    /// Gather the guitar's inputs and tell us if player is inputting
    /// </summary>
    void GuitarInputs()
    {
        //no key presses 
        keyPresses = 0;

        //loop through all input keys
        for (int i = 0; i < allGuitarInputs.Length; i++)
        {
            //input key of this Guitar Input 
            KeyCode inputKey = allGuitarInputs[i].inputKey;
            
            //check if key is just pressed down
            if (Input.GetKeyDown(inputKey))
            {
                //only add if not already added
                if(currentKeys.Contains(inputKey) == false)
                    currentKeys.Add(inputKey);
                keyPresses++;
            }
            
            //check if key held
            if (Input.GetKey(inputKey))
            {   
                //only add if not already added
                if(currentKeys.Contains(inputKey) == false)
                    currentKeys.Add(inputKey);
                keyPresses++;
            }
            
            //check if key released
            if (Input.GetKeyUp(inputKey))
            {
                //remove the key since its being released
                if(currentKeys.Contains(inputKey))
                    currentKeys.Remove(inputKey);
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
        //loop through currently pressed keys
        for (int i = 0; i < currentKeys.Count; i++)
        {
            //get the GuitarInput struct according to the key being pressed
            GuitarInput guitarInput = GetGuitarInput(currentKeys[i]);
            //play the correct audio array 
            PlaySoundMultipleAudioSources(guitarInput.chords);
            //play correct chord particles
            guitarInput.chordParticles.Play();
            
        }
    }

    /// <summary>
    /// Takes an input key and returns a specific GuitarInput struct from allguitarInputs
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    GuitarInput GetGuitarInput(KeyCode key)
    {
        for (int i = 0; i < allGuitarInputs.Length; i++)
        {
            KeyCode keyToCompare = allGuitarInputs[i].inputKey;
            //is it the right key?
            if (key == keyToCompare)
            {
                return allGuitarInputs[i];
            }
        }
        
        //just return an empty
        GuitarInput guitarInput = new GuitarInput();
        return guitarInput;
    }

    void PlayOpenNotes()
    {
        PlaySoundMultipleAudioSources(openNotes);
        bassParticles.Play();
    }
}
