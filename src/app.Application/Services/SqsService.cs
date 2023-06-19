﻿using app.Application.Interfaces;
using app.Domain.Entities;

namespace app.Domain.Services
{
    public class SqsService : ISqsService
    {
        public void PublishMessage(Transacao message, EstruturaComercial estrutura)
        {
            // Lógica para publicar mensagem no Amazon SQS
            Console.WriteLine($"Published message: {message.CustomerId}, {message.Amount}, {message.TransactionType}, {estrutura.NomeEspecialista}, {estrutura.EmailEspecialista}");
        }
    }
}
