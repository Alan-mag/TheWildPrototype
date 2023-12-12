// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System;
using TMPro;
using UnityEngine;

namespace Niantic.Lightship.Maps.Samples.GameSample.FloatingText
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textField;

        private void Start()
        {
            // destroy self after it's finished anim
            Destroy(gameObject, 2.0f);
        }

        public void SetText(string text)
        {
            _textField.text = text;
        }
    }
}
