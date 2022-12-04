using System;
using System.Threading;
using CartasLib;

public class Juego4 {
	static void Main(string[] args) {
		Console.WriteLine("Blackjack.-");

		bool apuestas;
		Console.WriteLine("¿Quieres activas las apuestas? S/[N]");
		String text = Console.ReadLine().ToUpper();
		apuestas = (text == "S");

		int fichas = 1;
		// Bucle que se asegura de que se introduzca un número válido de fichas.
		if (apuestas)
			do {
				Console.Write("Número de fichas a comprar.- ");
			} while (!Int32.TryParse(Console.ReadLine(), out fichas) || fichas < 1);

		Console.WriteLine("Pulsa cualquier tecla para comenzar.");
		Console.ReadKey();

		bool seguirJugando;

		do {
			if (apuestas)
				JuegoConApuestas(ref fichas);

			else
				Juego();

			if (fichas > 0) {
				Console.WriteLine("¿Volver a jugar? S/[N]");
				seguirJugando = (Console.ReadLine().ToUpper() == "S");
			}
			else {
				seguirJugando = false;
				Console.WriteLine("\nTe has quedado sin fichas.\nVete a ganar dinero para perderlo otra vez :D");
			}
		} while (seguirJugando);
	}

	// Método que gestiona el juego sin las apuestas habilitadas
	static void Juego() {
		Baraja baraja = new Baraja();
		baraja.Barajar();

		Jugador crupier = new Jugador();
		Jugador jugador = new Jugador();

		// El jugador comienza con dos cartas
		Jugada(ref baraja, ref jugador);
		Jugada(ref baraja, ref jugador);
		
		// El crupier saca una carta.
		Jugada(ref baraja, ref crupier);

		int resultadoPartida = -1;
		bool juego = true;

		while (juego) {
			Console.Clear();
			MuestraTablero(crupier, jugador);

			resultadoPartida = Comprobaciones(ref baraja, ref crupier, ref jugador, ref juego);
		}

		Fin(resultadoPartida);
	}

	// Método que gestiona el juego con las apuestas habilitadas
	static void JuegoConApuestas(ref int fichas) {
		Baraja baraja = new Baraja();
		baraja.Barajar();

		Jugador crupier = new Jugador();
		Jugador jugador = new Jugador();

		// El jugador comienza con dos cartas
		Jugada(ref baraja, ref jugador);
		Jugada(ref baraja, ref jugador);
		
		// El crupier saca una carta.
		Jugada(ref baraja, ref crupier);

		int resultadoPartida = -1;
		bool juego = true;

		Console.Clear();

		int apuesta;
		// Bucle que se asegura que la cantidad a apostar sea válida
		// Ha de ser un número natural mayor que 0 y menor que el número de fichas
		do {
			Console.Write("Fichas.- {0}\nApuesta.- ", fichas);
		} while (!Int32.TryParse(Console.ReadLine(), out apuesta) || (apuesta < 1 || apuesta > fichas));

		fichas -= apuesta;

		while (juego) {
			Console.Clear();
			Console.WriteLine("Fichas.- {0}", fichas);
			MuestraTablero(crupier, jugador);

			resultadoPartida = Comprobaciones(ref baraja, ref crupier , ref jugador, ref juego);
		}

		Fin(resultadoPartida);

		switch (resultadoPartida) {
			case 0:
				fichas += apuesta * 2;
				break;
			case 2:
				fichas += apuesta;
				break;
			case 3:
				fichas += (int)(apuesta * 2.5);
				break;
		}

		Console.WriteLine("Fichas.- {0}", fichas);
	}

	// Método que comprueba los puntos del jugador y determina si puede seguir sacando cartas
	// Devuelve el resultado de la partida
	private static int Comprobaciones(ref Baraja baraja, ref Jugador crupier, ref Jugador jugador, ref bool juego) {

		int resultadoPartida = -1;
		
		// El jugador se pasa de 21 y pierde
		if (jugador.Puntos > 21) {
			resultadoPartida = 1;
			juego = false;
		}
			
		else if (jugador.Puntos == 21) {
			// El jugador hace un blackjack, gana automáticamente
			if (jugador.Mano.Length == 2) {
				resultadoPartida = 3;
				juego = false;
			}
				
			// Hay posibilidad de empate
			else {
				resultadoPartida = Plantarse(ref baraja, ref crupier, jugador);
				juego = false;
			}
		}

		// En caso de tener menos puntuación de 21, el jugador puede elegir entre sacar carta o plantarse
		else {
			bool teclaValida = false;
			while (!teclaValida) {
				Console.WriteLine("\nPulsa Enter para robar una carta.\nPulsa Escape para plantarte.");
				ConsoleKey key = Console.ReadKey().Key;

				switch (key) {
					case ConsoleKey.Enter:
						Jugada(ref baraja, ref jugador);
						teclaValida = true;
						break;
					case ConsoleKey.Escape:
						Console.WriteLine("\n");
						resultadoPartida = Plantarse(ref baraja, ref crupier, jugador);
						teclaValida = true;
						juego = false;
						break;
				}
			}
		}

		return resultadoPartida;
	}

	// Método que muestra el tablero
	static void MuestraTablero(Jugador crupier, Jugador jugador) {
		// Se escribe el texto crupier en el centro del terminal
		string texto = $"Crupier.- {crupier.Puntos}";
		Console.WriteLine("{0," + ((Console.WindowWidth / 2) + (texto.Length / 2)) + "}", texto);

		// Se escribe la baraja del crupier en el centro del terminal
		Baraja.DibujaCartas(crupier.Mano, 1);
		Console.WriteLine("");

		// Se escribe la baraja del jugador
		Console.WriteLine("Jugador.- {0}", jugador.Puntos);
		Baraja.DibujaCartas(jugador.Mano, 0);
	}

	// Método que añade una carta a la mano y suma los puntos correspondientes
	static void Jugada(ref Baraja baraja, ref Jugador jugador) {
		jugador.AddCarta(baraja.Robar());

		jugador.Puntos = 0;

		foreach (Carta c in jugador.Mano) {
			switch (c.Valor) {
				case eValor.A:
					jugador.Puntos += 11;
					break;
				case eValor.J:
				case eValor.Q:
				case eValor.K:
					jugador.Puntos += 10;
					break;
				default:
					jugador.Puntos += (int)c.Valor;
					break;
			}
		}

		// Si los puntos superan el máximo se comprobará si hay ases
		// en ese caso se restarán los puntos correspondientes
		if (jugador.Puntos > 21)
			for (int i = 0; i < jugador.Mano.Length && jugador.Puntos > 21; i++)
				if (jugador.Mano[i].Valor == eValor.A)
					jugador.Puntos -= 10;
	}

	// Método que gestiona las acciones del crupier una vez el jugador se planta.
	static int Plantarse(ref Baraja baraja, ref Jugador crupier, Jugador jugador) {
		int salida;

		// El crupier saca cartas hasta que iguale o supere los 17 puntos.
		while (crupier.Puntos < 17) {
			Jugada(ref baraja, ref crupier);
			Thread.Sleep(750);

			Console.Clear();
			MuestraTablero(crupier, jugador);
		}

		// Victoria
		if (crupier.Puntos < jugador.Puntos || crupier.Puntos > 21)
			salida = 0;

		// Derrota
		// Tanto si el crupier saca más puntos que el jugador como si el crupier hace blackjack
		else if (crupier.Puntos > jugador.Puntos || (crupier.Mano.Length == 2 && crupier.Puntos == 21))
			salida = 1;

		// Empate
		else
			salida = 2;

		return salida;
	}

	// Método final
	static void Fin(int f) {
		switch (f) {
			case 0:
				Console.WriteLine("Has ganado lol lol");
				break;
			case 1:
				Console.WriteLine("Has perdido lol lol");
				break;
			case 2:
				Console.WriteLine("Empate lol lol");
				break;
			case 3:
				Console.WriteLine("Blackjack lol lol");
				break;
		}
	}
}

// Clase que almacena los datos tanto del jugador como del crupier.
class Jugador {
	public Carta[] Mano { get; private set; }
	public int Puntos { get; set; }

	// Constructor por defecto que incializa los valores.
	public Jugador() {
		Mano = new Carta[0];

		Puntos = 0;
	}

	// Método que añade una carta a la mano del objeto
	public void AddCarta(Carta carta) {
		Carta[] aux = Mano;
		Array.Resize(ref aux, Mano.Length + 1);
		Mano = aux;
		Mano[Mano.Length - 1] = carta;
	}
}