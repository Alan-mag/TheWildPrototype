// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System;
using System.Diagnostics;
using Niantic.Lightship.Maps.Coordinates;
using UnityEngine;

namespace Niantic.Lightship.Maps.Samples.Common.Coordinates
{
    /// <summary>
    /// A serializable version of <see cref="LatLng"/> (which, because it's
    /// a readonly struct, can't easily be modified in the Unity Editor).
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{Latitude}, {Longitude}")]
    public class SerializableLatLng
    {
        [SerializeField]
        private double _latitude;

        [SerializeField]
        private double _longitude;

        public double Latitude => _latitude;
        public double Longitude => _longitude;

        public SerializableLatLng(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public static implicit operator LatLng(SerializableLatLng value) =>
            new LatLng(value._latitude, value._longitude);

        public static implicit operator SerializableLatLng(in LatLng value) =>
            new SerializableLatLng(value.Latitude, value.Longitude);
    }
}
