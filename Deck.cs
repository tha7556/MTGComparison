using System.Collections.Generic;
using System;
using System.IO;
namespace MTGComparison {
    public class Deck {
        private Dictionary<string, int> cards;
        public int Count {get {return getLength();}}
        public Deck() {
            cards = new Dictionary<string, int>();
        }
        public Deck(Dictionary<string, int> cards) {
            this.cards = cards;
        }
        public int this[string card] {
            get {
                if(cards.ContainsKey(card.ToLower())) 
                    return cards[card.ToLower()];
                return 0;                    
            }
            set {
                if(cards.ContainsKey(card.ToLower()))
                    cards[card.ToLower()] = value;
                else
                    cards.Add(card.ToLower(), value);
            }
        }
        public bool Contains(string card) {
            return cards.ContainsKey(card.ToLower());
        }
        public string[] GetCards() {
            string[] result = new string[cards.Count];
            int i = 0;
            foreach(string card in cards.Keys) {
                result[i] = card;
                i++;
            }
            return result;
        }
        public static Deck GetDeckFromFile(string filename) {
            if(!File.Exists(filename)) {
                Program.PrintError("File: '" + filename + "' not found");
                return null;
            }
            string[] lines = File.ReadAllLines(filename);
            Deck result = new Deck();
            foreach(string line in lines) {
                int index = line.ToLower().IndexOf("x ");
                if(index == -1) {
                    Program.PrintError("Line: '"+line+"'\nImproperly formatted");
                    Program.PrintWarning("Should be in the form:\n{quantity}x {name} {#optional tags}");
                    continue;
                }
                int quantity = int.Parse(line.Substring(0, index).Trim());
                string name = line.Substring(index+2);
                index = name.IndexOf('#');
                if(index != -1) {
                    name = name.Substring(0, index);
                }
                index = name.IndexOf('*');
                if (index != -1) {
                    name = name.Substring(0, index);
                }
                name = name.Trim();
                if(result.Contains(name))
                    result[name] = result[name] + quantity;
                else
                    result[name] = quantity;
            }
            
            return result;
        }
        public static void WriteToFile(string filename, Deck[] decks) {
            if(decks.Length != 2) {
                Program.PrintError("\nSomething went wrong while writing the result to file");
                return;
            }
            if(File.Exists(filename)) {
                Program.PrintWarning("\nThe file exists, are you sure you want to override it? ", false);
                bool overwrite = Console.ReadLine().ToLower().Contains("y");
                if(!overwrite) 
                    return;
            }
            else
                Console.WriteLine();
            string result = "Add " + decks[0].Count + " cards:\n";
            result += decks[0];
            result += "\n\nRemove " + decks[1].Count + " cards:\n";
            result += decks[1];

            File.WriteAllText(filename, result);
            Program.PrintSuccess("Successfully wrote the results to: " + filename);
        }
        public static Deck[] Compare(Deck startDeck, Deck endDeck) {
            Deck add = new Deck(), remove = new Deck();
            foreach(string card in startDeck.GetCards()) {
                if(endDeck[card] < startDeck[card])
                    remove[card] = startDeck[card] - endDeck[card];
                else if(endDeck[card] > startDeck[card])
                    add[card] = endDeck[card] - startDeck[card];
            }
            foreach(string card in endDeck.GetCards()) {
                if (endDeck[card] < startDeck[card])
                    remove[card] = startDeck[card] - endDeck[card];
                else if (endDeck[card] > startDeck[card])
                    add[card] = endDeck[card] - startDeck[card];
            }
            return new Deck[] {add, remove};
        }
        public override string ToString() {
            string result = "";
            foreach(string card in cards.Keys)
                result += cards[card] + "x " + card + "\n";
            if(result.Length > 0)
                return result.TrimEnd();
            return result;
        }
        private int getLength() {
            int result = 0;
            foreach(string card in cards.Keys) {
                result += cards[card];
            }
            return result;
        }
    }
}