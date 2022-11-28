using System;

namespace CartasLib
{
    //Enumeración del conjunto de palos.
    public enum ePalo
    {
        Treboles = 1,
        Picas,
        Diamantes,
        Corazones
    }

    //Enumeración del conjunto de valores.
    public enum eValor
    {
        A = 1,
        Dos,
        Tres,
        Cuatro,
        Cinco,
        Seis,
        Siete,
        Ocho,
        Nueve,
        Diez,
        J,
        Q,
        K
    }

    public class Carta
    {
        //Constructor con parámetros.
        public Carta(eValor valor, ePalo palo)
        {
            Valor = valor;
            Palo = palo;
        }

        //Propiedad con setters y getters.
        public eValor Valor { get; }
        public ePalo Palo { get; }

        //ToString que retorna el contenido del objeto.
        public override string ToString()
        {
            char paloIcono = 'E';
            string valorCarta = Valor.ToString();

            //Se sustituyen los palos de la carta por los iconos.
            switch ((int)Palo)
            {
                case 1:
                    paloIcono = '♣';
                    break;
                case 2:
                    paloIcono = '♠';
                    break;
                case 3:
                    paloIcono = '♦';
                    break;
                case 4:
                    paloIcono = '♥';
                    break;
            }

            //Si el valor no es ninguna figura (A,J,Q,K).
            if ((int)Valor > 1 && (int)Valor <= 10)
                valorCarta = ((int)Valor).ToString();

            return "Carta: " + paloIcono + " " + valorCarta + " " + paloIcono;
        }
    }

    public class Baraja
    {
        //Definición de propiedades.
        public int CartasRestantes {get{return baraja.Length;}}

        //Definición de atributos.
        private const int TAM = 52; //Tamaño de la baraja.
        private const String AnsiRed = "\u001B[31m"; //Color rojo para la terminal.
        private const String AnsiGreen = "\u001B[32m"; //Color verde para la terminal.
        private const String AnsiBlue = "\u001B[34m"; //Color azul para la terminal.
        private const String AnsiReset = "\u001B[0m"; //Color por defecto para la terminal.
        private const String AnsiBlack = "\u001B[30m"; //Color negro.
        private const String AnsiWhiteBack = "\u001B[47m"; //Color blanco de fondo
        public const int LongitudCartaAscii = 13;
        private Carta[] baraja;
        

        public static string[] plantillaCarta = //Plantilla de la carta (Es public para poder acceder a ella)
        {
            AnsiWhiteBack + AnsiBlack + "┌───────────┐" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│{0}          │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│           │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│           │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│     {0}     │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│           │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│           │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│          {0}│" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "└───────────┘" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│{0}         │" + AnsiReset,
            AnsiWhiteBack + AnsiBlack + "│         {0}│" + AnsiReset
        };

        public Baraja()
        {
            baraja = new Carta[TAM];
            GeneraBaraja();
        }

        //Método que carga en el array baraja todas y cada una de las cartas ordenadas.
        private void GeneraBaraja()
        {
            int i = 0;
            foreach (ePalo palo in Enum.GetValues(typeof(ePalo)))
            foreach (eValor valor in Enum.GetValues(typeof(eValor)))
            {
                baraja[i] = new Carta(valor, palo);
                i++;
            }
        }

        //Método que aleatoriza el contenido de la baraja.
        public void Barajar()
        {
            Carta aux;
            Random r = new Random();
            int posicion;
            for (int i = 0; i < baraja.Length; i++)
            {
                posicion = r.Next(0, 51);
                aux = baraja[i];
                baraja[i] = baraja[posicion];
                baraja[posicion] = aux;
            }
        }

        //Método que devuelve una carta robada de la baraja.
        public Carta Robar()
        {
            Carta cartaRobada = null;
            if (baraja.Length != 0)
            {
                int carta = baraja.Length - 1;
                cartaRobada = baraja[carta];
                Array.Resize(ref baraja, baraja.Length - 1);
            }
            return cartaRobada;
        }

        //Método que recorre todas las posiciones de la baraja y muestra cada carta.
        public void MuestraBaraja() // Sigo pensando que debería llamarse ToString :(
        {
            foreach (Carta carta in baraja)
                Console.WriteLine(carta);
        }

        //Método que muestra en ASCII un array de cartas pasadas como parámetro.
        public static void DibujaCartas(Carta[] arrayCartas,int alineacion)
        {
            //Se calculan cuantas cartas caben en la pantalla.
            int cartasPosibles = Console.WindowWidth / (LongitudCartaAscii + 1);
            int filasPosibles = TAM/cartasPosibles;
            Carta[] aux = new Carta[cartasPosibles];

            //TODO Queremos crear arrays del tamaño de cartas posibles para luego llamar a IMprimeCartas e imprimir paquetes de cartas. (Idea: COn una matriz, se cargan los arrays )


        }

        //Método de sobrecarga que muestra una única carta en ascii.
        public static void DibujaCartas(Carta carta, int alineacion)
        {
            DibujaCartas(new Carta[]{carta},alineacion);
        }

        //Método que imprime un array de cartas por pantalla que caben.
        public static void ImprimeCartas(Carta[] arrayCartas, int alineacion)
        {
            for (int fila = 0; fila < 9; fila++)
            {
                //Se define la posición del cursor para alinear las cartas.
                switch (alineacion)
                {
                    case 1: Console.SetCursorPosition((Console.WindowWidth-((LongitudCartaAscii+1)*arrayCartas.Length))/2,Console.CursorTop);
                        break;
                    case 2:  Console.SetCursorPosition(Console.WindowWidth-((LongitudCartaAscii+1)*arrayCartas.Length),Console.CursorTop);
                        break;
                }

                foreach (Carta carta in arrayCartas)
                {
                    String paloIcono = "W";
                    String valorCarta = carta.Valor.ToString();
                    String color = "";

                    //Se sustituyen los palos de la carta por los iconos.
                    switch ((int)carta.Palo)
                    {
                        case 1:
                            paloIcono = "♣";
                            color = AnsiGreen;
                            break;
                        case 2:
                            paloIcono = "♠";
                            color = AnsiBlack;
                            break;
                        case 3:
                            paloIcono = "♦";
                            color = AnsiBlue;
                            break;
                        case 4:
                            paloIcono = "♥";
                            color = AnsiRed;
                            break;
                    }

                    //Si el valor no es ninguna figura (A,J,Q,K) se deja numérico.
                    if ((int)carta.Valor > 1 && (int)carta.Valor <= 10)
                        valorCarta = ((int)carta.Valor).ToString();

                    valorCarta = color + valorCarta + AnsiBlack;
                    paloIcono = color + paloIcono + AnsiBlack;
                                            
                    //Mostramos las filas sustituyendo los palos y los valores.
                    if (fila == 1 || fila == 7) //Si es 1 o 7 se sutituye el valor.
                    {
                        if ((int)carta.Valor != 10) //Si no es 10 se hace normal.
                            Console.Write(plantillaCarta[fila] + " ", valorCarta);
                        else
                            Console.Write(fila == 1 ? plantillaCarta[9] + " " : plantillaCarta[10] + " ",
                                valorCarta); //Línea 85 hola.txt
                    }
                    else if (fila == 4) //Si es 4 se sutituye el palo.
                        Console.Write(plantillaCarta[fila] + " ", paloIcono);
                    else
                        Console.Write(plantillaCarta[fila] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}