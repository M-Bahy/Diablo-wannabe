using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Selection_Script : MonoBehaviour
{
    public static GameObject player;
    public GameObject Wizard;
    public GameObject Barbarian;
    private void Awake() {
        SetPlayer();
        DistributeThePlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayer()
    {
        if(MainMenu_Script.isWizard){
            player = Wizard;
            Destroy(Barbarian);
        }
        else{
            player = Barbarian;
            Destroy(Wizard);
        }
    }
    public void DistributeThePlayer(){
        HUD_Script.player = player;
        Camera_Movement.player = player;   
    }
}
