﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagement.Models.ViewModels
{
    public class LancamentoViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public double Valor { get; set; }

        public DateTime Data { get; set; }

        public bool DespesaReceita { get; set; }

        public string Categoria { get; set; }

        public string Conta { get; set; }
    }
}
