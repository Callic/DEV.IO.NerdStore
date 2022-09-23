using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Data.Interfaces;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data.Implementations
{
    public class ProdutoRepository : IProdutoRepository
    {

        private readonly CatalogoContext _context;
        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        //EU NÃO DOU O SAVECHANGES AQUI PQ VOU CRIAR UM OUTRO MÉTODO PARA SALVAR (UnitOfWork)
        public void Adicionar(Produto produto)
        {
            _context.Produto.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produto.Update(produto);
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produto.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produto.AsNoTracking().ToListAsync();
        }
        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
