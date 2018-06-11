﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    public GameObject parentCanvas;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickHandler);
    }


    private void OnClickHandler()
    {
        if (string.IsNullOrEmpty(PlayerCtrl.Control.MidiInputDeviceName))
            throw new System.ArgumentException("No MIDI device provided");

        if (string.IsNullOrEmpty(PlayerCtrl.Control.MidiScoreResource))
            throw new System.ArgumentException("No MIDI score provided");

        if (string.IsNullOrEmpty(PlayerCtrl.Control.ParticipantID))
            throw new System.ArgumentException("No participant ID provided");

        PlayerCtrl.Control.StartVRMin();

        Destroy(parentCanvas);
    }
}