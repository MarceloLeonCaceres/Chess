using ConsoleApp;
using ConsoleApp.Auxiliares;
using ConsoleApp.Pieces;

//Ayudante.PrintTablero();

var a1 = new Casilla('a', 1);

//var d4 = new Casilla('d', 4);
//var torreB = new Rook(Color.White);

//Ayudante.PrintPosibles(d4, torreB);

var c8 = new Casilla('c', 8);
var e4 = new Casilla('e', 4);
//var alfilB = new Bishop(Color.White);

//Ayudante.PrintPosibles(c8, alfilB);

//var c5 = new Casilla('c', 5);
//var damaB = new Queen(Color.White);

//Ayudante.PrintPosibles(c5, damaB);

var caballoB = new Knight(Color.White);
var reyB = new King(Color.White);

Ayudante.PrintPosibles(e4, reyB);
Ayudante.PrintPosibles(a1, reyB);
//Ayudante.PrintPosibles(c8, caballoB);
//Ayudante.PrintPosibles(c8, caballoB);

Console.WriteLine("Fin");