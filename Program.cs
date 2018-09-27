using System;

namespace MTGComparison {
    class Program {
        static void Main(string[] args) {
            if(args.Length < 2) {
                PrintError("Missing required fields\nMTGComp {Path to first deck} {Path to second deck}");
                return;
            }
            Deck deck = Deck.getDeckFromFile("test.txt");
            Deck deck2 = Deck.getDeckFromFile("test2.txt");
            Compare(deck, deck2);
        }
        public static void PrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void PrintWarning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void PrintSuccess(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Compare(Deck deckA, Deck deckB) {
            if(deckA == null || deckB == null)
                return;
            Deck[] result = Deck.Compare(deckA, deckB);
            if(result == null || result.Length != 2) {
                PrintError("The comparison between the two decks has failed");
                return;
            }
            PrintSuccess("\nAdd " + result[0].Count + " cards:");
            Console.WriteLine(result[0]);
            PrintSuccess("\nRemove " + result[1].Count + " cards:");
            Console.WriteLine(result[1]);
        }
    }
}
