using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public bool hasWood = true;

    public bool ClaimWood() {
        bool givesWood = hasWood;
        hasWood = false;
        return givesWood;
    }
}
