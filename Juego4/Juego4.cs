using System;
using System.ComponentModel;
using CartasLib;

// Blackjack lko

public class Juego4 {

	private static bool juego = true;

	static void Main(string[] args) {

		Console.WriteLine("Juego de Blackjack.-\nPulsa cualquier tecla para comenzar.");
		Console.ReadKey();
		
		Juego();
	}

	static void Juego() {
		Baraja baraja = new Baraja();
		baraja.Barajar();

		Jugador crupier = new Jugador(baraja.Robar());
		Jugador jugador = new Jugador();
		
		Jugada(ref baraja, ref jugador, 21);

		while (juego) {
			muestraTablero(crupier, jugador);
			
			if (jugador.Puntos > 21)
				Fin(1);

			if (jugador.Puntos == 21)
				Plantarse(ref baraja, ref crupier, jugador);

			else {
				bool key = false;
				while (!key) {
					Console.WriteLine("\nPulsa Enter para robar una carta.\nPulsa Esc para plantarte.");

					if (Console.ReadKey().Key == ConsoleKey.Enter) {
						Jugada(ref baraja, ref jugador, 21);
						key = true;
					}
					else if (Console.ReadKey().Key == ConsoleKey.Escape) {
						Plantarse(ref baraja, ref crupier, jugador);
						key = true;
					}
				}
			}
		}
	}

	static void muestraTablero(Jugador crupier, Jugador jugador) {
		
		Console.Clear();
		
		string texto = "Crupier.-";
		Console.WriteLine("{0," + ((Console.WindowWidth / 2) + (texto.Length / 2)) + "}", texto);
		Baraja.DibujaCartas(crupier.Mano, 1);
		Console.WriteLine("\n\n\n\n");

		Console.WriteLine("Jugador.- {0}", jugador.Puntos);
		Baraja.DibujaCartas(jugador.Mano, 0);

	}

	static void Jugada(ref Baraja baraja, ref Jugador jugador, int puntosMax) {
		jugador.addCarta(baraja.Robar());

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
					jugador.Puntos += (int) c.Valor;
					break;
			}
		}
		
		if (jugador.Puntos > puntosMax) 
			for (int i = 0; i < jugador.Mano.Length && jugador.Puntos > puntosMax; i++) 
				if (jugador.Mano[i].Valor == eValor.A)
					jugador.Puntos -= 10;
		
	}

	static void Plantarse(ref Baraja baraja, ref Jugador crupier, Jugador jugador) {

		while (crupier.Puntos < 17) {
			Jugada(ref baraja, ref crupier, 17);
			muestraTablero(crupier, jugador);
		}
		
		Fin(2);
		
	}

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
		}
		
		juego = false;
	}
}

class Jugador {
	public Carta[] Mano { get; set; }
	public int Puntos { get; set; }

	public Jugador(Carta inicial) {
		Mano = new Carta[1];
		Mano[0] = inicial;

		Puntos = 0;
	}

	public Jugador() {
		Mano = new Carta[0];

		Puntos = 0;
	}

	public void addCarta(Carta carta) {

		Carta[] aux = Mano;
		Array.Resize(ref aux, Mano.Length+1);
		Mano = aux;
		Mano[^1] = carta;

	}
}
