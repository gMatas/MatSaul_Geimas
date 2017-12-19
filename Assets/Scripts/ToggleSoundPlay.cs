using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ToggleSoundPlay : MonoBehaviour
{
  
    public Toggle tggl;
   
    public void ActiveToggle()
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (tggl.isOn)
        {      
            audio.Play();
            audio.Play(44100);
        } 
       
        else 
        {
            audio.Pause();
        } 
        
    }

    public void OnSubmit()
    {
        ActiveToggle();
    }
}