﻿using System;
using CartasLib;

public class Juego3 {

    private static readonly int N;  // Cartas por jugador
    
    static void Main(string[] args) {

        new Juego3();

        Random random = new Random();
        int turnoRandom = random.Next(0, 2);

        bool turno = Convert.ToBoolean(turnoRandom);

        Console.WriteLine(turno ? "Juego Raro.- Empieza el jugador." : "Juego Raro.- Empieza el oponente.");
        Console.Write("Pulsa enter para comenzar.");
        
        Juego(turno);
        
    }

    static Juego3() {
        
        N = AsignaValor();
    }

    private static int AsignaValor() {

        bool valido;
        int valor;

        Console.Write("Introduce el número de cartas por jugador[10].- ");

        do {
            String valorText = Console.ReadLine();

            if (String.IsNullOrEmpty(valorText)) {
                valor = 10;
                valido = true;
            }
            else
                valido = Int32.TryParse(valorText, out valor);

            if (!valido)
                Console.Write("Introduce un número entero.- ");
            else if (valor > 26 || valor < 1) {
                Console.Write("El valor debe estar comprendido entre 1 y 26.- ");
                valido = false;
            }

        } while (!valido);
        
        Console.Clear();

        return valor;
    }

    static void Juego(bool jugadorPrimero) {
        
        Carta[] manoJugador = new Carta[N];
        Carta[] manoOponente = new Carta[N];

        int puntosJugador = 0;
        int puntosOponente = 0;
        
        Baraja baraja = new Baraja();
        baraja.Barajar();

        for (int i = 0; i < N; i++) {
            manoJugador[i] = baraja.Robar();
            manoOponente[i] = baraja.Robar();
        }

        for (int i = 0; i < N; i++) {

            Carta jugador = manoJugador[i];
            Carta oponente = manoOponente[i];

            Console.ReadKey();
            Console.Clear();

            Console.WriteLine(("Turno {0} de {1}\n".PadLeft(24)), i+1, N);
            
            Console.WriteLine("Jugador" + "Oponente".PadLeft(20));
            Baraja.DibujaCartas(new[] { jugador, oponente }, 0);

            if (jugador.Palo == oponente.Palo) {
                if (jugador.Valor > oponente.Valor) {
                    puntosJugador++;
                    jugadorPrimero = false;
                }
                
                else {
                    puntosOponente++;
                    jugadorPrimero = true;
                }
            }

            else {
                if (jugadorPrimero) {
                    puntosJugador++;
                    jugadorPrimero = false;
                }
                else {
                    puntosOponente++;
                    jugadorPrimero = true;
                }
            }
            
            Console.WriteLine((puntosJugador + " - " + puntosOponente).PadLeft(16));
        }

        Console.WriteLine();

        if (puntosJugador == puntosOponente)
            Console.WriteLine("Empate");
        else if (puntosJugador > puntosOponente) 
            Console.WriteLine("Gana el jugador");
        else
            Console.WriteLine("Gana el oponente");
        
    }
}
