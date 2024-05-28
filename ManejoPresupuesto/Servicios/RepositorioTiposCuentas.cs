﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
        void CrearSinAsync(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuario);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO dbo.TiposCuentas(Nombre,UsuarioId,Orden)
                                                Values (@Nombre, @UsuarioId,0);
                                                SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM dbo.TiposCuentas
                                                                        WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                                                                        new {nombre, usuarioId});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                            FROM dbo.TiposCuentas
                                                            Where UsuarioId = @UsuarioId", new {usuarioId});
        }




        //EJEMPLO METODO CREAR SIN ASYNC AWAIT (MALA PRACTICA)
        public void CrearSinAsync(TipoCuenta tipoCuenta)
        {
          using var connection = new SqlConnection(connectionString);
          var id = connection.QuerySingle<int>($@"INSERT INTO dbo.TiposCuentas(Nombre,UsuarioId,Orden)
                                                Values (@Nombre, @UsuarioId,0);
                                                SELECT SCOPE_IDENTITY();", tipoCuenta);
          tipoCuenta.Id = id;
        }
    }
}
