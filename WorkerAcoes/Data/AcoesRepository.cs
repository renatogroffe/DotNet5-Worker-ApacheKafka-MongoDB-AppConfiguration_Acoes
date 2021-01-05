using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WorkerAcoes.Models;
using WorkerAcoes.Documents;

namespace WorkerAcoes.Data
{
    public class AcoesRepository
    {
       private readonly IConfiguration _configuration;

        public AcoesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Save(Acao acao)
        {
            var client = new MongoClient(
                _configuration.GetConnectionString("MongoDBConnection"));
            IMongoDatabase db = client.GetDatabase(
                _configuration["MongoDatabase"]);

            var historico = db.GetCollection<AcaoDocument>(
                _configuration["MongoCollection"]);

            var horario = DateTime.Now;
            var document = new AcaoDocument();
            document.HistLancamento = acao.Codigo + horario.ToString("yyyyMMddHHmmss");
            document.Codigo = acao.Codigo;
            document.Valor = acao.Valor;
            document.DataReferencia = horario.ToString("yyyy-MM-dd HH:mm:ss");
            document.CodCorretora = acao.CodCorretora;
            document.NomeCorretora = acao.NomeCorretora;

            historico.InsertOne(document);
        }
    }
}