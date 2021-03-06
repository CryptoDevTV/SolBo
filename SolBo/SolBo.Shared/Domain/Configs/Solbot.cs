﻿using System.Text.Json.Serialization;

namespace SolBo.Shared.Domain.Configs
{
    public class Solbot
    {
        public Strategy Strategy { get; set; }
        public Actions Actions { get; set; }
        [JsonIgnore]
        public Communication Communication { get; set; }
    }
}