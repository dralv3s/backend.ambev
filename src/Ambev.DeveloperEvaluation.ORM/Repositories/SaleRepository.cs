using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Set<Sale>().AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Sale>()
                             .Include(s => s.Items)
                             .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Sale>()
                             .Include(s => s.Items)
                             .ToListAsync(cancellationToken);
    }

    //public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    //{
    //    _context.Set<Sale>().Update(sale);
    //    await _context.SaveChangesAsync(cancellationToken);
    //    return sale;
    //}

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingSale = await _context.Sales.FindAsync(sale.Id);
        if (existingSale != null)
        {
            _context.Entry(existingSale).CurrentValues.SetValues(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return existingSale;
        }
        return new Sale();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
        {
            return false;
        }

        _context.Set<Sale>().Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
