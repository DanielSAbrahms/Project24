using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ItemState
{
    Init, Equipped, InInventory, OnGround
}

public class Item : MonoBehaviour
{
    int value;
    string name;
    ItemState state;

    int[] size;
    Image iconImage; // Should match size

    Dictionary<string, float> extraProps;

    // Start is called before the first frame update
    void Start()
    {
        name = "Item Not Named";
        value = 0;
        state = ItemState.Init;

        extraProps = new Dictionary<string, float>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getExtraProp(string propName)
    {
        float propValue;
        try
        {
            extraProps.TryGetValue(propName, out float o_propValue);
            propValue = o_propValue;
            return propValue;
        } catch (Exception e)
        {
            return -1;
        }
        
    }
}
