using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

  public Text[] driveTexts;

  public void SetDriveText(int driveId, string driveText){
    driveTexts[driveId].text = "Drive " + driveId.ToString() + " - " + driveText;
  }
}
