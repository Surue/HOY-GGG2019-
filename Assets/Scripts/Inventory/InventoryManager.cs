﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager _instance;
    public static InventoryManager Instance => _instance;

    private void Awake() {
        DontDestroyOnLoad(this);
        if(_instance == null) {
            _instance = this;
        } else if(_instance != this) {
            Destroy(gameObject);
        }
    }

    public List<PickableObjectData> pickedUpObject;

    // Start is called before the first frame update
    void Start()
    {
        pickedUpObject = new List<PickableObjectData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject(PickableObjectData p)
    {
        pickedUpObject.Add(p);
    }
}
