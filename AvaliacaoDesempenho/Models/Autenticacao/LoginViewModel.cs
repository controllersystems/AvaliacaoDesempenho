﻿using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Models.Autenticacao
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Senha { get; set; }

        public bool Validado { get; set; }

        public LoginViewModel()
        {
            Validado = true;
        }
    }
}