using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGoldCount : MonoBehaviour {

    public Text countText;
    private static int count;

	// Update is called once per frame
	void Update () {

        count = PlayerController.count;
        countText.text = "Gold: " + count.ToString();

    }
}
