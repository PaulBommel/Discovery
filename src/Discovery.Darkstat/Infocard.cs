using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public readonly record struct Infocard
    {
        [JsonPropertyName("phrases")]
        public Phrase[] Phrases { get; init; }
    }

    public readonly record struct Phrase
    {
        [JsonPropertyName("phrase")]
        public string PhrasePhrase { get; init; }

        [JsonPropertyName("link")]
        public Uri Link { get; init; }

        [JsonPropertyName("bold")]
        public bool Bold { get; init; }
    }
}
