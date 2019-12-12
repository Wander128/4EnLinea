using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject[,] grid; //Malla de esferas
    private int heigth = 10; // Ancho
    private int width = 10; // Alto
    private bool player1 = true; // Boleano que permite el cambio de jugador
    private bool winner; // Boleano que permite la declaración del ganador
    public Text ganador; // Variable utilizada para ingresar el texto a mostrar en pantalla. 
    private bool horizontalWins; // Variable tipo booleana que servirá para indicar un ganador de forma horizontal 
    private bool verticalWins; // Variable tipo booleana que servirá para indicar un ganador de forma vertical
    private bool diagonalWins; // Variable tipo booleana que servirá para indicar un ganador de forma diagonal
    private bool diagonalLeftWins; // Variable tipo booleana que servirá para indicar un ganador de forma diagonal izquierda
    [SerializeField] private Text ruleText; //Se crea una variable privada con SerializeField, que de igual forma aparecerá en el inspector para agregar el texto en pantalla


    void Start()
    {
        Setup(); // La función llamada contiene el código que crea la malla de esferas.
    }
    /// <summary>
    /// En este bloque de líneas (29-39) es donde se crea las esferas y asi mismo el tablero de juego de 10x10 
    /// </summary>
    private void Setup()
    {
        Regla(); // La función llamada contiene los mensajes random del switch case creado 
        grid = new GameObject[width, heigth];
        for (var i = 0; i < heigth; i++)
        {
            for (var j = 0; j < width; j++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = new Vector3(x: j, y: i, z: 0);
                grid[j, i] = go;
            }
        }
    }

    void Update()
    {
        RestoreColor(); // La función llamada permite que al pasar el mouse las esferas retornen al color origen si no es seleccionada
        /// <summary>
        /// Las líneas de codigo (51-62) Permiten la localización del mouse en el plano (tablero de juego) y de igual forma evita que al dar clic 
        /// por fuera de este, afecte el juego
        /// </summary>
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

        if (!(mousePosition.x >= -0.5f) || !(mousePosition.x < width - 0.5f) || !(mousePosition.y >= -0.5f) ||
            !(mousePosition.y < heigth - 0.5f)) return;

        var x = (int)(mousePosition.x + 0.5f);
        var y = (int)(mousePosition.y + 0.5f);
        if (!winner) //En caso de encontrar un ganador se detiene la ejecición del juego. 
        {
            PickAPiece(x, y); // La función llamada nos permitimos cambiar el color de la esfera sin seleccionarla, para así identificar la ubicación del mouse
            if (Input.GetMouseButtonUp(0))
            SelectColor(x, y); // La función llamada es la contiene el cambio de jugador y los demas parametros para jugar 
        }
    }

    private void RestoreColor() // Con esta línea generamos la condición donde al pasar el mouse por fuera de la grid no pinte la última esfera tocada. 
    {
        for (var i = 0; i < heigth; i++)
        {
            for (var j = 0; j < width; j++)
            {

                if (grid[j, i].GetComponent<Renderer>().material.color == Color.black)
                {
                    grid[j, i].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }
    /// <summary>
    /// Con esta función nos permitimos cambiar el color de la esfera sin seleccionarla, para así identificar la ubicación del mouse
    /// </summary> 
    private void PickAPiece(int x, int y)
    {
        if (grid[x, y].GetComponent<Renderer>().material.color == Color.white)
        {
            grid[x, y].GetComponent<Renderer>().material.color = Color.black;
        }
    }
    /// <summary>
    /// En esta función se encuentran ubicadas los parámetros de juego, es decir, la selección del cambio de turno del jugador, las condiciones que 
    /// determina en cual de las direcciones se ganará y, además, el jugador ganador.
    /// </summary>

    private void SelectColor(int x, int y)
    {
        if (grid[x, y].GetComponent<Renderer>().material.color != Color.black) return; 

        var assignedColor = player1 ? Color.cyan : Color.magenta;

        grid[x, y].GetComponent<Renderer>().material.color = assignedColor;

        var thereIsAWinner = false;

        if (horizontalWins)
            thereIsAWinner = HorizontalConditions(x, y, assignedColor);
        if (!thereIsAWinner && verticalWins)
            thereIsAWinner = VerticalConditions(x, y, assignedColor);
        if (!thereIsAWinner && diagonalWins)
            thereIsAWinner = DiagonalConditions(x, y, assignedColor);
        if (!thereIsAWinner && diagonalLeftWins)
            thereIsAWinner = DiagonalConditionsIz(x, y, assignedColor);
        if (thereIsAWinner)

        {
            var winnerName = player1 ? "Azul" : "Magenta"; 
            DeclareWinner(winnerName); // Función llamada para mostrar el ganador 
        }

        player1 = !player1;
    }
    /// <summary>
    /// Con esta función declaramos el ganador.
    /// </summary>

    private void DeclareWinner(string winnerName)
    {
        winner = true;
        Debug.Log(message: "El ganador es el " + winnerName); // Se imprime el nombre del ganador
        ganador.gameObject.SetActive(winner); //Con ésta línea del juego mostramos en pantalla que ha terminado el juego, pues hay un ganador.
    }

    /// <summary>
    /// A partir de la línea 138 hasta la 238 se generan las condiciones para que Unity compare las esferas de forma horizontal, vertical, diagonal derecha e izquierda.
    /// En donde al no encontrar 4 esferas del mismo color el contador queda en 0 y continúa el juego.
    /// </summary>

    public bool HorizontalConditions(int x, int y, Color colorToCompare)
    {
        var counter = 0;
        for (var i = x - 3; i <= x + 3; i++)
        {
            if (i < 0 || i >= width) continue;

            if (grid[i, y].GetComponent<Renderer>().material.color == colorToCompare)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
            if (counter < 4) continue;
            {
                winner = true;
                return winner;
            }
        }
        return false;
    }

    private bool VerticalConditions(int x, int y, Color colorToCompare)
    {
        var counter = 0;
        for (var j = y - 3; j <= y + 3; j++)
        {
            if (j < 0 || j >= heigth) continue;

            if (grid[x, j].GetComponent<Renderer>().material.color == colorToCompare)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
            if (counter < 4) continue;
            {
                winner = true;
                return winner;
            }
        }
        return false;
    }

    private bool DiagonalConditions(int x, int y, Color colorToCompare)
    {
        var counter = 0;
        var z = y - 4;

        for (var i = x - 3; i <= x + 3; i++)
        {
            z++;

            if (i < 0 || i >= width || z < 0 || z >= heigth) continue;
            if (grid[i, z].GetComponent<Renderer>().material.color == colorToCompare)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
            if (counter >= 4)
            {
                winner = true;
                return winner;
            }
        }
        return false;
    }

    private bool DiagonalConditionsIz(int x, int y, Color colorToCompare)
    {
        var counter = 0;
        var z = y + 4;

        for (var i = x - 3; i <= x + 3; i++)
        {
            z--;

            if (i < 0 || i >= width || z < 0 || z >= heigth) continue;
            if (grid[i, z].GetComponent<Renderer>().material.color == colorToCompare)
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
            if (counter >= 4)
            {
                winner = true;
                return winner;
            }
        }
        return false;
    }
    
    public String Regla() //Se crea una función tipo menú para la asignación de los mensajes que deberan aparecer en pantalla
    {
        int randomName = UnityEngine.Random.Range(1, 4); // Se crea una variable de nombre randomName y con el metodo switch se crea un menú que va desde el 1 al 4
        string text = "nulo1";
        switch (randomName)
        {
            case 1:
                text = "Horizontal";
                horizontalWins = true;
                break;
            case 2:
                text = "Vertical";
                verticalWins = true;
                break;
            case 3:
                text = "Diagonal";
                diagonalWins = true;
                diagonalLeftWins = true;
                break;
        }
        ruleText.text = text; // con rule text se mostrará en pantalla la dirección random a ganar 
        return text; // Se retorna la variable text 
    }
}
    

