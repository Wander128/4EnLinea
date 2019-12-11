using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
   /// <summary>
   /// Con esta función invocamos por medio del botón Start el juego.
   /// </summary>
 
    public void IniciarGame (string juego)
    {
        SceneManager.LoadScene ("Juego");
    }
}

