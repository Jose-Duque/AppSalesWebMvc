using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exception;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();// Para salvar no Banco de Dados
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {

                var selle = await _context.Seller.FirstOrDefaultAsync(obj => obj.Id == id);
                _context.Remove(selle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntregridyException(e.Message);
            }
        }

        public async Task EditAsync(Seller seller)
        {
            if (!await _context.Seller.AnyAsync(obj => obj.Id == seller.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {

                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
