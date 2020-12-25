using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Registrar_Funcionario.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string PIS { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [RegularExpression(@"[0-9]{1,9}(,[0-9]{1,2})?$", ErrorMessage = "Exemplo de Salário: 1999,99")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Exemplo de Email: email@email.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data_admissao { get; set; }
    }
}