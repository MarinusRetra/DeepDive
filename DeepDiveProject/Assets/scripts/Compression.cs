using System;
using UnityEngine;

[Serializable]
public class Compression
{
    [field: SerializeField] public string WheelName { get; private set; }
    [field: SerializeField] public float CompressionValue { get; private set; }

    public Compression(float CompressionIn, string WheelNameIn)
    { 
        CompressionValue = CompressionIn;
        WheelName = WheelNameIn;
    }

    public void SetCompression(float compressionIn)
    { 
      CompressionValue = compressionIn;
    }
}
