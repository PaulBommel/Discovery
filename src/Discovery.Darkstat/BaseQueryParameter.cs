using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    public readonly record struct BaseQueryParameter()
    {
        [JsonPropertyName("filter_nicknames")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public required string[] NicknameFilter { get; init; }
        [JsonPropertyName("filter_market_good_category")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Category[] MarketGoodCategoryFilter { get; init; } = [Category.Commodity];
        [JsonPropertyName("filter_to_useful")]
        public bool FilterToUseful { get; init; } = true;
        [JsonPropertyName("include_market_goods")]
        public bool IncludeMarketGoods { get; init; } = true;
    }
}
