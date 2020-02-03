using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "Pickup")]
public class Pickups : ScriptableObject
{
    public string _name, _description;
    public float _fallingSpeed, _timeToLive;
    public AudioClip _getPickupSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
