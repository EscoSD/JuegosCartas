using System;
using CartasLib;

public class Juego1 {

    static void Main(string[] args) {
        Baraja baraja = new Baraja();
        
        baraja.Barajar();

        Carta carta;

        while ((carta = baraja.Robar()) != null) 
            Console.WriteLine(carta);
        
        /*Carta[] array = new Carta[52];

        while (baraja.CartasRestantes > 0) {

            for (int i = 0; i < array.Length; i++)
                array[i] = baraja.Robar();
            
            Baraja.DibujaCartas(array, 0);
        }*/
    }
}
