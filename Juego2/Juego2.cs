using System;
using CartasLib;

public class Juego2 {
	// N = número de cartas sacadas de la baraja. 5 por defecto
	private static readonly int N;

	// M = número de cartas del mismo palo. 3 por defecto
	private static readonly int M;

	static void Main(string[] args) {
		new Juego2();
		Videojogo();
	}

	// Constructor que se encarga de inicializar los valores de solo lectura
	static Juego2() {
		int[] valores = AsignaValores();
		N = valores[0];
		M = valores[1];
	}

	// Método que obtiene los valores del usuario
	private static int[] AsignaValores() {
		bool salida = true;

		int[] valores = new int[2];

		int cont = 0;
		// Bucle que inicializa tanto el valor N como el valor M
		do {
			// Si el contador es 0, se imprime el primer String
			Console.Write(cont == 0
				? "Número de cartas a robar por jugada[5]: "
				: "\nNúmero de palos coincidentes para ganar[3]: ");

			String input = Console.ReadLine();

			if (!String.IsNullOrEmpty(input)) {
				if (Int32.TryParse(input, out int valor))
					ComprobarInput(ref cont, ref valores, ref salida, valor);

				else {
					salida = false;
					Console.Write("Por favor inserta un número válido: \n");
				}
			}

			// En caso de una entrada vacía se asignan los valores por defecto
			else {
				salida = true;
				if (cont == 0)
					valores[cont] = 5;
				else
					valores[cont] = 3;
				cont++;
			}
		} while (!salida || cont != 2);

		return valores;
	}

	// Método que comprueba si los datos introducidos por el usuario son válidos
	private static void ComprobarInput(ref int cont, ref int[] valores, ref bool salida, int valor) {
		switch (cont) {
			case 0:
				if (valor > 2 && valor <= 52) {
					salida = true;
					valores[cont] = valor;
					cont++;
				}
				else {
					salida = false;
					Console.WriteLine("El valor debe estar comprendido entre 3 y 52.\n");
				}

				break;

			case 1:
				if (valor > 1 && (valor <= valores[cont - 1] && valor <= 13)) {
					salida = true;
					valores[cont] = valor;
					cont++;
				}
				else {
					salida = false;
					Console.WriteLine("El valor ha de ser mayor que 1 pero no mayor a 13 o al primer valor.\n");
				}

				break;
		}
	}

	// Método principal del juego
	static void Videojogo() {
		Baraja baraja = new Baraja();
		baraja.Barajar();

		Carta[] mano = new Carta[N];
		bool victoria = false;

		int jugadasPosibles = baraja.CartasRestantes / N;

		// Bucle que cuenta las jugadas realizadas por el jugador,
		// se repite mientras queden cartas o hasta que se gane.
		for (int jugada = 1; baraja.CartasRestantes >= N && !victoria; jugada++) {
			int tre = 0, pic = 0, dia = 0, cor = 0;

			Console.Write("Jugada.- {0}/{1}\nPulse Enter.", jugada, jugadasPosibles);
			Console.ReadKey();
			Console.WriteLine();

			for (int i = 0; i < mano.Length; i++) {
				mano[i] = baraja.Robar();

				CheckPalo(mano[i], ref tre, ref pic, ref dia, ref cor);
			}

			if (tre >= M || pic >= M || dia >= M || cor >= M)
				victoria = true;

			Baraja.DibujaCartas(mano, 0);

			Console.WriteLine("\nCartas Restantes.- {0}.\n", baraja.CartasRestantes);
		}
		
		// En caso de ganar, se printea el primer String
		Console.WriteLine(victoria ? "Has Ganado :D" : "Que pena eh");
	}

	// Método que comprueba el palo de la carta e incrementa el valor correspondiente
	private static void CheckPalo(Carta carta, ref int tre, ref int pic, ref int dia, ref int cor) {
		switch (carta.Palo) {
			case ePalo.Treboles:
				tre++;
				break;
			case ePalo.Picas:
				pic++;
				break;
			case ePalo.Diamantes:
				dia++;
				break;
			case ePalo.Corazones:
				cor++;
				break;
		}
	}
}