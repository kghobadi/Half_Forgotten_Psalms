using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class RhythmProducer : AudioHandler {
    protected GameObject player;
    
    public bool showRhythm;

    public bool randomTimeScale;
    public TickValue timeScale;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");

        if (randomTimeScale)
        {
            RandomizeTimeScale();
        }
    }

    public void RandomizeTimeScale()
    {
        //get random int
        int newTime = UnityEngine.Random.Range(1, 10);
        //set new time scale
        switch (newTime)
        {
            case 0:
                timeScale = TickValue.Max;
                break;
            case 1:
                timeScale = TickValue.Measure;
                break;
            case 2:
                timeScale = TickValue.Half;
                break;
            case 3:
                timeScale = TickValue.Quarter;
                break;
            case 4:
                timeScale = TickValue.QuarterTriplet;
                break;
            case 5:
                timeScale = TickValue.Eighth;
                break;
            case 6:
                timeScale = TickValue.EighthTriplet;
                break;
            case 7:
                timeScale = TickValue.Sixteenth;
                break;
            case 8:
                timeScale = TickValue.SixteenthTriplet;
                break;
            case 9:
                timeScale = TickValue.ThirtySecond;
                break;
        }
    }

    public virtual void OnEnable()
    {
        SimpleClock.ThirtySecond += OnThirtySecond;
    }

    public virtual void OnDisable()
    {
        SimpleClock.ThirtySecond -= OnThirtySecond;
    }

    public virtual void OnThirtySecond(BeatArgs e)
    {
        switch (timeScale)
        {
            case TickValue.Max:
                if (e.TickMask[TickValue.Max])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.Measure:
                if (e.TickMask[TickValue.Measure])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.Half:
                if (e.TickMask[TickValue.Half])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.Quarter:
                if (e.TickMask[TickValue.Quarter])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.QuarterTriplet:
                if (e.TickMask[TickValue.QuarterTriplet])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.Eighth:
                if (e.TickMask[TickValue.Eighth])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.EighthTriplet:
                if (e.TickMask[TickValue.EighthTriplet])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.Sixteenth:
                if (e.TickMask[TickValue.Sixteenth])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.SixteenthTriplet:
                if (e.TickMask[TickValue.SixteenthTriplet])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
            case TickValue.ThirtySecond:
                if (e.TickMask[TickValue.ThirtySecond])
                {
                    // rhythm creation / beat visual
                    showRhythm = true;
                }
                break;
        }

    }
    
}
