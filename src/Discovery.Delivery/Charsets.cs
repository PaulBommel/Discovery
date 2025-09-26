using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{
    public static class Charsets
    {
        public const string UpercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        public const string Numbers = "0123456789";

        public const string Cargo = UpercaseLetters + LowercaseLetters + Numbers + "() ";
        public const string Credits = Numbers;
        public const string Destination = UpercaseLetters + LowercaseLetters + Numbers + "-> ";
        public const string Time = Numbers + "-: ";
        public const string Shiptype = UpercaseLetters + LowercaseLetters + Numbers + "\" ";
    }
}
