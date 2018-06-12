﻿using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{        
    // Leaving for now. TODO: remove this at some point?
    public bool noteCtrlOn = true;

    public int minMidiNote = 36;                                // Note Range for Theremini (Configurable in Theremini settings)
    public int maxMidiNote = 72;

    public int startDelay;                                      // How long to wait before starting system
    public TimerCtrl timer;

    public GameObject completedMenuPrefab;                      // Menu Prefab to instantiate when a score is completed

    // Start Menu Input Items
    public string ParticipantID { get; set; }                   // Participant ID
    public string SessionNum { get; set; }                      // Session Number - to help keep track of log files
    public string MidiScoreResource { get; set; }               // Name of file containing Midi data
    public string MidiInputDeviceName { get; set; }             // Name of Midi Device to connect to

    // VRMin Components
    public static PlayerCtrl Control { get; private set; }      // Singleton Accessor
    public LogWriter Logger { get; private set; }               // Logger
    public MidiInputCtrl MidiIn { get; private set; }           // Main Midi Input Controller used to position left and right hands

    private GameObject completedMenu;                           // The instantiated completed menu game object

    void Awake ()
    {
        //Implement Psuedo-Singleton
        if (Control == null)
        {
            DontDestroyOnLoad(gameObject);
            Control = this;
        }
        else if (Control != this)
        {
            Destroy(gameObject);
        }

        //Initialize Midi In (so objects can subscribe to it upon load)
        MidiIn = new MidiInputCtrl();

        //Initialize Logger
        Logger = new LogWriter();
    }

    public void StartVRMin()
    {
        // Only need to startup these items once per at the beginning
        Logger.Start(string.Format("p{0}-session{1}-midi-log", ParticipantID, SessionNum));      // Start Up the Logger
        MidiIn.Connect(MidiInputDeviceName);                                                     // Connect to the Midi Device
        MidiIn.Start();

        // Start Notes on a Delay
        StartCoroutine(DelayedStart(startDelay));  
    }

    public void StartNewScore()
    {
        StartCoroutine(DelayedStart(startDelay));
    }

    private IEnumerator DelayedStart(int delay)
    {
        timer.StartProgressBar(delay, "Starting");
        yield return new WaitForSecondsRealtime(delay);

        // Start Playing notes
        NoteCtrl.Control.MidiScoreFile = MidiScoreResource;
        NoteCtrl.Control.PlayMidi(NoteCtrl.MidiStatus.Play);
    }

    public void MidiComplete()
    {   
        //instatiate completed menu and set player controller to this
        completedMenu = Instantiate(completedMenuPrefab);
    }

    void OnDisable()
    {
        // Clean up when controller is destroyed
        if(MidiIn != null)
            MidiIn.StopAndClose();
        if (Logger != null)
            Logger.Stop();
    }
}
