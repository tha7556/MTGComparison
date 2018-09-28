using System;

namespace MTGComparison {
    public class Program {
        static void Main(string[] args) {
            if(args.Length == 1 && args[0].ToLower().Contains("h")) {
                Console.WriteLine("This program is intended to help find the differences between 2 MTG decklists stored in 2 files (.txt is probably best).\n" +
                "The layout for the file should be identical to how TappedOut or most online MTG card shops format decklists. For example:");
                PrintSuccess("\n5x Island\n1x Sol Ring\n1x Commander Sphere\n");
                Console.WriteLine("To execute the program, just give it the paths to the 2 decks and optionally the path you want to save the output to");
                PrintSuccess("MTGComparison {Original Deck} {Modified Deck} {Optional File to save to}");
                return;
            }
            if(args.Length < 2) {
                PrintError("Missing required fields\nMTGComp {Path to first deck} {Path to second deck} {Optional path to output file}");
                return;
            }
            Deck deck = Deck.getDeckFromFile(args[0]);
            Deck deck2 = Deck.getDeckFromFile(args[1]);
            Deck[] result = Compare(deck, deck2);
            if(args.Length > 2) 
                Deck.writeToFile(args[2], result);
        }
        public static void PrintError(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void PrintWarning(string message, bool writeLine) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if(writeLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.ResetColor();
        }
        public static void PrintWarning(string message) {
            PrintWarning(message, false);
        }
        public static void PrintSuccess(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static Deck[] Compare(Deck deckA, Deck deckB) {
            if(deckA == null || deckB == null)
                return null;
            Deck[] result = Deck.Compare(deckA, deckB);
            if(result == null || result.Length != 2) {
                PrintError("The comparison between the two decks has failed");
                return null;
            }
            PrintSuccess("\nAdd " + result[0].Count + " cards:");
            Console.WriteLine(result[0]);
            PrintSuccess("\nRemove " + result[1].Count + " cards:");
            Console.WriteLine(result[1]);

            return result;
        }
    }
}
