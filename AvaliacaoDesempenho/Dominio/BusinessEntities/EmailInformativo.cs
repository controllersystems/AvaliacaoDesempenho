using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
//using System.Web.Mail;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace AvaliacaoDesempenho.Dominio.BusinessEntities
{
    public class EmailInformativo
    {
        [Display(Name="Assunto")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string Assunto { get; set; }

        [Display(Name = "Cabeçalho HTML")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [AllowHtml]
        public string CabecalhoHTML { get; set; }

        [Display(Name = "Mensagem a ser enviada")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(1000, ErrorMessage = "A {0} deve ter o tamanho máximo de 1000 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [AllowHtml]
        public string Mensagem { get; set; }

        [Display(Name = "Rodapé HTML")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [AllowHtml]
        public string RodapeHTML { get; set; }

        public List<string> ListaDeEmails { get; set; }

        public EmailInformativo()
        {
            ListaDeEmails = new List<string>();
        }

        public void Send()
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"].ToString());


            foreach (var item in this.ListaDeEmails)
            {
                msg.Bcc.Add(item);
            }

            msg.Subject = this.Assunto;
            msg.Body = this.CabecalhoHTML + "<br />" + this.Mensagem + "<br />" + this.RodapeHTML;
            msg.Priority = MailPriority.High;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();

            client.Host = ConfigurationManager.AppSettings["smtpServer"].ToString();
            client.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"].ToString());
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"].ToString());
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpUser"].ToString(), ConfigurationManager.AppSettings["smtpPass"].ToString());
            //client.Timeout = 20000;

            client.Send(msg);
        }
    }
}