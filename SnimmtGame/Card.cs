namespace SnimmtGame
{
    public class Card
    {
        public int BullValue =>  Util.BullValue(Number);

        public int Number =>  number;

        private int number;

        public Card(int number)
        {
            this.number = number;
        }

        public override string ToString() => $"[{number}]";

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Card);
        }

        public bool Equals(Card other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Number == this.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }


    }

}