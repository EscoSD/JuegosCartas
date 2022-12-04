using System;
using CartasLib;

public class Juego1 {
	static void Main(string[] args) {
		Baraja baraja = new Baraja();

		baraja.Barajar();

		Carta carta;

		while ((carta = baraja.Robar()) != null)
			Console.WriteLine(carta);
	}
}