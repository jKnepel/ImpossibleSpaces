using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float ID { get { return _ID; } }
    [SerializeField] private float _ID;
}
