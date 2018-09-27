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
                if(cards.ContainsKey(card))
                    cards[card] = value;
                else
                    cards.Add(card, value);
            }
        }
        public bool Contains(string card) {
            return cards.ContainsKey(card.ToLower());
        }
        public string[] getCards() {
            string[] result = new string[cards.Count];
            int i = 0;
            foreach(string card in cards.Keys) {
                result[i] = card;
                i++;
            }
            return result;
        }
        public static Deck getDeckFromFile(string filename) {
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
                name = name.Trim();
                if(result.Contains(name))
                    result[name] = result[name] + quantity;
                else
                    result[name] = quantity;
            }
            return result;
        }
        public static Deck[] Compare(Deck startDeck, Deck endDeck) {
            Deck add = new Deck(), remove = new Deck();
            foreach(string card in startDeck.getCards()) {
                if(endDeck[card] < startDeck[card])
                    remove[card] = startDeck[card] - endDeck[card];
                else if(endDeck[card] > startDeck[card])
                    add[card] = endDeck[card] - startDeck[card];
            }
            foreach(string card in endDeck.getCards()) {
                if(endDeck[card] < startDeck[card])
                    remove[card] = startDeck[card] - endDeck[card];
                else if(endDeck[card] > startDeck[card])
                    add[card] = endDeck[card] - startDeck[card];
            }
            return new Deck[] {add, remove};
        }
        public override string ToString() {
            string result = "";
            foreach(string card in cards.Keys)
                result += cards[card] + "x " + card + "\n";
            if(result.Length > 0)
                return result.Substring(0, result.Length-1);
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