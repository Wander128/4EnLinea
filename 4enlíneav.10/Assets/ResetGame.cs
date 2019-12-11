using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour

{/// <summary>
/// Con esta función reinciamos la escena del juego, sin afectar la jugabilidad.
/// </summary>
    public void ReiniciarGame ()
    {
        SceneManager.LoadScene("Juego");
        
    }
}



